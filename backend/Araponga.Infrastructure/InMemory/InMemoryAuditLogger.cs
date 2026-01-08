using Araponga.Application.Interfaces;
using Araponga.Application.Models;

namespace Araponga.Infrastructure.InMemory;

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
