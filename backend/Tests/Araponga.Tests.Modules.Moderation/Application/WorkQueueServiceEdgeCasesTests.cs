using Araponga.Application.Services;
using Araponga.Modules.Moderation.Domain.Work;
using Araponga.Infrastructure.InMemory;
using Xunit;

namespace Araponga.Tests.Modules.Moderation.Application;

public sealed class WorkQueueServiceEdgeCasesTests
{
    [Fact]
    public async Task ListAsync_WhenNoItems_ReturnsEmpty()
    {
        var ds = new InMemoryDataStore();
        ds.WorkItems.Clear();
        var svc = new WorkQueueService(new InMemoryWorkItemRepository(ds), new InMemoryAuditLogger(ds), new InMemoryUnitOfWork());
        var list = await svc.ListAsync(null, null, null, CancellationToken.None);
        Assert.NotNull(list);
        Assert.Empty(list);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ReturnsNull()
    {
        var ds = new InMemoryDataStore();
        var svc = new WorkQueueService(new InMemoryWorkItemRepository(ds), new InMemoryAuditLogger(ds), new InMemoryUnitOfWork());
        var item = await svc.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);
        Assert.Null(item);
    }

    [Fact]
    public async Task EnqueueAsync_WithValidArgs_ReturnsSuccess()
    {
        var ds = new InMemoryDataStore();
        var svc = new WorkQueueService(new InMemoryWorkItemRepository(ds), new InMemoryAuditLogger(ds), new InMemoryUnitOfWork());
        var territoryId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var result = await svc.EnqueueAsync(WorkItemType.IdentityVerification, territoryId, Guid.NewGuid(), null, null, "post", Guid.NewGuid(), null, CancellationToken.None);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(WorkItemStatus.Pending, result.Value.Status);
    }
}
