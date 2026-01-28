using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Tests.TestHelpers;

/// <summary>
/// Interface para configuração de serviços em testes.
/// Permite que testes usem o mesmo pipeline de registro que o host (shared + módulos),
/// garantindo que testes "vejam" o mesmo DI que a aplicação.
/// </summary>
public interface ITestServiceCollection
{
    /// <summary>
    /// Configura o IServiceCollection com infraestrutura InMemory + serviços compartilhados + módulos.
    /// </summary>
    IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration);

    /// <summary>
    /// Constrói o ServiceProvider a partir da configuração.
    /// </summary>
    IServiceProvider BuildServiceProvider(IServiceCollection services);
}
