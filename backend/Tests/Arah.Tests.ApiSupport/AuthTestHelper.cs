using System.Net.Http.Headers;
using System.Net.Http.Json;
using Arah.Api;
using Arah.Api.Contracts.Auth;

namespace Arah.Tests.ApiSupport;

/// <summary>
/// Helper de autenticação para testes de API. Compartilhado por Arah.Tests e Arah.Tests.Modules.Subscriptions
/// para manter uma única implementação de login (JWT, session) e evitar divergência.
/// </summary>
public static class AuthTestHelper
{
    /// <summary>
    /// Realiza login social e retorna o access token.
    /// </summary>
    public static async Task<string> LoginForTokenAsync(
        HttpClient client,
        string provider,
        string externalId,
        string? email = null,
        CancellationToken cancellationToken = default)
    {
        var response = await client.PostAsJsonAsync(
            "api/v1/auth/social",
            new SocialLoginRequest(
                provider,
                externalId,
                "Test User",
                "123.456.789-00",
                null,
                null,
                null,
                email ?? "test@Arah.com"),
            cancellationToken);
        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadFromJsonAsync<SocialLoginResponse>(cancellationToken);
        if (string.IsNullOrEmpty(payload?.Token))
            throw new InvalidOperationException($"Login failed: no token in response. Status: {response.StatusCode}");
        return payload.Token;
    }

    /// <summary>
    /// Realiza login social e retorna a resposta completa (User, Token, RefreshToken, ExpiresInSeconds).
    /// </summary>
    public static async Task<SocialLoginResponse> LoginAndGetResponseAsync(
        HttpClient client,
        string provider,
        string externalId,
        string? email = null,
        CancellationToken cancellationToken = default)
    {
        var response = await client.PostAsJsonAsync(
            "api/v1/auth/social",
            new SocialLoginRequest(
                provider,
                externalId,
                "Test User",
                "123.456.789-00",
                null,
                null,
                null,
                email ?? "test@Arah.com"),
            cancellationToken);
        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadFromJsonAsync<SocialLoginResponse>(cancellationToken);
        if (payload == null)
            throw new InvalidOperationException($"Login failed: null response. Status: {response.StatusCode}");
        return payload;
    }

    /// <summary>
    /// Define o header Authorization Bearer no cliente.
    /// </summary>
    public static void SetAuthHeader(HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    /// <summary>
    /// Define o header X-Session-Id.
    /// </summary>
    public static void SetSessionId(HttpClient client, string? sessionId = null)
    {
        if (client.DefaultRequestHeaders.Contains(ApiHeaders.SessionId))
            client.DefaultRequestHeaders.Remove(ApiHeaders.SessionId);
        client.DefaultRequestHeaders.Add(ApiHeaders.SessionId, sessionId ?? Guid.NewGuid().ToString());
    }

    /// <summary>
    /// Configura o cliente com Bearer token e SessionId (opcional).
    /// </summary>
    public static void SetupAuthenticatedClient(HttpClient client, string token, string? sessionId = null)
    {
        SetAuthHeader(client, token);
        SetSessionId(client, sessionId);
    }
}
