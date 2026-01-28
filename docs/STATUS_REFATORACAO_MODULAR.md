# Status da Refatoração Arquitetural Modular

**Data**: 2026-01-26  
**Status**: 🚧 Em Andamento - Fase 1 Concluída, Fase 2 Iniciada

---

## ✅ Fase 1: Domain.Core - CONCLUÍDA

### O que foi feito:

1. ✅ **Projeto `Araponga.Domain.Core` criado**
   - Estrutura de pastas: `Territories/`, `Users/`, `Membership/`
   - Projeto adicionado ao `Araponga.sln`

2. ✅ **Entidades compartilhadas movidas**:
   - `Territory`, `TerritoryStatus`
   - `User`, `UserIdentityVerificationStatus`
   - `TerritoryMembership`, `MembershipRole`, `ResidencyVerification`, `MembershipStatus`

3. ✅ **Referências adicionadas**:
   - `Araponga.Application` → referencia `Domain.Core`
   - `Araponga.Infrastructure` → referencia `Domain.Core`
   - `Araponga.Modules.Feed` → referencia `Domain.Core`

### ⚠️ Pendências da Fase 1:

- [ ] Atualizar namespaces nos projetos existentes:
  - `Araponga.Domain.Territories` → `Araponga.Domain.Core.Territories`
  - `Araponga.Domain.Users` → `Araponga.Domain.Core.Users`
  - `Araponga.Domain.Membership` → `Araponga.Domain.Core.Membership`
- [ ] Adicionar referência em `Araponga.Api` e `Araponga.Tests`
- [ ] Executar testes para validar mudanças

---

## 🚧 Fase 2: Módulo Feed - EM ANDAMENTO

### O que foi feito:

1. ✅ **Estrutura de pastas criada**:
   - `Araponga.Modules.Feed/Domain/` - Entidades criadas
   - `Araponga.Modules.Feed/Application/` - Estrutura criada

2. ✅ **Entidades de Feed criadas no módulo**:
   - `Post.cs` (equivalente a `CommunityPost`)
   - `PostComment.cs`
   - `PostStatus.cs`, `PostType.cs`, `PostVisibility.cs`
   - `PostAsset.cs`

3. ✅ **Estrutura Application criada**:
   - `Application/Interfaces/IFeedService.cs` - Interface pública
   - `Application/Common/Result.cs`, `PagedResult.cs`, `PaginationParameters.cs`
   - `Application/Models/GeoAnchorInput.cs`, `PostCounts.cs`

4. ✅ **Referências atualizadas**:
   - `Araponga.Modules.Feed.csproj` → referencia `Domain.Core`

### ⚠️ Pendências da Fase 2:

- [ ] **Criar alias de compatibilidade**:
  ```csharp
  // Em Araponga.Domain.Feed (temporário)
  using Post = Araponga.Modules.Feed.Domain.Post;
  using CommunityPost = Araponga.Modules.Feed.Domain.Post;
  ```

- [ ] **Mover services de Feed para módulo**:
  - `FeedService` → `Araponga.Modules.Feed.Application/Services/FeedService.cs`
  - `PostCreationService` → `Araponga.Modules.Feed.Application/Services/PostCreationService.cs`
  - `PostFilterService` → `Araponga.Modules.Feed.Application/Services/PostFilterService.cs`
  - `PostInteractionService` → `Araponga.Modules.Feed.Application/Services/PostInteractionService.cs`
  - `PostEditService` → `Araponga.Modules.Feed.Application/Services/PostEditService.cs`

- [ ] **Atualizar FeedService para implementar IFeedService**

- [ ] **Atualizar FeedModule**:
  ```csharp
  services.AddScoped<IFeedService, FeedService>();
  ```

- [ ] **Atualizar referências**:
  - `Araponga.Api` → usar `IFeedService` ao invés de `FeedService`
  - `Araponga.Tests` → atualizar testes

---

## 📊 Estrutura Atual vs. Desejada

### Estrutura Atual (Parcial):

```
Araponga.Domain.Core/          ✅ Criado
├── Territories/               ✅ Territory, TerritoryStatus
├── Users/                     ✅ User, UserIdentityVerificationStatus
└── Membership/               ✅ TerritoryMembership, MembershipRole, etc.

Araponga.Domain/               ⚠️ Ainda contém Feed, Marketplace, etc.
└── Feed/                      ⚠️ Ainda existe (será removido depois)

Araponga.Modules.Feed/         🚧 Em refatoração
├── Domain/                    ✅ Criado (Post, PostComment, etc.)
├── Application/               ✅ Estrutura criada
│   ├── Interfaces/           ✅ IFeedService
│   ├── Common/               ✅ Result, PagedResult
│   └── Models/               ✅ GeoAnchorInput, PostCounts
└── FeedModule.cs             ⚠️ Precisa atualizar

Araponga.Application/          ⚠️ Ainda contém FeedService, etc.
└── Services/
    ├── FeedService.cs         ⚠️ Precisa mover para módulo
    └── PostCreationService.cs ⚠️ Precisa mover para módulo
```

### Estrutura Desejada (Final):

```
Araponga.Domain.Core/          ✅ Apenas compartilhado
├── Territories/
├── Users/
└── Membership/

Araponga.Modules.Feed/         ✅ Independente
├── Domain/                   ✅ Post, PostComment, etc.
├── Application/              ✅ Services, Interfaces, DTOs
│   ├── Services/            ⚠️ FeedService, PostCreationService, etc.
│   ├── Interfaces/           ✅ IFeedService
│   └── DTOs/                 ⚠️ Criar DTOs se necessário
└── FeedModule.cs             ⚠️ Registrar tudo

Araponga.Application/          ⚠️ Apenas serviços compartilhados
└── Services/
    ├── TerritoryService.cs   ✅ Compartilhado
    └── MembershipService.cs ✅ Compartilhado
```

---

## 🎯 Próximos Passos Imediatos

### 🆕 NOVO: Plano de Modularização com Desacoplamento Real

**Documento Principal**: `docs/PLANO_MODULARIZACAO_DESACOPLAMENTO_REAL.md`

**Objetivo**: Implementar slice da infraestrutura para garantir desacoplamento real, onde cada módulo tem sua própria camada de infraestrutura.

**Fases**:
1. ✅ Criar `Araponga.Infrastructure.Shared` (infraestrutura compartilhada)
2. ✅ Criar `Araponga.Modules.Feed.Infrastructure` (exemplo)
3. ✅ Criar Infrastructure para outros módulos
4. ✅ Refatorar API e testes
5. ✅ Limpeza e otimização

**Prioridade**: 🔴 **ALTA** - Essencial para desacoplamento real

---

### 1. Completar Fase 2 - Módulo Feed

**Prioridade**: Alta

1. Criar alias de compatibilidade temporária em `Araponga.Domain.Feed`:
   ```csharp
   namespace Araponga.Domain.Feed;
   using Post = Araponga.Modules.Feed.Domain.Post;
   using CommunityPost = Araponga.Modules.Feed.Domain.Post;
   ```

2. Mover services para módulo Feed:
   - Copiar `FeedService.cs` para `Araponga.Modules.Feed.Application/Services/`
   - Atualizar namespace: `Araponga.Modules.Feed.Application.Services`
   - Atualizar usos de `CommunityPost` para `Post`
   - Fazer implementar `IFeedService`

3. Atualizar `FeedModule`:
   ```csharp
   services.AddScoped<IFeedService, FeedService>();
   services.AddScoped<PostCreationService>();
   // ...
   ```

4. Atualizar `Araponga.Api`:
   - Usar `IFeedService` ao invés de `FeedService`
   - Atualizar usos de `Araponga.Domain.Feed` para `Araponga.Modules.Feed.Domain`

### 2. Atualizar Namespaces (Fase 1 - Pendente)

**Prioridade**: Média

- Atualizar todos os usos de `Araponga.Domain.Territories` → `Araponga.Domain.Core.Territories`
- Atualizar todos os usos de `Araponga.Domain.Users` → `Araponga.Domain.Core.Users`
- Atualizar todos os usos de `Araponga.Domain.Membership` → `Araponga.Domain.Core.Membership`

**Ferramenta**: Usar "Find and Replace" no IDE ou script de refatoração.

---

## ⚠️ Pontos de Atenção

### 1. Compatibilidade Temporária

**Estratégia**: Criar aliases temporários para não quebrar código existente durante a migração.

```csharp
// Araponga.Domain.Feed/CommunityPost.cs (temporário)
namespace Araponga.Domain.Feed;
using CommunityPost = Araponga.Modules.Feed.Domain.Post;
```

**Quando remover**: Após atualizar todos os usos.

### 2. Dependências Circulares

**Problema**: Services de Feed podem depender de outros módulos.

**Solução**: Usar interfaces públicas e eventos.

### 3. Testes

**Estratégia**: Atualizar testes incrementalmente conforme refatoração avança.

**Ordem**:
1. Atualizar testes de Domain.Core
2. Atualizar testes de Feed
3. Executar suite completa

---

## 📈 Progresso Geral

| Fase | Status | Progresso |
|------|--------|-----------|
| **Fase 1: Domain.Core** | ✅ Concluída | 100% |
| **Fase 2: Módulo Feed** | 🚧 Em Andamento | 40% |
| **Fase 3: Módulo Marketplace** | ⏳ Pendente | 0% |
| **Fase 4: Outros Módulos** | ⏳ Pendente | 0% |
| **Fase 5: Refatorar API** | ⏳ Pendente | 0% |

---

## 🚀 Comandos Úteis

### Verificar Compilação

```bash
dotnet build backend/Araponga.Domain.Core/Araponga.Domain.Core.csproj
dotnet build backend/Araponga.Modules.Feed/Araponga.Modules.Feed.csproj
dotnet build backend/Araponga.Application/Araponga.Application.csproj
```

### Executar Testes

```bash
dotnet test backend/Araponga.Tests/Araponga.Tests.csproj
```

### Buscar Usos de Namespaces

```bash
# PowerShell
Select-String -Path "backend\**\*.cs" -Pattern "using Araponga\.Domain\.(Territories|Users|Membership)"
```

---

**Última Atualização**: 2026-01-26  
**Próxima Revisão**: Após completar Fase 2
