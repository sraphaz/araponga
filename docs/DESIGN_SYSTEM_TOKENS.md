# Design Tokens - Sistema Unificado

**Versão**: 1.0  
**Data**: 2025-01-20  
**Status**: ✅ Implementado  
**Localização**: `frontend/shared/styles/design-tokens.css`

---

## 📋 Sumário

Este documento descreve o sistema unificado de design tokens do Arah. Os tokens são a **fonte única de verdade** para todos os valores de design (cores, tipografia, espaçamento, etc.) e são usados por Wiki e DevPortal para garantir consistência visual total.

**Objetivo:** Garantir que Wiki e DevPortal compartilhem a mesma identidade visual, criando uma experiência unificada e profissional.

---

## 🎨 Sistema de Cores

### Primary (Verde) - Natureza, Território, Crescimento

Baseado no valor "Território como Referência" da Arah.

```css
--color-primary-400: #4dd4a8;  /* PRIMARY - Verde natural, equilibrado */
--color-primary-500: #3bc495;  /* PRIMARY HOVER - Mais saturado */
```

**Uso:** Ações principais, CTAs, destaques, confirmação

### Secondary (Azul) - Transparência, Clareza, Confiança

Baseado no valor "Transparência Radical" da Arah.

```css
--color-secondary-300: #7dd3ff;  /* SECONDARY - Azul claro, transparente */
--color-secondary-400: #9de3ff;  /* SECONDARY HOVER - Mais claro */
```

**Uso:** Links, informações, acentos suaves

### Neutros

```css
/* Light Mode */
--color-neutral-50 a --color-neutral-900

/* Dark Mode */
--color-dark-50 a --color-dark-950
```

**Uso:** Texto, backgrounds, bordas

### Semânticas

```css
--color-success: var(--color-primary-400);  /* Verde (usa primary) */
--color-warning: #f5c842;                   /* Amarelo suave */
--color-error: #f26d6d;                     /* Vermelho suave */
--color-info: var(--color-secondary-300);   /* Azul (usa secondary) */
```

---

## 📏 Tipografia

### Escala Harmônica (Ratio 1.125)

```css
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
```

### Line Heights (Otimizados para Leitura)

```css
--line-height-tight:   1.25;  /* Títulos curtos */
--line-height-snug:    1.375; /* Títulos médios */
--line-height-normal:  1.5;   /* Títulos longos */
--line-height-relaxed: 1.75;  /* Corpo de texto */
--line-height-loose:   2;     /* Texto espaçoso */
```

### Hierarquia Tipográfica

| Elemento | Tamanho | Weight | Line Height | Uso |
|----------|---------|--------|-------------|-----|
| H1 | `--font-size-5xl` (48px) | 700 | 1.2 | Título principal da página |
| H2 | `--font-size-4xl` (36px) | 600 | 1.3 | Seções principais |
| H3 | `--font-size-3xl` (30px) | 600 | 1.4 | Subseções |
| H4 | `--font-size-2xl` (24px) | 600 | 1.4 | Seções menores |
| Body | `--font-size-base` (16px) | 400 | 1.75 | Texto corrido |
| Small | `--font-size-sm` (14px) | 400 | 1.5 | Texto auxiliar |

---

## 📐 Espaçamento (Sistema Base 8px)

```css
--space-1:  0.25rem;  /* 4px */
--space-2:  0.5rem;   /* 8px */
--space-4:  1rem;     /* 16px */
--space-6:  1.5rem;   /* 24px */
--space-8:  2rem;     /* 32px */
--space-12: 3rem;     /* 48px */
--space-16: 4rem;     /* 64px */
--space-20: 5rem;     /* 80px */
--space-24: 6rem;     /* 96px */
```

**Regras de Aplicação:**

- **Entre seções principais**: `--space-16` (64px) ou `--space-20` (80px)
- **Entre subseções**: `--space-12` (48px)
- **Entre elementos relacionados**: `--space-4` (16px) ou `--space-6` (24px)
- **Padding em cards**: `--space-4` mobile, `--space-6` desktop
- **Gap em grids**: `--space-4` mobile, `--space-6` tablet, `--space-8` desktop

---

## 🎭 Shadows (Elevação)

```css
--shadow-sm:   0 1px 3px 0 rgba(0, 0, 0, 0.1);
--shadow-md:   0 4px 6px -1px rgba(0, 0, 0, 0.1);
--shadow-lg:   0 10px 15px -3px rgba(0, 0, 0, 0.1);
--shadow-xl:   0 20px 25px -5px rgba(0, 0, 0, 0.1);
```

---

## ⚡ Transições

```css
--transition-fast:   150ms cubic-bezier(0.4, 0, 0.2, 1);
--transition-base:   200ms cubic-bezier(0.4, 0, 0.2, 1);
--transition-slow:   300ms cubic-bezier(0.4, 0, 0.2, 1);
--transition-smooth: 400ms cubic-bezier(0.16, 1, 0.3, 1);
```

---

## 📦 Como Usar

### No CSS

```css
/* Use tokens diretamente */
.component {
  color: var(--color-primary-400);
  padding: var(--space-4);
  font-size: var(--font-size-base);
  line-height: var(--line-height-relaxed);
}
```

### No Tailwind (Wiki)

Os tokens são definidos no `globals.css` e podem ser referenciados via `var(--token-name)`.

---

**Referência Completa:** Ver `frontend/shared/styles/design-tokens.css`  
**Plano de Implementação:** Ver `docs/PLANO_SISTEMATICO_REFORMULACAO_DESIGN.md`
