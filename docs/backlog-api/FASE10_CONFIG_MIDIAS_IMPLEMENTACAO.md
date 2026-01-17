# Implementação: Configuração Avançada de Mídias por Território

## 📋 Resumo da Implementação

Esta implementação adiciona controle granular de mídias por território usando feature flags e configurações específicas, permitindo que cada território defina quais tipos de mídia estão disponíveis e quais limites aplicar. Também permite que usuários escolham quais tipos de mídia desejam visualizar.

## ✅ O que foi implementado

### 1. Feature Flags Novos
- ✅ `MediaImagesEnabled` (valor: 10)
- ✅ `MediaVideosEnabled` (valor: 11)
- ✅ `MediaAudioEnabled` (valor: 12)
- ✅ `ChatMediaImagesEnabled` (valor: 13)
- ✅ `ChatMediaAudioEnabled` (valor: 14)

### 2. Modelos de Domínio
- ✅ `TerritoryMediaConfig` - Configuração de mídias por território
- ✅ `MediaContentConfig` - Configuração para Posts, Eventos, Marketplace
- ✅ `MediaChatConfig` - Configuração específica para Chat
- ✅ `UserMediaPreferences` - Preferências de visualização do usuário

## 🚧 O que ainda precisa ser implementado

### 1. Repositories e Interfaces

#### ITerritoryMediaConfigRepository
```csharp
public interface ITerritoryMediaConfigRepository
{
    Task<TerritoryMediaConfig?> GetByTerritoryIdAsync(Guid territoryId, CancellationToken cancellationToken);
    Task<TerritoryMediaConfig> GetOrCreateDefaultAsync(Guid territoryId, CancellationToken cancellationToken);
    Task SaveAsync(TerritoryMediaConfig config, CancellationToken cancellationToken);
}
```

#### IUserMediaPreferencesRepository
```csharp
public interface IUserMediaPreferencesRepository
{
    Task<UserMediaPreferences?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<UserMediaPreferences> GetOrCreateDefaultAsync(Guid userId, CancellationToken cancellationToken);
    Task SaveAsync(UserMediaPreferences preferences, CancellationToken cancellationToken);
}
```

### 2. Services

#### TerritoryMediaConfigService
```csharp
public sealed class TerritoryMediaConfigService
{
    // Obter configuração (com valores padrão se não existir)
    Task<TerritoryMediaConfig> GetConfigAsync(Guid territoryId, CancellationToken cancellationToken);
    
    // Atualizar configuração (apenas Curators)
    Task<TerritoryMediaConfig> UpdateConfigAsync(
        Guid territoryId,
        TerritoryMediaConfig config,
        Guid updatedByUserId,
        CancellationToken cancellationToken);
    
    // Validar configuração contra feature flags
    Task<bool> IsMediaTypeEnabledAsync(
        Guid territoryId,
        MediaType mediaType,
        MediaContentType contentType,
        CancellationToken cancellationToken);
}
```

#### UserMediaPreferencesService
```csharp
public sealed class UserMediaPreferencesService
{
    // Obter preferências (com valores padrão se não existir)
    Task<UserMediaPreferences> GetPreferencesAsync(Guid userId, CancellationToken cancellationToken);
    
    // Atualizar preferências (apenas o próprio usuário)
    Task<UserMediaPreferences> UpdatePreferencesAsync(
        Guid userId,
        UserMediaPreferences preferences,
        CancellationToken cancellationToken);
}
```

### 3. Integração nos Services Existentes

#### PostCreationService
- Verificar feature flags antes de criar post com mídia
- Verificar `TerritoryMediaConfig.Posts` para limites
- Validar tipos de mídia permitidos
- Aplicar limites de tamanho/quantidade configurados

#### EventsService
- Similar ao PostCreationService, mas usando `TerritoryMediaConfig.Events`

#### StoreItemService
- Similar ao PostCreationService, mas usando `TerritoryMediaConfig.Marketplace`

#### ChatService
- Similar ao ChatService atual, mas usando `TerritoryMediaConfig.Chat`

#### FeedService
- Filtrar mídias baseado em:
  1. Feature flags do território
  2. TerritoryMediaConfig
  3. UserMediaPreferences

### 4. Controllers e Endpoints

#### MediaConfigController
```csharp
[Route("api/v1/territories/{territoryId:guid}/media-config")]
[Tags("Media Config")]

// GET: Obter configuração atual
[HttpGet]
Task<TerritoryMediaConfigResponse> Get([FromRoute] Guid territoryId);

// PUT: Atualizar configuração (requer Curator)
[HttpPut]
Task<TerritoryMediaConfigResponse> Update(
    [FromRoute] Guid territoryId,
    [FromBody] UpdateTerritoryMediaConfigRequest request);
```

#### UserMediaPreferencesController
```csharp
[Route("api/v1/user/media-preferences")]
[Tags("User Preferences")]

// GET: Obter preferências do usuário autenticado
[HttpGet]
Task<UserMediaPreferencesResponse> Get();

// PUT: Atualizar preferências do usuário autenticado
[HttpPut]
Task<UserMediaPreferencesResponse> Update([FromBody] UpdateUserMediaPreferencesRequest request);
```

### 5. Filtragem de Respostas

#### FeedService
```csharp
// Ao buscar posts, filtrar MediaUrls baseado em:
1. Feature flags habilitados
2. TerritoryMediaConfig (tipos permitidos)
3. UserMediaPreferences (o que o usuário quer ver)
```

#### EventsService
```csharp
// Ao buscar eventos, filtrar CoverImageUrl e AdditionalImageUrls
```

#### MarketplaceService
```csharp
// Ao buscar items, filtrar ImageUrls e PrimaryImageUrl
```

## 📊 Fluxo de Validação

### Criação de Conteúdo com Mídia

1. **Feature Flags**: Verificar se tipo de mídia está habilitado no território
2. **TerritoryMediaConfig**: Verificar se tipo está habilitado para o conteúdo específico
3. **Limites**: Aplicar limites configurados (quantidade, tamanho)
4. **Validação**: Retornar erro específico se configuração bloquear

### Visualização de Conteúdo com Mídia

1. **Feature Flags**: Verificar se tipo de mídia está habilitado no território
2. **TerritoryMediaConfig**: Verificar se tipo está habilitado para o conteúdo específico
3. **UserMediaPreferences**: Filtrar mídias que o usuário não quer ver
4. **Resposta**: Retornar apenas URLs de mídias permitidas/configuradas

## 🎯 Prioridades de Implementação

### Fase 1: Infraestrutura (P0)
1. Criar repositories e interfaces
2. Implementar services básicos
3. Criar endpoints de API

### Fase 2: Integração (P1)
1. Integrar validações em PostCreationService
2. Integrar validações em EventsService
3. Integrar validações em StoreItemService
4. Integrar validações em ChatService

### Fase 3: Filtragem (P2)
1. Implementar filtragem no FeedService
2. Implementar filtragem no EventsService (listagem)
3. Implementar filtragem no MarketplaceService

### Fase 4: Documentação (P3)
1. Atualizar DevPortal
2. Atualizar documentação técnica
3. Criar exemplos de uso

## 🔒 Segurança

- **TerritoryMediaConfig**: Apenas Curators podem modificar
- **UserMediaPreferences**: Cada usuário modifica apenas suas próprias preferências
- **Validação**: Todas as validações são server-side
- **Feature Flags**: Continuam sendo gerenciados por Curators via FeaturesController

## 📝 Valores Padrão

Se um território não tiver configuração, os valores padrão são aplicados:

- **Posts**: 10 mídias, 1 vídeo (50MB, 60s), 1 áudio (10MB, 5min)
- **Eventos**: 1 capa + 5 adicionais, 1 vídeo (100MB, 120s), 1 áudio (20MB, 10min)
- **Items**: 10 mídias, 1 vídeo (30MB, 30s), 1 áudio (5MB, 2min)
- **Chat**: Imagens (5MB), Áudios (2MB, 60s), Vídeos (nunca)

## 🎉 Benefícios

1. **Flexibilidade**: Cada território configura conforme suas necessidades
2. **Controle de Recursos**: Limitar uso de armazenamento e banda
3. **Experiência Personalizada**: Usuários escolhem o que visualizam
4. **Performance**: Filtragem server-side reduz dados transferidos
5. **Privacidade**: Usuários podem optar por não ver certos tipos de mídia
