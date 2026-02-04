// Reexport para compatibilidade. A implementação compartilhada está em Araponga.Tests.ApiSupport.
// Novos testes devem usar: using Araponga.Tests.ApiSupport;
using Araponga.Api.Contracts.Auth;

namespace Araponga.Tests.TestHelpers;

/// <summary>
/// Alias para Araponga.Tests.ApiSupport.AuthTestHelper. Use ApiSupport para nova escrita.
/// </summary>
public static class AuthTestHelper
{
    public static Task<string> LoginForTokenAsync(HttpClient client, string provider, string externalId, string? email = null, CancellationToken cancellationToken = default)
        => ApiSupport.AuthTestHelper.LoginForTokenAsync(client, provider, externalId, email, cancellationToken);

    public static Task<SocialLoginResponse> LoginAndGetResponseAsync(HttpClient client, string provider, string externalId, string? email = null, CancellationToken cancellationToken = default)
        => ApiSupport.AuthTestHelper.LoginAndGetResponseAsync(client, provider, externalId, email, cancellationToken);

    public static void SetAuthHeader(HttpClient client, string token)
        => ApiSupport.AuthTestHelper.SetAuthHeader(client, token);

    public static void SetSessionId(HttpClient client, string? sessionId = null)
        => ApiSupport.AuthTestHelper.SetSessionId(client, sessionId);

    public static void SetupAuthenticatedClient(HttpClient client, string token, string? sessionId = null)
        => ApiSupport.AuthTestHelper.SetupAuthenticatedClient(client, token, sessionId);
}
