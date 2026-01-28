using Araponga.Application.Services;
using Araponga.Domain.Events;
using Araponga.Domain.Territories;
using Araponga.Domain.Users;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Araponga.Tests.Application;

/// <summary>
/// Testes do EventsService usando ServiceTestFactory (composição baseada em módulos).
/// Demonstra a migração para o novo padrão de testes modularizáveis.
/// </summary>
public sealed class EventsServiceModularTests
{
    private static readonly DateTime TestDate = DateTime.UtcNow;
    private static readonly Guid TestTerritoryId = Guid.NewGuid();
    private static readonly Guid TestUserId = Guid.NewGuid();

    private static async Task EnsureTestUserExists(IServiceProvider provider, Guid userId)
    {
        var userRepository = provider.GetRequiredService<Araponga.Application.Interfaces.IUserRepository>();
        var existing = await userRepository.GetByIdAsync(userId, CancellationToken.None);
        if (existing is null)
        {
            var user = new User(
                userId,
                "Test User",
                "test@example.com",
                "123.456.789-00",
                null,
                null,
                null,
                "test",
                $"test-{userId}",
                TestDate);
            await userRepository.AddAsync(user, CancellationToken.None);
        }
    }

    [Fact]
    public async Task CreateEventAsync_WithEmptyTerritoryId_ReturnsFailure()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<EventsService>(config);
        var service = factory.CreateService();
        var provider = factory.CreateServiceProvider();
        await EnsureTestUserExists(provider, TestUserId);

        // Act
        var result = await service.CreateEventAsync(
            Guid.Empty,
            TestUserId,
            "Test Event",
            "Description",
            TestDate.AddDays(1),
            TestDate.AddDays(2),
            0.0,
            0.0,
            "Location",
            null,
            null,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Territory ID is required", result.Error ?? "");
    }

    [Fact]
    public async Task CreateEventAsync_WithEmptyUserId_ReturnsFailure()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<EventsService>(config);
        var service = factory.CreateService();

        // Act
        var result = await service.CreateEventAsync(
            TestTerritoryId,
            Guid.Empty,
            "Test Event",
            "Description",
            TestDate.AddDays(1),
            TestDate.AddDays(2),
            0.0,
            0.0,
            "Location",
            null,
            null,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("User ID is required", result.Error ?? "");
    }

    [Fact]
    public async Task CreateEventAsync_WithNullTitle_ReturnsFailure()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<EventsService>(config);
        var service = factory.CreateService();
        var provider = factory.CreateServiceProvider();
        await EnsureTestUserExists(provider, TestUserId);

        // Act
        var result = await service.CreateEventAsync(
            TestTerritoryId,
            TestUserId,
            null!,
            "Description",
            TestDate.AddDays(1),
            TestDate.AddDays(2),
            0.0,
            0.0,
            "Location",
            null,
            null,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Title", result.Error ?? "");
    }

    [Fact]
    public async Task CreateEventAsync_CreatesEvent_WhenValid()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<EventsService>(config);
        var service = factory.CreateService();
        var provider = factory.CreateServiceProvider();
        await EnsureTestUserExists(provider, TestUserId);

        // Act
        var result = await service.CreateEventAsync(
            TestTerritoryId,
            TestUserId,
            "Test Event",
            "Description",
            TestDate.AddDays(1),
            TestDate.AddDays(2),
            0.0,
            0.0,
            "Location",
            null,
            null,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.NotNull(result.Value!.Event);
        Assert.Equal("Test Event", result.Value.Event.Title);
        Assert.Equal(TestTerritoryId, result.Value.Event.TerritoryId);
        Assert.Equal(TestUserId, result.Value.Event.CreatedByUserId);
    }

    [Fact]
    public void EventsService_CreatesAllDependencies()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<EventsService>(config);

        // Act
        var service = factory.CreateService();
        var provider = factory.CreateServiceProvider();

        // Assert - verificar que todas as dependências foram criadas via módulos
        Assert.NotNull(service);
        Assert.NotNull(provider.GetService(typeof(EventsService)));
    }
}
