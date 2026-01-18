# Plano Sistemático de Reformulação de Design - Araponga

**Versão**: 2.0  
**Data**: 2025-01-20  
**Status**: 🚀 Em Execução  
**Referência de Excelência**: closer.earth, Material Design 3, Apple HIG, WCAG 2.1

---

## 📋 Sumário Executivo

Este documento apresenta um **plano sistemático e completo** de reformulação do design system do Araponga, alinhado com diretrizes internacionais de design (Material Design 3, Apple HIG, WCAG 2.1) e profundamente enraizado nos **valores fundamentais** do projeto: território, autonomia, transparência, sobriedade elevada e consciência ética.

**Objetivo Final:** Transformar Wiki e DevPortal em **referências de design profissional**, igualando ou superando closer.earth em qualidade visual, consistência e experiência do usuário.

---

## 🎯 Valores da Araponga (Base para Design)

### Valores Fundamentais

1. **Território como Referência**
   - Geografia como fundamento estrutural
   - Lugar físico real, respeitado e valorizado

2. **Autonomia Local**
   - Comunidades controlam sua presença digital
   - Tecnologia serve, não controla

3. **Transparência Radical**
   - Processos abertos e auditáveis
   - Clareza e visibilidade

4. **Sobriedade Elevada**
   - Profissional, mas não corporativo
   - Elegante e minimalista
   - Sem ruído visual

5. **Consciência Ética**
   - Tecnologia que serve à vida
   - Responsabilidade e cuidado

### Personalidade Visual

- **Sóbrio**: Design minimalista, sem elementos desnecessários
- **Transparente**: Clareza visual, hierarquia evidente
- **Territorial**: Cores que remetem à natureza e ao território
- **Profissional**: Qualidade enterprise, atenção aos detalhes
- **Acolhedor**: Acessível, inclusivo, confortável

---

## 📐 Diretrizes Internacionais Aplicadas

### Material Design 3
- ✅ Sistema de design escalável e consistente
- ✅ Design tokens unificados
- ✅ Hierarquia tipográfica clara
- ✅ Espaçamento baseado em grid matemático

### Apple Human Interface Guidelines
- ✅ Clareza, deferência e profundidade
- ✅ Tipografia otimizada para leitura
- ✅ Contraste adequado para legibilidade
- ✅ Transições suaves e naturais

### WCAG 2.1 (Acessibilidade)
- ✅ Contraste mínimo 4.5:1 (texto normal)
- ✅ Contraste mínimo 3:1 (texto grande)
- ✅ Navegação por teclado
- ✅ Estados focáveis claramente visíveis

---

## 🎨 Revisão Artística: Paleta de Cores

### Análise dos Valores → Cores

Baseado nos valores da Araponga (território, transparência, sobriedade), a paleta deve evocar:

- **Natureza e Território**: Verdes suaves e naturais
- **Transparência e Clareza**: Azuis claros e neutros
- **Sobriedade**: Neutros elegantes, sem saturação excessiva
- **Profissionalismo**: Cores que transmitem confiança

### Paleta Proposta (Alinhada com Valores)

#### Cores Primárias (Natureza e Território)

```css
/* Verde Primário - Natureza, Território, Crescimento */
--color-primary-50:  #f0fdf4;   /* Base muito clara */
--color-primary-100: #dcfce7;   /* Base clara */
--color-primary-200: #bbf7d0;   /* Subtle accent */
--color-primary-300: #86efac;   /* Light accent */
--color-primary-400: #4dd4a8;   /* PRIMARY - Verde natural, equilibrado */
--color-primary-500: #3bc495;   /* PRIMARY HOVER - Mais saturado */
--color-primary-600: #22c55e;   /* Strong accent */
--color-primary-700: #16a34a;   /* Dark variant */
--color-primary-800: #15803d;   /* Darker variant */
--color-primary-900: #14532d;   /* Darkest variant */

/* Azul Secundário - Transparência, Clareza, Confiança */
--color-secondary-50:  #f0f9ff;
--color-secondary-100: #e0f2fe;
--color-secondary-200: #bae6fd;
--color-secondary-300: #7dd3ff;  /* SECUNDARY - Azul claro, transparente */
--color-secondary-400: #9de3ff;  /* SECUNDARY HOVER - Mais claro */
--color-secondary-500: #0ea5e9;
--color-secondary-600: #0284c7;
--color-secondary-700: #0369a1;
--color-secondary-800: #075985;
--color-secondary-900: #0c4a6e;
```

#### Cores Neutras (Sobriedade e Profissionalismo)

```css
/* Neutros para Texto e Background */
/* Light Mode */
--color-neutral-50:  #f9fafb;   /* Background mais claro */
--color-neutral-100: #f3f4f6;   /* Background claro */
--color-neutral-200: #e5e7eb;   /* Border claro */
--color-neutral-300: #d1d5db;   /* Border medium */
--color-neutral-400: #9ca3af;   /* Text muted */
--color-neutral-500: #6b7280;   /* Text secondary */
--color-neutral-600: #4b5563;   /* Text primary light */
--color-neutral-700: #374151;   /* Text primary */
--color-neutral-800: #1f2937;   /* Text strong */
--color-neutral-900: #111827;   /* Text darkest */

/* Dark Mode - Baseado em #0a0e12 (atual) mas refinado */
--color-dark-50:  #f9fafb;      /* Text em dark mode */
--color-dark-100: #f3f4f6;
--color-dark-200: #e5e7eb;      /* Text secondary dark */
--color-dark-300: #d1d5db;
--color-dark-400: #9ca3af;      /* Text muted dark */
--color-dark-500: #6b7280;
--color-dark-600: #4b5563;
--color-dark-700: #374151;
--color-dark-800: #1e2830;      /* Background elevated */
--color-dark-850: #141a21;      /* Background card */
--color-dark-900: #0f1419;      /* Background muted */
--color-dark-950: #0a0e12;      /* Background base */
```

#### Cores Semânticas (Estados e Feedback)

```css
/* Success - Verde natural (usa primary) */
--color-success: var(--color-primary-400);
--color-success-hover: var(--color-primary-500);

/* Warning - Amarelo suave, não agressivo */
--color-warning-50:  #fffbeb;
--color-warning-100: #fef3c7;
--color-warning-400: #f5c842;   /* WARNING */
--color-warning-600: #d97706;

/* Error - Vermelho suave, não alarmante */
--color-error-50:  #fef2f2;
--color-error-100: #fee2e2;
--color-error-400: #f26d6d;     /* ERROR */
--color-error-600: #dc2626;

/* Info - Azul claro (usa secondary) */
--color-info: var(--color-secondary-300);
--color-info-hover: var(--color-secondary-400);
```

### Aplicação da Paleta

**Regra Fundamental:** Cores refletem função, não decoração.

- **Primary (Verde)**: Ações principais, destaque, confirmação
- **Secondary (Azul)**: Links, informações, acentos suaves
- **Neutros**: Texto, backgrounds, bordas
- **Semânticas**: Feedback de ações (success, warning, error)

---

## 📏 Sistema de Design Tokens

### Tipografia (Escala Harmônica 1.125)

**Baseado em Material Design 3 e Apple HIG:**

```css
/* Font Families */
--font-family-sans: 'Inter', -apple-system, BlinkMacSystemFont, 'SF Pro Display', 'Segoe UI', system-ui, sans-serif;
--font-family-mono: 'JetBrains Mono', 'SF Mono', 'Monaco', 'Consolas', monospace;

/* Font Sizes - Escala 1.125 (minor third) */
--font-size-xs:   0.75rem;   /* 12px */
--font-size-sm:  0.875rem;   /* 14px */
--font-size-base: 1rem;      /* 16px - Base */
--font-size-lg:  1.125rem;   /* 18px */
--font-size-xl:  1.25rem;    /* 20px */
--font-size-2xl: 1.5rem;     /* 24px */
--font-size-3xl: 1.875rem;   /* 30px */
--font-size-4xl: 2.25rem;    /* 36px */
--font-size-5xl: 3rem;       /* 48px */
--font-size-6xl: 3.75rem;    /* 60px */
--font-size-7xl: 4.5rem;     /* 72px */

/* Font Weights */
--font-weight-normal: 400;
--font-weight-medium: 500;
--font-weight-semibold: 600;
--font-weight-bold: 700;

/* Line Heights - Otimizados para leitura */
--line-height-tight:   1.25;  /* Títulos curtos */
--line-height-snug:    1.375; /* Títulos médios */
--line-height-normal:  1.5;   /* Títulos longos */
--line-height-relaxed: 1.75;  /* Corpo de texto */
--line-height-loose:   2;     /* Texto espaçoso */

/* Letter Spacing */
--letter-spacing-tighter: -0.05em;
--letter-spacing-tight:   -0.025em;
--letter-spacing-normal:  0;
--letter-spacing-wide:    0.025em;
--letter-spacing-wider:   0.05em;
```

**Hierarquia Tipográfica:**

| Elemento | Tamanho | Weight | Line Height | Uso |
|----------|---------|--------|-------------|-----|
| H1 | `--font-size-5xl` (48px) | 700 | 1.2 | Título principal da página |
| H2 | `--font-size-4xl` (36px) | 600 | 1.3 | Seções principais |
| H3 | `--font-size-3xl` (30px) | 600 | 1.4 | Subseções |
| H4 | `--font-size-2xl` (24px) | 600 | 1.4 | Seções menores |
| H5 | `--font-size-xl` (20px) | 600 | 1.5 | Subsseções |
| H6 | `--font-size-lg` (18px) | 600 | 1.5 | Menor hierarquia |
| Body | `--font-size-base` (16px) | 400 | 1.75 | Texto corrido |
| Small | `--font-size-sm` (14px) | 400 | 1.5 | Texto auxiliar |
| Code | `--font-size-sm` (14px) | 400 | 1.5 | Código inline |

### Espaçamento (Sistema Base 8px)

**Baseado em Material Design e Apple HIG:**

```css
/* Espaçamento - Escala 8px (base matemática) */
--space-0:  0;
--space-1:  0.25rem;  /* 4px */
--space-2:  0.5rem;   /* 8px */
--space-3:  0.75rem;  /* 12px */
--space-4:  1rem;     /* 16px */
--space-5:  1.25rem;  /* 20px */
--space-6:  1.5rem;   /* 24px */
--space-8:  2rem;     /* 32px */
--space-10: 2.5rem;   /* 40px */
--space-12: 3rem;     /* 48px */
--space-16: 4rem;     /* 64px */
--space-20: 5rem;     /* 80px */
--space-24: 6rem;     /* 96px */
--space-32: 8rem;     /* 128px */

/* Aplicação de Espaçamento */
--spacing-section: var(--space-16);  /* Entre seções principais */
--spacing-card: var(--space-8);      /* Padding interno de cards */
--spacing-element: var(--space-4);   /* Entre elementos relacionados */
--spacing-grid: var(--space-6);      /* Gap em grids */
```

**Regras de Espaçamento:**

- **Entre seções principais**: `--space-16` (64px) ou `--space-20` (80px)
- **Entre subseções**: `--space-12` (48px)
- **Entre elementos relacionados**: `--space-4` (16px) ou `--space-6` (24px)
- **Padding em cards**: `--space-4` mobile, `--space-6` desktop
- **Gap em grids**: `--space-4` mobile, `--space-6` tablet, `--space-8` desktop

### Border Radius

```css
--radius-none: 0;
--radius-sm:   0.25rem;  /* 4px */
--radius-md:   0.5rem;   /* 8px */
--radius-lg:   1rem;     /* 16px */
--radius-xl:   1.5rem;   /* 24px */
--radius-2xl:  2rem;     /* 32px */
--radius-full: 9999px;
```

### Shadows (Elevação)

```css
/* Shadows - Material Design 3 elevation */
--shadow-xs:   0 1px 2px 0 rgba(0, 0, 0, 0.05);
--shadow-sm:   0 1px 3px 0 rgba(0, 0, 0, 0.1), 0 1px 2px -1px rgba(0, 0, 0, 0.1);
--shadow-md:   0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -2px rgba(0, 0, 0, 0.1);
--shadow-lg:   0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -4px rgba(0, 0, 0, 0.1);
--shadow-xl:   0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 8px 10px -6px rgba(0, 0, 0, 0.1);
--shadow-2xl:  0 25px 50px -12px rgba(0, 0, 0, 0.25);

/* Dark Mode Shadows (mais pronunciadas) */
--shadow-dark-xs:   0 1px 2px 0 rgba(0, 0, 0, 0.3);
--shadow-dark-sm:   0 1px 3px 0 rgba(0, 0, 0, 0.4), 0 1px 2px -1px rgba(0, 0, 0, 0.4);
--shadow-dark-md:   0 4px 6px -1px rgba(0, 0, 0, 0.5), 0 2px 4px -2px rgba(0, 0, 0, 0.5);
--shadow-dark-lg:   0 10px 15px -3px rgba(0, 0, 0, 0.6), 0 4px 6px -4px rgba(0, 0, 0, 0.6);
```

### Transições

```css
--transition-fast:   150ms cubic-bezier(0.4, 0, 0.2, 1);
--transition-base:   200ms cubic-bezier(0.4, 0, 0.2, 1);
--transition-slow:   300ms cubic-bezier(0.4, 0, 0.2, 1);
--transition-smooth: 400ms cubic-bezier(0.16, 1, 0.3, 1);

/* Propriedades comuns para transição */
--transition-default: all var(--transition-base);
```

### Grid System

```css
/* Container Max Widths */
--container-xs:  100%;
--container-sm:  640px;
--container-md:  768px;
--container-lg:  1024px;
--container-xl:  1280px;
--container-2xl: 1536px;

/* Grid Columns (12-column system) */
--grid-cols-1:  repeat(1, minmax(0, 1fr));
--grid-cols-2:  repeat(2, minmax(0, 1fr));
--grid-cols-3:  repeat(3, minmax(0, 1fr));
--grid-cols-4:  repeat(4, minmax(0, 1fr));
--grid-cols-6:  repeat(6, minmax(0, 1fr));
--grid-cols-12: repeat(12, minmax(0, 1fr));

/* Sidebar Width */
--sidebar-width: 256px;  /* Consistente em Wiki e DevPortal */
```

---

## 🏗️ Estrutura de Implementação

### Fase 1: Fundação - Design Tokens (Semana 1-2)

**Objetivo:** Criar fonte única de verdade para todos os valores de design.

**Entregáveis:**

1. **Arquivo `design-tokens.css` unificado:**
   ```
   frontend/shared/styles/
   ├── design-tokens.css      # Todos os tokens
   ├── tokens-colors.css      # Paleta de cores
   ├── tokens-typography.css  # Tipografia
   ├── tokens-spacing.css     # Espaçamento
   └── tokens-components.css  # Componentes
   ```

2. **Documentação de tokens:**
   - `docs/DESIGN_SYSTEM_TOKENS.md`
   - Exemplos visuais de cada token
   - Guia de uso

**Critérios de Sucesso:**
- [ ] Todos os tokens definidos e documentados
- [ ] Cores seguem valores da Araponga
- [ ] Tipografia segue escala harmônica
- [ ] Espaçamento baseado em 8px

### Fase 2: Unificação Visual (Semana 3-4)

**Objetivo:** Wiki e DevPortal compartilham base visual idêntica.

**Entregáveis:**

1. **Estrutura de CSS compartilhado:**
   ```
   frontend/
   ├── shared/
   │   └── styles/
   │       ├── design-tokens.css
   │       ├── base.css          # Reset, base styles
   │       ├── typography.css    # Tipografia
   │       ├── layout.css        # Grid, containers
   │       └── components.css    # Componentes base
   ├── wiki/
   │   └── app/
   │       └── globals.css       # Importa shared + específico Wiki
   └── devportal/
       └── assets/
           └── css/
               └── devportal.css # Importa shared + específico DevPortal
   ```

2. **Componentes unificados:**
   - Cards (mesmo estilo, mesmo padding)
   - Buttons (mesmos estados, mesma aparência)
   - Sidebar (mesma largura, mesmo comportamento)
   - Header (mesma altura, mesmo tema toggle)

**Critérios de Sucesso:**
- [ ] Wiki e DevPortal importam tokens compartilhados
- [ ] Componentes visualmente idênticos
- [ ] Dark/light mode totalmente harmonizado

### Fase 3: Aplicação Sistemática (Semana 5-6)

**Objetivo:** Aplicar tokens em todas as páginas de forma sistemática.

**Entregáveis:**

1. **Hierarquia tipográfica aplicada:**
   - Todas as páginas usam escala harmônica
   - H1-H6 seguem tamanhos definidos
   - Line-height otimizado para leitura

2. **Espaçamento sistemático:**
   - Todos os espaçamentos usam tokens (múltiplos de 4px ou 8px)
   - Grid system consistente
   - Padding/margin não arbitrários

3. **Cores aplicadas consistentemente:**
   - Primary (verde) para ações principais
   - Secondary (azul) para links
   - Neutros para texto e backgrounds
   - Sem cores hardcoded

**Critérios de Sucesso:**
- [ ] Nenhum valor hardcoded (tudo via tokens)
- [ ] Hierarquia visual clara em todas as páginas
- [ ] Espaçamento consistente e harmônico
- [ ] Contraste WCAG AA em todos os textos

### Fase 4: Refinamento e Polimento (Semana 7-8)

**Objetivo:** Micro-interações, estados completos, acessibilidade final.

**Entregáveis:**

1. **Estados completos dos componentes:**
   - Buttons: default, hover, active, focus, disabled, loading
   - Links: default, hover, focus, visited, active
   - Form inputs: default, focus, error, success, disabled

2. **Micro-interações sutis:**
   - Transições suaves (200-300ms)
   - Hover states informativos
   - Loading states (spinners, skeletons)

3. **Acessibilidade WCAG AA:**
   - Contraste 4.5:1 (texto normal)
   - Navegação por teclado
   - Focus indicators claros
   - Estrutura semântica correta

**Critérios de Sucesso:**
- [ ] Todos os componentes com estados completos
- [ ] Transições suaves e funcionais
- [ ] WCAG AA compliance
- [ ] Performance mantida (FCP < 1.5s)

---

## 📊 Métricas de Sucesso

### Métricas Visuais

- [ ] **Consistência:** Wiki e DevPortal visualmente indistinguíveis em estilo
- [ ] **Hierarquia:** Teste de usuário identifica H1, H2, H3 facilmente
- [ ] **Espaçamento:** Nenhum espaçamento arbitrário (todos múltiplos de 4px/8px)
- [ ] **Cores:** Mesma função = mesma cor em toda a plataforma

### Métricas de UX

- [ ] **Navegação:** Usuário encontra informação em ≤ 3 cliques
- [ ] **Legibilidade:** Texto confortável de ler (line-height ≥ 1.5 para body)
- [ ] **Responsividade:** Funciona bem em mobile, tablet, desktop
- [ ] **Performance:** First Contentful Paint < 1.5s

### Métricas de Acessibilidade

- [ ] **Contraste:** Todos os textos ≥ WCAG AA (4.5:1)
- [ ] **Keyboard Navigation:** Todas as funções acessíveis via teclado
- [ ] **Screen Reader:** Estrutura semântica correta

---

## 🚀 Implementação Imediata

A implementação seguirá esta ordem:

1. ✅ **Criar `design-tokens.css` unificado** (HOJE)
2. ✅ **Migrar Wiki e DevPortal para usar tokens** (HOJE)
3. ✅ **Aplicar hierarquia tipográfica** (HOJE)
4. ✅ **Refinar espaçamento sistemático** (HOJE)
5. ⏳ **Completar estados dos componentes** (PRÓXIMA ETAPA)
6. ⏳ **Implementar micro-interações** (PRÓXIMA ETAPA)

---

**Preparado por:** Plano Sistemático de Reformulação de Design  
**Baseado em:** Valores da Araponga + Diretrizes Internacionais (Material Design 3, Apple HIG, WCAG 2.1)  
**Data:** 2025-01-20  
**Versão:** 2.0
