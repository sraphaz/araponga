# Fase 25: Dashboard de Métricas Comunitárias

**Duração**: 2 semanas (14 dias úteis)  
**Prioridade**: 🟡 ALTA (Transparência e governança)  
**Depende de**: Fase 4 (Observabilidade), Fase 14 (Governança)  
**Estimativa Total**: 112 horas  
**Status**: ⏳ Pendente  
**Nota**: Nova fase criada para Onda 5 (Conformidade e Soberania). Fase 25 agora é Dashboard Métricas, Fase 26 é Hub de Serviços Digitais.

---

## 🎯 Objetivo

Implementar **dashboard de métricas comunitárias** que permite:
- Territórios visualizarem métricas de atividade e engajamento
- Transparência sobre uso e participação da comunidade
- Métricas de governança (votações, participação)
- Métricas de economia local (marketplace, transações)
- Métricas de saúde territorial (observações, ações)
- Comparação temporal e entre territórios
- Exportação de relatórios

**Princípios**:
- ✅ **Transparência**: Métricas públicas ou para moradores
- ✅ **Governança**: Apoia tomada de decisão comunitária
- ✅ **Simplicidade**: Visualizações claras e compreensíveis
- ✅ **Privacidade**: Respeitar preferências de privacidade
- ✅ **Ação**: Métricas que levam a ações concretas

---

## 📋 Contexto e Requisitos

### Estado Atual
- ✅ Fase 4 (Observabilidade) fornece métricas técnicas
- ✅ Sistema de métricas de negócio existe (Prometheus)
- ✅ Fase 14 (Governança) fornece dados de votações
- ✅ Sistema de marketplace existe (transações)
- ✅ Sistema de feed existe (posts, interações)
- ❌ Não existe dashboard de métricas comunitárias
- ❌ Não existe visualização de métricas territoriais
- ❌ Não existe comparação entre territórios

### Requisitos Funcionais

#### 1. Métricas de Atividade
- ✅ Posts criados (diário, semanal, mensal)
- ✅ Eventos criados
- ✅ Membros ativos
- ✅ Interações (curtidas, comentários, compartilhamentos)
- ✅ Taxa de engajamento

#### 2. Métricas de Governança
- ✅ Votações realizadas
- ✅ Taxa de participação em votações
- ✅ Propostas aprovadas/rejeitadas
- ✅ Tempo médio de votação
- ✅ Participação por tipo de votação

#### 3. Métricas de Economia Local
- ✅ Transações no marketplace
- ✅ Volume financeiro (receitas, despesas)
- ✅ Itens vendidos/comprados
- ✅ Taxa de conversão (views → compras)
- ✅ Top vendedores

#### 4. Métricas de Saúde Territorial
- ✅ Observações de saúde criadas
- ✅ Ações territoriais realizadas
- ✅ Participação em mutirões
- ✅ Sensores ativos
- ✅ Indicadores de saúde (quando Fase 24 implementada)

#### 5. Métricas de Engajamento
- ✅ Usuários ativos (DAU, WAU, MAU)
- ✅ Taxa de retenção
- ✅ Novos membros
- ✅ Membros que participam regularmente
- ✅ Top contribuidores

#### 6. Visualizações e Comparações
- ✅ Gráficos temporais (linha, barra)
- ✅ Comparação entre períodos
- ✅ Comparação entre territórios (opcional, agregado)
- ✅ Filtros por período (diário, semanal, mensal, anual)
- ✅ Exportação de dados (CSV, PDF)

---

## 📋 Tarefas Detalhadas

### Semana 1: Modelo de Dados e Agregações

#### 25.1 Modelo de Dados - Métricas Territoriais
**Estimativa**: 24 horas (3 dias)  
**Status**: ❌ Não implementado

**Tarefas**:
- [ ] Criar enum `MetricType`:
  - [ ] `Activity = 1` (atividade: posts, eventos)
  - [ ] `Governance = 2` (governança: votações)
  - [ ] `Economy = 3` (economia: marketplace, transações)
  - [ ] `Health = 4` (saúde territorial)
  - [ ] `Engagement = 5` (engajamento: DAU, WAU, MAU)
- [ ] Criar enum `MetricPeriod`:
  - [ ] `Daily = 1`, `Weekly = 2`, `Monthly = 3`, `Yearly = 4`
- [ ] Criar modelo `TerritoryMetric`:
  - [ ] `Id`, `TerritoryId`, `MetricType`, `MetricKey` (string, ex: "posts.created")
  - [ ] `Period`, `PeriodStartUtc`, `PeriodEndUtc`
  - [ ] `Value` (decimal), `ValueInt` (int?), `ValueString` (string?)
  - [ ] `Metadata` (JSON, dados adicionais)
  - [ ] `CreatedAtUtc`, `UpdatedAtUtc`
- [ ] Criar modelo `TerritoryMetricComparison`:
  - [ ] `TerritoryId`, `MetricType`, `MetricKey`
  - [ ] `CurrentPeriod`, `PreviousPeriod`
  - [ ] `ChangePercent`, `ChangeAbsolute`
  - [ ] `Trend` (UP, DOWN, STABLE)
- [ ] Criar repositórios
- [ ] Criar migrations

**Arquivos a Criar**:
- `backend/Arah.Domain/Metrics/MetricType.cs`
- `backend/Arah.Domain/Metrics/MetricPeriod.cs`
- `backend/Arah.Domain/Metrics/TerritoryMetric.cs`
- `backend/Arah.Domain/Metrics/TerritoryMetricComparison.cs`
- `backend/Arah.Application/Interfaces/ITerritoryMetricRepository.cs`
- `backend/Arah.Infrastructure/Postgres/PostgresTerritoryMetricRepository.cs`

**Critérios de Sucesso**:
- ✅ Modelos criados
- ✅ Repositórios implementados
- ✅ Migrations criadas
- ✅ Testes de repositório passando

---

#### 25.2 Sistema de Agregação de Métricas
**Estimativa**: 32 horas (4 dias)  
**Status**: ❌ Não implementado

**Tarefas**:
- [ ] Criar `TerritoryMetricsAggregationService`:
  - [ ] `AggregateActivityMetricsAsync(Guid territoryId, MetricPeriod period)` → métricas de atividade
  - [ ] `AggregateGovernanceMetricsAsync(Guid territoryId, MetricPeriod period)` → métricas de governança
  - [ ] `AggregateEconomyMetricsAsync(Guid territoryId, MetricPeriod period)` → métricas de economia
  - [ ] `AggregateHealthMetricsAsync(Guid territoryId, MetricPeriod period)` → métricas de saúde
  - [ ] `AggregateEngagementMetricsAsync(Guid territoryId, MetricPeriod period)` → métricas de engajamento
- [ ] Agregações de atividade:
  - [ ] Contar posts criados no período
  - [ ] Contar eventos criados
  - [ ] Contar interações (curtidas, comentários)
  - [ ] Calcular taxa de engajamento
- [ ] Agregações de governança:
  - [ ] Contar votações realizadas
  - [ ] Calcular taxa de participação
  - [ ] Contar propostas aprovadas/rejeitadas
  - [ ] Calcular tempo médio de votação
- [ ] Agregações de economia:
  - [ ] Contar transações
  - [ ] Somar volume financeiro
  - [ ] Contar itens vendidos/comprados
  - [ ] Calcular taxa de conversão
- [ ] Agregações de saúde (quando Fase 24 implementada):
  - [ ] Contar observações de saúde
  - [ ] Contar ações territoriais
  - [ ] Contar participações em mutirões
- [ ] Agregações de engajamento:
  - [ ] Calcular DAU (Daily Active Users)
  - [ ] Calcular WAU (Weekly Active Users)
  - [ ] Calcular MAU (Monthly Active Users)
  - [ ] Calcular taxa de retenção
- [ ] Background job para calcular métricas periodicamente:
  - [ ] Calcular métricas diárias (todos os dias)
  - [ ] Calcular métricas semanais (toda segunda-feira)
  - [ ] Calcular métricas mensais (primeiro dia do mês)
- [ ] Testes unitários

**Arquivos a Criar**:
- `backend/Arah.Application/Services/TerritoryMetricsAggregationService.cs`
- `backend/Arah.Application/Jobs/TerritoryMetricsAggregationJob.cs`
- `backend/Arah.Tests/Application/TerritoryMetricsAggregationServiceTests.cs`

**Critérios de Sucesso**:
- ✅ Agregações funcionando
- ✅ Background job funcionando
- ✅ Métricas sendo calculadas corretamente
- ✅ Testes passando

---

### Semana 2: API, Visualizações e Exportação

#### 25.3 Controller de Métricas
**Estimativa**: 24 horas (3 dias)  
**Status**: ❌ Não implementado

**Tarefas**:
- [ ] Criar `TerritoryMetricsController`:
  - [ ] `GET /api/v1/territories/{territoryId}/metrics/activity` → métricas de atividade
  - [ ] `GET /api/v1/territories/{territoryId}/metrics/governance` → métricas de governança
  - [ ] `GET /api/v1/territories/{territoryId}/metrics/economy` → métricas de economia
  - [ ] `GET /api/v1/territories/{territoryId}/metrics/health` → métricas de saúde
  - [ ] `GET /api/v1/territories/{territoryId}/metrics/engagement` → métricas de engajamento
  - [ ] `GET /api/v1/territories/{territoryId}/metrics/summary` → resumo geral
  - [ ] `GET /api/v1/territories/{territoryId}/metrics/comparison` → comparação entre períodos
- [ ] Parâmetros de query:
  - [ ] `period` (daily, weekly, monthly, yearly)
  - [ ] `startDate`, `endDate` (período customizado)
  - [ ] `compareWith` (previous, same_period_last_year)
- [ ] Validações:
  - [ ] Verificar visibilidade (público ou apenas moradores)
  - [ ] Validar período
  - [ ] Validar permissões
- [ ] Feature flags: `TerritoryMetricsEnabled`, `TerritoryMetricsPublic`
- [ ] Testes de integração

**Arquivos a Criar**:
- `backend/Arah.Api/Controllers/TerritoryMetricsController.cs`
- `backend/Arah.Api/Contracts/Metrics/ActivityMetricsResponse.cs`
- `backend/Arah.Api/Contracts/Metrics/GovernanceMetricsResponse.cs`
- `backend/Arah.Api/Contracts/Metrics/EconomyMetricsResponse.cs`
- `backend/Arah.Api/Contracts/Metrics/HealthMetricsResponse.cs`
- `backend/Arah.Api/Contracts/Metrics/EngagementMetricsResponse.cs`
- `backend/Arah.Api/Contracts/Metrics/MetricsSummaryResponse.cs`
- `backend/Arah.Api/Contracts/Metrics/MetricsComparisonResponse.cs`
- `backend/Arah.Tests/Integration/TerritoryMetricsIntegrationTests.cs`

**Critérios de Sucesso**:
- ✅ API funcionando
- ✅ Validações funcionando
- ✅ Feature flags funcionando
- ✅ Testes passando

---

#### 25.4 Sistema de Comparação e Tendências
**Estimativa**: 16 horas (2 dias)  
**Status**: ❌ Não implementado

**Tarefas**:
- [ ] Criar `TerritoryMetricsComparisonService`:
  - [ ] `ComparePeriodsAsync(Guid territoryId, MetricType type, MetricPeriod period, ...)` → comparar períodos
  - [ ] `GetTrendAsync(Guid territoryId, string metricKey, ...)` → obter tendência
  - [ ] `CalculateChangePercentAsync(decimal current, decimal previous)` → calcular mudança percentual
- [ ] Comparações:
  - [ ] Período atual vs período anterior
  - [ ] Período atual vs mesmo período do ano anterior
  - [ ] Identificar tendências (UP, DOWN, STABLE)
- [ ] Visualizações de tendência:
  - [ ] Últimos 7 dias
  - [ ] Últimos 30 dias
  - [ ] Últimos 12 meses
- [ ] Alertas de mudanças significativas:
  - [ ] Mudança > 20% (positiva ou negativa)
  - [ ] Notificar curadores (opcional)
- [ ] Testes unitários

**Arquivos a Criar**:
- `backend/Arah.Application/Services/TerritoryMetricsComparisonService.cs`
- `backend/Arah.Tests/Application/TerritoryMetricsComparisonServiceTests.cs`

**Critérios de Sucesso**:
- ✅ Comparações funcionando
- ✅ Tendências sendo calculadas
- ✅ Alertas funcionando
- ✅ Testes passando

---

#### 25.5 Exportação de Relatórios
**Estimativa**: 16 horas (2 dias)  
**Status**: ❌ Não implementado

**Tarefas**:
- [ ] Criar `TerritoryMetricsExportService`:
  - [ ] `ExportToCsvAsync(Guid territoryId, MetricType? type, ...)` → exportar CSV
  - [ ] `ExportToPdfAsync(Guid territoryId, MetricType? type, ...)` → exportar PDF
  - [ ] `GenerateReportAsync(Guid territoryId, ...)` → gerar relatório completo
- [ ] Formato CSV:
  - [ ] Dados tabulares
  - [ ] Headers descritivos
  - [ ] Formatação de datas
- [ ] Formato PDF:
  - [ ] Relatório formatado
  - [ ] Gráficos incluídos (se possível)
  - [ ] Logo e branding
- [ ] Endpoints de exportação:
  - [ ] `GET /api/v1/territories/{territoryId}/metrics/export/csv`
  - [ ] `GET /api/v1/territories/{territoryId}/metrics/export/pdf`
- [ ] Validações e permissões
- [ ] Testes de integração

**Arquivos a Criar**:
- `backend/Arah.Application/Services/TerritoryMetricsExportService.cs`
- `backend/Arah.Infrastructure/Export/CsvExporter.cs`
- `backend/Arah.Infrastructure/Export/PdfExporter.cs`
- `backend/Arah.Tests/Integration/TerritoryMetricsExportIntegrationTests.cs`

**Critérios de Sucesso**:
- ✅ Exportação CSV funcionando
- ✅ Exportação PDF funcionando
- ✅ Relatórios formatados corretamente
- ✅ Testes passando

---

## 📊 Resumo da Fase 25

| Tarefa | Estimativa | Status | Prioridade |
|--------|------------|--------|------------|
| Modelo de Dados | 24h | ❌ Pendente | 🔴 Alta |
| Sistema de Agregação | 32h | ❌ Pendente | 🔴 Alta |
| Controller de Métricas | 24h | ❌ Pendente | 🔴 Alta |
| Sistema de Comparação | 16h | ❌ Pendente | 🟡 Média |
| Exportação de Relatórios | 16h | ❌ Pendente | 🟡 Média |
| **Total** | **112h (14 dias)** | | |

---

## ✅ Critérios de Sucesso da Fase 25

### Funcionalidades
- ✅ Dashboard completo de métricas comunitárias funcionando
- ✅ Métricas de atividade, governança, economia, saúde e engajamento
- ✅ Comparações entre períodos funcionando
- ✅ Exportação de relatórios funcionando
- ✅ Visualizações temporais funcionando

### Qualidade
- ✅ Testes com cobertura adequada
- ✅ Documentação completa
- ✅ Performance otimizada (agregações em background)
- ✅ Validações e permissões implementadas
- Considerar **Testcontainers + PostgreSQL** para testes de integração (métricas, agregações) com banco real (estratégia na Fase 43; [TESTCONTAINERS_POSTGRES_IMPACTO](../../TESTCONTAINERS_POSTGRES_IMPACTO.md)).

### Integração
- ✅ Integração com Fase 4 (Observabilidade) funcionando
- ✅ Integração com Fase 14 (Governança) funcionando
- ✅ Integração com Marketplace funcionando
- ✅ Integração com Feed funcionando
- ✅ Preparação para Fase 24 (Saúde Territorial)

---

## 🔗 Dependências

- **Fase 4**: Observabilidade (métricas técnicas, base de dados)
- **Fase 14**: Governança (dados de votações)
- **Marketplace**: Dados de transações
- **Feed**: Dados de posts e interações

---

## 📝 Notas de Implementação

### Métricas de Atividade

**Posts Criados**:
- Contar posts criados no período
- Filtrar por tipo (text, image, video)
- Agrupar por autor (opcional, agregado)

**Eventos Criados**:
- Contar eventos criados
- Contar participações
- Taxa de participação (participações / eventos)

**Interações**:
- Curtidas, comentários, compartilhamentos
- Taxa de engajamento = (interações / posts) * 100

### Métricas de Governança

**Votações**:
- Total de votações realizadas
- Taxa de participação = (votos / membros elegíveis) * 100
- Propostas aprovadas vs rejeitadas
- Tempo médio de votação

### Métricas de Economia

**Transações**:
- Total de transações
- Volume financeiro (receitas, despesas)
- Itens vendidos/comprados
- Taxa de conversão = (compras / views) * 100

### Métricas de Engajamento

**Usuários Ativos**:
- DAU (Daily Active Users): usuários únicos por dia
- WAU (Weekly Active Users): usuários únicos por semana
- MAU (Monthly Active Users): usuários únicos por mês
- Taxa de retenção = (usuários que retornaram / usuários totais) * 100

### Performance

**Otimizações**:
- Agregações calculadas em background (não em tempo real)
- Cache de métricas (5-15 minutos)
- Índices no banco para queries rápidas
- Paginação para grandes volumes de dados

---

**Status**: ⏳ **FASE 25 PENDENTE**  
**Depende de**: Fases 4, 14  
**Crítico para**: Transparência e Governança Comunitária
