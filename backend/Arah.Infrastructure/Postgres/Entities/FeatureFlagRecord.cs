using Arah.Application.Models;

namespace Arah.Infrastructure.Postgres.Entities;

public sealed class FeatureFlagRecord
{
    public Guid TerritoryId { get; set; }
    public FeatureFlag Flag { get; set; }
}
