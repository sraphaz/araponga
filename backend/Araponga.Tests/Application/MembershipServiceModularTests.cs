using Araponga.Application.Common;
using Araponga.Application.Services;
using Araponga.Domain.Membership;
using Araponga.Domain.Territories;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Araponga.Tests.Application;

/// <summary>
/// Testes do MembershipService usando ServiceTestFactory (composição baseada em módulos).
/// Demonstra a migração para o novo padrão de testes modularizáveis.
/// </summary>
public sealed class MembershipServiceModularTests
{
    private static readonly Guid TerritoryId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid TerritoryId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
    // Usar UserId diferente do pré-existente no InMemoryDataStore (aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa)
    private static readonly Guid UserId = Guid.Parse("99999999-9999-9999-9999-999999999999");
    
    // Coordenadas dos territórios (do InMemoryDataStore)
    private const double Territory1Lat = -23.3501;
    private const double Territory1Lng = -44.8912;
    private const double Territory2Lat = -23.3744;
    private const double Territory2Lng = -45.0205;
    
    // Coordenadas próximas (dentro de 5km)
    private const double NearTerritory1Lat = -23.3510;
    private const double NearTerritory1Lng = -44.8920;
    
    // Coordenadas distantes (fora de 5km)
    private const double FarLat = -23.4000;
    private const double FarLng = -44.9500;

    [Fact]
    public async Task EnterAsVisitorAsync_CreatesNewMembership()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MembershipService>(config);
        var service = factory.CreateService();

        // Act
        var membership = await service.EnterAsVisitorAsync(UserId, TerritoryId1, CancellationToken.None);

        // Assert
        Assert.NotNull(membership);
        Assert.Equal(UserId, membership.UserId);
        Assert.Equal(TerritoryId1, membership.TerritoryId);
        Assert.Equal(MembershipRole.Visitor, membership.Role);
        Assert.Equal(ResidencyVerification.None, membership.ResidencyVerification);
    }

    [Fact]
    public async Task EnterAsVisitorAsync_ReturnsExisting_IfAlreadyVisitor()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MembershipService>(config);
        var service = factory.CreateService();

        // Act
        var first = await service.EnterAsVisitorAsync(UserId, TerritoryId1, CancellationToken.None);
        var second = await service.EnterAsVisitorAsync(UserId, TerritoryId1, CancellationToken.None);

        // Assert
        Assert.Equal(first.Id, second.Id);
    }

    [Fact]
    public async Task BecomeResidentAsync_Succeeds_WhenNoExistingResident()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MembershipService>(config);
        var service = factory.CreateService();

        // Act
        var result = await service.BecomeResidentAsync(UserId, TerritoryId1, CancellationToken.None);

        // Assert
        if (!result.IsSuccess)
        {
            Assert.Fail($"BecomeResidentAsync failed: {result.Error}");
        }
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(MembershipRole.Resident, result.Value!.Role);
        Assert.Equal(ResidencyVerification.None, result.Value.ResidencyVerification);
    }

    [Fact]
    public async Task BecomeResidentAsync_Fails_WhenHasResidentInAnotherTerritory()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MembershipService>(config);
        var service = factory.CreateService();

        // Act - Criar Resident no território 1
        var firstResult = await service.BecomeResidentAsync(UserId, TerritoryId1, CancellationToken.None);
        Assert.True(firstResult.IsSuccess);

        // Act - Tentar criar Resident no território 2 (deve falhar)
        var secondResult = await service.BecomeResidentAsync(UserId, TerritoryId2, CancellationToken.None);

        // Assert
        Assert.True(secondResult.IsFailure);
        Assert.Contains("already has a Resident", secondResult.Error!);
    }

    [Fact]
    public async Task TransferResidencyAsync_DemotesCurrentResident()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MembershipService>(config);
        var service = factory.CreateService();
        var provider = factory.CreateServiceProvider();
        var repository = provider.GetRequiredService<Araponga.Application.Interfaces.ITerritoryMembershipRepository>();

        // Act - Criar Resident no território 1
        var becomeResult = await service.BecomeResidentAsync(UserId, TerritoryId1, CancellationToken.None);
        Assert.True(becomeResult.IsSuccess);

        // Act - Transferir para território 2
        var transferResult = await service.TransferResidencyAsync(UserId, TerritoryId2, CancellationToken.None);
        Assert.True(transferResult.IsSuccess);

        // Assert - Verificar que território 1 agora é Visitor
        var oldMembership = await repository.GetByUserAndTerritoryAsync(UserId, TerritoryId1, CancellationToken.None);
        Assert.NotNull(oldMembership);
        Assert.Equal(MembershipRole.Visitor, oldMembership!.Role);
        Assert.Equal(ResidencyVerification.None, oldMembership.ResidencyVerification);

        // Assert - Verificar que território 2 agora é Resident
        var newMembership = await repository.GetByUserAndTerritoryAsync(UserId, TerritoryId2, CancellationToken.None);
        Assert.NotNull(newMembership);
        Assert.Equal(MembershipRole.Resident, newMembership!.Role);
    }

    [Fact]
    public async Task TransferResidencyAsync_Fails_WhenNoCurrentResident()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MembershipService>(config);
        var service = factory.CreateService();

        // Act
        var result = await service.TransferResidencyAsync(UserId, TerritoryId2, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("does not have a Resident", result.Error!);
    }

    [Fact]
    public async Task VerifyResidencyByGeoAsync_UpdatesVerification()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MembershipService>(config);
        var service = factory.CreateService();
        var provider = factory.CreateServiceProvider();
        var repository = provider.GetRequiredService<Araponga.Application.Interfaces.ITerritoryMembershipRepository>();

        // Act - Criar Resident
        var becomeResult = await service.BecomeResidentAsync(UserId, TerritoryId1, CancellationToken.None);
        Assert.True(becomeResult.IsSuccess);

        // Act - Verificar geo com coordenadas próximas (dentro de 5km)
        var verifyResult = await service.VerifyResidencyByGeoAsync(
            UserId, 
            TerritoryId1, 
            NearTerritory1Lat, 
            NearTerritory1Lng, 
            DateTime.UtcNow, 
            CancellationToken.None);
        Assert.True(verifyResult.IsSuccess);

        // Assert - Verificar atualização
        var membership = await repository.GetByUserAndTerritoryAsync(UserId, TerritoryId1, CancellationToken.None);
        Assert.NotNull(membership);
        Assert.Equal(ResidencyVerification.GeoVerified, membership!.ResidencyVerification);
        Assert.NotNull(membership.LastGeoVerifiedAtUtc);
    }
    
    [Fact]
    public async Task VerifyResidencyByGeoAsync_Fails_WhenCoordinatesTooFar()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MembershipService>(config);
        var service = factory.CreateService();

        // Act - Criar Resident
        var becomeResult = await service.BecomeResidentAsync(UserId, TerritoryId1, CancellationToken.None);
        Assert.True(becomeResult.IsSuccess);

        // Act - Verificar geo com coordenadas distantes (fora de 5km)
        var verifyResult = await service.VerifyResidencyByGeoAsync(
            UserId, 
            TerritoryId1, 
            FarLat, 
            FarLng, 
            DateTime.UtcNow, 
            CancellationToken.None);

        // Assert
        Assert.True(verifyResult.IsFailure);
        Assert.Contains("too far", verifyResult.Error!, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task VerifyResidencyByGeoAsync_Fails_IfNotResident()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MembershipService>(config);
        var service = factory.CreateService();

        // Act - Criar apenas Visitor
        await service.EnterAsVisitorAsync(UserId, TerritoryId1, CancellationToken.None);

        // Act - Tentar verificar geo (deve falhar)
        var result = await service.VerifyResidencyByGeoAsync(
            UserId, 
            TerritoryId1, 
            NearTerritory1Lat, 
            NearTerritory1Lng, 
            DateTime.UtcNow, 
            CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("not a Resident", result.Error!);
    }

    [Fact]
    public async Task VerifyResidencyByDocumentAsync_UpdatesVerification()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MembershipService>(config);
        var service = factory.CreateService();
        var provider = factory.CreateServiceProvider();
        var repository = provider.GetRequiredService<Araponga.Application.Interfaces.ITerritoryMembershipRepository>();

        // Act - Criar Resident
        var becomeResult = await service.BecomeResidentAsync(UserId, TerritoryId1, CancellationToken.None);
        Assert.True(becomeResult.IsSuccess);

        // Act - Verificar documental
        var verifyResult = await service.VerifyResidencyByDocumentAsync(UserId, TerritoryId1, DateTime.UtcNow, CancellationToken.None);
        Assert.True(verifyResult.IsSuccess);

        // Assert - Verificar atualização
        var membership = await repository.GetByUserAndTerritoryAsync(UserId, TerritoryId1, CancellationToken.None);
        Assert.NotNull(membership);
        Assert.True(membership!.IsDocumentVerified());
        Assert.NotNull(membership.LastDocumentVerifiedAtUtc);
    }

    [Fact]
    public async Task ListMyMembershipsAsync_ReturnsAllMemberships()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MembershipService>(config);
        var service = factory.CreateService();

        // Act - Criar múltiplos Visitors
        await service.EnterAsVisitorAsync(UserId, TerritoryId1, CancellationToken.None);
        await service.EnterAsVisitorAsync(UserId, TerritoryId2, CancellationToken.None);

        var memberships = await service.ListMyMembershipsAsync(UserId, CancellationToken.None);

        // Assert
        Assert.Equal(2, memberships.Count);
        Assert.All(memberships, m => Assert.Equal(UserId, m.UserId));
        Assert.All(memberships, m => Assert.Equal(MembershipRole.Visitor, m.Role));
    }

    [Fact]
    public async Task ListMyMembershipsAsync_IncludesMultipleVisitors()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<MembershipService>(config);
        var service = factory.CreateService();

        var territory3 = Guid.NewGuid();
        
        // Act
        await service.EnterAsVisitorAsync(UserId, TerritoryId1, CancellationToken.None);
        await service.EnterAsVisitorAsync(UserId, TerritoryId2, CancellationToken.None);
        await service.EnterAsVisitorAsync(UserId, territory3, CancellationToken.None);

        var memberships = await service.ListMyMembershipsAsync(UserId, CancellationToken.None);

        // Assert
        Assert.Equal(3, memberships.Count);
        var territoryIds = memberships.Select(m => m.TerritoryId).ToHashSet();
        Assert.Contains(TerritoryId1, territoryIds);
        Assert.Contains(TerritoryId2, territoryIds);
        Assert.Contains(territory3, territoryIds);
    }
}
