# Mapa de Impacto: Suporte a Áudio em Mídias

## 🎯 Objetivo

Permitir áudio além de imagens e vídeos em Posts, Eventos e Marketplace, mantendo regras específicas baseadas em redes sociais existentes.

## 📊 Regras de Áudio em Redes Sociais (Referência)

### Limites de Duração
- **Instagram**: Até 60 segundos (stories), até 90 segundos (reels)
- **Twitter/X**: Até 140 segundos (2 min 20s) para áudio em tweets
- **Facebook**: Até 90 segundos (stories)
- **Podcasts**: Geralmente 15-60 minutos, mas para posts sociais: 1-5 minutos
- **Spotify**: Até 3 horas para podcasts completos

### Limites de Tamanho
- **Instagram**: 4MB para áudio em stories
- **Twitter/X**: 25MB para áudio em tweets
- **Facebook**: 4MB para áudio em stories
- **WhatsApp**: 16MB para áudio
- **Telegram**: 50MB para áudio

### Formatos Comuns
- **MP3**: Mais comum, compressão eficiente
- **WAV**: Alta qualidade, sem compressão
- **OGG**: Open source, boa compressão
- **AAC/M4A**: Apple, boa qualidade

### Regras para Araponga

#### Posts
- **Duração**: Até 5 minutos (podcasts curtos, narrações, depoimentos)
- **Tamanho**: Máximo 10MB por áudio
- **Quantidade**: Máximo 10 mídias (imagens + vídeos + áudios combinados), mas apenas 1 áudio por post
- **Formatos**: MP3, WAV, OGG
- **Uso**: Narrações, depoimentos, podcasts curtos, música local

#### Eventos
- **Duração**: Até 10 minutos (mais flexível para eventos)
- **Tamanho**: Máximo 20MB por áudio
- **Quantidade**: Máximo 1 áudio por evento (em capa ou adicionais)
- **Formatos**: MP3, WAV, OGG
- **Uso**: Promoção de eventos, trilhas sonoras, depoimentos de participantes

#### Marketplace (Items)
- **Duração**: Até 2 minutos (demonstração rápida de produto/serviço)
- **Tamanho**: Máximo 5MB por áudio
- **Quantidade**: Máximo 1 áudio por item
- **Formatos**: MP3, WAV, OGG
- **Uso**: Descrição de produto, demonstração de serviço, áudio promocional

#### Chat
- **Áudio não permitido**: Apenas imagens (por questões de performance e privacidade)
- **Razão**: Similar a vídeos, áudio em chat pode ser usado para spam ou conteúdo não desejado

## 🔍 Análise de Impacto

### 1. Domínio (MediaAsset/MediaType)
- ✅ **Sem mudanças**: `MediaType.Audio` já existe
- ✅ **Sem mudanças**: `MediaAsset` já suporta áudio

### 2. Infraestrutura (Storage/Validator)
- ⚠️ **Adicionar**: `AllowedAudioMimeTypes` em `MediaStorageOptions`
- ⚠️ **Adicionar**: `MaxAudioSizeBytes` em `MediaStorageOptions`
- ⚠️ **Atualizar**: `MediaValidator` para validar áudio
- ✅ **Sem mudanças**: Storage services já suportam áudio (pasta "audio")

### 3. Aplicação - Services

#### PostCreationService
- ⚠️ **Adicionar validação**: Apenas 1 áudio por post
- ⚠️ **Adicionar validação**: Tamanho máximo 10MB

#### EventsService
- ⚠️ **Adicionar validação**: Apenas 1 áudio por evento (em capa ou adicionais)
- ⚠️ **Adicionar validação**: Tamanho máximo 20MB

#### StoreItemService
- ⚠️ **Adicionar validação**: Apenas 1 áudio por item
- ⚠️ **Adicionar validação**: Tamanho máximo 5MB

#### ChatService
- ✅ **Manter restrição**: Apenas imagens (áudio não permitido)
- ✅ **Sem mudanças**: Restrição já implementada

### 4. API - Controllers e Validators

#### Validators
- ✅ **Sem mudanças**: Validators já aceitam MediaIds genéricos

#### Controllers
- ✅ **Sem mudanças**: Controllers já aceitam MediaIds genéricos

### 5. Documentação

#### DevPortal
- ⚠️ **Atualizar seções**: Indicar que áudio é permitido
- ⚠️ **Documentar limites**: Duração e tamanho para áudio
- ⚠️ **Documentar regras**: Apenas 1 áudio por post/evento/item

#### FASE10.md
- ⚠️ **Atualizar**: Indicar suporte a áudio
- ⚠️ **Documentar regras**: Baseadas em redes sociais

## 🚧 Limitações Atuais

### 1. Duração de Áudio
- **Problema**: Não há extração de metadados de duração de áudio
- **Impacto**: Validação de duração não pode ser feita no upload
- **Solução temporária**: Confiar apenas no limite de tamanho
- **Solução futura**: Integrar biblioteca de processamento de áudio (NAudio, TagLibSharp)

### 2. Processamento de Áudio
- **Problema**: Áudios são armazenados sem processamento
- **Impacto**: Não há transcoding, normalização ou otimização
- **Solução temporária**: Aceitar áudios sem processamento
- **Solução futura**: Processamento assíncrono de áudios (transcoding, normalização)

### 3. Waveform/Visualização
- **Problema**: Não há geração automática de waveform para áudio
- **Impacto**: Interface precisa de waveform fornecido pelo cliente ou usar placeholder
- **Solução futura**: Gerar waveform automaticamente do áudio

## 📋 Plano de Implementação

### Fase 1: Permitir Áudio (MVP)
1. ✅ Adicionar configuração de áudio em `MediaStorageOptions`
2. ✅ Atualizar `MediaValidator` para validar áudio
3. ✅ Adicionar validação: apenas 1 áudio por post/evento/item
4. ✅ Validar tamanho máximo (10MB posts, 20MB eventos, 5MB items)
5. ✅ Atualizar documentação e DevPortal

### Fase 2: Validação Avançada (Futuro)
1. Integrar extração de metadados de áudio
2. Validar duração máxima
3. Validar bitrate e codec
4. Gerar waveform automaticamente

### Fase 3: Processamento (Futuro)
1. Transcoding de áudios
2. Normalização de volume
3. Otimização de tamanho

## 🎯 Regras de Negócio Finais

### Posts
- ✅ Máximo 10 mídias (imagens + vídeos + áudios)
- ✅ Apenas 1 vídeo por post
- ✅ Apenas 1 áudio por post
- ✅ Áudio: máximo 10MB, até 5 minutos (validação de tamanho imediata, duração futura)

### Eventos
- ✅ 1 mídia de capa (pode ser imagem, vídeo ou áudio) + até 5 mídias adicionais
- ✅ Apenas 1 vídeo no total (capa ou adicional)
- ✅ Apenas 1 áudio no total (capa ou adicional)
- ✅ Áudio: máximo 20MB, até 10 minutos

### Marketplace (Items)
- ✅ Máximo 10 mídias (imagens + vídeos + áudios)
- ✅ Apenas 1 vídeo por item
- ✅ Apenas 1 áudio por item
- ✅ Áudio: máximo 5MB, até 2 minutos

### Chat
- ❌ Apenas imagens (áudio não permitido)
- ✅ Limite: 5MB, apenas imagens

## 📝 Formatos Suportados

- **MP3** (`audio/mpeg`): Mais comum, compressão eficiente
- **WAV** (`audio/wav`, `audio/x-wav`): Alta qualidade, sem compressão
- **OGG** (`audio/ogg`, `audio/vorbis`): Open source, boa compressão
