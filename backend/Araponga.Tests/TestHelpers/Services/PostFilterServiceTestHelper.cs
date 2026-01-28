using Araponga.Application.Interfaces;
using Araponga.Application.Services;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests.TestHelpers.Dependencies;

namespace Araponga.Tests.TestHelpers.Services;

/// <summary>
/// Helper para criar instâncias de PostFilterService em testes.
/// </summary>
public static class PostFilterServiceTestHelper
{
    /// <summary>
    /// Cria uma instância de PostFilterService com todas as dependências necessárias.
    /// </summary>
    public static PostFilterService CreateService(
        InMemoryDataStore dataStore,
        AccessEvaluator? accessEvaluator = null,
        UserBlockCacheService? userBlockCache = null)
    {
        var accessEvaluatorInstance = accessEvaluator ?? AccessEvaluatorTestHelper.CreateAccessEvaluator(dataStore);
        var blockRepository = new InMemoryUserBlockRepository(dataStore);
        var postAssetRepository = new InMemoryPostAssetRepository(dataStore);

        return new PostFilterService(
            accessEvaluatorInstance,
            blockRepository,
            postAssetRepository,
            userBlockCache);
    }
}
