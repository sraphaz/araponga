using Araponga.Application.Interfaces;
using Araponga.Modules.Moderation.Infrastructure.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Araponga.Modules.Moderation.Infrastructure.Postgres;

/// <summary>
/// DbContext específico do módulo Moderation, contendo apenas entidades relacionadas a Moderation:
/// - ModerationReport, UserBlock, Sanction
/// </summary>
public sealed class ModerationDbContext : DbContext, IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;

    public ModerationDbContext(DbContextOptions<ModerationDbContext> options)
        : base(options)
    {
    }

    // Entidades de Moderation
    public DbSet<ModerationReportRecord> ModerationReports => Set<ModerationReportRecord>();
    public DbSet<UserBlockRecord> UserBlocks => Set<UserBlockRecord>();
    public DbSet<SanctionRecord> Sanctions => Set<SanctionRecord>();

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
        // ModerationReport
        modelBuilder.Entity<ModerationReportRecord>(entity =>
        {
            entity.ToTable("moderation_reports");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.TerritoryId).IsRequired();
            entity.Property(r => r.TargetType).HasConversion<int>();
            entity.Property(r => r.Reason).HasMaxLength(300).IsRequired();
            entity.Property(r => r.Details).HasMaxLength(2000);
            entity.Property(r => r.Status).HasConversion<int>();
            entity.Property(r => r.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(r => r.ReporterUserId);
            entity.HasIndex(r => new { r.TargetType, r.TargetId });
            entity.HasIndex(r => new { r.TargetType, r.TargetId, r.CreatedAtUtc });
            entity.HasIndex(r => new { r.TerritoryId, r.CreatedAtUtc });
            entity.HasIndex(r => r.CreatedAtUtc);
        });

        // UserBlock
        modelBuilder.Entity<UserBlockRecord>(entity =>
        {
            entity.ToTable("user_blocks");
            entity.HasKey(b => new { b.BlockerUserId, b.BlockedUserId });
            entity.Property(b => b.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(b => b.BlockerUserId);
        });

        // Sanction
        modelBuilder.Entity<SanctionRecord>(entity =>
        {
            entity.ToTable("sanctions");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Scope).HasConversion<int>();
            entity.Property(s => s.TargetType).HasConversion<int>();
            entity.Property(s => s.Type).HasConversion<int>();
            entity.Property(s => s.Status).HasConversion<int>();
            entity.Property(s => s.Reason).HasMaxLength(400).IsRequired();
            entity.Property(s => s.StartAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(s => s.EndAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(s => s.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(s => s.TargetId);
            entity.HasIndex(s => s.TerritoryId);
        });

        base.OnModelCreating(modelBuilder);
    }
}
