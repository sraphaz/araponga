using Araponga.Application.Interfaces;
using Araponga.Domain.Marketplace;

namespace Araponga.Infrastructure.InMemory;

public sealed class InMemoryInquiryRepository : IInquiryRepository
{
    private readonly InMemoryDataStore _dataStore;

    public InMemoryInquiryRepository(InMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task AddAsync(ListingInquiry inquiry, CancellationToken cancellationToken)
    {
        _dataStore.ListingInquiries.Add(inquiry);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<ListingInquiry>> ListByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var inquiries = _dataStore.ListingInquiries
            .Where(i => i.FromUserId == userId)
            .OrderByDescending(i => i.CreatedAtUtc)
            .ToList();

        return Task.FromResult<IReadOnlyList<ListingInquiry>>(inquiries);
    }

    public Task<IReadOnlyList<ListingInquiry>> ListByStoreIdsAsync(IReadOnlyCollection<Guid> storeIds, CancellationToken cancellationToken)
    {
        if (storeIds.Count == 0)
        {
            return Task.FromResult<IReadOnlyList<ListingInquiry>>(Array.Empty<ListingInquiry>());
        }

        var inquiries = _dataStore.ListingInquiries
            .Where(i => storeIds.Contains(i.StoreId))
            .OrderByDescending(i => i.CreatedAtUtc)
            .ToList();

        return Task.FromResult<IReadOnlyList<ListingInquiry>>(inquiries);
    }

    public Task<IReadOnlyList<ListingInquiry>> ListByUserPagedAsync(
        Guid userId,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        var inquiries = _dataStore.ListingInquiries
            .Where(i => i.FromUserId == userId)
            .OrderByDescending(i => i.CreatedAtUtc)
            .Skip(skip)
            .Take(take)
            .ToList();

        return Task.FromResult<IReadOnlyList<ListingInquiry>>(inquiries);
    }

    public Task<IReadOnlyList<ListingInquiry>> ListByStoreIdsPagedAsync(
        IReadOnlyCollection<Guid> storeIds,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        if (storeIds.Count == 0)
        {
            return Task.FromResult<IReadOnlyList<ListingInquiry>>(Array.Empty<ListingInquiry>());
        }

        var inquiries = _dataStore.ListingInquiries
            .Where(i => storeIds.Contains(i.StoreId))
            .OrderByDescending(i => i.CreatedAtUtc)
            .Skip(skip)
            .Take(take)
            .ToList();

        return Task.FromResult<IReadOnlyList<ListingInquiry>>(inquiries);
    }

    public Task<int> CountByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var count = _dataStore.ListingInquiries.Count(i => i.FromUserId == userId);
        return Task.FromResult(count);
    }

    public Task<int> CountByStoreIdsAsync(IReadOnlyCollection<Guid> storeIds, CancellationToken cancellationToken)
    {
        if (storeIds.Count == 0)
        {
            return Task.FromResult(0);
        }

        var count = _dataStore.ListingInquiries.Count(i => storeIds.Contains(i.StoreId));
        return Task.FromResult(count);
    }
}
