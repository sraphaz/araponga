namespace Arah.Application.Events;

public interface IAppEvent
{
    DateTime OccurredAtUtc { get; }
}
