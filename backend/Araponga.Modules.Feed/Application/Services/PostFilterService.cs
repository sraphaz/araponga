using Araponga.Application.Interfaces;
using Araponga.Domain.Feed;
using Araponga.Domain.Membership;
using Araponga.Modules.Feed.Application.Adapters;
using Araponga.Modules.Feed.Application.Common;
using Araponga.Modules.Feed.Domain;

namespace Araponga.Modules.Feed.Application.Services;

/// <summary>
/// Service responsible for filtering posts based on visibility and access rules in the Feed module.
/// </summary>
public sealed class PostFilterService
{
    private readonly AccessEvaluator _accessEvaluator;
    private readonly UserBlockCacheService? _userBlockCache;
    private readonly IUserBlockRepository _userBlockRepository;
    private readonly IPostAssetRepository _postAssetRepository;

    public PostFilterService(
        AccessEvaluator accessEvaluator,
        IUserBlockRepository userBlockRepository,
        IPostAssetRepository postAssetRepository,
        UserBlockCacheService? userBlockCache = null)
    {
        _accessEvaluator = accessEvaluator;
        _userBlockRepository = userBlockRepository;
        _postAssetRepository = postAssetRepository;
        _userBlockCache = userBlockCache;
    }

    public async Task<IReadOnlyList<Post>> FilterPostsAsync(
        IReadOnlyList<Post> posts,
        Guid territoryId,
        Guid? userId,
        Guid? mapEntityId,
        Guid? assetId,
        CancellationToken cancellationToken)
    {
        // Converter para CommunityPost temporariamente para usar lógica existente
        var communityPosts = posts.Select(p => new CommunityPost(
            p.Id,
            p.TerritoryId,
            p.AuthorUserId,
            p.Title,
            p.Content,
            (Araponga.Domain.Feed.PostType)(int)p.Type,
            (Araponga.Domain.Feed.PostVisibility)(int)p.Visibility,
            (Araponga.Domain.Feed.PostStatus)(int)p.Status,
            p.MapEntityId,
            p.CreatedAtUtc,
            p.ReferenceType,
            p.ReferenceId,
            p.EditedAtUtc,
            p.EditCount,
            p.Tags)).ToList();

        var blockedUserIds = userId is null
            ? Array.Empty<Guid>()
            : _userBlockCache is not null
                ? await _userBlockCache.GetBlockedUserIdsAsync(userId.Value, cancellationToken)
                : await _userBlockRepository.GetBlockedUserIdsAsync(userId.Value, cancellationToken);

        var visiblePosts = blockedUserIds.Count == 0
            ? communityPosts
            : communityPosts.Where(post => !blockedUserIds.Contains(post.AuthorUserId)).ToList();

        if (userId is null)
        {
            var publicPosts = visiblePosts
                .Where(post => post.Visibility == Araponga.Domain.Feed.PostVisibility.Public && post.Status == Araponga.Domain.Feed.PostStatus.Published)
                .ToList();
            var filtered = await ApplyFilters(publicPosts, mapEntityId, assetId, cancellationToken);
            return filtered.Select(PostAdapter.ToPost).ToList();
        }

        var isResident = await _accessEvaluator.IsResidentAsync(userId.Value, territoryId, cancellationToken);

        var statusFiltered = isResident
            ? visiblePosts.Where(post => post.Status != Araponga.Domain.Feed.PostStatus.Rejected && post.Status != Araponga.Domain.Feed.PostStatus.Hidden).ToList()
            : visiblePosts.Where(post => post.Visibility == Araponga.Domain.Feed.PostVisibility.Public && post.Status == Araponga.Domain.Feed.PostStatus.Published).ToList();

        var filtered = await ApplyFilters(statusFiltered, mapEntityId, assetId, cancellationToken);
        return filtered.Select(PostAdapter.ToPost).ToList();
    }

    private async Task<IReadOnlyList<CommunityPost>> ApplyFilters(
        IReadOnlyList<CommunityPost> posts,
        Guid? mapEntityId,
        Guid? assetId,
        CancellationToken cancellationToken)
    {
        var scoped = mapEntityId is null
            ? posts
            : posts.Where(post => post.MapEntityId == mapEntityId).ToList();

        if (assetId is null)
        {
            return scoped;
        }

        var postIds = await _postAssetRepository.ListPostIdsByAssetIdAsync(assetId.Value, cancellationToken);
        if (postIds.Count == 0)
        {
            return Array.Empty<CommunityPost>();
        }

        var postLookup = postIds.ToHashSet();
        return scoped.Where(post => postLookup.Contains(post.Id)).ToList();
    }

    public async Task<PagedResult<Post>> FilterAndPaginateAsync(
        IReadOnlyList<Post> posts,
        Guid territoryId,
        Guid? userId,
        Guid? mapEntityId,
        Guid? assetId,
        PaginationParameters pagination,
        CancellationToken cancellationToken)
    {
        var filtered = await FilterPostsAsync(posts, territoryId, userId, mapEntityId, assetId, cancellationToken);

        const int maxInt32 = int.MaxValue;
        var count = filtered.Count;
        var totalCount = count > maxInt32 ? maxInt32 : count;
        var pagedItems = filtered
            .OrderByDescending(p => p.CreatedAtUtc)
            .Skip(pagination.Skip)
            .Take(pagination.Take)
            .ToList();

        return new PagedResult<Post>(pagedItems, pagination.PageNumber, pagination.PageSize, totalCount);
    }
}
