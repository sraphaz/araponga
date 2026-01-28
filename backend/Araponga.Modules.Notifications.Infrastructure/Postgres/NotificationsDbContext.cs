using Araponga.Application.Interfaces;
using Araponga.Modules.Notifications.Infrastructure.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Araponga.Modules.Notifications.Infrastructure.Postgres;

/// <summary>
/// DbContext específico do módulo Notifications, contendo apenas entidades relacionadas a Notifications:
/// - UserNotification
/// </summary>
public sealed class NotificationsDbContext : DbContext, IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;

    public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
        : base(options)
    {
    }

    // Entidades de Notifications
    public DbSet<UserNotificationRecord> UserNotifications => Set<UserNotificationRecord>();

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
        // UserNotification
        modelBuilder.Entity<UserNotificationRecord>(entity =>
        {
            entity.ToTable("user_notifications");
            entity.HasKey(n => n.Id);
            entity.Property(n => n.Title).HasMaxLength(200).IsRequired();
            entity.Property(n => n.Body).HasMaxLength(1000);
            entity.Property(n => n.Kind).HasMaxLength(200).IsRequired();
            entity.Property(n => n.DataJson).HasColumnType("jsonb");
            entity.Property(n => n.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(n => n.ReadAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(n => n.UserId);
            entity.HasIndex(n => n.CreatedAtUtc);
            entity.HasIndex(n => new { n.SourceOutboxId, n.UserId }).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }
}
