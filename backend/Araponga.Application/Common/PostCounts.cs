namespace Araponga.Application.Common;

public sealed record PostCounts
{
    public int LikeCount { get; init; }
    public int ShareCount { get; init; }

    public PostCounts(int likeCount, int shareCount)
    {
        LikeCount = likeCount > int.MaxValue ? int.MaxValue : likeCount;
        ShareCount = shareCount > int.MaxValue ? int.MaxValue : shareCount;
    }
}
