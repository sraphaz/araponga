using Arah.Application.Interfaces;
using Arah.Domain.Financial;

namespace Arah.Infrastructure.InMemory;

public sealed class InMemoryReconciliationRecordRepository : IReconciliationRecordRepository
{
    private readonly InMemoryDataStore _dataStore;

    public InMemoryReconciliationRecordRepository(InMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<ReconciliationRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var record = _dataStore.ReconciliationRecords.FirstOrDefault(r => r.Id == id);
        return Task.FromResult(record);
    }

    public Task<List<ReconciliationRecord>> GetByTerritoryIdAsync(Guid territoryId, CancellationToken cancellationToken)
    {
        var records = _dataStore.ReconciliationRecords
            .Where(r => r.TerritoryId == territoryId)
            .OrderByDescending(r => r.ReconciliationDate)
            .ToList();
        return Task.FromResult(records);
    }

    public Task<List<ReconciliationRecord>> GetByStatusAsync(Guid territoryId, ReconciliationStatus status, CancellationToken cancellationToken)
    {
        var records = _dataStore.ReconciliationRecords
            .Where(r => r.TerritoryId == territoryId && r.Status == status)
            .OrderByDescending(r => r.ReconciliationDate)
            .ToList();
        return Task.FromResult(records);
    }

    public Task AddAsync(ReconciliationRecord record, CancellationToken cancellationToken)
    {
        _dataStore.ReconciliationRecords.Add(record);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ReconciliationRecord record, CancellationToken cancellationToken)
    {
        var index = _dataStore.ReconciliationRecords.FindIndex(r => r.Id == record.Id);
        if (index >= 0)
        {
            _dataStore.ReconciliationRecords[index] = record;
        }
        return Task.CompletedTask;
    }
}
