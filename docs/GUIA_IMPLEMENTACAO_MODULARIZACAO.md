# Guia de Implementação: Modularização com Desacoplamento Real

**Data**: 2026-01-27  
**Status**: ✅ Fases 1, 2, 3 e 4 (Events, Map, Chat, Subscriptions, Moderation, Notifications, Alerts, Assets, Admin) Completas - **FASE 4 COMPLETA** ✅

---

## 📋 Checklist de Implementação

### ✅ Fase 1: Infrastructure.Shared - COMPLETA ✅

- [x] Criar projeto `Araponga.Infrastructure.Shared`
- [x] Adicionar ao solution
- [x] Configurar dependências
- [x] Criar estrutura de pastas
- [x] Criar `SharedDbContext` base
- [x] Copiar entidades compartilhadas para `Postgres/Entities/` (14 entidades)
- [x] Completar configurações do `SharedDbContext.OnModelCreating`
- [x] Mover repositórios compartilhados (10 repositórios)
- [x] Mover serviços cross-cutting (6 serviços + 3 FileStorage)
- [x] Criar `ServiceCollectionExtensions` com `AddSharedInfrastructure()` e `AddSharedCrossCuttingServices()`

**📄 Documentação**: Ver `FASE1_INFRASTRUCTURE_SHARED_COMPLETA.md`

### ✅ Fase 2: Feed.Infrastructure - COMPLETA ✅

- [x] Criar projeto `Araponga.Modules.Feed.Infrastructure`
- [x] Adicionar ao solution
- [x] Configurar dependências (sem dependência circular)
- [x] Criar estrutura de pastas
- [x] Criar `FeedDbContext` base
- [x] Copiar entidades de Feed para `Postgres/Entities/` (6 entidades)
- [x] Completar configurações do `FeedDbContext.OnModelCreating`
- [x] Criar `FeedMappers` (CommunityPost, PostComment)
- [x] Mover repositórios de Feed (1 repositório: PostgresFeedRepository)
- [x] Criar `ServiceCollectionExtensions` com `AddFeedInfrastructure()`
- [x] Atualizar `FeedModule` para usar `AddFeedInfrastructure()`

**📄 Documentação**: Ver `FASE2_FEED_INFRASTRUCTURE_COMPLETA.md`

### ✅ Fase 3: Marketplace.Infrastructure - COMPLETA ✅

- [x] Criar projeto `Araponga.Modules.Marketplace.Infrastructure`
- [x] Adicionar ao solution
- [x] Configurar dependências (sem dependência circular)
- [x] Criar estrutura de pastas
- [x] Criar `MarketplaceDbContext` base
- [x] Copiar entidades de Marketplace para `Postgres/Entities/` (12 entidades)
- [x] Completar configurações do `MarketplaceDbContext.OnModelCreating`
- [x] Criar `MarketplaceMappers` (Store, StoreItem, ItemInquiry, Cart, CartItem, Checkout, CheckoutItem, PlatformFeeConfig)
- [x] Mover repositórios de Marketplace (4 repositórios: Store, StoreItem, Cart, Inquiry)
- [x] Criar `ServiceCollectionExtensions` com `AddMarketplaceInfrastructure()`
- [x] Atualizar `MarketplaceModule` para usar `AddMarketplaceInfrastructure()`

**📄 Documentação**: Ver `FASE3_MARKETPLACE_INFRASTRUCTURE_COMPLETA.md`

### ✅ Fase 4 (Parte 1): Events.Infrastructure - COMPLETA ✅

- [x] Criar projeto `Araponga.Modules.Events.Infrastructure`
- [x] Adicionar ao solution
- [x] Configurar dependências (sem dependência circular)
- [x] Criar estrutura de pastas
- [x] Criar `EventsDbContext` base
- [x] Copiar entidades de Events para `Postgres/Entities/` (2 entidades)
- [x] Completar configurações do `EventsDbContext.OnModelCreating`
- [x] Criar `EventsMappers` (TerritoryEvent, EventParticipation)
- [x] Mover repositórios de Events (2 repositórios: TerritoryEvent, EventParticipation)
- [x] Criar `ServiceCollectionExtensions` com `AddEventsInfrastructure()`
- [x] Atualizar `EventsModule` para usar `AddEventsInfrastructure()`

**📄 Documentação**: Ver `FASE4_EVENTS_INFRASTRUCTURE_COMPLETA.md`

### ✅ Fase 4 (Parte 2): Map.Infrastructure - COMPLETA ✅

- [x] Criar projeto `Araponga.Modules.Map.Infrastructure`
- [x] Adicionar ao solution
- [x] Configurar dependências (sem dependência circular)
- [x] Criar estrutura de pastas
- [x] Criar `MapDbContext` base
- [x] Copiar entidades de Map para `Postgres/Entities/` (2 entidades)
- [x] Completar configurações do `MapDbContext.OnModelCreating`
- [x] Criar `MapMappers` (MapEntity, MapEntityRelation)
- [x] Mover repositórios de Map (2 repositórios: Map, MapEntityRelation)
- [x] Criar `ServiceCollectionExtensions` com `AddMapInfrastructure()`
- [x] Atualizar `MapModule` para usar `AddMapInfrastructure()`

**📄 Documentação**: Ver `FASE4_MAP_INFRASTRUCTURE_COMPLETA.md`

### ✅ Fase 4 (Parte 8): Assets.Infrastructure - COMPLETA ✅

- [x] Criar projeto `Araponga.Modules.Assets.Infrastructure`
- [x] Adicionar ao solution
- [x] Configurar dependências (sem dependência circular)
- [x] Criar estrutura de pastas
- [x] Criar `AssetsDbContext` base
- [x] Copiar entidades de Assets para `Postgres/Entities/` (6 entidades: TerritoryAsset, AssetGeoAnchor, AssetValidation, PostAsset, MediaAsset, MediaAttachment)
- [x] Completar configurações do `AssetsDbContext.OnModelCreating`
- [x] Criar `AssetsMappers` (TerritoryAsset, AssetGeoAnchor, AssetValidation, PostAsset, MediaAsset, MediaAttachment)
- [x] Mover repositórios de Assets (6 repositórios: Asset, AssetGeoAnchor, AssetValidation, PostAsset, MediaAsset, MediaAttachment)
- [x] Criar `ServiceCollectionExtensions` com `AddAssetsInfrastructure()`
- [x] Atualizar `AssetsModule` para usar `AddAssetsInfrastructure()`

**📄 Documentação**: Ver `FASE4_ASSETS_INFRASTRUCTURE_COMPLETA.md`

### ✅ Fase 4 (Parte 9): Admin.Infrastructure - COMPLETA ✅

- [x] Criar projeto `Araponga.Modules.Admin.Infrastructure`
- [x] Adicionar ao solution
- [x] Configurar dependências (sem dependência circular)
- [x] Criar estrutura de pastas
- [x] Criar `AdminDbContext` base
- [x] Copiar entidades de Admin para `Postgres/Entities/` (2 entidades: WorkItem, DocumentEvidence)
- [x] Completar configurações do `AdminDbContext.OnModelCreating`
- [x] Criar `AdminMappers` (WorkItem, DocumentEvidence)
- [x] Mover repositórios de Admin (2 repositórios: WorkItem, DocumentEvidence)
- [x] Criar `ServiceCollectionExtensions` com `AddAdminInfrastructure()`
- [x] Atualizar `AdminModule` para usar `AddAdminInfrastructure()`

**📄 Documentação**: Ver `FASE4_ADMIN_INFRASTRUCTURE_COMPLETA.md`

---

## 🔧 Passos Detalhados

### 1. Copiar Entidades Compartilhadas

**De**: `backend/Araponga.Infrastructure/Postgres/Entities/`  
**Para**: `backend/Araponga.Infrastructure.Shared/Postgres/Entities/`

**Entidades a copiar**:
- `TerritoryRecord.cs`
- `UserRecord.cs`
- `UserPreferencesRecord.cs`
- `UserDeviceRecord.cs`
- `UserInterestRecord.cs`
- `TerritoryMembershipRecord.cs`
- `MembershipSettingsRecord.cs`
- `MembershipCapabilityRecord.cs`
- `SystemPermissionRecord.cs`
- `SystemConfigRecord.cs`
- `OutboxMessageRecord.cs`
- `AuditEntryRecord.cs`
- `FeatureFlagRecord.cs`
- `ActiveTerritoryRecord.cs`

**Ação**:
```bash
# Copiar arquivos (ajustar namespaces depois)
Copy-Item backend\Araponga.Infrastructure\Postgres\Entities\TerritoryRecord.cs backend\Araponga.Infrastructure.Shared\Postgres\Entities\
# ... repetir para cada entidade
```

**Atualizar namespaces**:
- `Araponga.Infrastructure.Postgres.Entities` → `Araponga.Infrastructure.Shared.Postgres.Entities`

---

### 2. Completar SharedDbContext.OnModelCreating

**Copiar configurações do `ArapongaDbContext.OnModelCreating`** para as entidades compartilhadas:

- Territory (linhas 167-183)
- User (linhas 185-206)
- UserPreferences (linhas 208-221)
- UserInterest (linhas 223-236)
- TerritoryMembership (linhas 312-329)
- MembershipSettings (linhas 331-342)
- MembershipCapability (linhas 344-359)
- SystemPermission (linhas 361-378)
- SystemConfig (linhas 380-392)
- OutboxMessage (linhas 700-713)
- AuditEntry (linhas 648-657)
- FeatureFlag (linhas 640-646)
- ActiveTerritory (linhas 631-638)

---

### 3. Mover Repositórios Compartilhados

**De**: `backend/Araponga.Infrastructure/Postgres/`  
**Para**: `backend/Araponga.Infrastructure.Shared/Repositories/`

**Repositórios a mover**:
1. `PostgresTerritoryRepository.cs`
2. `PostgresUserRepository.cs`
3. `PostgresUserPreferencesRepository.cs`
4. `PostgresUserDeviceRepository.cs`
5. `PostgresUserInterestRepository.cs`
6. `PostgresTerritoryMembershipRepository.cs`
7. `PostgresMembershipSettingsRepository.cs`
8. `PostgresMembershipCapabilityRepository.cs`
9. `PostgresSystemPermissionRepository.cs`
10. `PostgresSystemConfigRepository.cs`

**Ações**:
1. Copiar arquivo
2. Atualizar namespace: `Araponga.Infrastructure.Postgres` → `Araponga.Infrastructure.Shared.Repositories`
3. Atualizar referências ao DbContext: `ArapongaDbContext` → `SharedDbContext`
4. Atualizar referências às entidades: `Araponga.Infrastructure.Postgres.Entities` → `Araponga.Infrastructure.Shared.Postgres.Entities`

---

### 4. Mover Serviços Cross-Cutting

**CacheService**:
- De: `backend/Araponga.Infrastructure/Caching/`
- Para: `backend/Araponga.Infrastructure.Shared/Services/`
- Atualizar namespace

**EmailService**:
- De: `backend/Araponga.Infrastructure/Email/`
- Para: `backend/Araponga.Infrastructure.Shared/Services/`
- Atualizar namespace

**MediaStorageService**:
- De: `backend/Araponga.Infrastructure/FileStorage/`
- Para: `backend/Araponga.Infrastructure.Shared/Services/`
- Atualizar namespace

**EventBus**:
- De: `backend/Araponga.Infrastructure/Eventing/`
- Para: `backend/Araponga.Infrastructure.Shared/Services/`
- Atualizar namespace

**Outbox**:
- De: `backend/Araponga.Infrastructure/Outbox/`
- Para: `backend/Araponga.Infrastructure.Shared/Services/`
- Atualizar namespace

**AuditLogger**:
- De: `backend/Araponga.Infrastructure/Postgres/PostgresAuditLogger.cs`
- Para: `backend/Araponga.Infrastructure.Shared/Services/`
- Atualizar namespace e referências ao DbContext

---

### 5. Atualizar Referências

**Projetos que precisam referenciar `Araponga.Infrastructure.Shared`**:
- `Araponga.Api`
- `Araponga.Application` (se necessário)
- Todos os módulos que usam infraestrutura compartilhada

**Atualizar `Program.cs`**:
```csharp
// Antes
services.AddDbContext<ArapongaDbContext>(...);

// Depois
services.AddDbContext<SharedDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("SharedDb")));
```

---

## 🚀 Próximas Fases

### ⏳ Fase 4: Outros Módulos

Criar Infrastructure para módulos restantes:
- Events
- Map
- Chat
- Subscriptions
- Moderation
- Notifications
- Alerts
- Assets
- Admin

---

## ⚠️ Atenção

1. **Não remover `Araponga.Infrastructure` ainda** - manter até migração completa
2. **Atualizar testes** conforme mover arquivos
3. **Validar build** após cada etapa
4. **Executar testes** após cada etapa

---

**Última Atualização**: 2026-01-27
