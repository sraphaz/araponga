# Configuração Avançada de Mídias por Território - Fase 10

## 🎯 Objetivo

Permitir que cada território configure quais tipos de mídia estão disponíveis e como a plataforma se comporta, usando feature flags e configurações granulares. Também permite que usuários escolham quais tipos de mídia desejam visualizar.

## 📋 Funcionalidades

### 1. Feature Flags de Mídia

Flags para habilitar/desabilitar tipos de mídia por território:

- **MediaImagesEnabled**: Habilita imagens em Posts, Eventos, Marketplace
- **MediaVideosEnabled**: Habilita vídeos em Posts, Eventos, Marketplace
- **MediaAudioEnabled**: Habilita áudios em Posts, Eventos, Marketplace
- **ChatMediaImagesEnabled**: Habilita imagens no Chat
- **ChatMediaAudioEnabled**: Habilita áudios no Chat (mensagens de voz)

### 2. Configuração Avançada por Tipo de Conteúdo

Cada território pode configurar limites específicos para:

#### Posts
- Tipos permitidos (imagens, vídeos, áudios)
- Quantidade máxima de mídias (padrão: 10)
- Quantidade máxima de vídeos (padrão: 1)
- Quantidade máxima de áudios (padrão: 1)
- Tamanho máximo por tipo:
  - Imagens: padrão 10MB
  - Vídeos: padrão 50MB
  - Áudios: padrão 10MB
- Duração máxima (planejado para validação futura):
  - Vídeos: padrão 60s
  - Áudios: padrão 5min

#### Eventos
- Tipos permitidos (imagens, vídeos, áudios)
- Quantidade máxima total (1 capa + 5 adicionais)
- Quantidade máxima de vídeos (padrão: 1)
- Quantidade máxima de áudios (padrão: 1)
- Tamanho máximo:
  - Imagens: padrão 10MB
  - Vídeos: padrão 100MB
  - Áudios: padrão 20MB
- Duração máxima:
  - Vídeos: padrão 120s
  - Áudios: padrão 10min

#### Marketplace (Items)
- Tipos permitidos (imagens, vídeos, áudios)
- Quantidade máxima de mídias (padrão: 10)
- Quantidade máxima de vídeos (padrão: 1)
- Quantidade máxima de áudios (padrão: 1)
- Tamanho máximo:
  - Imagens: padrão 10MB
  - Vídeos: padrão 30MB
  - Áudios: padrão 5MB
- Duração máxima:
  - Vídeos: padrão 30s
  - Áudios: padrão 2min

#### Chat
- Tipos permitidos (imagens, áudios - vídeos sempre bloqueados)
- Tamanho máximo:
  - Imagens: padrão 5MB
  - Áudios: padrão 2MB
- Duração máxima de áudio: padrão 60s (mensagens de voz)

### 3. Preferências do Usuário

Cada usuário pode configurar quais tipos de mídia deseja visualizar:

- **Visualizar Imagens**: on/off (padrão: on)
- **Visualizar Vídeos**: on/off (padrão: on)
- **Visualizar Áudios**: on/off (padrão: on)
- **Auto-play Vídeos**: on/off (padrão: off)
- **Auto-play Áudios**: on/off (padrão: off)

Essas preferências afetam:
- **Feed**: Filtra mídias baseado nas preferências
- **Eventos**: Filtra mídias adicionais
- **Marketplace**: Filtra mídias de items
- **Chat**: Não afeta (sempre mostra o que foi enviado)

## 🔧 Implementação

### 1. Modelo de Configuração

```csharp
public sealed class TerritoryMediaConfig
{
    public Guid TerritoryId { get; set; }
    
    // Posts
    public MediaContentConfig Posts { get; set; } = new();
    
    // Eventos
    public MediaContentConfig Events { get; set; } = new();
    
    // Marketplace
    public MediaContentConfig Marketplace { get; set; } = new();
    
    // Chat
    public MediaChatConfig Chat { get; set; } = new();
}

public sealed class MediaContentConfig
{
    public bool ImagesEnabled { get; set; } = true;
    public bool VideosEnabled { get; set; } = true;
    public bool AudioEnabled { get; set; } = true;
    
    public int MaxMediaCount { get; set; } = 10;
    public int MaxVideoCount { get; set; } = 1;
    public int MaxAudioCount { get; set; } = 1;
    
    public long MaxImageSizeBytes { get; set; } = 10 * 1024 * 1024; // 10MB
    public long MaxVideoSizeBytes { get; set; } = 50 * 1024 * 1024; // 50MB
    public long MaxAudioSizeBytes { get; set; } = 10 * 1024 * 1024; // 10MB
    
    public int? MaxVideoDurationSeconds { get; set; } = null; // Validação futura
    public int? MaxAudioDurationSeconds { get; set; } = null; // Validação futura
}

public sealed class MediaChatConfig
{
    public bool ImagesEnabled { get; set; } = true;
    public bool AudioEnabled { get; set; } = true; // Mensagens de voz
    public bool VideosEnabled { get; set; } = false; // Sempre bloqueado
    
    public long MaxImageSizeBytes { get; set; } = 5 * 1024 * 1024; // 5MB
    public long MaxAudioSizeBytes { get; set; } = 2 * 1024 * 1024; // 2MB
    public int? MaxAudioDurationSeconds { get; set; } = 60; // Mensagens de voz
}

public sealed class UserMediaPreferences
{
    public Guid UserId { get; set; }
    
    public bool ShowImages { get; set; } = true;
    public bool ShowVideos { get; set; } = true;
    public bool ShowAudio { get; set; } = true;
    
    public bool AutoPlayVideos { get; set; } = false;
    public bool AutoPlayAudio { get; set; } = false;
}
```

### 2. Validações Integradas

Os services de criação de conteúdo devem:

1. Verificar feature flags do território
2. Verificar configuração de mídia do território
3. Aplicar limites configurados (tamanho, quantidade, tipo)
4. Retornar erros específicos quando configurações bloqueiam

### 3. Filtragem de Respostas

Os services de listagem devem:

1. Verificar feature flags do território
2. Verificar configuração de mídia do território
3. Verificar preferências do usuário
4. Filtrar mídias não permitidas/configuradas
5. Retornar apenas URLs de mídias permitidas

## 📊 Hierarquia de Configuração

1. **Feature Flags** (nível mais alto): Habilita/desabilita tipos de mídia globalmente no território
2. **TerritoryMediaConfig**: Configura limites e regras específicas por tipo de conteúdo
3. **UserMediaPreferences**: Filtra o que o usuário vê (não afeta criação, apenas visualização)

## 🔒 Segurança

- **Configuração de Território**: Apenas Curators podem modificar
- **Preferências de Usuário**: Cada usuário modifica apenas suas próprias preferências
- **Validação**: Todas as validações são server-side (não confiar em client-side)

## 📝 Endpoints

### Admin/Territory (Curators)

- `GET /api/v1/territories/{territoryId}/media-config`: Obtém configuração atual
- `PUT /api/v1/territories/{territoryId}/media-config`: Atualiza configuração

### User Preferences

- `GET /api/v1/user/media-preferences`: Obtém preferências do usuário autenticado
- `PUT /api/v1/user/media-preferences`: Atualiza preferências do usuário autenticado

## 🎯 Benefícios

1. **Flexibilidade**: Cada território configura conforme suas necessidades
2. **Controle**: Territórios podem limitar uso de recursos (armazenamento, banda)
3. **Experiência Personalizada**: Usuários escolhem o que visualizam
4. **Performance**: Filtragem server-side reduz dados transferidos
5. **Privacidade**: Usuários podem optar por não ver certos tipos de mídia
