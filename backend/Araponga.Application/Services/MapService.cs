using Araponga.Application.Interfaces;
using Araponga.Domain.Map;

namespace Araponga.Application.Services;

public sealed class MapService
{
    private readonly IMapRepository _mapRepository;
    private readonly AccessEvaluator _accessEvaluator;
    private readonly IAuditLogger _auditLogger;

    public MapService(IMapRepository mapRepository, AccessEvaluator accessEvaluator, IAuditLogger auditLogger)
    {
        _mapRepository = mapRepository;
        _accessEvaluator = accessEvaluator;
        _auditLogger = auditLogger;
    }

    public async Task<IReadOnlyList<MapEntity>> ListEntitiesAsync(
        Guid territoryId,
        Guid? userId,
        CancellationToken cancellationToken)
    {
        var entities = await _mapRepository.ListByTerritoryAsync(territoryId, cancellationToken);

        if (userId is null)
        {
            return entities
                .Where(entity => entity.Visibility == MapEntityVisibility.Public)
                .ToList();
        }

        var isResident = await _accessEvaluator.IsResidentAsync(userId.Value, territoryId, cancellationToken);

        return isResident
            ? entities.ToList()
            : entities.Where(entity => entity.Visibility == MapEntityVisibility.Public).ToList();
    }

    public async Task<(bool success, string? error, MapEntity? entity)> SuggestAsync(
        Guid territoryId,
        Guid userId,
        string name,
        string category,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(category))
        {
            return (false, "Name and category are required.", null);
        }

        var isResident = await _accessEvaluator.IsResidentAsync(userId, territoryId, cancellationToken);
        if (!isResident)
        {
            return (false, "Only residents can suggest entities.", null);
        }

        var entity = new MapEntity(
            Guid.NewGuid(),
            territoryId,
            name,
            category,
            MapEntityStatus.Suggested,
            MapEntityVisibility.ResidentsOnly,
            0,
            DateTime.UtcNow);

        await _mapRepository.AddAsync(entity, cancellationToken);

        await _auditLogger.LogAsync(
            new Models.AuditEntry("map.suggested", userId, territoryId, entity.Id, DateTime.UtcNow),
            cancellationToken);

        return (true, null, entity);
    }

    public async Task<bool> ValidateAsync(
        Guid territoryId,
        Guid entityId,
        Guid curatorId,
        MapEntityStatus status,
        CancellationToken cancellationToken)
    {
        var entity = await _mapRepository.GetByIdAsync(entityId, cancellationToken);
        if (entity is null || entity.TerritoryId != territoryId)
        {
            return false;
        }

        await _mapRepository.UpdateStatusAsync(entityId, status, cancellationToken);

        await _auditLogger.LogAsync(
            new Models.AuditEntry($"map.{status.ToString().ToLowerInvariant()}", curatorId, territoryId, entityId, DateTime.UtcNow),
            cancellationToken);

        return true;
    }

    public async Task<(bool success, string? error)> ConfirmAsync(
        Guid territoryId,
        Guid entityId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var entity = await _mapRepository.GetByIdAsync(entityId, cancellationToken);
        if (entity is null || entity.TerritoryId != territoryId)
        {
            return (false, "Entity not found.");
        }

        var isResident = await _accessEvaluator.IsResidentAsync(userId, territoryId, cancellationToken);
        if (!isResident)
        {
            return (false, "Only residents can confirm entities.");
        }

        await _mapRepository.IncrementConfirmationAsync(entityId, cancellationToken);

        await _auditLogger.LogAsync(
            new Models.AuditEntry("map.confirmed", userId, territoryId, entityId, DateTime.UtcNow),
            cancellationToken);

        return (true, null);
    }
}
