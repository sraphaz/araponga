using Araponga.Application.Common;
using Araponga.Application.Services;
using Araponga.Domain.Feed;
using Araponga.Domain.Moderation;
using Araponga.Domain.Users;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests;
using Araponga.Tests.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Araponga.Tests.Application;

/// <summary>
/// Testes do ReportService usando ServiceTestFactory (composição baseada em módulos).
/// Demonstra a migração para o novo padrão de testes modularizáveis.
/// </summary>
public sealed class ReportServiceModularTests
{
    [Fact]
    public async Task ReportPostAsync_ReturnsError_WhenReasonIsEmpty()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<ReportService>(config);
        var service = factory.CreateService();
        var reporterId = TestIds.TestUserId1;
        var postId = TestIds.Post1;

        // Act
        var (created, error, report) = await service.ReportPostAsync(
            reporterId,
            postId,
            "",
            null,
            CancellationToken.None);

        // Assert
        Assert.False(created);
        Assert.Equal("Reason is required.", error);
        Assert.Null(report);
    }

    [Fact]
    public async Task ReportPostAsync_ReturnsError_WhenPostNotFound()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<ReportService>(config);
        var service = factory.CreateService();
        var reporterId = TestIds.TestUserId1;
        var nonExistentPostId = Guid.NewGuid();

        // Act
        var (created, error, report) = await service.ReportPostAsync(
            reporterId,
            nonExistentPostId,
            "Spam",
            null,
            CancellationToken.None);

        // Assert
        Assert.False(created);
        Assert.Equal("Post not found.", error);
        Assert.Null(report);
    }

    [Fact]
    public async Task ReportPostAsync_CreatesReport_WhenValid()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<ReportService>(config);
        var service = factory.CreateService();
        var reporterId = TestIds.TestUserId1;
        var postId = TestIds.Post1;

        // Act
        var (created, error, report) = await service.ReportPostAsync(
            reporterId,
            postId,
            "Spam",
            "This is spam content",
            CancellationToken.None);

        // Assert
        Assert.True(created);
        Assert.Null(error);
        Assert.NotNull(report);
        Assert.Equal(reporterId, report!.ReporterUserId);
        Assert.Equal(postId, report.TargetId);
        Assert.Equal(ReportTargetType.Post, report.TargetType);
        Assert.Equal("Spam", report.Reason);
        Assert.Equal("This is spam content", report.Details);
        Assert.Equal(ReportStatus.Open, report.Status);
    }

    [Fact]
    public async Task ReportPostAsync_ReturnsNull_WhenDuplicateReport()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<ReportService>(config);
        var service = factory.CreateService();
        var reporterId = TestIds.TestUserId1;
        var postId = TestIds.Post1;

        // Act - Primeiro report
        var (created1, error1, report1) = await service.ReportPostAsync(
            reporterId,
            postId,
            "Spam",
            null,
            CancellationToken.None);
        Assert.True(created1);

        // Act - Segundo report dentro da janela de duplicação
        var (created2, error2, report2) = await service.ReportPostAsync(
            reporterId,
            postId,
            "Spam",
            null,
            CancellationToken.None);

        // Assert
        Assert.False(created2);
        Assert.Null(error2);
        Assert.Null(report2);
    }

    [Fact]
    public async Task ReportUserAsync_ReturnsError_WhenReasonIsEmpty()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<ReportService>(config);
        var service = factory.CreateService();
        var reporterId = TestIds.TestUserId1;
        var reportedUserId = TestIds.ResidentUser;
        var territoryId = TestIds.Territory1;

        // Act
        var (created, error, report) = await service.ReportUserAsync(
            reporterId,
            territoryId,
            reportedUserId,
            "",
            null,
            CancellationToken.None);

        // Assert
        Assert.False(created);
        Assert.Equal("Reason is required.", error);
        Assert.Null(report);
    }

    [Fact]
    public async Task ReportUserAsync_ReturnsError_WhenUserNotFound()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<ReportService>(config);
        var service = factory.CreateService();
        var reporterId = TestIds.TestUserId1;
        var nonExistentUserId = Guid.NewGuid();
        var territoryId = TestIds.Territory1;

        // Act
        var (created, error, report) = await service.ReportUserAsync(
            reporterId,
            territoryId,
            nonExistentUserId,
            "Harassment",
            null,
            CancellationToken.None);

        // Assert
        Assert.False(created);
        Assert.Equal("User not found.", error);
        Assert.Null(report);
    }

    [Fact]
    public async Task ReportUserAsync_ReturnsError_WhenTerritoryIdIsEmpty()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<ReportService>(config);
        var service = factory.CreateService();
        var reporterId = TestIds.TestUserId1;
        var reportedUserId = TestIds.ResidentUser;

        // Act
        var (created, error, report) = await service.ReportUserAsync(
            reporterId,
            Guid.Empty,
            reportedUserId,
            "Harassment",
            null,
            CancellationToken.None);

        // Assert
        Assert.False(created);
        Assert.Equal("Territory ID is required.", error);
        Assert.Null(report);
    }

    [Fact]
    public async Task ReportUserAsync_CreatesReport_WhenValid()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<ReportService>(config);
        var service = factory.CreateService();
        var reporterId = TestIds.TestUserId1;
        var reportedUserId = TestIds.ResidentUser;
        var territoryId = TestIds.Territory1;

        // Act
        var (created, error, report) = await service.ReportUserAsync(
            reporterId,
            territoryId,
            reportedUserId,
            "Harassment",
            "User is harassing others",
            CancellationToken.None);

        // Assert
        Assert.True(created);
        Assert.Null(error);
        Assert.NotNull(report);
        Assert.Equal(reporterId, report!.ReporterUserId);
        Assert.Equal(reportedUserId, report.TargetId);
        Assert.Equal(ReportTargetType.User, report.TargetType);
        Assert.Equal("Harassment", report.Reason);
        Assert.Equal("User is harassing others", report.Details);
        Assert.Equal(ReportStatus.Open, report.Status);
    }

    [Fact]
    public async Task ListPagedAsync_ReturnsPagedResults()
    {
        // Arrange
        var dataStore = new InMemoryDataStore();
        var config = new DefaultTestServiceCollection(dataStore);
        var factory = new ServiceTestFactory<ReportService>(config);
        var service = factory.CreateService();
        var provider = factory.CreateServiceProvider();
        var feedRepository = provider.GetRequiredService<Araponga.Application.Interfaces.IFeedRepository>();
        
        // Verificar qual território o post pertence
        var post = await feedRepository.GetPostAsync(TestIds.Post1, CancellationToken.None);
        var territoryId = post?.TerritoryId ?? TestIds.Territory1;
        var reporterId = TestIds.TestUserId1;
        var postId = TestIds.Post1;

        // Act - Criar alguns reports
        var (created1, error1, _) = await service.ReportPostAsync(reporterId, postId, "Spam", null, CancellationToken.None);
        var (created2, error2, _) = await service.ReportPostAsync(reporterId, postId, "Inappropriate", null, CancellationToken.None);

        var pagination = new PaginationParameters(1, 10);
        var result = await service.ListPagedAsync(
            territoryId,
            null,
            null,
            null,
            null,
            pagination,
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        // Pelo menos 1 report deve existir (o primeiro, se criado)
        Assert.True(result.TotalCount >= (created1 ? 1 : 0));
        Assert.True(result.Items.Count <= 10);
    }
}
