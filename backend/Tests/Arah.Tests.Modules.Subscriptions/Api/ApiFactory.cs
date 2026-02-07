using Arah.Tests.ApiSupport;

namespace Arah.Tests.Modules.Subscriptions.Api;

/// <summary>
/// Factory para testes do módulo Subscriptions. Usa BaseApiFactory (ApiSupport) para manter
/// a mesma configuração de JWT, RateLimiting e Persistence que o Core.
/// </summary>
public sealed class ApiFactory : BaseApiFactory
{
}
