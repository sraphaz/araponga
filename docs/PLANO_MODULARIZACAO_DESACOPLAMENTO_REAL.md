# Plano: Modularização com Desacoplamento Real

**Versão**: 1.0  
**Data**: 2026-01-27  
**Status**: 🚧 Em Andamento  
**Tipo**: Documentação Técnica de Arquitetura

---

## 🎯 Objetivo

Implementar modularização completa com **desacoplamento real**, onde cada módulo é totalmente independente, incluindo:
- ✅ Domain próprio
- ✅ Application própria
- ✅ **Infrastructure própria (slice da infraestrutura)** ← **NOVO**
- ✅ Interfaces públicas bem definidas
- ✅ Comunicação via contratos explícitos

**Princípio Fundamental**: Cada módulo deve poder ser extraído para um microserviço sem dependências diretas de outros módulos.

---

## 📊 Situação Atual vs. Desejada

### Situação Atual ❌

```
Araponga.Infrastructure (Monolítico)
├── Repositories/
│   ├── FeedRepository.cs          # Feed
│   ├── MarketplaceRepository.cs    # Marketplace
│   ├── EventsRepository.cs         # Events
│   └── ... (todos os repositórios juntos)
├── Postgres/
│   └── ArapongaDbContext.cs       # Um DbContext para tudo
└── Services/
    └── ... (serviços compartilhados)

Araponga.Modules.Feed (Parcial)
├── Domain/                         ✅ Criado
├── Application/                    ✅ Criado
└── Infrastructure/                  ❌ NÃO EXISTE (usa Araponga.Infrastructure)
```

**Problemas**:
- ❌ Módulos dependem de `Araponga.Infrastructure` monolítico
- ❌ Um `DbContext` único para todos os módulos
- ❌ Repositórios de diferentes módulos no mesmo projeto
- ❌ Impossível extrair módulo sem levar parte da Infrastructure
- ❌ Migrações de banco acopladas

---

### Situação Desejada ✅

```
Araponga.Infrastructure.Shared (Apenas Compartilhado)
├── Postgres/
│   ├── SharedDbContext.cs          # Apenas entidades compartilhadas (Territory, User, Membership)
│   └── Migrations/                 # Migrações compartilhadas
├── Services/
│   ├── CacheService.cs             # Serviços cross-cutting
│   ├── EmailService.cs
│   └── MediaStorageService.cs
└── Repositories/
    ├── TerritoryRepository.cs      # Apenas repositórios compartilhados
    └── UserRepository.cs

Araponga.Modules.Feed (Independente)
├── Domain/                         ✅ Post, PostComment, etc.
├── Application/                    ✅ FeedService, PostCreationService, etc.
└── Infrastructure/                 🆕 PRÓPRIO
    ├── Repositories/
    │   ├── FeedRepository.cs        # Implementação própria
    │   └── PostCommentRepository.cs
    ├── Postgres/
    │   ├── FeedDbContext.cs         # DbContext próprio
    │   └── Migrations/              # Migrações próprias
    └── Services/
        └── FeedCacheService.cs      # Cache específico do Feed (opcional)

Araponga.Modules.Marketplace (Independente)
├── Domain/                         ✅ Store, StoreItem, etc.
├── Application/                    ✅ StoreService, CartService, etc.
└── Infrastructure/                 🆕 PRÓPRIO
    ├── Repositories/
    │   ├── StoreRepository.cs
    │   └── CartRepository.cs
    ├── Postgres/
    │   ├── MarketplaceDbContext.cs  # DbContext próprio
    │   └── Migrations/              # Migrações próprias
    └── Services/
        └── PaymentIntegrationService.cs
```

**Benefícios**:
- ✅ Cada módulo tem sua própria infraestrutura
- ✅ DbContexts separados por módulo
- ✅ Migrações independentes
- ✅ Fácil extrair módulo para microserviço
- ✅ Escalabilidade independente por módulo

---

## 🏗️ Arquitetura com Slice da Infraestrutura

### 1. Araponga.Infrastructure.Shared

**Responsabilidade**: Apenas infraestrutura compartilhada entre módulos.

**Conteúdo**:
```
Araponga.Infrastructure.Shared/
├── Postgres/
│   ├── SharedDbContext.cs          # Territory, User, Membership
│   └── Migrations/                  # Migrações compartilhadas
├── Repositories/
│   ├── TerritoryRepository.cs
│   ├── UserRepository.cs
│   └── MembershipRepository.cs
├── Services/
│   ├── CacheService.cs              # IDistributedCacheService
│   ├── EmailService.cs              # IEmailSender
│   ├── MediaStorageService.cs       # IMediaStorageService
│   └── EventBus.cs                  # IEventBus
└── Configurations/
    └── ServiceCollectionExtensions.cs
```

**Regra**: Apenas infraestrutura usada por **múltiplos módulos** fica aqui.

---

### 2. Araponga.Modules.Feed.Infrastructure

**Responsabilidade**: Infraestrutura específica do módulo Feed.

**Estrutura**:
```
Araponga.Modules.Feed.Infrastructure/
├── Repositories/
│   ├── FeedRepository.cs            # Implementa IFeedRepository
│   ├── PostCommentRepository.cs      # Implementa IPostCommentRepository
│   └── PostAssetRepository.cs        # Implementa IPostAssetRepository
├── Postgres/
│   ├── FeedDbContext.cs              # DbContext próprio
│   ├── Configurations/
│   │   ├── PostEntityConfiguration.cs
│   │   └── PostCommentEntityConfiguration.cs
│   └── Migrations/                   # Migrações próprias
└── Services/
    └── FeedCacheService.cs           # Cache específico (opcional)
```

**Dependências**:
- ✅ `Araponga.Modules.Feed.Domain` (entidades)
- ✅ `Araponga.Modules.Feed.Application` (interfaces de repositórios)
- ✅ `Araponga.Infrastructure.Shared` (serviços compartilhados)
- ✅ `Araponga.Domain.Core` (entidades compartilhadas)
- ❌ NÃO depende de outros módulos

---

### 3. Comunicação entre Módulos

#### 3.1 Via Interfaces Públicas

**Cada módulo expõe interfaces públicas**:

```csharp
// Araponga.Modules.Feed.Application/Interfaces/IFeedService.cs
namespace Araponga.Modules.Feed.Application.Interfaces;

public interface IFeedService
{
    Task<Result<PostResponse>> CreatePostAsync(CreatePostRequest request, Guid userId);
    Task<Result<PagedResult<PostResponse>>> ListPostsAsync(Guid territoryId, PostFilterOptions options);
}
```

**Outros módulos usam via DI**:

```csharp
// Araponga.Modules.Marketplace.Application/Services/StoreService.cs
public class StoreService
{
    private readonly IFeedService _feedService;  // Injetado via DI
    
    // Pode usar FeedService sem conhecer implementação
}
```

#### 3.2 Via Eventos de Domínio

**Eventos tipados para comunicação assíncrona**:

```csharp
// Araponga.Modules.Feed.Domain/Events/PostCreatedEvent.cs
namespace Araponga.Modules.Feed.Domain.Events;

public record PostCreatedEvent(
    Guid PostId,
    Guid TerritoryId,
    Guid AuthorUserId,
    DateTime OccurredAt) : IDomainEvent;
```

**Outros módulos podem escutar**:

```csharp
// Araponga.Modules.Marketplace.Application/Handlers/PostCreatedHandler.cs
public class PostCreatedHandler : INotificationHandler<PostCreatedEvent>
{
    public Task Handle(PostCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Reagir a criação de post
    }
}
```

#### 3.3 Via Contratos Compartilhados

**Contratos em projeto compartilhado**:

```csharp
// Araponga.Shared/Contracts/Feed/PostContract.cs
namespace Araponga.Shared.Contracts.Feed;

public record PostContract(
    Guid Id,
    Guid TerritoryId,
    string Title,
    string Content);
```

---

## 📋 Plano de Implementação

### Fase 1: Criar Infrastructure.Shared (Semana 1)

**Objetivo**: Separar infraestrutura compartilhada.

**Tarefas**:
1. ✅ Criar projeto `Araponga.Infrastructure.Shared`
2. ✅ Mover `SharedDbContext` (apenas Territory, User, Membership)
3. ✅ Mover repositórios compartilhados (Territory, User, Membership)
4. ✅ Mover serviços cross-cutting (Cache, Email, MediaStorage, EventBus)
5. ✅ Criar migrações compartilhadas
6. ✅ Atualizar referências em todos os projetos

**Entregáveis**:
- Projeto `Araponga.Infrastructure.Shared` criado
- `SharedDbContext` com apenas entidades compartilhadas
- Repositórios compartilhados movidos
- Serviços cross-cutting movidos

---

### Fase 2: Criar Infrastructure para Módulo Feed (Semana 2)

**Objetivo**: Criar infraestrutura própria para Feed.

**Tarefas**:
1. ✅ Criar projeto `Araponga.Modules.Feed.Infrastructure`
2. ✅ Criar `FeedDbContext` (próprio, apenas entidades de Feed)
3. ✅ Mover repositórios de Feed de `Araponga.Infrastructure`
4. ✅ Criar configurações de entidades (Entity Framework)
5. ✅ Criar migrações próprias para Feed
6. ✅ Atualizar `FeedModule` para registrar infraestrutura
7. ✅ Atualizar referências

**Entregáveis**:
- Projeto `Araponga.Modules.Feed.Infrastructure` criado
- `FeedDbContext` próprio
- Repositórios de Feed no módulo
- Migrações próprias funcionando

---

### Fase 3: Criar Infrastructure para Módulo Marketplace (Semana 3)

**Objetivo**: Criar infraestrutura própria para Marketplace.

**Tarefas**:
1. ✅ Criar projeto `Araponga.Modules.Marketplace.Infrastructure`
2. ✅ Criar `MarketplaceDbContext` (próprio)
3. ✅ Mover repositórios de Marketplace
4. ✅ Criar configurações de entidades
5. ✅ Criar migrações próprias
6. ✅ Atualizar `MarketplaceModule`
7. ✅ Atualizar referências

**Entregáveis**:
- Projeto `Araponga.Modules.Marketplace.Infrastructure` criado
- `MarketplaceDbContext` próprio
- Repositórios de Marketplace no módulo
- Migrações próprias funcionando

---

### Fase 4: Criar Infrastructure para Outros Módulos (Semanas 4-6)

**Objetivo**: Aplicar padrão aos demais módulos.

**Módulos a refatorar**:
- Events
- Map
- Chat
- Subscriptions
- Moderation
- Notifications
- Alerts
- Assets
- Admin

**Tarefas por módulo**:
1. ✅ Criar projeto `Araponga.Modules.{Nome}.Infrastructure`
2. ✅ Criar `{Nome}DbContext` próprio
3. ✅ Mover repositórios específicos
4. ✅ Criar configurações de entidades
5. ✅ Criar migrações próprias
6. ✅ Atualizar `{Nome}Module`
7. ✅ Atualizar referências

**Entregáveis**:
- Todos os módulos com infraestrutura própria
- DbContexts separados
- Migrações independentes

---

### Fase 5: Refatorar API e Testes (Semana 7)

**Objetivo**: API e testes usam apenas interfaces de módulos.

**Tarefas**:
1. ✅ Refatorar controllers para usar interfaces de módulos
2. ✅ Atualizar `Program.cs` para registrar múltiplos DbContexts
3. ✅ Atualizar testes para usar infraestrutura modular
4. ✅ Criar mappers se necessário
5. ✅ Validar que tudo funciona

**Entregáveis**:
- Controllers refatorados
- API desacoplada de Infrastructure monolítico
- Testes atualizados
- Suite completa passando

---

### Fase 6: Limpeza e Otimização (Semana 8)

**Objetivo**: Remover código obsoleto e otimizar.

**Tarefas**:
1. ✅ Remover `Araponga.Infrastructure` monolítico (após migração completa)
2. ✅ Renomear `Araponga.Infrastructure.Shared` para `Araponga.Infrastructure.Shared`
3. ✅ Documentar padrões de infraestrutura modular
4. ✅ Criar guia de migração para novos módulos
5. ✅ Validar performance

**Entregáveis**:
- Código obsoleto removido
- Documentação completa
- Performance validada

---

## 🔧 Detalhes Técnicos

### 1. Múltiplos DbContexts

**Problema**: Como gerenciar múltiplos DbContexts?

**Solução**: Cada módulo registra seu próprio DbContext:

```csharp
// Araponga.Modules.Feed/FeedModule.cs
public void RegisterServices(IServiceCollection services, IConfiguration configuration)
{
    // Registrar DbContext próprio
    services.AddDbContext<FeedDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("FeedDb")));
    
    // Registrar repositórios
    services.AddScoped<IFeedRepository, FeedRepository>();
    
    // Registrar services
    services.AddScoped<IFeedService, FeedService>();
}
```

**Connection Strings**:
```json
{
  "ConnectionStrings": {
    "SharedDb": "Host=localhost;Database=araponga_shared;...",
    "FeedDb": "Host=localhost;Database=araponga_feed;...",
    "MarketplaceDb": "Host=localhost;Database=araponga_marketplace;..."
  }
}
```

**Alternativa (mesmo banco, schemas diferentes)**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=araponga;..."
  },
  "DatabaseSchemas": {
    "Shared": "shared",
    "Feed": "feed",
    "Marketplace": "marketplace"
  }
}
```

---

### 2. Migrações Independentes

**Cada módulo gerencia suas próprias migrações**:

```bash
# Migrações do Feed
dotnet ef migrations add InitialFeed --project Araponga.Modules.Feed.Infrastructure --context FeedDbContext

# Migrações do Marketplace
dotnet ef migrations add InitialMarketplace --project Araponga.Modules.Marketplace.Infrastructure --context MarketplaceDbContext

# Migrações compartilhadas
dotnet ef migrations add InitialShared --project Araponga.Infrastructure.Shared --context SharedDbContext
```

---

### 3. Relacionamentos entre Módulos

**Problema**: Como lidar com relacionamentos entre entidades de módulos diferentes?

**Solução**: Usar **Foreign Keys por Guid** (sem constraint física):

```csharp
// Araponga.Modules.Feed.Domain/Post.cs
public class Post
{
    public Guid Id { get; set; }
    public Guid TerritoryId { get; set; }  // FK para Territory (em Domain.Core)
    public Guid AuthorUserId { get; set; } // FK para User (em Domain.Core)
    
    // Não há navegação direta para Territory/User
    // Usar serviços para buscar dados relacionados
}
```

**Ou usar navegação sem constraint**:

```csharp
// Araponga.Modules.Feed.Infrastructure/Postgres/FeedDbContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Post>()
        .HasOne<Territory>()
        .WithMany()
        .HasForeignKey(p => p.TerritoryId)
        .HasPrincipalKey(t => t.Id)
        .OnDelete(DeleteBehavior.Restrict);
    
    // Mas Territory não está neste DbContext!
    // Solução: Não criar constraint física, apenas lógica
}
```

**Melhor solução**: **Sem navegação, apenas FKs**:

```csharp
// Araponga.Modules.Feed.Domain/Post.cs
public class Post
{
    public Guid Id { get; set; }
    public Guid TerritoryId { get; set; }  // Apenas FK, sem navegação
    public Guid AuthorUserId { get; set; } // Apenas FK, sem navegação
    
    // Para buscar Territory/User, usar serviços
}
```

---

### 4. Transações entre Módulos

**Problema**: Como garantir transações que envolvem múltiplos módulos?

**Solução**: **Unit of Work distribuído** ou **Saga Pattern**:

```csharp
// Araponga.Application/Common/IUnitOfWork.cs
public interface IUnitOfWork
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}

// Implementação distribuída (usando Outbox Pattern)
public class DistributedUnitOfWork : IUnitOfWork
{
    private readonly List<DbContext> _contexts;
    
    public async Task CommitAsync()
    {
        // Salvar em todos os DbContexts
        foreach (var context in _contexts)
        {
            await context.SaveChangesAsync();
        }
        
        // Publicar eventos via Outbox
        await _eventBus.PublishOutboxEventsAsync();
    }
}
```

---

## ✅ Benefícios do Slice da Infraestrutura

### 1. Desacoplamento Real

- ✅ Cada módulo é totalmente independente
- ✅ Pode ser extraído para microserviço sem dependências
- ✅ Escalabilidade independente

### 2. Manutenibilidade

- ✅ Migrações independentes
- ✅ Mudanças em um módulo não afetam outros
- ✅ Fácil identificar código relacionado

### 3. Testabilidade

- ✅ Testes isolados por módulo
- ✅ Fácil mockar infraestrutura
- ✅ Testes mais rápidos

### 4. Performance

- ✅ Escalabilidade por módulo
- ✅ Otimizações independentes
- ✅ Cache específico por módulo

---

## 📊 Comparação

| Aspecto | Antes (Monolítico) | Depois (Modular com Slice) |
|---------|-------------------|---------------------------|
| **Infraestrutura** | ❌ Um projeto único | ✅ Um projeto por módulo + Shared |
| **DbContext** | ❌ Um DbContext para tudo | ✅ Um DbContext por módulo |
| **Migrações** | ❌ Migrações acopladas | ✅ Migrações independentes |
| **Repositórios** | ❌ Todos juntos | ✅ Separados por módulo |
| **Extração para Microserviço** | ❌ Difícil | ✅ Fácil (já está separado) |
| **Escalabilidade** | ❌ Tudo escala junto | ✅ Cada módulo escala independente |

---

## 🚀 Próximos Passos Imediatos

1. ✅ Criar `Araponga.Infrastructure.Shared`
2. ✅ Mover infraestrutura compartilhada
3. ✅ Criar `Araponga.Modules.Feed.Infrastructure` (exemplo)
4. ✅ Aplicar aos demais módulos
5. ✅ Refatorar API e testes

---

## 📚 Referências

- **Documentação Modularização**: `docs/REFATORACAO_ARQUITETURA_MODULAR.md`
- **Status Atual**: `docs/STATUS_REFATORACAO_MODULAR.md`
- **Avaliação Desacoplamento**: `docs/AVALIACAO_DESACOPLAMENTO_ARQUITETURA.md`

---

**Versão**: 1.0  
**Última Atualização**: 2026-01-27  
**Status**: 📋 Pronto para Implementação
