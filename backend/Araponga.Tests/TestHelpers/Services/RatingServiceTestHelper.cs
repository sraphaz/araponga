using Araponga.Application.Interfaces;
using Araponga.Application.Services;
using Araponga.Infrastructure.InMemory;

namespace Araponga.Tests.TestHelpers.Services;

/// <summary>
/// Helper para criar instâncias de RatingService em testes.
/// </summary>
public static class RatingServiceTestHelper
{
    /// <summary>
    /// Cria uma instância de RatingService com todas as dependências necessárias.
    /// </summary>
    public static RatingService CreateService(
        InMemoryDataStore dataStore,
        IUnitOfWork? unitOfWork = null)
    {
        var storeRatingRepository = new InMemoryStoreRatingRepository(dataStore);
        var itemRatingRepository = new InMemoryStoreItemRatingRepository(dataStore);
        var ratingResponseRepository = new InMemoryStoreRatingResponseRepository(dataStore);
        var storeRepository = new InMemoryStoreRepository(dataStore);
        var itemRepository = new InMemoryStoreItemRepository(dataStore);
        var checkoutRepository = new InMemoryCheckoutRepository(dataStore);
        var unitOfWorkInstance = unitOfWork ?? new InMemoryUnitOfWork();

        return new RatingService(
            storeRatingRepository,
            itemRatingRepository,
            ratingResponseRepository,
            storeRepository,
            itemRepository,
            checkoutRepository,
            unitOfWorkInstance);
    }
}
