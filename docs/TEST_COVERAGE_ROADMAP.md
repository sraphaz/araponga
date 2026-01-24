# 📊 PLANO DE COBERTURA DE TESTES: 50% → 90%

## 🎯 Objetivo
Aumentar a cobertura de testes de **~50% para 90%+** (padrão enterprise)

---

## 📈 Situação Atual vs Meta

```
AGORA                          META
├─ Testes: 798                ├─ Testes: ~2.338 (+1.540)
├─ Cobertura: ~50%            ├─ Cobertura: 90%+
├─ Domain: 70-80%             ├─ Domain: 95%+
├─ Application: 40-50%        ├─ Application: 85%+
├─ Infrastructure: 30-40%     ├─ Infrastructure: 80%+
├─ API: 50-60%                ├─ API: 85%+
└─ Helpers: 20-30%            └─ Helpers: 75%+
```

---

## 🏗️ Cenários Faltantes por Camada

### DOMAIN LAYER (250 testes novos)

**Entity: Territory**
- ✅ Criação com validações
- ❌ Edge cases (caracteres especiais, limites)
- ❌ Validações de coordenadas inválidas
- ❌ Métodos de domínio (distância, contém ponto)

**Entity: User**
- ✅ Autenticação básica
- ❌ CPF vs Foreign Document (mutual exclusivity)
- ❌ 2FA enable/disable workflows
- ❌ Bio truncation (máximo 500 chars)

**Entity: Post**
- ✅ Criação com interações
- ❌ Tags (duplicatas, caracteres especiais)
- ❌ GeoAnchor validation
- ❌ Transições de estado (draft → published → deleted)
- ❌ Concurrency checks

**Entity: Voting/Governance**
- ❌ Vote weight calculation
- ❌ Majority/quorum checks
- ❌ Tie scenarios
- ❌ Admin veto scenarios

**Entity: Marketplace (Store/Item)**
- ❌ Inventory management
- ❌ Pricing validation
- ❌ Platform fees calculation

---

### APPLICATION LAYER (520 testes novos)

**Service: FeedService** (100 testes)
- ❌ Pagination edge cases (page 0, negative limits)
- ❌ Filtering by interests
- ❌ Post deletion cascade
- ❌ Media cleanup

**Service: MarketplaceService** (80 testes)
- ❌ Cart consistency checks
- ❌ Concurrent checkout attempts
- ❌ Inventory race conditions
- ❌ Platform fee calculations

**Service: VotingService** (80 testes)
- ❌ Vote weight application
- ❌ Result calculations
- ❌ Voting period enforcement
- ❌ Admin override

**Service: NotificationService** (70 testes)
- ❌ Bulk notifications
- ❌ Deduplication
- ❌ Delivery retry logic
- ❌ Configuration per type

**Service: ChatService** (60 testes)
- ❌ Channel permissions
- ❌ Message moderation
- ❌ User timeout/mute

**Service: MediaService** (70 testes)
- ❌ Provider failover (S3 → Azure)
- ❌ Cache invalidation
- ❌ Thumbnail generation
- ❌ CDN integration

**Service: ModerationService** (60 testes)
- ❌ Auto-moderation triggers
- ❌ Sanction escalation
- ❌ Appeal scenarios
- ❌ Audit logs

**Validators** (140 testes)
- ❌ All 14 validators × 10 edge cases cada
- ❌ SQL injection attempts
- ❌ XSS payloads
- ❌ Unicode/internationalization

---

### INFRASTRUCTURE LAYER (340 testes novos)

**Repositories** (100 testes)
- ❌ Complex queries (joins, includes)
- ❌ Concurrency conflicts
- ❌ Batch operations (1000+ items)
- ❌ Soft delete scenarios

**Cache** (60 testes)
- ❌ Eviction policies
- ❌ Redis connection failures
- ❌ Memory cache pressure
- ❌ Cache poisoning

**Email Service** (40 testes)
- ❌ SMTP failures
- ❌ Template rendering edge cases
- ❌ Attachment handling
- ❌ Rate limiting

**Media Storage** (60 testes)
- ❌ S3 API failures
- ❌ Azure Blob failures
- ❌ Disk full scenarios
- ❌ Permission issues

**Database Migrations** (80 testes)
- ❌ Rollback scenarios
- ❌ Data integrity
- ❌ Large table performance

---

### API LAYER (310 testes novos)

**Controllers** (180 testes)
- ❌ Invalid pagination
- ❌ Unauthorized access
- ❌ Malformed requests
- ❌ Rate limiting enforcement

**Auth Controller** (70 testes)
- ❌ 2FA scenarios
- ❌ Token refresh edge cases
- ❌ Session invalidation
- ❌ Brute force protection

**Middleware** (60 testes)
- ❌ Invalid token formats
- ❌ CORS violations
- ❌ Header injection attempts

---

### HELPERS & UTILS (120 testes novos)

**Mappers** (60 testes)
- ❌ Null handling
- ❌ Collection edge cases
- ❌ Circular references

**Extensions** (40 testes)
- ❌ String edge cases
- ❌ Collection performance

**Constants** (20 testes)
- ❌ Value validation

---

## 📅 Cronograma de Implementação

| Fase | Semanas | Camada | Testes | Horas | Dev |
|------|---------|--------|--------|-------|-----|
| 1 | 2 | Domain | 250 | 200h | 1 |
| 2 | 3 | Application | 520 | 300h | 2 |
| 3 | 2 | Infrastructure | 340 | 200h | 1 |
| 4 | 2 | API | 310 | 180h | 1 |
| 5 | 1 | Helpers | 120 | 80h | 1 |
| **TOTAL** | **10** | **ALL** | **1.540** | **960h** | **5** |

---

## 💰 Estimativas

### Por Camada
```
Domain:          250 testes = ~200 horas   = $5-6K
Application:     520 testes = ~300 horas   = $9-10K
Infrastructure:  340 testes = ~200 horas   = $6-8K
API:             310 testes = ~180 horas   = $5-7K
Helpers:         120 testes = ~80 horas    = $2-3K
─────────────────────────────────────────────────────
TOTAL:         1.540 testes = ~960 horas   = $30-35K
```

### Por Timing
- **1 dev full-time**: 10 semanas (2.5 meses)
- **2 devs part-time**: 15 semanas (4 meses)
- **1 dev half-time**: 20 semanas (5 meses)

---

## 🎯 Resultado Esperado

### Antes
```
Testes:        798
Cobertura:     ~50%
Linhas:        ~40.000
Confidence:    Medium
```

### Depois
```
Testes:        ~2.338
Cobertura:     90%+
Linhas:        ~40.000
Confidence:    Enterprise-grade
```

---

## ✨ Benefícios

✅ **Detecção precoce de bugs** - bugs encontrados antes de produção  
✅ **Refatoração segura** - mudanças com confiança  
✅ **Documentação viva** - testes documentam comportamento esperado  
✅ **Confiança em deploy** - releases com segurança  
✅ **Padrão enterprise** - código production-ready  
✅ **Onboarding facilitado** - novos devs entendem sistema via testes  

---

## 📚 Documentação Criada

1. **PLANO_COBERTURA_TESTES_90.md**
   - Visão estratégica completa
   - Estimativas por camada
   - Cronograma de implementação

2. **CENARIOS_TESTE_DETALHADOS.md**
   - 48+ classes de teste
   - ~410 métodos de teste detalhados
   - Exemplos de código para cada cenário
   - Específico para cada serviço

---

## 🚀 Próximos Passos

1. **Validar prioridades** com team
2. **Iniciar Fase 1** (Domain Layer) - foundational
3. **Paralelizar Fase 2 + 3** após Domain estável
4. **API Layer** depois de Application pronto
5. **Helpers** como quick wins

---

## 📌 Links Úteis

- 📖 [`docs/PLANO_COBERTURA_TESTES_90.md`](./docs/PLANO_COBERTURA_TESTES_90.md) - Plano estratégico
- 🧪 [`docs/CENARIOS_TESTE_DETALHADOS.md`](./docs/CENARIOS_TESTE_DETALHADOS.md) - Cenários específicos
- 📊 [`docs/22_COHESION_AND_TESTS.md`](./docs/22_COHESION_AND_TESTS.md) - Padrões de teste existentes

---

**Status**: 📋 PRONTO PARA EXECUÇÃO  
**Próximo passo**: Revisar com equipe e priorizar Phase 1 (Domain Layer)
