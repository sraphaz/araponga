using Araponga.Application.Common;
using Araponga.Modules.Feed.Application.Common;
using Araponga.Modules.Feed.Application.Models;
using Araponga.Modules.Feed.Domain;
using OperationResult = Araponga.Application.Common.OperationResult;

namespace Araponga.Modules.Feed.Application.Interfaces;

/// <summary>
/// Interface pública do serviço de Feed.
/// Expõe apenas os métodos necessários para outros módulos e API.
/// </summary>
public interface IFeedService
{
    Task<IReadOnlyList<Post>> ListForTerritoryAsync(
        Guid territoryId,
        Guid? userId,
        Guid? mapEntityId,
        Guid? assetId,
        bool filterByInterests = false,
        CancellationToken cancellationToken = default);

    Task<PagedResult<Post>> ListForTerritoryPagedAsync(
        Guid territoryId,
        Guid? userId,
        Guid? mapEntityId,
        Guid? assetId,
        PaginationParameters pagination,
        bool filterByInterests = false,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Post>> ListForUserAsync(
        Guid userId,
        CancellationToken cancellationToken);

    Task<PagedResult<Post>> ListForUserPagedAsync(
        Guid userId,
        PaginationParameters pagination,
        CancellationToken cancellationToken);

    Task<Result<Post>> CreatePostAsync(
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
        IReadOnlyCollection<string>? tags = null);

    Task<bool> LikeAsync(
        Guid territoryId,
        Guid postId,
        string actorId,
        Guid? userId,
        CancellationToken cancellationToken);

    Task<OperationResult> CommentAsync(
        Guid territoryId,
        Guid postId,
        Guid userId,
        string content,
        CancellationToken cancellationToken);

    Task<OperationResult> ShareAsync(
        Guid territoryId,
        Guid postId,
        Guid userId,
        CancellationToken cancellationToken);

    Task<int> GetLikeCountAsync(Guid postId, CancellationToken cancellationToken);

    Task<int> GetShareCountAsync(Guid postId, CancellationToken cancellationToken);

    Task<IReadOnlyDictionary<Guid, PostCounts>> GetCountsByPostIdsAsync(
        IReadOnlyCollection<Guid> postIds,
        CancellationToken cancellationToken);

    Task<Post?> GetPostAsync(Guid postId, CancellationToken cancellationToken);

    Task DeletePostAsync(Guid postId, CancellationToken cancellationToken);
}
