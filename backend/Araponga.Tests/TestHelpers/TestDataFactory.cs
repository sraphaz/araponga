using Araponga.Domain.Events;
using Araponga.Domain.Feed;
using Araponga.Domain.Membership;
using Araponga.Domain.Social.JoinRequests;
using Araponga.Domain.Territories;
using Araponga.Domain.Users;

namespace Araponga.Tests.TestHelpers;

/// <summary>
/// Factory para criar dados de teste reutilizáveis.
/// Reduz duplicação e facilita a criação de entidades de teste com valores padrão sensatos.
/// </summary>
public static class TestDataFactory
{
    /// <summary>
    /// Cria um Territory com valores padrão.
    /// </summary>
    public static Territory CreateTerritory(
        Guid? id = null,
        Guid? parentTerritoryId = null,
        string? name = null,
        string? description = null,
        TerritoryStatus? status = null,
        string? city = null,
        string? state = null,
        double? latitude = null,
        double? longitude = null)
    {
        return new Territory(
            id ?? Guid.NewGuid(),
            parentTerritoryId,
            name ?? "Test Territory",
            description,
            status ?? TerritoryStatus.Active,
            city ?? "Ubatuba",
            state ?? "SP",
            latitude ?? -23.3501,
            longitude ?? -44.8912,
            DateTime.UtcNow);
    }

    /// <summary>
    /// Cria um User com valores padrão.
    /// </summary>
    public static User CreateUser(
        Guid? id = null,
        string? displayName = null,
        string? email = null,
        string? cpf = null,
        string? foreignDocument = null,
        string? phoneNumber = null,
        string? address = null,
        string? authProvider = null,
        string? externalId = null)
    {
        return new User(
            id ?? Guid.NewGuid(),
            displayName ?? "Test User",
            email ?? "test@example.com",
            cpf ?? "123.456.789-00",
            foreignDocument,
            phoneNumber,
            address,
            authProvider ?? "google",
            externalId ?? Guid.NewGuid().ToString(),
            DateTime.UtcNow);
    }

    /// <summary>
    /// Cria um CommunityPost com valores padrão.
    /// </summary>
    public static CommunityPost CreatePost(
        Guid territoryId,
        Guid userId,
        Guid? id = null,
        string? title = null,
        string? content = null,
        PostType? type = null,
        PostVisibility? visibility = null,
        PostStatus? status = null)
    {
        return new CommunityPost(
            id ?? Guid.NewGuid(),
            territoryId,
            userId,
            title ?? "Test Post",
            content ?? "Test Content",
            type ?? PostType.General,
            visibility ?? PostVisibility.Public,
            status ?? PostStatus.Published,
            null,
            DateTime.UtcNow);
    }

    /// <summary>
    /// Cria um TerritoryEvent com valores padrão.
    /// </summary>
    public static TerritoryEvent CreateEvent(
        Guid territoryId,
        Guid userId,
        Guid? id = null,
        string? title = null,
        string? description = null,
        DateTime? startsAtUtc = null,
        DateTime? endsAtUtc = null,
        double? latitude = null,
        double? longitude = null,
        MembershipRole? membershipRole = null,
        EventStatus? status = null)
    {
        var now = DateTime.UtcNow;
        return new TerritoryEvent(
            id ?? Guid.NewGuid(),
            territoryId,
            title ?? "Test Event",
            description,
            startsAtUtc ?? now.AddDays(1),
            endsAtUtc ?? now.AddDays(2),
            latitude ?? -23.3501,
            longitude ?? -44.8912,
            null,
            userId,
            membershipRole ?? MembershipRole.Resident,
            status ?? EventStatus.Scheduled,
            now,
            now);
    }

    /// <summary>
    /// Cria um TerritoryMembership com valores padrão.
    /// </summary>
    public static TerritoryMembership CreateMembership(
        Guid userId,
        Guid territoryId,
        Guid? id = null,
        MembershipRole? role = null,
        ResidencyVerification? verification = null)
    {
        return new TerritoryMembership(
            id ?? Guid.NewGuid(),
            userId,
            territoryId,
            role ?? MembershipRole.Visitor,
            verification ?? ResidencyVerification.None,
            null,
            null,
            DateTime.UtcNow);
    }

    /// <summary>
    /// Cria um TerritoryJoinRequest com valores padrão.
    /// </summary>
    public static TerritoryJoinRequest CreateJoinRequest(
        Guid territoryId,
        Guid requesterUserId,
        Guid? id = null,
        string? message = null,
        TerritoryJoinRequestStatus? status = null)
    {
        var now = DateTime.UtcNow;
        return new TerritoryJoinRequest(
            id ?? Guid.NewGuid(),
            territoryId,
            requesterUserId,
            message,
            status ?? TerritoryJoinRequestStatus.Pending,
            now,
            null,
            null,
            null);
    }

    /// <summary>
    /// Cria um TerritoryJoinRequestRecipient com valores padrão.
    /// </summary>
    public static TerritoryJoinRequestRecipient CreateJoinRequestRecipient(
        Guid requestId,
        Guid recipientUserId,
        DateTime? createdAtUtc = null)
    {
        return new TerritoryJoinRequestRecipient(
            requestId,
            recipientUserId,
            createdAtUtc ?? DateTime.UtcNow,
            null);
    }
}
