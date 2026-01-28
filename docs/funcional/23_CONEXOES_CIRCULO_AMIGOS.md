# Conexões e Círculo de Amigos - Araponga

**Versão**: 1.0  
**Data**: 2026-01-28  
**Status**: 📋 Planejamento  
**Tipo**: Funcionalidade de Rede Social Territorial

---

## 📋 Índice

1. [Visão Geral](#visão-geral)
2. [Motivação e Propósito](#motivação-e-propósito)
3. [Funcionalidades Principais](#funcionalidades-principais)
4. [Modelo de Domínio](#modelo-de-domínio)
5. [Fluxos de Usuário](#fluxos-de-usuário)
6. [Integração com Feed](#integração-com-feed)
7. [Regras de Negócio](#regras-de-negócio)
8. [Privacidade e Segurança](#privacidade-e-segurança)

---

## 🎯 Visão Geral

O **Módulo de Conexões e Círculo de Amigos** permite que moradores e visitantes do território se conectem entre si, formando uma rede social local baseada em relacionamentos mútuos. As conexões estabelecidas influenciam a priorização de conteúdo no feed, criando uma experiência mais personalizada e relevante.

### Princípios Fundamentais

1. **Conexões Mútuas**: Requer aceitação explícita de ambas as partes
2. **Territorial**: Conexões são contextuais ao território, mas podem ser globais
3. **Privacidade**: Usuário controla quem pode vê-lo e adicioná-lo
4. **Feed Inteligente**: Feed prioriza conteúdo de conexões estabelecidas
5. **Não Invasivo**: Não força conexões, respeita escolhas do usuário

---

## 🌱 Motivação e Propósito

### O Problema

Atualmente, o feed do Araponga mostra conteúdo de forma cronológica ou baseada em interesses, mas não considera relacionamentos pessoais. Isso significa que:

- Conteúdo de pessoas próximas pode se perder no feed
- Não há forma de priorizar conexões pessoais
- Usuários não têm controle sobre quem os vê ou adiciona
- Feed não reflete a rede social real do território

### A Solução

O módulo de conexões permite:

- **Estabelecer relacionamentos**: Moradores e visitantes podem se adicionar mutuamente
- **Priorizar conteúdo**: Feed mostra primeiro conteúdo de conexões estabelecidas
- **Privacidade**: Controle sobre quem pode ver e adicionar
- **Rede social local**: Fortalece vínculos dentro do território

### Valores Fundamentais

- **Autonomia**: Usuário decide com quem se conectar
- **Privacidade**: Controle total sobre visibilidade e conexões
- **Relevância**: Feed mais relevante baseado em relacionamentos
- **Comunidade**: Fortalece laços locais dentro do território

---

## 💼 Funcionalidades Principais

### 1. Gerenciamento de Conexões

#### 1.1 Enviar Solicitação de Conexão
- **Quem pode**: Moradores e visitantes autenticados
- **Como**: Buscar usuário por nome ou visualizar perfil
- **Ação**: Enviar solicitação de conexão
- **Notificação**: Usuário recebe notificação da solicitação

#### 1.2 Aceitar/Rejeitar Solicitação
- **Quem pode**: Usuário que recebeu a solicitação
- **Ações disponíveis**:
  - ✅ Aceitar: Cria conexão mútua
  - ❌ Rejeitar: Remove solicitação, não cria conexão
  - 🔕 Bloquear: Bloqueia usuário (funcionalidade existente)

#### 1.3 Remover Conexão
- **Quem pode**: Qualquer parte da conexão
- **Ação**: Remove conexão mútua
- **Efeito**: Não recebe mais notificações de conexão, mas pode se reconectar no futuro

#### 1.4 Listar Conexões
- **Quem pode**: Próprio usuário
- **Filtros**:
  - Todas as conexões
  - Conexões por território
  - Conexões recentes
  - Busca por nome

### 2. Busca e Descoberta

#### 2.1 Buscar Usuários
- **Critérios de busca**:
  - Nome de exibição
  - Território ativo
  - Status de conexão (conectado, pendente, não conectado)
- **Filtros**:
  - Apenas moradores
  - Apenas visitantes
  - Apenas no território ativo

#### 2.2 Sugestões de Conexão
- **Algoritmo de sugestão**:
  - Conexões mútuas (amigos em comum)
  - Mesmo território
  - Interações recentes (comentários, curtidas)
  - Eventos em comum
- **Privacidade**: Respeita configurações de privacidade do usuário

### 3. Perfil e Visibilidade

#### 3.1 Configurações de Privacidade
- **Quem pode me adicionar**:
  - Qualquer pessoa
  - Apenas moradores
  - Apenas pessoas que eu adicionei primeiro
  - Ninguém (desabilitar conexões)
- **Quem pode ver minhas conexões**:
  - Apenas eu
  - Minhas conexões
  - Todos no território
  - Todos

#### 3.2 Visibilidade de Perfil
- **Informações visíveis para não-conectados**:
  - Nome de exibição
  - Avatar
  - Bio (se público)
  - Território ativo
- **Informações visíveis apenas para conexões**:
  - Lista de conexões (se permitido)
  - Atividade recente
  - Informações de contato (se compartilhadas)

---

## 📐 Modelo de Domínio

### Entidades Principais

#### 1. UserConnection (Conexão entre Usuários)

```csharp
public class UserConnection
{
    public Guid Id { get; }
    public Guid RequesterUserId { get; }      // Usuário que enviou a solicitação
    public Guid TargetUserId { get; }          // Usuário que recebeu a solicitação
    public ConnectionStatus Status { get; }    // Pending, Accepted, Rejected, Removed
    public Guid? TerritoryId { get; }         // Território onde conexão foi criada (opcional, pode ser global)
    public DateTime RequestedAtUtc { get; }    // Quando solicitação foi enviada
    public DateTime? RespondedAtUtc { get; }    // Quando foi aceita/rejeitada
    public DateTime? RemovedAtUtc { get; }    // Quando foi removida
    public DateTime CreatedAtUtc { get; }
    public DateTime UpdatedAtUtc { get; }
}
```

**Regras de Negócio**:
- Uma conexão é única por par de usuários (RequesterUserId, TargetUserId)
- Status `Accepted` cria conexão mútua (ambos podem ver um ao outro como conectados)
- Status `Rejected` não cria conexão, mas impede nova solicitação por 30 dias
- Status `Removed` permite nova solicitação no futuro

#### 2. ConnectionStatus (Enum)

```csharp
public enum ConnectionStatus
{
    Pending,    // Solicitação enviada, aguardando resposta
    Accepted,   // Conexão aceita, relação mútua estabelecida
    Rejected,   // Solicitação rejeitada
    Removed     // Conexão removida por uma das partes
}
```

#### 3. ConnectionPrivacySettings (Configurações de Privacidade)

```csharp
public class ConnectionPrivacySettings
{
    public Guid UserId { get; }
    public ConnectionRequestPolicy WhoCanAddMe { get; }  // Quem pode me adicionar
    public ConnectionVisibility WhoCanSeeMyConnections { get; }  // Quem pode ver minhas conexões
    public bool ShowConnectionsInProfile { get; }  // Mostrar conexões no perfil
    public DateTime CreatedAtUtc { get; }
    public DateTime UpdatedAtUtc { get; }
}
```

#### 4. ConnectionRequestPolicy (Enum)

```csharp
public enum ConnectionRequestPolicy
{
    Anyone,              // Qualquer pessoa pode me adicionar
    ResidentsOnly,       // Apenas moradores podem me adicionar
    ConnectionsOnly,     // Apenas pessoas que eu já adicionei
    Disabled            // Ninguém pode me adicionar
}
```

#### 5. ConnectionVisibility (Enum)

```csharp
public enum ConnectionVisibility
{
    OnlyMe,             // Apenas eu
    MyConnections,      // Minhas conexões
    TerritoryMembers,   // Todos no território
    Everyone           // Todos
}
```

### Relacionamentos

```
User 1..N UserConnection (como Requester)
User 1..N UserConnection (como Target)
User 1..1 ConnectionPrivacySettings
```

**Regra de Unicidade**:
- Uma conexão é única por par (RequesterUserId, TargetUserId)
- Não pode haver duas conexões ativas entre os mesmos usuários

---

## 🔄 Fluxos de Usuário

### Fluxo 1: Criar Conexão

```
1. Usuário A busca Usuário B
2. Usuário A visualiza perfil de B
3. Usuário A clica em "Adicionar como Conexão"
4. Sistema verifica:
   - B permite ser adicionado? (ConnectionRequestPolicy)
   - Já existe conexão entre A e B?
   - A não está bloqueado por B?
5. Se permitido:
   - Cria UserConnection com Status=Pending
   - Envia notificação para B
6. B recebe notificação
7. B pode:
   - Aceitar → Status=Accepted, ambos são conexões
   - Rejeitar → Status=Rejected, não cria conexão
   - Bloquear → Usa sistema de bloqueio existente
```

### Fluxo 2: Feed com Priorização de Conexões

```
1. Usuário A acessa feed do território
2. Sistema busca posts do território
3. Sistema aplica filtros:
   a. Filtros existentes (visibilidade, bloqueios, etc.)
   b. NOVO: Separa posts em duas listas:
      - Posts de conexões (Status=Accepted)
      - Posts de não-conexões
4. Sistema ordena:
   a. Primeiro: Posts de conexões (ordenados por data)
   b. Depois: Posts de não-conexões (ordenados por data)
5. Aplica paginação
6. Retorna feed priorizado
```

### Fluxo 3: Remover Conexão

```
1. Usuário A visualiza lista de conexões
2. Usuário A seleciona conexão com B
3. Usuário A clica em "Remover Conexão"
4. Sistema:
   - Atualiza UserConnection: Status=Removed, RemovedAtUtc=now
   - Remove notificações pendentes relacionadas
5. B não é mais considerado conexão de A
6. Feed de A não prioriza mais posts de B
```

---

## 🔗 Integração com Feed

### Modificação no PostFilterService

O `PostFilterService` atual filtra posts por:
- Visibilidade (PUBLIC, RESIDENTS_ONLY)
- Status (PUBLISHED, etc.)
- Bloqueios de usuários
- Filtros por mapEntityId e assetId

**Nova funcionalidade**: Adicionar filtro de priorização por conexões.

#### Algoritmo de Priorização

```csharp
public async Task<IReadOnlyList<CommunityPost>> FilterAndPrioritizeByConnectionsAsync(
    IReadOnlyList<CommunityPost> posts,
    Guid territoryId,
    Guid? userId,
    CancellationToken cancellationToken)
{
    if (userId is null)
    {
        // Usuário não autenticado: sem priorização
        return await FilterPostsAsync(posts, territoryId, null, null, null, cancellationToken);
    }

    // 1. Aplicar filtros existentes
    var filtered = await FilterPostsAsync(posts, territoryId, userId, null, null, cancellationToken);

    // 2. Buscar conexões aceitas do usuário
    var connections = await _connectionRepository.GetAcceptedConnectionsAsync(userId.Value, cancellationToken);
    var connectionUserIds = connections
        .Select(c => c.RequesterUserId == userId.Value ? c.TargetUserId : c.RequesterUserId)
        .ToHashSet();

    // 3. Separar posts de conexões e não-conexões
    var postsFromConnections = filtered
        .Where(p => connectionUserIds.Contains(p.AuthorUserId))
        .OrderByDescending(p => p.CreatedAtUtc)
        .ToList();

    var postsFromOthers = filtered
        .Where(p => !connectionUserIds.Contains(p.AuthorUserId))
        .OrderByDescending(p => p.CreatedAtUtc)
        .ToList();

    // 4. Combinar: conexões primeiro, depois outros
    return postsFromConnections.Concat(postsFromOthers).ToList();
}
```

### Novo Parâmetro no FeedController

Adicionar parâmetro opcional `prioritizeConnections` (bool, default: true):

```csharp
[HttpGet]
public async Task<ActionResult<IEnumerable<FeedItemResponse>>> GetFeed(
    [FromQuery] Guid? territoryId,
    [FromQuery] Guid? mapEntityId,
    [FromQuery] Guid? assetId,
    [FromQuery] bool filterByInterests = false,
    [FromQuery] bool prioritizeConnections = true,  // NOVO
    CancellationToken cancellationToken)
{
    // ...
}
```

### Configuração por Território (Opcional)

Adicionar feature flag no Territory:

```csharp
public class TerritoryFeatureFlags
{
    // ... flags existentes
    public bool ConnectionsEnabled { get; set; } = true;
    public bool FeedPrioritizeConnections { get; set; } = true;
}
```

---

## 📋 Regras de Negócio

### Regras de Conexão

1. **Unicidade**: Uma conexão é única por par de usuários
2. **Mútua**: Quando aceita, ambos os usuários são conexões um do outro
3. **Territorial (Opcional)**: Conexões podem ser globais ou específicas por território
4. **Não Recíproca Necessária**: Usuário A pode adicionar B, mas B precisa aceitar
5. **Rejeição Temporária**: Após rejeição, nova solicitação só após 30 dias

### Regras de Privacidade

1. **Quem pode me adicionar**: Controlado por `ConnectionRequestPolicy`
2. **Visibilidade de conexões**: Controlado por `ConnectionVisibility`
3. **Bloqueio**: Sistema de bloqueio existente prevalece sobre conexões
4. **Respeito a configurações**: Todas as ações respeitam configurações de privacidade

### Regras de Feed

1. **Priorização opcional**: Pode ser desabilitada por usuário ou território
2. **Não exclusiva**: Prioriza conexões, mas não esconde outros posts
3. **Ordenação**: Dentro de cada grupo (conexões/outros), ordena por data
4. **Performance**: Cache de conexões para evitar queries repetidas

### Regras de Notificações

1. **Solicitação recebida**: Notifica quando recebe solicitação
2. **Conexão aceita**: Notifica quando solicitação é aceita
3. **Conexão removida**: Não notifica (ação silenciosa)
4. **Respeito a preferências**: Respeita `NotificationPreferences` do usuário

---

## 🔒 Privacidade e Segurança

### Privacidade

1. **Controle total**: Usuário controla quem pode adicioná-lo
2. **Visibilidade configurável**: Controla quem vê suas conexões
3. **Não invasivo**: Não força conexões, respeita escolhas
4. **Dados mínimos**: Armazena apenas dados necessários

### Segurança

1. **Validação de entrada**: Todas as solicitações são validadas
2. **Autorização**: Verifica permissões antes de criar/aceitar/rejeitar
3. **Rate limiting**: Limite de solicitações por dia (ex: 50)
4. **Auditoria**: Registra todas as ações (criar, aceitar, rejeitar, remover)
5. **Bloqueio**: Integra com sistema de bloqueio existente

### Proteção contra Abuso

1. **Limite de solicitações**: Máximo de solicitações pendentes (ex: 100)
2. **Cooldown após rejeição**: 30 dias antes de nova solicitação
3. **Detecção de spam**: Múltiplas rejeições podem resultar em bloqueio temporário
4. **Reporte**: Usuários podem reportar solicitações inadequadas

---

## 📊 Métricas e Analytics (Futuro)

### Métricas de Engajamento

- Taxa de aceitação de conexões
- Número médio de conexões por usuário
- Tempo médio para aceitar solicitação
- Taxa de remoção de conexões

### Métricas de Feed

- % de posts visualizados de conexões vs outros
- Engajamento (curtidas, comentários) em posts de conexões
- Tempo de permanência no feed com priorização

---

## 🚀 Roadmap de Implementação

### Fase 1: MVP (Mínimo Viável)
- ✅ Modelo de domínio (UserConnection, ConnectionStatus)
- ✅ CRUD básico de conexões (criar, aceitar, rejeitar, remover)
- ✅ Listagem de conexões
- ✅ Notificações básicas

### Fase 2: Privacidade e Configurações
- ✅ ConnectionPrivacySettings
- ✅ Configurações de visibilidade
- ✅ Integração com sistema de bloqueio

### Fase 3: Integração com Feed
- ✅ Priorização de posts de conexões no feed
- ✅ Parâmetro `prioritizeConnections` no FeedController
- ✅ Cache de conexões para performance

### Fase 4: Busca e Descoberta
- ✅ Busca de usuários
- ✅ Sugestões de conexão
- ✅ Filtros avançados

### Fase 5: Analytics e Otimização
- ✅ Métricas de engajamento
- ✅ Otimização de algoritmo de priorização
- ✅ A/B testing de diferentes estratégias

---

## 📚 Documentação Relacionada

- [Feed Comunitário](./03_FEED_COMUNITARIO.md) - Integração com feed
- [Notificações](./11_NOTIFICACOES.md) - Sistema de notificações
- [Moderação](./10_MODERACAO.md) - Proteção contra abuso
- [Autenticação e Identidade](./01_AUTENTICACAO_IDENTIDADE.md) - Modelo de usuário

---

**Última Atualização**: 2026-01-28  
**Versão**: 1.0  
**Status**: 📋 Planejamento
