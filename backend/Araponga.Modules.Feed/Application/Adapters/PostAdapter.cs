using Araponga.Domain.Feed;
using Araponga.Modules.Feed.Domain;

namespace Araponga.Modules.Feed.Application.Adapters;

/// <summary>
/// Adapter temporário para converter entre CommunityPost (antigo) e Post (novo).
/// TODO: Remover após migração completa.
/// </summary>
internal static class PostAdapter
{
    public static Post ToPost(CommunityPost communityPost)
    {
        return new Post(
            communityPost.Id,
            communityPost.TerritoryId,
            communityPost.AuthorUserId,
            communityPost.Title,
            communityPost.Content,
            (PostType)(int)communityPost.Type,
            (PostVisibility)(int)communityPost.Visibility,
            (PostStatus)(int)communityPost.Status,
            communityPost.MapEntityId,
            communityPost.CreatedAtUtc,
            communityPost.ReferenceType,
            communityPost.ReferenceId,
            communityPost.EditedAtUtc,
            communityPost.EditCount,
            communityPost.Tags);
    }

    public static IReadOnlyList<Post> ToPostList(IReadOnlyList<CommunityPost> communityPosts)
    {
        return communityPosts.Select(ToPost).ToList();
    }
}
