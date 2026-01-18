# Guia de Migração para Utilitários Centralizados

Este documento descreve como migrar código duplicado para usar os utilitários centralizados em `lib/`.

## Status da Migração

### ✅ Criado (Estrutura Base)
- `lib/markdown.ts` - Utilitários de Markdown
- `lib/file-utils.ts` - Utilitários de arquivos
- `lib/document.ts` - Processamento de documentos
- `lib/README.md` - Documentação

### ⚠️ Pendente (Refatoração de Páginas)
- `app/page.tsx` - Usa funções locais `getTextContent`, `processMarkdownLinks`
- `app/docs/[slug]/page.tsx` - Usa funções locais `getTextContent`, `processMarkdownLinks`, `removeNumericPrefix`
- `app/docs/[...slug]/page.tsx` - Usa funções locais `getTextContent`, `processMarkdownLinks`, `removeNumericPrefix`
- `app/docs/[slug]/content-sections.tsx` - Usa função local `getTextContent`

## Como Migrar

### Passo 1: Substituir `getTextContent` local

**Antes:**
```typescript
function getTextContent(html: string): string {
  return sanitizeHtml(html, {
    allowedTags: [],
    allowedAttributes: {},
  });
}
```

**Depois:**
```typescript
import { getTextContent } from "@/lib/markdown";
```

### Passo 2: Substituir `processMarkdownLinks` local

**Antes:**
```typescript
function processMarkdownLinks(html: string, basePath: string = '/wiki'): string {
  // ... implementação local ...
}
```

**Depois:**
```typescript
import { processMarkdownLinks } from "@/lib/markdown";
```

### Passo 3: Substituir `removeNumericPrefix` local

**Antes:**
```typescript
function removeNumericPrefix(text: string): string {
  return text.replace(/^\d+_/, "");
}
```

**Depois:**
```typescript
import { removeNumericPrefix } from "@/lib/file-utils";
```

### Passo 4: Usar `processMarkdownContent` (Opcional, Simplifica Muito)

**Antes:**
```typescript
async function getDocContent(filePath: string) {
  try {
    const docsPath = join(process.cwd(), "..", "..", "docs", filePath);
    const fileContents = await readFile(docsPath, "utf8");
    const { content, data } = matter(fileContents);
    
    const processedContent = await remark()
      .use(remarkHtml)
      .use(remarkGfm)
      .process(content);
    
    let htmlContent = processedContent.toString();
    // ... muito processamento ...
    
    return {
      content: htmlContent,
      frontMatter: data,
      title: data.title || "fallback",
    };
  } catch (error) {
    // ...
  }
}
```

**Depois:**
```typescript
import { processMarkdownContent } from "@/lib/document";

const doc = await processMarkdownContent(filePath, "/wiki");
```

## Benefícios

1. **Menos Código Duplicado** - Funções compartilhadas em um único lugar
2. **Manutenção Mais Fácil** - Correções e melhorias em um só lugar
3. **Type Safety** - TypeScript garante consistência
4. **Testabilidade** - Utilitários podem ser testados isoladamente
5. **Documentação Centralizada** - Um lugar para entender como tudo funciona

## Próximos Passos

1. Refatorar `app/page.tsx` para usar utilitários
2. Refatorar `app/docs/[slug]/page.tsx` para usar `processMarkdownContent`
3. Refatorar `app/docs/[...slug]/page.tsx` para usar `processMarkdownContent`
4. Refatorar `app/docs/[slug]/content-sections.tsx` para usar `getTextContent` de `lib/markdown`
