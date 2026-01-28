using Araponga.Application.Interfaces;
using Araponga.Domain.Feed;
using Araponga.Modules.Assets.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace Araponga.Modules.Assets.Infrastructure.Repositories;

public sealed class PostgresPostAssetRepository : IPostAssetRepository
{
    private readonly AssetsDbContext _dbContext;

    public PostgresPostAssetRepository(AssetsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(IReadOnlyCollection<PostAsset> postAssets, CancellationToken cancellationToken)
    {
        if (postAssets.Count == 0)
        {
            return Task.CompletedTask;
        }

        _dbContext.PostAssets.AddRange(postAssets.Select(postAsset => postAsset.ToRecord()));
        return Task.CompletedTask;
    }

    public async Task<IReadOnlyList<Guid>> ListPostIdsByAssetIdAsync(Guid assetId, CancellationToken cancellationToken)
    {
        return await _dbContext.PostAssets
            .AsNoTracking()
            .Where(postAsset => postAsset.AssetId == assetId)
            .Select(postAsset => postAsset.PostId)
            .Distinct()
            .ToListAsync(cancellationToken);
    }
}
