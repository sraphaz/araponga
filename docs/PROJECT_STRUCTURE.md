# 📁 Estrutura do Projeto Arah

**Guia Visual e Explicativo da Organização do Código**

**Versão**: 1.0  
**Data**: 2025-01-20

---

## 🎯 Propósito

Este documento ajuda você a **navegar o código** do Arah, entendendo onde cada coisa está e por quê.

**Não precisa decorar** - use como referência quando precisar!

---

## 📂 Estrutura de Diretórios

```
Arah/
├── backend/              # Código do backend (.NET)
│   ├── Arah.Api      # Interface HTTP (controllers, endpoints)
│   ├── Arah.Application  # Lógica de negócio (services)
│   ├── Arah.Domain   # Conceitos centrais (entidades, value objects)
│   ├── Arah.Infrastructure  # Persistência (banco, armazenamento)
│   ├── Arah.Shared   # Tipos e utilitários compartilhados
│   └── Arah.Tests    # Testes automatizados
│
├── frontend/             # Frontend (Flutter - planejado)
│   └── portal/           # Portal React (site institucional)
│
├── docs/                 # Toda a documentação
│   ├── backlog-api/      # Documentação das 29 fases
│   ├── prs/              # Documentação de Pull Requests
│   └── ...               # Outros documentos
│
├── scripts/              # Scripts úteis
│   ├── generate-mermaid-diagrams.js
│   └── fix-diagram-colors.js
│
├── .cursorrules          # Regras que o Cursor lê automaticamente
├── .github/              # Configurações do GitHub
│   ├── workflows/        # CI/CD
│   └── ISSUE_TEMPLATE/   # Templates de Issues
│
├── docker-compose.yml    # Configuração Docker (Postgres)
├── Dockerfile            # Imagem Docker da API
└── README.md             # Visão geral do projeto
```

---

## 🏗️ Backend - Clean Architecture

O backend segue **Clean Architecture**, organizado em camadas:

### 📍 Arah.Domain

**O que é**: Conceitos centrais do domínio (territory, posts, events, etc.)

**O que contém**:
- **Entidades** (`Entities/`): `Territory`, `Post`, `Event`, `Membership`, etc.
- **Value Objects** (`ValueObjects/`): `GeoAnchor`, `Address`, etc.
- **Interfaces de Repositórios** (`Interfaces/`): `ITerritoryRepository`, `IFeedRepository`, etc.
- **Exceções de Domínio** (`Exceptions/`): `TerritoryNotFoundException`, etc.

**Características**:
- ✅ **Não depende de nada** - camada mais interna
- ✅ **Não tem lógica de persistência** - apenas conceitos
- ✅ **Puro C#** - sem frameworks externos

**Exemplo de navegação**:
```
Arah.Domain/
├── Entities/
│   ├── Territory.cs          # Entidade Territory
│   ├── Post.cs               # Entidade Post
│   └── Membership.cs         # Entidade Membership
├── ValueObjects/
│   ├── GeoAnchor.cs          # Ponto georreferenciado
│   └── Address.cs            # Endereço
└── Interfaces/
    └── Repositories/         # Interfaces de repositórios
```

---

### 📍 Arah.Application

**O que é**: Lógica de negócio e casos de uso

**O que contém**:
- **Services** (`Services/`): `TerritoryService`, `FeedService`, `EventsService`, etc.
- **DTOs** (`DTOs/`): `CreatePostRequest`, `PostResponse`, etc.
- **Interfaces de Serviços Externos** (`Interfaces/`): `IMediaStorageService`, etc.
- **Result Pattern** (`Results/`): `Result<T>` para operações que podem falhar

**Características**:
- ✅ **Depende apenas de Domain** - não conhece HTTP ou banco
- ✅ **Contém regras de negócio** - validações, lógica
- ✅ **Orquestra operações** - coordena repositórios e serviços

**Exemplo de navegação**:
```
Arah.Application/
├── Services/
│   ├── TerritoryService.cs   # Lógica de territórios
│   ├── FeedService.cs        # Lógica de feed
│   └── EventsService.cs      # Lógica de eventos
├── DTOs/
│   ├── CreatePostRequest.cs
│   └── PostResponse.cs
└── Interfaces/
    └── Media/                # Interfaces de serviços externos
```

---

### 📍 Arah.Infrastructure

**O que é**: Persistência e integrações externas

**O que contém**:
- **Repositórios** (`Repositories/`): Implementações de `ITerritoryRepository`, etc.
- **DbContext** (`Postgres/` ou `InMemory/`): Acesso ao banco de dados
- **Serviços Externos** (`Services/`): `S3MediaStorageService`, etc.
- **Configurações** (`Configurations/`): `ServiceCollectionExtensions`, etc.

**Características**:
- ✅ **Depende de Domain e Application** - implementa interfaces
- ✅ **Lógica técnica** - banco, APIs externas, storage
- ✅ **Não tem lógica de negócio** - apenas implementação

**Exemplo de navegação**:
```
Arah.Infrastructure/
├── Repositories/
│   ├── PostgresTerritoryRepository.cs
│   └── InMemoryTerritoryRepository.cs
├── Postgres/
│   └── ArapongaDbContext.cs
└── InMemory/
    └── InMemoryDataStore.cs
```

---

### 📍 Arah.Api

**O que é**: Interface HTTP (endpoints, controllers)

**O que contém**:
- **Controllers** (`Controllers/`): `TerritoriesController`, `FeedController`, etc.
- **Middlewares** (`Middlewares/`): `AuthenticationMiddleware`, `ErrorHandlingMiddleware`
- **Configuração** (`Program.cs`, `Startup.cs`): Setup da API
- **DevPortal** (`wwwroot/devportal/`): Site do desenvolvedor

**Características**:
- ✅ **Depende de todas as camadas** - orquestra tudo
- ✅ **Lógica HTTP** - routing, autenticação, validação de entrada
- ✅ **Não tem lógica de negócio** - delega para Application

**Exemplo de navegação**:
```
Arah.Api/
├── Controllers/
│   ├── TerritoriesController.cs
│   ├── FeedController.cs
│   └── EventsController.cs
├── Middlewares/
│   └── AuthenticationMiddleware.cs
├── wwwroot/
│   └── devportal/          # Portal do desenvolvedor
└── Program.cs              # Configuração principal
```

---

### 📍 Arah.Tests

**O que é**: Testes automatizados

**O que contém**:
- **Domain Tests** (`Domain/`): Testes de entidades e value objects
- **Application Tests** (`Application/`): Testes de services
- **Infrastructure Tests** (`Infrastructure/`): Testes de repositórios
- **API Tests** (`Api/`): Testes end-to-end

**Características**:
- ✅ **Cobertura >90%** - objetivo de alta cobertura
- ✅ **Isolados** - usam `InMemoryDataStore` quando possível
- ✅ **E2E** - testes de integração completos

**Exemplo de navegação**:
```
Arah.Tests/
├── Domain/
│   └── TerritoryTests.cs
├── Application/
│   └── TerritoryServiceTests.cs
└── Api/
    └── TerritoriesControllerTests.cs
```

---

## 🔍 Como Navegar o Código

### Exemplo: Quero entender como criar um Post

**Passo 1**: Procure pelo endpoint
- Vá para `Arah.Api/Controllers/FeedController.cs`
- Encontre `POST /api/v1/feed` ou método `CreatePost`

**Passo 2**: Entenda o controller
- Controller recebe `CreatePostRequest` (DTO)
- Chama `FeedService.CreatePostAsync()`

**Passo 3**: Entenda o service
- Vá para `Arah.Application/Services/FeedService.cs`
- Encontre método `CreatePostAsync()`
- Veja regras de negócio e validações

**Passo 4**: Entenda a entidade
- Vá para `Arah.Domain/Entities/Post.cs`
- Veja estrutura e validações da entidade

**Passo 5**: Entenda persistência
- Vá para `Arah.Infrastructure/Repositories/PostgresFeedRepository.cs`
- Veja como é salvo no banco

**Dica**: Use o Cursor! Pergunte: "Como funciona a criação de posts?" e ele te mostra o fluxo completo.

---

### Exemplo: Quero adicionar um novo endpoint

**Passo 1**: Defina a entidade (se nova)
- Crie em `Arah.Domain/Entities/`

**Passo 2**: Defina interface do repositório
- Crie em `Arah.Domain/Interfaces/Repositories/`

**Passo 3**: Implemente repositório
- Crie em `Arah.Infrastructure/Repositories/`

**Passo 4**: Crie service
- Crie em `Arah.Application/Services/`

**Passo 5**: Crie controller
- Crie em `Arah.Api/Controllers/`

**Passo 6**: Adicione testes
- Crie em `Arah.Tests/`

**Dica**: Use o Cursor! Ele sabe seguir essa estrutura automaticamente.

---

## 📚 Documentação Relacionada

- [`docs/12_DOMAIN_MODEL.md`](./12_DOMAIN_MODEL.md) - Modelo de domínio detalhado
- [`docs/10_ARCHITECTURE_DECISIONS.md`](./10_ARCHITECTURE_DECISIONS.md) - Decisões arquiteturais
- [`docs/11_ARCHITECTURE_SERVICES.md`](./11_ARCHITECTURE_SERVICES.md) - Documentação de services
- [`docs/ONBOARDING_DEVELOPERS.md`](./ONBOARDING_DEVELOPERS.md) - Guia para desenvolvedores

---

## 🎯 Resumo Visual

```
Usuário faz requisição HTTP
  ↓
Arah.Api (Controller recebe)
  ↓
Arah.Application (Service processa regra de negócio)
  ↓
Arah.Domain (Entidade/Value Object)
  ↓
Arah.Application (Service orquestra)
  ↓
Arah.Infrastructure (Repository persiste)
  ↓
Banco de Dados (Postgres ou InMemory)
```

**Fluxo de dados**: HTTP → API → Application → Domain → Infrastructure → DB  
**Fluxo inverso**: DB → Infrastructure → Application → API → HTTP

---

## 💡 Dicas de Navegação

### 1. Use o Cursor

**Pergunte ao Cursor**:
- "Onde está a lógica de criação de territórios?"
- "Como funciona o sistema de feed?"
- "Onde está a validação de GeoAnchor?"

**O Cursor entende o contexto** e te mostra o código certo.

---

### 2. Busque por Nomes

**Use busca no editor** (Ctrl+F / Cmd+F):
- Busque por nome da entidade: "Territory"
- Busque por nome do service: "TerritoryService"
- Busque por nome do controller: "TerritoriesController"

---

### 3. Siga Interfaces

**Interfaces estão em Domain ou Application**:
- Se você encontrar `ITerritoryRepository`, a implementação está em `Infrastructure`
- Se você encontrar `IMediaStorageService`, a implementação está em `Infrastructure`

---

### 4. Use o Índice de Documentação

**Veja [`docs/00_INDEX.md`](./00_INDEX.md)**:
- Lista todos os documentos
- Organizados por categoria
- Links para documentação técnica

---

## 🌱 Conclusão

A estrutura do Arah é **clara e organizada** seguindo Clean Architecture.

**Lembre-se**:
- ✅ **Domain** não depende de nada
- ✅ **Application** depende de Domain
- ✅ **Infrastructure** depende de Domain e Application
- ✅ **Api** depende de todas

**Use o Cursor** para navegar - ele entende a estrutura e te ajuda!

---

**Última Atualização**: 2025-01-20  
**Versão**: 1.0

**Dúvidas sobre estrutura?** Pergunte no Discord ou use o Cursor para explorar!
