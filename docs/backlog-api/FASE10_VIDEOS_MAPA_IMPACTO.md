# Mapa de Impacto: Suporte a Vídeos em Mídias

## 🎯 Objetivo

Permitir vídeos além de imagens em Posts, Eventos e Marketplace, mantendo regras específicas baseadas em redes sociais existentes.

## 📊 Regras de Vídeos em Redes Sociais (Referência)

### Limites de Duração
- **TikTok**: Até 60s (shorts), até 10 min (longos)
- **Instagram**: Até 60s (stories), até 60s (reels), até 10 min (feed)
- **Facebook**: Até 240 min (feed)
- **Twitter/X**: Até 140s (2 min 20s) não-verificados, até 10 min verificados
- **YouTube Shorts**: Até 60s

### Limites de Tamanho
- **Instagram**: 4GB (feed), 100MB (reels/stories)
- **TikTok**: 287MB
- **Facebook**: 10GB
- **Twitter/X**: 512MB

### Regras para Araponga

#### Posts
- **Duração**: Até 60 segundos (similar a TikTok/Instagram Reels)
- **Tamanho**: Máximo 50MB (similar ao limite atual configurado)
- **Quantidade**: Máximo 10 mídias (imagens + vídeos combinados), mas apenas 1 vídeo por post

#### Eventos
- **Duração**: Até 2 minutos (mais flexível para eventos)
- **Tamanho**: Máximo 100MB
- **Quantidade**: 1 imagem de capa + até 5 mídias adicionais (imagens ou 1 vídeo)

#### Marketplace (Items)
- **Duração**: Até 30 segundos (demonstração rápida de produto)
- **Tamanho**: Máximo 30MB
- **Quantidade**: Máximo 10 mídias (imagens + vídeos), mas apenas 1 vídeo por item

#### Chat
- **Manter apenas imagens** (vídeos não permitidos por questões de performance e privacidade)
- Limite atual: 5MB, apenas imagens

## 🔍 Análise de Impacto

### 1. Domínio (MediaAsset/MediaType)
- ✅ **Sem mudanças**: `MediaType.Video` já existe
- ✅ **Sem mudanças**: `MediaAsset` já suporta vídeos

### 2. Infraestrutura (Storage/Validator)
- ✅ **Sem mudanças**: `MediaValidator` já valida vídeos (50MB)
- ✅ **Sem mudanças**: Storage services já suportam vídeos
- ⚠️ **Atenção**: `LocalMediaProcessingService` só processa imagens (OK para vídeos)

### 3. Aplicação - Services

#### PostCreationService
- ✅ **Permitir vídeos**: Remover restrição implícita (se houver)
- ⚠️ **Adicionar validação**: Apenas 1 vídeo por post
- ⚠️ **Adicionar validação**: Duração máxima 60s (requer metadados do vídeo)

#### EventsService
- ✅ **Permitir vídeos**: Remover restrição implícita
- ⚠️ **Adicionar validação**: Apenas 1 vídeo por evento (em capa ou adicionais)
- ⚠️ **Adicionar validação**: Duração máxima 120s

#### StoreItemService
- ✅ **Permitir vídeos**: Remover restrição implícita
- ⚠️ **Adicionar validação**: Apenas 1 vídeo por item
- ⚠️ **Adicionar validação**: Duração máxima 30s

#### ChatService
- ✅ **Manter restrição**: Apenas imagens (vídeo não permitido)
- ✅ **Sem mudanças**: Restrição já implementada

### 4. API - Controllers e Validators

#### Validators
- ⚠️ **Atualizar mensagens**: Indicar que vídeos são permitidos
- ⚠️ **Adicionar validações**: Limites de vídeos (1 por post/evento/item)

#### Controllers
- ✅ **Sem mudanças**: Controllers já aceitam MediaIds genéricos

### 5. Documentação

#### DevPortal
- ⚠️ **Atualizar seções**: Indicar que vídeos são permitidos
- ⚠️ **Documentar limites**: Duração e tamanho para vídeos
- ⚠️ **Documentar regras**: Apenas 1 vídeo por post/evento/item

#### FASE10.md
- ⚠️ **Atualizar**: Indicar suporte a vídeos
- ⚠️ **Documentar regras**: Baseadas em redes sociais

## 🚧 Limitações Atuais

### 1. Duração de Vídeo
- **Problema**: Não há extração de metadados de duração de vídeo
- **Impacto**: Validação de duração não pode ser feita no upload
- **Solução temporária**: Confiar apenas no limite de tamanho
- **Solução futura**: Integrar biblioteca de processamento de vídeo (FFmpeg, MediaInfo)

### 2. Processamento de Vídeo
- **Problema**: `LocalMediaProcessingService` só processa imagens
- **Impacto**: Vídeos são armazenados sem processamento/otimização
- **Solução temporária**: Aceitar vídeos sem processamento
- **Solução futura**: Processamento assíncrono de vídeos (transcoding, thumbnails)

### 3. Thumbnails
- **Problema**: Não há geração automática de thumbnails para vídeos
- **Impacto**: Interface precisa de thumbnail fornecido pelo cliente
- **Solução futura**: Gerar thumbnail automaticamente do primeiro frame

## 📋 Plano de Implementação

### Fase 1: Permitir Vídeos (MVP)
1. ✅ Remover restrições implícitas em services
2. ✅ Adicionar validação: apenas 1 vídeo por post/evento/item
3. ✅ Validar tamanho máximo (já existe no MediaValidator)
4. ✅ Atualizar documentação e DevPortal

### Fase 2: Validação Avançada (Futuro)
1. Integrar extração de metadados de vídeo
2. Validar duração máxima
3. Validar codec e resolução
4. Gerar thumbnails automaticamente

### Fase 3: Processamento (Futuro)
1. Transcoding de vídeos
2. Múltiplas resoluções (adaptive streaming)
3. Otimização de tamanho

## 🎯 Regras de Negócio Finais

### Posts
- ✅ Máximo 10 mídias (imagens + vídeos)
- ✅ Apenas 1 vídeo por post
- ✅ Vídeo: máximo 50MB, até 60s (validação de tamanho imediata, duração futura)

### Eventos
- ✅ 1 imagem de capa (pode ser vídeo) + até 5 mídias adicionais
- ✅ Apenas 1 vídeo no total (capa ou adicional)
- ✅ Vídeo: máximo 100MB, até 120s

### Marketplace (Items)
- ✅ Máximo 10 mídias (imagens + vídeos)
- ✅ Apenas 1 vídeo por item
- ✅ Vídeo: máximo 30MB, até 30s

### Chat
- ❌ Apenas imagens (vídeos não permitidos)
- ✅ Limite: 5MB, apenas imagens
