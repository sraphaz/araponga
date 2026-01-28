using Araponga.Application.Services;
using Araponga.Domain.Marketplace;
using Araponga.Domain.Membership;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Araponga.Tests.Application;

/// <summary>
/// Testes do MarketplaceService usando ServiceTestFactory (composição baseada em módulos).
/// Demonstra a migração para o novo padrão de testes modularizáveis.
/// </summary>
public sealed class MarketplaceServiceModularTests
{
    private static readonly Guid TerritoryId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid ResidentUserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    [Fact]
    public async Task ResidentCanCreateStore_UsingServiceTestFactory()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<StoreService>(config);
        var service = factory.CreateService();
        var provider = factory.CreateServiceProvider();
        
        // Garantir opt-in do residente
        var membershipRepository = provider.GetRequiredService<Araponga.Application.Interfaces.ITerritoryMembershipRepository>();
        var settingsRepository = provider.GetRequiredService<Araponga.Application.Interfaces.IMembershipSettingsRepository>();
        var residentMembership = await membershipRepository.GetByUserAndTerritoryAsync(ResidentUserId, TerritoryId, CancellationToken.None);
        if (residentMembership is not null)
        {
            var residentSettings = await settingsRepository.GetByMembershipIdAsync(residentMembership.Id, CancellationToken.None);
            if (residentSettings is not null && !residentSettings.MarketplaceOptIn)
            {
                residentSettings.UpdateMarketplaceOptIn(true, DateTime.UtcNow);
                await settingsRepository.UpdateAsync(residentSettings, CancellationToken.None);
            }
        }

        // Act
        var storeResult = await service.UpsertMyStoreAsync(
            TerritoryId,
            ResidentUserId,
            "Loja do Morador",
            "Descrição",
            StoreContactVisibility.OnInquiryOnly,
            "(11) 90000-0000",
            null,
            "loja@exemplo.com",
            null,
            null,
            "whatsapp",
            CancellationToken.None);

        // Assert
        Assert.True(storeResult.IsSuccess);
        Assert.NotNull(storeResult.Value);
        Assert.Equal("Loja do Morador", storeResult.Value!.DisplayName);
    }

    [Fact]
    public async Task ResidentCanCreateItem_UsingServiceTestFactory()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var storeFactory = new ServiceTestFactory<StoreService>(config);
        var itemFactory = new ServiceTestFactory<StoreItemService>(config);
        var storeService = storeFactory.CreateService();
        var itemService = itemFactory.CreateService();
        var provider = storeFactory.CreateServiceProvider();
        
        // Garantir opt-in do residente
        var membershipRepository = provider.GetRequiredService<Araponga.Application.Interfaces.ITerritoryMembershipRepository>();
        var settingsRepository = provider.GetRequiredService<Araponga.Application.Interfaces.IMembershipSettingsRepository>();
        var residentMembership = await membershipRepository.GetByUserAndTerritoryAsync(ResidentUserId, TerritoryId, CancellationToken.None);
        if (residentMembership is not null)
        {
            var residentSettings = await settingsRepository.GetByMembershipIdAsync(residentMembership.Id, CancellationToken.None);
            if (residentSettings is not null && !residentSettings.MarketplaceOptIn)
            {
                residentSettings.UpdateMarketplaceOptIn(true, DateTime.UtcNow);
                await settingsRepository.UpdateAsync(residentSettings, CancellationToken.None);
            }
        }

        // Criar store primeiro
        var storeResult = await storeService.UpsertMyStoreAsync(
            TerritoryId,
            ResidentUserId,
            "Loja do Morador",
            "Descrição",
            StoreContactVisibility.OnInquiryOnly,
            "(11) 90000-0000",
            null,
            "loja@exemplo.com",
            null,
            null,
            "whatsapp",
            CancellationToken.None);
        Assert.True(storeResult.IsSuccess);

        // Act - Criar item
        var listingResult = await itemService.CreateItemAsync(
            TerritoryId,
            ResidentUserId,
            storeResult.Value!.Id,
            ItemType.Product,
            "Produto",
            null,
            "Categoria",
            "tag",
            ItemPricingType.Fixed,
            10m,
            "BRL",
            "unidade",
            null,
            null,
            ItemStatus.Active,
            null,
            CancellationToken.None);

        // Assert
        Assert.True(listingResult.IsSuccess);
        Assert.NotNull(listingResult.Value);
        Assert.Equal("Produto", listingResult.Value!.Title);
        Assert.Equal(10m, listingResult.Value.PriceAmount);
    }

    [Fact]
    public async Task StoreService_CreatesAllDependencies()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<StoreService>(config);

        // Act
        var service = factory.CreateService();
        var provider = factory.CreateServiceProvider();

        // Assert - verificar que todas as dependências foram criadas via módulos
        Assert.NotNull(service);
        Assert.NotNull(provider.GetService(typeof(StoreItemService)));
    }
}
