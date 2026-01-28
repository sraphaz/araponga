using Araponga.Application.Interfaces;
using Araponga.Modules.Events.Infrastructure.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Araponga.Modules.Events.Infrastructure.Postgres;

/// <summary>
/// DbContext específico do módulo Events, contendo apenas entidades relacionadas a Events:
/// - TerritoryEvent, EventParticipation
/// </summary>
public sealed class EventsDbContext : DbContext, IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;

    public EventsDbContext(DbContextOptions<EventsDbContext> options)
        : base(options)
    {
    }

    // Entidades de Events
    public DbSet<TerritoryEventRecord> TerritoryEvents => Set<TerritoryEventRecord>();
    public DbSet<EventParticipationRecord> EventParticipations => Set<EventParticipationRecord>();

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
        // TerritoryEvent
        modelBuilder.Entity<TerritoryEventRecord>(entity =>
        {
            entity.ToTable("territory_events");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.StartsAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(e => e.EndsAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(e => e.Latitude).HasColumnType("double precision");
            entity.Property(e => e.Longitude).HasColumnType("double precision");
            entity.Property(e => e.LocationLabel).HasMaxLength(200);
            entity.Property(e => e.CreatedByMembership).HasConversion<string>().HasMaxLength(32);
            entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(32);
            entity.Property(e => e.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(e => e.UpdatedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(e => e.RowVersion).IsRowVersion();
            entity.HasIndex(e => new { e.TerritoryId, e.StartsAtUtc });
            entity.HasIndex(e => new { e.TerritoryId, e.Status });
            entity.HasIndex(e => new { e.Latitude, e.Longitude });
        });

        // EventParticipation
        modelBuilder.Entity<EventParticipationRecord>(entity =>
        {
            entity.ToTable("event_participations");
            entity.HasKey(p => new { p.EventId, p.UserId });
            entity.Property(p => p.Status).HasConversion<string>().HasMaxLength(32);
            entity.Property(p => p.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(p => p.UpdatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(p => new { p.EventId, p.Status });
            entity.HasIndex(p => new { p.UserId, p.Status });
        });

        base.OnModelCreating(modelBuilder);
    }
}
