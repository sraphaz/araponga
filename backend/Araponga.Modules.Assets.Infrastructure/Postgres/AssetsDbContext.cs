using Araponga.Application.Interfaces;
using Araponga.Modules.Assets.Infrastructure.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Araponga.Modules.Assets.Infrastructure.Postgres;

/// <summary>
/// DbContext específico do módulo Assets, contendo apenas entidades relacionadas a Assets e Media:
/// - TerritoryAsset, AssetGeoAnchor, AssetValidation, PostAsset, MediaAsset, MediaAttachment
/// </summary>
public sealed class AssetsDbContext : DbContext, IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;

    public AssetsDbContext(DbContextOptions<AssetsDbContext> options)
        : base(options)
    {
    }

    // Entidades de Assets
    public DbSet<TerritoryAssetRecord> TerritoryAssets => Set<TerritoryAssetRecord>();
    public DbSet<AssetGeoAnchorRecord> AssetGeoAnchors => Set<AssetGeoAnchorRecord>();
    public DbSet<AssetValidationRecord> AssetValidations => Set<AssetValidationRecord>();
    public DbSet<PostAssetRecord> PostAssets => Set<PostAssetRecord>();

    // Entidades de Media
    public DbSet<MediaAssetRecord> MediaAssets => Set<MediaAssetRecord>();
    public DbSet<MediaAttachmentRecord> MediaAttachments => Set<MediaAttachmentRecord>();

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
        // TerritoryAsset
        modelBuilder.Entity<TerritoryAssetRecord>(entity =>
        {
            entity.ToTable("territory_assets");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Type).HasMaxLength(120).IsRequired();
            entity.Property(a => a.Name).HasMaxLength(200).IsRequired();
            entity.Property(a => a.Description).HasMaxLength(1000);
            entity.Property(a => a.Status).HasConversion<int>();
            entity.Property(a => a.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(a => a.UpdatedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(a => a.ArchivedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(a => a.ArchiveReason).HasMaxLength(500);
            entity.HasIndex(a => a.TerritoryId);
            entity.HasIndex(a => new { a.TerritoryId, a.Status });
            entity.HasIndex(a => new { a.TerritoryId, a.Type });
        });

        // AssetGeoAnchor
        modelBuilder.Entity<AssetGeoAnchorRecord>(entity =>
        {
            entity.ToTable("asset_geo_anchors");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Latitude).HasColumnType("double precision");
            entity.Property(a => a.Longitude).HasColumnType("double precision");
            entity.Property(a => a.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(a => a.AssetId);
        });

        // AssetValidation
        modelBuilder.Entity<AssetValidationRecord>(entity =>
        {
            entity.ToTable("asset_validations");
            entity.HasKey(a => new { a.AssetId, a.UserId });
            entity.Property(a => a.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(a => a.UserId);
        });

        // PostAsset
        modelBuilder.Entity<PostAssetRecord>(entity =>
        {
            entity.ToTable("post_assets");
            entity.HasKey(p => new { p.PostId, p.AssetId });
            entity.HasIndex(p => p.AssetId);
        });

        // MediaAsset
        modelBuilder.Entity<MediaAssetRecord>(entity =>
        {
            entity.ToTable("media_assets");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.MediaType).HasConversion<int>().IsRequired();
            entity.Property(m => m.MimeType).HasMaxLength(100).IsRequired();
            entity.Property(m => m.StorageKey).HasMaxLength(500).IsRequired();
            entity.Property(m => m.SizeBytes).IsRequired();
            entity.Property(m => m.Checksum).HasMaxLength(64).IsRequired();
            entity.Property(m => m.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(m => m.DeletedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(m => m.UploadedByUserId);
            entity.HasIndex(m => m.CreatedAtUtc);
            entity.HasIndex(m => m.DeletedAtUtc);
            entity.HasIndex(m => new { m.UploadedByUserId, m.DeletedAtUtc });
        });

        // MediaAttachment
        modelBuilder.Entity<MediaAttachmentRecord>(entity =>
        {
            entity.ToTable("media_attachments");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.OwnerType).HasConversion<int>().IsRequired();
            entity.Property(a => a.DisplayOrder).IsRequired();
            entity.Property(a => a.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(a => new { a.OwnerType, a.OwnerId });
            entity.HasIndex(a => a.MediaAssetId);
            entity.HasIndex(a => new { a.OwnerType, a.OwnerId, a.DisplayOrder });
        });

        base.OnModelCreating(modelBuilder);
    }
}
