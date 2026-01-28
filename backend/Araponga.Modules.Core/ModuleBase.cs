using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Core;

/// <summary>
/// Classe base abstrata para módulos, reduzindo boilerplate.
/// </summary>
public abstract class ModuleBase : IModule
{
    public abstract string Id { get; }
    public virtual string[] DependsOn => Array.Empty<string>();
    public virtual bool IsRequired => false;

    public abstract void RegisterServices(IServiceCollection services, IConfiguration configuration);
}
