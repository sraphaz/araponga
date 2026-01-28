using Araponga.Domain.Moderation;
using Araponga.Modules.Moderation.Infrastructure.Postgres.Entities;

namespace Araponga.Modules.Moderation.Infrastructure.Postgres;

public static class ModerationMappers
{
    public static ModerationReportRecord ToRecord(this ModerationReport report)
    {
        return new ModerationReportRecord
        {
            Id = report.Id,
            ReporterUserId = report.ReporterUserId,
            TerritoryId = report.TerritoryId,
            TargetType = report.TargetType,
            TargetId = report.TargetId,
            Reason = report.Reason,
            Details = report.Details,
            Status = report.Status,
            CreatedAtUtc = report.CreatedAtUtc
        };
    }

    public static ModerationReport ToDomain(this ModerationReportRecord record)
    {
        return new ModerationReport(
            record.Id,
            record.ReporterUserId,
            record.TerritoryId,
            record.TargetType,
            record.TargetId,
            record.Reason,
            record.Details,
            record.Status,
            record.CreatedAtUtc);
    }

    public static UserBlockRecord ToRecord(this UserBlock block)
    {
        return new UserBlockRecord
        {
            BlockerUserId = block.BlockerUserId,
            BlockedUserId = block.BlockedUserId,
            CreatedAtUtc = block.CreatedAtUtc
        };
    }

    public static UserBlock ToDomain(this UserBlockRecord record)
    {
        return new UserBlock(
            record.BlockerUserId,
            record.BlockedUserId,
            record.CreatedAtUtc);
    }

    public static SanctionRecord ToRecord(this Sanction sanction)
    {
        return new SanctionRecord
        {
            Id = sanction.Id,
            TerritoryId = sanction.TerritoryId,
            Scope = sanction.Scope,
            TargetType = sanction.TargetType,
            TargetId = sanction.TargetId,
            Type = sanction.Type,
            Reason = sanction.Reason,
            Status = sanction.Status,
            StartAtUtc = sanction.StartAtUtc,
            EndAtUtc = sanction.EndAtUtc,
            CreatedAtUtc = sanction.CreatedAtUtc
        };
    }

    public static Sanction ToDomain(this SanctionRecord record)
    {
        return new Sanction(
            record.Id,
            record.TerritoryId,
            record.Scope,
            record.TargetType,
            record.TargetId,
            record.Type,
            record.Reason,
            record.Status,
            record.StartAtUtc,
            record.EndAtUtc,
            record.CreatedAtUtc);
    }
}
