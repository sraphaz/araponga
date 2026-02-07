using Arah.Application.Models;

namespace Arah.Application.Interfaces;

/// <summary>
/// Repositório para consulta de entradas de auditoria.
/// </summary>
public interface IAuditRepository
{
    Task<IReadOnlyList<AuditEntry>> ListAsync(
        Guid? territoryId,
        Guid? actorUserId,
        string? action,
        int skip,
        int take,
        CancellationToken cancellationToken);

    Task<int> CountAsync(
        Guid? territoryId,
        Guid? actorUserId,
        string? action,
        CancellationToken cancellationToken);
}
