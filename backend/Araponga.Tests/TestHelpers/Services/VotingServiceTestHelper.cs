using Araponga.Application.Interfaces;
using Araponga.Application.Services;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests.TestHelpers.Dependencies;

namespace Araponga.Tests.TestHelpers.Services;

/// <summary>
/// Helper para criar instâncias de VotingService em testes.
/// </summary>
public static class VotingServiceTestHelper
{
    /// <summary>
    /// Cria uma instância de VotingService com todas as dependências necessárias.
    /// </summary>
    public static VotingService CreateService(
        InMemoryDataStore dataStore,
        AccessEvaluator? accessEvaluator = null,
        IUnitOfWork? unitOfWork = null,
        TerritoryCharacterizationService? characterizationService = null,
        TerritoryModerationService? moderationService = null)
    {
        var votingRepository = new InMemoryVotingRepository(dataStore);
        var voteRepository = new InMemoryVoteRepository(dataStore);
        var membershipRepository = new InMemoryTerritoryMembershipRepository(dataStore);
        var accessEvaluatorInstance = accessEvaluator ?? AccessEvaluatorTestHelper.CreateAccessEvaluator(dataStore);
        var unitOfWorkInstance = unitOfWork ?? new InMemoryUnitOfWork();

        return new VotingService(
            votingRepository,
            voteRepository,
            membershipRepository,
            accessEvaluatorInstance,
            unitOfWorkInstance,
            characterizationService,
            moderationService);
    }
}
