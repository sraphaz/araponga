namespace Araponga.Modules.Feed.Application.Common;

public sealed class PagedResult<T>
{
    private const int MaxInt32 = int.MaxValue;

    public PagedResult(
        IReadOnlyList<T> items,
        int pageNumber,
        int pageSize,
        int totalCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        
        TotalCount = totalCount > MaxInt32 ? MaxInt32 : totalCount;
        
        if (pageSize <= 0)
        {
            TotalPages = 0;
        }
        else if (TotalCount == 0)
        {
            TotalPages = 0;
        }
        else
        {
            var pages = (long)TotalCount / (long)pageSize;
            var remainder = (long)TotalCount % (long)pageSize;
            TotalPages = (int)Math.Min(pages + (remainder > 0 ? 1 : 0), MaxInt32);
        }

        HasPreviousPage = PageNumber > 1;
        HasNextPage = PageNumber < TotalPages;
    }

    public IReadOnlyList<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }
    public bool HasPreviousPage { get; }
    public bool HasNextPage { get; }
}
