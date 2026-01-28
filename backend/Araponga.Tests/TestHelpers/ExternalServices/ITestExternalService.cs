namespace Araponga.Tests.TestHelpers.ExternalServices;

/// <summary>
/// Interface para serviços externos em testes.
/// Permite reset e configuração de comportamento para testes isolados.
/// </summary>
public interface ITestExternalService
{
    /// <summary>
    /// Reseta o estado do serviço (limpa dados, reseta comportamentos configurados).
    /// </summary>
    void Reset();

    /// <summary>
    /// Configura um comportamento específico para o serviço.
    /// </summary>
    /// <param name="behavior">Nome do comportamento (ex: "throw_exception", "return_null").</param>
    /// <param name="result">Resultado opcional para o comportamento.</param>
    void ConfigureBehavior(string behavior, object? result = null);
}
