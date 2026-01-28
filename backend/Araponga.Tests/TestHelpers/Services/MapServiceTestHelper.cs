using Araponga.Application.Interfaces;
using Araponga.Application.Services;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests.TestHelpers.Dependencies;

namespace Araponga.Tests.TestHelpers.Services;

/// <summary>
/// Helper para criar instâncias de MapService em testes.
/// </summary>
public static class MapServiceTestHelper
{
    /// <summary>
    /// Cria uma instância de MapService com todas as dependências necessárias.
    /// </summary>
    public static MapService CreateService(
        InMemoryDataStore dataStore,
        AccessEvaluator? accessEvaluator = null,
        IAuditLogger? auditLogger = null,
        MapEntityCacheService? mapEntityCache = null,
        UserBlockCacheService? userBlockCache = null,
        CacheInvalidationService? cacheInvalidation = null)
    {
        var mapRepository = new InMemoryMapRepository(dataStore);
        var accessEvaluatorInstance = accessEvaluator ?? AccessEvaluatorTestHelper.CreateAccessEvaluator(dataStore);
        var auditLoggerInstance = auditLogger ?? new InMemoryAuditLogger(dataStore);
        var blockRepository = new InMemoryUserBlockRepository(dataStore);
        var relationRepository = new InMemoryMapEntityRelationRepository(dataStore);
        var unitOfWork = new InMemoryUnitOfWork();

        return new MapService(
            mapRepository,
            accessEvaluatorInstance,
            auditLoggerInstance,
            blockRepository,
            relationRepository,
            unitOfWork,
            mapEntityCache,
            userBlockCache,
            cacheInvalidation);
    }
}
