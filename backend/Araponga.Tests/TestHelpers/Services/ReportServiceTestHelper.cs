using Araponga.Application.Events;
using Araponga.Application.Interfaces;
using Araponga.Application.Services;
using Araponga.Infrastructure.Eventing;
using Araponga.Infrastructure.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Tests.TestHelpers.Services;

public static class ReportServiceTestHelper
{
    public static ReportService CreateService(
        InMemoryDataStore dataStore,
        IEventBus? eventBus = null,
        IUnitOfWork? unitOfWork = null)
    {
        var services = new ServiceCollection();
        services.AddSingleton(dataStore);
        var serviceProvider = services.BuildServiceProvider();
        
        return new ReportService(
            new InMemoryReportRepository(dataStore),
            new InMemoryFeedRepository(dataStore),
            new InMemoryUserRepository(dataStore),
            new InMemorySanctionRepository(dataStore),
            new InMemoryMediaAttachmentRepository(dataStore),
            new InMemoryAuditLogger(dataStore),
            eventBus ?? new InMemoryEventBus(serviceProvider),
            unitOfWork ?? new InMemoryUnitOfWork(),
            observabilityLogger: null);
    }
}
