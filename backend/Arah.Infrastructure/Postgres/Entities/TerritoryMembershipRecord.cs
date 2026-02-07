using Arah.Domain.Membership;

namespace Arah.Infrastructure.Postgres.Entities;

public sealed class TerritoryMembershipRecord
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid TerritoryId { get; set; }
    public MembershipRole Role { get; set; }
    public ResidencyVerification ResidencyVerification { get; set; }
    public DateTime? LastGeoVerifiedAtUtc { get; set; }
    public DateTime? LastDocumentVerifiedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
