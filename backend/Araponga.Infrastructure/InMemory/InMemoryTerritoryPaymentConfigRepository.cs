using Araponga.Application.Interfaces;
using Araponga.Domain.Marketplace;

namespace Araponga.Infrastructure.InMemory;

public sealed class InMemoryTerritoryPaymentConfigRepository : ITerritoryPaymentConfigRepository
{
    private readonly InMemoryDataStore _dataStore;

    public InMemoryTerritoryPaymentConfigRepository(InMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<TerritoryPaymentConfig?> GetActiveAsync(Guid territoryId, CancellationToken cancellationToken)
    {
        var config = _dataStore.TerritoryPaymentConfigs
            .FirstOrDefault(c => c.TerritoryId == territoryId && c.IsActive);
        return Task.FromResult(config);
    }

    public Task<TerritoryPaymentConfig?> GetByIdAsync(Guid configId, CancellationToken cancellationToken)
    {
        var config = _dataStore.TerritoryPaymentConfigs.FirstOrDefault(c => c.Id == configId);
        return Task.FromResult(config);
    }

    public Task<IReadOnlyList<TerritoryPaymentConfig>> ListByTerritoryAsync(
        Guid territoryId,
        CancellationToken cancellationToken)
    {
        var configs = _dataStore.TerritoryPaymentConfigs
            .Where(c => c.TerritoryId == territoryId)
            .OrderByDescending(c => c.CreatedAtUtc)
            .ToList();
        return Task.FromResult<IReadOnlyList<TerritoryPaymentConfig>>(configs);
    }

    public Task AddAsync(TerritoryPaymentConfig config, CancellationToken cancellationToken)
    {
        _dataStore.TerritoryPaymentConfigs.Add(config);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TerritoryPaymentConfig config, CancellationToken cancellationToken)
    {
        var index = _dataStore.TerritoryPaymentConfigs.FindIndex(c => c.Id == config.Id);
        if (index >= 0)
        {
            _dataStore.TerritoryPaymentConfigs[index] = config;
        }
        return Task.CompletedTask;
    }
}
