# Alterações locais ainda não commitadas

Este arquivo lista o que estava aberto localmente e **não** foi incluído no último commit (PR do lançamento do app). Use como checklist para decidir em qual branch/PR subir.

**Data de referência:** fevereiro 2025 (após commit `chore(app): preparar lançamento do app`).

---

## Modificados (não commitados)

| Caminho | Descrição provável |
|--------|---------------------|
| `Arah.sln` | Solução |
| `Dockerfile` (raiz) | Build da API (raiz) |
| `backend/Arah.Api.Bff/Arah.Api.Bff.http` | Requisições HTTP |
| `backend/Arah.Api/Controllers/Feed/FeedController.cs` | Controller feed |
| `backend/Arah.Api/Controllers/Journeys/FeedJourneyController.cs` | Jornada feed |
| `backend/Arah.Api/Dockerfile` | Docker da API (usa SDK/Runtime 8.0; CVE em 8.0.12 via Directory.Build.props) |
| `backend/Arah.Api/Extensions/ServiceCollectionExtensions.cs` | DI |
| `backend/Arah.Application/Models/FeatureFlag.cs` | Feature flags |
| `backend/Arah.Application/Services/Platform/MembershipService.cs` | Membership |
| `backend/Arah.Application/Services/Platform/TerritoryService.cs` | Territórios |
| `backend/Arah.Domain/Territories/Territory.cs` | Entidade território |
| `backend/Arah.Domain/Users/SystemPermissionType.cs` | Permissões |
| `backend/Arah.Infrastructure.Shared/Postgres/*` | Shared EF + mappers |
| `backend/Arah.Infrastructure/Postgres/*` | DbContext, migrations, mappers |
| `backend/Directory.Build.props` | Versão .NET (RuntimeFrameworkVersion 8.0.12) |
| `backend/Tests/Arah.Tests/GlobalUsings.cs` | Usings |
| `backend/docs/IMPLEMENTATION_PLAN_NEXT_STEPS.md` | Plano |
| `backend/scripts/validate-module-boundaries.ps1` | Script validação |
| `scripts/run-coverage.ps1` | Coverage |

---

## Não rastreados (novos arquivos)

| Caminho | Descrição provável |
|--------|---------------------|
| `backend/Arah.Application/Common/GeographyHelper.cs` | Helper geografia |
| `backend/Arah.Application/Interfaces/Platform/IGeoConvergenceBypassService.cs` | Interface bypass geo |
| `backend/Arah.Application/Services/Platform/GeoConvergenceBypassService.cs` | Serviço bypass geo |
| `backend/Arah.Infrastructure/Postgres/Migrations/20260203120000_AddRadiusKmToTerritories.cs` | Migração RadiusKm |

---

## Como subir

- **Opção A:** Criar um branch (ex.: `fix/trivy-dotnet-8.0.13` ou `feat/backend-territory-geo`) e commitar apenas as alterações de CVE + Docker/CI (já feitas neste PR) ou também o backend/território/geo.
- **Opção B:** Incluir as alterações de backend/território/geo no mesmo branch do app (ex.: `feat/frontend-Arah-app`) em um novo commit e atualizar o PR.

**Nota:** Este arquivo pode ser removido após as alterações serem commitadas ou quando não for mais necessário.
