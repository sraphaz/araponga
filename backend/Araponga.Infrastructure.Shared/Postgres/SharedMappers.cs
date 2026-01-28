using Araponga.Domain.Territories;
using Araponga.Domain.Users;
using Araponga.Domain.Membership;
using Araponga.Domain.Configuration;
using Araponga.Infrastructure.Shared.Postgres.Entities;
using System.Text.Json;

namespace Araponga.Infrastructure.Shared.Postgres;

/// <summary>
/// Mappers para entidades compartilhadas entre módulos.
/// </summary>
public static class SharedMappers
{
    public static TerritoryRecord ToRecord(this Territory territory)
    {
        return new TerritoryRecord
        {
            Id = territory.Id,
            ParentTerritoryId = territory.ParentTerritoryId,
            Name = territory.Name,
            Description = territory.Description,
            Status = territory.Status,
            City = territory.City,
            State = territory.State,
            Latitude = territory.Latitude,
            Longitude = territory.Longitude,
            CreatedAtUtc = territory.CreatedAtUtc
        };
    }

    public static Territory ToDomain(this TerritoryRecord record)
    {
        return new Territory(
            record.Id,
            record.ParentTerritoryId,
            record.Name,
            record.Description,
            record.Status,
            record.City,
            record.State,
            record.Latitude,
            record.Longitude,
            record.CreatedAtUtc);
    }

    public static UserRecord ToRecord(this User user)
    {
        return new UserRecord
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email,
            Cpf = user.Cpf,
            ForeignDocument = user.ForeignDocument,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            AuthProvider = user.AuthProvider,
            ExternalId = user.ExternalId,
            TwoFactorEnabled = user.TwoFactorEnabled,
            TwoFactorSecret = user.TwoFactorSecret,
            TwoFactorRecoveryCodesHash = user.TwoFactorRecoveryCodesHash,
            TwoFactorVerifiedAtUtc = user.TwoFactorVerifiedAtUtc,
            IdentityVerificationStatus = user.IdentityVerificationStatus,
            IdentityVerifiedAtUtc = user.IdentityVerifiedAtUtc,
            AvatarMediaAssetId = user.AvatarMediaAssetId,
            Bio = user.Bio,
            CreatedAtUtc = user.CreatedAtUtc
        };
    }

    public static User ToDomain(this UserRecord record)
    {
        return new User(
            record.Id,
            record.DisplayName,
            record.Email,
            record.Cpf,
            record.ForeignDocument,
            record.PhoneNumber,
            record.Address,
            record.AuthProvider,
            record.ExternalId,
            record.TwoFactorEnabled,
            record.TwoFactorSecret,
            record.TwoFactorRecoveryCodesHash,
            record.TwoFactorVerifiedAtUtc,
            record.IdentityVerificationStatus,
            record.IdentityVerifiedAtUtc,
            record.AvatarMediaAssetId,
            record.Bio,
            record.CreatedAtUtc);
    }

    public static TerritoryMembershipRecord ToRecord(this TerritoryMembership membership)
    {
        return new TerritoryMembershipRecord
        {
            Id = membership.Id,
            UserId = membership.UserId,
            TerritoryId = membership.TerritoryId,
            Role = membership.Role,
            ResidencyVerification = membership.ResidencyVerification,
            LastGeoVerifiedAtUtc = membership.LastGeoVerifiedAtUtc,
            LastDocumentVerifiedAtUtc = membership.LastDocumentVerifiedAtUtc,
            CreatedAtUtc = membership.CreatedAtUtc
        };
    }

    public static TerritoryMembership ToDomain(this TerritoryMembershipRecord record)
    {
        return new TerritoryMembership(
            record.Id,
            record.UserId,
            record.TerritoryId,
            record.Role,
            record.ResidencyVerification,
            record.LastGeoVerifiedAtUtc,
            record.LastDocumentVerifiedAtUtc,
            record.CreatedAtUtc);
    }

    public static UserPreferencesRecord ToRecord(this UserPreferences preferences)
    {
        return new UserPreferencesRecord
        {
            UserId = preferences.UserId,
            ProfileVisibility = preferences.ProfileVisibility,
            ContactVisibility = preferences.ContactVisibility,
            ShareLocation = preferences.ShareLocation,
            ShowMemberships = preferences.ShowMemberships,
            NotificationsPostsEnabled = preferences.NotificationPreferences.PostsEnabled,
            NotificationsCommentsEnabled = preferences.NotificationPreferences.CommentsEnabled,
            NotificationsEventsEnabled = preferences.NotificationPreferences.EventsEnabled,
            NotificationsAlertsEnabled = preferences.NotificationPreferences.AlertsEnabled,
            NotificationsMarketplaceEnabled = preferences.NotificationPreferences.MarketplaceEnabled,
            NotificationsModerationEnabled = preferences.NotificationPreferences.ModerationEnabled,
            NotificationsMembershipRequestsEnabled = preferences.NotificationPreferences.MembershipRequestsEnabled,
            EmailReceiveEmails = preferences.EmailPreferences.ReceiveEmails,
            EmailFrequency = (int)preferences.EmailPreferences.EmailFrequency,
            EmailTypes = (int)preferences.EmailPreferences.EmailTypes,
            CreatedAtUtc = preferences.CreatedAtUtc,
            UpdatedAtUtc = preferences.UpdatedAtUtc
        };
    }

    public static UserPreferences ToDomain(this UserPreferencesRecord record)
    {
        var notificationPreferences = new NotificationPreferences(
            record.NotificationsPostsEnabled,
            record.NotificationsCommentsEnabled,
            record.NotificationsEventsEnabled,
            record.NotificationsAlertsEnabled,
            record.NotificationsMarketplaceEnabled,
            record.NotificationsModerationEnabled,
            record.NotificationsMembershipRequestsEnabled);

        var emailPreferences = new EmailPreferences(
            record.EmailReceiveEmails,
            (EmailFrequency)record.EmailFrequency,
            (EmailTypes)record.EmailTypes);

        return new UserPreferences(
            record.UserId,
            record.ProfileVisibility,
            record.ContactVisibility,
            record.ShareLocation,
            record.ShowMemberships,
            notificationPreferences,
            emailPreferences,
            record.CreatedAtUtc,
            record.UpdatedAtUtc);
    }

    public static MembershipSettingsRecord ToRecord(this MembershipSettings settings)
    {
        return new MembershipSettingsRecord
        {
            MembershipId = settings.MembershipId,
            MarketplaceOptIn = settings.MarketplaceOptIn,
            CreatedAtUtc = settings.CreatedAtUtc,
            UpdatedAtUtc = settings.UpdatedAtUtc
        };
    }

    public static MembershipSettings ToDomain(this MembershipSettingsRecord record)
    {
        return new MembershipSettings(
            record.MembershipId,
            record.MarketplaceOptIn,
            record.CreatedAtUtc,
            record.UpdatedAtUtc);
    }

    public static MembershipCapabilityRecord ToRecord(this MembershipCapability capability)
    {
        return new MembershipCapabilityRecord
        {
            Id = capability.Id,
            MembershipId = capability.MembershipId,
            CapabilityType = capability.CapabilityType,
            GrantedAtUtc = capability.GrantedAtUtc,
            RevokedAtUtc = capability.RevokedAtUtc,
            GrantedByUserId = capability.GrantedByUserId,
            GrantedByMembershipId = capability.GrantedByMembershipId,
            Reason = capability.Reason
        };
    }

    public static MembershipCapability ToDomain(this MembershipCapabilityRecord record)
    {
        return new MembershipCapability(
            record.Id,
            record.MembershipId,
            record.CapabilityType,
            record.GrantedAtUtc,
            record.GrantedByUserId,
            record.GrantedByMembershipId,
            record.Reason);
    }

    public static SystemPermissionRecord ToRecord(this SystemPermission permission)
    {
        return new SystemPermissionRecord
        {
            Id = permission.Id,
            UserId = permission.UserId,
            PermissionType = permission.PermissionType,
            GrantedAtUtc = permission.GrantedAtUtc,
            GrantedByUserId = permission.GrantedByUserId,
            RevokedAtUtc = permission.RevokedAtUtc,
            RevokedByUserId = permission.RevokedByUserId
        };
    }

    public static SystemPermission ToDomain(this SystemPermissionRecord record)
    {
        return new SystemPermission(
            record.Id,
            record.UserId,
            record.PermissionType,
            record.GrantedAtUtc,
            record.GrantedByUserId,
            record.RevokedAtUtc,
            record.RevokedByUserId);
    }

    public static SystemConfigRecord ToRecord(this SystemConfig config)
    {
        return new SystemConfigRecord
        {
            Id = config.Id,
            Key = config.Key,
            Value = config.Value,
            Category = config.Category,
            Description = config.Description,
            CreatedAtUtc = config.CreatedAtUtc,
            CreatedByUserId = config.CreatedByUserId,
            UpdatedAtUtc = config.UpdatedAtUtc,
            UpdatedByUserId = config.UpdatedByUserId
        };
    }

    public static SystemConfig ToDomain(this SystemConfigRecord record)
    {
        return new SystemConfig(
            record.Id,
            record.Key,
            record.Value,
            record.Category,
            record.Description,
            record.CreatedAtUtc,
            record.CreatedByUserId,
            record.UpdatedAtUtc,
            record.UpdatedByUserId);
    }
}
