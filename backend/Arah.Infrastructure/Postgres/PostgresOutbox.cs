using Arah.Application.Interfaces;
using Arah.Application.Models;
using Arah.Infrastructure.Postgres.Entities;

namespace Arah.Infrastructure.Postgres;

public sealed class PostgresOutbox : IOutbox
{
    private readonly ArahDbContext _dbContext;

    public PostgresOutbox(ArahDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task EnqueueAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        _dbContext.OutboxMessages.Add(new OutboxMessageRecord
        {
            Id = message.Id,
            Type = message.Type,
            PayloadJson = message.PayloadJson,
            OccurredAtUtc = message.OccurredAtUtc,
            ProcessedAtUtc = message.ProcessedAtUtc,
            Attempts = message.Attempts,
            LastError = message.LastError,
            ProcessAfterUtc = message.ProcessAfterUtc
        });

        return Task.CompletedTask;
    }
}
