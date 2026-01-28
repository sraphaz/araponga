using Araponga.Application.Common;
using Araponga.Application.Services;
using Araponga.Domain.Membership;
using Araponga.Domain.Social.JoinRequests;
using Araponga.Domain.Territories;
using Araponga.Domain.Users;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests;
using Araponga.Tests.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Araponga.Tests.Application;

/// <summary>
/// Testes do JoinRequestService usando ServiceTestFactory (composição baseada em módulos).
/// Demonstra a migração para o novo padrão de testes modularizáveis.
/// </summary>
public sealed class JoinRequestServiceModularTests
{
    private static async Task EnsureTestUserExists(IServiceProvider provider, Guid userId, string displayName, string cpf)
    {
        var userRepository = provider.GetRequiredService<Araponga.Application.Interfaces.IUserRepository>();
        var existing = await userRepository.GetByIdAsync(userId, CancellationToken.None);
        if (existing is null)
        {
            var user = new User(userId, displayName, null, cpf, null, null, null, "test", $"test-{userId}", DateTime.UtcNow);
            await userRepository.AddAsync(user, CancellationToken.None);
        }
    }

    [Fact]
    public async Task CreateAsync_ReturnsError_WhenNoRecipients()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<JoinRequestService>(config);
        var service = factory.CreateService();
        var requesterId = TestIds.TestUserId1;
        var territoryId = TestIds.Territory1;

        // Act
        var (created, error, request) = await service.CreateAsync(
            requesterId,
            territoryId,
            Array.Empty<Guid>(),
            null,
            CancellationToken.None);

        // Assert
        Assert.False(created);
        Assert.Equal("Recipients are required.", error);
        Assert.Null(request);
    }

    [Fact]
    public async Task CreateAsync_ReturnsError_WhenDuplicateRecipients()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<JoinRequestService>(config);
        var service = factory.CreateService();
        var requesterId = TestIds.TestUserId1;
        var territoryId = TestIds.Territory1;
        var recipientId = TestIds.ResidentUser;

        // Act
        var (created, error, request) = await service.CreateAsync(
            requesterId,
            territoryId,
            new[] { recipientId, recipientId },
            null,
            CancellationToken.None);

        // Assert
        Assert.False(created);
        Assert.Equal("Recipients must be unique.", error);
        Assert.Null(request);
    }

    [Fact]
    public async Task CreateAsync_ReturnsError_WhenRequesterIsRecipient()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<JoinRequestService>(config);
        var service = factory.CreateService();
        var requesterId = TestIds.TestUserId1;
        var territoryId = TestIds.Territory1;

        // Act
        var (created, error, request) = await service.CreateAsync(
            requesterId,
            territoryId,
            new[] { requesterId },
            null,
            CancellationToken.None);

        // Assert
        Assert.False(created);
        Assert.Equal("Requester cannot be a recipient.", error);
        Assert.Null(request);
    }

    [Fact]
    public async Task CreateAsync_ReturnsError_WhenRecipientNotFound()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<JoinRequestService>(config);
        var service = factory.CreateService();
        var requesterId = TestIds.TestUserId1;
        var territoryId = TestIds.Territory1;
        var nonExistentRecipientId = Guid.NewGuid();

        // Act
        var (created, error, request) = await service.CreateAsync(
            requesterId,
            territoryId,
            new[] { nonExistentRecipientId },
            null,
            CancellationToken.None);

        // Assert
        Assert.False(created);
        Assert.Equal("Recipient not found.", error);
        Assert.Null(request);
    }

    [Fact]
    public async Task CreateAsync_CreatesRequest_WhenValid()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<JoinRequestService>(config);
        var service = factory.CreateService();
        var provider = factory.CreateServiceProvider();
        await EnsureTestUserExists(provider, TestIds.TestUserId1, "Test User 1", "111.111.111-11");
        
        var requesterId = TestIds.TestUserId1;
        var territoryId = TestIds.Territory2; // Territory2 tem ResidentUser como resident
        var recipientId = TestIds.ResidentUser;

        // Act
        var (created, error, request) = await service.CreateAsync(
            requesterId,
            territoryId,
            new[] { recipientId },
            "Please approve my request",
            CancellationToken.None);

        // Assert
        Assert.True(created);
        Assert.Null(error);
        Assert.NotNull(request);
        Assert.Equal(requesterId, request!.RequesterUserId);
        Assert.Equal(territoryId, request.TerritoryId);
        Assert.Equal("Please approve my request", request.Message);
        Assert.Equal(TerritoryJoinRequestStatus.Pending, request.Status);
    }

    [Fact]
    public async Task ApproveAsync_ReturnsError_WhenRequestNotFound()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<JoinRequestService>(config);
        var service = factory.CreateService();
        var nonExistentRequestId = Guid.NewGuid();
        var actorId = TestIds.ResidentUser;

        // Act
        var result = await service.ApproveAsync(
            nonExistentRequestId,
            actorId,
            false,
            CancellationToken.None);

        // Assert
        Assert.False(result.Found);
        Assert.False(result.Forbidden);
        Assert.Null(result.Request);
        Assert.False(result.Updated);
    }

    [Fact]
    public async Task RejectAsync_RejectsRequest_WhenValid()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<JoinRequestService>(config);
        var service = factory.CreateService();
        var provider = factory.CreateServiceProvider();
        await EnsureTestUserExists(provider, TestIds.TestUserId1, "Test User 1", "111.111.111-11");
        
        var requesterId = TestIds.TestUserId1;
        var territoryId = TestIds.Territory2;
        var recipientId = TestIds.ResidentUser;

        // Act - Criar request
        var (created, error, request) = await service.CreateAsync(
            requesterId,
            territoryId,
            new[] { recipientId },
            "Test request",
            CancellationToken.None);
        Assert.True(created);

        // Act - Rejeitar
        var result = await service.RejectAsync(
            request!.Id,
            recipientId,
            false,
            CancellationToken.None);

        // Assert
        Assert.True(result.Found);
        Assert.False(result.Forbidden);
        Assert.NotNull(result.Request);
        Assert.True(result.Updated);
        Assert.Equal(TerritoryJoinRequestStatus.Rejected, result.Request.Status);
    }

    [Fact]
    public async Task CancelAsync_CancelsRequest_WhenValid()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<JoinRequestService>(config);
        var service = factory.CreateService();
        var provider = factory.CreateServiceProvider();
        await EnsureTestUserExists(provider, TestIds.TestUserId1, "Test User 1", "111.111.111-11");
        
        var requesterId = TestIds.TestUserId1;
        var territoryId = TestIds.Territory2;
        var recipientId = TestIds.ResidentUser;

        // Act - Criar request
        var (created, error, request) = await service.CreateAsync(
            requesterId,
            territoryId,
            new[] { recipientId },
            "Test request",
            CancellationToken.None);
        Assert.True(created);

        // Act - Cancelar
        var result = await service.CancelAsync(
            request!.Id,
            requesterId,
            CancellationToken.None);

        // Assert
        Assert.True(result.Found);
        Assert.False(result.Forbidden);
        Assert.NotNull(result.Request);
        Assert.True(result.Updated);
        Assert.Equal(TerritoryJoinRequestStatus.Canceled, result.Request.Status);
    }

    [Fact]
    public async Task ListIncomingPagedAsync_ReturnsPagedResults()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<JoinRequestService>(config);
        var service = factory.CreateService();
        var provider = factory.CreateServiceProvider();
        await EnsureTestUserExists(provider, TestIds.TestUserId1, "Test User 1", "111.111.111-11");
        await EnsureTestUserExists(provider, TestIds.TestUserId3, "Test User 3", "333.333.333-33");
        
        var requesterId = TestIds.TestUserId1;
        var territoryId = TestIds.Territory2;
        var recipientId = TestIds.ResidentUser;

        // Act - Criar alguns requests
        await service.CreateAsync(requesterId, territoryId, new[] { recipientId }, "Request 1", CancellationToken.None);
        await service.CreateAsync(TestIds.TestUserId3, territoryId, new[] { recipientId }, "Request 2", CancellationToken.None);

        var pagination = new PaginationParameters(1, 10);
        var result = await service.ListIncomingPagedAsync(
            recipientId,
            pagination,
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.TotalCount >= 2);
        Assert.True(result.Items.Count <= 10);
    }
}
