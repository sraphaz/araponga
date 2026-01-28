using Araponga.Application.Common;
using Araponga.Application.Services;
using Araponga.Domain.Map;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests.TestHelpers;
using Xunit;

namespace Araponga.Tests.Application;

/// <summary>
/// Testes do MapService usando ServiceTestFactory (composição baseada em módulos).
/// Demonstra a migração para o novo padrão de testes modularizáveis.
/// </summary>
public sealed class MapServiceModularTests
{
    private static readonly Guid TerritoryB = Guid.Parse("22222222-2222-2222-2222-222222222222");

    [Fact]
    public async Task ListEntitiesAsync_WhenUserIdNull_ReturnsOnlyPublic()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MapService>(config);
        var service = factory.CreateService();

        // Act
        var list = await service.ListEntitiesAsync(TerritoryB, null, CancellationToken.None);

        // Assert
        Assert.NotNull(list);
        Assert.All(list, e => Assert.Equal(MapEntityVisibility.Public, e.Visibility));
    }

    [Fact]
    public async Task ListEntitiesPagedAsync_ReturnsPagedResult()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MapService>(config);
        var service = factory.CreateService();
        var paging = new PaginationParameters(1, 10);

        // Act
        var result = await service.ListEntitiesPagedAsync(TerritoryB, null, paging, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(10, result.PageSize);
        Assert.All(result.Items, e => Assert.Equal(MapEntityVisibility.Public, e.Visibility));
    }

    [Fact]
    public async Task ListEntitiesAsync_WhenResident_ReturnsIncludingResidentsOnly()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MapService>(config);
        var service = factory.CreateService();
        var residentId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        // Act
        var list = await service.ListEntitiesAsync(TerritoryB, residentId, CancellationToken.None);

        // Assert
        Assert.NotNull(list);
        var hasResidentsOnly = list.Any(e => e.Visibility == MapEntityVisibility.ResidentsOnly);
        Assert.True(hasResidentsOnly, "Resident should see ResidentsOnly entities.");
    }

    [Fact]
    public async Task ListEntitiesAsync_WhenTerritoryHasNoEntities_ReturnsEmpty()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        dataStore.MapEntities.Clear();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MapService>(config);
        var service = factory.CreateService();

        // Act
        var list = await service.ListEntitiesAsync(TerritoryB, null, CancellationToken.None);

        // Assert
        Assert.NotNull(list);
        Assert.Empty(list);
    }

    [Fact]
    public void MapService_CreatesAllDependencies()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MapService>(config);

        // Act
        var service = factory.CreateService();
        var provider = factory.CreateServiceProvider();

        // Assert - verificar que todas as dependências foram criadas via módulos
        Assert.NotNull(service);
        Assert.NotNull(provider.GetService(typeof(MapService)));
    }
}
