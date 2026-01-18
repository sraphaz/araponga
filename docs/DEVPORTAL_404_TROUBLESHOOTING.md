# Troubleshooting: DevPortal 404 no GitHub Pages

## Problema

O DevPortal em `devportal.araponga.app` está retornando 404, mesmo com o workflow de deploy executando com sucesso.

## Causa Provável

O arquivo `CNAME` na raiz do repositório (`araponga.eco.br`) pode estar conflitando com o CNAME criado pelo workflow no `dist/` (`devportal.araponga.app`).

## Configuração Atual

- **GitHub Pages CNAME**: `devportal.araponga.app` (configurado via API, confirmado)
- **CNAME na raiz do repo**: `araponga.eco.br` (pode estar conflitando)
- **CNAME criado no dist/**: `devportal.araponga.app` (correto, criado pelo workflow)

## Soluções Possíveis

### Solução 1: Remover CNAME da Raiz (Recomendado)

Se o `CNAME` na raiz não é mais necessário:

```bash
git rm CNAME
git commit -m "fix: Remove CNAME da raiz que conflita com devportal.araponga.app"
git push
```

**Nota**: Verifique se `araponga.eco.br` ainda está em uso antes de remover.

### Solução 2: Mover CNAME para Outra Localização

Se o `CNAME` `araponga.eco.br` ainda é necessário para outro propósito, mova para uma pasta específica:

```bash
mkdir -p docs/legacy-dns
mv CNAME docs/legacy-dns/
git commit -m "fix: Move CNAME para evitar conflito com GitHub Pages"
git push
```

### Solução 3: Garantir que Workflow Sobrescreve CNAME

O workflow já cria `dist/CNAME` corretamente. O problema pode ser que o GitHub Pages está priorizando o CNAME da raiz.

**Verificação**: Após remover/mover o CNAME da raiz, o próximo deploy deve funcionar automaticamente.

## Validação

Após aplicar a solução:

1. Verifique se o deploy foi bem-sucedido:
   ```bash
   gh run list --workflow=devportal-pages.yml --limit 1
   ```

2. Aguarde 1-2 minutos para propagação do GitHub Pages

3. Acesse: `https://devportal.araponga.app/`

4. Verifique o status do GitHub Pages:
   ```bash
   gh api repos/sraphaz/araponga/pages --jq '.cname, .status'
   ```

## Status do Deploy

- ✅ Build: Sucesso
- ✅ Deploy: Sucesso  
- ✅ CNAME configurado: `devportal.araponga.app`
- ❌ Acesso: 404 (possível conflito com CNAME na raiz)

## Referências

- Workflow: `.github/workflows/devportal-pages.yml`
- Configuração GitHub Pages: Linha 66 cria `dist/CNAME` com `devportal.araponga.app`
- Arquivo CNAME na raiz: `CNAME` (conteúdo: `araponga.eco.br`)
