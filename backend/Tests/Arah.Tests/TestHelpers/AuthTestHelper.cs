// Reexport para compatibilidade. A implementação compartilhada está em Arah.Tests.ApiSupport.
// Novos testes devem usar: using Arah.Tests.ApiSupport;
using Arah.Api.Contracts.Auth;

namespace Arah.Tests.TestHelpers;

/// <summary>
/// Alias para Arah.Tests.ApiSupport.AuthTestHelper. Use ApiSupport para nova escrita.
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
