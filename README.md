# Araponga

**Araponga** é uma plataforma digital comunitária orientada ao território. Tecnologia que serve à vida local, à convivência e à autonomia das comunidades.

Não é uma rede social genérica. É uma **extensão digital do território vivo**.

---

## Propósito

Plataformas digitais capturam atenção, desorganizam comunidades e desconectam pessoas do lugar onde vivem.

O Araponga é um contraponto consciente a esse modelo.

**Território como referência. Comunidade como prioridade. Tecnologia como ferramenta — não como fim.**

---

## O que é o Araponga

Plataforma que permite:

- **Descobrir e reconhecer territórios reais**
- **Organizar comunidades locais**
- **Compartilhar informações relevantes ao lugar**
- **Visualizar eventos, avisos e iniciativas no mapa**
  - Entidades do território podem ser estabelecimentos, órgãos do governo, espaços públicos ou espaços naturais.
- **Diferenciar moradores e visitantes com respeito**
- **Fortalecer redes locais de cuidado, troca e presença**
- **Marketplace territorial** para trocas locais
- **Eventos comunitários** organizados por território
- **Alertas de saúde pública** e comunicação emergencial
- **Chat territorial (canais e grupos)** com governança (curadoria/moderação) e feature flags por território

Sem algoritmos de manipulação, feed global infinito ou extração de dados para publicidade.

---

## Princípios Fundamentais

### 1. Território é geográfico (e neutro)

No Araponga, `territory` representa **apenas um lugar físico real**:

- nome
- localização
- fronteira geográfica

Ele **não contém lógica social**.

> O território existe antes do app  
> e continua existindo mesmo sem usuários.

Essa decisão arquitetural evita:
- centralização indevida
- conflitos de poder
- confusão entre espaço físico e relações sociais

---

### 2. Vida social acontece em camadas separadas

Relações humanas como:
- moradores
- visitantes
- visibilidade de conteúdo
- regras de convivência
- moderação

**não pertencem ao território**.

Elas pertencem a **camadas sociais que referenciam o território**.

Isso torna o sistema:
- mais claro
- mais justo
- mais adaptável ao tempo

---

### 3. Tecnologia a serviço do território

O Araponga **não é**:
- um marketplace agressivo
- uma rede de engajamento infinito
- um produto de vigilância

Ele é uma **infraestrutura digital comunitária**, pensada para:

- autonomia local
- cuidado coletivo
- continuidade da vida no território
- fortalecimento do vínculo entre pessoas e lugar

---

## Arquitetura

O backend segue princípios de **Clean Architecture**, com separação clara de responsabilidades:

backend/
├── Araponga.Api # API HTTP (controllers, endpoints, middlewares)
├── Araponga.Application # Casos de uso / regras de aplicação
├── Araponga.Domain # Modelo de domínio (territory, regras centrais)
├── Araponga.Infrastructure # Persistência, integrações, adapters
├── Araponga.Shared # Tipos e utilitários compartilhados
└── Araponga.Tests # Testes automatizados

### Conceitos centrais do domínio

- **Territory**  
  Lugar físico real, neutro e persistente.

- **Membership**  
  Relação entre uma pessoa e um território (morador, visitante, etc.).

- **Feed / Map**  
  Informação contextual, sempre relacionada a um território específico.

- **Marketplace**  
  Sistema de trocas locais integrado ao território (stores, listings, cart, checkout).

- **Events**  
  Eventos comunitários organizados por território.

- **Alerts**  
  Alertas de saúde pública e comunicação emergencial.

- **Assets**  
  Recursos compartilhados do território (documentos, mídias, etc.).

---

## Documentação

**[Índice Completo da Documentação](./docs/00_INDEX.md)** — Navegação estruturada

### Fases e Roadmap
- [**Backlog de Fases (1-29)** →  `docs/backlog-api/`](./docs/backlog-api/) — **29 fases planejadas, 14.5 implementadas**
- [Roadmap Completo](./docs/02_ROADMAP.md) — Visão de longo prazo
- [Estrutura da Documentação](./docs/STRUCTURE.md) — Onde encontrar cada documento

### Visão e Produto
- [Visão do Produto](./docs/01_PRODUCT_VISION.md) — Princípios e valores
- [Glossário](./docs/05_GLOSSARY.md) — Terminologia do projeto

### Arquitetura
- [Decisões Arquiteturais (ADRs)](./docs/10_ARCHITECTURE_DECISIONS.md)
- [Arquitetura de Services](./docs/11_ARCHITECTURE_SERVICES.md)
- [Modelo de Domínio](./docs/12_DOMAIN_MODEL.md)

### Desenvolvimento
- [Guia de Desenvolvimento](./docs/DEVELOPMENT.md) — Setup local e padrões
- [Configuração e Setup](./docs/SETUP.md) — Instalar e rodar o projeto
- [API Documentation](./docs/API.md) — Endpoints principais
- [Plano de Implementação](./docs/20_IMPLEMENTATION_PLAN.md)
- [Análise de Coesão e Testes](./docs/22_COHESION_AND_TESTS.md)

### Operações e Segurança
- [Documentação de Segurança](./docs/SECURITY_CONFIGURATION.md) — Configuração segura
- [Avaliação para Produção](./docs/50_PRODUCAO_AVALIACAO_COMPLETA.md) — Checklist
- [Governance System](./docs/GOVERNANCE_SYSTEM.md) — Votação e moderação
- [Community Moderation](./docs/COMMUNITY_MODERATION.md) — Políticas de moderação
- [Voting System](./docs/VOTING_SYSTEM.md) — Sistema de votação

### Frontend e Wiki
- [Wiki Frontend](./frontend/wiki) — Documentação interativa com Next.js
- [Developer Portal](./frontend/devportal) — Portal público (GitHub Pages)

---

## 🚀 Estado do Projeto - 29 Fases de Desenvolvimento

O Araponga está em **desenvolvimento ativo** com **14+ fases implementadas** e validadas. O projeto segue um modelo de desenvolvimento disciplinado com foco em arquitetura sólida.

### 📊 Progresso do Desenvolvimento

| Fases | Status | Descrição |
|-------|--------|-----------|
| **Fases 1-14** | ✅ **IMPLEMENTADAS** | Core features, feed, marketplace, governança, segurança |
| **Fase 14.5** | ✅ **IMPLEMENTADA** | Verificação de aderência, melhorias de performance, refinamentos |
| **Fases 15-29** | 📋 **PLANEJADAS** | Futuras evoluções (veja roadmap completo) |

**Total de fases planejadas**: 29  
**Fases implementadas**: 14.5  
**Progresso**: ~50% do roadmap

---

### ✅ Funcionalidades Implementadas (Fases 1-14.5)

#### Fase 1: Infraestrutura e Autenticação
- ✅ Backend com Clean Architecture
- ✅ Autenticação social (Google, Apple, Facebook) com JWT
- ✅ Gestão de usuários com perfil completo
- ✅ Segurança: validação de JWT, secret management, HTTPS/HSTS obrigatório

#### Fase 2: Territórios e Descoberta
- ✅ Territórios como entidades geográficas neutras
- ✅ Descoberta e seleção de territórios
- ✅ Caracterização do território (tags, informações geográficas)
- ✅ Memberships: morador vs visitante com regras de visibilidade

#### Fase 3: Vínculos e Visibilidade
- ✅ Sistema de vínculos territoriais com papéis diferenciados
- ✅ Regras de visibilidade baseadas em membership
- ✅ Perfis públicos de usuários com validação de acesso

#### Fase 4: Feed Territorial e Posts
- ✅ Feed cronológico por território (sem algoritmos manipulativos)
- ✅ Criação de posts com georreferenciamento (GeoAnchors)
- ✅ Posts com múltiplas imagens (até 10 por post)
- ✅ Interações: likes, comentários, shares
- ✅ Paginação eficiente com cursor-based pagination

#### Fase 5: Chat Territorial
- ✅ Chat com canais (público/moradores) e grupos
- ✅ Aprovação de entrada em canais por curadoria
- ✅ Suporte a envio de imagens em mensagens (1 imagem/msg, máx. 5MB)
- ✅ Notificações de chat em tempo real

#### Fase 6-9: Mapa, Eventos, Alertas
- ✅ Mapa territorial com MapEntity (estabelecimentos, órgãos, espaços)
- ✅ Pins integrados com GeoAnchors de posts
- ✅ Eventos comunitários com participações e RSVP
- ✅ Alertas de saúde pública e comunicação emergencial

#### Fase 10: Mídia Avançada e Armazenamento
- ✅ Sistema completo de mídia com 3 provedores (Local, S3, Azure Blob)
- ✅ Upload de imagens, vídeos e documentos com validação
- ✅ Cache de URLs com Redis/Memory Cache
- ✅ Georreferenciamento de assets (Territory Assets)
- ✅ Configuração por território de provedores de mídia

#### Fase 11: Marketplace Territorial
- ✅ Stores (lojas/comércios) por território
- ✅ Items (produtos/serviços) com busca full-text
- ✅ Cart e checkout com sistema de inquiries
- ✅ Platform Fees configuráveis por território
- ✅ Ratings de stores e items
- ✅ Seller Balance e transações

#### Fase 12: Moderação e Segurança
- ✅ Reports de posts e usuários com categorização
- ✅ Bloqueios de usuários
- ✅ Sanções territoriais e globais
- ✅ Moderação automática por threshold
- ✅ Logs de auditoria completos

#### Fase 13: Notificações Avançadas
- ✅ Notificações in-app com outbox pattern
- ✅ Inbox persistido com estado
- ✅ Sistema confiável de entrega
- ✅ Configuração por usuário de tipos de notificação

#### Fase 14: Governança Comunitária
- ✅ Interesses do usuário (personalização de feed)
- ✅ Sistema de votação comunitária (5 tipos)
- ✅ Moderação dinâmica definida pela comunidade
- ✅ Caracterização do território via votação
- ✅ Regras de convivência configuráveis
- ✅ Histórico de participação no perfil do usuário

#### Fase 14.5: Refinamentos e Validação
- ✅ Métricas de connection pooling em tempo real (ObservableGauge)
- ✅ Tags explícitas em posts (filtro avançado)
- ✅ Configuração avançada de notificações
- ✅ Verificação de aderência entre código e documentação
- ✅ Reorganização e limpeza da documentação

---

### 🔒 Segurança e Confiabilidade (Cross-Phase)

- ✅ JWT com secret de 32+ caracteres via variáveis de ambiente
- ✅ HTTPS obrigatório com HSTS em produção
- ✅ Rate limiting com proteção contra DDoS/abuso:
  - Auth: 5 req/min
  - Feed: 100 req/min
  - Escrita: 30 req/min
- ✅ Security headers (X-Frame-Options, CSP, X-Content-Type-Options, etc.)
- ✅ 14 validadores FluentValidation
- ✅ Testes de segurança: SQL injection, XSS, CSRF, path traversal, etc.
- ✅ CORS configurado com validação em produção
- ✅ Connection pooling com retry policies
- ✅ Índices de banco para performance crítica
- ✅ Cache invalidation automático em 9 services
- ✅ Logging estruturado com Serilog
- ✅ Health checks (liveness + readiness)

---

### 🧪 Testes (Fases 1-14.5)

- ✅ **798 testes** passando (100%)
- ✅ Testes de unidade, integração e E2E
- ✅ 14 testes de segurança
- ✅ 7 testes de performance com SLAs
- ✅ Cobertura ~50% (objetivo >90%)
- ✅ CI configurado com builds reprodutíveis

---

### 📋 Fases 15-29: Próximas Evoluções (Planejadas)

| Fase | Foco | Estimativa |
|------|------|-----------|
| **15-18** | Admin/Observabilidade avançada, friends/stories, otimizações | Q2-Q3 2025 |
| **19-22** | Economias locais, moedas, trocas avançadas | Q3-Q4 2025 |
| **23-26** | Governança distribuída, DAOs, participação ampliada | Q4 2025-Q1 2026 |
| **27-29** | Integrações regenerativas, dados soberados, escalabilidade | Q1-Q2 2026 |

**Roadmap completo**: [`docs/02_ROADMAP.md`](./docs/02_ROADMAP.md)

---

### 📈 Métricas do Projeto

| Métrica | Valor |
|---------|-------|
| **Linhas de código** | ~40.000+ |
| **Endpoints de API** | 150+ |
| **Testes automatizados** | 798 (100% passando) |
| **Cobertura de testes** | ~50% (meta: >90%) |
| **Camadas de arquitetura** | 5 (Domain, Application, Infrastructure, API, Tests) |
| **Serviços de domínio** | 25+ |
| **Repositórios** | 20+ |
| **Migrations do BD** | 40+ |
| **Security validators** | 14 |
| **Fases planejadas** | 29 |
| **Fases implementadas** | 14.5 |
| **Progresso do roadmap** | ~50% |

---

### 🎯 Próximos Passos Imediatos

1. **Frontend**: Começar desenvolvimento da interface (Vue/React)
2. **Performance**: Aumentar cobertura de testes para >90%
3. **Documentação**: Manter wiki sincronizado com código
4. **Admin Dashboard**: Ferramentas de observabilidade para moderadores
5. **Escalabilidade**: Preparar para múltiplos territórios/usuários em produção

O projeto está em **evolução disciplinada**, com foco em solidez e escalabilidade antes de crescimento agressivo.

---

## Como Rodar Localmente

> A documentação canônica de operação está em [`docs/README.md`](docs/README.md).

### Pré-requisitos

- .NET 8 SDK
- Docker (opcional, para Postgres)
- Git

### InMemory (padrão, para desenvolvimento)

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project backend/Araponga.Api
```

A API estará disponível em `http://localhost:5000` (ou porta configurada).

### Postgres (docker compose, recomendado)

```bash
docker compose up --build
```

Isso sobe a API e o PostgreSQL em containers Docker.

### Migrations (Postgres)

```bash
dotnet ef database update \
  --project backend/Araponga.Infrastructure \
  --startup-project backend/Araponga.Api
```

### Configuração (Produção)

Para rodar em produção, configure as variáveis de ambiente:

**Obrigatório**:
```bash
# JWT Secret - Mínimo 32 caracteres
# Gere com: openssl rand -base64 32
JWT__SIGNINGKEY=<secret-forte-de-pelo-menos-32-caracteres>

# CORS Origins - Não pode ser wildcard (*) em produção
Cors__AllowedOrigins__0=https://app.araponga.com
Cors__AllowedOrigins__1=https://www.araponga.com
```

**Opcional** (se usar Postgres):
```bash
ConnectionStrings__Postgres=<connection-string>
Persistence__Provider=Postgres
Persistence__ApplyMigrations=true
```

**Opcional** (ajustar rate limiting):
```json
{
  "RateLimiting": {
    "PermitLimit": 60,
    "WindowSeconds": 60,
    "QueueLimit": 0
  }
}
```

**Documentação Completa**:
- [Configuração de Segurança](./docs/SECURITY_CONFIGURATION.md) - Guia completo de configuração
- [Avaliação para Produção](./docs/50_PRODUCAO_AVALIACAO_COMPLETA.md) - Checklist completo
- [Política de Segurança](./SECURITY.md) - Medidas de segurança implementadas

### Portal de autosserviço

A página inicial da API (`/`) serve um portal estático com explicação do produto,
domínios, fluxos e quickstart. Em desenvolvimento, acesse também:

- `/swagger` (documentação interativa da API)
- `/health` (health check de liveness)
- `/health/ready` (health check de readiness, verifica dependências)

Quando a API está rodando localmente em ambiente de desenvolvimento, o portal
exibe um preview do Swagger para navegação e testes rápidos.

Para publicação como site estático, o portal também está disponível em `docs/` e
pode ser hospedado via GitHub Pages (basta apontar a origem para a pasta `docs`).
A versão do GitHub Pages inclui links diretos para documentação, user stories e changelog.

---

## 🤝 Contribuindo

Consulte o guia em [`docs/41_CONTRIBUTING.md`](./docs/41_CONTRIBUTING.md).

O Araponga é aberto à colaboração, especialmente de pessoas interessadas em:

- tecnologia com impacto social
- comunidades locais
- território, cultura e soberania
- arquitetura de software consciente
- regeneração e autonomia

Formas de contribuir:

- código
- testes
- documentação
- ideias
- feedback conceitual

Antes de abrir PRs grandes, abra uma issue para alinharmos a direção.

---

## Visão de Futuro

Direções possíveis (não promessas fechadas):

- Economias e moedas locais
- Trocas de serviços comunitários
- Governança distribuída
- Integração com iniciativas regenerativas
- Tecnologia como guardiã do território, não como exploradora

O Araponga não quer crescer rápido. Quer criar raízes profundas.

---

## Uma Nota

Este projeto nasce de uma escuta atenta da vida, do território, das comunidades e dos limites do modelo digital atual.

Se você chegou até aqui e sentiu que isso faz sentido, você já faz parte da conversa.

---

## Developer Portal (GitHub Pages)

O conteúdo estático do Developer Portal vive em `frontend/devportal` e é publicado automaticamente via GitHub Actions na branch `gh-pages` quando há push em `main` ou `master`.

---

## Licença

Este projeto é distribuído sob uma **licença aberta orientada à comunidade e ao território**.

- Versão oficial (EN): `LICENSE`
- Versão em português (PT-BR): `LICENSE.pt-BR`

Araponga canta para avisar, proteger e comunicar. Que esta plataforma faça o mesmo.
