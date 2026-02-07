# Backend: camadas, projetos e nomenclatura

Este documento descreve **o que cada projeto e pasta contém**, a diferença entre eles e o significado de nomes como "Core" e "Shared". A estrutura física atual é **projetos na raiz de `backend/`** (sem pastas `Core/` ou `Modules/`).

---

## 1. Arah.Api

**Papel:** Camada de entrada HTTP — controllers, contratos (DTOs), validação, autenticação, middleware, health checks.

**Conteúdo principal:**
- **Controllers/** — endpoints REST; subpasta **Platform/** agrupa endpoints **transversais** (não específicos de um módulo):
  - Auth, Territories, Memberships, JoinRequests, Verification, Privacy/Terms, Analytics, Devices, Features, Votings, DataExport, Admin (seed, system config), etc.
- **Contracts/** — DTOs de request/response da API
- **Security/** — JWT, current user, auth handlers
- **Validators/** — FluentValidation
- **Middleware/** — correlation ID, logging, security headers
- **HealthChecks/** — cache, database, event bus, storage
- **Services/** — serviços de apoio à API (ex.: chamadas externas)
- **Extensions/** — registro de serviços (DI)

**Por que existe a pasta "Platform" dentro de Controllers?**  
"Platform" indica funcionalidade **transversal** (auth, territórios, membros, verificação, termos, etc.), em oposição a controllers por módulo (Feed, Map, Marketplace, Moderation, etc.). Evita confusão com o conceito de "Core" (conjunto de projetos do núcleo).

---

## 2. Arah.Application

**Papel:** Casos de uso, orquestração, interfaces de repositórios e serviços transversais. Não contém entidades de domínio (ficam em Domain ou nos módulos).

**Conteúdo principal:**
- **Interfaces/** — contratos de repositórios e serviços. A subpasta **Platform/** reúne interfaces **compartilhadas** (IUserRepository, ITerritoryRepository, IUnitOfWork, IFileStorage, ITokenService, IAuditLogger, etc.) usadas em toda a aplicação.
- **Services/** — implementação dos serviços. A subpasta **Platform/** reúne serviços **transversais** (AuthService, TerritoryService, MembershipService, JoinRequestService, EmailTemplateService, FeatureFlagService, etc.).
- **Models/** — DTOs e resultados de aplicação (CartDetails, CheckoutResult, StoreSearchResult, etc.)
- **Events/** — eventos de domínio e handlers (ConnectionAccepted, ReportCreated, etc.)
- **Exceptions/** — exceções de aplicação
- **Common/** — tipos/helpers comuns
- **ModuleRegistry.cs** — registro dos módulos (IModule)

**Por que existe a pasta "Platform" dentro de Application?**  
Interfaces e serviços em `Interfaces/Platform/` e `Services/Platform/` são os que **não pertencem a um módulo específico** (Feed, Map, Marketplace, etc.) e são usados em vários lugares. O nome "Platform" deixa claro que é funcionalidade transversal, não um projeto "Core" separado.

**Não existe** um projeto "Core.Application" ou "Domain.Core". O projeto único é **Arah.Application**.

---

## 3. Arah.Domain

**Papel:** Entidades e value objects do **núcleo compartilhado** do domínio. Sem infraestrutura, sem casos de uso.

**Conteúdo principal (por pasta):**
- **Users/** — User, UserDevice, UserPreferences, UserInterest, roles, verification status, etc.
- **Territories/** — Territory, TerritoryStatus, TerritoryCharacterization, SensitivityLevel
- **Membership/** — TerritoryMembership, MembershipRole, MembershipCapability, ResidencyVerification, etc.
- **Governance/** — Voting, Vote, TerritoryModerationRule, RuleType
- **Policies/** — PrivacyPolicy, TermsOfService, acceptances
- **Social/JoinRequests/** — TerritoryJoinRequest, status, recipients
- **Financial/** — FinancialTransaction, PlatformRevenueTransaction, PlatformExpenseTransaction, ReconciliationRecord, etc.
- **Geo/** — GeoCoordinate
- **Media/** — MediaAsset, MediaAttachment, MediaType, MediaStorageConfig, TerritoryMediaConfig
- **Email/** — EmailQueueItem
- **Configuration/** — SystemConfig, SystemConfigCategory

**"Domain Core"?**  
Na documentação (ADRs), "Core.Domain" designa **este projeto** (Arah.Domain): o domínio **compartilhado** que os módulos podem referenciar (UserId, TerritoryId, GeoCoordinate, etc.). Não existe um projeto separado chamado "Domain.Core" ou "Arah.Domain.Core".

---

## 4. Arah.Infrastructure

**Papel:** Implementações de persistência, integrações e infraestrutura técnica usada pela aplicação e pelos módulos (quando não está no próprio módulo).

**Conteúdo principal:**
- **Postgres/** — DbContexts e repositórios PostgreSQL para módulos e cross-cutting (ex.: ArapongaDbContext, mapeamentos, migrations)
- **InMemory/** — implementações em memória para testes e dev (repositórios que usam InMemoryDataStore, etc.)
- **Email/** — envio de e-mail
- **FileStorage/** — armazenamento de arquivos (S3, Azure Blob, etc.)
- **Caching/** — cache distribuído/memória
- **Eventing/** — event bus / outbox
- **Notifications/** — push (ex.: Firebase), providers
- **Media/** — processamento de mídia
- **Payments/** — gateways de pagamento
- **Security/** — JwtTokenService, secrets
- **Background/** — workers (ex.: outbox, payouts)

Ou seja: **infraestrutura “principal”** — Postgres/InMemory gerais, e-mail, storage, cache, eventos, workers, etc.

---

## 5. Arah.Infrastructure.Shared

**Papel:** Persistência **compartilhada** das entidades do **núcleo** (Domain): User, Territory, Membership, Terms, Privacy, SystemConfig, JoinRequests, Voting, etc.

**Conteúdo principal:**
- **Postgres/** — SharedDbContext, entidades (UserRecord, TerritoryRecord, TerritoryMembershipRecord, etc.), repositórios Postgres (PostgresUserRepository, PostgresTerritoryRepository, PostgresTerritoryMembershipRepository, etc.)
- **InMemory/** — mesmos repositórios em memória (InMemoryUserRepository, InMemoryTerritoryRepository, etc.)
- **DbContextUnitOfWorkParticipant.cs** — participação no Unit of Work
- **ServiceCollectionExtensions.cs** — registro dos serviços/repositórios compartilhados

**Diferença em relação a Arah.Infrastructure:**  
- **Infrastructure** — infraestrutura geral (vários DbContexts, módulos, e-mail, storage, cache, workers).  
- **Infrastructure.Shared** — um **DbContext e repositórios** dedicados às entidades **compartilhadas** (User, Territory, Membership, Policies, etc.) usadas em toda a aplicação. Evita que cada módulo implemente seu próprio UserRepository; centraliza o que é “núcleo”.

---

## 6. Arah.Shared

**Papel:** Biblioteca para **tipos e utilitários compartilhados** entre Application e outros projetos do backend (constantes, extensões, DTOs comuns que não pertencem a Domain nem a um módulo).

**Conteúdo:** Um assembly marcador e, no futuro, tipos compartilhados. Referenciado por **Arah.Application**. Ver `Arah.Shared/README.md` para convenções de uso.

---

## 7. Arah.Application.Abstractions

**Papel:** Contratos que **quebram ciclos de dependência** entre Application e módulos.

**Conteúdo:** Apenas duas interfaces:
- **IModule** — contrato de registro do módulo (DI, DbContext, etc.)
- **IUnitOfWorkParticipant** — participação no Unit of Work

Módulos referenciam **Application.Abstractions** (e não Application inteiro) para implementar IModule e IUnitOfWorkParticipant, evitando que Core.Application e Modules.Infrastructure se referenciem em ciclo.

---

## 8. Arah.Modules.Admin.Infrastructure

**Papel:** Módulo **apenas de infraestrutura** — sem Domain nem Application próprios.

**Conteúdo:**  
- **AdminModule.cs** — registro do módulo (IModule)  
- Projeto pequeno que registra coisas de admin (ex.: seeds, configuração) na infraestrutura. Não há entidades de domínio de “Admin”; é só wiring para funcionalidades administrativas.

**Por que “Admin” só tem Infrastructure?**  
Por decisão arquitetural (ADR): Admin não modela um contexto de negócio com entidades próprias; apenas expõe endpoints e comportamentos administrativos que usam o domínio compartilhado e outros módulos.

---

## 9. Arah.Api.Bff

**Papel:** Backend for Frontend — aplicação **separada** que expõe as jornadas (onboarding, feed, eventos, marketplace) para o frontend. O BFF **não** contém lógica de negócio; encaminha as requisições para a API principal (`Arah.Api`) e pode aplicar cache em respostas GET.

**Conteúdo principal:**
- **Middleware/** — `JourneyProxyMiddleware`: intercepta `/api/v2/journeys/*`, repassa para a API e aplica cache quando configurado.
- **Services/** — `JourneyApiProxy` (encaminha HTTP para a API), `JourneyResponseCache` (cache em memória com TTL configurável por path).
- **Contracts/** — DTOs das jornadas (espelho dos contratos da API, para documentação Swagger do BFF).
- **Namespace:** `Arah.Bff` (sem `.Api` no nome do namespace).

**Configuração:** `Bff:ApiBaseUrl` aponta para a API principal; `Bff:EnableCache`, `Bff:CacheTtlSeconds` e `Bff:CacheTtlByPath` controlam o cache de respostas GET 2xx.

**Desacoplamento:** O BFF não referencia Arah.Application nem módulos; apenas faz HTTP para a API. Testes do BFF cobrem proxy e cache em isolamento.

---

## Regras de decisão e convenções

### Onde colocar um novo repositório

- **Entidade do núcleo (Arah.Domain):** User, Territory, Membership, Policies, JoinRequests, Voting, etc. → **Arah.Infrastructure.Shared** (SharedDbContext e repositórios Postgres/InMemory).
- **Entidade de módulo:** repositório no **próprio módulo** (pasta Infrastructure do módulo, com DbContext do módulo).
- **Outro cross-cutting** (e-mail, cache, outbox, mídia, etc.) → **Arah.Infrastructure**.

### Registro de serviços (API vs IModule)

- **Repositórios e implementações de persistência** → sempre registrados no **módulo** (via `IModule.RegisterServices`) ou em **Infrastructure.Shared** para o núcleo.
- **Provedores de aplicação** (ex.: `IAcceptedConnectionsProvider`) que dependem de Application podem ser registrados na API ou em um extension method do Core chamado pela API. Evitar proliferação de registros na API; preferir registro no módulo quando possível.

### O que entra no Arah.Domain

Está no **Domain** o que é **identidade/contexto universal**: User, Territory, Membership, políticas globais (Terms, Privacy), JoinRequests, Governance (Voting), Geo (coordenadas), Configuration. O que é **contexto de negócio com regras próprias** tende a **módulo** (ex.: Feed, Map, Marketplace, Connections). Revisar periodicamente; não mover por mover — migrar para módulo só se houver ganho claro (ownership, testes isolados, evolução independente).

### Módulos: Domain não referenciar Infrastructure

Dentro de um módulo (um projeto com pastas Domain/, Application/, Infrastructure/), a pasta **Domain/** não deve referenciar tipos da pasta **Infrastructure/** do mesmo projeto. A separação não é enforced por assembly; manter por **convenção** e code review. Opcional: analyzer ou regra de CI que falhe se Domain do módulo referenciar Application ou Infrastructure.

---

## Resumo rápido

| Projeto / Pasta | O que é | “Core” / “Shared” |
|-----------------|---------|-------------------|
| **Arah.Api** | API HTTP, controllers, DTOs, auth, middleware | Pasta **Platform** em Controllers = endpoints transversais (auth, territórios, membros, etc.) |
| **Arah.Application** | Casos de uso, interfaces, serviços | Pasta **Platform** em Interfaces/Services = contratos e serviços transversais |
| **Arah.Domain** | Entidades e value objects do núcleo | É o “Core.Domain” dos ADRs — domínio compartilhado; não existe projeto “Domain.Core” |
| **Arah.Infrastructure** | Postgres/InMemory gerais, e-mail, storage, cache, eventos, workers | Infraestrutura principal |
| **Arah.Infrastructure.Shared** | Persistência (Postgres/InMemory) de User, Territory, Membership, Terms, Privacy, etc. | “Shared” = repositórios compartilhados por toda a aplicação |
| **Arah.Shared** | Tipos/utilitários compartilhados (ver README no projeto) | Referenciado por Application; evita duplicar constantes ou helpers entre projetos |
| **Arah.Application.Abstractions** | IModule, IUnitOfWorkParticipant | Quebra ciclo Application ↔ módulos |
| **Arah.Modules.Admin.Infrastructure** | Só registro de infraestrutura para admin | Sem Domain/Application; só Infrastructure por opção de design |
| **Arah.Api.Bff** | BFF: proxy de jornadas para a API, cache opcional | Namespace Arah.Bff; desacoplado da API (apenas HTTP) |

---

## Nomenclatura e redundância

- **“Platform”** (pastas em Api e Application) indica funcionalidade **transversal** (Controllers/Platform, Interfaces/Platform, Services/Platform). Nos ADRs, “Core” designa o **conjunto** de projetos do núcleo (Api, Application, Domain, Infrastructure, Infrastructure.Shared, Shared), não uma pasta.
- **Não existe** projeto “Domain.Core” ou “Arah.Domain.Core”; o domínio compartilhado é o projeto **Arah.Domain**.
- **Infrastructure vs Infrastructure.Shared:** o primeiro é a infraestrutura geral; o segundo é a persistência **compartilhada** do núcleo (User, Territory, Membership, etc.). Não são redundantes — são escopos diferentes.
- **Arah.Shared** existe para tipos e utilitários compartilhados; ver README no projeto.

Este documento reflete o estado **atual** dos projetos e pastas (projetos na raiz de `backend/`, pastas **Platform** em vez de Core onde havia ambiguidade).

Para **melhorias propostas**, **erros conhecidos** (ex.: duplicação de entidades em dois DbContexts) e **problemas comuns** de diferentes abordagens (modular monolith, shared kernel, vertical slices), ver [IMPROVEMENTS_AND_KNOWN_ISSUES.md](IMPROVEMENTS_AND_KNOWN_ISSUES.md).
