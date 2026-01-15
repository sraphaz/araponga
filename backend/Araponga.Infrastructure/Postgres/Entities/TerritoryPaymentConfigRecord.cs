using Araponga.Domain.Marketplace;

namespace Araponga.Infrastructure.Postgres.Entities;

public sealed class TerritoryPaymentConfigRecord
{
    public Guid Id { get; set; }
    public Guid TerritoryId { get; set; }
    public string GatewayProvider { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string Currency { get; set; } = "BRL";
    public long MinimumAmount { get; set; }
    public long? MaximumAmount { get; set; }
    public bool ShowFeeBreakdown { get; set; }
    public FeeTransparencyLevel FeeTransparencyLevel { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
