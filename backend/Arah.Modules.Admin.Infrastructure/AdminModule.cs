using Arah.Application;
using Arah.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arah.Modules.Admin;

public sealed class AdminModule : IModule
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // TODO: Criar AdminDbContext e entidades quando necessário
        // Por enquanto, Admin pode usar SharedDbContext ou ArahDbContext temporariamente
    }
}
