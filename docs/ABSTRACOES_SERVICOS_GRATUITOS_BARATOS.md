# Abstrações para Serviços de Infraestrutura Gratuitos/Baratos

**Data**: 2026-01-27  
**Status**: 📋 Análise e Proposta  
**Objetivo**: Investigar e implementar abstrações que permitam usar serviços de infraestrutura de zero custo ou menor custo possível

---

## 🎯 Objetivo

Criar abstrações adequadas que permitam:
1. **Trocar provedores** sem alterar código de aplicação
2. **Usar serviços gratuitos** quando disponíveis
3. **Escalar para serviços pagos** quando necessário
4. **Reduzir custos** de infraestrutura ao mínimo

---

## 📊 Situação Atual

### ✅ Abstrações Já Implementadas

#### 1. Cache (`IDistributedCacheService`)
- ✅ **Interface**: `IDistributedCacheService`
- ✅ **Implementações**:
  - `RedisCacheService` (com fallback para `IMemoryCache`)
  - Fallback automático se Redis falhar
- ✅ **Status**: **Bom** - já suporta serviço gratuito (IMemoryCache)

#### 2. Storage (`IFileStorage`)
- ✅ **Interface**: `IFileStorage`
- ✅ **Implementações**:
  - `LocalFileStorage` (gratuito)
  - `S3FileStorage` (pago)
- ✅ **Status**: **Bom** - já suporta serviço gratuito (Local)

#### 3. Email (`IEmailSender`)
- ✅ **Interface**: `IEmailSender`
- ✅ **Implementações**:
  - `SmtpEmailSender` (gratuito com Gmail/Outlook)
- ✅ **Status**: **Bom** - já suporta serviço gratuito (SMTP)

#### 4. Event Bus (`IEventBus`)
- ✅ **Interface**: `IEventBus`
- ✅ **Implementações**:
  - `InMemoryEventBus` (gratuito)
- ✅ **Status**: **Bom** - já usa serviço gratuito

#### 5. Database
- ✅ **Suporte**: PostgreSQL e InMemory
- ⚠️ **Falta**: SQLite (gratuito, útil para desenvolvimento/testes)
- ✅ **Status**: **Razoável** - PostgreSQL local é gratuito

---

## 💰 Serviços Gratuitos/Baratos Disponíveis

### 1. Cache

| Provedor | Free Tier | Limites | Custo Pós-Free |
|----------|-----------|--------|----------------|
| **IMemoryCache** | ✅ Ilimitado | Apenas memória local | $0 |
| **Redis Cloud** | ✅ 30MB | 30MB, 30 conexões | $0.10/GB |
| **Azure Cache Redis** | ❌ Não há | - | $0.018/GB/hora |
| **AWS ElastiCache** | ❌ Não há | - | $0.017/hora |

**Recomendação**: 
- **Desenvolvimento/Testes**: IMemoryCache (gratuito)
- **Produção Pequena**: Redis Cloud free tier (30MB)
- **Produção Média**: Redis Cloud pago ($0.10/GB)

---

### 2. Storage (File Storage)

| Provedor | Free Tier | Limites | Custo Pós-Free |
|----------|-----------|---------|----------------|
| **LocalFileStorage** | ✅ Ilimitado | Apenas disco local | $0 |
| **Azure Blob Storage** | ✅ 5GB | 5GB storage, 20K operações | $0.0184/GB |
| **AWS S3** | ✅ 5GB | 5GB por 12 meses | $0.023/GB |
| **Google Cloud Storage** | ✅ 5GB | 5GB, 5K operações | $0.020/GB |
| **Backblaze B2** | ✅ 10GB | 10GB storage | $0.005/GB |

**Recomendação**:
- **Desenvolvimento/Testes**: LocalFileStorage (gratuito)
- **Produção Pequena**: Azure Blob Storage free tier (5GB)
- **Produção Média**: Backblaze B2 (mais barato: $0.005/GB)

---

### 3. Email

| Provedor | Free Tier | Limites | Custo Pós-Free |
|----------|-----------|---------|----------------|
| **SMTP (Gmail/Outlook)** | ✅ Ilimitado | 500 emails/dia (Gmail) | $0 |
| **SendGrid** | ✅ 100/dia | 100 emails/dia | $19.95/mês (40K) |
| **Mailgun** | ✅ 5.000/mês | 5.000 emails/mês | $35/mês (50K) |
| **AWS SES** | ✅ 62.000/mês | 62.000 emails/mês | $0.10/1.000 |
| **Resend** | ✅ 3.000/mês | 3.000 emails/mês | $20/mês (50K) |

**Recomendação**:
- **Desenvolvimento/Testes**: SMTP local ou Gmail (gratuito)
- **Produção Pequena**: AWS SES free tier (62.000/mês)
- **Produção Média**: AWS SES pago ($0.10/1.000)

---

### 4. Database

| Provedor | Free Tier | Limites | Custo Pós-Free |
|----------|-----------|---------|----------------|
| **SQLite** | ✅ Ilimitado | Apenas arquivo local | $0 |
| **PostgreSQL Local** | ✅ Ilimitado | Apenas servidor local | $0 |
| **Supabase** | ✅ 500MB | 500MB, 2GB bandwidth | $25/mês (8GB) |
| **Neon** | ✅ 512MB | 512MB, serverless | $19/mês (10GB) |
| **Railway** | ✅ $5 crédito | PostgreSQL gerenciado | $5/mês (1GB) |
| **Render** | ✅ Não há | - | $7/mês (1GB) |
| **Azure Database** | ❌ Não há | - | $0.02/hora |
| **AWS RDS** | ❌ Não há | - | $0.017/hora |

**Recomendação**:
- **Desenvolvimento/Testes**: SQLite (gratuito, rápido)
- **Produção Pequena**: Supabase free tier (500MB)
- **Produção Média**: Neon ($19/mês para 10GB)

---

### 5. Event Bus / Message Queue

| Provedor | Free Tier | Limites | Custo Pós-Free |
|----------|-----------|---------|----------------|
| **InMemory** | ✅ Ilimitado | Apenas memória local | $0 |
| **RabbitMQ Local** | ✅ Ilimitado | Apenas servidor local | $0 |
| **Azure Service Bus** | ❌ Não há | - | $0.05/milhão |
| **AWS SQS** | ✅ 1 milhão/mês | 1 milhão requests/mês | $0.40/milhão |
| **Google Cloud Pub/Sub** | ✅ 10GB/mês | 10GB mensagens/mês | $0.40/milhão |

**Recomendação**:
- **Desenvolvimento/Testes**: InMemory (gratuito)
- **Produção Pequena**: AWS SQS free tier (1 milhão/mês)
- **Produção Média**: AWS SQS pago ($0.40/milhão)

---

## 🔧 Nível de Abstração Necessário

### Análise Atual

**✅ Bom Nível de Abstração**:
- Cache: `IDistributedCacheService` ✅
- Storage: `IFileStorage` ✅
- Email: `IEmailSender` ✅
- Event Bus: `IEventBus` ✅

**⚠️ Melhorias Necessárias**:
- Database: Falta abstração para múltiplos provedores
- Factory Pattern: Facilitar troca de provedores via configuração

---

## 📋 Implementações Sugeridas

### 1. Adicionar Suporte a SQLite

**Objetivo**: Permitir desenvolvimento/testes sem PostgreSQL

**Implementação**:

```csharp
// Araponga.Infrastructure.Shared/Postgres/SqliteDbContext.cs
public sealed class SqliteDbContext : DbContext, IUnitOfWork
{
    public SqliteDbContext(DbContextOptions<SqliteDbContext> options)
        : base(options)
    {
    }

    // Mesmas entidades do SharedDbContext
    public DbSet<TerritoryRecord> Territories => Set<TerritoryRecord>();
    // ... outras entidades

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurações similares ao SharedDbContext
        // Ajustar tipos específicos do SQLite se necessário
        base.OnModelCreating(modelBuilder);
    }

    // Implementar IUnitOfWork (mesmo padrão)
}
```

**Registro**:
```csharp
// ServiceCollectionExtensions.cs
var persistenceProvider = configuration.GetValue<string>("Persistence:Provider") ?? "InMemory";

if (string.Equals(persistenceProvider, "SQLite", StringComparison.OrdinalIgnoreCase))
{
    var dbPath = configuration.GetValue<string>("Persistence:Sqlite:DatabasePath") 
        ?? "araponga.db";
    
    services.AddDbContext<SqliteDbContext>(options =>
        options.UseSqlite($"Data Source={dbPath}"));
    
    services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<SqliteDbContext>());
    services.AddSqliteRepositories();
}
```

---

### 2. Adicionar Suporte a Azure Blob Storage

**Objetivo**: Usar free tier do Azure (5GB)

**Implementação**:

```csharp
// Araponga.Infrastructure.Shared/Services/AzureBlobStorage.cs
using Azure.Storage.Blobs;
using Araponga.Application.Interfaces;
using Araponga.Domain.Evidence;

namespace Araponga.Infrastructure.Shared.Services;

public sealed class AzureBlobStorage : IFileStorage
{
    private readonly BlobContainerClient _containerClient;

    public AzureBlobStorage(AzureBlobStorageOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            throw new ArgumentException("Azure Blob Storage connection string is required.");
        }

        if (string.IsNullOrWhiteSpace(options.ContainerName))
        {
            throw new ArgumentException("Azure Blob Storage container name is required.");
        }

        var blobServiceClient = new BlobServiceClient(options.ConnectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient(options.ContainerName);
        _containerClient.CreateIfNotExists();
    }

    public StorageProvider Provider => StorageProvider.AzureBlob;

    public async Task<string> SaveAsync(
        Stream content,
        string fileName,
        string contentType,
        CancellationToken cancellationToken)
    {
        var safeName = string.IsNullOrWhiteSpace(fileName) ? "upload.bin" : Path.GetFileName(fileName);
        var blobName = $"{DateTime.UtcNow:yyyyMMdd}/{Guid.NewGuid():N}-{safeName}";

        var blobClient = _containerClient.GetBlobClient(blobName);
        
        var blobHttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
        {
            ContentType = contentType
        };

        await blobClient.UploadAsync(content, new Azure.Storage.Blobs.Models.BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders
        }, cancellationToken);

        return blobName;
    }

    public async Task<Stream> OpenReadAsync(string storageKey, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(storageKey))
        {
            throw new ArgumentException("storageKey is required.", nameof(storageKey));
        }

        var blobClient = _containerClient.GetBlobClient(storageKey);
        var response = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
        
        return response.Value.Content;
    }
}

public class AzureBlobStorageOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = "araponga-uploads";
}
```

**Atualizar Enum**:
```csharp
// Araponga.Domain/Evidence/StorageProvider.cs
public enum StorageProvider
{
    Local = 1,
    S3 = 2,
    AzureBlob = 3
}
```

**Registro**:
```csharp
var storageProvider = configuration.GetValue<string>("Storage:Provider") ?? "Local";

if (string.Equals(storageProvider, "AzureBlob", StringComparison.OrdinalIgnoreCase))
{
    var options = configuration.GetSection("Storage:AzureBlob").Get<AzureBlobStorageOptions>() 
        ?? new AzureBlobStorageOptions();
    services.AddSingleton<IFileStorage>(_ => new AzureBlobStorage(options));
}
```

---

### 3. Adicionar Suporte a SendGrid/Mailgun

**Objetivo**: Usar free tiers de email (SendGrid: 100/dia, Mailgun: 5.000/mês)

**Implementação**:

```csharp
// Araponga.Infrastructure.Shared/Services/SendGridEmailSender.cs
using SendGrid;
using SendGrid.Helpers.Mail;
using Araponga.Application.Common;
using Araponga.Application.Interfaces;
using Araponga.Application.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Araponga.Infrastructure.Shared.Services;

public sealed class SendGridEmailSender : IEmailSender
{
    private readonly SendGridClient _client;
    private readonly string _fromEmail;
    private readonly string _fromName;
    private readonly ILogger<SendGridEmailSender> _logger;

    public SendGridEmailSender(
        IOptions<SendGridOptions> options,
        ILogger<SendGridEmailSender> logger)
    {
        if (string.IsNullOrWhiteSpace(options.Value.ApiKey))
        {
            throw new ArgumentException("SendGrid API key is required.");
        }

        _client = new SendGridClient(options.Value.ApiKey);
        _fromEmail = options.Value.FromEmail ?? "noreply@araponga.com";
        _fromName = options.Value.FromName ?? "Araponga";
        _logger = logger;
    }

    public async Task<OperationResult> SendEmailAsync(
        EmailMessage message,
        CancellationToken cancellationToken)
    {
        try
        {
            var msg = MailHelper.CreateSingleEmail(
                new EmailAddress(_fromEmail, _fromName),
                new EmailAddress(message.To),
                message.Subject,
                message.IsHtml ? null : message.Body,
                message.IsHtml ? message.Body : null);

            var response = await _client.SendEmailAsync(msg, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Email sent via SendGrid. Subject: {Subject}", message.Subject);
                return OperationResult.Success();
            }

            var body = await response.Body.ReadAsStringAsync(cancellationToken);
            _logger.LogError("SendGrid error: {StatusCode} - {Body}", response.StatusCode, body);
            return OperationResult.Failure($"SendGrid error: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email via SendGrid. Subject: {Subject}", message.Subject);
            return OperationResult.Failure($"Failed to send email: {ex.Message}");
        }
    }

    // Implementar outros métodos de IEmailSender...
}

public class SendGridOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string FromEmail { get; set; } = "noreply@araponga.com";
    public string FromName { get; set; } = "Araponga";
}
```

**Registro**:
```csharp
var emailProvider = configuration.GetValue<string>("Email:Provider") ?? "SMTP";

if (string.Equals(emailProvider, "SendGrid", StringComparison.OrdinalIgnoreCase))
{
    services.Configure<SendGridOptions>(configuration.GetSection("Email:SendGrid"));
    services.AddScoped<IEmailSender, SendGridEmailSender>();
}
else
{
    // SMTP padrão
    services.Configure<EmailConfiguration>(configuration.GetSection("Email:SMTP"));
    services.AddScoped<IEmailSender, SmtpEmailSender>();
}
```

---

### 4. Factory Pattern para Facilitar Troca

**Objetivo**: Centralizar lógica de seleção de provedores

**Implementação**:

```csharp
// Araponga.Infrastructure.Shared/Factories/InfrastructureFactory.cs
namespace Araponga.Infrastructure.Shared.Factories;

public static class InfrastructureFactory
{
    public static IFileStorage CreateFileStorage(IConfiguration configuration)
    {
        var provider = configuration.GetValue<string>("Storage:Provider") ?? "Local";

        return provider.ToUpperInvariant() switch
        {
            "LOCAL" => new LocalFileStorage(
                configuration.GetValue<string>("Storage:Local:Path") 
                ?? Path.Combine(AppContext.BaseDirectory, "app_data", "uploads")),
            
            "S3" => new S3FileStorage(
                configuration.GetSection("Storage:S3").Get<S3StorageOptions>() 
                ?? new S3StorageOptions()),
            
            "AZUREBLOB" => new AzureBlobStorage(
                configuration.GetSection("Storage:AzureBlob").Get<AzureBlobStorageOptions>() 
                ?? new AzureBlobStorageOptions()),
            
            _ => throw new InvalidOperationException($"Unknown storage provider: {provider}")
        };
    }

    public static IEmailSender CreateEmailSender(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        var provider = configuration.GetValue<string>("Email:Provider") ?? "SMTP";
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        return provider.ToUpperInvariant() switch
        {
            "SMTP" => new SmtpEmailSender(
                Options.Create(configuration.GetSection("Email:SMTP").Get<EmailConfiguration>() 
                    ?? new EmailConfiguration()),
                loggerFactory.CreateLogger<SmtpEmailSender>(),
                serviceProvider.GetService<IEmailTemplateService>()),
            
            "SENDGRID" => new SendGridEmailSender(
                Options.Create(configuration.GetSection("Email:SendGrid").Get<SendGridOptions>() 
                    ?? new SendGridOptions()),
                loggerFactory.CreateLogger<SendGridEmailSender>()),
            
            "MAILGUN" => new MailgunEmailSender(
                Options.Create(configuration.GetSection("Email:Mailgun").Get<MailgunOptions>() 
                    ?? new MailgunOptions()),
                loggerFactory.CreateLogger<MailgunEmailSender>()),
            
            _ => throw new InvalidOperationException($"Unknown email provider: {provider}")
        };
    }
}
```

---

## 📝 Exemplos de Configuração

### Configuração para Desenvolvimento (100% Gratuito)

```json
{
  "Persistence": {
    "Provider": "SQLite",
    "Sqlite": {
      "DatabasePath": "araponga-dev.db"
    }
  },
  "Storage": {
    "Provider": "Local",
    "Local": {
      "Path": "wwwroot/uploads"
    }
  },
  "Email": {
    "Provider": "SMTP",
    "SMTP": {
      "Host": "smtp.gmail.com",
      "Port": 587,
      "EnableSsl": true,
      "Username": "seu-email@gmail.com",
      "Password": "sua-senha-app",
      "FromAddress": "seu-email@gmail.com",
      "FromName": "Araponga Dev"
    }
  },
  "Cache": {
    "Provider": "Memory",
    "Redis": {
      "ConnectionString": ""
    }
  },
  "EventBus": {
    "Provider": "InMemory"
  }
}
```

### Configuração para Produção Pequena (Free Tiers)

```json
{
  "Persistence": {
    "Provider": "Postgres",
    "Postgres": {
      "ConnectionString": "Host=db.supabase.co;Database=araponga;Username=postgres;Password=..."
    }
  },
  "Storage": {
    "Provider": "AzureBlob",
    "AzureBlob": {
      "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=...",
      "ContainerName": "araponga-uploads"
    }
  },
  "Email": {
    "Provider": "SendGrid",
    "SendGrid": {
      "ApiKey": "SG...",
      "FromEmail": "noreply@araponga.com",
      "FromName": "Araponga"
    }
  },
  "Cache": {
    "Provider": "Redis",
    "Redis": {
      "ConnectionString": "redis-12345.c1.us-east-1-1.ec2.cloud.redislabs.com:12345"
    }
  },
  "EventBus": {
    "Provider": "InMemory"
  }
}
```

---

## 💰 Comparação de Custos

### Cenário: Aplicação Pequena (1.000 usuários ativos)

| Serviço | Gratuito | Free Tier | Custo Mensal Estimado |
|---------|----------|-----------|----------------------|
| **Database** | SQLite | Supabase (500MB) | $0 - $25 |
| **Storage** | Local | Azure Blob (5GB) | $0 - $0.10 |
| **Email** | SMTP Gmail | SendGrid (100/dia) | $0 - $20 |
| **Cache** | IMemoryCache | Redis Cloud (30MB) | $0 - $0.10 |
| **Event Bus** | InMemory | - | $0 |
| **Total** | | | **$0 - $45.20/mês** |

### Cenário: Aplicação Média (10.000 usuários ativos)

| Serviço | Provedor | Custo Mensal Estimado |
|---------|----------|----------------------|
| **Database** | Neon (10GB) | $19 |
| **Storage** | Backblaze B2 (100GB) | $0.50 |
| **Email** | AWS SES (100K/mês) | $10 |
| **Cache** | Redis Cloud (1GB) | $0.10 |
| **Event Bus** | AWS SQS (1M/mês) | $0 |
| **Total** | | **$29.60/mês** |

---

## 🚀 Plano de Implementação

### Fase 1: SQLite Support (1 semana)

**Objetivos**:
- ✅ Adicionar suporte a SQLite
- ✅ Criar `SqliteDbContext`
- ✅ Criar repositórios SQLite
- ✅ Atualizar configuração

**Benefícios**:
- Desenvolvimento sem PostgreSQL
- Testes mais rápidos
- Zero custo

---

### Fase 2: Azure Blob Storage (1 semana)

**Objetivos**:
- ✅ Adicionar `AzureBlobStorage`
- ✅ Atualizar enum `StorageProvider`
- ✅ Atualizar factory
- ✅ Documentar configuração

**Benefícios**:
- Free tier: 5GB
- Escalável
- Custo baixo: $0.0184/GB

---

### Fase 3: SendGrid/Mailgun (1 semana)

**Objetivos**:
- ✅ Adicionar `SendGridEmailSender`
- ✅ Adicionar `MailgunEmailSender`
- ✅ Atualizar factory
- ✅ Documentar configuração

**Benefícios**:
- SendGrid: 100 emails/dia grátis
- Mailgun: 5.000 emails/mês grátis
- Melhor deliverability

---

### Fase 4: Factory Pattern (1 semana)

**Objetivos**:
- ✅ Criar `InfrastructureFactory`
- ✅ Centralizar lógica de seleção
- ✅ Simplificar configuração
- ✅ Documentar padrões

**Benefícios**:
- Código mais limpo
- Fácil trocar provedores
- Configuração centralizada

---

## 📚 Guias de Configuração

### 1. Configurar SQLite

**Passos**:
1. Adicionar pacote: `Microsoft.EntityFrameworkCore.Sqlite`
2. Configurar `appsettings.json`:
```json
{
  "Persistence": {
    "Provider": "SQLite",
    "Sqlite": {
      "DatabasePath": "araponga.db"
    }
  }
}
```
3. Aplicar migrations:
```bash
dotnet ef database update --project backend/Araponga.Infrastructure.Shared
```

---

### 2. Configurar Azure Blob Storage (Free Tier)

**Passos**:
1. Criar conta Azure (gratuita)
2. Criar Storage Account
3. Obter connection string
4. Configurar `appsettings.json`:
```json
{
  "Storage": {
    "Provider": "AzureBlob",
    "AzureBlob": {
      "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=...",
      "ContainerName": "araponga-uploads"
    }
  }
}
```

**Limites Free Tier**:
- 5GB storage
- 20.000 operações/mês
- 5GB egress/mês

---

### 3. Configurar SendGrid (Free Tier)

**Passos**:
1. Criar conta SendGrid (gratuita)
2. Verificar domínio (opcional)
3. Gerar API Key
4. Configurar `appsettings.json`:
```json
{
  "Email": {
    "Provider": "SendGrid",
    "SendGrid": {
      "ApiKey": "SG.xxxxx",
      "FromEmail": "noreply@araponga.com",
      "FromName": "Araponga"
    }
  }
}
```

**Limites Free Tier**:
- 100 emails/dia
- Sem limite de tempo

---

### 4. Configurar Redis Cloud (Free Tier)

**Passos**:
1. Criar conta Redis Cloud (gratuita)
2. Criar database
3. Obter connection string
4. Configurar `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "Redis": "redis://default:password@host:port"
  }
}
```

**Limites Free Tier**:
- 30MB storage
- 30 conexões simultâneas

---

## ✅ Checklist de Implementação

### Abstrações
- [x] `IDistributedCacheService` - ✅ Implementado
- [x] `IFileStorage` - ✅ Implementado
- [x] `IEmailSender` - ✅ Implementado
- [x] `IEventBus` - ✅ Implementado
- [ ] `IDatabaseProvider` - ⏳ Sugerido

### Implementações Gratuitas
- [x] `IMemoryCache` - ✅ Implementado
- [x] `LocalFileStorage` - ✅ Implementado
- [x] `SmtpEmailSender` - ✅ Implementado
- [x] `InMemoryEventBus` - ✅ Implementado
- [ ] `SqliteDbContext` - ⏳ Sugerido

### Implementações Free Tier
- [ ] `AzureBlobStorage` - ⏳ Sugerido
- [ ] `SendGridEmailSender` - ⏳ Sugerido
- [ ] `MailgunEmailSender` - ⏳ Sugerido
- [ ] `RedisCloudCache` - ⏳ Já suportado via RedisCacheService

### Factory Pattern
- [ ] `InfrastructureFactory` - ⏳ Sugerido

---

## 🎯 Recomendações Finais

### Para Desenvolvimento/Testes
- ✅ **SQLite** para database (gratuito, rápido)
- ✅ **LocalFileStorage** para storage (gratuito)
- ✅ **SMTP Gmail** para email (gratuito, 500/dia)
- ✅ **IMemoryCache** para cache (gratuito)
- ✅ **InMemoryEventBus** para eventos (gratuito)

**Custo Total**: **$0/mês**

---

### Para Produção Pequena (Free Tiers)
- ✅ **Supabase** para database (500MB grátis)
- ✅ **Azure Blob Storage** para storage (5GB grátis)
- ✅ **SendGrid** para email (100/dia grátis)
- ✅ **Redis Cloud** para cache (30MB grátis)
- ✅ **InMemoryEventBus** para eventos (gratuito)

**Custo Total**: **$0/mês** (dentro dos limites free tier)

---

### Para Produção Média (Custo Baixo)
- ✅ **Neon** para database ($19/mês, 10GB)
- ✅ **Backblaze B2** para storage ($0.50/mês, 100GB)
- ✅ **AWS SES** para email ($10/mês, 100K emails)
- ✅ **Redis Cloud** para cache ($0.10/mês, 1GB)
- ✅ **AWS SQS** para eventos ($0/mês, 1M requests)

**Custo Total**: **~$30/mês**

---

## 📖 Referências

- [Azure Blob Storage Pricing](https://azure.microsoft.com/pricing/details/storage/blobs/)
- [AWS S3 Pricing](https://aws.amazon.com/s3/pricing/)
- [SendGrid Pricing](https://sendgrid.com/pricing/)
- [Supabase Pricing](https://supabase.com/pricing)
- [Neon Pricing](https://neon.tech/pricing)
- [Redis Cloud Pricing](https://redis.com/pricing/)

---

**Última Atualização**: 2026-01-27  
**Status**: 📋 Análise Completa - Pronto para Implementação
