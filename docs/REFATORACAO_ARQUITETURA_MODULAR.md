# Refatoração: Arquitetura Modular Escalável

**Versão**: 1.0  
**Data**: 2026-01-26  
**Status**: 📋 Plano de Refatoração  
**Tipo**: Documentação Técnica de Arquitetura

---

## 🎯 Objetivo

Refatorar a arquitetura atual para permitir que **cada módulo seja independente e escalável**, seguindo princípios de **Clean Architecture** e preparando para possível migração futura para **microserviços**.

---

## 📊 Situação Atual vs. Desejada

### Situação Atual ❌

```
Araponga.Domain (Monolítico)
├── Feed/Post.cs
├── Marketplace/Store.cs
├── Territories/Territory.cs
└── Users/User.cs

Araponga.Application (Monolítico)
├── FeedService.cs
├── StoreService.cs
└── TerritoryService.cs

Araponga.Modules.Feed (Apenas Registrador)
└── FeedModule.cs → registra FeedService

Araponga.Modules.Marketplace (Apenas Registrador)
└── MarketplaceModule.cs → registra StoreService
```

**Problemas**:
- ❌ Módulos não podem escalar independentemente
- ❌ Toda a inteligência está em Domain/Application monolíticos
- ❌ Módulos são apenas registradores, não têm autonomia
- ❌ Dificulta migração para microserviços

---

### Situação Desejada ✅

```
Araponga.Domain.Core (Apenas Compartilhado)
├── Territories/Territory.cs      # Compartilhado
├── Users/User.cs                  # Compartilhado
└── Membership/TerritoryMembership.cs  # Compartilhado

Araponga.Modules.Feed (Independente)
├── Domain/
│   └── Post.cs                    # Próprio domínio
├── Application/
│   ├── FeedService.cs             # Própria lógica
│   └── Interfaces/
│       └── IFeedService.cs        # Contrato público
└── FeedModule.cs                  # Registra tudo

Araponga.Modules.Marketplace (Independente)
├── Domain/
│   └── Store.cs                   # Próprio domínio
├── Application/
│   ├── StoreService.cs            # Própria lógica
│   └── Interfaces/
│       └── IStoreService.cs       # Contrato público
└── MarketplaceModule.cs           # Registra tudo
```

**Benefícios**:
- ✅ Cada módulo é independente e pode escalar sozinho
- ✅ Módulos têm sua própria inteligência (Domain + Application)
- ✅ Apenas entidades compartilhadas ficam em Domain.Core
- ✅ Facilita migração para microserviços

---

## 🏗️ Nova Estrutura de Projetos

### 1. Araponga.Domain.Core

**Responsabilidade**: Apenas entidades compartilhadas entre módulos.

**Conteúdo**:
```
Araponga.Domain.Core/
├── Territories/
│   └── Territory.cs              # Compartilhado
├── Users/
│   └── User.cs                   # Compartilhado
├── Membership/
│   ├── TerritoryMembership.cs   # Compartilhado
│   └── MembershipRole.cs        # Compartilhado
└── Interfaces/
    └── ITerritoryRepository.cs   # Interface compartilhada
```

**Regra**: Apenas entidades que são usadas por **múltiplos módulos** ficam aqui.

---

### 2. Araponga.Modules.Feed

**Responsabilidade**: Módulo Feed completo e independente.

**Estrutura**:
```
Araponga.Modules.Feed/
├── Domain/
│   ├── Post.cs                   # Entidade própria
│   ├── PostComment.cs
│   └── PostStatus.cs
├── Application/
│   ├── Services/
│   │   ├── FeedService.cs
│   │   ├── PostCreationService.cs
│   │   └── PostFilterService.cs
│   ├── Interfaces/
│   │   └── IFeedService.cs      # Contrato público
│   └── DTOs/
│       ├── PostRequest.cs
│       └── PostResponse.cs
├── Infrastructure/               # Opcional (se módulo tiver persistência própria)
│   └── Repositories/
│       └── FeedRepository.cs
└── FeedModule.cs                 # Registra tudo
```

**Dependências**:
- ✅ `Araponga.Domain.Core` (para Territory, User, Membership)
- ✅ `Araponga.Modules.Core` (para IModule)
- ❌ NÃO depende de outros módulos

---

### 3. Araponga.Modules.Marketplace

**Responsabilidade**: Módulo Marketplace completo e independente.

**Estrutura**:
```
Araponga.Modules.Marketplace/
├── Domain/
│   ├── Store.cs                  # Entidade própria
│   ├── StoreItem.cs
│   └── Cart.cs
├── Application/
│   ├── Services/
│   │   ├── StoreService.cs
│   │   └── CartService.cs
│   ├── Interfaces/
│   │   └── IStoreService.cs     # Contrato público
│   └── DTOs/
│       ├── StoreRequest.cs
│       └── StoreResponse.cs
└── MarketplaceModule.cs         # Registra tudo
```

---

## 🔄 Comunicação entre Módulos

### 1. Via Interfaces Públicas

**Cada módulo expõe interfaces públicas**:

```csharp
// Araponga.Modules.Feed/Application/Interfaces/IFeedService.cs
namespace Araponga.Modules.Feed.Application.Interfaces;

public interface IFeedService
{
    Task<Result<PostResponse>> CreatePostAsync(CreatePostRequest request, Guid userId);
    Task<Result<PagedResult<PostResponse>>> ListPostsAsync(Guid territoryId, PostFilterOptions options);
}
```

**Outros módulos podem usar via DI**:

```csharp
// Araponga.Modules.Marketplace/Application/Services/StoreService.cs
public class StoreService
{
    private readonly IFeedService _feedService;  // Injetado via DI
    
    // Pode usar FeedService sem conhecer implementação
}
```

---

### 2. Via Eventos de Domínio

**Eventos tipados para comunicação assíncrona**:

```csharp
// Araponga.Modules.Feed/Domain/Events/PostCreatedEvent.cs
namespace Araponga.Modules.Feed.Domain.Events;

public record PostCreatedEvent(
    Guid PostId,
    Guid TerritoryId,
    Guid AuthorUserId,
    DateTime OccurredAt) : IDomainEvent;
```

**Outros módulos podem escutar**:

```csharp
// Araponga.Modules.Marketplace/Application/Handlers/PostCreatedHandler.cs
public class PostCreatedHandler : INotificationHandler<PostCreatedEvent>
{
    public Task Handle(PostCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Reagir a criação de post
    }
}
```

---

### 3. Via Contratos Compartilhados

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

**Módulos usam contratos para comunicação**:

```csharp
// Araponga.Modules.Feed retorna PostContract
// Araponga.Modules.Marketplace consome PostContract
```

---

## 📋 Plano de Refatoração

### Fase 1: Criar Domain.Core (Semana 1)

**Objetivo**: Separar entidades compartilhadas.

1. ✅ Criar projeto `Araponga.Domain.Core`
2. ✅ Mover `Territory`, `User`, `TerritoryMembership` para Core
3. ✅ Atualizar referências em todos os projetos
4. ✅ Manter apenas entidades compartilhadas

**Entregáveis**:
- Projeto `Araponga.Domain.Core` criado
- Entidades compartilhadas movidas
- Referências atualizadas

---

### Fase 2: Refatorar Módulo Feed (Semana 2)

**Objetivo**: Tornar Feed independente.

1. ✅ Criar `Araponga.Modules.Feed.Domain` com entidades de Feed
2. ✅ Criar `Araponga.Modules.Feed.Application` com services de Feed
3. ✅ Mover `Post`, `PostComment`, etc. de Domain para Feed.Domain
4. ✅ Mover `FeedService`, `PostCreationService`, etc. de Application para Feed.Application
5. ✅ Criar interface `IFeedService` pública
6. ✅ Atualizar `FeedModule` para registrar tudo

**Entregáveis**:
- Módulo Feed independente
- Interface pública `IFeedService`
- DTOs em Feed.Application

---

### Fase 3: Refatorar Módulo Marketplace (Semana 3)

**Objetivo**: Tornar Marketplace independente.

1. ✅ Criar `Araponga.Modules.Marketplace.Domain` com entidades de Marketplace
2. ✅ Criar `Araponga.Modules.Marketplace.Application` com services de Marketplace
3. ✅ Mover `Store`, `StoreItem`, `Cart`, etc. de Domain para Marketplace.Domain
4. ✅ Mover `StoreService`, `CartService`, etc. de Application para Marketplace.Application
5. ✅ Criar interface `IStoreService` pública
6. ✅ Atualizar `MarketplaceModule` para registrar tudo

**Entregáveis**:
- Módulo Marketplace independente
- Interface pública `IStoreService`
- DTOs em Marketplace.Application

---

### Fase 4: Refatorar API (Semana 4)

**Objetivo**: API usa apenas interfaces de módulos.

1. ✅ Refatorar controllers para usar interfaces de módulos
2. ✅ Remover uso direto de Domain
3. ✅ Usar apenas DTOs de Application dos módulos
4. ✅ Criar mappers se necessário

**Entregáveis**:
- Controllers refatorados
- API desacoplada de Domain
- Uso apenas de interfaces públicas

---

### Fase 5: Refatorar Outros Módulos (Semanas 5-6)

**Objetivo**: Aplicar padrão aos demais módulos.

1. ✅ Refatorar Events, Map, Chat, etc.
2. ✅ Cada módulo segue o mesmo padrão
3. ✅ Todos os módulos independentes

**Entregáveis**:
- Todos os módulos independentes
- Arquitetura consistente

---

## 🔍 Exemplo de Refatoração

### Antes (Monolítico)

```csharp
// Araponga.Domain/Feed/Post.cs
namespace Araponga.Domain.Feed;
public class Post { ... }

// Araponga.Application/Services/FeedService.cs
namespace Araponga.Application.Services;
public class FeedService { ... }

// Araponga.Modules.Feed/FeedModule.cs
public class FeedModule : IModule
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<FeedService>();  // Apenas registra
    }
}
```

---

### Depois (Modular)

```csharp
// Araponga.Modules.Feed/Domain/Post.cs
namespace Araponga.Modules.Feed.Domain;
public class Post { ... }

// Araponga.Modules.Feed/Application/Services/FeedService.cs
namespace Araponga.Modules.Feed.Application.Services;
public class FeedService : IFeedService { ... }

// Araponga.Modules.Feed/Application/Interfaces/IFeedService.cs
namespace Araponga.Modules.Feed.Application.Interfaces;
public interface IFeedService
{
    Task<Result<PostResponse>> CreatePostAsync(CreatePostRequest request, Guid userId);
}

// Araponga.Modules.Feed/FeedModule.cs
public class FeedModule : IModule
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Registra Domain (repositórios)
        services.AddScoped<IFeedRepository, FeedRepository>();
        
        // Registra Application (services)
        services.AddScoped<IFeedService, FeedService>();
        services.AddScoped<PostCreationService>();
        
        // Registra Infrastructure (se necessário)
        // ...
    }
}
```

---

## ✅ Benefícios da Nova Arquitetura

### 1. Escalabilidade Independente

- ✅ Cada módulo pode escalar separadamente
- ✅ Feed pode ter mais instâncias que Marketplace
- ✅ Preparado para migração para microserviços

### 2. Desacoplamento

- ✅ Módulos não dependem uns dos outros diretamente
- ✅ Comunicação via interfaces públicas
- ✅ Fácil substituir implementação de um módulo

### 3. Testabilidade

- ✅ Cada módulo pode ser testado isoladamente
- ✅ Fácil mockar interfaces públicas
- ✅ Testes mais rápidos e focados

### 4. Manutenibilidade

- ✅ Código organizado por módulo
- ✅ Fácil encontrar código relacionado
- ✅ Mudanças em um módulo não afetam outros

---

## 📊 Comparação

| Aspecto | Antes (Monolítico) | Depois (Modular) |
|---------|-------------------|------------------|
| **Escalabilidade** | ❌ Tudo escala junto | ✅ Cada módulo escala independente |
| **Desacoplamento** | ❌ Tudo acoplado | ✅ Módulos desacoplados |
| **Testabilidade** | ⚠️ Testes acoplados | ✅ Testes isolados |
| **Manutenibilidade** | ⚠️ Código espalhado | ✅ Código organizado |
| **Migração Microserviços** | ❌ Difícil | ✅ Fácil |

---

## 🚀 Próximos Passos

1. ✅ Criar `Araponga.Domain.Core`
2. ✅ Refatorar módulo Feed (exemplo)
3. ✅ Refatorar módulo Marketplace
4. ✅ Refatorar API
5. ✅ Aplicar aos demais módulos

---

**Versão**: 1.0  
**Última Atualização**: 2026-01-26  
**Autor**: Refatoração Arquitetural  
**Status**: 📋 Pronto para Implementação
