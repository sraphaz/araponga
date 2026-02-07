# Componentes Interativos da Wiki

Esta documentação descreve os componentes interativos disponíveis para revelação progressiva de conteúdo na Wiki Arah.

## 📦 Componentes Disponíveis

### Accordion

Seções expansíveis para organizar conteúdo e revelar informações progressivamente.

```tsx
import { Accordion } from "@/components/ui";

<Accordion title="Título da Seção" icon="💡" defaultOpen={false}>
  <p>Conteúdo que será revelado ao clicar no título.</p>
</Accordion>
```

**Props:**
- `title`: Título da seção (string, obrigatório)
- `children`: Conteúdo a ser revelado (ReactNode, obrigatório)
- `defaultOpen`: Abrir por padrão? (boolean, opcional, default: false)
- `icon`: Ícone opcional antes do título (string, opcional)

**Características:**
- Animação suave de abertura/fechamento
- Hover states refinados
- Acessibilidade (aria-expanded)
- Ícone chevron animado

---

### Tabs

Sistema de abas para organizar conteúdo relacionado em diferentes painéis.

```tsx
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui";

<Tabs defaultValue="tab1">
  <TabsList>
    <TabsTrigger value="tab1" icon="📖">Tab 1</TabsTrigger>
    <TabsTrigger value="tab2" icon="⚙️">Tab 2</TabsTrigger>
  </TabsList>
  <TabsContent value="tab1">
    <p>Conteúdo da Tab 1</p>
  </TabsContent>
  <TabsContent value="tab2">
    <p>Conteúdo da Tab 2</p>
  </TabsContent>
</Tabs>
```

**Componentes:**
- `Tabs`: Container principal (props: `defaultValue`, `children`)
- `TabsList`: Lista de triggers (props: `children`)
- `TabsTrigger`: Botão de tab (props: `value`, `children`, `icon?`)
- `TabsContent`: Conteúdo da tab (props: `value`, `children`)

**Características:**
- Context API para gerenciamento de estado
- Animação de transição entre tabs
- Indicador visual ativo (linha inferior)
- Suporte a ícones

---

### Tooltip

Tooltips informativos que aparecem ao passar o mouse ou focar em elementos.

```tsx
import { Tooltip } from "@/components/ui";

<Tooltip content="Informação adicional sobre este elemento" position="top">
  <button>Hover me</button>
</Tooltip>
```

**Props:**
- `content`: Conteúdo do tooltip (string | ReactNode, obrigatório)
- `children`: Elemento que aciona o tooltip (ReactNode, obrigatório)
- `position`: Posição do tooltip (top | bottom | left | right, opcional, default: "top")

**Características:**
- Posicionamento inteligente
- Auto-ajuste em scroll/resize
- Animação fade-in suave
- Acessibilidade (focus/blur)

---

### ExpandableCard

Cards que revelam conteúdo adicional ao clicar.

```tsx
import { ExpandableCard } from "@/components/ui";

<ExpandableCard
  title="Título do Card"
  summary="Resumo breve do conteúdo"
  icon="📚"
  color="forest"
>
  <p>Conteúdo expandido que aparece ao clicar.</p>
</ExpandableCard>
```

**Props:**
- `title`: Título do card (string, obrigatório)
- `summary`: Resumo breve (string, obrigatório)
- `children`: Conteúdo expandido (ReactNode, obrigatório)
- `icon`: Ícone opcional (string, opcional)
- `color`: Variante de cor (forest | accent | link, opcional, default: "forest")

**Características:**
- Design harmonizado com FeatureCard
- Transições suaves
- Variantes de cor da paleta Arah
- Ícone animado

---

### InteractiveDemo

Componente especializado para demonstrações e exemplos interativos.

```tsx
import { InteractiveDemo } from "@/components/ui";

<InteractiveDemo
  title="Exemplo Interativo"
  description="Descrição do que será demonstrado"
  defaultOpen={false}
>
  <pre className="code-block">// Código de exemplo</pre>
</InteractiveDemo>
```

**Props:**
- `title`: Título da demonstração (string, obrigatório)
- `description`: Descrição do que será mostrado (string, obrigatório)
- `children`: Conteúdo da demonstração (ReactNode, obrigatório)
- `defaultOpen`: Abrir por padrão? (boolean, opcional, default: false)

**Características:**
- Área dedicada para exemplos
- Botão toggle claro ("Explorar"/"Ocultar")
- Background diferenciado para código
- Ideal para snippets e demonstrações

---

## 🎨 Padrões de Uso

### Revelação Progressiva

Use componentes interativos para revelar conteúdo gradualmente, evitando sobrecarregar a página:

1. **Accordion** para seções de FAQ ou detalhes técnicos
2. **Tabs** para organizar conteúdo relacionado
3. **ExpandableCard** para informações complementares
4. **InteractiveDemo** para exemplos de código

### Hierarquia Visual

- Mantenha a hierarquia clara: título sempre visível, conteúdo revelado
- Use ícones para indicar interatividade
- Animações sutis para feedback visual

### Acessibilidade

Todos os componentes incluem:
- Suporte a teclado (Enter, Space, Tab)
- Atributos ARIA apropriados
- Focus states visíveis
- Compatibilidade com leitores de tela

---

## 🚀 Exemplos de Uso

### FAQ Section

```tsx
<Accordion title="Como começar?" icon="🚀">
  <p>Primeiro, leia a documentação...</p>
</Accordion>
```

### Multiple Code Examples

```tsx
<Tabs defaultValue="csharp">
  <TabsList>
    <TabsTrigger value="csharp">C#</TabsTrigger>
    <TabsTrigger value="javascript">JavaScript</TabsTrigger>
  </TabsList>
  <TabsContent value="csharp">
    <pre>{csharpCode}</pre>
  </TabsContent>
  <TabsContent value="javascript">
    <pre>{jsCode}</pre>
  </TabsContent>
</Tabs>
```

### Progressive Disclosure

```tsx
<ExpandableCard
  title="Detalhes Técnicos"
  summary="Informações avançadas sobre a implementação"
>
  <TechnicalDetails />
</ExpandableCard>
```

---

## 💡 Benefícios

1. **Performance**: Conteúdo carregado sob demanda
2. **UX**: Revelação progressiva reduz sobrecarga cognitiva
3. **Organização**: Conteúdo melhor estruturado e navegável
4. **Engajamento**: Interação motiva exploração
5. **Acessibilidade**: Todos os componentes são acessíveis

---

**Última Atualização**: 2025-01-20  
**Versão**: 1.0
