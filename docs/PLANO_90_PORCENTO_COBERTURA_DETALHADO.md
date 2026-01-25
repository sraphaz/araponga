# 🎯 Plano Detalhado para Atingir 90% de Cobertura

**Data**: 2026-01-24  
**Status Atual**:
- Domain Layer: **82.23% linhas, 74.39% branches** (gap: ~8% linhas, ~16% branches)
- Application Layer: **66.37% linhas, 50.39% branches** (gap: ~24% linhas, ~40% branches)

**Meta**: 90%+ em ambas as camadas

---

## 📊 Análise de Gap

### Domain Layer (82.23% → 90%)

**Gap**: ~8% linhas, ~16% branches  
**Prioridade**: 🟡 Média (já está próximo)

#### O que falta:
1. **Cobertura de Branches** (74.39% → 90%)
   - Testar todos os caminhos condicionais
   - Testar todos os casos de validação
   - Testar transições de estado

2. **Entidades com Baixa Cobertura** (estimado):
   - Entidades de eventos (TerritoryEvent, EventParticipation)
   - Entidades de chat (ChatConversation, ChatMessage)
   - Entidades de mídia (MediaAttachment edge cases)
   - Entidades financeiras (edge cases de valores)

**Estimativa**: ~30-40 testes adicionais focados em branches e edge cases

---

### Application Layer (66.37% → 90%)

**Gap**: ~24% linhas, ~40% branches  
**Prioridade**: 🔴 Alta (maior gap)

#### Serviços com Baixa Cobertura Identificados:

##### 1. Serviços sem Testes de Edge Cases (Alta Prioridade)

| Serviço | Linhas Estimadas | Prioridade | Testes Necessários |
|---------|------------------|------------|-------------------|
| AnalyticsService | ~200-300 | 🟡 Média | 8-12 testes |
| UserProfileService | ~150-200 | 🟡 Média | 6-10 testes |
| UserProfileStatsService | ~100-150 | 🟡 Média | 5-8 testes |
| UserPreferencesService | ~100-150 | 🟡 Média | 5-8 testes |
| UserInterestService | ~100-150 | 🟡 Média | 5-8 testes |
| InterestFilterService | ~150-200 | 🟡 Média | 6-10 testes |
| TermsAcceptanceService | ~100-150 | 🟡 Média | 5-8 testes |
| PrivacyPolicyAcceptanceService | ~100-150 | 🟡 Média | 5-8 testes |
| TermsOfServiceService | ~100-150 | 🟡 Média | 5-8 testes |
| PrivacyPolicyService | ~100-150 | 🟡 Média | 5-8 testes |
| PolicyRequirementService | ~150-200 | 🟡 Média | 6-10 testes |
| ResidencyRequestService | ~200-300 | 🔴 Alta | 8-12 testes |
| AccountDeletionService | ~200-300 | 🔴 Alta | 8-12 testes |
| DataExportService | ~150-200 | 🟡 Média | 6-10 testes |
| PushNotificationService | ~150-200 | 🟡 Média | 6-10 testes |
| NotificationConfigService | ~100-150 | 🟡 Média | 5-8 testes |
| UserBlockService | ~150-200 | 🟡 Média | 6-10 testes |
| TerritoryModerationService | ~200-300 | 🔴 Alta | 8-12 testes |
| PlatformFeeService | ~150-200 | 🔴 Alta | 6-10 testes |
| SellerPayoutService | ~200-300 | 🔴 Alta | 8-12 testes |
| MarketplaceSearchService | ~150-200 | 🟡 Média | 6-10 testes |
| InputSanitizationService | ~100-150 | 🟡 Média | 5-8 testes |
| AuditService | ~100-150 | 🟡 Média | 5-8 testes |
| CacheInvalidationService | ~100-150 | 🟡 Média | 5-8 testes |
| VotingService | ~150-200 | 🟡 Média | 6-10 testes |

**Total Estimado**: ~150-200 testes

##### 2. Serviços com Testes Básicos (Precisam Edge Cases)

| Serviço | Status Atual | Edge Cases Faltantes | Testes Necessários |
|---------|--------------|---------------------|-------------------|
| MembershipService | ✅ Testes básicos | Edge cases de transições | 8-12 testes |
| TerritoryService | ✅ Testes básicos | Edge cases de validação | 6-10 testes |
| MapService | ✅ Testes básicos | Edge cases de coordenadas | 6-10 testes |
| PostCreationService | ✅ Testes básicos | Edge cases de validação | 8-12 testes |
| PostInteractionService | ✅ Testes básicos | Edge cases de interação | 6-10 testes |
| PostEditService | ✅ Testes básicos | Edge cases de edição | 6-10 testes |
| PostFilterService | ✅ Testes básicos | Edge cases de filtros | 5-8 testes |
| StoreService | ✅ Testes básicos | Edge cases de marketplace | 8-12 testes |
| StoreItemService | ✅ Testes básicos | Edge cases de items | 6-10 testes |
| CartService | ✅ Testes básicos | Edge cases de carrinho | 6-10 testes |
| InquiryService | ✅ Testes básicos | Edge cases de inquiries | 5-8 testes |
| RatingService | ✅ Testes básicos | Edge cases de ratings | 5-8 testes |
| DocumentEvidenceService | ✅ Testes básicos | Edge cases de documentos | 6-10 testes |
| HealthService | ✅ Testes básicos | Edge cases de health checks | 5-8 testes |
| ActiveTerritoryService | ✅ Testes básicos | Edge cases de território ativo | 5-8 testes |
| SystemConfigService | ✅ Testes básicos | Edge cases adicionais | 4-6 testes |
| SystemPermissionService | ✅ Testes básicos | Edge cases de permissões | 6-10 testes |
| MembershipCapabilityService | ✅ Testes básicos | Edge cases de capabilities | 5-8 testes |
| WorkQueueService | ✅ Testes básicos | Edge cases de work queue | 6-10 testes |
| VerificationQueueService | ✅ Testes básicos | Edge cases de verificação | 6-10 testes |
| TerritoryPayoutConfigService | ✅ Testes básicos | Edge cases de payout | 6-10 testes |
| MediaStorageConfigService | ✅ Testes básicos | Edge cases de storage | 5-8 testes |
| TerritoryMediaConfigService | ✅ Testes básicos | Edge cases de media config | 5-8 testes |

**Total Estimado**: ~150-200 testes

##### 3. Serviços de Cache (Precisam Mais Cobertura de Branches)

| Serviço | Status | Testes Necessários |
|---------|--------|-------------------|
| AlertCacheService | ✅ Edge cases básicos | 4-6 testes adicionais |
| EventCacheService | ✅ Edge cases básicos | 4-6 testes adicionais |
| FeatureFlagCacheService | ✅ Edge cases básicos | 4-6 testes adicionais |
| MapEntityCacheService | ✅ Edge cases básicos | 4-6 testes adicionais |
| TerritoryCacheService | ✅ Edge cases básicos | 4-6 testes adicionais |
| UserBlockCacheService | ✅ Edge cases básicos | 4-6 testes adicionais |
| SystemConfigCacheService | ✅ Edge cases completos | ✅ Completo |

**Total Estimado**: ~25-35 testes

---

## 🎯 Plano de Ação Prioritizado

### Fase 1: Application Layer - Serviços Críticos (Alta Prioridade)

**Objetivo**: Aumentar de 66.37% para ~80%  
**Estimativa**: 80-100 testes  
**Tempo**: 2-3 semanas

#### Serviços Prioritários:

1. **ResidencyRequestService** (8-12 testes)
   - Edge cases de validação de residência
   - Status transitions
   - Error handling

2. **AccountDeletionService** (8-12 testes)
   - Edge cases de deleção
   - Validação de dependências
   - Error handling

3. **TerritoryModerationService** (8-12 testes)
   - Edge cases de moderação
   - Status transitions
   - Error handling

4. **PlatformFeeService** (6-10 testes)
   - Edge cases de taxas
   - Validação de valores
   - Error handling

5. **SellerPayoutService** (8-12 testes)
   - Edge cases de payout
   - Validação de valores
   - Status transitions

6. **StoreService** (8-12 testes)
   - Edge cases de marketplace
   - Status transitions
   - Validação de dados

7. **PostCreationService** (8-12 testes)
   - Edge cases de criação
   - Validação de conteúdo
   - Error handling

8. **MembershipService** (8-12 testes)
   - Edge cases de transições
   - Validação de membership
   - Error handling

**Total Fase 1**: ~60-90 testes

---

### Fase 2: Application Layer - Serviços Médios (Média Prioridade)

**Objetivo**: Aumentar de ~80% para ~85%  
**Estimativa**: 60-80 testes  
**Tempo**: 2-3 semanas

#### Serviços:

1. **UserProfileService** (6-10 testes)
2. **UserProfileStatsService** (5-8 testes)
3. **UserPreferencesService** (5-8 testes)
4. **UserInterestService** (5-8 testes)
5. **InterestFilterService** (6-10 testes)
6. **AnalyticsService** (8-12 testes)
7. **DataExportService** (6-10 testes)
8. **PushNotificationService** (6-10 testes)
9. **UserBlockService** (6-10 testes)
10. **StoreItemService** (6-10 testes)
11. **CartService** (6-10 testes)
12. **InquiryService** (5-8 testes)
13. **RatingService** (5-8 testes)
14. **DocumentEvidenceService** (6-10 testes)
15. **SystemPermissionService** (6-10 testes)
16. **MembershipCapabilityService** (5-8 testes)
17. **WorkQueueService** (6-10 testes)
18. **VerificationQueueService** (6-10 testes)
19. **TerritoryPayoutConfigService** (6-10 testes)
20. **PostInteractionService** (6-10 testes)
21. **PostEditService** (6-10 testes)
22. **PostFilterService** (5-8 testes)
23. **TerritoryService** (6-10 testes)
24. **MapService** (6-10 testes)
25. **TermsAcceptanceService** (5-8 testes)
26. **PrivacyPolicyAcceptanceService** (5-8 testes)
27. **TermsOfServiceService** (5-8 testes)
28. **PrivacyPolicyService** (5-8 testes)
29. **PolicyRequirementService** (6-10 testes)
30. **NotificationConfigService** (5-8 testes)
31. **InputSanitizationService** (5-8 testes)
32. **AuditService** (5-8 testes)
33. **CacheInvalidationService** (5-8 testes)
34. **VotingService** (6-10 testes)
35. **MarketplaceSearchService** (6-10 testes)
36. **MediaStorageConfigService** (5-8 testes)
37. **TerritoryMediaConfigService** (5-8 testes)
38. **HealthService** (5-8 testes)
39. **ActiveTerritoryService** (5-8 testes)
40. **SystemConfigService** (4-6 testes)

**Total Fase 2**: ~220-300 testes

---

### Fase 3: Application Layer - Cobertura de Branches

**Objetivo**: Aumentar de ~85% para 90%+  
**Estimativa**: 50-70 testes focados em branches  
**Tempo**: 1-2 semanas

#### Foco:
- Testar todos os caminhos condicionais
- Testar todos os casos de erro
- Testar todas as validações
- Testar todas as transições de estado

---

### Fase 4: Domain Layer - Cobertura de Branches

**Objetivo**: Aumentar de 74.39% para 90%+  
**Estimativa**: 30-40 testes focados em branches  
**Tempo**: 1 semana

#### Foco:
- Testar todas as validações de entidades
- Testar todas as transições de estado
- Testar todos os edge cases de value objects
- Testar todos os métodos de domínio

---

## 📋 Resumo do Plano

| Fase | Camada | Testes | Objetivo | Tempo |
|------|--------|--------|----------|-------|
| Fase 1 | Application | 60-90 | 66% → 80% | 2-3 semanas |
| Fase 2 | Application | 220-300 | 80% → 85% | 2-3 semanas |
| Fase 3 | Application | 50-70 | 85% → 90%+ | 1-2 semanas |
| Fase 4 | Domain | 30-40 | 74% → 90%+ | 1 semana |
| **Total** | **Ambas** | **360-500** | **90%+** | **6-9 semanas** |

---

## 🎯 Priorização por Impacto

### 🔴 Alta Prioridade (Fase 1)
**Razão**: Maior impacto na cobertura (66% → 80%)

1. ResidencyRequestService
2. AccountDeletionService
3. TerritoryModerationService
4. PlatformFeeService
5. SellerPayoutService
6. StoreService
7. PostCreationService
8. MembershipService

**Impacto Esperado**: +14% cobertura (66% → 80%)

### 🟡 Média Prioridade (Fase 2)
**Razão**: Aumentar de 80% para 85%

- Serviços de usuário (Profile, Preferences, Interest)
- Serviços de marketplace (StoreItem, Cart, Inquiry)
- Serviços de sistema (Permissions, Capabilities, WorkQueue)
- Serviços de conteúdo (PostInteraction, PostEdit, PostFilter)

**Impacto Esperado**: +5% cobertura (80% → 85%)

### 🟢 Baixa Prioridade (Fase 3 e 4)
**Razão**: Refinamento para atingir 90%+

- Cobertura de branches
- Edge cases adicionais
- Validações completas

**Impacto Esperado**: +5% cobertura (85% → 90%+)

---

## 📊 Métricas de Sucesso

### Por Fase
- ✅ **Fase 1**: Application Layer ≥ 80%
- ✅ **Fase 2**: Application Layer ≥ 85%
- ✅ **Fase 3**: Application Layer ≥ 90%
- ✅ **Fase 4**: Domain Layer ≥ 90%

### Geral
- ✅ **Domain Layer**: ≥ 90% linhas e branches
- ✅ **Application Layer**: ≥ 90% linhas e branches
- ✅ **Taxa de Sucesso**: 100%
- ✅ **Zero Regressions**: Mantido

---

## 🔧 Estratégia de Implementação

### Abordagem Incremental
1. **Fase por Fase**: Implementar uma fase por vez
2. **Testes Passando**: Garantir 100% de sucesso antes de avançar
3. **Code Review**: Revisar cada fase antes de merge
4. **Documentação**: Documentar cada fase conforme implementada

### Padrões Estabelecidos
- ✅ Padrão AAA (Arrange-Act-Assert)
- ✅ Nomenclatura: `MethodName_Scenario_ExpectedBehavior`
- ✅ Comentários descritivos
- ✅ Testes isolados e independentes

---

## 📝 Checklist de Implementação

### Para Cada Serviço
- [ ] Identificar métodos com baixa cobertura
- [ ] Criar arquivo de testes (se não existir)
- [ ] Implementar edge cases
- [ ] Garantir 100% de sucesso
- [ ] Validar aumento de cobertura

### Geral
- [ ] Todas as fases completas
- [ ] 90%+ em todas as camadas
- [ ] Documentação atualizada
- [ ] Métricas validadas

---

## 💡 Dicas de Implementação

### 1. Focar em Branches (Application Layer)
- Application Layer tem apenas 50.39% de cobertura de branches
- Priorizar testes que cubram caminhos condicionais
- Testar todos os casos de erro
- Testar todas as validações

### 2. Reutilizar Padrões
- Usar os padrões estabelecidos nas fases anteriores
- Seguir estrutura de arquivos existente
- Manter consistência de nomenclatura

### 3. Priorizar Impacto
- Começar pelos serviços mais críticos
- Focar em áreas com maior gap
- Validar com métricas reais após cada fase

---

## 📈 Progresso Esperado

### Timeline Estimada

| Semana | Fase | Testes | Cobertura Esperada |
|--------|------|--------|-------------------|
| 1-3 | Fase 1 | 60-90 | Application: 66% → 80% |
| 4-6 | Fase 2 | 220-300 | Application: 80% → 85% |
| 7-8 | Fase 3 | 50-70 | Application: 85% → 90% |
| 9 | Fase 4 | 30-40 | Domain: 74% → 90% |

**Total**: 6-9 semanas para 90%+ em todas as camadas

---

## ✅ Critérios de Aceitação

### Para Cada Fase
- ✅ Todos os testes passando (100%)
- ✅ Zero regressions
- ✅ Build succeeds
- ✅ Documentação atualizada
- ✅ Code review aprovado
- ✅ Cobertura validada

### Final
- ✅ 90%+ cobertura em todas as camadas
- ✅ Média geral ≥ 90%
- ✅ 100% taxa de sucesso
- ✅ Documentação completa

---

**Status**: 📋 Planejado  
**Próxima Fase**: Fase 1 (Application Layer - Serviços Críticos)  
**Estimativa Total**: 360-500 testes adicionais em 6-9 semanas
