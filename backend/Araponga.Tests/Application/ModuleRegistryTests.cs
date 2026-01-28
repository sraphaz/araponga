using Araponga.Modules.Core;
using Araponga.Modules.CoreModule;
using Araponga.Modules.Feed;
using Araponga.Modules.Marketplace;
using Araponga.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using Xunit;

namespace Araponga.Tests.Application;

public sealed class ModuleRegistryTests
{
    [Fact]
    public void Apply_WhenAllModulesEnabled_RegistersAllServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateConfiguration(new Dictionary<string, object?>
        {
            { "Modules:Core:Enabled", true },
            { "Modules:Core:Required", true },
            { "Modules:Feed:Enabled", true },
            { "Modules:Marketplace:Enabled", true }
        });

        var modules = new IModule[]
        {
            new Araponga.Modules.CoreModule.CoreModule(),
            new Araponga.Modules.Feed.FeedModule(),
            new Araponga.Modules.Marketplace.MarketplaceModule()
        };

        var loggerFactory = LoggerFactory.Create(b => b.AddConsole().SetMinimumLevel(LogLevel.Warning));
        var registry = new ModuleRegistry(modules, loggerFactory.CreateLogger<ModuleRegistry>());

        // Act
        registry.Apply(services, configuration);

        // Assert - Verificar que serviços foram registrados (sem criar instâncias)
        var territoryServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(TerritoryService));
        var authServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(AuthService));
        var feedServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(FeedService));
        var postCreationServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(PostCreationService));
        var storeServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(StoreService));
        
        Assert.NotNull(territoryServiceDescriptor);
        Assert.NotNull(authServiceDescriptor);
        Assert.NotNull(feedServiceDescriptor);
        Assert.NotNull(postCreationServiceDescriptor);
        Assert.NotNull(storeServiceDescriptor);
        
        // Verificar que módulos estão habilitados
        Assert.True(registry.IsModuleEnabled("Core"));
        Assert.True(registry.IsModuleEnabled("Feed"));
        Assert.True(registry.IsModuleEnabled("Marketplace"));
    }

    [Fact]
    public void Apply_WhenModuleDisabled_DoesNotRegisterServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateConfiguration(new Dictionary<string, object?>
        {
            { "Modules:Core:Enabled", true },
            { "Modules:Core:Required", true },
            { "Modules:Feed:Enabled", false }
        });

        var modules = new IModule[]
        {
            new Araponga.Modules.CoreModule.CoreModule(),
            new Araponga.Modules.Feed.FeedModule()
        };

        var loggerFactory = LoggerFactory.Create(b => b.AddConsole().SetMinimumLevel(LogLevel.Warning));
        var registry = new ModuleRegistry(modules, loggerFactory.CreateLogger<ModuleRegistry>());

        // Act
        registry.Apply(services, configuration);

        // Assert - Verificar registro sem criar instâncias
        var territoryServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(TerritoryService));
        var feedServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(FeedService));
        
        // Core deve estar registrado
        Assert.NotNull(territoryServiceDescriptor);
        Assert.True(registry.IsModuleEnabled("Core"));
        
        // Feed não deve estar registrado
        Assert.Null(feedServiceDescriptor);
        Assert.False(registry.IsModuleEnabled("Feed"));
    }

    [Fact]
    public void Apply_WhenRequiredModuleDisabled_ThrowsException()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateConfiguration(new Dictionary<string, object?>
        {
            { "Modules:Core:Enabled", false },
            { "Modules:Core:Required", true }
        });

        var modules = new IModule[]
        {
            new Araponga.Modules.CoreModule.CoreModule()
        };

        var loggerFactory = LoggerFactory.Create(b => b.AddConsole().SetMinimumLevel(LogLevel.Warning));
        var registry = new ModuleRegistry(modules, loggerFactory.CreateLogger<ModuleRegistry>());

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => registry.Apply(services, configuration));
        Assert.Contains("obrigatório", exception.Message);
        Assert.Contains("Core", exception.Message);
    }

    [Fact]
    public void Apply_WhenDependentModuleDisabled_ThrowsException()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateConfiguration(new Dictionary<string, object?>
        {
            { "Modules:Core:Enabled", false },
            { "Modules:Core:Required", false },
            { "Modules:Feed:Enabled", true }
        });

        var modules = new IModule[]
        {
            new Araponga.Modules.CoreModule.CoreModule(),
            new Araponga.Modules.Feed.FeedModule()
        };

        var loggerFactory = LoggerFactory.Create(b => b.AddConsole().SetMinimumLevel(LogLevel.Warning));
        var registry = new ModuleRegistry(modules, loggerFactory.CreateLogger<ModuleRegistry>());

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => registry.Apply(services, configuration));
        Assert.Contains("depende", exception.Message);
        Assert.Contains("Feed", exception.Message);
        Assert.Contains("Core", exception.Message);
    }

    [Fact]
    public void Apply_WhenNoModulesSection_TreatsAllAsEnabled()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build(); // Config vazia

        var modules = new IModule[]
        {
            new Araponga.Modules.CoreModule.CoreModule(),
            new Araponga.Modules.Feed.FeedModule()
        };

        var loggerFactory = LoggerFactory.Create(b => b.AddConsole().SetMinimumLevel(LogLevel.Warning));
        var registry = new ModuleRegistry(modules, loggerFactory.CreateLogger<ModuleRegistry>());

        // Act
        registry.Apply(services, configuration);

        // Assert - Verificar registro sem criar instâncias
        var territoryServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(TerritoryService));
        var feedServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(FeedService));
        
        // Todos os módulos devem estar habilitados (default)
        Assert.NotNull(territoryServiceDescriptor);
        Assert.NotNull(feedServiceDescriptor);
        Assert.True(registry.IsModuleEnabled("Core"));
        Assert.True(registry.IsModuleEnabled("Feed"));
    }

    [Fact]
    public void Apply_RespectsDependencyOrder()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateConfiguration(new Dictionary<string, object?>
        {
            { "Modules:Core:Enabled", true },
            { "Modules:Feed:Enabled", true },
            { "Modules:Marketplace:Enabled", true }
        });

        var modules = new IModule[]
        {
            new Araponga.Modules.Feed.FeedModule(), // Feed depende de Core
            new Araponga.Modules.CoreModule.CoreModule(),
            new Araponga.Modules.Marketplace.MarketplaceModule() // Marketplace depende de Core
        };

        var loggerFactory = LoggerFactory.Create(b => b.AddConsole().SetMinimumLevel(LogLevel.Warning));
        var registry = new ModuleRegistry(modules, loggerFactory.CreateLogger<ModuleRegistry>());

        // Act
        registry.Apply(services, configuration);

        // Assert - Verificar registro sem criar instâncias
        var territoryServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(TerritoryService));
        var feedServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(FeedService));
        var storeServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(StoreService));
        
        // Todos devem estar registrados, independente da ordem de entrada
        Assert.NotNull(territoryServiceDescriptor);
        Assert.NotNull(feedServiceDescriptor);
        Assert.NotNull(storeServiceDescriptor);
    }

    [Fact]
    public void IsModuleEnabled_ReturnsCorrectState()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = CreateConfiguration(new Dictionary<string, object?>
        {
            { "Modules:Core:Enabled", true },
            { "Modules:Feed:Enabled", true },
            { "Modules:Marketplace:Enabled", false }
        });

        var modules = new IModule[]
        {
            new Araponga.Modules.CoreModule.CoreModule(),
            new Araponga.Modules.Feed.FeedModule(),
            new Araponga.Modules.Marketplace.MarketplaceModule()
        };

        var loggerFactory = LoggerFactory.Create(b => b.AddConsole().SetMinimumLevel(LogLevel.Warning));
        var registry = new ModuleRegistry(modules, loggerFactory.CreateLogger<ModuleRegistry>());

        // Act
        registry.Apply(services, configuration);

        // Assert
        Assert.True(registry.IsModuleEnabled("Core"));
        Assert.True(registry.IsModuleEnabled("Feed"));
        Assert.False(registry.IsModuleEnabled("Marketplace"));
        Assert.False(registry.IsModuleEnabled("NonExistent"));
    }

    private static IConfiguration CreateConfiguration(Dictionary<string, object?> values)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values.Select(kvp => new KeyValuePair<string, string?>(kvp.Key, kvp.Value?.ToString())))
            .Build();
    }
}
