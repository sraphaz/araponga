using Arah.Domain.Marketplace;

namespace Arah.Application.Interfaces;

public interface IStoreRatingResponseRepository
{
    Task<StoreRatingResponse?> GetByRatingIdAsync(Guid ratingId, CancellationToken cancellationToken);
    Task AddAsync(StoreRatingResponse response, CancellationToken cancellationToken);
}
