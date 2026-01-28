using Araponga.Application.Interfaces;
using Araponga.Modules.Subscriptions.Infrastructure.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Araponga.Modules.Subscriptions.Infrastructure.Postgres;

/// <summary>
/// DbContext específico do módulo Subscriptions, contendo apenas entidades relacionadas a Subscriptions:
/// - Subscription, SubscriptionPlan, SubscriptionPayment, SubscriptionCoupon, SubscriptionPlanHistory
/// </summary>
public sealed class SubscriptionsDbContext : DbContext, IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;

    public SubscriptionsDbContext(DbContextOptions<SubscriptionsDbContext> options)
        : base(options)
    {
    }

    // Entidades de Subscriptions
    public DbSet<SubscriptionRecord> Subscriptions => Set<SubscriptionRecord>();
    public DbSet<SubscriptionPlanRecord> SubscriptionPlans => Set<SubscriptionPlanRecord>();
    public DbSet<SubscriptionPaymentRecord> SubscriptionPayments => Set<SubscriptionPaymentRecord>();
    public DbSet<SubscriptionCouponRecord> SubscriptionCoupons => Set<SubscriptionCouponRecord>();
    public DbSet<SubscriptionPlanHistoryRecord> SubscriptionPlanHistories => Set<SubscriptionPlanHistoryRecord>();

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
        // SubscriptionPlan
        modelBuilder.Entity<SubscriptionPlanRecord>(entity =>
        {
            entity.ToTable("subscription_plans");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).HasMaxLength(200).IsRequired();
            entity.Property(p => p.Description).HasMaxLength(1000);
            entity.Property(p => p.Tier).HasConversion<int>().IsRequired();
            entity.Property(p => p.Scope).HasConversion<int>().IsRequired();
            entity.Property(p => p.PricePerCycle).HasColumnType("numeric(18,2)");
            entity.Property(p => p.BillingCycle).HasConversion<int>();
            entity.Property(p => p.CapabilitiesJson).HasColumnType("jsonb").IsRequired();
            entity.Property(p => p.LimitsJson).HasColumnType("jsonb");
            entity.Property(p => p.StripePriceId).HasMaxLength(200);
            entity.Property(p => p.StripeProductId).HasMaxLength(200);
            entity.Property(p => p.CreatedAtUtc).HasColumnType("timestamp with time zone").IsRequired();
            entity.Property(p => p.UpdatedAtUtc).HasColumnType("timestamp with time zone").IsRequired();
            entity.HasIndex(p => p.Scope);
            entity.HasIndex(p => p.TerritoryId);
            entity.HasIndex(p => new { p.Scope, p.TerritoryId });
            entity.HasIndex(p => new { p.Scope, p.IsDefault });
        });

        // Subscription
        modelBuilder.Entity<SubscriptionRecord>(entity =>
        {
            entity.ToTable("subscriptions");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Status).HasConversion<int>().IsRequired();
            entity.Property(s => s.CurrentPeriodStart).HasColumnType("timestamp with time zone").IsRequired();
            entity.Property(s => s.CurrentPeriodEnd).HasColumnType("timestamp with time zone").IsRequired();
            entity.Property(s => s.TrialStart).HasColumnType("timestamp with time zone");
            entity.Property(s => s.TrialEnd).HasColumnType("timestamp with time zone");
            entity.Property(s => s.CanceledAt).HasColumnType("timestamp with time zone");
            entity.Property(s => s.StripeSubscriptionId).HasMaxLength(200);
            entity.Property(s => s.StripeCustomerId).HasMaxLength(200);
            entity.Property(s => s.CreatedAtUtc).HasColumnType("timestamp with time zone").IsRequired();
            entity.Property(s => s.UpdatedAtUtc).HasColumnType("timestamp with time zone").IsRequired();
            entity.HasIndex(s => s.UserId);
            entity.HasIndex(s => s.TerritoryId);
            entity.HasIndex(s => s.PlanId);
            entity.HasIndex(s => new { s.UserId, s.TerritoryId });
            entity.HasIndex(s => s.Status);
            entity.HasIndex(s => s.StripeSubscriptionId).IsUnique().HasFilter("\"StripeSubscriptionId\" IS NOT NULL");
        });

        // SubscriptionPayment
        modelBuilder.Entity<SubscriptionPaymentRecord>(entity =>
        {
            entity.ToTable("subscription_payments");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Amount).HasColumnType("numeric(18,2)").IsRequired();
            entity.Property(p => p.Currency).HasMaxLength(10).IsRequired();
            entity.Property(p => p.Status).HasConversion<int>().IsRequired();
            entity.Property(p => p.PaymentDate).HasColumnType("timestamp with time zone").IsRequired();
            entity.Property(p => p.PeriodStart).HasColumnType("timestamp with time zone").IsRequired();
            entity.Property(p => p.PeriodEnd).HasColumnType("timestamp with time zone").IsRequired();
            entity.Property(p => p.StripeInvoiceId).HasMaxLength(200);
            entity.Property(p => p.StripePaymentIntentId).HasMaxLength(200);
            entity.Property(p => p.FailureReason).HasMaxLength(500);
            entity.Property(p => p.CreatedAtUtc).HasColumnType("timestamp with time zone").IsRequired();
            entity.Property(p => p.UpdatedAtUtc).HasColumnType("timestamp with time zone").IsRequired();
            entity.HasIndex(p => p.SubscriptionId);
            entity.HasIndex(p => p.Status);
            entity.HasIndex(p => p.StripeInvoiceId).IsUnique().HasFilter("\"StripeInvoiceId\" IS NOT NULL");
        });

        // SubscriptionCoupon
        modelBuilder.Entity<SubscriptionCouponRecord>(entity =>
        {
            entity.ToTable("subscription_coupons");
            entity.HasKey(sc => sc.Id);
            entity.Property(sc => sc.AppliedAtUtc).HasColumnType("timestamp with time zone").IsRequired();
            entity.HasIndex(sc => sc.SubscriptionId);
            entity.HasIndex(sc => sc.CouponId);
            entity.HasIndex(sc => new { sc.SubscriptionId, sc.CouponId }).IsUnique();
        });

        // SubscriptionPlanHistory
        modelBuilder.Entity<SubscriptionPlanHistoryRecord>(entity =>
        {
            entity.ToTable("subscription_plan_histories");
            entity.HasKey(h => h.Id);
            entity.Property(h => h.ChangeType).HasConversion<int>().IsRequired();
            entity.Property(h => h.PreviousStateJson).HasColumnType("jsonb");
            entity.Property(h => h.NewStateJson).HasColumnType("jsonb");
            entity.Property(h => h.ChangeReason).HasMaxLength(500);
            entity.Property(h => h.ChangedAtUtc).HasColumnType("timestamp with time zone").IsRequired();
            entity.HasIndex(h => h.PlanId);
            entity.HasIndex(h => h.ChangedByUserId);
            entity.HasIndex(h => new { h.PlanId, h.ChangedAtUtc });
        });

        base.OnModelCreating(modelBuilder);
    }
}
