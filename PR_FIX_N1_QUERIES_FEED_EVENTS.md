# PR: Fix N+1 Queries em FeedController e EventsService

## Resumo

Este PR corrige problemas críticos de N+1 queries no `FeedController` e `EventsService`, reduzindo drasticamente o número de queries ao banco de dados. Para 20 posts, reduz de 200+ queries para apenas 3-5 queries.

## Problema Identificado

### FeedController - N+1 Queries de Counts
**Localização**: `FeedController.GetFeedPaged`, `GetFeed`, `GetMyFeed`, `GetMyFeedPaged`

**Problema**: Para cada post, eram executadas 2 queries separadas:
- `GetLikeCountAsync(post.Id)` - 1 query por post
- `GetShareCountAsync(post.Id)` - 1 query por post

**Impacto**: 
- 20 posts = 40 queries extras
- 100 posts = 200 queries extras

### EventsService - N+1 Queries de Usuários
**Localização**: `EventsService.BuildSummariesAsync`

**Problema**: Para cada usuário único, era executada 1 query:
- `GetByIdAsync(userId)` - 1 query por usuário

**Impacto**:
- 20 eventos com usuários diferentes = 20 queries extras

## Solução Implementada

### 1. Método Batch para Counts de Posts
Adicionado método `GetCountsByPostIdsAsync` que busca todos os counts em 2 queries (uma para likes, uma para shares) usando `GROUP BY`.

### 2. Método Batch para Usuários
Adicionado método `ListByIdsAsync` no `IUserRepository` para buscar múltiplos usuários de uma vez.

## Mudanças Implementadas

### Novos Arquivos
- ✅ `backend/Araponga.Application/Common/PostCounts.cs` - Record para counts de posts

### Interfaces
- ✅ `IFeedRepository`: Adicionado `GetCountsByPostIdsAsync`
- ✅ `IUserRepository`: Adicionado `ListByIdsAsync`

### Implementações
- ✅ `PostgresFeedRepository`: Implementado `GetCountsByPostIdsAsync` com GROUP BY
- ✅ `InMemoryFeedRepository`: Implementado `GetCountsByPostIdsAsync` em memória
- ✅ `PostgresUserRepository`: Implementado `ListByIdsAsync`
- ✅ `InMemoryUserRepository`: Implementado `ListByIdsAsync`

### Services
- ✅ `FeedService`: Adicionado método `GetCountsByPostIdsAsync`
- ✅ `EventsService`: Atualizado `BuildSummariesAsync` para usar batch

### Controllers
- ✅ `FeedController`: Atualizado `GetFeed`, `GetFeedPaged`, `GetMyFeed`, `GetMyFeedPaged` para usar batch

## Impacto de Performance

### Antes
- **FeedController (20 posts)**: 1 query (posts) + 40 queries (counts) = **41 queries**
- **EventsService (20 eventos)**: 1 query (eventos) + 20 queries (usuários) = **21 queries**

### Depois
- **FeedController (20 posts)**: 1 query (posts) + 2 queries (counts batch) = **3 queries**
- **EventsService (20 eventos)**: 1 query (eventos) + 1 query (usuários batch) = **2 queries**

### Redução
- **FeedController**: ~93% de redução (41 → 3 queries)
- **EventsService**: ~90% de redução (21 → 2 queries)

## Arquivos Modificados

### Novos Arquivos
- `backend/Araponga.Application/Common/PostCounts.cs`

### Arquivos Modificados
- `backend/Araponga.Application/Interfaces/IFeedRepository.cs`
- `backend/Araponga.Application/Interfaces/IUserRepository.cs`
- `backend/Araponga.Infrastructure/Postgres/PostgresFeedRepository.cs`
- `backend/Araponga.Infrastructure/Postgres/PostgresUserRepository.cs`
- `backend/Araponga.Infrastructure/InMemory/InMemoryFeedRepository.cs`
- `backend/Araponga.Infrastructure/InMemory/InMemoryUserRepository.cs`
- `backend/Araponga.Application/Services/FeedService.cs`
- `backend/Araponga.Application/Services/EventsService.cs`
- `backend/Araponga.Api/Controllers/FeedController.cs`

## Testes

### Testes Necessários
- [ ] Teste unitário: `GetCountsByPostIdsAsync` retorna counts corretos
- [ ] Teste unitário: `ListByIdsAsync` retorna usuários corretos
- [ ] Teste de integração: FeedController retorna counts corretos
- [ ] Teste de integração: EventsService retorna displayNames corretos

## Breaking Changes

⚠️ **Nenhum breaking change**. Métodos novos foram adicionados, métodos antigos mantidos.

## Commits

- `fix: adicionar PostCounts record para batch counts`
- `fix: adicionar método batch GetCountsByPostIdsAsync no IFeedRepository`
- `fix: implementar GetCountsByPostIdsAsync em PostgresFeedRepository`
- `fix: implementar GetCountsByPostIdsAsync em InMemoryFeedRepository`
- `fix: adicionar método ListByIdsAsync no IUserRepository`
- `fix: implementar ListByIdsAsync em PostgresUserRepository`
- `fix: implementar ListByIdsAsync em InMemoryUserRepository`
- `fix: atualizar FeedService para usar método batch de counts`
- `fix: corrigir N+1 queries no FeedController usando batch counts`
- `fix: corrigir N+1 queries no EventsService usando ListByIdsAsync`
