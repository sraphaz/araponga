using Arah.Application.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Arah.Infrastructure.Eventing;

public sealed class InMemoryEventBus : IEventBus
{
    private readonly IServiceProvider _serviceProvider;

    public InMemoryEventBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<TEvent>(TEvent appEvent, CancellationToken cancellationToken)
        where TEvent : IAppEvent
    {
        var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();
        foreach (var handler in handlers)
        {
            await handler.HandleAsync(appEvent, cancellationToken);
        }
    }
}
