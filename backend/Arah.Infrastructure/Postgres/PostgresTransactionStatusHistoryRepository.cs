using Arah.Application.Interfaces;
using Arah.Domain.Financial;
using Microsoft.EntityFrameworkCore;

namespace Arah.Infrastructure.Postgres;

public sealed class PostgresTransactionStatusHistoryRepository : ITransactionStatusHistoryRepository
{
    private readonly ArahDbContext _dbContext;

    public PostgresTransactionStatusHistoryRepository(ArahDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TransactionStatusHistory>> GetByTransactionIdAsync(Guid financialTransactionId, CancellationToken cancellationToken)
    {
        var records = await _dbContext.TransactionStatusHistories
            .Where(r => r.FinancialTransactionId == financialTransactionId)
            .OrderBy(r => r.ChangedAtUtc)
            .ToListAsync(cancellationToken);
        return records.Select(r => r.ToDomain()).ToList();
    }

    public Task AddAsync(TransactionStatusHistory history, CancellationToken cancellationToken)
    {
        _dbContext.TransactionStatusHistories.Add(history.ToRecord());
        return Task.CompletedTask;
    }
}
