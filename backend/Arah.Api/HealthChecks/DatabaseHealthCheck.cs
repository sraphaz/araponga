using Arah.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Arah.Api.HealthChecks;

public sealed class DatabaseHealthCheck : IHealthCheck
{
    private readonly ArahDbContext _dbContext;

    public DatabaseHealthCheck(ArahDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);
            return canConnect
                ? HealthCheckResult.Healthy("Banco de dados disponível.")
                : HealthCheckResult.Unhealthy("Banco de dados indisponível.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Falha ao acessar banco de dados.", ex);
        }
    }
}
