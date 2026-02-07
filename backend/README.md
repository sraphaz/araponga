# Backend Arah

Estrutura do backend após modularização e organização de pastas.

## Estrutura de pastas

Os projetos do núcleo e dos módulos ficam **na raiz de `backend/`** (sem pastas físicas `Core/` ou `Modules/`).

```
backend/
├── Arah.Api/                    # API HTTP principal
├── Arah.Api.Bff/                # BFF (Backend for Frontend)
├── Arah.Application/            # Serviços transversais, orquestração
├── Arah.Application.Abstractions/  # IModule, IUnitOfWorkParticipant (evita ciclo)
├── Arah.Domain/                 # Domínio compartilhado (Users, Territories, etc.)
├── Arah.Infrastructure/         # Infraestrutura principal (Postgres, Redis, e-mail, etc.)
├── Arah.Infrastructure.Shared/  # Repositórios compartilhados (User, Territory, Membership, etc.)
├── Arah.Shared/                 # Projeto compartilhado (atualmente apenas referência)
├── Arah.Modules.Admin.Infrastructure/
├── Arah.Modules.Alerts/
├── Arah.Modules.Assets/
├── Arah.Modules.Chat/
├── Arah.Modules.Connections/
├── Arah.Modules.Events/
├── Arah.Modules.Feed/
├── Arah.Modules.Map/
├── Arah.Modules.Marketplace/
├── Arah.Modules.Moderation/
├── Arah.Modules.Notifications/
├── Arah.Modules.Subscriptions/
├── Tests/                           # Todos os projetos de teste
│   ├── Arah.Tests/
│   ├── Arah.Tests.Shared/
│   ├── Arah.Tests.Modules.Connections/
│   ├── Arah.Tests.Modules.Map/
│   ├── Arah.Tests.Modules.Marketplace/
│   ├── Arah.Tests.Modules.Moderation/
│   ├── Arah.Tests.Modules.Subscriptions/
│   ├── Arah.Tests.ApiSupport/
│   └── Arah.Tests.Bff/
└── docs/
    └── BACKEND_LAYERS_AND_NAMING.md  # Detalhe do que cada projeto/pasta contém
```

## Estrutura dos módulos

Use **um único padrão** para todos os módulos: **estrutura flat** (como Connections e Feed).

- **Padrão:** `Arah.Modules.<Nome>/` contém o `.csproj` na raiz e as pastas `Domain/`, `Application/`, `Infrastructure/` diretamente (sem subpasta aninhada `Arah.Modules.<Nome>/Arah.Modules.<Nome>/`).
- Ao criar um novo módulo, copie a estrutura de **Connections** ou **Feed** (raiz com .csproj + Domain/Application/Infrastructure).
- Documentação detalhada: [BACKEND_LAYERS_AND_NAMING.md](docs/BACKEND_LAYERS_AND_NAMING.md) e [IMPROVEMENTS_AND_KNOWN_ISSUES.md](docs/IMPROVEMENTS_AND_KNOWN_ISSUES.md).

## Onde colocar um novo repositório

- **Entidade do núcleo (Arah.Domain):** User, Territory, Membership, Policies, etc. → **Arah.Infrastructure.Shared**.
- **Entidade de módulo:** repositório no **próprio módulo** (pasta Infrastructure do módulo).
- **Outro cross-cutting** (e-mail, cache, outbox, mídia): **Arah.Infrastructure**.

Detalhes: [BACKEND_LAYERS_AND_NAMING.md](docs/BACKEND_LAYERS_AND_NAMING.md) (seção “Regras de decisão”).

## Regras de dependência (evitar referências circulares)

1. **Core.Application** referencia todos os módulos (camada Application) para contratos; não referencia *Infrastructure* dos módulos.
2. **Core.Infrastructure** referencia Core + módulos necessários (Application/Domain); não referencia *Infrastructure* dos módulos que referenciam Application (evita ciclo).
3. **Módulos** (um projeto cada, com Domain/Application/Infrastructure) referenciam **Arah.Application.Abstractions** (IModule, IUnitOfWorkParticipant). **Connections** e **Feed** têm sua própria Infrastructure (DbContext e repositórios Postgres no módulo); Feed registra também IPostGeoAnchorRepository e IPostAssetRepository.
4. **Módulos não referenciam outros módulos** em princípio. Exceção documentada: **Feed** referencia **Map** (tipos de geo para itens do feed); ver ADR-014 e [Modules/README.md](Modules/README.md).

## Referências circulares — verificação

- Não há ciclo: Core.Application → Modules (Application); Modules.Infrastructure → Core.Application.Abstractions ou Core.Infrastructure.Shared (não Core.Application). O projeto **Arah.Application.Abstractions** quebra o ciclo entre Application e módulos que implementam IModule.
- **Feed → Map**: única dependência entre módulos; é explícita e documentada.

## Logging

- **ILogger** (Microsoft.Extensions.Logging / Serilog): uso geral para logs operacionais, debug e rastreamento. Controllers, serviços e workers usam ILogger.
- **IObservabilityLogger**: interface de aplicação para eventos de negócio e métricas que devem ser tratados de forma uniforme (agregação, alertas). Usado em RequestLoggingMiddleware e ReportService; use para eventos como “report criado”, “falha de moderação”, “erro de geolocalização” e métricas de requisição HTTP.

## Build e testes

```bash
# Na raiz do repositório
dotnet build Arah.sln
dotnet test Arah.sln

# Testes de um módulo
dotnet test backend/Tests/Arah.Tests.Modules.Marketplace/Arah.Tests.Modules.Marketplace.csproj
dotnet test backend/Tests/Arah.Tests.Modules.Map/Arah.Tests.Modules.Map.csproj
```

## Documentação

- [Camadas e nomenclatura](docs/BACKEND_LAYERS_AND_NAMING.md) — o que está em Api, Application, Domain, Infrastructure, Infrastructure.Shared, Shared; significado de "Platform" e "Shared".
- [Melhorias, erros conhecidos e problemas comuns](docs/IMPROVEMENTS_AND_KNOWN_ISSUES.md) — melhorias de estrutura e módulos, assimetrias conhecidas, problemas comuns de abordagens (modular monolith, shared kernel, etc.) e ações priorizadas.
- [Decisões arquiteturais (ADRs)](../docs/10_ARCHITECTURE_DECISIONS.md) — ADR-012, ADR-014, ADR-015, ADR-016.
