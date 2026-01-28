using Araponga.Application.Interfaces;
using Araponga.Application.Services;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests.TestHelpers.Dependencies;

namespace Araponga.Tests.TestHelpers.Services;

/// <summary>
/// Helper para criar instâncias de JoinRequestService em testes.
/// </summary>
public static class JoinRequestServiceTestHelper
{
    /// <summary>
    /// Cria uma instância de JoinRequestService com todas as dependências necessárias.
    /// </summary>
    public static JoinRequestService CreateService(
        InMemoryDataStore dataStore,
        AccessEvaluator? accessEvaluator = null,
        IUnitOfWork? unitOfWork = null)
    {
        var joinRequestRepository = new InMemoryTerritoryJoinRequestRepository(dataStore);
        var membershipRepository = new InMemoryTerritoryMembershipRepository(dataStore);
        var settingsRepository = new InMemoryMembershipSettingsRepository(dataStore);
        var userRepository = new InMemoryUserRepository(dataStore);
        var accessEvaluatorInstance = accessEvaluator ?? AccessEvaluatorTestHelper.CreateAccessEvaluator(dataStore);
        var unitOfWorkInstance = unitOfWork ?? new InMemoryUnitOfWork();

        return new JoinRequestService(
            joinRequestRepository,
            membershipRepository,
            settingsRepository,
            userRepository,
            accessEvaluatorInstance,
            unitOfWorkInstance);
    }
}
