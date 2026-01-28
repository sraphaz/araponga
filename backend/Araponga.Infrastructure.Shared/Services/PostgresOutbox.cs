using Araponga.Application.Interfaces;
using Araponga.Application.Models;
using Araponga.Infrastructure.Shared.Postgres;
using Araponga.Infrastructure.Shared.Postgres.Entities;

namespace Araponga.Infrastructure.Shared.Services;

public sealed class PostgresOutbox : IOutbox
{
    private readonly SharedDbContext _dbContext;

    public PostgresOutbox(SharedDbContext dbContext)
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
