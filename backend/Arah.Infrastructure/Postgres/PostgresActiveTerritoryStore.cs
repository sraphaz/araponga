using Arah.Application.Interfaces;
using Arah.Infrastructure.Postgres.Entities;
using Microsoft.EntityFrameworkCore;

namespace Arah.Infrastructure.Postgres;

public sealed class PostgresActiveTerritoryStore : IActiveTerritoryStore
{
    private readonly ArahDbContext _dbContext;

    public PostgresActiveTerritoryStore(ArahDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid?> GetAsync(string sessionId, CancellationToken cancellationToken)
    {
        return await _dbContext.ActiveTerritories
            .AsNoTracking()
            .Where(entry => entry.SessionId == sessionId)
            .Select(entry => (Guid?)entry.TerritoryId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task SetAsync(string sessionId, Guid territoryId, CancellationToken cancellationToken)
    {
        var existing = await _dbContext.ActiveTerritories
            .FirstOrDefaultAsync(entry => entry.SessionId == sessionId, cancellationToken);

        if (existing is null)
        {
            _dbContext.ActiveTerritories.Add(new ActiveTerritoryRecord
            {
                SessionId = sessionId,
                TerritoryId = territoryId,
                UpdatedAtUtc = DateTime.UtcNow
            });
        }
        else
        {
            existing.TerritoryId = territoryId;
            existing.UpdatedAtUtc = DateTime.UtcNow;
            _dbContext.ActiveTerritories.Update(existing);
        }

    }
}
