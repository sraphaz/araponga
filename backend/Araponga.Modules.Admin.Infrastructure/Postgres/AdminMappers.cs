using Araponga.Domain.Evidence;
using Araponga.Domain.Work;
using Araponga.Modules.Admin.Infrastructure.Postgres.Entities;

namespace Araponga.Modules.Admin.Infrastructure.Postgres;

public static class AdminMappers
{
    public static WorkItemRecord ToRecord(this WorkItem item)
    {
        return new WorkItemRecord
        {
            Id = item.Id,
            Type = item.Type,
            Status = item.Status,
            TerritoryId = item.TerritoryId,
            CreatedByUserId = item.CreatedByUserId,
            CreatedAtUtc = item.CreatedAtUtc,
            RequiredSystemPermission = item.RequiredSystemPermission,
            RequiredCapability = item.RequiredCapability,
            SubjectType = item.SubjectType,
            SubjectId = item.SubjectId,
            PayloadJson = item.PayloadJson,
            Outcome = item.Outcome,
            CompletedAtUtc = item.CompletedAtUtc,
            CompletedByUserId = item.CompletedByUserId,
            CompletionNotes = item.CompletionNotes
        };
    }

    public static WorkItem ToDomain(this WorkItemRecord record)
    {
        return new WorkItem(
            record.Id,
            record.Type,
            record.Status,
            record.TerritoryId,
            record.CreatedByUserId,
            record.CreatedAtUtc,
            record.RequiredSystemPermission,
            record.RequiredCapability,
            record.SubjectType,
            record.SubjectId,
            record.PayloadJson,
            record.Outcome,
            record.CompletedAtUtc,
            record.CompletedByUserId,
            record.CompletionNotes);
    }

    public static DocumentEvidenceRecord ToRecord(this DocumentEvidence evidence)
    {
        return new DocumentEvidenceRecord
        {
            Id = evidence.Id,
            UserId = evidence.UserId,
            TerritoryId = evidence.TerritoryId,
            Kind = evidence.Kind,
            StorageProvider = evidence.StorageProvider,
            StorageKey = evidence.StorageKey,
            ContentType = evidence.ContentType,
            SizeBytes = evidence.SizeBytes,
            Sha256 = evidence.Sha256,
            OriginalFileName = evidence.OriginalFileName,
            CreatedAtUtc = evidence.CreatedAtUtc
        };
    }

    public static DocumentEvidence ToDomain(this DocumentEvidenceRecord record)
    {
        return new DocumentEvidence(
            record.Id,
            record.UserId,
            record.TerritoryId,
            record.Kind,
            record.StorageProvider,
            record.StorageKey,
            record.ContentType,
            record.SizeBytes,
            record.Sha256,
            record.OriginalFileName,
            record.CreatedAtUtc);
    }
}
