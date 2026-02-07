using System.Text.Json;

namespace Arah.Infrastructure.Postgres.Entities;

public sealed class TerritoryCharacterizationRecord
{
    public Guid TerritoryId { get; set; }
    public string TagsJson { get; set; } = "[]";
    public DateTime UpdatedAtUtc { get; set; }
}
