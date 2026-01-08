using Araponga.Application.Interfaces;
using Araponga.Domain.Feed;
using Araponga.Domain.Social;

namespace Araponga.Application.Services;

public sealed class FeedService
{
    private readonly IFeedRepository _feedRepository;
    private readonly AccessEvaluator _accessEvaluator;
    private readonly IFeatureFlagService _featureFlags;
    private readonly IAuditLogger _auditLogger;

    public FeedService(
        IFeedRepository feedRepository,
        AccessEvaluator accessEvaluator,
        IFeatureFlagService featureFlags,
        IAuditLogger auditLogger)
    {
        _feedRepository = feedRepository;
        _accessEvaluator = accessEvaluator;
        _featureFlags = featureFlags;
        _auditLogger = auditLogger;
    }

    public async Task<IReadOnlyList<CommunityPost>> ListForTerritoryAsync(
        Guid territoryId,
        Guid? userId,
        CancellationToken cancellationToken)
    {
        var posts = await _feedRepository.ListByTerritoryAsync(territoryId, cancellationToken);

        if (userId is null)
        {
            return posts
                .Where(post => post.Visibility == PostVisibility.Public)
                .ToList();
        }

        var isResident = await _accessEvaluator.IsResidentAsync(userId.Value, territoryId, cancellationToken);

        return isResident
            ? posts.ToList()
            : posts.Where(post => post.Visibility == PostVisibility.Public).ToList();
    }

    public async Task<(bool success, string? error, CommunityPost? post)> CreatePostAsync(
        Guid territoryId,
        Guid userId,
        string title,
        string content,
        PostType type,
        PostVisibility visibility,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
        {
            return (false, "Title and content are required.", null);
        }

        if (type == PostType.Alert && !_featureFlags.IsEnabled(territoryId, Models.FeatureFlag.AlertPosts))
        {
            return (false, "Alert posts are disabled for this territory.", null);
        }

        if (type == PostType.Event && !_featureFlags.IsEnabled(territoryId, Models.FeatureFlag.EventPosts))
        {
            return (false, "Event posts are disabled for this territory.", null);
        }

        var post = new CommunityPost(
            Guid.NewGuid(),
            territoryId,
            title,
            content,
            type,
            visibility,
            DateTime.UtcNow);

        await _feedRepository.AddPostAsync(post, cancellationToken);

        await _auditLogger.LogAsync(
            new Models.AuditEntry("post.created", userId, territoryId, post.Id, DateTime.UtcNow),
            cancellationToken);

        return (true, null, post);
    }

    public async Task<bool> LikeAsync(
        Guid territoryId,
        Guid postId,
        string actorId,
        Guid? userId,
        CancellationToken cancellationToken)
    {
        var post = await _feedRepository.GetPostAsync(postId, cancellationToken);
        if (post is null || post.TerritoryId != territoryId)
        {
            return false;
        }

        if (post.Visibility == PostVisibility.ResidentsOnly)
        {
            if (userId is null)
            {
                return false;
            }

            var membershipRole = await _accessEvaluator.GetRoleAsync(userId.Value, territoryId, cancellationToken);
            if (membershipRole != MembershipRole.Resident)
            {
                return false;
            }
        }

        await _feedRepository.AddLikeAsync(postId, actorId, cancellationToken);
        return true;
    }

    public async Task<(bool success, string? error)> CommentAsync(
        Guid territoryId,
        Guid postId,
        Guid userId,
        string content,
        CancellationToken cancellationToken)
    {
        var post = await _feedRepository.GetPostAsync(postId, cancellationToken);
        if (post is null || post.TerritoryId != territoryId)
        {
            return (false, "Post not found.");
        }

        var isResident = await _accessEvaluator.IsResidentAsync(userId, territoryId, cancellationToken);
        if (!isResident)
        {
            return (false, "Only residents can comment.");
        }

        var comment = new PostComment(Guid.NewGuid(), postId, userId, content, DateTime.UtcNow);
        await _feedRepository.AddCommentAsync(comment, cancellationToken);

        await _auditLogger.LogAsync(
            new Models.AuditEntry("comment.created", userId, territoryId, comment.Id, DateTime.UtcNow),
            cancellationToken);

        return (true, null);
    }

    public async Task<(bool success, string? error)> ShareAsync(
        Guid territoryId,
        Guid postId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var post = await _feedRepository.GetPostAsync(postId, cancellationToken);
        if (post is null || post.TerritoryId != territoryId)
        {
            return (false, "Post not found.");
        }

        var isResident = await _accessEvaluator.IsResidentAsync(userId, territoryId, cancellationToken);
        if (!isResident)
        {
            return (false, "Only residents can share.");
        }

        await _feedRepository.AddShareAsync(postId, userId, cancellationToken);

        await _auditLogger.LogAsync(
            new Models.AuditEntry("post.shared", userId, territoryId, postId, DateTime.UtcNow),
            cancellationToken);

        return (true, null);
    }

    public Task<int> GetLikeCountAsync(Guid postId, CancellationToken cancellationToken)
    {
        return _feedRepository.GetLikeCountAsync(postId, cancellationToken);
    }

    public Task<int> GetShareCountAsync(Guid postId, CancellationToken cancellationToken)
    {
        return _feedRepository.GetShareCountAsync(postId, cancellationToken);
    }
}
