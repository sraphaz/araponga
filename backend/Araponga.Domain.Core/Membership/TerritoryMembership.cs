namespace Araponga.Domain.Core.Membership;

/// <summary>
/// Representa o vínculo entre um User e um Territory.
/// Define o papel (Role) e o status de verificação de residência (ResidencyVerification).
/// </summary>
public sealed class TerritoryMembership
{
    /// <summary>
    /// Inicializa uma nova instância de TerritoryMembership.
    /// </summary>
    public TerritoryMembership(
        Guid id,
        Guid userId,
        Guid territoryId,
        MembershipRole role,
        ResidencyVerification residencyVerification,
        DateTime? lastGeoVerifiedAtUtc,
        DateTime? lastDocumentVerifiedAtUtc,
        DateTime createdAtUtc)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID is required.", nameof(userId));
        }

        if (territoryId == Guid.Empty)
        {
            throw new ArgumentException("Territory ID is required.", nameof(territoryId));
        }

        Id = id;
        UserId = userId;
        TerritoryId = territoryId;
        Role = role;
        ResidencyVerification = residencyVerification;
        LastGeoVerifiedAtUtc = lastGeoVerifiedAtUtc;
        LastDocumentVerifiedAtUtc = lastDocumentVerifiedAtUtc;
        CreatedAtUtc = createdAtUtc;
    }

    public Guid Id { get; }
    public Guid UserId { get; }
    public Guid TerritoryId { get; }
    public MembershipRole Role { get; private set; }
    public ResidencyVerification ResidencyVerification { get; private set; }
    public DateTime? LastGeoVerifiedAtUtc { get; private set; }
    public DateTime? LastDocumentVerifiedAtUtc { get; private set; }
    public DateTime CreatedAtUtc { get; }

    public void UpdateRole(MembershipRole role)
    {
        Role = role;
    }

    public void UpdateResidencyVerification(ResidencyVerification verification)
    {
        ResidencyVerification = verification;
    }

    public void AddGeoVerification(DateTime verifiedAtUtc)
    {
        ResidencyVerification |= ResidencyVerification.GeoVerified;
        LastGeoVerifiedAtUtc = verifiedAtUtc;
    }

    public void AddDocumentVerification(DateTime verifiedAtUtc)
    {
        ResidencyVerification |= ResidencyVerification.DocumentVerified;
        LastDocumentVerifiedAtUtc = verifiedAtUtc;
    }

    public void RemoveGeoVerification()
    {
        ResidencyVerification &= ~ResidencyVerification.GeoVerified;
        LastGeoVerifiedAtUtc = null;
    }

    public void RemoveDocumentVerification()
    {
        ResidencyVerification &= ~ResidencyVerification.DocumentVerified;
        LastDocumentVerifiedAtUtc = null;
    }

    public bool HasAnyVerification() => ResidencyVerification != ResidencyVerification.None;

    public bool IsGeoVerified() => (ResidencyVerification & ResidencyVerification.GeoVerified) != 0;

    public bool IsDocumentVerified() => (ResidencyVerification & ResidencyVerification.DocumentVerified) != 0;

    [Obsolete("Use AddGeoVerification instead")]
    public void UpdateGeoVerification(DateTime verifiedAtUtc)
    {
        AddGeoVerification(verifiedAtUtc);
    }

    [Obsolete("Use AddDocumentVerification instead")]
    public void UpdateDocumentVerification(DateTime verifiedAtUtc)
    {
        AddDocumentVerification(verifiedAtUtc);
    }
}
