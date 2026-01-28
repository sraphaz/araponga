using Araponga.Application.Interfaces;
using Araponga.Domain.Moderation;
using Araponga.Modules.Moderation.Infrastructure.Postgres;
using Araponga.Modules.Moderation.Infrastructure.Postgres.Entities;
using Microsoft.EntityFrameworkCore;

namespace Araponga.Modules.Moderation.Infrastructure.Repositories;

public sealed class PostgresUserBlockRepository : IUserBlockRepository
{
    private readonly ModerationDbContext _dbContext;

    public PostgresUserBlockRepository(ModerationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistsAsync(Guid blockerUserId, Guid blockedUserId, CancellationToken cancellationToken)
    {
        return await _dbContext.UserBlocks
            .AsNoTracking()
            .AnyAsync(block =>
                block.BlockerUserId == blockerUserId &&
                block.BlockedUserId == blockedUserId,
                cancellationToken);
    }

    public Task AddAsync(UserBlock block, CancellationToken cancellationToken)
    {
        _dbContext.UserBlocks.Add(block.ToRecord());
        return Task.CompletedTask;
    }

    public async Task RemoveAsync(Guid blockerUserId, Guid blockedUserId, CancellationToken cancellationToken)
    {
        var record = await _dbContext.UserBlocks
            .FirstOrDefaultAsync(block =>
                block.BlockerUserId == blockerUserId &&
                block.BlockedUserId == blockedUserId,
                cancellationToken);

        if (record is null)
        {
            return;
        }

        _dbContext.UserBlocks.Remove(record);
    }

    public async Task<IReadOnlyCollection<Guid>> GetBlockedUserIdsAsync(
        Guid blockerUserId,
        CancellationToken cancellationToken)
    {
        var blocked = await _dbContext.UserBlocks
            .AsNoTracking()
            .Where(block => block.BlockerUserId == blockerUserId)
            .Select(block => block.BlockedUserId)
            .ToListAsync(cancellationToken);

        return blocked.AsReadOnly();
    }
}
