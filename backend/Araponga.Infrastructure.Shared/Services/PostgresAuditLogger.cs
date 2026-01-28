using Araponga.Application.Interfaces;
using Araponga.Application.Models;
using Araponga.Infrastructure.Shared.Postgres;
using Araponga.Infrastructure.Shared.Postgres.Entities;

namespace Araponga.Infrastructure.Shared.Services;

public sealed class PostgresAuditLogger : IAuditLogger
{
    private readonly SharedDbContext _dbContext;

    public PostgresAuditLogger(SharedDbContext dbContext)
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
