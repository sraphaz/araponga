# Decisões Arquiteturais

Este documento registra decisões arquiteturais importantes tomadas durante o desenvolvimento do projeto Araponga.

## ADR-001: Marketplace Implementado Antes do POST-MVP

**Status**: Aceito  
**Data**: 2024  
**Contexto**: A especificação original marcava Marketplace como funcionalidade POST-MVP, mas durante o desenvolvimento identificamos que:

1. **Necessidade de validação**: A funcionalidade de marketplace é crítica para validar o modelo de negócio territorial
2. **Dependências já implementadas**: As dependências necessárias (stores, listings, inquiries) já estavam sendo desenvolvidas para outros casos de uso
3. **Feedback de usuários**: Stakeholders indicaram que marketplace seria essencial para o MVP

**Decisão**: Implementar Marketplace completo no MVP, incluindo:
- Gestão de lojas (stores)
- Listagens de produtos e serviços (listings)
- Sistema de inquiries
- Carrinho e checkout
- Taxas de plataforma

**Consequências**:
- ✅ Funcionalidade completa disponível no MVP
- ✅ Maior complexidade no MVP inicial
- ✅ Necessidade de testes abrangentes
- ⚠️ Possível necessidade de ajustes baseados em feedback real

**Alternativas Consideradas**:
1. Implementar apenas inquiries (rejeitado - muito limitado)
2. Implementar apenas stores sem listings (rejeitado - não valida modelo completo)
3. Adiar para POST-MVP (rejeitado - perde oportunidade de validação)

---

## ADR-002: Sistema de Notificações com Outbox/Inbox

**Status**: Aceito  
**Data**: 2024  
**Contexto**: Necessidade de garantir entrega confiável de notificações in-app mesmo em caso de falhas.

**Decisão**: Implementar padrão Outbox/Inbox para notificações:
- Eventos de domínio geram mensagens no Outbox
- Worker processa Outbox e cria notificações no Inbox
- Inbox é consultado pela API de notificações

**Consequências**:
- ✅ Garantia de entrega (at-least-once)
- ✅ Resiliência a falhas
- ✅ Possibilidade de reprocessamento
- ⚠️ Complexidade adicional
- ⚠️ Necessidade de worker processando Outbox

**Alternativas Consideradas**:
1. Notificações síncronas diretas (rejeitado - sem garantia de entrega)
2. Fila externa (rejeitado - adiciona dependência externa no MVP)

---

## ADR-003: Separação Território vs Camadas Sociais

**Status**: Aceito  
**Data**: 2024  
**Contexto**: Princípio fundamental do projeto - território é geográfico e neutro.

**Decisão**: Território contém apenas dados geográficos. Toda lógica social (memberships, visibilidade, moderação) fica em camadas separadas que referenciam o território.

**Consequências**:
- ✅ Território pode existir sem usuários
- ✅ Lógica social pode evoluir independentemente
- ✅ Evita centralização indevida
- ✅ Mais claro e justo
- ⚠️ Mais complexidade em queries que precisam juntar dados

**Alternativas Consideradas**:
1. Território contém lógica social (rejeitado - viola princípio fundamental)
2. Território é apenas ID (rejeitado - perde dados geográficos essenciais)

---

## ADR-004: PresencePolicy para Validação de Presença Física

**Status**: Aceito  
**Data**: 2024  
**Contexto**: MVP exige presença física para vínculo, mas diferentes políticas podem ser necessárias.

**Decisão**: Implementar `PresencePolicy` configurável:
- `None`: Nenhum vínculo exige geo
- `ResidentOnly`: Apenas RESIDENT exige geo (padrão)
- `VisitorAndResident`: Ambos exigem geo

**Consequências**:
- ✅ Flexibilidade para diferentes contextos
- ✅ Configurável via appsettings
- ✅ Validação consistente em toda aplicação
- ⚠️ Necessidade de documentar política escolhida

**Alternativas Consideradas**:
1. Sempre exigir geo para todos (rejeitado - muito restritivo)
2. Nunca exigir geo (rejeitado - viola princípio do MVP)

---

## ADR-005: GeoAnchors Derivados de Mídias

**Status**: Aceito  
**Data**: 2024  
**Contexto**: Posts podem ter GeoAnchors, mas client não deve definir manualmente.

**Decisão**: GeoAnchors são derivados automaticamente de metadados de mídias quando disponíveis. Client não envia GeoAnchors manualmente.

**Consequências**:
- ✅ Dados geográficos mais confiáveis (vêm de EXIF)
- ✅ Simplifica API do client
- ✅ Posts podem existir sem GeoAnchors (não aparecem no mapa)
- ⚠️ Dependência de processamento de mídia (POST-MVP)

**Alternativas Consideradas**:
1. Client define GeoAnchors manualmente (rejeitado - menos confiável)
2. GeoAnchors obrigatórios (rejeitado - muito restritivo)

---

## ADR-006: Clean Architecture com InMemory e Postgres

**Status**: Aceito  
**Data**: 2024  
**Contexto**: Necessidade de suportar desenvolvimento rápido e produção robusta.

**Decisão**: Implementar repositórios InMemory para desenvolvimento/testes e Postgres para produção. Switch via configuração `Persistence:Provider`.

**Consequências**:
- ✅ Desenvolvimento rápido sem dependências externas
- ✅ Testes rápidos e isolados
- ✅ Produção robusta com Postgres
- ✅ Mesma interface para ambos
- ⚠️ Necessidade de manter duas implementações sincronizadas

**Alternativas Consideradas**:
1. Apenas Postgres (rejeitado - dificulta desenvolvimento)
2. Apenas InMemory (rejeitado - não adequado para produção)

---

## ADR-007: Moderação Automática por Threshold

**Status**: Aceito  
**Data**: 2024  
**Contexto**: Necessidade de proteger comunidade de conteúdo inadequado rapidamente.

**Decisão**: Implementar moderação automática quando threshold de reports únicos é atingido (padrão: 3 reports). Ações automáticas incluem ocultar conteúdo e aplicar sanções.

**Consequências**:
- ✅ Proteção rápida da comunidade
- ✅ Reduz carga de moderação manual
- ✅ Threshold configurável
- ⚠️ Possibilidade de falsos positivos
- ⚠️ Necessidade de auditoria

**Alternativas Consideradas**:
1. Apenas moderação manual (rejeitado - muito lento)
2. Threshold muito baixo (rejeitado - muitos falsos positivos)
3. Threshold muito alto (rejeitado - pouco efetivo)

---

## ADR-008: Feature Flags por Território

**Status**: Aceito  
**Data**: 2024  
**Contexto**: Diferentes territórios podem precisar de funcionalidades diferentes.

**Decisão**: Implementar feature flags por território, gerenciadas por curadores.

**Consequências**:
- ✅ Flexibilidade por território
- ✅ Rollout gradual de funcionalidades
- ✅ Possibilidade de desabilitar funcionalidades problemáticas
- ⚠️ Necessidade de gerenciar flags

**Alternativas Consideradas**:
1. Feature flags globais (rejeitado - menos flexível)
2. Sem feature flags (rejeitado - muito rígido)

---

## ADR-009: Work Queue Genérica (WorkItem) para Revisões Humanas

**Status**: Aceito  
**Data**: 2026  
**Contexto**: O sistema precisa de filas de revisão para diferentes domínios (verificação, curadoria, moderação) sem criar “N filas” específicas e inconsistentes.

**Decisão**: Introduzir um modelo genérico `WorkItem` (Work Queue) com:
- `Type`, `Status`, `Outcome`
- `TerritoryId` opcional (itens globais vs territoriais)
- `RequiredSystemPermission` e/ou `RequiredCapability`
- `SubjectType/SubjectId` + `PayloadJson` para contexto mínimo

**Consequências**:
- ✅ Filas padronizadas e extensíveis (novos tipos sem refatorar infra)
- ✅ Governança clara (SystemAdmin vs Curator/Moderator)
- ✅ Auditoria consistente de criação/conclusão
- ⚠️ `PayloadJson` exige disciplina para evitar acoplamento excessivo

**Alternativas Consideradas**:
1. Filas específicas por domínio (rejeitado - duplicação e divergência)
2. “Somente notificações” sem fila (rejeitado - falta rastreabilidade e SLA humano)

---

## ADR-010: Download de Arquivos por Proxy (inicial) vs URL Pré-assinada

**Status**: Aceito  
**Data**: 2026  
**Contexto**: Precisamos suportar evidências/mídias em storage externo (S3/MinIO), controlando acesso e auditando downloads.

**Decisão**: Adotar **download por proxy** inicialmente:
- Cliente chama API
- API autentica/autoriza, audita e faz streaming do storage

**Consequências**:
- ✅ Controle total de acesso e auditoria
- ✅ Simplifica (não exige política de expiração e assinatura no client)
- ⚠️ Maior carga na API (bandwidth/streaming)

**Alternativas Consideradas**:
1. URLs pré-assinadas (rejeitado nesta fase - mais variáveis operacionais no client)
2. URLs públicas (rejeitado - risco de vazamento)

---

## ADR-011: BFF como Aplicação Separada (Fase 2)

**Status**: Aceito  
**Data**: 2026  
**Contexto**: A documentação do projeto prevê um Backend for Frontend (BFF) para reduzir chamadas de rede e expor jornadas formatadas para UI. Após a Fase 1 (BFF como módulo interno na API), evoluiu-se para a Fase 2: o BFF **não é um módulo interno** — é **outra aplicação**.

**Decisão**: O BFF é uma **aplicação separada** (`Araponga.Api.Bff`):
- Projeto próprio: `backend/Araponga.Api.Bff`, deployável de forma independente
- Expõe as mesmas rotas de jornada sob `/api/v2/journeys` (onboarding, feed, events, marketplace), encaminhando as requisições para a API principal (`Araponga.Api`)
- Configuração `Bff:ApiBaseUrl` na aplicação BFF aponta para a URL da API
- O frontend consome o BFF; o BFF faz proxy (e futuramente pode orquestrar chamadas à API v1 diretamente)
- A API principal continua expondo `/api/v2/journeys` para atender as requisições encaminhadas pelo BFF (ou consumo direto em cenários legados)
- **Cache quando necessário**: respostas GET 2xx podem ser cacheadas em memória no BFF (`Bff:EnableCache`, `Bff:CacheTtlSeconds`, `Bff:CacheTtlByPath`) para reduzir chamadas à API; chave inclui path, query e autorização (respostas por usuário). Header de resposta `X-Bff-Cache: HIT` ou `MISS` indica origem.

**Consequências**:
- ✅ BFF e API são aplicações distintas (deploy, escala e evolução independentes)
- ✅ Frontend fala apenas com o BFF; a API pode ficar interna à rede
- ✅ Mesma autenticação JWT: o BFF repassa o token à API
- ✅ Cache no BFF reduz carga na API em leituras repetidas (GET)
- ⚠️ Latência adicional de uma hop HTTP entre BFF e API (aceitável na maioria dos cenários)

**Referências**:
- [Avaliação BFF](docs/AVALIACAO_BFF_BACKEND_FOR_FRONTEND.md)
- [Contrato OpenAPI BFF](docs/BFF_API_CONTRACT.yaml)

---

## Referências

- [Product Vision](./01_PRODUCT_VISION.md)
- [User Stories](./04_USER_STORIES.md)
- [Architecture C4](./design/Archtecture/C4_Components.md)
