using Araponga.Application.Interfaces;
using Araponga.Application.Services;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests.TestHelpers.Dependencies;

namespace Araponga.Tests.TestHelpers.Services;

/// <summary>
/// Helper para criar instâncias de PostInteractionService em testes.
/// </summary>
public static class PostInteractionServiceTestHelper
{
    /// <summary>
    /// Cria uma instância de PostInteractionService com todas as dependências necessárias.
    /// </summary>
    public static PostInteractionService CreateService(
        InMemoryDataStore dataStore,
        AccessEvaluator? accessEvaluator = null,
        IAuditLogger? auditLogger = null,
        IUnitOfWork? unitOfWork = null)
    {
        var feedRepository = new InMemoryFeedRepository(dataStore);
        var accessEvaluatorInstance = accessEvaluator ?? AccessEvaluatorTestHelper.CreateAccessEvaluator(dataStore);
        var sanctionRepository = new InMemorySanctionRepository(dataStore);
        var auditLoggerInstance = auditLogger ?? new InMemoryAuditLogger(dataStore);
        var unitOfWorkInstance = unitOfWork ?? new InMemoryUnitOfWork();

        return new PostInteractionService(
            feedRepository,
            accessEvaluatorInstance,
            sanctionRepository,
            auditLoggerInstance,
            unitOfWorkInstance);
    }
}
