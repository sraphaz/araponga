namespace Araponga.Modules.Feed.Application.Common;

public sealed class PaginationParameters
{
    public PaginationParameters(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentException("Page number must be greater than 0.", nameof(pageNumber));
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new ArgumentException("Page size must be between 1 and 100.", nameof(pageSize));
        }

        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public int PageNumber { get; }
    public int PageSize { get; }
    public int Skip => (PageNumber - 1) * PageSize;
    public int Take => PageSize;
}
