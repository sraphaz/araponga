using Araponga.Application.Models;
using Araponga.Modules.Notifications.Infrastructure.Postgres.Entities;

namespace Araponga.Modules.Notifications.Infrastructure.Postgres;

public static class NotificationsMappers
{
    public static UserNotificationRecord ToRecord(this UserNotification notification)
    {
        return new UserNotificationRecord
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Title = notification.Title,
            Body = notification.Body,
            Kind = notification.Kind,
            DataJson = notification.DataJson,
            CreatedAtUtc = notification.CreatedAtUtc,
            ReadAtUtc = notification.ReadAtUtc,
            SourceOutboxId = notification.SourceOutboxId
        };
    }

    public static UserNotification ToDomain(this UserNotificationRecord record)
    {
        return new UserNotification(
            record.Id,
            record.UserId,
            record.Title,
            record.Body,
            record.Kind,
            record.DataJson,
            record.CreatedAtUtc,
            record.ReadAtUtc,
            record.SourceOutboxId);
    }
}
