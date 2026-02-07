using Arah.Application.Interfaces;
using Arah.Modules.Marketplace.Application.Interfaces;
using Arah.Modules.Marketplace.Domain;

namespace Arah.Infrastructure.InMemory;

public sealed class InMemoryCheckoutItemRepository : ICheckoutItemRepository
{
    private readonly InMemoryDataStore _dataStore;

    public InMemoryCheckoutItemRepository(InMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task AddRangeAsync(IReadOnlyList<CheckoutItem> items, CancellationToken cancellationToken)
    {
        _dataStore.CheckoutItems.AddRange(items);
        return Task.CompletedTask;
    }
}
