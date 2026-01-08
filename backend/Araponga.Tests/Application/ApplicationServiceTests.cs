using Araponga.Application.Services;
using Araponga.Domain.Feed;
using Araponga.Domain.Map;
using Araponga.Domain.Social;
using Araponga.Domain.Users;
using Araponga.Infrastructure.InMemory;
using Xunit;

namespace Araponga.Tests.Application;

public sealed class ApplicationServiceTests
{
    private static readonly Guid ActiveTerritoryId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid PilotTerritoryId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    [Fact]
    public async Task FeedService_ValidatesInputsAndFlags()
    {
        var dataStore = new InMemoryDataStore();
        var feedRepository = new InMemoryFeedRepository(dataStore);
        var accessEvaluator = new AccessEvaluator(new InMemoryTerritoryMembershipRepository(dataStore));
        var featureFlags = new InMemoryFeatureFlagService();
        var auditLogger = new InMemoryAuditLogger(dataStore);
        var service = new FeedService(feedRepository, accessEvaluator, featureFlags, auditLogger);

        var invalid = await service.CreatePostAsync(
            ActiveTerritoryId,
            Guid.NewGuid(),
            "",
            "",
            PostType.General,
            PostVisibility.Public,
            CancellationToken.None);

        Assert.False(invalid.success);

        var disabledAlert = await service.CreatePostAsync(
            PilotTerritoryId,
            Guid.NewGuid(),
            "Titulo",
            "Conteudo",
            PostType.Alert,
            PostVisibility.Public,
            CancellationToken.None);

        Assert.False(disabledAlert.success);

        var likeMissing = await service.LikeAsync(
            ActiveTerritoryId,
            Guid.NewGuid(),
            "session:1",
            null,
            CancellationToken.None);

        Assert.False(likeMissing);

        var residentOnlyPost = new CommunityPost(
            Guid.NewGuid(),
            ActiveTerritoryId,
            "Title",
            "Content",
            PostType.General,
            PostVisibility.ResidentsOnly,
            DateTime.UtcNow);
        await feedRepository.AddPostAsync(residentOnlyPost, CancellationToken.None);

        var likeBlocked = await service.LikeAsync(
            ActiveTerritoryId,
            residentOnlyPost.Id,
            "session:2",
            null,
            CancellationToken.None);

        Assert.False(likeBlocked);
    }

    [Fact]
    public async Task FeedService_BlocksNonResidentCommentAndShare()
    {
        var dataStore = new InMemoryDataStore();
        var feedRepository = new InMemoryFeedRepository(dataStore);
        var accessEvaluator = new AccessEvaluator(new InMemoryTerritoryMembershipRepository(dataStore));
        var featureFlags = new InMemoryFeatureFlagService();
        var auditLogger = new InMemoryAuditLogger(dataStore);
        var service = new FeedService(feedRepository, accessEvaluator, featureFlags, auditLogger);

        var post = new CommunityPost(
            Guid.NewGuid(),
            ActiveTerritoryId,
            "Title",
            "Content",
            PostType.General,
            PostVisibility.Public,
            DateTime.UtcNow);

        await feedRepository.AddPostAsync(post, CancellationToken.None);

        var comment = await service.CommentAsync(
            ActiveTerritoryId,
            post.Id,
            Guid.NewGuid(),
            "Comentario",
            CancellationToken.None);

        Assert.False(comment.success);

        var share = await service.ShareAsync(
            ActiveTerritoryId,
            post.Id,
            Guid.NewGuid(),
            CancellationToken.None);

        Assert.False(share.success);
    }

    [Fact]
    public async Task MapService_RejectsNonResidentsAndHandlesMissingEntities()
    {
        var dataStore = new InMemoryDataStore();
        var repository = new InMemoryMapRepository(dataStore);
        var accessEvaluator = new AccessEvaluator(new InMemoryTerritoryMembershipRepository(dataStore));
        var auditLogger = new InMemoryAuditLogger(dataStore);
        var service = new MapService(repository, accessEvaluator, auditLogger);

        var suggestion = await service.SuggestAsync(
            ActiveTerritoryId,
            Guid.NewGuid(),
            "Lugar",
            "Categoria",
            CancellationToken.None);

        Assert.False(suggestion.success);

        var validate = await service.ValidateAsync(
            ActiveTerritoryId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            MapEntityStatus.Validated,
            CancellationToken.None);

        Assert.False(validate);

        var confirm = await service.ConfirmAsync(
            ActiveTerritoryId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            CancellationToken.None);

        Assert.False(confirm.success);
    }

    [Fact]
    public async Task HealthService_ValidatesAlerts()
    {
        var dataStore = new InMemoryDataStore();
        var feedRepository = new InMemoryFeedRepository(dataStore);
        var alertRepository = new InMemoryHealthAlertRepository(dataStore);
        var auditLogger = new InMemoryAuditLogger(dataStore);
        var service = new HealthService(alertRepository, feedRepository, auditLogger);

        var report = await service.ReportAlertAsync(
            ActiveTerritoryId,
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            "Titulo",
            "Desc",
            CancellationToken.None);

        Assert.True(report.success);

        var validated = await service.ValidateAlertAsync(
            ActiveTerritoryId,
            report.alert!.Id,
            Guid.Parse("cccccccc-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Araponga.Domain.Health.HealthAlertStatus.Validated,
            CancellationToken.None);

        Assert.True(validated);
        Assert.Contains(dataStore.Posts, post => post.Type == PostType.Alert);
    }

    [Fact]
    public async Task MembershipService_ReturnsStatusAndValidates()
    {
        var dataStore = new InMemoryDataStore();
        var repository = new InMemoryTerritoryMembershipRepository(dataStore);
        var auditLogger = new InMemoryAuditLogger(dataStore);
        var service = new MembershipService(repository, auditLogger);

        var status = await service.GetStatusAsync(Guid.NewGuid(), ActiveTerritoryId, CancellationToken.None);
        Assert.Null(status);

        var membership = await service.DeclareMembershipAsync(
            Guid.NewGuid(),
            ActiveTerritoryId,
            MembershipRole.Resident,
            CancellationToken.None);

        await service.ValidateAsync(
            membership.Id,
            Guid.NewGuid(),
            ActiveTerritoryId,
            VerificationStatus.Validated,
            CancellationToken.None);

        var updated = await repository.GetByUserAndTerritoryAsync(membership.UserId, ActiveTerritoryId, CancellationToken.None);
        Assert.Equal(VerificationStatus.Validated, updated!.VerificationStatus);
    }
}
