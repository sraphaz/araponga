using Araponga.Application.Interfaces;
using Araponga.Modules.Map.Infrastructure.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Araponga.Modules.Map.Infrastructure.Postgres;

/// <summary>
/// DbContext específico do módulo Map, contendo apenas entidades relacionadas a Map:
/// - MapEntity, MapEntityRelation
/// </summary>
public sealed class MapDbContext : DbContext, IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;

    public MapDbContext(DbContextOptions<MapDbContext> options)
        : base(options)
    {
    }

    // Entidades de Map
    public DbSet<MapEntityRecord> MapEntities => Set<MapEntityRecord>();
    public DbSet<MapEntityRelationRecord> MapEntityRelations => Set<MapEntityRelationRecord>();

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
        // MapEntity
        modelBuilder.Entity<MapEntityRecord>(entity =>
        {
            entity.ToTable("map_entities");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedByUserId).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Category).HasMaxLength(120).IsRequired();
            entity.Property(e => e.Latitude).HasColumnType("double precision");
            entity.Property(e => e.Longitude).HasColumnType("double precision");
            entity.Property(e => e.Status).HasConversion<int>();
            entity.Property(e => e.Visibility).HasConversion<int>();
            entity.Property(e => e.ConfirmationCount).IsRequired();
            entity.Property(e => e.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(e => e.RowVersion).IsRowVersion();
            entity.HasIndex(e => e.TerritoryId);
            entity.HasIndex(e => new { e.TerritoryId, e.Status });
            entity.HasIndex(e => new { e.TerritoryId, e.Visibility });
            entity.HasIndex(e => e.CreatedByUserId);
        });

        // MapEntityRelation
        modelBuilder.Entity<MapEntityRelationRecord>(entity =>
        {
            entity.ToTable("map_entity_relations");
            entity.HasKey(r => new { r.UserId, r.EntityId });
            entity.Property(r => r.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(r => r.EntityId);
        });

        base.OnModelCreating(modelBuilder);
    }
}
