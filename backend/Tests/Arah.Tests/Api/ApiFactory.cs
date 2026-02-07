using Arah.Tests.ApiSupport;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Arah.Tests.Api;

/// <summary>
/// Factory para criar instâncias da API para testes.
/// Estende BaseApiFactory (ApiSupport) e adiciona carregamento de appsettings.json do projeto de testes.
/// </summary>
public sealed class ApiFactory : BaseApiFactory
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureAppConfiguration((context, config) =>
        {
            var assemblyLocation = typeof(ApiFactory).Assembly.Location;
            var testProjectDir = Path.GetDirectoryName(assemblyLocation);
            if (testProjectDir != null)
            {
                var appsettingsInBin = Path.Combine(testProjectDir, "appsettings.json");
                if (File.Exists(appsettingsInBin))
                    config.AddJsonFile(appsettingsInBin, optional: false, reloadOnChange: false);
                else
                {
                    var appsettingsInProject = Path.Combine(testProjectDir, "..", "..", "..", "appsettings.json");
                    var normalizedPath = Path.GetFullPath(appsettingsInProject);
                    if (File.Exists(normalizedPath))
                        config.AddJsonFile(normalizedPath, optional: false, reloadOnChange: false);
                }
            }
        });
    }
}
