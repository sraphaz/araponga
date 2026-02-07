using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Arah.Infrastructure.Security;

/// <summary>
/// Implementação de ISecretsService usando variáveis de ambiente (fallback quando Key Vault não está disponível).
/// </summary>
public sealed class EnvironmentSecretsService : ISecretsService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EnvironmentSecretsService> _logger;

    public EnvironmentSecretsService(IConfiguration configuration, ILogger<EnvironmentSecretsService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public Task<string?> GetSecretAsync(string secretName, CancellationToken cancellationToken = default)
    {
        // Tentar ler de variável de ambiente primeiro (formato: SECRET_NAME)
        var envValue = Environment.GetEnvironmentVariable(secretName.Replace(":", "__"));
        
        // Se não encontrado, tentar de Configuration (appsettings.json)
        var configValue = _configuration[secretName];
        
        var value = envValue ?? configValue;
        
        if (string.IsNullOrWhiteSpace(value))
        {
            _logger.LogWarning("Secret '{SecretName}' not found in environment or configuration", secretName);
            return Task.FromResult<string?>(null);
        }

        return Task.FromResult<string?>(value);
    }

    public async Task<string> GetSecretOrDefaultAsync(string secretName, string defaultValue, CancellationToken cancellationToken = default)
    {
        var secret = await GetSecretAsync(secretName, cancellationToken);
        return secret ?? defaultValue;
    }
}
