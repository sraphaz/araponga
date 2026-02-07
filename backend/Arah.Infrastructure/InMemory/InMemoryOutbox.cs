using Arah.Application.Interfaces;
using Arah.Application.Models;

namespace Arah.Infrastructure.InMemory;

public sealed class InMemoryOutbox : IOutbox
{
    private readonly InMemoryDataStore _dataStore;

    public InMemoryOutbox(InMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task EnqueueAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        _dataStore.OutboxMessages.Add(message);
        return Task.CompletedTask;
    }
}
