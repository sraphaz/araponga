using Arah.Modules.Map.Domain;

namespace Arah.Modules.Map.Application.Interfaces;

public interface IMapEntityRelationRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid entityId, CancellationToken cancellationToken);
    Task AddAsync(MapEntityRelation relation, CancellationToken cancellationToken);
}
