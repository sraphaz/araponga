# Health Checks e Métricas por Módulo

**Data de Implementação**: 2026-01-26  
**Status**: ✅ **COMPLETO**

---

## 🎯 Objetivo

Implementar health checks e métricas específicas para o sistema de módulos, permitindo monitoramento e observabilidade do status dos módulos da aplicação.

---

## ✅ Implementações

### 1. Health Checks por Módulo

#### 1.1 ModuleHealthCheck

**Arquivo**: `backend/Araponga.Api/HealthChecks/ModuleHealthCheck.cs`

**Funcionalidades**:
- Verifica se todos os módulos obrigatórios estão habilitados
- Retorna status `Unhealthy` se módulos obrigatórios estiverem desabilitados
- Retorna status `Degraded` se nenhum módulo estiver habilitado
- Retorna status `Healthy` com informações sobre módulos habilitados
- Inclui dados sobre quantidade de módulos habilitados e obrigatórios

**Tags**: `modules`, `ready`

**Exemplo de Resposta**:
```json
{
  "status": "Healthy",
  "description": "Módulos funcionando corretamente. 12 módulo(s) habilitado(s).",
  "data": {
    "enabled_modules": 12,
    "enabled_modules_list": "Core, Feed, Marketplace, Subscriptions, Chat, Events, Map, Moderation, Notifications, Alerts, Assets, Admin",
    "required_modules": 1
  }
}
```

#### 1.2 Integração no Program.cs

O health check é registrado automaticamente após o `ModuleRegistry` ser criado:

```csharp
healthChecksBuilder.AddCheck<ModuleHealthCheck>(
    "modules",
    failureStatus: HealthStatus.Degraded,
    tags: new[] { "modules", "ready" });
```

**Endpoint**: `/health/ready` (incluído no health check geral)

---

### 2. Métricas por Módulo

#### 2.1 Métricas Adicionadas

**Arquivo**: `backend/Araponga.Application/Metrics/ArapongaMetrics.cs`

**Métricas Implementadas**:

1. **ObservableGauge: `araponga.modules.enabled`**
   - Tipo: ObservableGauge<long>
   - Unidade: count
   - Descrição: Número de módulos habilitados
   - Atualização: Em tempo real via callback

2. **ObservableGauge: `araponga.modules.total`**
   - Tipo: ObservableGauge<long>
   - Unidade: count
   - Descrição: Total de módulos disponíveis
   - Atualização: Em tempo real via callback

3. **Counter: `araponga.modules.registration.attempts`**
   - Tipo: Counter<long>
   - Unidade: count
   - Descrição: Total de tentativas de registro de módulos
   - Tags: `module_id` (ID do módulo sendo registrado)

4. **Counter: `araponga.modules.registration.failures`**
   - Tipo: Counter<long>
   - Unidade: count
   - Descrição: Total de falhas no registro de módulos
   - Tags: `module_id` (ID do módulo que falhou)

#### 2.2 Instrumentação no ModuleRegistry

**Arquivo**: `backend/Araponga.Application/Modules/ModuleRegistry.cs`

**Instrumentação Adicionada**:
- Incrementa `ModuleRegistrationAttempts` antes de registrar cada módulo
- Incrementa `ModuleRegistrationFailures` quando ocorre exceção durante registro
- Tags incluem `module_id` para rastreamento por módulo

**Exemplo**:
```csharp
ArapongaMetrics.ModuleRegistrationAttempts.Add(1, 
    new KeyValuePair<string, object?>("module_id", moduleId));
state.Module.RegisterServices(services, configuration);
_enabledModules.Add(moduleId);
```

#### 2.3 Configuração no Program.cs

As métricas são configuradas após o `ModuleRegistry` ser criado:

```csharp
ArapongaMetrics.ConfigureModuleMetrics(
    () => moduleRegistryForMetrics.GetEnabledModules().Count,
    () => modules.Length);
```

**ObservableGauges**: Atualizados automaticamente quando Prometheus faz scraping do endpoint `/metrics`

---

## 📊 Métricas Expostas

### Endpoint Prometheus

**URL**: `/metrics`

**Métricas de Módulos**:
```
# HELP araponga_modules_enabled Number of enabled modules
# TYPE araponga_modules_enabled gauge
araponga_modules_enabled 12

# HELP araponga_modules_total Total number of modules
# TYPE araponga_modules_total gauge
araponga_modules_total 12

# HELP araponga_modules_registration_attempts Total number of module registration attempts
# TYPE araponga_modules_registration_attempts counter
araponga_modules_registration_attempts{module_id="Core"} 1
araponga_modules_registration_attempts{module_id="Feed"} 1
...

# HELP araponga_modules_registration_failures Total number of module registration failures
# TYPE araponga_modules_registration_failures counter
araponga_modules_registration_failures{module_id="..."} 0
```

---

## 🔍 Uso

### Health Check

**Verificar status dos módulos**:
```bash
curl http://localhost:5000/health/ready
```

**Filtrar apenas health check de módulos**:
```bash
curl http://localhost:5000/health/ready?tags=modules
```

### Métricas

**Consultar métricas de módulos**:
```bash
curl http://localhost:5000/metrics | grep araponga_modules
```

**Exemplo de Query Prometheus**:
```promql
# Taxa de falhas no registro de módulos
rate(araponga_modules_registration_failures[5m])

# Percentual de módulos habilitados
(araponga_modules_enabled / araponga_modules_total) * 100
```

---

## 📈 Benefícios

1. **Observabilidade**: Visibilidade completa do status dos módulos
2. **Alertas**: Possibilidade de criar alertas baseados em health checks e métricas
3. **Debugging**: Identificar rapidamente qual módulo está causando problemas
4. **Monitoramento**: Acompanhar tendências de falhas e sucessos no registro de módulos

---

## 🚀 Próximos Passos (Opcional)

1. Adicionar health checks específicos por módulo (ex.: verificar se serviços do módulo estão disponíveis)
2. Adicionar métricas de performance por módulo (ex.: tempo de inicialização)
3. Criar dashboards Grafana para visualização das métricas
4. Configurar alertas baseados em health checks e métricas

---

## 📚 Referências

- **Health Check**: `backend/Araponga.Api/HealthChecks/ModuleHealthCheck.cs`
- **Métricas**: `backend/Araponga.Application/Metrics/ArapongaMetrics.cs`
- **ModuleRegistry**: `backend/Araponga.Application/Modules/ModuleRegistry.cs`
- **Configuração**: `backend/Araponga.Api/Program.cs`

---

**Status**: ✅ **IMPLEMENTAÇÃO COMPLETA**  
**Última Atualização**: 2026-01-26
