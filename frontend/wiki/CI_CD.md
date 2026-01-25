# CI/CD - Wiki Araponga

Documentação completa do pipeline de CI/CD do Wiki Araponga.

## 🚀 Visão Geral

O Wiki Araponga possui um pipeline automatizado de CI/CD usando **GitHub Actions** que:

1. **Testa** o código em cada PR e push
2. **Constrói** o site estático
3. **Faz deploy** automaticamente para GitHub Pages em `wiki.araponga.app`

## 📋 Workflow

O workflow está definido em `.github/workflows/wiki-pages.yml` e é executado:

- ✅ Em **pull requests** para `main` (apenas CI)
- ✅ Em **push** para `main` (CI + CD)
- ✅ Manualmente via `workflow_dispatch`

## 🔧 Jobs do Pipeline

### 1. CI (Continuous Integration)

**Nome**: `ci`  
**Executa**: Sempre (PRs e pushes)

**Passos**:
- ✅ Checkout do código
- ✅ Setup Node.js 20.x com cache
- ✅ Instala dependências (`npm ci`)
- ✅ Lint (`npm run lint`)
- ✅ Type check (`npm run type-check`)
- ✅ Testes Jest (`npm test`)
- ✅ Build de validação (`npm run build`)
- ✅ Verificação de documentos markdown

**Objetivo**: Validar que o código compila e os testes passam antes de fazer deploy.

### 2. Build (Produção)

**Nome**: `build`  
**Executa**: Apenas em push para `main`

**Passos**:
- ✅ Build estático do Next.js (`NEXT_EXPORT=true npm run build`)
- ✅ Export para diretório `out/`
- ✅ Preparação dos artifacts para GitHub Pages (em `dist/wiki/`)
- ℹ️ Wiki será servido via `devportal.araponga.app/wiki`

**Objetivo**: Gerar o site estático pronto para deploy.

### 3. Deploy (GitHub Pages)

**Nome**: `deploy`  
**Executa**: Apenas em push para `main`

**Passos**:
- ✅ Deploy dos artifacts para GitHub Pages
- ✅ Configuração automática do domínio `wiki.araponga.app`

**Objetivo**: Publicar o site automaticamente.

## 📝 Testes

### Testes Automatizados

O wiki inclui testes básicos em `__tests__/docs.test.ts`:

- ✅ Verificação de existência da pasta `docs/`
- ✅ Verificação de arquivos markdown
- ✅ Validação de arquivos principais (`00_INDEX.md`, `ONBOARDING_PUBLICO.md`)
- ✅ Teste de encoding UTF-8

### Executar Testes Localmente

```bash
cd frontend/wiki

# Instalar dependências
npm install

# Executar testes
npm test

# Testes em modo watch
npm run test:watch

# Type check
npm run type-check

# Lint
npm run lint

# Testar links da wiki (requer wiki em produção ou local)
npm run test:links
```

### Teste de links (`test:links`)

O script `scripts/test-links.mjs` valida se os links principais da wiki respondem corretamente (ex.: `/`, `/docs/`, onboarding, etc.). Ele faz requisições HTTP ao host configurado.

**Causa das falhas antigas**: o script usava **caminho base fixo** (`https://devportal.araponga.app/wiki`). Em local ou CI sem deploy, esse host não resolve → `ENOTFOUND`. O último deploy falhou por isso; **DevPortal e Wiki ficaram fora** até a correção. Agora a base é configurável via `WIKI_URL` e, se o host estiver inacessível, os testes são pulados (exit 0).

**Comportamento**:

- **Host acessível** (ex.: `devportal.araponga.app` em produção): executa os testes e falha se algum link estiver quebrado.
- **Host inacessível** (local sem deploy, DNS não configurado, `ENOTFOUND`, etc.): o script **termina com sucesso** (exit 0), exibe um aviso e **pula** os testes. Assim, `npm ci` e `npm run test:links` não quebram em ambiente local ou em CI antes do deploy.

**Testar localmente** (com a wiki rodando em `http://localhost:3001`):

```bash
WIKI_URL=http://localhost:3001/wiki npm run test:links
```

O workflow **DevPortal Pages** (`.github/workflows/devportal-pages.yml`) roda `test:links` após o deploy, com `WIKI_URL=https://devportal.araponga.app/wiki`.

## 🌐 Deploy e Domínio

### GitHub Pages

O site é deployado automaticamente para GitHub Pages quando:
- Push para `main` é bem-sucedido
- Todos os testes passam
- Build estático é gerado corretamente

### Domínio e Roteamento

**Acesso**: `devportal.araponga.app/wiki`  
**Base Path**: `/wiki` (configurado no `next.config.mjs`)

### Configuração DNS

**Nenhuma configuração DNS adicional necessária!**

O wiki é servido como subpasta do DevPortal:
- **URL**: `devportal.araponga.app/wiki`
- **DNS**: Usa a mesma configuração de `devportal.araponga.app`
- **CNAME**: Já configurado para `devportal.araponga.app` → `sraphaz.github.io`

## 🐛 Troubleshooting

### Deploy falhou / DevPortal e Wiki fora

**Contexto**: O último deploy falhou por causa do `test:links` (caminhos fixos). DevPortal e Wiki ficaram fora até a correção. O script agora usa `WIKI_URL`, pula os testes quando o host está inacessível e não bloqueia mais o pipeline. Faça push dos commits com a correção e dispare o workflow **DevPortal Pages** para subir os sites de novo.

### Build Falha

**Erro**: `.next` ou `out/` não encontrado

**Solução**:
- Verifique se `npm run build` está funcionando localmente
- Verifique logs do workflow no GitHub Actions

### Testes Falham

**Erro**: Testes não encontram arquivos

**Solução**:
- Verifique se a pasta `docs/` existe na raiz do projeto
- Execute `npm test` localmente para ver erros específicos

### Deploy Não Funciona

**Erro**: GitHub Pages não atualiza

**Solução**:
- Verifique permissões do workflow (precisa `pages: write`)
- Verifique se o arquivo `CNAME` está sendo criado em `dist/CNAME`
- Verifique logs do job `deploy` no GitHub Actions

## 📊 Status Badge

Você pode adicionar um badge de status do workflow no README:

```markdown
![Wiki CI/CD](https://github.com/sraphaz/araponga/workflows/Build,%20Test%20&%20Deploy%20Wiki%20to%20GitHub%20Pages/badge.svg)
```

## 🔗 Links Úteis

- [GitHub Actions - Wiki Pages Workflow](.github/workflows/wiki-pages.yml)
- [Next.js - Static Exports](https://nextjs.org/docs/app/building-your-application/deploying/static-exports)
- [GitHub Pages - Custom Domains](https://docs.github.com/en/pages/configuring-a-custom-domain-for-your-github-pages-site)
- [Jest - Testing Framework](https://jestjs.io/)
