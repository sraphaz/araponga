# Bibliotecas de Utilitários da Wiki

Este diretório contém funções utilitárias compartilhadas para processamento de Markdown, manipulação de arquivos e processamento de documentos.

## Estrutura

### `markdown.ts`
Utilitários para processamento de Markdown:
- `getTextContent()` - Extrai texto puro de HTML (remove tags)
- `generateUniqueId()` - Gera IDs únicos a partir de texto (slug)
- `processMarkdownLinks()` - Processa links no HTML para incluir basePath
- `removeNumericPrefix()` - Remove prefixos numéricos (00_, 01_, etc.)
- `extractFirstH1()` - Extrai e remove o primeiro H1 do markdown
- `addHeadingIds()` - Adiciona IDs únicos aos headings (h2-h4)

### `file-utils.ts`
Utilitários para manipulação de arquivos e caminhos:
- `removeNumericPrefix()` - Remove prefixos numéricos de nomes de arquivo
- `getTitleFromFileName()` - Gera título a partir do nome do arquivo
- `getAllMarkdownFiles()` - Busca recursivamente todos os arquivos .md

### `document.ts`
Utilitários de alto nível para processamento de documentos:
- `processMarkdownContent()` - Processa conteúdo Markdown completo (leitura, parsing, processamento, link fixing)

## Uso

```typescript
import { processMarkdownContent } from "@/lib/document";

// Processa um documento completo
const doc = await processMarkdownContent("ONBOARDING_PUBLICO.md", "/wiki");
```

## Princípios

1. **DRY (Don't Repeat Yourself)** - Funções compartilhadas são centralizadas aqui
2. **Single Responsibility** - Cada função tem uma responsabilidade clara
3. **Type Safety** - Todas as funções são tipadas com TypeScript
4. **Reusabilidade** - Funções são genéricas e reutilizáveis
