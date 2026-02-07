using Arah.Application.Models;

namespace Arah.Application.Interfaces;

public interface INotificationInboxRepository
{
    Task AddAsync(UserNotification notification, CancellationToken cancellationToken);
    Task<IReadOnlyList<UserNotification>> ListByUserAsync(
        Guid userId,
        int skip,
        int take,
        CancellationToken cancellationToken);
    Task<int> CountByUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<bool> MarkAsReadAsync(Guid notificationId, Guid userId, DateTime readAtUtc, CancellationToken cancellationToken);
}
