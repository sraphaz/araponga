# Fase 5: Refatoração API e Testes - Plano de Implementação

**Data**: 2026-01-27  
**Status**: 🚧 **Em Planejamento**

---

## 📋 Objetivo

Atualizar a API e testes para usar a infraestrutura modular, removendo dependências do `ArapongaDbContext` monolítico e utilizando os múltiplos `DbContext`s modulares.

---

## 🎯 Tarefas Principais

### 1. Atualizar Program.cs

- [ ] Adicionar registro de módulos via `ModuleRegistry`
- [ ] Registrar `SharedDbContext` primeiro (infraestrutura compartilhada)
- [ ] Remover registro do `ArapongaDbContext` monolítico
- [ ] Atualizar health checks para usar `SharedDbContext` ou múltiplos DbContexts
- [ ] Atualizar `ConnectionPoolMetricsService` para trabalhar com múltiplos DbContexts

### 2. Atualizar ServiceCollectionExtensions

- [ ] Modificar `AddInfrastructure()` para:
  - Registrar `SharedDbContext` e infraestrutura compartilhada
  - Remover registros de repositórios que agora estão nos módulos
  - Manter apenas serviços cross-cutting (cache, email, storage, etc.)
- [ ] Remover `AddPostgresRepositories()` ou adaptá-lo para registrar apenas repositórios compartilhados
- [ ] Garantir que módulos registrem suas próprias infraestruturas via `ModuleRegistry`

### 3. Atualizar Health Checks

- [ ] Atualizar `DatabaseHealthCheck` para verificar múltiplos DbContexts
- [ ] Criar health checks específicos por módulo (opcional)
- [ ] Atualizar `ModuleHealthCheck` se necessário

### 4. Atualizar Testes

- [ ] Verificar que `ServiceTestFactory` já registra módulos corretamente
- [ ] Atualizar testes que usam `ArapongaDbContext` diretamente
- [ ] Garantir que testes usem repositórios modulares
- [ ] Validar suite completa de testes

### 5. Atualizar Controllers (se necessário)

- [ ] Verificar se controllers já usam interfaces de repositórios (não DbContext diretamente)
- [ ] Atualizar controllers que injetam `ArapongaDbContext` diretamente

---

## 📝 Detalhamento das Mudanças

### Mudanças em Program.cs

**Antes:**
```csharp
builder.Services.AddInfrastructure(builder.Configuration);
// Usa ArapongaDbContext monolítico
```

**Depois:**
```csharp
// Registrar infraestrutura compartilhada primeiro
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddSharedCrossCuttingServices(builder.Configuration);

// Registrar módulos (que registram suas próprias infraestruturas)
var modules = new IModule[]
{
    new CoreModule(),
    new FeedModule(),
    new MarketplaceModule(),
    // ... outros módulos
};
var moduleRegistry = new ModuleRegistry(modules, logger);
moduleRegistry.Apply(builder.Services, builder.Configuration);
builder.Services.AddSingleton<IModuleRegistry>(moduleRegistry);
```

### Mudanças em ServiceCollectionExtensions

**Remover:**
- Registro de `ArapongaDbContext`
- Registros de repositórios que agora estão nos módulos (Feed, Marketplace, Events, etc.)

**Manter:**
- Registros de repositórios compartilhados (Territory, User, Membership, etc.)
- Serviços cross-cutting (cache, email, storage, etc.)

---

## ⚠️ Pontos de Atenção

1. **Ordem de Registro**: Infraestrutura compartilhada deve ser registrada antes dos módulos
2. **Connection String**: Todos os DbContexts podem usar a mesma connection string, mas com tabelas de migração diferentes
3. **UnitOfWork**: Cada DbContext implementa `IUnitOfWork`, mas pode ser necessário um `IUnitOfWork` agregado para transações cross-module
4. **Health Checks**: Precisam verificar todos os DbContexts ou pelo menos o SharedDbContext
5. **Testes**: Garantir que testes continuem funcionando com a nova estrutura

---

## ✅ Critérios de Sucesso

- [ ] API inicia sem erros
- [ ] Todos os módulos registram suas infraestruturas corretamente
- [ ] Health checks funcionam
- [ ] Testes passam
- [ ] Não há referências ao `ArapongaDbContext` monolítico (exceto em código legado que será removido na Fase 6)

---

**Próxima Fase**: Fase 6 - Cleanup e Otimização (remover código monolítico)
