using Araponga.Application.Interfaces;
using Araponga.Modules.Feed.Infrastructure.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Araponga.Modules.Feed.Infrastructure.Postgres;

/// <summary>
/// DbContext específico do módulo Feed, contendo apenas entidades relacionadas a Feed:
/// - CommunityPost, PostComment, PostLike, PostShare, PostAsset, PostGeoAnchor
/// </summary>
public sealed class FeedDbContext : DbContext, IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;

    public FeedDbContext(DbContextOptions<FeedDbContext> options)
        : base(options)
    {
    }

    // Entidades de Feed
    public DbSet<CommunityPostRecord> CommunityPosts => Set<CommunityPostRecord>();
    public DbSet<PostCommentRecord> PostComments => Set<PostCommentRecord>();
    public DbSet<PostLikeRecord> PostLikes => Set<PostLikeRecord>();
    public DbSet<PostShareRecord> PostShares => Set<PostShareRecord>();
    public DbSet<PostAssetRecord> PostAssets => Set<PostAssetRecord>();
    public DbSet<PostGeoAnchorRecord> PostGeoAnchors => Set<PostGeoAnchorRecord>();

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InvalidOperationException(
                "Concurrency conflict detected. The entity was modified by another process. Please retry the operation.",
                ex);
        }

        if (_currentTransaction is not null)
        {
            await _currentTransaction.CommitAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction is not null)
        {
            throw new InvalidOperationException("A transaction is already active.");
        }

        _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction is null)
        {
            return;
        }

        try
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public Task<bool> HasActiveTransactionAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_currentTransaction is not null);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // CommunityPost
        modelBuilder.Entity<CommunityPostRecord>(entity =>
        {
            entity.ToTable("community_posts");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.AuthorUserId).IsRequired();
            entity.Property(p => p.Title).HasMaxLength(200).IsRequired();
            entity.Property(p => p.Content).HasMaxLength(4000).IsRequired();
            entity.Property(p => p.Type).HasConversion<int>();
            entity.Property(p => p.Visibility).HasConversion<int>();
            entity.Property(p => p.Status).HasConversion<int>();
            entity.Property(p => p.ReferenceType).HasMaxLength(40);
            entity.Property(p => p.TagsJson).HasColumnType("jsonb"); // JSONB para busca eficiente
            entity.Property(p => p.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(p => p.RowVersion).IsRowVersion();
            entity.HasIndex(p => p.TerritoryId);
            entity.HasIndex(p => new { p.TerritoryId, p.CreatedAtUtc });
            entity.HasIndex(p => new { p.TerritoryId, p.Status, p.CreatedAtUtc });
            entity.HasIndex(p => p.AuthorUserId);
            entity.HasIndex(p => p.MapEntityId);
            entity.HasIndex(p => new { p.ReferenceType, p.ReferenceId });
            // Índice GIN para busca eficiente em tags JSONB
            entity.HasIndex(p => p.TagsJson)
                .HasMethod("gin")
                .HasFilter("\"TagsJson\" IS NOT NULL");
        });

        // PostComment
        modelBuilder.Entity<PostCommentRecord>(entity =>
        {
            entity.ToTable("post_comments");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Content).HasMaxLength(2000).IsRequired();
            entity.Property(c => c.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(c => c.PostId);
        });

        // PostLike
        modelBuilder.Entity<PostLikeRecord>(entity =>
        {
            entity.ToTable("post_likes");
            entity.HasKey(l => new { l.PostId, l.ActorId });
            entity.Property(l => l.ActorId).HasMaxLength(200).IsRequired();
            entity.Property(l => l.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(l => l.PostId);
            entity.HasIndex(l => l.ActorId);
        });

        // PostShare
        modelBuilder.Entity<PostShareRecord>(entity =>
        {
            entity.ToTable("post_shares");
            entity.HasKey(s => new { s.PostId, s.UserId });
            entity.Property(s => s.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(s => s.PostId);
            entity.HasIndex(s => s.UserId);
        });

        // PostAsset
        modelBuilder.Entity<PostAssetRecord>(entity =>
        {
            entity.ToTable("post_assets");
            entity.HasKey(a => new { a.PostId, a.AssetId });
            entity.HasIndex(a => a.PostId);
            entity.HasIndex(a => a.AssetId);
        });

        // PostGeoAnchor
        modelBuilder.Entity<PostGeoAnchorRecord>(entity =>
        {
            entity.ToTable("post_geo_anchors");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Type).HasMaxLength(120).IsRequired();
            entity.Property(a => a.Latitude).HasColumnType("double precision");
            entity.Property(a => a.Longitude).HasColumnType("double precision");
            entity.Property(a => a.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(a => a.PostId);
        });

        base.OnModelCreating(modelBuilder);
    }
}
