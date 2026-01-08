namespace Araponga.Application.Models;

public sealed record AuditEntry(
    string Action,
    Guid ActorUserId,
    Guid TerritoryId,
    Guid TargetId,
    DateTime TimestampUtc);
