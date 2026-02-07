using Arah.Application.Interfaces;
using Arah.Application.Models;

namespace Arah.Infrastructure.InMemory;

public sealed class InMemoryAuditLogger : IAuditLogger
{
    private readonly InMemoryDataStore _dataStore;

    public InMemoryAuditLogger(InMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task LogAsync(AuditEntry entry, CancellationToken cancellationToken)
    {
        _dataStore.AuditEntries.Add(entry);
        return Task.CompletedTask;
    }
}
