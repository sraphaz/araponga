using Araponga.Application.Services;
using Araponga.Domain.Feed;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests.TestHelpers;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Araponga.Tests.Application;

/// <summary>
/// Testes do FeedService usando ServiceTestFactory (composição baseada em módulos).
/// Demonstra a migração para o novo padrão de testes modularizáveis.
/// </summary>
public sealed class FeedServiceModularTests
{
    [Fact]
    public async Task CreatePost_UsingServiceTestFactory_WorksCorrectly()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<FeedService>(config);

        var service = factory.CreateService();
        var territoryId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Act
        var result = await service.CreatePostAsync(
            territoryId,
            userId,
            "Test Post",
            "Content",
            PostType.General,
            PostVisibility.Public,
            PostStatus.Published,
            null,
            null,
            null,
            null,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Test Post", result.Value.Title);
    }

    [Fact]
    public async Task ListForTerritory_UsingServiceTestFactory_ReturnsPosts()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<FeedService>(config);

        var service = factory.CreateService();
        var territoryId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Criar alguns posts
        await service.CreatePostAsync(
            territoryId,
            userId,
            "Post 1",
            "Content 1",
            PostType.General,
            PostVisibility.Public,
            PostStatus.Published,
            null,
            null,
            null,
            null,
            CancellationToken.None);

        await service.CreatePostAsync(
            territoryId,
            userId,
            "Post 2",
            "Content 2",
            PostType.General,
            PostVisibility.Public,
            PostStatus.Published,
            null,
            null,
            null,
            null,
            CancellationToken.None);

        // Act
        var posts = await service.ListForTerritoryAsync(
            territoryId,
            userId,
            null,
            null,
            false,
            CancellationToken.None);

        // Assert
        Assert.NotNull(posts);
        Assert.Equal(2, posts.Count);
    }

    [Fact]
    public async Task LikePost_UsingServiceTestFactory_WorksCorrectly()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<FeedService>(config);

        var service = factory.CreateService();
        var territoryId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Criar post
        var createResult = await service.CreatePostAsync(
            territoryId,
            userId,
            "Test Post",
            "Content",
            PostType.General,
            PostVisibility.Public,
            PostStatus.Published,
            null,
            null,
            null,
            null,
            CancellationToken.None);

        Assert.True(createResult.IsSuccess);
        var postId = createResult.Value!.Id;

        // Act
        var liked = await service.LikeAsync(
            territoryId,
            postId,
            $"session:{userId}",
            userId,
            CancellationToken.None);

        // Assert
        Assert.True(liked);
    }

    [Fact]
    public void ServiceTestFactory_CreatesAllDependencies()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<FeedService>(config);

        // Act
        var service = factory.CreateService();
        var provider = factory.CreateServiceProvider();

        // Assert - verificar que todas as dependências foram criadas via módulos
        Assert.NotNull(service);
        Assert.NotNull(provider.GetService(typeof(PostCreationService)));
        Assert.NotNull(provider.GetService(typeof(PostInteractionService)));
        Assert.NotNull(provider.GetService(typeof(PostFilterService)));
    }
}
