using Araponga.Application.Common;
using Araponga.Application.Interfaces;
using Araponga.Application.Services;
using Araponga.Domain.Feed;
using Araponga.Modules.Feed.Application.Adapters;
using Araponga.Modules.Feed.Application.Common;
using Araponga.Modules.Feed.Application.Interfaces;
using Araponga.Modules.Feed.Application.Models;
using Araponga.Modules.Feed.Domain;

namespace Araponga.Modules.Feed.Application.Services;

/// <summary>
/// Implementação do IFeedService usando services próprios do módulo.
/// </summary>
public sealed class FeedService : IFeedService
{
    private readonly IFeedRepository _feedRepository;
    private readonly PostCreationService _postCreationService;
    private readonly PostInteractionService _postInteractionService;
    private readonly PostFilterService _postFilterService;
    private readonly InterestFilterService? _interestFilterService;

    public FeedService(
        IFeedRepository feedRepository,
        PostCreationService postCreationService,
        PostInteractionService postInteractionService,
        PostFilterService postFilterService,
        InterestFilterService? interestFilterService = null)
    {
        _feedRepository = feedRepository;
        _postCreationService = postCreationService;
        _postInteractionService = postInteractionService;
        _postFilterService = postFilterService;
        _interestFilterService = interestFilterService;
    }

    public async Task<IReadOnlyList<Post>> ListForTerritoryAsync(
        Guid territoryId,
        Guid? userId,
        Guid? mapEntityId,
        Guid? assetId,
        bool filterByInterests = false,
        CancellationToken cancellationToken = default)
    {
        var communityPosts = await _feedRepository.ListByTerritoryAsync(territoryId, cancellationToken);
        var posts = PostAdapter.ToPostList(communityPosts);
        var filtered = await _postFilterService.FilterPostsAsync(posts, territoryId, userId, mapEntityId, assetId, cancellationToken);

        // Aplicar filtro de interesses se solicitado e se serviço disponível
        if (filterByInterests && _interestFilterService is not null && userId.HasValue)
        {
            var communityFiltered = filtered.Select(p => new CommunityPost(
                p.Id, p.TerritoryId, p.AuthorUserId, p.Title, p.Content,
                (Araponga.Domain.Feed.PostType)(int)p.Type,
                (Araponga.Domain.Feed.PostVisibility)(int)p.Visibility,
                (Araponga.Domain.Feed.PostStatus)(int)p.Status,
                p.MapEntityId, p.CreatedAtUtc, p.ReferenceType, p.ReferenceId,
                p.EditedAtUtc, p.EditCount, p.Tags)).ToList();
            
            var interestFiltered = await _interestFilterService.FilterFeedByInterestsAsync(
                communityFiltered,
                userId.Value,
                territoryId,
                cancellationToken);
            
            return PostAdapter.ToPostList(interestFiltered);
        }

        return filtered;
    }

    public async Task<PagedResult<Post>> ListForTerritoryPagedAsync(
        Guid territoryId,
        Guid? userId,
        Guid? mapEntityId,
        Guid? assetId,
        PaginationParameters pagination,
        bool filterByInterests = false,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await _feedRepository.CountByTerritoryAsync(territoryId, cancellationToken);
        var communityPosts = await _feedRepository.ListByTerritoryPagedAsync(territoryId, pagination.Skip, pagination.Take, cancellationToken);
        var posts = PostAdapter.ToPostList(communityPosts);
        var filtered = await _postFilterService.FilterPostsAsync(posts, territoryId, userId, mapEntityId, assetId, cancellationToken);

        // Aplicar filtro de interesses se solicitado e se serviço disponível
        if (filterByInterests && _interestFilterService is not null && userId.HasValue)
        {
            var communityFiltered = filtered.Select(p => new CommunityPost(
                p.Id, p.TerritoryId, p.AuthorUserId, p.Title, p.Content,
                (Araponga.Domain.Feed.PostType)(int)p.Type,
                (Araponga.Domain.Feed.PostVisibility)(int)p.Visibility,
                (Araponga.Domain.Feed.PostStatus)(int)p.Status,
                p.MapEntityId, p.CreatedAtUtc, p.ReferenceType, p.ReferenceId,
                p.EditedAtUtc, p.EditCount, p.Tags)).ToList();
            
            var interestFiltered = await _interestFilterService.FilterFeedByInterestsAsync(
                communityFiltered,
                userId.Value,
                territoryId,
                cancellationToken);
            
            filtered = PostAdapter.ToPostList(interestFiltered);
        }

        return new PagedResult<Post>(filtered, pagination.PageNumber, pagination.PageSize, totalCount);
    }

    public async Task<IReadOnlyList<Post>> ListForUserAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var communityPosts = await _feedRepository.ListByAuthorAsync(userId, cancellationToken);
        return PostAdapter.ToPostList(communityPosts);
    }

    public async Task<PagedResult<Post>> ListForUserPagedAsync(
        Guid userId,
        PaginationParameters pagination,
        CancellationToken cancellationToken)
    {
        var totalCount = await _feedRepository.CountByAuthorAsync(userId, cancellationToken);
        var communityPosts = await _feedRepository.ListByAuthorPagedAsync(userId, pagination.Skip, pagination.Take, cancellationToken);
        var posts = PostAdapter.ToPostList(communityPosts);

        return new PagedResult<Post>(posts, pagination.PageNumber, pagination.PageSize, totalCount);
    }

    public Task<Result<Post>> CreatePostAsync(
        Guid territoryId,
        Guid userId,
        string title,
        string content,
        PostType type,
        PostVisibility visibility,
        PostStatus status,
        Guid? mapEntityId,
        IReadOnlyCollection<GeoAnchorInput>? geoAnchors,
        IReadOnlyCollection<Guid>? assetIds,
        IReadOnlyCollection<Guid>? mediaIds,
        CancellationToken cancellationToken,
        IReadOnlyCollection<string>? tags = null)
    {
        return _postCreationService.CreatePostAsync(
            territoryId, userId, title, content, type, visibility, status,
            mapEntityId, geoAnchors, assetIds, mediaIds, cancellationToken, tags);
    }

    public Task<bool> LikeAsync(
        Guid territoryId,
        Guid postId,
        string actorId,
        Guid? userId,
        CancellationToken cancellationToken)
    {
        return _postInteractionService.LikeAsync(territoryId, postId, actorId, userId, cancellationToken);
    }

    public Task<OperationResult> CommentAsync(
        Guid territoryId,
        Guid postId,
        Guid userId,
        string content,
        CancellationToken cancellationToken)
    {
        return _postInteractionService.CommentAsync(territoryId, postId, userId, content, cancellationToken);
    }

    public Task<OperationResult> ShareAsync(
        Guid territoryId,
        Guid postId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        return _postInteractionService.ShareAsync(territoryId, postId, userId, cancellationToken);
    }

    public Task<int> GetLikeCountAsync(Guid postId, CancellationToken cancellationToken)
    {
        return _feedRepository.GetLikeCountAsync(postId, cancellationToken);
    }

    public Task<int> GetShareCountAsync(Guid postId, CancellationToken cancellationToken)
    {
        return _feedRepository.GetShareCountAsync(postId, cancellationToken);
    }

    public Task<IReadOnlyDictionary<Guid, PostCounts>> GetCountsByPostIdsAsync(
        IReadOnlyCollection<Guid> postIds,
        CancellationToken cancellationToken)
    {
        return _feedRepository.GetCountsByPostIdsAsync(postIds, cancellationToken)
            .ContinueWith(t => (IReadOnlyDictionary<Guid, PostCounts>)t.Result.ToDictionary(
                kvp => kvp.Key,
                kvp => new PostCounts(kvp.Value.LikeCount, kvp.Value.ShareCount)),
                cancellationToken);
    }

    public async Task<Post?> GetPostAsync(Guid postId, CancellationToken cancellationToken)
    {
        var communityPost = await _feedRepository.GetPostAsync(postId, cancellationToken);
        return communityPost is null ? null : PostAdapter.ToPost(communityPost);
    }

    public Task DeletePostAsync(Guid postId, CancellationToken cancellationToken)
    {
        return _feedRepository.DeletePostAsync(postId, cancellationToken);
    }
}
