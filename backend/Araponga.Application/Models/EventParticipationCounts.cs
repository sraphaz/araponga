namespace Araponga.Application.Models;

public sealed record EventParticipationCounts
{
    public Guid EventId { get; init; }
    public int InterestedCount { get; init; }
    public int ConfirmedCount { get; init; }

    public EventParticipationCounts(Guid eventId, int interestedCount, int confirmedCount)
    {
        EventId = eventId;
        InterestedCount = interestedCount > int.MaxValue ? int.MaxValue : interestedCount;
        ConfirmedCount = confirmedCount > int.MaxValue ? int.MaxValue : confirmedCount;
    }
}
