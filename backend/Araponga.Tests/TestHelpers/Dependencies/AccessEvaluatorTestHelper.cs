using Araponga.Application.Interfaces;
using Araponga.Application.Services;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests.TestHelpers;

namespace Araponga.Tests.TestHelpers.Dependencies;

/// <summary>
/// Helper para criar instâncias de AccessEvaluator em testes.
/// AccessEvaluator é uma dependência comum usada em múltiplos serviços.
/// </summary>
public static class AccessEvaluatorTestHelper
{
    /// <summary>
    /// Cria uma instância de AccessEvaluator com todas as dependências necessárias.
    /// </summary>
    public static AccessEvaluator CreateAccessEvaluator(InMemoryDataStore dataStore)
    {
        var membershipRepository = new InMemoryTerritoryMembershipRepository(dataStore);
        var capabilityRepository = new InMemoryMembershipCapabilityRepository(dataStore);
        var permissionRepository = new InMemorySystemPermissionRepository(dataStore);
        var settingsRepository = new InMemoryMembershipSettingsRepository(dataStore);
        var userRepository = new InMemoryUserRepository(dataStore);
        var featureFlags = new InMemoryFeatureFlagService();
        var accessRules = new MembershipAccessRules(
            membershipRepository,
            settingsRepository,
            userRepository,
            featureFlags);
        var cache = CacheTestHelper.CreateDistributedCacheService();

        return new AccessEvaluator(
            membershipRepository,
            capabilityRepository,
            permissionRepository,
            accessRules,
            cache);
    }
}
