using Araponga.Application.Interfaces;
using Araponga.Application.Services;
using Araponga.Infrastructure.InMemory;

namespace Araponga.Tests.TestHelpers.Services;

public static class MembershipServiceTestHelper
{
    public static MembershipService CreateService(
        InMemoryDataStore dataStore,
        IUnitOfWork? unitOfWork = null,
        CacheInvalidationService? cacheInvalidation = null)
    {
        return new MembershipService(
            new InMemoryTerritoryMembershipRepository(dataStore),
            new InMemoryMembershipSettingsRepository(dataStore),
            new InMemoryTerritoryRepository(dataStore),
            new InMemoryAuditLogger(dataStore),
            unitOfWork ?? new InMemoryUnitOfWork(),
            cacheInvalidation);
    }
}
