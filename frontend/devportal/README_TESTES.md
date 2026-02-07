# Testes do DevPortal

## 📋 Visão Geral

Testes automatizados para o Developer Portal da API Arah. O DevPortal é uma aplicação HTML estática com JavaScript vanilla, então os testes validam estrutura HTML, funcionalidades JavaScript e links.

## 🧪 Estrutura de Testes

### Testes Unitários (Jest + jsdom)

- **`__tests__/html-structure.test.js`**: Valida estrutura HTML, IDs, links, phase-panels e acessibilidade
- **`__tests__/javascript-functionality.test.js`**: Valida funcionalidades JavaScript (navegação, scroll sync, accordions)

### Scripts de Validação

- **`scripts/test-links.mjs`**: Valida links internos e externos (formato)

## 🚀 Executando Testes

```bash
# Instalar dependências
cd frontend/devportal
npm install

# Executar todos os testes
npm test

# Executar testes em modo watch
npm run test:watch

# Testar apenas links
npm run test:links
```

## 📝 Cobertura de Testes

### Estrutura HTML

- ✅ IDs únicos e válidos
- ✅ Links da sidebar apontam para IDs existentes
- ✅ Phase-panels e phase-tabs correspondem
- ✅ Acessibilidade básica (ARIA, headings, alt text)

### Funcionalidades JavaScript

- ✅ Navegação entre phase-panels
- ✅ Sidebar toggle (abrir/fechar seções)
- ✅ Accordions (colapsar/expandir)
- ✅ Links e navegação
- ✅ Theme toggle

### Links

- ✅ Links internos apontam para IDs válidos
- ✅ Links externos têm formato válido
- ✅ Links da sidebar estão corretos

## 🔧 Configuração

### Jest Config (`jest.config.js`)

- **Test Environment**: `jsdom` (para simular DOM do navegador)
- **Coverage Threshold**: 60% (branches, functions, lines, statements)
- **Test Match**: `**/__tests__/**/*.test.js`

### Dependências

- `jest`: Framework de testes
- `jsdom`: Ambiente DOM para testes
- `jest-environment-jsdom`: Ambiente Jest para jsdom

## 📋 Checklist de Testes

### Antes de Criar PR

- [ ] Todos os testes passam: `npm test`
- [ ] Links válidos: `npm run test:links`
- [ ] Nenhum ID duplicado
- [ ] Todos os links da sidebar funcionam
- [ ] Phase-panels estão corretos
- [ ] Acessibilidade básica validada

### Testes Contínuos

- [ ] Testes executam no CI/CD (se configurado)
- [ ] Cobertura acima de 60%
- [ ] Novos recursos têm testes correspondentes

## 🐛 Troubleshooting

### Testes falhando

1. Verificar se o HTML está atualizado: `git status frontend/devportal/index.html`
2. Verificar IDs únicos: `npm test -- html-structure`
3. Verificar links: `npm run test:links`

### Problemas com jsdom

Se houver problemas com jsdom, verificar:
- Versão do Node.js (recomendado: >= 18)
- Limpeza do `node_modules`: `rm -rf node_modules && npm install`

## 📚 Referências

- [Jest Documentation](https://jestjs.io/docs/getting-started)
- [jsdom Documentation](https://github.com/jsdom/jsdom)
- [Testing HTML/JS Static Sites](https://jestjs.io/docs/tutorial-webpack)
