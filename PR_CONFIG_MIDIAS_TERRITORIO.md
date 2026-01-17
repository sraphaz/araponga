# PR: Configuração Avançada de Mídias por Território + Atualização Documentação

## 📋 Resumo

Este PR implementa a **configuração avançada de mídias por território**, permitindo que curadores controlem granularmente quais tipos de mídia (imagens, vídeos, áudios) são permitidos em cada território, com limites personalizáveis por tipo de conteúdo (Posts, Eventos, Marketplace, Chat). Também adiciona **preferências do usuário** para controlar visualização de mídias.

Além disso, atualiza toda a **documentação Flutter e estratégica** para refletir o suporte completo a **vídeos e áudios** em todos os tipos de conteúdo.

---

## 🎯 Objetivos

1. ✅ Permitir controle granular de mídias por território via feature flags
2. ✅ Permitir configuração de limites personalizados por tipo de conteúdo
3. ✅ Permitir preferências do usuário para visualização de mídias
4. ✅ Atualizar documentação para refletir suporte a vídeos e áudios

---

## 🚀 Mudanças Implementadas

### 1. Feature Flags Novos

- `MediaImagesEnabled` (valor: 10) - Controla se imagens são permitidas
- `MediaVideosEnabled` (valor: 11) - Controla se vídeos são permitidos
- `MediaAudioEnabled` (valor: 12) - Controla se áudios são permitidos
- `ChatMediaImagesEnabled` (valor: 13) - Controla se imagens são permitidas em chat
- `ChatMediaAudioEnabled` (valor: 14) - Controla se áudios são permitidos em chat

### 2. Modelos de Domínio

- `TerritoryMediaConfig` - Configuração de mídias por território
- `MediaContentConfig` - Configuração para Posts, Eventos, Marketplace
- `MediaChatConfig` - Configuração específica para Chat
- `UserMediaPreferences` - Preferências de visualização do usuário

### 3. Repositories e Services

**Interfaces**:
- `ITerritoryMediaConfigRepository`
- `IUserMediaPreferencesRepository`

**Implementações InMemory**:
- `InMemoryTerritoryMediaConfigRepository`
- `InMemoryUserMediaPreferencesRepository`

**Services**:
- `TerritoryMediaConfigService` - Gerencia configurações de mídia
- `UserMediaPreferencesService` - Gerencia preferências do usuário

### 4. Endpoints de API

**Configuração de Mídias por Território** (requer Curator):
- `GET /api/v1/territories/{territoryId}/media-config` - Obter configuração
- `PUT /api/v1/territories/{territoryId}/media-config` - Atualizar configuração

**Preferências do Usuário**:
- `GET /api/v1/user/media-preferences` - Obter preferências
- `PUT /api/v1/user/media-preferences` - Atualizar preferências

### 5. Contracts (API)

- `TerritoryMediaConfigResponse`
- `UpdateTerritoryMediaConfigRequest`
- `UserMediaPreferencesResponse`
- `UpdateUserMediaPreferencesRequest`

### 6. Documentação Atualizada

**Documentos Estratégicos**:
- ✅ `34_FLUTTER_API_STRATEGIC_ALIGNMENT.md` - Status atualizado (Fase 1 e Fase 3 completas)
- ✅ `35_PRIORIZACAO_ESTRATEGICA_API_FRONTEND.md` - Fase 3 atualizada com vídeos e áudios

**Documentação Técnica**:
- ✅ `60_API_LÓGICA_NEGÓCIO.md` - Nova seção "📸🎥🎧 Mídias em Conteúdo"
  - Upload de mídia
  - Mídias em Posts (imagens, vídeos, áudios)
  - Mídias em Eventos (imagens, vídeos, áudios)
  - Mídias em Marketplace (imagens, vídeos, áudios)
  - Mídias em Chat (imagens e áudios apenas)
  - Configuração avançada por território
  - Preferências do usuário

**Documentos da Fase 10**:
- ✅ `FASE10_CONFIG_MIDIAS_TERRITORIO.md` - Especificação completa
- ✅ `FASE10_CONFIG_MIDIAS_IMPLEMENTACAO.md` - Plano de implementação
- ✅ `FASE10_CONFIG_MIDIAS_STATUS.md` - Status da implementação

---

## 📊 Limites de Mídias Documentados

### Posts
- **Imagens**: Múltiplas (até 10)
- **Vídeos**: 1 por post (até 50MB, 5 minutos)
- **Áudios**: 1 por post (até 10MB, 5 minutos)
- **Total**: Máximo 10 mídias (imagens + 1 vídeo ou 1 áudio)

### Eventos
- **Capa**: 1 (imagem, vídeo ou áudio)
- **Imagens adicionais**: Até 5
- **Vídeos adicionais**: 1 (até 100MB, 10 minutos)
- **Áudios adicionais**: 1 (até 20MB, 10 minutos)
- **Total**: Máximo 6 mídias (1 capa + 5 adicionais)

### Marketplace
- **Imagens**: Múltiplas (até 10)
- **Vídeos**: 1 por item (até 30MB, 2 minutos)
- **Áudios**: 1 por item (até 5MB, 2 minutos)
- **Total**: Máximo 10 mídias (imagens + 1 vídeo ou 1 áudio)

### Chat
- **Imagens**: 1 por mensagem (até 5MB)
- **Áudios**: 1 por mensagem (até 2MB, 60 segundos)
- **Vídeos**: Não permitidos (performance e privacidade)

---

## ✅ Checklist

- [x] Feature flags criados
- [x] Modelos de domínio criados
- [x] Repositories InMemory implementados
- [x] Services criados
- [x] Controllers e endpoints criados
- [x] Contracts criados
- [x] Dependency Injection configurada
- [x] Build passando (0 erros)
- [x] Documentação atualizada
- [x] Documentação Flutter atualizada

---

## 🚧 Pendente (Próximos Passos)

### Integração nos Services Existentes
- ⚠️ `PostCreationService` - Integrar validações de configuração
- ⚠️ `EventsService` - Integrar validações de configuração
- ⚠️ `StoreItemService` - Integrar validações de configuração
- ⚠️ `ChatService` - Integrar validações de configuração

### Filtragem de Respostas
- ⚠️ `FeedService` - Filtrar mídias baseado em configurações e preferências
- ⚠️ `EventsService` (listagem) - Filtrar mídias baseado em configurações
- ⚠️ `MarketplaceService` - Filtrar mídias baseado em configurações

### Repositories Postgres
- ⚠️ `PostgresTerritoryMediaConfigRepository` - Implementação Postgres
- ⚠️ `PostgresUserMediaPreferencesRepository` - Implementação Postgres
- ⚠️ Migrations do banco de dados

### Testes
- ⚠️ Testes de integração para endpoints de configuração
- ⚠️ Testes de validação de configuração nos services
- ⚠️ Testes de filtragem baseado em preferências

---

## 📝 Notas

- **Estrutura Base**: 100% completa e funcional
- **Integração**: Pendente (próxima etapa)
- **Postgres**: Pendente (implementação futura)

Esta PR implementa a **estrutura base completa** para configuração avançada de mídias. A integração nos services existentes e a filtragem de respostas podem ser feitas incrementalmente em PRs futuros.

---

## 🔗 Referências

- `docs/backlog-api/FASE10_CONFIG_MIDIAS_TERRITORIO.md` - Especificação completa
- `docs/backlog-api/FASE10_CONFIG_MIDIAS_IMPLEMENTACAO.md` - Plano de implementação
- `docs/backlog-api/FASE10_CONFIG_MIDIAS_STATUS.md` - Status da implementação
- `docs/backlog-api/FASE10.md` - Fase 10 completa
