using Araponga.Domain.Marketplace;

namespace Araponga.Application.Interfaces;

public interface ICheckoutRepository
{
    Task AddAsync(Checkout checkout, CancellationToken cancellationToken);
    Task UpdateAsync(Checkout checkout, CancellationToken cancellationToken);
    Task<Checkout?> GetByIdAsync(Guid checkoutId, CancellationToken cancellationToken);
    Task<Checkout?> GetByPaymentIntentIdAsync(string paymentIntentId, CancellationToken cancellationToken);
}
