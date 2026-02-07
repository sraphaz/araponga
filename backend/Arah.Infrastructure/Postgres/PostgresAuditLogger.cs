using Arah.Application.Interfaces;
using Arah.Application.Models;
using Arah.Infrastructure.Postgres.Entities;

namespace Arah.Infrastructure.Postgres;

public sealed class PostgresAuditLogger : IAuditLogger
{
    private readonly ArahDbContext _dbContext;

    public PostgresAuditLogger(ArahDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task LogAsync(AuditEntry entry, CancellationToken cancellationToken)
    {
        _dbContext.AuditEntries.Add(new AuditEntryRecord
        {
            Id = Guid.NewGuid(),
            Action = entry.Action,
            ActorUserId = entry.ActorUserId,
            TerritoryId = entry.TerritoryId,
            TargetId = entry.TargetId,
            TimestampUtc = entry.TimestampUtc
        });

        return Task.CompletedTask;
    }
}
