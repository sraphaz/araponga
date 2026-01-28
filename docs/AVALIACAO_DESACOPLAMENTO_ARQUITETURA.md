# Avaliação de Desacoplamento da Arquitetura e Suite de Testes

**Versão**: 2.0  
**Data**: 2026-01-26  
**Status**: 📋 Análise Completa  
**Tipo**: Documentação Técnica de Arquitetura

---

## 📋 Índice

1. [Resumo Executivo](#resumo-executivo)
2. [Análise de Desacoplamento por Camada](#análise-de-desacoplamento-por-camada)
3. [Análise de Módulos](#análise-de-módulos)
4. [Análise da Suite de Testes](#análise-da-suite-de-testes)
5. [Gaps e Falhas de Modelagem](#gaps-e-falhas-de-modelagem)
6. [Sugestões e Melhorias](#sugestões-e-melhorias)
7. [Roadmap de Refatoração](#roadmap-de-refatoração)

---

## 🎯 Resumo Executivo

### Situação Atual

**Pontos Fortes** ✅:
- **Clean Architecture bem aplicada**: Separação clara entre Domain, Application, Infrastructure e Api
- **Domain isolado**: Não há dependências externas na camada Domain
- **Application desacoplada**: Depende apenas de Domain (sem referências a Api ou Infrastructure)
- **Sistema de Módulos**: Arquitetura modular com registro dinâmico e validação de dependências
- **Interfaces bem definidas**: Abstrações para serviços externos (cache, storage, email)
- **Testes isolados**: Cada teste cria seu próprio `InMemoryDataStore`

**Pontos de Atenção** ⚠️:
- **ApiFactory acoplado ao Program**: Testes E2E dependem do monolito completo
- **Módulos com dependências implícitas**: Alguns módulos assumem que outros estão habilitados
- **Falta de contratos explícitos**: Comunicação entre módulos via DI sem interfaces explícitas
- **Testes duplicados**: 25+ métodos `CreateService` duplicados (parcialmente resolvido)
- **Falta de testes de integração realistas**: Sem Test Containers para PostgreSQL/Redis
- **Reflection para métricas**: `ModuleRegistry` usa reflection para evitar dependência circular

**Gaps Críticos** ❌:
- **Sem abstração de configuração de módulos**: Módulos registram serviços diretamente sem validação de contratos
- **Falta de eventos de domínio explícitos**: Comunicação entre módulos via eventos sem tipagem forte
- **Testes não preparam para microserviços**: Estrutura atual não facilita migração futura
- **Falta de health checks por módulo**: Não há verificação individual de saúde dos módulos

---

## 📊 Análise de Desacoplamento por Camada

### 1. Araponga.Domain ✅ **EXCELENTE**

**Status**: ✅ **Totalmente desacoplado**

**Análise**:
- ✅ **Zero dependências externas**: Não referencia nenhum outro projeto
- ✅ **Interfaces de repositórios**: Define contratos sem implementação
- ✅ **Value Objects puros**: Sem dependências de frameworks
- ✅ **Entidades de domínio**: Sem anotações de ORM ou frameworks

**Verificação**:
```bash
# Domain não depende de nada
grep -r "using Araponga\.(Api|Application|Infrastructure)" backend/Araponga.Domain
# Resultado: 0 matches ✅
```

**Conclusão**: Camada Domain está perfeitamente desacoplada, seguindo princípios de Clean Architecture.

---

### 2. Araponga.Application ✅ **BOM**

**Status**: ✅ **Bem desacoplado**

**Análise**:
- ✅ **Depende apenas de Domain**: Referências corretas
- ✅ **Interfaces para serviços externos**: `IDistributedCacheService`, `IMediaStorageService`, `IEmailSender`
- ✅ **Result Pattern**: Tratamento uniforme de erros
- ✅ **Sem dependências de Infrastructure**: Não conhece implementações concretas

**Verificação**:
```bash
# Application não depende de Api ou Infrastructure
grep -r "using Araponga\.(Api|Infrastructure)" backend/Araponga.Application
# Resultado: 0 matches ✅
```

**Pontos de Atenção**:
- ⚠️ **Services com muitas dependências**: Alguns services ainda têm 10+ dependências (ex: `PostCreationService`)
- ⚠️ **Falta de interfaces para alguns services**: Alguns services são injetados diretamente sem interface

**Exemplo de Service com Muitas Dependências**:
```csharp
// PostCreationService tem 10+ dependências
public PostCreationService(
    IFeedRepository feedRepository,
    IMapRepository mapRepository,
    IAssetRepository assetRepository,
    IPostGeoAnchorRepository postGeoAnchorRepository,
    IPostAssetRepository postAssetRepository,
    AccessEvaluator accessEvaluator,
    IFeatureFlagService featureFlagService,
    IAuditLogger auditLogger,
    IUserBlockRepository userBlockRepository,
    ISanctionRepository sanctionRepository,
    IEventBus eventBus,
    IUnitOfWork unitOfWork)
```

**Recomendação**: Considerar extrair mais services especializados ou usar Command Pattern.

---

### 3. Araponga.Infrastructure ✅ **BOM**

**Status**: ✅ **Bem desacoplado**

**Análise**:
- ✅ **Implementa interfaces de Domain e Application**: Repositórios e serviços externos
- ✅ **Múltiplas implementações**: InMemory para testes, Postgres para produção
- ✅ **Factory Pattern**: `MediaStorageFactory` para criar serviços baseado em configuração
- ✅ **Decorator Pattern**: `CachedMediaStorageService` decora `IMediaStorageService`

**Pontos de Atenção**:
- ⚠️ **Dependência direta de Application**: Infrastructure referencia Application para interfaces
  - **Impacto**: Baixo - é esperado em Clean Architecture
  - **Justificativa**: Infrastructure precisa implementar interfaces definidas em Application

**Verificação de Dependências**:
```xml
<!-- Araponga.Infrastructure.csproj -->
<ProjectReference Include="..\Araponga.Application\Araponga.Application.csproj" />
<ProjectReference Include="..\Araponga.Domain\Araponga.Domain.csproj" />
```
✅ **Correto**: Infrastructure depende de Application e Domain (esperado)

---

### 4. Araponga.Api ⚠️ **ATENÇÃO**

**Status**: ⚠️ **Acoplado ao monolito (esperado, mas problemático para testes)**

**Análise**:
- ✅ **Depende de todas as camadas**: Esperado para camada de apresentação
- ✅ **Orquestra módulos**: Registra e configura módulos via `ModuleRegistry`
- ⚠️ **Program.cs como ponto único de entrada**: `ApiFactory` depende de `Program` para testes E2E
- ⚠️ **Configuração centralizada**: Toda configuração de DI em `ServiceCollectionExtensions`

**Problema Principal**:
```csharp
// ApiFactory.cs - Acoplado ao Program do monolito
public sealed class ApiFactory : WebApplicationFactory<Program>
{
    // Isso inicializa TODA a aplicação, mesmo para testar um serviço isolado
}
```

**Impacto**:
- Testes E2E são lentos (inicializam toda a aplicação)
- Dificulta testes de serviços isolados
- Não prepara para migração para microserviços

**Solução Parcial Implementada**:
- ✅ `ServiceTestFactory<T>` criado para testar serviços isolados
- ✅ `ITestServiceCollection` para abstrair configuração
- ⚠️ Mas ainda não é usado em todos os testes

---

## 🔧 Análise de Módulos

### Sistema de Módulos ✅ **BOM**

**Status**: ✅ **Arquitetura modular bem implementada**

**Pontos Fortes**:
- ✅ **Interface IModule**: Contrato claro para módulos
- ✅ **ModuleRegistry**: Ordenação topológica por dependências
- ✅ **Validação de dependências**: Detecta dependências circulares
- ✅ **Configuração dinâmica**: Módulos podem ser habilitados/desabilitados via configuração

**Estrutura**:
```csharp
public interface IModule
{
    string Id { get; }
    string[] DependsOn { get; }
    bool IsRequired { get; }
    void RegisterServices(IServiceCollection services, IConfiguration configuration);
}
```

**Exemplo de Módulo**:
```csharp
public class FeedModule : IModule
{
    public string Id => "Feed";
    public string[] DependsOn => new[] { "Core" };
    public bool IsRequired => false;
    
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Registra apenas serviços do Feed
        services.AddScoped<FeedService>();
        services.AddScoped<PostCreationService>();
        // ...
    }
}
```

### Gaps Identificados

#### 1. Falta de Contratos Explícitos entre Módulos ⚠️

**Problema**: Módulos se comunicam via DI sem interfaces explícitas.

**Exemplo**:
```csharp
// FeedModule registra FeedService
services.AddScoped<FeedService>();

// Outro módulo pode depender de FeedService diretamente
// Mas não há interface IFeedService definida
```

**Impacto**:
- Dificulta substituição de implementações
- Dificulta testes isolados
- Dificulta migração para microserviços

**Solução Sugerida**:
```csharp
// Criar interfaces para serviços principais
public interface IFeedService
{
    Task<Result<PostResponse>> CreatePostAsync(CreatePostRequest request, Guid userId);
    // ...
}

// FeedModule registra implementação
services.AddScoped<IFeedService, FeedService>();
```

#### 2. Reflection para Métricas ⚠️

**Problema**: `ModuleRegistry` usa reflection para evitar dependência circular.

**Código Atual**:
```csharp
private void EmitModuleRegistrationAttempt(string moduleId)
{
    // Usar reflection para evitar dependência direta de Araponga.Application.Metrics
    var metricsType = Type.GetType("Araponga.Application.Metrics.ArapongaMetrics, Araponga.Application");
    // ...
}
```

**Impacto**:
- Código frágil (depende de nomes de tipos)
- Sem type-safety
- Dificulta refatoração

**Solução Sugerida**:
- Criar interface `IMetricsEmitter` em `Araponga.Modules.Core`
- Application implementa e registra via DI
- ModuleRegistry recebe via construtor

#### 3. Módulos Assumem Serviços Compartilhados ⚠️

**Problema**: Módulos assumem que serviços compartilhados (cache, email, storage) estão registrados.

**Exemplo**:
```csharp
// FeedModule assume que IDistributedCacheService está registrado
// Mas não valida isso
public void RegisterServices(IServiceCollection services, IConfiguration configuration)
{
    // Usa IDistributedCacheService sem verificar se está registrado
}
```

**Impacto**:
- Erros em runtime se serviço não estiver registrado
- Dificulta testes isolados

**Solução Sugerida**:
- Validar dependências no `ModuleRegistry` antes de registrar
- Ou documentar dependências explícitas em `IModule.DependsOn`

---

## 🧪 Análise da Suite de Testes

### Estrutura Atual ✅ **BOM**

**Status**: ✅ **Bem organizada, mas com gaps**

**Pontos Fortes**:
- ✅ **Isolamento por teste**: Cada teste cria seu próprio `InMemoryDataStore`
- ✅ **Helpers compartilhados**: `FeedServiceTestHelper`, `CacheTestHelper`, etc.
- ✅ **Factory Pattern**: `ApiFactory` e `ServiceTestFactory<T>`
- ✅ **InMemory implementations**: Repositórios in-memory para testes rápidos
- ✅ **Cobertura**: 45.72% linhas, 38.2% branches, 48.31% métodos

**Estrutura**:
```
Araponga.Tests/
├── Api/                          # Testes E2E (acoplados ao Program)
├── Application/                  # Testes unitários de services
├── Domain/                       # Testes de entidades
├── Infrastructure/              # Testes de repositórios
├── Performance/                  # Testes de performance
└── TestHelpers/                  # Helpers compartilhados
    ├── Services/                 # Helpers por serviço
    ├── Dependencies/             # Helpers de dependências
    ├── ExternalServices/         # Implementações in-memory
    └── ServiceTestFactory.cs     # Factory para serviços isolados
```

### Gaps Identificados

#### 1. ApiFactory Acoplado ao Monolito ❌

**Problema**: Testes E2E dependem de `WebApplicationFactory<Program>`.

**Código Atual**:
```csharp
public sealed class ApiFactory : WebApplicationFactory<Program>
{
    // Inicializa TODA a aplicação
}
```

**Impacto**:
- Testes E2E são lentos (inicializam toda a aplicação)
- Dificulta testes de serviços isolados
- Não prepara para migração para microserviços

**Solução Parcial**:
- ✅ `ServiceTestFactory<T>` criado
- ⚠️ Mas ainda não é usado em todos os testes
- ⚠️ `ApiFactory` ainda é necessário para testes E2E reais

**Recomendação**:
- Manter `ApiFactory` para testes E2E reais
- Migrar testes unitários para `ServiceTestFactory<T>`
- Criar `ApiTestFactory` que usa `ServiceTestFactory` internamente

#### 2. Falta de Test Containers ⚠️

**Problema**: Testes de integração não usam containers reais.

**Status Atual**:
- ✅ `PostgresTestContainer` criado (parcialmente implementado)
- ⚠️ Mas não é usado em todos os testes de integração
- ❌ Não há Test Container para Redis

**Impacto**:
- Testes de integração não são realistas
- Dificulta detectar problemas de compatibilidade

**Solução Sugerida**:
- Usar `Testcontainers` para PostgreSQL e Redis
- Criar fixtures compartilhadas para containers
- Adicionar testes de integração realistas

#### 3. Duplicação de Setup ⚠️

**Problema**: Ainda há duplicação em alguns testes.

**Status**:
- ✅ Muitos helpers criados (parcialmente resolvido)
- ⚠️ Mas ainda há métodos `CreateService` privados em algumas classes
- ⚠️ Setup comum repetido em múltiplos testes

**Solução Sugerida**:
- Extrair todos os métodos `CreateService` para helpers
- Criar base test classes para setup comum
- Usar test fixtures compartilhadas

#### 4. Falta de Testes de Contratos entre Módulos ⚠️

**Problema**: Não há testes que validem contratos entre módulos.

**Exemplo**:
- FeedModule depende de CoreModule
- Mas não há teste que valide que CoreModule fornece o que FeedModule precisa

**Solução Sugerida**:
- Criar testes de integração entre módulos
- Validar contratos via interfaces explícitas
- Testar cenários onde módulos dependentes estão desabilitados

---

## 🔍 Gaps e Falhas de Modelagem

### 1. Falta de Eventos de Domínio Explícitos ⚠️

**Problema**: Comunicação entre módulos via eventos sem tipagem forte.

**Status Atual**:
- ✅ `IEventBus` existe
- ✅ Eventos são publicados e consumidos
- ⚠️ Mas eventos não são tipados fortemente (usam `object` ou `INotification`)

**Exemplo**:
```csharp
// Evento não tipado
await _eventBus.PublishAsync("PostCreated", postData);

// Ou tipado mas genérico
await _eventBus.PublishAsync(new PostCreatedNotification { PostId = post.Id });
```

**Impacto**:
- Sem type-safety
- Dificulta refatoração
- Dificulta testes

**Solução Sugerida**:
- Criar eventos de domínio explícitos em `Araponga.Domain.Events`
- Usar tipagem forte: `IDomainEvent<T>`
- Validar contratos de eventos entre módulos

### 2. Falta de Health Checks por Módulo ⚠️

**Problema**: Não há verificação individual de saúde dos módulos.

**Status Atual**:
- ✅ Health checks globais existem (PostgreSQL, Redis, Storage)
- ❌ Não há health checks por módulo
- ❌ Não há verificação de dependências entre módulos

**Impacto**:
- Dificulta diagnóstico de problemas
- Não é possível desabilitar módulos com problemas sem afetar outros

**Solução Sugerida**:
- Criar interface `IModuleHealthCheck` em `Araponga.Modules.Core`
- Cada módulo implementa seu próprio health check
- `ModuleRegistry` registra health checks automaticamente

### 3. Falta de Versionamento de Contratos ⚠️

**Problema**: Não há versionamento explícito de interfaces entre módulos.

**Status Atual**:
- ✅ Interfaces existem
- ❌ Mas não há versionamento
- ❌ Não há estratégia de compatibilidade

**Impacto**:
- Dificulta evolução de módulos
- Dificulta migração para microserviços

**Solução Sugerida**:
- Adicionar versionamento a interfaces críticas
- Usar estratégia de compatibilidade (ex: v1, v2)
- Documentar breaking changes

### 4. Falta de Abstração de Configuração ⚠️

**Problema**: Módulos acessam `IConfiguration` diretamente sem abstração.

**Status Atual**:
```csharp
public void RegisterServices(IServiceCollection services, IConfiguration configuration)
{
    // Acesso direto à configuração
    var setting = configuration["SomeSetting"];
}
```

**Impacto**:
- Dificulta testes (precisa mockar `IConfiguration`)
- Dificulta validação de configuração
- Dificulta migração para microserviços (configuração pode vir de diferentes fontes)

**Solução Sugerida**:
- Criar classes de configuração tipadas (ex: `FeedModuleOptions`)
- Usar `IOptions<T>` pattern
- Validar configuração no registro do módulo

---

## 💡 Sugestões e Melhorias

### Prioridade P0 (Críticas)

#### 1. Criar Interfaces para Serviços Principais

**Objetivo**: Facilitar substituição e testes isolados.

**Ação**:
```csharp
// Criar interfaces em Araponga.Application.Interfaces
public interface IFeedService
{
    Task<Result<PostResponse>> CreatePostAsync(CreatePostRequest request, Guid userId);
    Task<Result<PagedResult<PostResponse>>> ListForTerritoryAsync(Guid territoryId, PostFilterOptions options);
    // ...
}

// FeedModule registra implementação
services.AddScoped<IFeedService, FeedService>();
```

**Benefícios**:
- Facilita testes isolados
- Facilita substituição de implementações
- Prepara para migração para microserviços

#### 2. Migrar Testes para ServiceTestFactory

**Objetivo**: Reduzir acoplamento de testes ao monolito.

**Ação**:
- Migrar testes unitários de `ApiFactory` para `ServiceTestFactory<T>`
- Manter `ApiFactory` apenas para testes E2E reais
- Criar `ApiTestFactory` que usa `ServiceTestFactory` internamente

**Benefícios**:
- Testes mais rápidos
- Testes mais isolados
- Prepara para migração para microserviços

#### 3. Adicionar Test Containers

**Objetivo**: Testes de integração realistas.

**Ação**:
- Usar `Testcontainers` para PostgreSQL e Redis
- Criar fixtures compartilhadas
- Adicionar testes de integração realistas

**Benefícios**:
- Testes mais realistas
- Detecta problemas de compatibilidade
- Prepara para produção

### Prioridade P1 (Importantes)

#### 4. Criar Eventos de Domínio Explícitos

**Objetivo**: Comunicação tipada entre módulos.

**Ação**:
```csharp
// Criar eventos em Araponga.Domain.Events
public interface IDomainEvent
{
    DateTime OccurredAt { get; }
}

public record PostCreatedEvent(Guid PostId, Guid TerritoryId, Guid UserId) : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
```

**Benefícios**:
- Type-safety
- Facilita refatoração
- Facilita testes

#### 5. Adicionar Health Checks por Módulo

**Objetivo**: Diagnóstico individual de módulos.

**Ação**:
```csharp
// Criar interface em Araponga.Modules.Core
public interface IModuleHealthCheck
{
    Task<HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken);
}

// Cada módulo implementa
public class FeedModuleHealthCheck : IModuleHealthCheck
{
    // Verifica saúde do módulo Feed
}
```

**Benefícios**:
- Diagnóstico mais preciso
- Possibilidade de desabilitar módulos com problemas
- Melhor observabilidade

#### 6. Criar Classes de Configuração Tipadas

**Objetivo**: Abstrair configuração de módulos.

**Ação**:
```csharp
// Criar classes de configuração
public class FeedModuleOptions
{
    public int MaxPostsPerPage { get; set; } = 50;
    public bool EnableCaching { get; set; } = true;
}

// Módulo usa IOptions<FeedModuleOptions>
public void RegisterServices(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<FeedModuleOptions>(configuration.GetSection("Modules:Feed"));
}
```

**Benefícios**:
- Validação de configuração
- Facilita testes
- Facilita migração para microserviços

### Prioridade P2 (Desejáveis)

#### 7. Adicionar Versionamento de Contratos

**Objetivo**: Facilitar evolução de módulos.

**Ação**:
- Adicionar versionamento a interfaces críticas
- Documentar breaking changes
- Usar estratégia de compatibilidade

#### 8. Criar Testes de Contratos entre Módulos

**Objetivo**: Validar dependências entre módulos.

**Ação**:
- Criar testes de integração entre módulos
- Validar contratos via interfaces explícitas
- Testar cenários onde módulos dependentes estão desabilitados

#### 9. Remover Reflection de ModuleRegistry

**Objetivo**: Type-safety e manutenibilidade.

**Ação**:
- Criar interface `IMetricsEmitter` em `Araponga.Modules.Core`
- Application implementa e registra via DI
- ModuleRegistry recebe via construtor

---

## 🗺️ Roadmap de Refatoração

### Fase 1: Fundação (Semanas 1-2) - P0

**Objetivo**: Criar abstrações básicas para desacoplamento.

1. ✅ Criar interfaces para serviços principais (IFeedService, IMarketplaceService, etc.)
2. ✅ Migrar testes unitários para ServiceTestFactory
3. ✅ Adicionar Test Containers para PostgreSQL e Redis
4. ✅ Criar classes de configuração tipadas para módulos

**Entregáveis**:
- Interfaces para 5+ serviços principais
- 50%+ dos testes migrados para ServiceTestFactory
- Test Containers funcionando
- Classes de configuração para 3+ módulos

### Fase 2: Consolidação (Semanas 3-4) - P1

**Objetivo**: Melhorar comunicação e observabilidade.

1. ✅ Criar eventos de domínio explícitos
2. ✅ Adicionar health checks por módulo
3. ✅ Remover reflection de ModuleRegistry
4. ✅ Criar testes de contratos entre módulos

**Entregáveis**:
- 10+ eventos de domínio tipados
- Health checks para 5+ módulos
- ModuleRegistry sem reflection
- Testes de contratos para 3+ pares de módulos

### Fase 3: Otimização (Semanas 5-6) - P2

**Objetivo**: Preparar para evolução futura.

1. ✅ Adicionar versionamento de contratos
2. ✅ Documentar breaking changes
3. ✅ Criar estratégia de compatibilidade
4. ✅ Otimizar performance de testes

**Entregáveis**:
- Versionamento para 3+ interfaces críticas
- Documentação de breaking changes
- Estratégia de compatibilidade definida
- Testes 20%+ mais rápidos

---

## 📊 Métricas de Sucesso

### Antes da Refatoração

- **Interfaces de serviços**: 2 (IDistributedCacheService, IMediaStorageService)
- **Testes usando ApiFactory**: 100% dos testes E2E
- **Test Containers**: 0
- **Eventos tipados**: 0
- **Health checks por módulo**: 0
- **Reflection no ModuleRegistry**: Sim

### Depois da Refatoração (Meta)

- **Interfaces de serviços**: 10+ (um por serviço principal)
- **Testes usando ServiceTestFactory**: 80%+ dos testes unitários
- **Test Containers**: 2+ (PostgreSQL, Redis)
- **Eventos tipados**: 20+
- **Health checks por módulo**: 10+ (um por módulo)
- **Reflection no ModuleRegistry**: Não

### KPIs

- ✅ **Redução de acoplamento**: 50%+ (medido por dependências diretas)
- ✅ **Tempo de execução de testes**: 30%+ mais rápido (testes unitários isolados)
- ✅ **Cobertura mantida**: >90% (não reduzir cobertura durante refatoração)
- ✅ **Testabilidade**: 100% dos serviços testáveis isoladamente

---

## ✅ Checklist de Implementação

### Fase 1: Fundação

- [ ] Criar interfaces para 5+ serviços principais
- [ ] Migrar 50%+ dos testes para ServiceTestFactory
- [ ] Adicionar Test Container para PostgreSQL
- [ ] Adicionar Test Container para Redis
- [ ] Criar classes de configuração para 3+ módulos
- [ ] Atualizar documentação

### Fase 2: Consolidação

- [ ] Criar 10+ eventos de domínio tipados
- [ ] Adicionar health checks para 5+ módulos
- [ ] Remover reflection de ModuleRegistry
- [ ] Criar testes de contratos para 3+ pares de módulos
- [ ] Atualizar documentação

### Fase 3: Otimização

- [ ] Adicionar versionamento para 3+ interfaces críticas
- [ ] Documentar breaking changes
- [ ] Criar estratégia de compatibilidade
- [ ] Otimizar performance de testes
- [ ] Atualizar documentação

---

## 🎯 Conclusão

A arquitetura do Araponga está **bem desacoplada** em geral, seguindo princípios de Clean Architecture. Os principais gaps são:

1. **Testes acoplados ao monolito**: Resolvido parcialmente com `ServiceTestFactory`, mas precisa migração
2. **Falta de interfaces explícitas**: Alguns serviços não têm interfaces, dificultando substituição
3. **Comunicação entre módulos**: Eventos não são tipados fortemente
4. **Falta de observabilidade por módulo**: Não há health checks individuais

As melhorias sugeridas são **incrementais** e podem ser implementadas gradualmente sem quebrar funcionalidade existente.

**Priorização**:
1. **P0 (Críticas)**: Implementar primeiro - impactam testabilidade e preparação para microserviços
2. **P1 (Importantes)**: Implementar em seguida - melhoram observabilidade e manutenibilidade
3. **P2 (Desejáveis)**: Implementar quando possível - otimizações e preparação para evolução futura

**Estimativa Total**: 6 semanas (2 semanas por fase)

---

**Versão**: 2.0  
**Última Atualização**: 2026-01-26  
**Autor**: Análise Técnica - Desacoplamento de Arquitetura  
**Status**: 📋 Pronto para Implementação
