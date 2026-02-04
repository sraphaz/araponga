using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace Araponga.Bff.Services;

/// <summary>
/// Encaminha requisições de jornada para a API principal (Araponga.Api).
/// O BFF é uma aplicação separada que repassa as chamadas para a API.
/// </summary>
public interface IJourneyApiProxy
{
    /// <summary>
    /// Encaminha a requisição HTTP atual para o path de jornada na API e retorna a resposta.
    /// </summary>
    /// <param name="request">Requisição HTTP atual.</param>
    /// <param name="pathAndQuery">Path relativo (ex: feed/territory-feed) + QueryString já incluso no request.</param>
    Task<HttpResponseMessage> ForwardAsync(
        HttpRequest request,
        string pathAndQuery,
        CancellationToken cancellationToken = default);
}

public sealed class JourneyApiProxy : IJourneyApiProxy
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<BffOptions> _options;

    public JourneyApiProxy(HttpClient httpClient, IOptions<BffOptions> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public async Task<HttpResponseMessage> ForwardAsync(
        HttpRequest request,
        string pathAndQuery,
        CancellationToken cancellationToken = default)
    {
        var baseUrl = _options.Value.ApiBaseUrl?.TrimEnd('/') ?? "";
        var path = pathAndQuery.TrimStart('/');
        var uri = $"{baseUrl}/api/v2/journeys/{path}{request.QueryString}";
        var requestMessage = new HttpRequestMessage(MapMethod(request.Method), uri);

        foreach (var header in request.Headers)
        {
            if (header.Key.Equals("Host", StringComparison.OrdinalIgnoreCase))
                continue;
            if (requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
                continue;
            requestMessage.Content ??= new StreamContent(Stream.Null);
            requestMessage.Content.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
        }

        if (request.ContentLength.GetValueOrDefault() > 0 && request.Body.CanRead)
        {
            if (request.Body.CanSeek)
                request.Body.Position = 0;
            requestMessage.Content = new StreamContent(request.Body);
            if (request.ContentType is not null)
                requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(request.ContentType);
        }

        return await _httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
    }

    private static HttpMethod MapMethod(string method)
    {
        return method.ToUpperInvariant() switch
        {
            "GET" => HttpMethod.Get,
            "POST" => HttpMethod.Post,
            "PUT" => HttpMethod.Put,
            "DELETE" => HttpMethod.Delete,
            "PATCH" => HttpMethod.Patch,
            _ => new HttpMethod(method)
        };
    }
}

public sealed class BffOptions
{
    public const string SectionName = "Bff";
    public string? ApiBaseUrl { get; set; }

    /// <summary>Habilita cache de respostas GET (jornadas).</summary>
    public bool EnableCache { get; set; } = true;

    /// <summary>TTL padrão do cache em segundos (apenas para GET 2xx).</summary>
    public int CacheTtlSeconds { get; set; } = 60;

    /// <summary>TTL por prefixo de path (ex: "feed" => 30, "onboarding" => 300). Sobrescreve CacheTtlSeconds.</summary>
    public Dictionary<string, int>? CacheTtlByPath { get; set; }
}
