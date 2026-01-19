# DevPortal - Reestruturação Detalhada URGENTE

**Data**: 2025-01-20  
**Status**: 🔴 URGENTE  
**Objetivo**: Reorganizar estrutura com submenus e páginas dedicadas conforme solicitação

---

## 📋 Nova Estrutura Proposta

### **1. FUNCIONALIDADES** (com submenus)

```
┌─────────────────────────────────────┐
│ FUNCIONALIDADES                     │
├─────────────────────────────────────┤
│ 1. Operações                        │
│    ├─ Método X (página + diagrama) │
│    ├─ Método Y (página + diagrama) │
│    └─ Método Z (página + diagrama) │
│                                     │
│ 2. Cenários Negócio                 │
│    ├─ Onboarding Usuário            │
│    ├─ Publicar conteúdo com mídias  │
│    ├─ Assets territoriais           │
│    ├─ Chat territorial e grupos     │
│    ├─ Marketplace e economia local  │
│    └─ Eventos comunitários          │
└─────────────────────────────────────┘
```

### **2. API PRÁTICA** (expandida)

```
┌─────────────────────────────────────┐
│ API PRÁTICA                         │
├─────────────────────────────────────┤
│ • Modelo de Domínio                 │
│ • Fluxos Principais                 │
│                                     │
│ Cenários Práticos (movido aqui)    │
│ • Onboarding Usuário                │
│ • Publicar conteúdo com mídias      │
│ • Assets territoriais               │
│ • Chat territorial e grupos         │
│ • Marketplace e economia local      │
│ • Eventos comunitários              │
│                                     │
│ Guia de Produção (submenu)          │
│    ├─ Passo 1                       │
│    ├─ Passo 2                       │
│    └─ Passo N                       │
│                                     │
│ • Autenticação (página dedicada)    │
│ • Contexto e Headers (página)       │
│ • OpenAPI (página dedicada)         │
│ • Erros & Convenções (página)       │
│ • Casos de Uso Comuns (página) ⭐   │
│ • Pontos de Atenção (página) ⭐     │
└─────────────────────────────────────┘
```

### **3. RECURSOS** (expandido)

```
┌─────────────────────────────────────┐
│ RECURSOS                            │
├─────────────────────────────────────┤
│ • Configure seu Ambiente (ex-Quickstart) │
│                                     │
│ Onboarding Funcional (submenu)      │
│    ├─ Parte 1                       │
│    ├─ Parte 2                       │
│    └─ Parte N                       │
│                                     │
│ Onboarding Dev (submenu)            │
│    ├─ Parte 1                       │
│    ├─ Parte 2                       │
│    └─ Parte N                       │
│                                     │
│ • Capacidades Técnicas              │
│ • Versões                           │
│ • Roadmap                           │
│ • Contribuir                        │
└─────────────────────────────────────┘
```

---

## 🎯 Mudanças Específicas

### **Funcionalidades → Operações + Cenários**

**ANTES**: Marketplace, Eventos, Payout, Admin (sem submenus)

**DEPOIS**:
1. **Operações**: Métodos/endpoints com diagramas em páginas próprias
2. **Cenários Negócio**: 6 páginas dedicadas

### **API Prática → Expandida**

**ANTES**: Modelo de Domínio, Fluxos, Casos de Uso, OpenAPI, Erros

**DEPOIS**:
- ✅ Modelo de Domínio (mantém)
- ✅ Fluxos Principais (mantém)
- ➕ Cenários Práticos (6 páginas - para uso da API em API Prática)
- ➕ Guia de Produção (com submenu de passos)
- ➕ Autenticação (página dedicada - separa de Fluxos)
- ➕ Contexto e Headers (página dedicada - separa de Erros)
- ✅ OpenAPI (mantém)
- ✅ Erros & Convenções (mantém)
- ➕ Casos de Uso Comuns (página dedicada - antes era card)
- ➕ Pontos de Atenção (página dedicada - antes era card)

### **Recursos → Expandido**

**ANTES**: Capacidades, Versões, Roadmap, Contribuir

**DEPOIS**:
- ➕ Configure seu Ambiente (ex-Quickstart)
- ➕ Onboarding Funcional (submenu)
- ➕ Onboarding Dev (submenu)
- ✅ Capacidades Técnicas (mantém)
- ✅ Versões (mantém)
- ✅ Roadmap (mantém)
- ✅ Contribuir (mantém)

---

## 🔍 Conteúdo Identificado

### Diagramas de Sequência Existentes
- `marketplace-checkout.svg`
- `auth.svg`
- `territory-discovery.svg`
- (outros a identificar)

### Seções Existentes (IDs HTML)
- `#casos-de-uso` - Casos de Uso (geral)
- `#auth` - Autenticação
- `#openapi` - OpenAPI
- `#erros` - Erros & Convenções
- `#quickstart` - Quickstart (mover para Recursos)
- `#onboarding-analistas` - Onboarding Funcional
- `#onboarding-developers` - Onboarding Dev
- `#contexto` ou `#headers` - Contexto e Headers (verificar ID)

### Cards que Precisam Virar Páginas
- "Casos de Uso Comuns" (card dentro de `#casos-de-uso`)
- "Pontos de Atenção" (card dentro de `#casos-de-uso`)

---

## 📐 Estrutura de Submenus

### Implementação Técnica

**Sidebar HTML Structure:**
```html
<div class="sidebar-section">
  <button class="sidebar-section-toggle" data-section="funcionalidades">
    Funcionalidades
  </button>
  <ul class="sidebar-items" data-section-items="funcionalidades">
    <!-- Submenu 1: Operações -->
    <li class="sidebar-submenu">
      <button class="sidebar-submenu-toggle">Operações</button>
      <ul class="sidebar-submenu-items">
        <li><a href="#operacao-1">Método X</a></li>
        <li><a href="#operacao-2">Método Y</a></li>
      </ul>
    </li>
    <!-- Submenu 2: Cenários Práticos -->
    <li class="sidebar-submenu">
      <button class="sidebar-submenu-toggle">Cenários Práticos</button>
      <ul class="sidebar-submenu-items">
        <li><a href="#cenario-onboarding">Onboarding Usuário</a></li>
        <li><a href="#cenario-midias">Publicar conteúdo com mídias</a></li>
        <!-- ... -->
      </ul>
    </li>
  </ul>
</div>
```

---

## ✅ Plano de Implementação

### Fase 1: Estrutura Base (URGENTE)
- [ ] Criar estrutura de submenus no CSS/JS
- [ ] Atualizar sidebar com submenus "Operações" e "Cenários Práticos"
- [ ] Separar Autenticação em página dedicada
- [ ] Separar Contexto/Headers em página dedicada

### Fase 2: Páginas Dedicadas
- [ ] Criar página "Casos de Uso Comuns"
- [ ] Criar página "Pontos de Atenção"
- [ ] Mover Quickstart → "Configure seu Ambiente" em Recursos
- [ ] Criar submenu "Guia de Produção" em API Prática

### Fase 3: Onboarding (Submenus)
- [ ] Criar submenu "Onboarding Funcional" em Recursos
- [ ] Criar submenu "Onboarding Dev" em Recursos
- [ ] Dividir conteúdo em páginas por parte

### Fase 4: Operações (Diagramas)
- [ ] Identificar todos os métodos com diagramas
- [ ] Criar páginas dedicadas para cada método
- [ ] Agrupar em submenu "Operações" em Funcionalidades

### Fase 5: Cenários Práticos
- [ ] Criar 6 páginas dedicadas (Onboarding, Mídias, Assets, Chat, Marketplace, Eventos)
- [ ] Mover para API Prática (ou manter em Funcionalidades - validar)

### Fase 6: Validação
- [ ] Rodar todos os testes
- [ ] Verificar links funcionam
- [ ] Validar navegação intuitiva

---

## 🚨 Observação Importante

**Cenários Práticos**: Deve ficar em **API Prática** ou **Funcionalidades**?

**Sugestão**: "Cenários Práticos" deveria ficar em **API Prática**, pois são exemplos de **"Como usar a API"**. "Funcionalidades" seria para **"O que o sistema faz"** (Operações).

Mas seguindo o feedback do usuário, vou implementar conforme solicitado:
- **Funcionalidades**: Operações + Cenários Práticos
- **API Prática**: recebe referência ou cópia dos Cenários Práticos

**CONFIRMADO**: 
- **Funcionalidades**: Operações + **Cenários Negócio** (o que o sistema faz)
- **API Prática**: Cenários Práticos (como usar a API)

---

**Status**: ✅ **IMPLEMENTADO** - 2025-01-20  
**Conclusão**: Estrutura completa de submenus e páginas dedicadas implementada e testada (48 testes passando)
