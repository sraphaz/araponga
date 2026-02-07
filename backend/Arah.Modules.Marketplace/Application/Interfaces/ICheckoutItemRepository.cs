using Arah.Modules.Marketplace.Domain;

namespace Arah.Modules.Marketplace.Application.Interfaces;

public interface ICheckoutItemRepository
{
    Task AddRangeAsync(IReadOnlyList<CheckoutItem> items, CancellationToken cancellationToken);
}
