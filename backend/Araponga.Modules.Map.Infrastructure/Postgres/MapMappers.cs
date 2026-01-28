using Araponga.Domain.Map;
using Araponga.Modules.Map.Infrastructure.Postgres.Entities;

namespace Araponga.Modules.Map.Infrastructure.Postgres;

public static class MapMappers
{
    public static MapEntityRecord ToRecord(this MapEntity entity)
    {
        return new MapEntityRecord
        {
            Id = entity.Id,
            TerritoryId = entity.TerritoryId,
            CreatedByUserId = entity.CreatedByUserId,
            Name = entity.Name,
            Category = entity.Category,
            Latitude = entity.Latitude,
            Longitude = entity.Longitude,
            Status = entity.Status,
            Visibility = entity.Visibility,
            ConfirmationCount = entity.ConfirmationCount,
            CreatedAtUtc = entity.CreatedAtUtc
        };
    }

    public static MapEntity ToDomain(this MapEntityRecord record)
    {
        const int maxInt32 = int.MaxValue;
        var confirmationCount = record.ConfirmationCount > maxInt32 ? maxInt32 : record.ConfirmationCount;
        return new MapEntity(
            record.Id,
            record.TerritoryId,
            record.CreatedByUserId,
            record.Name,
            record.Category,
            record.Latitude,
            record.Longitude,
            record.Status,
            record.Visibility,
            confirmationCount,
            record.CreatedAtUtc);
    }

    public static MapEntityRelationRecord ToRecord(this MapEntityRelation relation)
    {
        return new MapEntityRelationRecord
        {
            UserId = relation.UserId,
            EntityId = relation.EntityId,
            CreatedAtUtc = relation.CreatedAtUtc
        };
    }

    public static MapEntityRelation ToDomain(this MapEntityRelationRecord record)
    {
        return new MapEntityRelation(
            record.UserId,
            record.EntityId,
            record.CreatedAtUtc);
    }
}
