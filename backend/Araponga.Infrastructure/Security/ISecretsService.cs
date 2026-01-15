namespace Araponga.Infrastructure.Security;

/// <summary>
/// Interface para serviços de gerenciamento de secrets (Key Vault, Secrets Manager, etc).
/// </summary>
public interface ISecretsService
{
    /// <summary>
    /// Obtém um secret pelo nome.
    /// </summary>
    Task<string?> GetSecretAsync(string secretName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um secret ou retorna o valor padrão se não encontrado.
    /// </summary>
    Task<string> GetSecretOrDefaultAsync(string secretName, string defaultValue, CancellationToken cancellationToken = default);
}
