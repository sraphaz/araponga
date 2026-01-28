using Araponga.Application.Interfaces;
using Araponga.Application.Services;
using Araponga.Infrastructure.InMemory;

namespace Araponga.Tests.TestHelpers.Services;

public static class VerificationServiceTestHelper
{
    public static VerificationQueueService CreateService(
        InMemoryDataStore dataStore,
        IUnitOfWork? unitOfWork = null)
    {
        return new VerificationQueueService(
            new InMemoryUserRepository(dataStore),
            new InMemoryTerritoryMembershipRepository(dataStore),
            new InMemoryWorkItemRepository(dataStore),
            new InMemoryDocumentEvidenceRepository(dataStore),
            new InMemoryAuditLogger(dataStore),
            unitOfWork ?? new InMemoryUnitOfWork());
    }
}
