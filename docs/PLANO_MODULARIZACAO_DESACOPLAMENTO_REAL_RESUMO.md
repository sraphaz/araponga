# Resumo: Plano de Modularização com Desacoplamento Real

**Versão**: 1.0  
**Data**: 2026-01-27  
**Status**: 📋 Pronto para Implementação

---

## 🎯 Objetivo Principal

Implementar **desacoplamento real** através do **slice da infraestrutura**, onde cada módulo tem sua própria camada de infraestrutura (repositórios, DbContext, migrações).

---

## 📊 O Problema

**Situação Atual**:
- ❌ `Araponga.Infrastructure` é monolítico (todos os repositórios juntos)
- ❌ Um único `DbContext` para todos os módulos
- ❌ Migrações acopladas
- ❌ Impossível extrair módulo sem levar parte da Infrastructure

**Impacto**:
- Dificulta extração para microserviços
- Escalabilidade limitada
- Manutenção complexa

---

## ✅ A Solução

**Slice da Infraestrutura**:
- ✅ `Araponga.Infrastructure.Shared` - Apenas infraestrutura compartilhada
- ✅ `Araponga.Modules.Feed.Infrastructure` - Infraestrutura própria do Feed
- ✅ `Araponga.Modules.Marketplace.Infrastructure` - Infraestrutura própria do Marketplace
- ✅ Cada módulo com seu próprio `DbContext`
- ✅ Migrações independentes por módulo

---

## 🏗️ Estrutura Final

```
Araponga.Infrastructure.Shared/
├── SharedDbContext.cs          # Apenas Territory, User, Membership
├── Repositories/               # Territory, User, Membership
└── Services/                   # Cache, Email, MediaStorage, EventBus

Araponga.Modules.Feed.Infrastructure/
├── FeedDbContext.cs            # Próprio, apenas entidades de Feed
├── Repositories/               # FeedRepository, PostCommentRepository
└── Migrations/                 # Migrações próprias

Araponga.Modules.Marketplace.Infrastructure/
├── MarketplaceDbContext.cs     # Próprio, apenas entidades de Marketplace
├── Repositories/               # StoreRepository, CartRepository
└── Migrations/                 # Migrações próprias
```

---

## 📋 Fases de Implementação

### Fase 1: Infrastructure.Shared (Semana 1)
- Criar projeto `Araponga.Infrastructure.Shared`
- Mover `SharedDbContext` (apenas entidades compartilhadas)
- Mover repositórios compartilhados
- Mover serviços cross-cutting

### Fase 2: Feed.Infrastructure (Semana 2)
- Criar projeto `Araponga.Modules.Feed.Infrastructure`
- Criar `FeedDbContext` próprio
- Mover repositórios de Feed
- Criar migrações próprias

### Fase 3: Marketplace.Infrastructure (Semana 3)
- Criar projeto `Araponga.Modules.Marketplace.Infrastructure`
- Criar `MarketplaceDbContext` próprio
- Mover repositórios de Marketplace
- Criar migrações próprias

### Fase 4: Outros Módulos (Semanas 4-6)
- Aplicar padrão aos demais módulos (Events, Map, Chat, etc.)

### Fase 5: Refatorar API e Testes (Semana 7)
- Atualizar `Program.cs` para múltiplos DbContexts
- Atualizar controllers
- Atualizar testes

### Fase 6: Limpeza (Semana 8)
- Remover `Araponga.Infrastructure` monolítico
- Documentar padrões

---

## 🔧 Detalhes Técnicos Importantes

### 1. Múltiplos DbContexts

Cada módulo registra seu próprio DbContext:

```csharp
// FeedModule.cs
services.AddDbContext<FeedDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("FeedDb")));
```

### 2. Connection Strings

**Opção 1: Bancos separados**:
```json
{
  "ConnectionStrings": {
    "SharedDb": "Host=localhost;Database=araponga_shared;...",
    "FeedDb": "Host=localhost;Database=araponga_feed;...",
    "MarketplaceDb": "Host=localhost;Database=araponga_marketplace;..."
  }
}
```

**Opção 2: Mesmo banco, schemas diferentes**:
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

### 3. Relacionamentos entre Módulos

**Solução**: Usar FKs por Guid **sem navegação direta**:

```csharp
// Post.cs
public class Post
{
    public Guid Id { get; set; }
    public Guid TerritoryId { get; set; }  // FK, sem navegação
    public Guid AuthorUserId { get; set; } // FK, sem navegação
    
    // Para buscar Territory/User, usar serviços
}
```

### 4. Transações entre Módulos

**Solução**: Unit of Work distribuído ou Saga Pattern:

```csharp
public class DistributedUnitOfWork : IUnitOfWork
{
    private readonly List<DbContext> _contexts;
    
    public async Task CommitAsync()
    {
        foreach (var context in _contexts)
        {
            await context.SaveChangesAsync();
        }
        await _eventBus.PublishOutboxEventsAsync();
    }
}
```

---

## ✅ Benefícios

| Aspecto | Antes | Depois |
|---------|-------|--------|
| **Infraestrutura** | ❌ Monolítica | ✅ Modular |
| **DbContext** | ❌ Um único | ✅ Um por módulo |
| **Migrações** | ❌ Acopladas | ✅ Independentes |
| **Extração para Microserviço** | ❌ Difícil | ✅ Fácil |
| **Escalabilidade** | ❌ Tudo junto | ✅ Independente |

---

## 🚀 Próximos Passos Imediatos

1. ✅ **Criar `Araponga.Infrastructure.Shared`**
   - Mover `SharedDbContext`
   - Mover repositórios compartilhados
   - Mover serviços cross-cutting

2. ✅ **Criar `Araponga.Modules.Feed.Infrastructure`** (exemplo)
   - Criar `FeedDbContext`
   - Mover repositórios de Feed
   - Criar migrações

3. ✅ **Aplicar aos demais módulos**

---

## 📚 Documentação Completa

- **Plano Completo**: `docs/PLANO_MODULARIZACAO_DESACOPLAMENTO_REAL.md`
- **Status Atual**: `docs/STATUS_REFATORACAO_MODULAR.md`
- **Avaliação**: `docs/AVALIACAO_DESACOPLAMENTO_ARQUITETURA.md`

---

**Versão**: 1.0  
**Última Atualização**: 2026-01-27  
**Status**: 📋 Pronto para Implementação
