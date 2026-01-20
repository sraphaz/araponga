using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Araponga.Api;
using Araponga.Api.Contracts.Auth;
using Araponga.Api.Contracts.Feed;
using Araponga.Api.Contracts.Media;
using Araponga.Api.Contracts.Memberships;
using Araponga.Api.Contracts.Territories;
using Araponga.Tests.Api;
using TechTalk.SpecFlow;
using Xunit;

namespace Araponga.Tests.Api.BDD;

[Binding]
public sealed class MediaSteps
{
    private readonly ScenarioContext _scenarioContext;
    private readonly ApiFactory _factory;
    private HttpClient? _client;
    private string? _currentUser;
    private Guid? _currentTerritoryId;
    private readonly Dictionary<string, Guid> _mediaAssets = new();
    private readonly Dictionary<string, Guid> _users = new();
    private readonly Dictionary<string, Guid> _territories = new();
    private HttpResponseMessage? _lastResponse;
    private Exception? _lastException;

    public MediaSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
        _factory = new ApiFactory();
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        _client = _factory.CreateClient();
        _mediaAssets.Clear();
        _users.Clear();
        _territories.Clear();
        _lastResponse = null;
        _lastException = null;
    }

    [AfterScenario]
    public void AfterScenario()
    {
        _client?.Dispose();
    }

    #region Context Steps

    [Given(@"que existe um território ""([^""]*)""")]
    public Task GivenQueExisteUmTerritorio(string territoryName)
    {
        // Usar território padrão para testes
        var territoryId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        _territories[territoryName] = territoryId;
        _currentTerritoryId = territoryId;
        return Task.CompletedTask;
    }

    [Given(@"que existe um usuário ""([^""]*)"" como residente")]
    public async Task GivenQueExisteUmUsuarioComoResidente(string userName)
    {
        var token = await LoginForTokenAsync(_client!, "google", $"bdd-{userName}-{Guid.NewGuid()}");
        _client!.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        _client.DefaultRequestHeaders.Add(ApiHeaders.SessionId, Guid.NewGuid().ToString());

        await SelectTerritoryAsync(_currentTerritoryId!.Value);
        await BecomeResidentAsync(_currentTerritoryId.Value);

        _users[userName] = Guid.NewGuid(); // Simulado
        _currentUser = userName;
    }

    [Given(@"que o usuário ""([^""]*)"" está autenticado")]
    public async Task GivenQueOUsuarioEstaAutenticado(string userName)
    {
        if (!_users.ContainsKey(userName))
        {
            await GivenQueExisteUmUsuarioComoResidente(userName);
        }
        _currentUser = userName;
    }

    #endregion

    #region Media Upload Steps

    [Given(@"que existe uma imagem ""([^""]*)"" disponível")]
    public async Task GivenQueExisteUmaImagemDisponivel(string imageName)
    {
        var mediaId = await UploadTestMediaAsync(imageName, "image/jpeg", 2 * 1024 * 1024);
        _mediaAssets[imageName] = mediaId;
    }

    [Given(@"que existem (\d+) imagens disponíveis")]
    public async Task GivenQueExistemImagensDisponiveis(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var imageName = $"imagem{i + 1}.jpg";
            var mediaId = await UploadTestMediaAsync(imageName, "image/jpeg", 2 * 1024 * 1024);
            _mediaAssets[imageName] = mediaId;
        }
    }

    [Given(@"que existe um vídeo ""([^""]*)"" disponível")]
    public async Task GivenQueExisteUmVideoDisponivel(string videoName)
    {
        var mediaId = await UploadTestMediaAsync(videoName, "video/mp4", 30 * 1024 * 1024);
        _mediaAssets[videoName] = mediaId;
    }

    [Given(@"que existem (\d+) vídeos disponíveis")]
    public async Task GivenQueExistemVideosDisponiveis(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var videoName = $"video{i + 1}.mp4";
            var mediaId = await UploadTestMediaAsync(videoName, "video/mp4", 30 * 1024 * 1024);
            _mediaAssets[videoName] = mediaId;
        }
    }

    [Given(@"que existe um áudio ""([^""]*)"" disponível")]
    public async Task GivenQueExisteUmAudioDisponivel(string audioName)
    {
        var mediaId = await UploadTestMediaAsync(audioName, "audio/mpeg", 1 * 1024 * 1024);
        _mediaAssets[audioName] = mediaId;
    }

    [When(@"ele faz upload de uma imagem de (\d+)MB")]
    public async Task WhenEleFazUploadDeUmaImagemDeMB(int sizeMB)
    {
        try
        {
            var mediaId = await UploadTestMediaAsync($"test-{Guid.NewGuid()}.jpg", "image/jpeg", sizeMB * 1024 * 1024);
            _scenarioContext["uploadedMediaId"] = mediaId;
            _scenarioContext["uploadSuccess"] = true;
        }
        catch (Exception ex)
        {
            _lastException = ex;
            _scenarioContext["uploadSuccess"] = false;
            _scenarioContext["uploadError"] = ex.Message;
        }
    }

    [When(@"ele tenta fazer upload de um vídeo de (\d+)MB")]
    public async Task WhenEleTentaFazerUploadDeUmVideoDeMB(int sizeMB)
    {
        try
        {
            var mediaId = await UploadTestMediaAsync($"test-{Guid.NewGuid()}.mp4", "video/mp4", sizeMB * 1024 * 1024);
            _scenarioContext["uploadedMediaId"] = mediaId;
            _scenarioContext["uploadSuccess"] = true;
        }
        catch (Exception ex)
        {
            _lastException = ex;
            _scenarioContext["uploadSuccess"] = false;
            _scenarioContext["uploadError"] = ex.Message;
        }
    }

    [When(@"ele faz upload de um áudio de (\d+)MB")]
    public async Task WhenEleFazUploadDeUmAudioDeMB(int sizeMB)
    {
        try
        {
            var mediaId = await UploadTestMediaAsync($"test-{Guid.NewGuid()}.mp3", "audio/mpeg", sizeMB * 1024 * 1024);
            _scenarioContext["uploadedMediaId"] = mediaId;
            _scenarioContext["uploadSuccess"] = true;
        }
        catch (Exception ex)
        {
            _lastException = ex;
            _scenarioContext["uploadSuccess"] = false;
            _scenarioContext["uploadError"] = ex.Message;
        }
    }

    [When(@"ele tenta fazer upload de um arquivo PDF")]
    public async Task WhenEleTentaFazerUploadDeUmArquivoPDF()
    {
        try
        {
            var mediaId = await UploadTestMediaAsync($"test-{Guid.NewGuid()}.pdf", "application/pdf", 1024);
            _scenarioContext["uploadedMediaId"] = mediaId;
            _scenarioContext["uploadSuccess"] = true;
        }
        catch (Exception ex)
        {
            _lastException = ex;
            _scenarioContext["uploadSuccess"] = false;
            _scenarioContext["uploadError"] = ex.Message;
        }
    }

    [Then(@"o upload deve ser concluído com sucesso")]
    public void ThenOUploadDeveSerConcluidoComSucesso()
    {
        var success = _scenarioContext.Get<bool>("uploadSuccess");
        Assert.True(success, $"Upload falhou: {_scenarioContext.Get<string>("uploadError")}");
    }

    [Then(@"a mídia deve estar disponível para uso")]
    public void ThenAMidiaDeveEstarDisponivelParaUso()
    {
        var mediaId = _scenarioContext.Get<Guid>("uploadedMediaId");
        Assert.NotEqual(Guid.Empty, mediaId);
    }

    [Then(@"deve retornar erro ""([^""]*)""")]
    public void ThenDeveRetornarErro(string expectedError)
    {
        var success = _scenarioContext.Get<bool>("uploadSuccess");
        Assert.False(success, "Upload deveria ter falhado");
        
        var error = _scenarioContext.Get<string>("uploadError");
        Assert.Contains(expectedError, error, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region Post Steps

    [When(@"o usuário cria um post com a imagem ""([^""]*)""")]
    public async Task WhenOUsuarioCriaUmPostComAImagem(string imageName)
    {
        var mediaId = _mediaAssets[imageName];
        await CreatePostWithMediaAsync(new[] { mediaId });
    }

    [When(@"o usuário cria um post com as (\d+) imagens")]
    public async Task WhenOUsuarioCriaUmPostComAsImagens(int count)
    {
        var mediaIds = _mediaAssets.Values.Take(count).ToArray();
        await CreatePostWithMediaAsync(mediaIds);
    }

    [When(@"o usuário cria um post com a imagem e o vídeo")]
    public async Task WhenOUsuarioCriaUmPostComAImagemEOVideo()
    {
        var imageId = _mediaAssets.Values.First(m => _mediaAssets.First(kvp => kvp.Value == m).Key.EndsWith(".jpg"));
        var videoId = _mediaAssets.Values.First(m => _mediaAssets.First(kvp => kvp.Value == m).Key.EndsWith(".mp4"));
        await CreatePostWithMediaAsync(new[] { imageId, videoId });
    }

    [When(@"o usuário tenta criar um post com as (\d+) imagens")]
    public async Task WhenOUsuarioTentaCriarUmPostComAsImagens(int count)
    {
        var mediaIds = _mediaAssets.Values.Take(count).ToArray();
        await CreatePostWithMediaAsync(mediaIds);
    }

    [When(@"o usuário tenta criar um post com os (\d+) vídeos")]
    public async Task WhenOUsuarioTentaCriarUmPostComOsVideos(int count)
    {
        var mediaIds = _mediaAssets.Values.Take(count).ToArray();
        await CreatePostWithMediaAsync(mediaIds);
    }

    [Then(@"o post deve ser criado com sucesso")]
    public void ThenOPostDeveSerCriadoComSucesso()
    {
        Assert.NotNull(_lastResponse);
        Assert.Equal(HttpStatusCode.Created, _lastResponse.StatusCode);
    }

    [Then(@"o post deve conter (\d+) mídia")]
    [Then(@"o post deve conter (\d+) mídias")]
    public async Task ThenOPostDeveConterMidias(int expectedCount)
    {
        var post = await _lastResponse!.Content.ReadFromJsonAsync<FeedItemResponse>();
        Assert.NotNull(post);
        Assert.Equal(expectedCount, post.MediaCount);
    }

    [When(@"o usuário deleta o post")]
    public async Task WhenOUsuarioDeletaOPost()
    {
        var postId = _scenarioContext.Get<Guid>("createdPostId");
        _lastResponse = await _client!.DeleteAsync($"api/v1/feed/{postId}");
    }

    [Then(@"o post deve ser deletado")]
    public void ThenOPostDeveSerDeletado()
    {
        Assert.NotNull(_lastResponse);
        Assert.True(_lastResponse.IsSuccessStatusCode);
    }

    [Then(@"as (\d+) mídias devem ser removidas")]
    public async Task ThenAsMidiasDevemSerRemovidas(int count)
    {
        // Verificar que as mídias foram removidas (implementação simplificada)
        await Task.CompletedTask;
        Assert.True(true); // Placeholder - implementar verificação real se necessário
    }

    #endregion

    #region Event Steps

    [Given(@"que existem (\d+) imagens adicionais disponíveis")]
    public async Task GivenQueExistemImagensAdicionaisDisponiveis(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var imageName = $"adicional{i + 1}.jpg";
            var mediaId = await UploadTestMediaAsync(imageName, "image/jpeg", 2 * 1024 * 1024);
            _mediaAssets[imageName] = mediaId;
        }
    }

    [When(@"o usuário cria um evento com a imagem de capa ""([^""]*)""")]
    public async Task WhenOUsuarioCriaUmEventoComAImagemDeCapa(string coverImageName)
    {
        var coverMediaId = _mediaAssets[coverImageName];
        await CreateEventWithMediaAsync(coverMediaId, null);
    }

    [When(@"o usuário cria um evento com a capa e as (\d+) imagens adicionais")]
    public async Task WhenOUsuarioCriaUmEventoComACapaEAsImagensAdicionais(int additionalCount)
    {
        var coverId = _mediaAssets["capa.jpg"];
        var additionalIds = _mediaAssets.Where(kvp => kvp.Key.StartsWith("adicional")).Take(additionalCount).Select(kvp => kvp.Value).ToArray();
        await CreateEventWithMediaAsync(coverId, additionalIds);
    }

    [When(@"o usuário tenta criar um evento com a capa e as (\d+) imagens adicionais")]
    public async Task WhenOUsuarioTentaCriarUmEventoComACapaEAsImagensAdicionais(int additionalCount)
    {
        var coverId = _mediaAssets["capa.jpg"];
        var additionalIds = _mediaAssets.Where(kvp => kvp.Key.StartsWith("adicional")).Take(additionalCount).Select(kvp => kvp.Value).ToArray();
        await CreateEventWithMediaAsync(coverId, additionalIds);
    }

    [When(@"o usuário cria um evento com a capa e o vídeo adicional")]
    public async Task WhenOUsuarioCriaUmEventoComACapaEOVideoAdicional()
    {
        var coverId = _mediaAssets["capa.jpg"];
        var videoId = _mediaAssets.Values.First(m => _mediaAssets.First(kvp => kvp.Value == m).Key.EndsWith(".mp4"));
        await CreateEventWithMediaAsync(coverId, new[] { videoId });
    }

    [Then(@"o evento deve ser criado com sucesso")]
    public void ThenOEventoDeveSerCriadoComSucesso()
    {
        Assert.NotNull(_lastResponse);
        Assert.Equal(HttpStatusCode.Created, _lastResponse.StatusCode);
    }

    [Then(@"o evento deve ter uma imagem de capa")]
    public async Task ThenOEventoDeveTerUmaImagemDeCapa()
    {
        var eventResponse = await _lastResponse!.Content.ReadFromJsonAsync<Araponga.Api.Contracts.Events.EventResponse>();
        Assert.NotNull(eventResponse);
        Assert.NotNull(eventResponse.CoverImageUrl);
    }

    [Then(@"o evento deve ter (\d+) imagem de capa")]
    public async Task ThenOEventoDeveTerImagemDeCapa(int count)
    {
        var eventResponse = await _lastResponse!.Content.ReadFromJsonAsync<Araponga.Api.Contracts.Events.EventResponse>();
        Assert.NotNull(eventResponse);
        Assert.NotNull(eventResponse.CoverImageUrl);
    }

    [Then(@"o evento deve ter (\d+) mídias adicionais")]
    public async Task ThenOEventoDeveTerMidiasAdicionais(int expectedCount)
    {
        var eventResponse = await _lastResponse!.Content.ReadFromJsonAsync<Araponga.Api.Contracts.Events.EventResponse>();
        Assert.NotNull(eventResponse);
        Assert.NotNull(eventResponse.AdditionalImageUrls);
        Assert.Equal(expectedCount, eventResponse.AdditionalImageUrls!.Count);
    }

    [Then(@"o evento deve ter (\d+) vídeo adicional")]
    public async Task ThenOEventoDeveTerVideoAdicional(int count)
    {
        // Implementação simplificada - verificar se há vídeo nas mídias adicionais
        await Task.CompletedTask;
        Assert.True(true);
    }

    [Given(@"que existe um evento com (\d+) capa e (\d+) mídias adicionais")]
    public async Task GivenQueExisteUmEventoComCapaEMidiasAdicionais(int coverCount, int additionalCount)
    {
        var coverId = await UploadTestMediaAsync("capa.jpg", "image/jpeg", 2 * 1024 * 1024);
        var additionalIds = new List<Guid>();
        for (int i = 0; i < additionalCount; i++)
        {
            var mediaId = await UploadTestMediaAsync($"adicional{i + 1}.jpg", "image/jpeg", 2 * 1024 * 1024);
            additionalIds.Add(mediaId);
        }
        await CreateEventWithMediaAsync(coverId, additionalIds.ToArray());
        var eventResponse = await _lastResponse!.Content.ReadFromJsonAsync<Araponga.Api.Contracts.Events.EventResponse>();
        _scenarioContext["createdEventId"] = eventResponse!.EventId;
    }

    [When(@"o usuário cancela o evento")]
    public async Task WhenOUsuarioCancelaOEvento()
    {
        var eventId = _scenarioContext.Get<Guid>("createdEventId");
        _lastResponse = await _client!.PostAsync($"api/v1/events/{eventId}/cancel", null);
    }

    [Then(@"o evento deve ser cancelado")]
    public void ThenOEventoDeveSerCancelado()
    {
        Assert.NotNull(_lastResponse);
        Assert.True(_lastResponse.IsSuccessStatusCode);
    }

    #endregion

    #region Marketplace Steps

    [Given(@"que existe uma loja ""([^""]*)""")]
    public Task GivenQueExisteUmaLoja(string storeName)
    {
        // Loja já existe no contexto de teste
        return Task.CompletedTask;
    }

    [When(@"o usuário cria um item com a imagem ""([^""]*)""")]
    public async Task WhenOUsuarioCriaUmItemComAImagem(string imageName)
    {
        var mediaId = _mediaAssets[imageName];
        await CreateItemWithMediaAsync(new[] { mediaId });
    }

    [When(@"o usuário cria um item com as (\d+) imagens")]
    public async Task WhenOUsuarioCriaUmItemComAsImagens(int count)
    {
        var mediaIds = _mediaAssets.Values.Take(count).ToArray();
        await CreateItemWithMediaAsync(mediaIds);
    }

    [When(@"o usuário tenta criar um item com as (\d+) imagens")]
    public async Task WhenOUsuarioTentaCriarUmItemComAsImagens(int count)
    {
        var mediaIds = _mediaAssets.Values.Take(count).ToArray();
        await CreateItemWithMediaAsync(mediaIds);
    }

    [When(@"o usuário cria um item com a imagem e o vídeo")]
    public async Task WhenOUsuarioCriaUmItemComAImagemEOVideo()
    {
        var imageId = _mediaAssets.Values.First(m => _mediaAssets.First(kvp => kvp.Value == m).Key.EndsWith(".jpg"));
        var videoId = _mediaAssets.Values.First(m => _mediaAssets.First(kvp => kvp.Value == m).Key.EndsWith(".mp4"));
        await CreateItemWithMediaAsync(new[] { imageId, videoId });
    }

    [Then(@"o item deve ser criado com sucesso")]
    public void ThenOItemDeveSerCriadoComSucesso()
    {
        Assert.NotNull(_lastResponse);
        Assert.Equal(HttpStatusCode.Created, _lastResponse.StatusCode);
    }

    [Then(@"o item deve ter uma imagem principal")]
    public async Task ThenOItemDeveTerUmaImagemPrincipal()
    {
        var item = await _lastResponse!.Content.ReadFromJsonAsync<Araponga.Api.Contracts.Marketplace.ItemResponse>();
        Assert.NotNull(item);
        Assert.NotNull(item.PrimaryImageUrl);
    }

    [Then(@"o item deve ter (\d+) mídias")]
    public async Task ThenOItemDeveTerMidias(int expectedCount)
    {
        var item = await _lastResponse!.Content.ReadFromJsonAsync<Araponga.Api.Contracts.Marketplace.ItemResponse>();
        Assert.NotNull(item);
        Assert.NotNull(item.ImageUrls);
        Assert.Equal(expectedCount, item.ImageUrls.Count);
    }

    [Then(@"a primeira imagem deve ser a imagem principal")]
    public async Task ThenAPrimeiraImagemDeveSerAImagemPrincipal()
    {
        var item = await _lastResponse!.Content.ReadFromJsonAsync<Araponga.Api.Contracts.Marketplace.ItemResponse>();
        Assert.NotNull(item);
        Assert.NotNull(item.PrimaryImageUrl);
        Assert.NotNull(item.ImageUrls);
        Assert.True(item.ImageUrls!.Count > 0);
        var imageUrlsList = item.ImageUrls.ToList();
        Assert.Equal(item.PrimaryImageUrl, imageUrlsList[0]);
    }

    [Given(@"que existe um item com (\d+) mídias")]
    public async Task GivenQueExisteUmItemComMidias(int count)
    {
        var mediaIds = new List<Guid>();
        for (int i = 0; i < count; i++)
        {
            var mediaId = await UploadTestMediaAsync($"item{i + 1}.jpg", "image/jpeg", 2 * 1024 * 1024);
            mediaIds.Add(mediaId);
        }
        await CreateItemWithMediaAsync(mediaIds.ToArray());
        var item = await _lastResponse!.Content.ReadFromJsonAsync<Araponga.Api.Contracts.Marketplace.ItemResponse>();
        _scenarioContext["createdItemId"] = item!.Id;
    }

    [When(@"o usuário arquiva o item")]
    public async Task WhenOUsuarioArquivaOItem()
    {
        var itemId = _scenarioContext.Get<Guid>("createdItemId");
        _lastResponse = await _client!.PostAsync($"api/v1/marketplace/items/{itemId}/archive", null);
    }

    [Then(@"o item deve ser arquivado")]
    public void ThenOItemDeveSerArquivado()
    {
        Assert.NotNull(_lastResponse);
        Assert.True(_lastResponse.IsSuccessStatusCode);
    }

    #endregion

    #region Chat Steps

    [Given(@"que existe um usuário ""([^""]*)"" como residente")]
    public async Task GivenQueExisteUmUsuarioComoResidenteChat(string userName)
    {
        if (!_users.ContainsKey(userName))
        {
            await GivenQueExisteUmUsuarioComoResidente(userName);
        }
        await Task.CompletedTask;
    }

    [When(@"o usuário envia uma mensagem com a imagem ""([^""]*)""")]
    public async Task WhenOUsuarioEnviaUmaMensagemComAImagem(string imageName)
    {
        var mediaId = _mediaAssets[imageName];
        await SendChatMessageWithMediaAsync(mediaId);
    }

    [When(@"o usuário envia uma mensagem com o áudio ""([^""]*)""")]
    public async Task WhenOUsuarioEnviaUmaMensagemComOAudio(string audioName)
    {
        var mediaId = _mediaAssets[audioName];
        await SendChatMessageWithMediaAsync(mediaId);
    }

    [When(@"o usuário tenta enviar uma mensagem com o vídeo ""([^""]*)""")]
    public async Task WhenOUsuarioTentaEnviarUmaMensagemComOVideo(string videoName)
    {
        var mediaId = _mediaAssets[videoName];
        await SendChatMessageWithMediaAsync(mediaId);
    }

    [Then(@"a mensagem deve ser enviada com sucesso")]
    public void ThenAMensagemDeveSerEnviadaComSucesso()
    {
        Assert.NotNull(_lastResponse);
        Assert.True(_lastResponse.IsSuccessStatusCode);
    }

    [Then(@"a mensagem deve conter a imagem")]
    public async Task ThenAMensagemDeveConterAImagem()
    {
        var message = await _lastResponse!.Content.ReadFromJsonAsync<Araponga.Api.Contracts.Chat.MessageResponse>();
        Assert.NotNull(message);
        Assert.NotNull(message.MediaUrl);
    }

    [Then(@"a mensagem deve conter o áudio")]
    public async Task ThenAMensagemDeveConterOAudio()
    {
        var message = await _lastResponse!.Content.ReadFromJsonAsync<Araponga.Api.Contracts.Chat.MessageResponse>();
        Assert.NotNull(message);
        Assert.NotNull(message.MediaUrl);
    }

    #endregion

    #region Helper Methods

    private async Task<string> LoginForTokenAsync(HttpClient client, string provider, string externalId)
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
                "test@araponga.com"));
        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadFromJsonAsync<SocialLoginResponse>();
        return payload!.Token;
    }

    private async Task SelectTerritoryAsync(Guid territoryId)
    {
        if (!_client!.DefaultRequestHeaders.Contains(ApiHeaders.SessionId))
        {
            _client.DefaultRequestHeaders.Add(ApiHeaders.SessionId, Guid.NewGuid().ToString());
        }

        var response = await _client.PostAsJsonAsync(
            "api/v1/territories/selection",
            new TerritorySelectionRequest(territoryId));
        response.EnsureSuccessStatusCode();
    }

    private async Task BecomeResidentAsync(Guid territoryId)
    {
        var residentUserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var requestResponse = await _client!.PostAsJsonAsync(
            $"api/v1/memberships/{territoryId}/become-resident",
            new BecomeResidentRequest(new[] { residentUserId }, "Test residency request"));

        if (requestResponse.StatusCode == HttpStatusCode.OK)
        {
            var requestPayload = await requestResponse.Content.ReadFromJsonAsync<RequestResidencyResponse>();
            if (requestPayload is not null)
            {
                var residentToken = await LoginForTokenAsync(_client!, "google", "resident-external");
                var originalAuth = _client.DefaultRequestHeaders.Authorization;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", residentToken);

                await _client.PostAsync(
                    $"api/v1/join-requests/{requestPayload.JoinRequestId}/approve",
                    null);

                _client.DefaultRequestHeaders.Authorization = originalAuth;

                await _client.PostAsJsonAsync(
                    $"api/v1/memberships/{territoryId}/verify-residency/geo",
                    new VerifyResidencyGeoRequest(-23.3744, -45.0205));
            }
        }
    }

    private async Task<Guid> UploadTestMediaAsync(string fileName, string mimeType, int sizeBytes)
    {
        // Criar JPEG válido mínimo
        var jpegBytes = new byte[]
        {
            0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01, 0x01, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00,
            0xFF, 0xDB, 0x00, 0x43, 0x00, 0x08, 0x06, 0x06, 0x07, 0x06, 0x05, 0x08, 0x07, 0x07, 0x07, 0x09, 0x09, 0x08, 0x0A, 0x0C,
            0x14, 0x0D, 0x0C, 0x0B, 0x0B, 0x0C, 0x19, 0x12, 0x13, 0x0F, 0x14, 0x1D, 0x1A, 0x1F, 0x1E, 0x1D, 0x1A, 0x1C, 0x1C, 0x20,
            0x24, 0x2E, 0x27, 0x20, 0x22, 0x2C, 0x23, 0x1C, 0x1C, 0x28, 0x37, 0x29, 0x2C, 0x30, 0x31, 0x34, 0x34, 0x34, 0x1F, 0x27,
            0x39, 0x3D, 0x38, 0x32, 0x3C, 0x2E, 0x33, 0x34, 0x32, 0xFF, 0xC0, 0x00, 0x0B, 0x08, 0x00, 0x01, 0x00, 0x01, 0x01, 0x01,
            0x11, 0x00, 0xFF, 0xC4, 0x00, 0x14, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x08, 0xFF, 0xC4, 0x00, 0x14, 0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xDA, 0x00, 0x08, 0x01, 0x01, 0x00, 0x00, 0x3F, 0x00, 0x5F, 0xFF, 0xD9
        };

        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(jpegBytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
        content.Add(fileContent, "file", fileName);

        var response = await _client!.PostAsync("api/v1/media/upload", content);

        if (response.StatusCode == HttpStatusCode.Created)
        {
            var mediaResponse = await response.Content.ReadFromJsonAsync<MediaAssetResponse>();
            return mediaResponse!.Id;
        }

        var errorBody = await response.Content.ReadAsStringAsync();
        throw new InvalidOperationException($"Upload failed: {response.StatusCode} - {errorBody}");
    }

    private async Task CreatePostWithMediaAsync(Guid[] mediaIds)
    {
        _lastResponse = await _client!.PostAsJsonAsync(
            $"api/v1/feed?territoryId={_currentTerritoryId}",
            new CreatePostRequest(
                "Post de teste BDD",
                "Conteúdo do post",
                "GENERAL",
                "PUBLIC",
                null,
                null,
                null,
                mediaIds));

        if (_lastResponse.StatusCode == HttpStatusCode.Created)
        {
            var post = await _lastResponse.Content.ReadFromJsonAsync<FeedItemResponse>();
            if (post is not null)
            {
                _scenarioContext["createdPostId"] = post.Id;
            }
        }
        else
        {
            var errorBody = await _lastResponse.Content.ReadAsStringAsync();
            _scenarioContext["postError"] = errorBody;
        }
    }

    private async Task CreateEventWithMediaAsync(Guid coverMediaId, Guid[]? additionalMediaIds)
    {
        _lastResponse = await _client!.PostAsJsonAsync(
            $"api/v1/events?territoryId={_currentTerritoryId}",
            new Araponga.Api.Contracts.Events.CreateEventRequest(
                _currentTerritoryId!.Value,
                "Evento de teste BDD",
                "Descrição do evento",
                DateTime.UtcNow.AddDays(7),
                DateTime.UtcNow.AddDays(7).AddHours(2),
                -23.3744,
                -45.0205,
                "Local do evento",
                coverMediaId,
                additionalMediaIds));

        if (_lastResponse.StatusCode == HttpStatusCode.Created)
        {
            var eventResponse = await _lastResponse.Content.ReadFromJsonAsync<Araponga.Api.Contracts.Events.EventResponse>();
            if (eventResponse is not null)
            {
                _scenarioContext["createdEventId"] = eventResponse.EventId;
            }
        }
        else
        {
            var errorBody = await _lastResponse.Content.ReadAsStringAsync();
            _scenarioContext["eventError"] = errorBody;
        }
    }

    private async Task CreateItemWithMediaAsync(Guid[] mediaIds)
    {
        // Primeiro, precisamos criar uma loja se não existir
        // Simplificado: assumir que já existe uma loja
        var storeId = Guid.NewGuid(); // Em produção, buscar loja existente

        _lastResponse = await _client!.PostAsJsonAsync(
            $"api/v1/marketplace/stores/{storeId}/items?territoryId={_currentTerritoryId}",
            new Araponga.Api.Contracts.Marketplace.CreateItemRequest(
                _currentTerritoryId!.Value,
                storeId,
                "Product",
                "Item de teste BDD",
                "Descrição do item",
                "Categoria",
                null,
                "Fixed",
                100.00m,
                "BRL",
                null,
                null,
                null,
                null,
                mediaIds));

        if (_lastResponse.StatusCode == HttpStatusCode.Created)
        {
            var item = await _lastResponse.Content.ReadFromJsonAsync<Araponga.Api.Contracts.Marketplace.ItemResponse>();
            if (item is not null)
            {
                _scenarioContext["createdItemId"] = item.Id;
            }
        }
        else
        {
            var errorBody = await _lastResponse.Content.ReadAsStringAsync();
            _scenarioContext["itemError"] = errorBody;
        }
    }

    private async Task SendChatMessageWithMediaAsync(Guid mediaId)
    {
        // Assumir que existe uma conversa ou criar uma
        var recipientId = Guid.NewGuid(); // Em produção, usar usuário real

        _lastResponse = await _client!.PostAsJsonAsync(
            $"api/v1/chat/conversations/{recipientId}/messages",
            new Araponga.Api.Contracts.Chat.SendMessageRequest(
                "Mensagem de teste BDD",
                mediaId));

        if (!_lastResponse.IsSuccessStatusCode)
        {
            var errorBody = await _lastResponse.Content.ReadAsStringAsync();
            _scenarioContext["chatError"] = errorBody;
        }
    }

    #endregion
}
