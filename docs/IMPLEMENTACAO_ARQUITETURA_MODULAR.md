# Implementação: Arquitetura Modular Escalável

**Versão**: 1.0  
**Data**: 2026-01-26  
**Status**: 🚧 Em Implementação  
**Tipo**: Guia de Implementação

---

## 🎯 Estratégia de Implementação

### Abordagem Incremental

Refatoração será feita **incrementalmente** para não quebrar funcionalidade existente:

1. ✅ **Fase 1**: Criar estrutura base (Domain.Core)
2. ✅ **Fase 2**: Refatorar módulo Feed (exemplo)
3. ✅ **Fase 3**: Refatorar módulo Marketplace
4. ✅ **Fase 4**: Aplicar aos demais módulos
5. ✅ **Fase 5**: Refatorar API

---

## 📋 Passo a Passo

### Passo 1: Criar Araponga.Domain.Core

**Objetivo**: Separar entidades compartilhadas.

**Ações**:
1. Criar projeto `backend/Araponga.Domain.Core/Araponga.Domain.Core.csproj`
2. Mover entidades compartilhadas:
   - `Territories/Territory.cs`
   - `Users/User.cs`
   - `Membership/TerritoryMembership.cs`
   - `Membership/MembershipRole.cs`
   - `Membership/ResidencyVerification.cs`
3. Mover interfaces compartilhadas:
   - `Interfaces/Repositories/ITerritoryRepository.cs`
   - `Interfaces/Repositories/IUserRepository.cs`
   - `Interfaces/Repositories/ITerritoryMembershipRepository.cs`
4. Atualizar referências em todos os projetos

**Comandos**:
```bash
# Criar projeto
dotnet new classlib -n Araponga.Domain.Core -o backend/Araponga.Domain.Core

# Mover arquivos (manualmente ou via IDE)
# Atualizar namespaces de Araponga.Domain.* para Araponga.Domain.Core.*
```

---

### Passo 2: Refatorar Módulo Feed

**Objetivo**: Tornar Feed independente.

#### 2.1 Criar Estrutura de Pastas

```
Araponga.Modules.Feed/
├── Domain/
│   ├── Post.cs
│   ├── PostComment.cs
│   └── PostStatus.cs
├── Application/
│   ├── Services/
│   │   ├── FeedService.cs
│   │   ├── PostCreationService.cs
│   │   └── PostFilterService.cs
│   ├── Interfaces/
│   │   └── IFeedService.cs
│   └── DTOs/
│       ├── PostRequest.cs
│       └── PostResponse.cs
└── FeedModule.cs
```

#### 2.2 Mover Entidades

**De**: `Araponga.Domain/Feed/Post.cs`  
**Para**: `Araponga.Modules.Feed/Domain/Post.cs`

**Atualizar namespace**:
```csharp
// Antes
namespace Araponga.Domain.Feed;

// Depois
namespace Araponga.Modules.Feed.Domain;
```

#### 2.3 Mover Services

**De**: `Araponga.Application/Services/FeedService.cs`  
**Para**: `Araponga.Modules.Feed/Application/Services/FeedService.cs`

**Atualizar namespace**:
```csharp
// Antes
namespace Araponga.Application.Services;

// Depois
namespace Araponga.Modules.Feed.Application.Services;
```

#### 2.4 Criar Interface Pública

**Criar**: `Araponga.Modules.Feed/Application/Interfaces/IFeedService.cs`

```csharp
namespace Araponga.Modules.Feed.Application.Interfaces;

public interface IFeedService
{
    Task<Result<PostResponse>> CreatePostAsync(CreatePostRequest request, Guid userId);
    Task<Result<PagedResult<PostResponse>>> ListPostsAsync(Guid territoryId, PostFilterOptions options);
    // ... outros métodos públicos
}
```

**Atualizar FeedService**:
```csharp
public class FeedService : IFeedService
{
    // Implementação
}
```

#### 2.5 Atualizar FeedModule

```csharp
public class FeedModule : IModule
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Registrar repositórios
        services.AddScoped<IFeedRepository, FeedRepository>();
        
        // Registrar services
        services.AddScoped<IFeedService, FeedService>();
        services.AddScoped<PostCreationService>();
        services.AddScoped<PostFilterService>();
        
        // Registrar handlers de eventos
        // ...
    }
}
```

#### 2.6 Atualizar Referências

**Atualizar projetos que usam Feed**:
- `Araponga.Api` → usar `IFeedService` ao invés de `FeedService`
- `Araponga.Infrastructure` → atualizar repositórios
- `Araponga.Tests` → atualizar testes

---

### Passo 3: Refatorar API

**Objetivo**: API usa apenas interfaces de módulos.

#### 3.1 Atualizar Controllers

**Antes**:
```csharp
using Araponga.Domain.Feed;  // ❌

public class FeedController
{
    private readonly FeedService _feedService;  // ❌
}
```

**Depois**:
```csharp
using Araponga.Modules.Feed.Application.Interfaces;  // ✅
using Araponga.Modules.Feed.Application.DTOs;  // ✅

public class FeedController
{
    private readonly IFeedService _feedService;  // ✅
}
```

#### 3.2 Remover Usos Diretos de Domain

**Buscar e substituir**:
- `using Araponga.Domain.Feed` → remover
- `using Araponga.Domain.Marketplace` → remover
- Usar apenas DTOs de Application dos módulos

---

## 🔧 Estrutura de Projetos Final

```
backend/
├── Araponga.Domain.Core/          # ✅ Apenas compartilhado
│   ├── Territories/
│   ├── Users/
│   └── Membership/
│
├── Araponga.Modules.Feed/          # ✅ Independente
│   ├── Domain/
│   ├── Application/
│   └── FeedModule.cs
│
├── Araponga.Modules.Marketplace/  # ✅ Independente
│   ├── Domain/
│   ├── Application/
│   └── MarketplaceModule.cs
│
├── Araponga.Application/           # ⚠️ Apenas serviços compartilhados
│   └── Services/
│       ├── TerritoryService.cs    # Compartilhado
│       └── MembershipService.cs   # Compartilhado
│
├── Araponga.Infrastructure/        # ✅ Implementações
│   └── Repositories/
│
└── Araponga.Api/                  # ✅ Usa interfaces
    └── Controllers/
```

---

## ✅ Checklist de Implementação

### Fase 1: Domain.Core
- [ ] Criar projeto `Araponga.Domain.Core`
- [ ] Mover `Territory`, `User`, `TerritoryMembership`
- [ ] Atualizar namespaces
- [ ] Atualizar referências em todos os projetos
- [ ] Executar testes

### Fase 2: Módulo Feed
- [ ] Criar estrutura de pastas (Domain, Application)
- [ ] Mover entidades de Feed para `Feed.Domain`
- [ ] Mover services de Feed para `Feed.Application`
- [ ] Criar interface `IFeedService`
- [ ] Atualizar `FeedModule`
- [ ] Atualizar referências
- [ ] Executar testes

### Fase 3: Módulo Marketplace
- [ ] Aplicar mesmo padrão do Feed
- [ ] Criar `IStoreService`
- [ ] Atualizar referências
- [ ] Executar testes

### Fase 4: API
- [ ] Refatorar controllers para usar interfaces
- [ ] Remover usos diretos de Domain
- [ ] Usar apenas DTOs de Application
- [ ] Executar testes

---

## 🚨 Pontos de Atenção

### 1. Dependências Circulares

**Problema**: Módulos não podem depender uns dos outros diretamente.

**Solução**: Usar interfaces públicas e eventos.

### 2. Entidades Compartilhadas

**Regra**: Apenas entidades usadas por **múltiplos módulos** ficam em Domain.Core.

**Exemplos**:
- ✅ `Territory` → usado por Feed, Marketplace, Events, etc.
- ✅ `User` → usado por todos os módulos
- ✅ `TerritoryMembership` → usado por todos os módulos
- ❌ `Post` → usado apenas por Feed
- ❌ `Store` → usado apenas por Marketplace

### 3. Migração Incremental

**Estratégia**: Refatorar um módulo por vez, testando após cada mudança.

**Ordem sugerida**:
1. Feed (menor impacto)
2. Marketplace
3. Events
4. Map
5. Chat
6. Outros

---

## 📊 Métricas de Sucesso

### Antes
- ❌ Módulos são apenas registradores
- ❌ Domain monolítico
- ❌ Application monolítico
- ❌ API usa Domain diretamente

### Depois
- ✅ Cada módulo é independente
- ✅ Domain.Core apenas compartilhado
- ✅ Cada módulo tem seu próprio Domain e Application
- ✅ API usa apenas interfaces públicas

---

**Versão**: 1.0  
**Última Atualização**: 2026-01-26  
**Status**: 🚧 Em Implementação
