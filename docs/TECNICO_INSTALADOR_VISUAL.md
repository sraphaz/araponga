# Instalador Visual Araponga

**Versão**: 1.0  
**Data**: 2026-01-28  
**Status**: 📋 Planejamento Completo  
**Tipo**: Documentação Técnica - Fase de Instalação e Deploy

---

## 📋 Índice

1. [Visão Geral](#visão-geral)
2. [Objetivo e Escopo](#objetivo-e-escopo)
3. [Arquitetura do Instalador](#arquitetura-do-instalador)
4. [Fluxo de Instalação](#fluxo-de-instalação)
5. [Funcionalidades Principais](#funcionalidades-principais)
6. [Configurações Suportadas](#configurações-suportadas)
7. [Arquiteturas de Deployment](#arquiteturas-de-deployment)
8. [Módulos e Feature Flags](#módulos-e-feature-flags)
9. [Estrutura Técnica](#estrutura-técnica)
10. [APIs e Integrações](#apis-e-integrações)
11. [Segurança](#segurança)
12. [Troubleshooting](#troubleshooting)
13. [Roadmap de Implementação](#roadmap-de-implementação)

---

## 🎯 Visão Geral

O **Instalador Visual Araponga** é uma aplicação web moderna e amigável que permite a sysadmins fazer o deploy e configuração completa do Araponga através de uma interface gráfica intuitiva, sem necessidade de conhecimento técnico profundo em linha de comando.

### Características Principais

- ✅ **Interface Visual Moderna**: Wizard passo a passo com design amigável
- ✅ **Validação Automática**: Testes de conectividade e validação de configurações
- ✅ **Suporte Multi-Arquitetura**: Monolito e Multi-Cluster
- ✅ **Seleção de Módulos**: Escolha quais funcionalidades habilitar
- ✅ **Configuração Completa**: Banco, cache, storage, segurança, feature flags, SystemConfig
- ✅ **Deploy Automatizado**: Docker Compose e Kubernetes
- ✅ **Geração de Configurações**: Arquivos `.env` e manifestos gerados automaticamente

---

## 🎯 Objetivo e Escopo

### Objetivo

Facilitar o processo de instalação e configuração do Araponga para sysadmins, reduzindo:
- Tempo de setup de horas para minutos
- Erros de configuração manual
- Necessidade de conhecimento técnico profundo
- Complexidade de deployment multi-cluster

### Escopo

O instalador cobre:

1. **Verificação de Pré-requisitos**
   - Docker e Docker Compose
   - Portas disponíveis
   - Espaço em disco
   - Permissões

2. **Configuração de Infraestrutura**
   - Banco de dados (PostgreSQL)
   - Cache (Redis)
   - Storage (MinIO, S3, Azure Blob, Local)
   - Segurança (JWT, CORS, Rate Limiting)

3. **Seleção de Arquitetura**
   - Monolito (instalação simples)
   - Multi-Cluster (alta disponibilidade)

4. **Seleção e Configuração de Módulos**
   - Módulos core e opcionais
   - Feature flags padrão
   - SystemConfig inicial
   - Configurações específicas por módulo

5. **Deploy e Validação**
   - Inicialização de containers
   - Aplicação de migrations
   - Validação de serviços
   - Health checks

---

## 🏗️ Arquitetura do Instalador

### Tecnologia

**Stack Recomendado**: Next.js 15 + React + TypeScript + Tailwind CSS

**Justificativa**:
- Alinhado com frontend existente (wiki e portal)
- Pode ser executado standalone ou integrado
- Interface moderna e responsiva
- Fácil de manter e atualizar

### Estrutura de Diretórios

```
installer/
├── app/                          # Next.js App Router
│   ├── page.tsx                 # Wizard principal
│   ├── layout.tsx               # Layout base
│   ├── api/                     # API routes
│   │   └── installer/
│   │       ├── validate-docker/route.ts
│   │       ├── start-containers/route.ts
│   │       ├── test-db/route.ts
│   │       ├── apply-migrations/route.ts
│   │       ├── setup-system-config/route.ts
│   │       ├── setup-feature-flags/route.ts
│   │       ├── setup-load-balancer/route.ts
│   │       └── generate-env/route.ts
│   └── globals.css              # Estilos globais
├── components/
│   ├── wizard/                  # Componentes do wizard
│   │   ├── Wizard.tsx
│   │   ├── StepIndicator.tsx
│   │   └── NavigationButtons.tsx
│   ├── forms/                   # Formulários de configuração
│   │   ├── DatabaseConfigForm.tsx
│   │   ├── RedisConfigForm.tsx
│   │   ├── StorageConfigForm.tsx
│   │   ├── SecurityConfigForm.tsx
│   │   ├── ArchitectureSelectionForm.tsx
│   │   ├── LoadBalancerConfigForm.tsx
│   │   ├── ModulesSelectionForm.tsx
│   │   ├── FeatureFlagsConfigForm.tsx
│   │   ├── SystemConfigForm.tsx
│   │   ├── ModuleSpecificConfigForm.tsx
│   │   └── OptionalConfigForm.tsx
│   ├── validation/              # Componentes de validação
│   │   ├── ConnectionTest.tsx
│   │   └── ValidationMessage.tsx
│   └── installation/            # Telas de instalação
│       ├── ProgressView.tsx
│       ├── LogViewer.tsx
│       └── CompletionView.tsx
├── lib/
│   ├── config-validator.ts      # Validação de configs
│   ├── docker-client.ts         # Cliente Docker
│   ├── db-client.ts             # Cliente PostgreSQL
│   ├── redis-client.ts          # Cliente Redis
│   ├── storage-client.ts        # Cliente Storage
│   ├── env-generator.ts         # Geração de .env
│   ├── migration-runner.ts      # Executor de migrations
│   ├── modules-config.ts        # Configuração de módulos
│   ├── feature-flags-setup.ts   # Setup de feature flags
│   ├── system-config-setup.ts   # Setup de SystemConfig
│   ├── load-balancer-setup.ts   # Setup de load balancer
│   └── multi-cluster-setup.ts   # Setup de multi-cluster
├── types/
│   └── installer.ts             # Tipos TypeScript
└── package.json
```

---

## 🔄 Fluxo de Instalação

### Passo 1: Boas-vindas e Pré-requisitos

**Validações**:
- ✅ Docker instalado e rodando
- ✅ Docker Compose disponível
- ✅ Portas disponíveis (8080, 5432, 6379, 9000, 9001)
- ✅ Espaço em disco suficiente (mínimo 2GB)
- ✅ Permissões de escrita no diretório

### Passo 2: Configuração do Ambiente

**Opções**:
- **Desenvolvimento**: Configurações simplificadas, valores padrão seguros
- **Produção**: Configurações completas obrigatórias, validações rigorosas

### Passo 3: Seleção da Arquitetura

**Opções**:

#### Monolito
- Uma única instância da API
- Configuração simples
- Ideal para desenvolvimento, testes e instalações pequenas/médias

#### Multi-Cluster
- Múltiplas instâncias da API (réplicas)
- Load balancer para distribuição de carga
- Alta disponibilidade e escalabilidade horizontal
- Suporte a read replicas do banco

**Configurações Multi-Cluster**:
- Número de réplicas (mínimo 2, recomendado 3-5)
- Tipo de load balancer (Nginx, AWS ALB, Azure LB, Kubernetes)
- Estratégia de balanceamento (Round Robin, Least Connections, IP Hash)
- Health checks configuráveis
- Read replicas do banco (opcional)

### Passo 4: Configuração do Banco de Dados

**Opções**:
- Docker (recomendado) - Cria container automaticamente
- Existente - Conecta a banco existente

**Campos**:
- Host, Porta, Database, Username, Password
- Teste de conectividade
- Verificação de extensão PostGIS

### Passo 5: Configuração de Cache (Redis)

**Opções**:
- Docker (recomendado)
- Existente

**Nota**: Multi-cluster requer Redis obrigatório (não pode usar fallback IMemoryCache)

### Passo 6: Configuração de Storage

**Opções**:
1. **MinIO (Docker)** - Desenvolvimento
2. **MinIO Existente**
3. **AWS S3** - Produção
4. **Azure Blob Storage** - Produção
5. **Local** - Desenvolvimento (não disponível em multi-cluster)

### Passo 7: Configuração de Segurança

**Campos**:
- **JWT Signing Key** (OBRIGATÓRIO): Mínimo 32 caracteres, gerador automático
- **CORS Origins**: Lista de origens permitidas (não pode ser wildcard em produção)
- **Rate Limiting**: Permit Limit, Window Seconds, Queue Limit

### Passo 8: Seleção de Módulos

**Módulos Disponíveis**:

#### Core (Sempre Habilitados)
- Autenticação e Identidade
- Territórios
- Memberships

#### Conteúdo
- Feed Comunitário
- Eventos
- Mapa Territorial
- Alertas
- Assets

#### Comunicação e Economia
- Chat
- Marketplace
- Subscriptions

#### Governança
- Moderação
- Governança e Votação
- Notificações

#### Administrativos
- Admin e Configuração

### Passo 9: Configuração de Feature Flags

**Flags por Módulo**:
- Feed: AlertPosts, EventPosts
- Marketplace: MarketplaceEnabled
- Chat: ChatEnabled, ChatTerritoryPublicChannel, ChatTerritoryResidentsChannel, ChatGroups, ChatDmEnabled, ChatMediaEnabled
- Mídias: MediaImagesEnabled, MediaVideosEnabled, MediaAudioEnabled, ChatMediaImagesEnabled, ChatMediaAudioEnabled

### Passo 10: Configuração de SystemConfig

**Categorias**:
- Moderation (Moderação)
- Validation (Validação)
- Assets (Recursos)
- Providers (Provedores)
- Observability (Observabilidade)
- Security (Segurança)

### Passo 11: Configurações Específicas por Módulo

**Exemplos**:
- Chat: Max message length, Max participants per group, Message retention days
- Marketplace: Max items per store, Transaction fee percentage, Payout delay days
- Subscriptions: Free plan enabled, Trial period days, Currency
- Moderação: Auto-approve threshold, Moderation queue size
- Notificações: Push notifications enabled, Email notifications enabled

### Passo 12: Configurações Opcionais

- Email (SMTP)
- Logging (Seq)
- OpenTelemetry
- Portas e URLs

### Passo 13: Revisão e Confirmação

**Exibe**:
- Arquitetura selecionada
- Módulos selecionados
- Feature flags padrão
- SystemConfig inicial
- Configurações por módulo
- Configurações de load balancer (se multi-cluster)

**Ações**:
- Exportar/Importar configuração (JSON)
- Voltar para editar
- Instalar

### Passo 14: Instalação e Deploy

**Etapas**:
1. Gerando arquivo `.env`
2. Configurando load balancer (se multi-cluster)
3. Iniciando containers Docker
4. Iniciando réplicas da API (se multi-cluster)
5. Aguardando serviços ficarem prontos
6. Verificando health checks (se multi-cluster)
7. Aplicando migrations do banco
8. Criando SystemConfig inicial
9. Configurando feature flags padrão
10. Inicializando módulos selecionados
11. Verificando conectividade
12. Testando API
13. Testando load balancing (se multi-cluster)
14. Validando módulos habilitados

### Passo 15: Conclusão

**Informações**:
- Status de cada serviço
- Arquitetura instalada
- Número de réplicas ativas (se multi-cluster)
- Status do load balancer (se multi-cluster)
- Módulos habilitados
- Feature flags configuradas
- SystemConfig criado
- Links úteis (API, Swagger, Admin Panel, MinIO Console)

---

## ⚙️ Funcionalidades Principais

### Validação de Configurações

- JWT Secret: mínimo 32 caracteres, não pode ser valor padrão em produção
- CORS: não pode ser wildcard em produção, pelo menos uma origem
- Connection Strings: formato válido
- Portas: disponíveis e não em uso
- URLs: formato válido
- Senhas: força mínima (opcional)

### Geração de .env

- Gera arquivo `.env` baseado nas configurações
- Formato compatível com docker-compose
- Comentários explicativos
- Valores mascarados para senhas (não exibidos em logs)

### Cliente Docker/Docker Compose

- Verificar se Docker está rodando
- Executar `docker-compose up -d`
- Verificar status de containers
- Ler logs de containers
- Parar/Iniciar containers

### Cliente de Banco de Dados

- Testar conexão PostgreSQL
- Verificar extensão PostGIS
- Aplicar migrations via Entity Framework
- Verificar se banco está vazio ou tem dados

### Setup de Load Balancer

- Gerar configuração de Nginx baseada em número de réplicas
- Gerar manifestos Kubernetes (Deployment, Service) se necessário
- Validar configuração de load balancer
- Testar conectividade com todas as réplicas
- Configurar health checks automáticos

### Setup de Multi-Cluster

- Gerar docker-compose para múltiplas réplicas
- Gerar manifestos Kubernetes para deployment escalável
- Validar requisitos de multi-cluster (Redis obrigatório, storage compartilhado)
- Iniciar múltiplas instâncias da API
- Verificar health checks de todas as instâncias
- Testar distribuição de carga
- Configurar read replicas do banco (se habilitado)

---

## 🔧 Configurações Suportadas

### Banco de Dados

- **PostgreSQL 16** com PostGIS
- Docker ou instalação existente
- Read replicas (opcional, multi-cluster)

### Cache

- **Redis 7**
- Docker ou instalação existente
- Obrigatório em multi-cluster

### Storage

- **MinIO** (S3-compatible) - Desenvolvimento
- **AWS S3** - Produção
- **Azure Blob Storage** - Produção
- **Local** - Desenvolvimento (não disponível em multi-cluster)

### Segurança

- **JWT**: Secret de 32+ caracteres, gerador automático
- **CORS**: Lista de origens (sem wildcard em produção)
- **Rate Limiting**: Configurável por tipo de endpoint

### Load Balancers

- **Nginx** (Docker Compose)
- **AWS Application Load Balancer**
- **Azure Load Balancer**
- **Kubernetes Service**

---

## 🏛️ Arquiteturas de Deployment

### Monolito

**Características**:
- Uma única instância da API
- Configuração simples
- Ideal para desenvolvimento, testes e instalações pequenas/médias

**Requisitos**:
- 1 servidor/container para API
- PostgreSQL (pode ser no mesmo servidor)
- Redis (opcional, fallback para IMemoryCache)
- Storage (Local, S3 ou Azure Blob)

### Multi-Cluster

**Características**:
- Múltiplas instâncias da API (réplicas)
- Load balancer para distribuição de carga
- Alta disponibilidade e escalabilidade horizontal
- Tolerância a falhas

**Requisitos**:
- Múltiplos servidores/containers para API (mínimo 2, recomendado 3+)
- Load balancer (Nginx, AWS ALB, Azure LB, Kubernetes Service)
- PostgreSQL com suporte a read replicas (opcional mas recomendado)
- Redis obrigatório (cache distribuído)
- Storage compartilhado (S3, Azure Blob - não pode ser Local)

**Validações**:
- Multi-cluster requer Redis obrigatório
- Multi-cluster não pode usar storage Local
- Número mínimo de réplicas: 2
- Load balancer deve ser configurado

---

## 🧩 Módulos e Feature Flags

### Módulos Disponíveis

#### Core (Sempre Habilitados)
- **Autenticação e Identidade**: Gerenciamento de usuários e autenticação
- **Territórios**: Gerenciamento de territórios geográficos
- **Memberships**: Vínculos entre usuários e territórios

#### Conteúdo
- **Feed Comunitário**: Publicações e timeline territorial
- **Eventos**: Organização de eventos comunitários
- **Mapa Territorial**: Visualização geográfica de conteúdos
- **Alertas**: Alertas de saúde pública e comunicação emergencial
- **Assets**: Recursos compartilhados do território

#### Comunicação e Economia
- **Chat**: Comunicação territorial (canais, grupos, DM)
- **Marketplace**: Sistema de trocas locais
- **Subscriptions**: Sistema de assinaturas recorrentes

#### Governança
- **Moderação**: Sistema de moderação comunitária
- **Governança e Votação**: Decisões coletivas e votação
- **Notificações**: Sistema de notificações in-app

#### Administrativos
- **Admin e Configuração**: Painel administrativo e SystemConfig

### Feature Flags Padrão

O instalador permite configurar feature flags padrão que serão aplicadas a novos territórios:

- **AlertPosts**: Permitir posts do tipo ALERT no feed
- **EventPosts**: Permitir posts de eventos no feed
- **MarketplaceEnabled**: Habilitar marketplace no território
- **ChatEnabled**: Master switch do chat
- **ChatTerritoryPublicChannel**: Canal público do território
- **ChatTerritoryResidentsChannel**: Canal exclusivo para moradores
- **ChatGroups**: Permitir criação de grupos no chat
- **ChatDmEnabled**: Habilitar mensagens diretas (DM)
- **ChatMediaEnabled**: Habilitar mídias no chat
- **MediaImagesEnabled**: Habilitar imagens em posts/eventos/marketplace
- **MediaVideosEnabled**: Habilitar vídeos em posts/eventos/marketplace
- **MediaAudioEnabled**: Habilitar áudios em posts/eventos/marketplace
- **ChatMediaImagesEnabled**: Habilitar imagens no chat
- **ChatMediaAudioEnabled**: Habilitar áudios no chat

---

## 🛠️ Estrutura Técnica

### Componentes Principais

- **Wizard**: Container principal com navegação
- **StepIndicator**: Indicador de progresso (1/15, 2/15, etc.)
- **ConfigForm**: Formulário de configuração com validação
- **ConnectionTest**: Botão de teste de conexão
- **PasswordGenerator**: Gerador de senhas seguras
- **ProgressBar**: Barra de progresso da instalação
- **LogViewer**: Visualizador de logs em tempo real

### Design

- Seguir identidade visual do Araponga (cores Forest, glass morphism)
- Componentes reutilizáveis do frontend existente
- Mobile-first e responsivo
- Feedback visual claro (sucesso, aviso, erro)
- Animações suaves para transições

---

## 🔌 APIs e Integrações

### API Routes (Next.js)

- `POST /api/installer/validate-docker` - Valida Docker
- `POST /api/installer/start-containers` - Inicia containers
- `POST /api/installer/check-status` - Verifica status
- `POST /api/installer/test-db` - Testa conexão PostgreSQL
- `POST /api/installer/apply-migrations` - Aplica migrations
- `POST /api/installer/setup-system-config` - Cria SystemConfig inicial
- `POST /api/installer/setup-feature-flags` - Configura feature flags padrão
- `POST /api/installer/validate-modules` - Valida módulos selecionados
- `POST /api/installer/setup-load-balancer` - Configura load balancer (multi-cluster)
- `POST /api/installer/start-replicas` - Inicia réplicas da API (multi-cluster)
- `POST /api/installer/validate-cluster` - Valida configuração de cluster
- `POST /api/installer/generate-env` - Gera arquivo .env

---

## 🔒 Segurança

### Tratamento de Secrets

- **Nunca** exibir senhas em logs ou na interface
- **Sempre** mascarar senhas em exibição (••••••••)
- **Gerar** secrets fortes automaticamente quando possível
- **Validar** força de senhas antes de aceitar
- **Armazenar** `.env` com permissões restritas (600)

### Validações de Segurança

- JWT Secret: mínimo 32 caracteres, não pode ser padrão
- CORS: não pode ser wildcard em produção
- Senhas de banco: mínimo 8 caracteres (recomendado)
- URLs: validação de formato e protocolo

---

## 🔍 Troubleshooting

### Problemas Comuns

#### Docker não está rodando
**Solução**: Iniciar Docker Desktop ou serviço Docker

#### Porta já em uso
**Solução**: Alterar porta na configuração ou parar serviço que está usando a porta

#### Conexão com banco falha
**Solução**: Verificar credenciais, host, porta e firewall

#### Redis não disponível em multi-cluster
**Solução**: Redis é obrigatório em multi-cluster, configurar antes de continuar

#### Storage Local não disponível em multi-cluster
**Solução**: Usar S3 ou Azure Blob Storage em multi-cluster

#### Migrations falham
**Solução**: Verificar logs, garantir que banco está acessível e tem permissões

---

## 📅 Roadmap de Implementação

### Fases

1. **Fase 1**: Estrutura base e componentes UI (3 dias)
2. **Fase 2**: Formulários de configuração básicos (5 dias)
3. **Fase 3**: Seleção de arquitetura e load balancer (4 dias)
4. **Fase 4**: Seleção e configuração de módulos (4 dias)
5. **Fase 5**: Configuração de feature flags e SystemConfig (4 dias)
6. **Fase 6**: Configurações específicas por módulo (3 dias)
7. **Fase 7**: Validações e testes de conexão (3 dias)
8. **Fase 8**: Integração Docker e geração de .env (4 dias)
9. **Fase 9**: Setup de multi-cluster e load balancer (3 dias)
10. **Fase 10**: Aplicação de migrations, SystemConfig e feature flags (3 dias)
11. **Fase 11**: Testes e refinamento (3 dias)
12. **Fase 12**: Documentação e guia do usuário (2 dias)

**Total estimado**: ~41 dias

---

## 📚 Referências

- [Plano Completo do Instalador](../.cursor/plans/instalador_visual_araponga_4ad83ba7.plan.md)
- [docker-compose.dev.yml](../docker-compose.dev.yml) - Configuração Docker existente
- [.env.example](../.env.example) - Exemplo de variáveis de ambiente
- [docs/SECURITY_CONFIGURATION.md](./SECURITY_CONFIGURATION.md) - Configurações de segurança
- [docs/SETUP.md](./SETUP.md) - Guia de setup atual
- [docs/DEPLOYMENT_MULTI_INSTANCE.md](./DEPLOYMENT_MULTI_INSTANCE.md) - Deployment multi-instância

---

**Última atualização**: 2026-01-28  
**Versão**: 1.0  
**Status**: 📋 Planejamento Completo
