using Araponga.Application.Interfaces;
using Araponga.Domain.Moderation;
using Araponga.Modules.Moderation.Infrastructure.Postgres;
using Araponga.Modules.Moderation.Infrastructure.Postgres.Entities;
using Microsoft.EntityFrameworkCore;

namespace Araponga.Modules.Moderation.Infrastructure.Repositories;

public sealed class PostgresSanctionRepository : ISanctionRepository
{
    private readonly ModerationDbContext _dbContext;

    public PostgresSanctionRepository(ModerationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(Sanction sanction, CancellationToken cancellationToken)
    {
        _dbContext.Sanctions.Add(sanction.ToRecord());
        return Task.CompletedTask;
    }

    public async Task<IReadOnlyList<Sanction>> ListActiveForTargetAsync(
        Guid targetId,
        DateTime referenceUtc,
        CancellationToken cancellationToken)
    {
        var records = await _dbContext.Sanctions
            .AsNoTracking()
            .Where(sanction =>
                sanction.TargetId == targetId &&
                sanction.Status == SanctionStatus.Active &&
                sanction.StartAtUtc <= referenceUtc &&
                (sanction.EndAtUtc == null || sanction.EndAtUtc >= referenceUtc))
            .ToListAsync(cancellationToken);

        return records.Select(record => record.ToDomain()).ToList();
    }

    public async Task<bool> HasActiveSanctionAsync(
        Guid targetId,
        Guid territoryId,
        SanctionType type,
        DateTime referenceUtc,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Sanctions
            .AsNoTracking()
            .AnyAsync(sanction =>
                sanction.TargetId == targetId &&
                sanction.Type == type &&
                sanction.Status == SanctionStatus.Active &&
                sanction.StartAtUtc <= referenceUtc &&
                (sanction.EndAtUtc == null || sanction.EndAtUtc >= referenceUtc) &&
                (sanction.Scope == SanctionScope.Global ||
                 (sanction.Scope == SanctionScope.Territory && sanction.TerritoryId == territoryId)),
                cancellationToken);
    }
}
