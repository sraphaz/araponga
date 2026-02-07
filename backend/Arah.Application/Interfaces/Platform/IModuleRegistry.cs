using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arah.Application.Interfaces;

/// <summary>
/// Registry de módulos para gerenciar registro de serviços modulares
/// </summary>
public interface IModuleRegistry
{
    void Apply(IServiceCollection services, IConfiguration configuration);
}
