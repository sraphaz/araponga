using Araponga.Modules.Core;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Araponga.Api.HealthChecks;

/// <summary>
/// Health check que verifica o status dos módulos da aplicação.
/// Verifica se os módulos obrigatórios estão habilitados e se os módulos habilitados estão funcionando.
/// </summary>
public sealed class ModuleHealthCheck : IHealthCheck
{
    private readonly IModuleRegistry _moduleRegistry;

    public ModuleHealthCheck(IModuleRegistry moduleRegistry)
    {
        _moduleRegistry = moduleRegistry ?? throw new ArgumentNullException(nameof(moduleRegistry));
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var enabledModules = _moduleRegistry.GetEnabledModules();
            var requiredModules = _moduleRegistry.GetRequiredModules();

            // Verificar se todos os módulos obrigatórios estão habilitados
            var missingRequired = requiredModules
                .Where(required => !enabledModules.Contains(required))
                .ToList();

            if (missingRequired.Any())
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(
                    $"Módulos obrigatórios desabilitados: {string.Join(", ", missingRequired)}"));
            }

            // Verificar se há módulos habilitados
            if (!enabledModules.Any())
            {
                return Task.FromResult(HealthCheckResult.Degraded(
                    "Nenhum módulo habilitado."));
            }

            var data = new Dictionary<string, object>
            {
                ["enabled_modules"] = enabledModules.Count,
                ["enabled_modules_list"] = string.Join(", ", enabledModules),
                ["required_modules"] = requiredModules.Count
            };

            return Task.FromResult(HealthCheckResult.Healthy(
                $"Módulos funcionando corretamente. {enabledModules.Count} módulo(s) habilitado(s).",
                data));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy(
                "Falha ao verificar status dos módulos.",
                ex));
        }
    }
}
