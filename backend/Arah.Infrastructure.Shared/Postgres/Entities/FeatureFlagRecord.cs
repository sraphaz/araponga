using Arah.Application.Models;

namespace Arah.Infrastructure.Shared.Postgres.Entities;

public sealed class FeatureFlagRecord
{
    public Guid TerritoryId { get; set; }
    public FeatureFlag Flag { get; set; }
}
