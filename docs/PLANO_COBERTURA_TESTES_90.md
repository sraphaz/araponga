# 📊 Plano de Aumento de Cobertura de Testes - 50% → 90%

**Data**: 2026-01-24  
**Objetivo**: Aumentar code coverage de ~50% para 90%+ (objetivo enterprise)  
**Status**: 📋 PLANEJAMENTO  

---

## 🎯 Situação Atual

### Métricas Base
- **Testes Totais**: 798 (100% passando)
- **Cobertura**: ~50%
- **Camadas**: Domain, Application, Infrastructure, API, Tests
- **Linhas de código**: ~40.000+

### Distribuição Atual
| Camada | Status | Estimativa |
|--------|--------|-----------|
| **Domain** | ✅ Bem testada | ~70-80% cobertura |
| **Application** | ⚠️ Parcial | ~40-50% cobertura |
| **Infrastructure** | ⚠️ Baixa | ~30-40% cobertura |
| **API** | ⚠️ Média | ~50-60% cobertura |
| **Helpers/Utils** | ❌ Baixa | ~20-30% cobertura |

---

## 📋 Cenários de Teste Faltantes

### CAMADA DOMAIN (Objetivo: 95%+)

#### 1. Territory Entity
- ✅ Criação com validações
- ❌ **Faltam**: Edge cases (caracteres especiais, limites de tamanho)
- ❌ **Faltam**: Validações de coordenadas inválidas (lat/long)
- ❌ **Faltam**: Imutabilidade de propriedades críticas
- ❌ **Faltam**: Métodos de domínio (Distance calculation, contains point)

#### 2. User Entity
- ✅ Autenticação básica
- ❌ **Faltam**: Validação de CPF vs Foreign Document (mutual exclusivity)
- ❌ **Faltam**: 2FA enable/disable workflows
- ❌ **Faltam**: Avatar/Bio update edge cases
- ❌ **Faltam**: Bio truncation (500 caracteres máximo)

#### 3. Post Entity
- ✅ Criação com interações
- ❌ **Faltam**: Tags validation (empty, duplicates, special chars)
- ❌ **Faltam**: GeoAnchor validation (invalid coordinates)
- ❌ **Faltam**: Media references validation
- ❌ **Faltam**: State transitions (draft → published → deleted)
- ❌ **Faltam**: Concurrency checks (version mismatches)

#### 4. Membership Entity
- ✅ Básico
- ❌ **Faltam**: Role transitions (visitor → resident → moderator)
- ❌ **Faltam**: Permission validation por role
- ❌ **Faltam**: Expiration/revocation scenarios
- ❌ **Faltam**: Duplicate membership prevention

#### 5. Voting/Governance Entities
- ✅ Criação e votação
- ❌ **Faltam**: Vote weight calculations
- ❌ **Faltam**: Majority/quorum checks
- ❌ **Faltam**: Vote finalization and results calculation
- ❌ **Faltam**: Edge cases (tie scenarios, low participation)

#### 6. Store/Item Marketplace
- ✅ Básico
- ❌ **Faltam**: Inventory management (stock depletion)
- ❌ **Faltam**: Pricing validation (negative, decimals)
- ❌ **Faltam**: Item state transitions
- ❌ **Faltam**: Platform fees calculation

#### 7. Notification/Alert Entities
- ✅ Criação
- ❌ **Faltam**: Notification routing rules
- ❌ **Faltam**: Delivery state transitions
- ❌ **Faltam**: Expiration scenarios
- ❌ **Faltam**: Alert escalation logic

---

### CAMADA APPLICATION (Objetivo: 85%)

#### 1. Services - Feed
- ✅ Criação de posts
- ❌ **Faltam**: Feed filtering by interests
- ❌ **Faltam**: Pagination edge cases (page 0, negative limits)
- ❌ **Faltam**: Feed sorting (chronological vs relevance)
- ❌ **Faltam**: Post deletion cascade (comments, likes, shares)
- ❌ **Faltam**: Media cleanup on post deletion

#### 2. Services - Marketplace
- ✅ Básico
- ❌ **Faltam**: Cart serialization/deserialization edge cases
- ❌ **Faltam**: Checkout with invalid items (out of stock)
- ❌ **Faltam**: Platform fee calculation variations
- ❌ **Faltam**: Seller balance reconciliation
- ❌ **Faltam**: Refund scenarios

#### 3. Services - Governance/Voting
- ✅ Criação e votação
- ❌ **Faltam**: Vote weight application
- ❌ **Faltam**: Result calculation algorithms
- ❌ **Faltam**: Voting period enforcement
- ❌ **Faltam**: Veto scenarios (curator/admin override)

#### 4. Services - Notifications
- ✅ Notificações básicas
- ❌ **Faltam**: Bulk notification scenarios
- ❌ **Faltam**: Notification deduplication
- ❌ **Faltam**: Delivery retry logic
- ❌ **Faltam**: Configuration per notification type

#### 5. Services - Chat
- ✅ Básico
- ❌ **Faltam**: Channel permission checks
- ❌ **Faltam**: Message moderation scenarios
- ❌ **Faltam**: File upload in messages
- ❌ **Faltam**: User timeout/mute scenarios

#### 6. Services - Media
- ✅ Upload básico
- ❌ **Faltam**: Multiple provider failover (S3 → Azure Blob)
- ❌ **Faltam**: Cache invalidation scenarios
- ❌ **Faltam**: Virus scan integration
- ❌ **Faltam**: Thumbnail generation
- ❌ **Faltam**: CDN integration

#### 7. Services - Moderation
- ✅ Reports básicos
- ❌ **Faltam**: Auto-moderation trigger thresholds
- ❌ **Faltam**: Sanction escalation (warning → ban)
- ❌ **Faltam**: Appeal scenarios
- ❌ **Faltam**: Audit log completeness

#### 8. Services - Search
- ✅ Parcial
- ❌ **Faltam**: Full-text search variations
- ❌ **Faltam**: Fuzzy matching
- ❌ **Faltam**: Filtering combinations
- ❌ **Faltam**: Search performance (large datasets)

#### 9. Validators
- ⚠️ 14 validators existentes
- ❌ **Faltam**: All edge cases (min/max values, special characters)
- ❌ **Faltam**: Unicode/internationalization
- ❌ **Faltam**: SQL injection attempts
- ❌ **Faltam**: XSS payload attempts

---

### CAMADA INFRASTRUCTURE (Objetivo: 80%)

#### 1. Repositories - Generic
- ✅ CRUD básico
- ❌ **Faltam**: Query optimization scenarios
- ❌ **Faltam**: Concurrency conflicts (OptimisticLocking)
- ❌ **Faltam**: Transaction rollback scenarios
- ❌ **Faltam**: Large batch operations (1000+ items)

#### 2. Repositories - Specific
- ✅ Parcial
- ❌ **Faltam**: Complex queries (joins, includes, filters)
- ❌ **Faltam**: Pagination with ordering
- ❌ **Faltam**: Soft delete scenarios
- ❌ **Faltam**: Archival strategies

#### 3. Cache
- ✅ Básico
- ❌ **Faltam**: Cache eviction policies
- ❌ **Faltam**: Redis connection failures
- ❌ **Faltam**: Memory cache pressure
- ❌ **Faltam**: Cache poisoning scenarios

#### 4. Database Migrations
- ⚠️ 40+ migrations
- ❌ **Faltam**: Rollback scenarios (Down methods)
- ❌ **Faltam**: Data migration integrity checks
- ❌ **Faltam**: Large table migration performance

#### 5. Email Service
- ✅ Básico
- ❌ **Faltam**: SMTP connection failures
- ❌ **Faltam**: Template rendering edge cases
- ❌ **Faltam**: Attachment handling
- ❌ **Faltam**: Rate limiting

#### 6. Media Storage
- ✅ Parcial
- ❌ **Faltam**: S3 API failures
- ❌ **Faltam**: Azure Blob failures
- ❌ **Faltam**: Local storage disk full
- ❌ **Faltam**: File permission issues

#### 7. External Integrations
- ❌ **Faltam**: Discord integration scenarios
- ❌ **Faltam**: OAuth provider failures
- ❌ **Faltam**: Rate limit handling
- ❌ **Faltam**: Timeout scenarios

---

### CAMADA API (Objetivo: 85%)

#### 1. Controllers - Feed
- ✅ Endpoints básicos
- ❌ **Faltam**: Invalid pagination parameters
- ❌ **Faltam**: Unauthorized access (private feed)
- ❌ **Faltam**: Malformed request bodies
- ❌ **Faltam**: Rate limiting enforcement

#### 2. Controllers - Marketplace
- ✅ Básico
- ❌ **Faltam**: Cart consistency checks
- ❌ **Faltam**: Concurrent checkout attempts
- ❌ **Faltam**: Inventory race conditions
- ❌ **Faltam**: Payment validation

#### 3. Controllers - Auth
- ✅ Login/Token
- ❌ **Faltam**: 2FA scenarios
- ❌ **Faltam**: Token refresh edge cases
- ❌ **Faltam**: Session invalidation
- ❌ **Faltam**: Brute force protection

#### 4. Controllers - Governance
- ✅ Básico
- ❌ **Faltam**: Vote casting after voting ends
- ❌ **Faltam**: Double vote prevention
- ❌ **Faltam**: Admin override scenarios
- ❌ **Faltam**: Result aggregation

#### 5. Middleware
- ⚠️ Authentication middleware
- ❌ **Faltam**: Invalid token formats
- ❌ **Faltam**: Expired token handling
- ❌ **Faltam**: CORS violation scenarios
- ❌ **Faltam**: Header injection attempts

#### 6. Error Handling
- ✅ Básico
- ❌ **Faltam**: All exception types (DomainException, ValidationException, etc)
- ❌ **Faltam**: Stack trace sanitization
- ❌ **Faltam**: Error message localization

---

### HELPERS/UTILS (Objetivo: 75%)

#### 1. Mappers
- ⚠️ Parcial
- ❌ **Faltam**: Null object handling
- ❌ **Faltam**: Collection mapping edge cases
- ❌ **Faltam**: Circular reference handling
- ❌ **Faltam**: Data type conversions

#### 2. Extensions
- ⚠️ Algumas existem
- ❌ **Faltam**: String extension edge cases
- ❌ **Faltam**: Collection extension performance
- ❌ **Faltam**: DateTime calculations

#### 3. Constants
- ✅ Definidas
- ❌ **Faltam**: Validation de valores de constantes

---

## 📈 Plano de Implementação

### Fase 1: Domain Layer (2 semanas)
**Objetivo**: 95% cobertura

1. **Week 1**: Entidades principais
   - Territory (40 testes)
   - User (50 testes)
   - Membership (30 testes)

2. **Week 2**: Entidades de negócio
   - Post (50 testes)
   - Voting/Governance (40 testes)
   - Store/Item (40 testes)

**Resultado**: ~250 testes novos

---

### Fase 2: Application Layer (3 semanas)
**Objetivo**: 85% cobertura

1. **Week 1**: Services críticos
   - FeedService (100 testes)
   - MarketplaceService (80 testes)

2. **Week 2**: Services de governança
   - VotingService (80 testes)
   - NotificationService (70 testes)

3. **Week 3**: Services auxiliares
   - ChatService (60 testes)
   - MediaService (70 testes)
   - ModerationService (60 testes)

**Resultado**: ~520 testes novos

---

### Fase 3: Infrastructure Layer (2 semanas)
**Objetivo**: 80% cobertura

1. **Week 1**: Repositories
   - Generic repository patterns (100 testes)
   - Specific repositories (80 testes)

2. **Week 2**: Integrations
   - Cache scenarios (60 testes)
   - Email service (40 testes)
   - Media storage (60 testes)

**Resultado**: ~340 testes novos

---

### Fase 4: API Layer (2 semanas)
**Objetivo**: 85% cobertura

1. **Week 1**: Controllers principais
   - FeedController (60 testes)
   - MarketplaceController (50 testes)
   - AuthController (70 testes)

2. **Week 2**: Controllers e middleware
   - GovernanceController (50 testes)
   - Middleware & Filters (80 testes)

**Resultado**: ~310 testes novos

---

### Fase 5: Helpers/Utils (1 semana)
**Objetivo**: 75% cobertura

- Mappers (60 testes)
- Extensions (40 testes)
- Constants validation (20 testes)

**Resultado**: ~120 testes novos

---

## 📊 Estimativas

| Camada | Testes Novos | Semanas | Desenvolvedores | Custo |
|--------|--------------|---------|-----------------|-------|
| Domain | 250 | 2 | 1 | 40h |
| Application | 520 | 3 | 2 | 120h |
| Infrastructure | 340 | 2 | 1 | 80h |
| API | 310 | 2 | 1 | 80h |
| Helpers | 120 | 1 | 1 | 40h |
| **TOTAL** | **1.540** | **10** | **5** | **360h** |

---

## 🎯 Resultado Final

### Antes
```
Testes:    798
Cobertura: ~50%
Lines:     ~40.000
```

### Depois
```
Testes:    ~2.338 (798 + 1.540)
Cobertura: ~90%+ (meta enterprise)
Lines:     ~40.000 (sem mudança)
```

### Benefícios
✅ Detecção de bugs em fase inicial  
✅ Refatoração segura  
✅ Documentação viva (testes = spec)  
✅ Confiança em deploy  
✅ Qualidade enterprise  
✅ Onboarding facilitado para novos devs  

---

## 📋 Próximos Passos

1. **Priorizar Domain Layer** (foundational)
2. **Paralelizar Application + Infrastructure** (depois de Domain estável)
3. **API Layer** (depois de Application pronto)
4. **Helpers** (last, quick wins)
5. **Refine & Optimize** (based on coverage reports)

---

**Tempo estimado para atingir 90%**: 10 semanas com 1 dev full-time  
**Custo aproximado**: 360 horas = ~$10-15k (consultoria senior)

---

**Status**: 📋 PRONTO PARA EXECUÇÃO  
**Próximo passo**: Validar prioridades com team
