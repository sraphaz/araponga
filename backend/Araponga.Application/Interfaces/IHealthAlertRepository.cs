using Araponga.Domain.Health;

namespace Araponga.Application.Interfaces;

public interface IHealthAlertRepository
{
    Task<IReadOnlyList<HealthAlert>> ListByTerritoryAsync(Guid territoryId, CancellationToken cancellationToken);
    Task<HealthAlert?> GetByIdAsync(Guid alertId, CancellationToken cancellationToken);
    Task AddAsync(HealthAlert alert, CancellationToken cancellationToken);
    Task UpdateStatusAsync(Guid alertId, HealthAlertStatus status, CancellationToken cancellationToken);
}
