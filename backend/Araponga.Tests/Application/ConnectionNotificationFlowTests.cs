using System.Text.Json;
using Araponga.Application.Events;
using Araponga.Application.Interfaces;
using Araponga.Application.Models;
using Araponga.Application.Services.Connections;
using Araponga.Domain.Connections;
using Araponga.Infrastructure.Eventing;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Araponga.Tests.Application;

/// <summary>
/// Testes de integração do fluxo de notificações de conexões:
/// RequestConnection → ConnectionRequestedEvent → notification.dispatch (connection.request)
/// AcceptConnection → ConnectionAcceptedEvent → notification.dispatch (connection.accepted)
/// </summary>
public sealed class ConnectionNotificationFlowTests
{
    [Fact]
    public async Task RequestConnectionAsync_EnqueuesConnectionRequestNotification()
    {
        var dataStore = new InMemoryDataStore();
        var serviceProvider = BuildServiceProvider(dataStore);
        var eventBus = new InMemoryEventBus(serviceProvider);
        var connectionService = CreateConnectionService(dataStore, eventBus);

        var requesterId = TestIds.TestUserId1;
        var targetId = TestIds.TestUserId2FA;
        EnsureUsersExist(dataStore, requesterId, targetId);

        var result = await connectionService.RequestConnectionAsync(
            requesterId,
            targetId,
            null,
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Single(dataStore.OutboxMessages);

        var message = dataStore.OutboxMessages[0];
        Assert.Equal("notification.dispatch", message.Type);

        var payload = JsonSerializer.Deserialize<NotificationDispatchPayload>(
            message.PayloadJson,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));
        Assert.NotNull(payload);
        Assert.Equal("connection.request", payload!.Kind);
        Assert.Single(payload.Recipients);
        Assert.Equal(targetId, payload.Recipients.First());
        Assert.Equal("Nova solicitação de conexão", payload.Title);
        Assert.True(payload.Data?.ContainsKey("connectionId") == true);
        Assert.True(payload.Data?.ContainsKey("requesterUserId") == true);
    }

    [Fact]
    public async Task AcceptConnectionAsync_EnqueuesConnectionAcceptedNotification()
    {
        var dataStore = new InMemoryDataStore();
        var serviceProvider = BuildServiceProvider(dataStore);
        var eventBus = new InMemoryEventBus(serviceProvider);
        var connectionService = CreateConnectionService(dataStore, eventBus);

        var requesterId = TestIds.TestUserId1;
        var targetId = TestIds.TestUserId2FA;
        EnsureUsersExist(dataStore, requesterId, targetId);

        var requestResult = await connectionService.RequestConnectionAsync(
            requesterId,
            targetId,
            null,
            CancellationToken.None);
        Assert.True(requestResult.IsSuccess);
        var connectionId = requestResult.Value!.Id;

        dataStore.OutboxMessages.Clear();

        var acceptResult = await connectionService.AcceptConnectionAsync(
            connectionId,
            targetId,
            CancellationToken.None);

        Assert.True(acceptResult.IsSuccess);
        Assert.Single(dataStore.OutboxMessages);

        var message = dataStore.OutboxMessages[0];
        Assert.Equal("notification.dispatch", message.Type);

        var payload = JsonSerializer.Deserialize<NotificationDispatchPayload>(
            message.PayloadJson,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));
        Assert.NotNull(payload);
        Assert.Equal("connection.accepted", payload!.Kind);
        Assert.Single(payload.Recipients);
        Assert.Equal(requesterId, payload.Recipients.First());
        Assert.Equal("Solicitação de conexão aceita", payload.Title);
        Assert.True(payload.Data?.ContainsKey("connectionId") == true);
        Assert.True(payload.Data?.ContainsKey("acceptorUserId") == true);
    }

    private static void EnsureUsersExist(InMemoryDataStore dataStore, Guid user1, Guid user2)
    {
        if (dataStore.Users.All(u => u.Id != user1))
        {
            dataStore.Users.Add(new Araponga.Domain.Users.User(
                user1,
                "User One",
                "u1@test.com",
                "123.456.789-01",
                null,
                null,
                null,
                "google",
                "ext-1",
                DateTime.UtcNow));
        }
        if (dataStore.Users.All(u => u.Id != user2))
        {
            dataStore.Users.Add(new Araponga.Domain.Users.User(
                user2,
                "User Two",
                "u2@test.com",
                "123.456.789-02",
                null,
                null,
                null,
                "google",
                "ext-2",
                DateTime.UtcNow));
        }
    }

    private static ServiceProvider BuildServiceProvider(InMemoryDataStore dataStore)
    {
        var services = new ServiceCollection();
        services.AddSingleton(dataStore);
        services.AddSingleton<IOutbox, InMemoryOutbox>();
        services.AddSingleton<IEventHandler<ConnectionRequestedEvent>, ConnectionRequestedNotificationHandler>();
        services.AddSingleton<IEventHandler<ConnectionAcceptedEvent>, ConnectionAcceptedNotificationHandler>();
        return services.BuildServiceProvider();
    }

    private static ConnectionService CreateConnectionService(
        InMemoryDataStore dataStore,
        InMemoryEventBus eventBus)
    {
        var connectionRepo = new InMemoryUserConnectionRepository(dataStore);
        var privacyRepo = new InMemoryConnectionPrivacySettingsRepository(dataStore);
        var blockRepo = new InMemoryUserBlockRepository(dataStore);
        var userRepo = new InMemoryUserRepository(dataStore);
        var membershipRepo = new InMemoryTerritoryMembershipRepository(dataStore);
        var unitOfWork = new InMemoryUnitOfWork();
        return new ConnectionService(
            connectionRepo,
            privacyRepo,
            blockRepo,
            userRepo,
            membershipRepo,
            unitOfWork,
            eventBus);
    }
}
