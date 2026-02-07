using Arah.Domain.Events;

namespace Arah.Application.Models;

public sealed record EventParticipant(
    Guid UserId,
    string DisplayName,
    EventParticipationStatus Status,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
