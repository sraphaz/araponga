using Arah.Domain.Membership;

namespace Arah.Infrastructure.Postgres.Entities;

public sealed class MembershipSettingsRecord
{
    public Guid MembershipId { get; set; }
    public bool MarketplaceOptIn { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
