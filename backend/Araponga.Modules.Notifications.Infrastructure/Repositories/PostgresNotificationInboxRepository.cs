using Araponga.Application.Interfaces;
using Araponga.Application.Models;
using Araponga.Modules.Notifications.Infrastructure.Postgres;
using Araponga.Modules.Notifications.Infrastructure.Postgres.Entities;
using Microsoft.EntityFrameworkCore;

namespace Araponga.Modules.Notifications.Infrastructure.Repositories;

public sealed class PostgresNotificationInboxRepository : INotificationInboxRepository
{
    private readonly NotificationsDbContext _dbContext;

    public PostgresNotificationInboxRepository(NotificationsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(UserNotification notification, CancellationToken cancellationToken)
    {
        _dbContext.UserNotifications.Add(notification.ToRecord());
        return Task.CompletedTask;
    }

    public async Task<IReadOnlyList<UserNotification>> ListByUserAsync(
        Guid userId,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        var records = await _dbContext.UserNotifications
            .AsNoTracking()
            .Where(notification => notification.UserId == userId)
            .OrderByDescending(notification => notification.CreatedAtUtc)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return records.Select(record => record.ToDomain()).ToList();
    }

    public async Task<int> CountByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        const int maxInt32 = int.MaxValue;
        var count = await _dbContext.UserNotifications
            .Where(notification => notification.UserId == userId)
            .CountAsync(cancellationToken);
        return count > maxInt32 ? maxInt32 : (int)count;
    }

    public async Task<bool> MarkAsReadAsync(
        Guid notificationId,
        Guid userId,
        DateTime readAtUtc,
        CancellationToken cancellationToken)
    {
        var record = await _dbContext.UserNotifications
            .FirstOrDefaultAsync(
                notification => notification.Id == notificationId && notification.UserId == userId,
                cancellationToken);

        if (record is null)
        {
            return false;
        }

        if (record.ReadAtUtc is null)
        {
            record.ReadAtUtc = readAtUtc;
        }

        return true;
    }
}
