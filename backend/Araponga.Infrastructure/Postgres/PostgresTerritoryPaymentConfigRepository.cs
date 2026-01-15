using Araponga.Application.Interfaces;
using Araponga.Domain.Marketplace;
using Microsoft.EntityFrameworkCore;

namespace Araponga.Infrastructure.Postgres;

public sealed class PostgresTerritoryPaymentConfigRepository : ITerritoryPaymentConfigRepository
{
    private readonly ArapongaDbContext _dbContext;

    public PostgresTerritoryPaymentConfigRepository(ArapongaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TerritoryPaymentConfig?> GetActiveAsync(Guid territoryId, CancellationToken cancellationToken)
    {
        var record = await _dbContext.TerritoryPaymentConfigs
            .AsNoTracking()
            .FirstOrDefaultAsync(
                c => c.TerritoryId == territoryId && c.IsActive,
                cancellationToken);

        return record?.ToDomain();
    }

    public async Task<TerritoryPaymentConfig?> GetByIdAsync(Guid configId, CancellationToken cancellationToken)
    {
        var record = await _dbContext.TerritoryPaymentConfigs
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == configId, cancellationToken);

        return record?.ToDomain();
    }

    public async Task<IReadOnlyList<TerritoryPaymentConfig>> ListByTerritoryAsync(
        Guid territoryId,
        CancellationToken cancellationToken)
    {
        var records = await _dbContext.TerritoryPaymentConfigs
            .AsNoTracking()
            .Where(c => c.TerritoryId == territoryId)
            .OrderByDescending(c => c.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return records.Select(r => r.ToDomain()).ToList();
    }

    public Task AddAsync(TerritoryPaymentConfig config, CancellationToken cancellationToken)
    {
        _dbContext.TerritoryPaymentConfigs.Add(config.ToRecord());
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TerritoryPaymentConfig config, CancellationToken cancellationToken)
    {
        _dbContext.TerritoryPaymentConfigs.Update(config.ToRecord());
        return Task.CompletedTask;
    }
}
