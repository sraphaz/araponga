using Arah.Application.Interfaces;
using Arah.Application.Services;
using Arah.Domain.Membership;
using Arah.Modules.Moderation.Application.Interfaces;
using Arah.Modules.Moderation.Domain.Work;
using Arah.Infrastructure.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Arah.Tests.Modules.Moderation.Application;

public sealed class WorkQueueServiceTests
{
    [Fact]
    public async Task EnqueueAsync_CreatesWorkItem_AndWritesAudit()
    {
        var dataStore = new InMemoryDataStore();
        var services = new ServiceCollection();
        services.AddSingleton<InMemoryDataStore>(dataStore);
        services.AddScoped<IWorkItemRepository, InMemoryWorkItemRepository>();
        services.AddScoped<IAuditLogger, InMemoryAuditLogger>();
        services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();
        services.AddScoped<WorkQueueService>();
        var sp = services.BuildServiceProvider();
        var svc = sp.GetRequiredService<WorkQueueService>();
        var actor = Guid.NewGuid();
        var territoryId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var result = await svc.EnqueueAsync(WorkItemType.ResidencyVerification, territoryId, actor, null, MembershipCapabilityType.Curator, "MEMBERSHIP", subjectId, null, CancellationToken.None);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Contains(dataStore.AuditEntries, e => e.Action == "work_item.created" && e.ActorUserId == actor && e.TerritoryId == territoryId && e.TargetId == result.Value!.Id);
    }

    [Fact]
    public async Task CompleteAsync_CompletesWorkItem_AndWritesAudit()
    {
        var dataStore = new InMemoryDataStore();
        var services = new ServiceCollection();
        services.AddSingleton<InMemoryDataStore>(dataStore);
        services.AddScoped<IWorkItemRepository, InMemoryWorkItemRepository>();
        services.AddScoped<IAuditLogger, InMemoryAuditLogger>();
        services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();
        services.AddScoped<WorkQueueService>();
        var sp = services.BuildServiceProvider();
        var svc = sp.GetRequiredService<WorkQueueService>();
        var actor = Guid.NewGuid();
        var territoryId = Guid.NewGuid();
        var created = await svc.EnqueueAsync(WorkItemType.ModerationCase, territoryId, actor, null, MembershipCapabilityType.Moderator, "REPORT", Guid.NewGuid(), null, CancellationToken.None);
        var complete = await svc.CompleteAsync(created.Value!.Id, actor, WorkItemOutcome.Approved, "done", CancellationToken.None);
        Assert.True(complete.IsSuccess);
        Assert.Contains(dataStore.AuditEntries, e => e.Action == "work_item.completed" && e.ActorUserId == actor && e.TargetId == created.Value!.Id);
    }
}
