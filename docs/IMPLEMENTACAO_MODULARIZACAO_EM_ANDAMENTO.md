# Implementação da Modularização - Em Andamento

**Data**: 2026-01-27  
**Status**: 🚧 Implementação Iniciada

---

## ✅ Progresso

### Fase 1: Infrastructure.Shared - EM ANDAMENTO

1. ✅ Projeto `Araponga.Infrastructure.Shared` criado
2. ✅ Projeto adicionado ao solution
3. ✅ Dependências configuradas
4. ⏳ Criar estrutura de pastas
5. ⏳ Mover SharedDbContext (apenas Territory, User, Membership)
6. ⏳ Mover repositórios compartilhados
7. ⏳ Mover serviços cross-cutting

---

## 📋 Próximos Passos Imediatos

### 1. Criar Estrutura de Pastas

```
Araponga.Infrastructure.Shared/
├── Postgres/
│   ├── SharedDbContext.cs
│   ├── Entities/ (apenas entidades compartilhadas)
│   └── Migrations/
├── Repositories/
│   ├── TerritoryRepository.cs
│   ├── UserRepository.cs
│   └── MembershipRepository.cs
└── Services/
    ├── CacheService.cs
    ├── EmailService.cs
    ├── MediaStorageService.cs
    └── EventBus.cs
```

### 2. Identificar Entidades Compartilhadas

**Entidades que vão para SharedDbContext**:
- `TerritoryRecord`
- `UserRecord`
- `UserPreferencesRecord`
- `UserDeviceRecord`
- `UserInterestRecord`
- `TerritoryMembershipRecord`
- `MembershipSettingsRecord`
- `MembershipCapabilityRecord`
- `SystemPermissionRecord`
- `SystemConfigRecord`
- `OutboxMessageRecord` (cross-cutting)
- `AuditEntryRecord` (cross-cutting)
- `FeatureFlagRecord` (cross-cutting)
- `ActiveTerritoryRecord` (cross-cutting)

**Entidades que NÃO vão (são específicas de módulos)**:
- Feed: `CommunityPostRecord`, `PostCommentRecord`, `PostLikeRecord`, `PostShareRecord`, `PostAssetRecord`, `PostGeoAnchorRecord`
- Marketplace: `TerritoryStoreRecord`, `StoreItemRecord`, `CartRecord`, `CartItemRecord`, `CheckoutRecord`, `CheckoutItemRecord`, `StoreRatingRecord`, etc.
- Events: `TerritoryEventRecord`, `EventParticipationRecord`
- Map: `MapEntityRecord`, `MapEntityRelationRecord`
- Chat: `ChatConversationRecord`, `ChatMessageRecord`, etc.
- Subscriptions: `SubscriptionPlanRecord`, `SubscriptionRecord`, etc.
- Moderation: `ModerationReportRecord`, `UserBlockRecord`, `SanctionRecord`
- Notifications: `UserNotificationRecord`, `NotificationConfigRecord`
- Alerts: `HealthAlertRecord`
- Assets: `TerritoryAssetRecord`, `AssetGeoAnchorRecord`, `AssetValidationRecord`
- Media: `MediaAssetRecord`, `MediaAttachmentRecord`
- Financial: `FinancialTransactionRecord`, `SellerBalanceRecord`, etc.
- Policies: `TermsOfServiceRecord`, `PrivacyPolicyRecord`, etc.
- WorkItems: `WorkItemRecord`
- JoinRequests: `TerritoryJoinRequestRecord`
- Voting: `VotingRecord`, `VoteRecord`
- ModerationRules: `TerritoryModerationRuleRecord`
- Characterizations: `TerritoryCharacterizationRecord`

---

## 🔧 Comandos Úteis

### Build
```bash
dotnet build backend/Araponga.Infrastructure.Shared/Araponga.Infrastructure.Shared.csproj
```

### Adicionar ao Solution
```bash
dotnet sln add backend/Araponga.Infrastructure.Shared/Araponga.Infrastructure.Shared.csproj
```

---

**Última Atualização**: 2026-01-27
