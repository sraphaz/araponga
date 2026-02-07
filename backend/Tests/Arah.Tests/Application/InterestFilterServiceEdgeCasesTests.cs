using Arah.Application.Services;
using Arah.Domain.Feed;
using Arah.Domain.Users;
using Arah.Infrastructure.InMemory;
using Arah.Infrastructure.Shared.InMemory;
using Xunit;

namespace Arah.Tests.Application;

/// <summary>
/// Edge case tests for InterestFilterService (tags overlap, fallback title/content).
/// </summary>
public sealed class InterestFilterServiceEdgeCasesTests
{
    [Fact]
    public async Task FilterFeedByInterestsAsync_WhenPostHasMatchingTags_IncludesPost()
    {
        var sharedStore = new InMemorySharedStore();
        var repo = new InMemoryUserInterestRepository(sharedStore);
        var svc = new InterestFilterService(repo);
        var userId = Guid.NewGuid();
        var territoryId = Guid.NewGuid();

        await repo.AddAsync(new UserInterest(Guid.NewGuid(), userId, "eventos", DateTime.UtcNow), CancellationToken.None);

        var postWithTag = new CommunityPost(
            Guid.NewGuid(),
            territoryId,
            Guid.NewGuid(),
            "Other title",
            "Other content",
            PostType.General,
            PostVisibility.Public,
            PostStatus.Published,
            null,
            DateTime.UtcNow,
            tags: new[] { "eventos" });

        var filtered = await svc.FilterFeedByInterestsAsync(
            new[] { postWithTag },
            userId,
            territoryId,
            CancellationToken.None);

        Assert.Single(filtered);
        Assert.Equal(postWithTag.Id, filtered[0].Id);
    }

    [Fact]
    public async Task FilterFeedByInterestsAsync_WhenPostTagsNoOverlap_ExcludesIfNoTitleContentMatch()
    {
        var sharedStore = new InMemorySharedStore();
        var repo = new InMemoryUserInterestRepository(sharedStore);
        var svc = new InterestFilterService(repo);
        var userId = Guid.NewGuid();
        var territoryId = Guid.NewGuid();

        await repo.AddAsync(new UserInterest(Guid.NewGuid(), userId, "eventos", DateTime.UtcNow), CancellationToken.None);

        var postNoMatch = new CommunityPost(
            Guid.NewGuid(),
            territoryId,
            Guid.NewGuid(),
            "Unrelated",
            "Content",
            PostType.General,
            PostVisibility.Public,
            PostStatus.Published,
            null,
            DateTime.UtcNow,
            tags: new[] { "sports" });

        var filtered = await svc.FilterFeedByInterestsAsync(
            new[] { postNoMatch },
            userId,
            territoryId,
            CancellationToken.None);

        Assert.Empty(filtered);
    }
}
