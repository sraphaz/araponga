using Arah.Domain.Events;

namespace Arah.Application.Models;

public sealed record EventSummary(
    TerritoryEvent Event,
    int InterestedCount,
    int ConfirmedCount,
    string? CreatedByDisplayName);
