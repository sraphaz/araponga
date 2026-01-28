# FASE 49: Conexões e Círculo de Amigos - Resumo Executivo

**Versão**: 1.0  
**Data**: 2026-01-28  
**Status**: 📋 Planejamento  
**Prioridade**: Alta  
**Duração Estimada**: 21 dias

---

## 🎯 Objetivo

Implementar módulo completo de conexões e círculo de amigos, permitindo que moradores e visitantes se conectem mutuamente e priorizem conteúdo de conexões no feed do território.

---

## 📊 Visão Geral

### Problema Atual

- Feed não considera relacionamentos pessoais
- Conteúdo de pessoas próximas pode se perder no feed
- Não há forma de priorizar conexões pessoais
- Usuários não têm controle sobre quem os vê ou adiciona

### Solução Proposta

- **Sistema de Conexões**: Moradores e visitantes podem se adicionar mutuamente
- **Feed Inteligente**: Feed prioriza conteúdo de conexões estabelecidas
- **Privacidade**: Controle total sobre quem pode ver e adicionar
- **Rede Social Local**: Fortalece vínculos dentro do território

---

## 🏗️ Arquitetura

### Módulo Novo

```
Araponga.Modules.Connections/
├── Domain/          # Entidades: UserConnection, ConnectionPrivacySettings
├── Application/     # Services: ConnectionService, ConnectionPrivacyService
├── Infrastructure/  # Repositórios PostgreSQL
└── Api/            # ConnectionsController
```

### Integrações

- **Feed**: Modifica `PostFilterService` para priorizar conexões
- **Notificações**: Usa sistema existente para notificar solicitações
- **Users**: Usa entidade `User` existente
- **Moderation**: Integra com sistema de bloqueio

---

## 📋 Funcionalidades Principais

### 1. Gerenciamento de Conexões

- ✅ Enviar solicitação de conexão
- ✅ Aceitar/rejeitar solicitação
- ✅ Remover conexão
- ✅ Listar conexões (com filtros)

### 2. Privacidade e Configurações

- ✅ Configurar quem pode me adicionar
- ✅ Configurar visibilidade de conexões
- ✅ Integração com sistema de bloqueio

### 3. Integração com Feed

- ✅ Priorizar posts de conexões no feed
- ✅ Parâmetro opcional `prioritizeConnections` (default: true)
- ✅ Cache de conexões para performance

### 4. Busca e Descoberta

- ✅ Buscar usuários por nome
- ✅ Sugestões de conexão (amigos em comum, mesmo território, etc.)

---

## 🔗 Integração com Feed

### Algoritmo de Priorização

1. Aplicar filtros existentes (visibilidade, bloqueios, etc.)
2. Buscar conexões aceitas do usuário (com cache)
3. Separar posts em duas listas:
   - Posts de conexões (Status=Accepted)
   - Posts de não-conexões
4. Ordenar cada lista por data (mais recente primeiro)
5. Combinar: conexões primeiro, depois outros

### Novo Parâmetro

```csharp
GET /api/v1/feed?prioritizeConnections=true  // default: true
```

---

## 📐 Modelo de Domínio

### Entidades Principais

1. **UserConnection**
   - RequesterUserId, TargetUserId
   - Status: Pending, Accepted, Rejected, Removed
   - TerritoryId (opcional, pode ser global)

2. **ConnectionPrivacySettings**
   - WhoCanAddMe: Anyone, ResidentsOnly, ConnectionsOnly, Disabled
   - WhoCanSeeMyConnections: OnlyMe, MyConnections, TerritoryMembers, Everyone

---

## 🌐 APIs Principais

### Endpoints

- `POST /api/v1/connections/request` - Enviar solicitação
- `POST /api/v1/connections/{id}/accept` - Aceitar
- `POST /api/v1/connections/{id}/reject` - Rejeitar
- `DELETE /api/v1/connections/{id}` - Remover
- `GET /api/v1/connections` - Listar conexões
- `GET /api/v1/connections/pending` - Solicitações pendentes
- `GET /api/v1/connections/users/search` - Buscar usuários
- `GET /api/v1/connections/suggestions` - Sugestões
- `GET /api/v1/connections/privacy` - Configurações
- `PUT /api/v1/connections/privacy` - Atualizar configurações

---

## 📅 Cronograma

### Semana 1: Modelo de Domínio e Repositórios (5 dias)
- Domain models e interfaces
- Infrastructure (PostgreSQL)
- Módulo e registro

### Semana 2: Application Layer (5 dias)
- ConnectionService
- ConnectionPrivacyService
- ConnectionSuggestionService
- DTOs e mappers

### Semana 3: API e Integração (5 dias)
- ConnectionsController
- Integração com Feed
- Integração com Notificações
- Documentação

### Semana 4: Testes e Validação (6 dias)
- Testes unitários (>90% cobertura)
- Testes de integração
- Testes E2E
- Validação final

**Total**: 21 dias

---

## 🧪 Testes

### Cobertura

- ✅ Testes unitários: Domain, Services (>90%)
- ✅ Testes de integração: Repositórios, Cache
- ✅ Testes E2E: Fluxos completos
- ✅ Testes de performance: Cache, queries
- ✅ Testes de segurança: Autorização, rate limiting

---

## 🔒 Segurança

### Validações

- ✅ Usuário não pode adicionar a si mesmo
- ✅ Verificar política de privacidade
- ✅ Verificar se conexão já existe
- ✅ Verificar se usuário está bloqueado
- ✅ Rate limiting: 50 solicitações/dia
- ✅ Cooldown: 30 dias após rejeição

### Autorização

- ✅ Apenas destinatário pode aceitar/rejeitar
- ✅ Apenas partes da conexão podem remover
- ✅ Apenas próprio usuário pode ver/editar privacidade

---

## 📊 Métricas de Sucesso

### Funcionais

- ✅ Usuários podem criar conexões
- ✅ Feed prioriza conteúdo de conexões
- ✅ Configurações de privacidade funcionam
- ✅ Notificações são enviadas corretamente

### Técnicas

- ✅ Cobertura de testes >90%
- ✅ Performance: feed com priorização <500ms
- ✅ Cache hit rate >80%
- ✅ Zero breaking changes no feed existente

---

## 📚 Documentação

### Documentos Criados

1. **Funcional**: `docs/funcional/23_CONEXOES_CIRCULO_AMIGOS.md`
   - Visão completa da funcionalidade
   - Fluxos de usuário
   - Regras de negócio

2. **Técnico**: `docs/backlog-api/FASE49_CONEXOES_CIRCULO_AMIGOS.md`
   - Arquitetura técnica
   - Modelo de domínio
   - APIs e endpoints
   - Tarefas detalhadas

3. **Resumo**: Este documento

### Documentos a Atualizar

- [ ] `docs/12_DOMAIN_MODEL.md` - Adicionar entidades de conexões
- [ ] `docs/60_API_LÓGICA_NEGÓCIO.md` - Adicionar endpoints
- [ ] `docs/funcional/03_FEED_COMUNITARIO.md` - Documentar priorização
- [ ] `docs/funcional/11_NOTIFICACOES.md` - Documentar notificações de conexão
- [ ] `docs/40_CHANGELOG.md` - Adicionar nova funcionalidade

---

## 🚀 Próximos Passos

### Imediato

1. Revisar planejamento com equipe
2. Validar requisitos com stakeholders
3. Iniciar implementação (Semana 1)

### Futuro (Fase 2)

- Algoritmo de sugestão mais sofisticado (ML)
- Grupos de conexões (círculos)
- Analytics e métricas avançadas

---

## ✅ Checklist de Implementação

### Domain Layer
- [ ] UserConnection domain model
- [ ] ConnectionStatus enum
- [ ] ConnectionPrivacySettings domain model
- [ ] Enums (ConnectionRequestPolicy, ConnectionVisibility)
- [ ] Repository interfaces

### Infrastructure Layer
- [ ] PostgreSQL entities (Records)
- [ ] Repositories implementation
- [ ] DbContext updates
- [ ] Migration
- [ ] ConnectionsModule (IModule)

### Application Layer
- [ ] ConnectionService
- [ ] ConnectionPrivacyService
- [ ] ConnectionSuggestionService
- [ ] DTOs e mappers

### API Layer
- [ ] ConnectionsController
- [ ] Validação (FluentValidation)
- [ ] Rate limiting
- [ ] Documentação (Swagger)

### Integrações
- [ ] Modificar PostFilterService
- [ ] ConnectionCacheService
- [ ] Integração com Notificações
- [ ] Atualizar FeedController

### Testes
- [ ] Testes unitários (>90%)
- [ ] Testes de integração
- [ ] Testes E2E
- [ ] Testes de performance

### Documentação
- [ ] Atualizar documentação técnica
- [ ] Atualizar documentação funcional
- [ ] Atualizar DevPortal
- [ ] Changelog

---

**Última Atualização**: 2026-01-28  
**Versão**: 1.0  
**Status**: 📋 Planejamento
