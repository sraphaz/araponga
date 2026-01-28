using Araponga.Application.Interfaces;
using Araponga.Infrastructure.Shared.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Araponga.Infrastructure.Shared.Postgres;

/// <summary>
/// DbContext compartilhado contendo apenas entidades compartilhadas entre módulos:
/// - Territory, User, Membership (entidades core)
/// - Outbox, Audit, FeatureFlags (cross-cutting)
/// </summary>
public sealed class SharedDbContext : DbContext, IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;

    public SharedDbContext(DbContextOptions<SharedDbContext> options)
        : base(options)
    {
    }

    // Entidades Core Compartilhadas
    public DbSet<TerritoryRecord> Territories => Set<TerritoryRecord>();
    public DbSet<UserRecord> Users => Set<UserRecord>();
    public DbSet<UserPreferencesRecord> UserPreferences => Set<UserPreferencesRecord>();
    public DbSet<UserDeviceRecord> UserDevices => Set<UserDeviceRecord>();
    public DbSet<UserInterestRecord> UserInterests => Set<UserInterestRecord>();
    public DbSet<TerritoryMembershipRecord> TerritoryMemberships => Set<TerritoryMembershipRecord>();
    public DbSet<MembershipSettingsRecord> MembershipSettings => Set<MembershipSettingsRecord>();
    public DbSet<MembershipCapabilityRecord> MembershipCapabilities => Set<MembershipCapabilityRecord>();
    public DbSet<SystemPermissionRecord> SystemPermissions => Set<SystemPermissionRecord>();
    public DbSet<SystemConfigRecord> SystemConfigs => Set<SystemConfigRecord>();

    // Cross-cutting (usado por múltiplos módulos)
    public DbSet<OutboxMessageRecord> OutboxMessages => Set<OutboxMessageRecord>();
    public DbSet<AuditEntryRecord> AuditEntries => Set<AuditEntryRecord>();
    public DbSet<FeatureFlagRecord> FeatureFlags => Set<FeatureFlagRecord>();
    public DbSet<ActiveTerritoryRecord> ActiveTerritories => Set<ActiveTerritoryRecord>();

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
        // Territory
        modelBuilder.Entity<TerritoryRecord>(entity =>
        {
            entity.ToTable("territories");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).HasMaxLength(200).IsRequired();
            entity.Property(t => t.Description).HasMaxLength(1000);
            entity.Property(t => t.Status).HasConversion<int>();
            entity.Property(t => t.City).HasMaxLength(120).IsRequired();
            entity.Property(t => t.State).HasMaxLength(60).IsRequired();
            entity.Property(t => t.Latitude).HasColumnType("double precision");
            entity.Property(t => t.Longitude).HasColumnType("double precision");
            entity.Property(t => t.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(t => t.Name);
            entity.HasIndex(t => t.City);
            entity.HasIndex(t => t.State);
            entity.HasIndex(t => new { t.City, t.State });
        });

        // User
        modelBuilder.Entity<UserRecord>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.DisplayName).HasMaxLength(200).IsRequired();
            entity.Property(u => u.Email).HasMaxLength(320).IsRequired();
            entity.Property(u => u.AuthProvider).HasMaxLength(80).IsRequired();
            entity.Property(u => u.ExternalId).HasMaxLength(160).IsRequired();
            entity.Property(u => u.TwoFactorSecret).HasMaxLength(500);
            entity.Property(u => u.TwoFactorRecoveryCodesHash).HasMaxLength(500);
            entity.Property(u => u.TwoFactorVerifiedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(u => u.IdentityVerificationStatus).HasConversion<int>().IsRequired();
            entity.Property(u => u.IdentityVerifiedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(u => u.AvatarMediaAssetId);
            entity.Property(u => u.Bio).HasMaxLength(500);
            entity.Property(u => u.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => new { u.AuthProvider, u.ExternalId }).IsUnique();
        });

        // UserPreferences
        modelBuilder.Entity<UserPreferencesRecord>(entity =>
        {
            entity.ToTable("user_preferences");
            entity.HasKey(p => p.UserId);
            entity.Property(p => p.ProfileVisibility).HasConversion<int>().IsRequired();
            entity.Property(p => p.ContactVisibility).HasConversion<int>().IsRequired();
            entity.Property(p => p.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(p => p.UpdatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasOne<UserRecord>()
                .WithOne()
                .HasForeignKey<UserPreferencesRecord>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(p => p.UserId).IsUnique();
        });

        // UserDevice
        modelBuilder.Entity<UserDeviceRecord>(entity =>
        {
            entity.ToTable("user_devices");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.UserId).IsRequired();
            entity.Property(d => d.DeviceToken).HasMaxLength(500).IsRequired();
            entity.Property(d => d.Platform).HasMaxLength(50).IsRequired();
            entity.Property(d => d.DeviceName).HasMaxLength(200);
            entity.Property(d => d.RegisteredAtUtc).HasColumnType("timestamp with time zone").IsRequired();
            entity.Property(d => d.LastUsedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(d => d.IsActive).IsRequired();
            entity.HasIndex(d => d.UserId);
            entity.HasIndex(d => d.DeviceToken).IsUnique();
            entity.HasIndex(d => new { d.UserId, d.IsActive });
        });

        // UserInterest
        modelBuilder.Entity<UserInterestRecord>(entity =>
        {
            entity.ToTable("user_interests");
            entity.HasKey(i => i.Id);
            entity.Property(i => i.InterestTag).HasMaxLength(50).IsRequired();
            entity.Property(i => i.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(i => i.UserId);
            entity.HasIndex(i => i.InterestTag);
            entity.HasIndex(i => new { i.UserId, i.InterestTag }).IsUnique();
            entity.HasOne<UserRecord>()
                .WithMany()
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // TerritoryMembership
        modelBuilder.Entity<TerritoryMembershipRecord>(entity =>
        {
            entity.ToTable("territory_memberships");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Role).HasConversion<int>();
            entity.Property(m => m.ResidencyVerification).HasConversion<int>();
            entity.Property(m => m.LastGeoVerifiedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(m => m.LastDocumentVerifiedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(m => m.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(m => m.RowVersion).IsRowVersion();
            entity.HasIndex(m => m.UserId);
            entity.HasIndex(m => m.TerritoryId);
            entity.HasIndex(m => new { m.UserId, m.TerritoryId }).IsUnique();
            entity.HasIndex(m => m.UserId)
                .HasFilter("\"Role\" = 1")
                .IsUnique();
        });

        // MembershipSettings
        modelBuilder.Entity<MembershipSettingsRecord>(entity =>
        {
            entity.ToTable("membership_settings");
            entity.HasKey(s => s.MembershipId);
            entity.Property(s => s.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(s => s.UpdatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasOne<TerritoryMembershipRecord>()
                .WithOne()
                .HasForeignKey<MembershipSettingsRecord>(s => s.MembershipId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(s => s.MembershipId).IsUnique();
        });

        // MembershipCapability
        modelBuilder.Entity<MembershipCapabilityRecord>(entity =>
        {
            entity.ToTable("membership_capabilities");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.CapabilityType).HasConversion<int>();
            entity.Property(c => c.GrantedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(c => c.RevokedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(c => c.Reason).HasMaxLength(500);
            entity.HasOne<TerritoryMembershipRecord>()
                .WithMany()
                .HasForeignKey(c => c.MembershipId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(c => c.MembershipId);
            entity.HasIndex(c => new { c.MembershipId, c.CapabilityType })
                .HasFilter("\"RevokedAtUtc\" IS NULL");
        });

        // SystemPermission
        modelBuilder.Entity<SystemPermissionRecord>(entity =>
        {
            entity.ToTable("system_permissions");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.PermissionType).HasConversion<int>();
            entity.Property(p => p.GrantedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(p => p.RevokedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasOne<UserRecord>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(p => p.UserId);
            entity.HasIndex(p => new { p.UserId, p.PermissionType })
                .IsUnique()
                .HasFilter("\"RevokedAtUtc\" IS NULL");
            entity.HasIndex(p => p.PermissionType)
                .HasFilter("\"RevokedAtUtc\" IS NULL");
        });

        // SystemConfig
        modelBuilder.Entity<SystemConfigRecord>(entity =>
        {
            entity.ToTable("system_configs");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Key).HasMaxLength(200).IsRequired();
            entity.Property(c => c.Value).HasColumnType("text").IsRequired();
            entity.Property(c => c.Category).HasConversion<int>();
            entity.Property(c => c.Description).HasMaxLength(1000);
            entity.Property(c => c.CreatedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(c => c.UpdatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(c => c.Key).IsUnique();
            entity.HasIndex(c => c.Category);
        });

        // OutboxMessage
        modelBuilder.Entity<OutboxMessageRecord>(entity =>
        {
            entity.ToTable("outbox_messages");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Type).HasMaxLength(200).IsRequired();
            entity.Property(o => o.PayloadJson).HasColumnType("jsonb");
            entity.Property(o => o.OccurredAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(o => o.ProcessedAtUtc).HasColumnType("timestamp with time zone");
            entity.Property(o => o.ProcessAfterUtc).HasColumnType("timestamp with time zone");
            entity.Property(o => o.Attempts).HasDefaultValue(0);
            entity.Property(o => o.LastError).HasColumnType("text");
            entity.HasIndex(o => new { o.ProcessedAtUtc, o.ProcessAfterUtc });
            entity.HasIndex(o => new { o.Type, o.ProcessedAtUtc });
        });

        // AuditEntry
        modelBuilder.Entity<AuditEntryRecord>(entity =>
        {
            entity.ToTable("audit_entries");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Action).HasMaxLength(200).IsRequired();
            entity.Property(a => a.TimestampUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(a => a.TerritoryId);
            entity.HasIndex(a => a.ActorUserId);
            entity.HasIndex(a => a.TimestampUtc);
        });

        // FeatureFlag
        modelBuilder.Entity<FeatureFlagRecord>(entity =>
        {
            entity.ToTable("feature_flags");
            entity.HasKey(f => new { f.TerritoryId, f.Flag });
            entity.Property(f => f.Flag).HasConversion<int>();
            entity.HasIndex(f => f.TerritoryId);
        });

        // ActiveTerritory
        modelBuilder.Entity<ActiveTerritoryRecord>(entity =>
        {
            entity.ToTable("active_territories");
            entity.HasKey(a => a.SessionId);
            entity.Property(a => a.SessionId).HasMaxLength(200).IsRequired();
            entity.Property(a => a.UpdatedAtUtc).HasColumnType("timestamp with time zone");
            entity.HasIndex(a => a.TerritoryId);
        });

        base.OnModelCreating(modelBuilder);
    }
}
