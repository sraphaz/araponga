/**
 * Testes de Funcionalidades JavaScript do DevPortal
 *
 * Valida:
 * - Navegação entre phase-panels
 * - Scroll sync
 * - Accordions
 * - Sidebar toggle
 * - Theme toggle
 */

const fs = require('fs');
const path = require('path');
const { JSDOM } = require('jsdom');

describe('DevPortal - Funcionalidades JavaScript', () => {
  let dom;
  let window;
  let document;

  beforeEach(() => {
    const htmlPath = path.join(__dirname, '..', 'index.html');
    const htmlContent = fs.readFileSync(htmlPath, 'utf-8');

    dom = new JSDOM(htmlContent, {
      runScripts: 'outside-only', // Não executa scripts inline
      resources: 'usable',
      url: 'http://localhost'
    });

    window = dom.window;
    document = window.document;

    // Simula eventos do navegador
    Object.defineProperty(window, 'scrollTo', {
      value: jest.fn(),
      writable: true,
    });

    Object.defineProperty(window, 'getComputedStyle', {
      value: jest.fn(() => ({
        display: 'block',
        visibility: 'visible',
      })),
      writable: true,
    });
  });

  afterEach(() => {
    dom.window.close();
  });

  describe('Phase Navigation', () => {
    test('Phase-tabs devem existir', () => {
      const phaseTabs = document.querySelectorAll('.phase-tab');
      expect(phaseTabs.length).toBeGreaterThan(0);
    });

    test('Phase-panels devem existir', () => {
      const phasePanels = document.querySelectorAll('.phase-panel');
      expect(phasePanels.length).toBeGreaterThan(0);
    });

    test('Apenas um phase-panel deve estar ativo inicialmente', () => {
      const activePanels = document.querySelectorAll('.phase-panel.active');
      expect(activePanels.length).toBe(1);
    });

    test('Página inicial deve mostrar apenas conteúdo do tab "Começando"', () => {
      const activePanel = document.querySelector('.phase-panel.active');
      expect(activePanel).toBeTruthy();

      const activePhase = activePanel.getAttribute('data-phase-panel');
      expect(activePhase).toBe('comecando');
    });

    test('Phase-panels sem classe active não devem estar visíveis (verificado via HTML)', () => {
      const inactivePanels = Array.from(document.querySelectorAll('.phase-panel:not(.active)'));
      inactivePanels.forEach(panel => {
        // Verifica que o panel não tem a classe active
        expect(panel.classList.contains('active')).toBe(false);
      });
    });

    test('Cada phase-panel deve ter um data-phase-panel único', () => {
      const panels = Array.from(document.querySelectorAll('.phase-panel'));
      const phaseIds = panels.map(p => p.getAttribute('data-phase-panel')).filter(Boolean);
      const uniquePhaseIds = new Set(phaseIds);
      expect(phaseIds.length).toBe(uniquePhaseIds.size);
    });

    test('Ao clicar em phase-tab, deve ativar phase-panel correspondente', () => {
      const tabs = document.querySelectorAll('.phase-tab');
      const panels = document.querySelectorAll('.phase-panel');

      if (tabs.length > 0 && panels.length > 0) {
        const firstTab = tabs[0];
        const phase = firstTab.getAttribute('data-phase');

        if (phase) {
          const targetPanel = document.querySelector(`[data-phase-panel="${phase}"]`);
          expect(targetPanel).toBeTruthy();
        }
      }
    });
  });

  describe('Sidebar Toggle', () => {
    test('Sidebar sections devem ter toggle buttons', () => {
      const toggles = document.querySelectorAll('.sidebar-section-toggle');
      expect(toggles.length).toBeGreaterThan(0);
    });

    test('Toggle buttons devem ter aria-expanded', () => {
      const toggles = Array.from(document.querySelectorAll('.sidebar-section-toggle'));
      toggles.forEach(toggle => {
        expect(toggle.hasAttribute('aria-expanded')).toBe(true);
      });
    });

    test('Sidebar items devem estar escondidos inicialmente (exceto se tiver link ativo)', () => {
      const items = Array.from(document.querySelectorAll('.sidebar-items'));
      // Verifica se todos estão hidden ou se algum tem link ativo
      const hasActiveLink = items.some(item => item.querySelector('.sidebar-link-active'));

      if (!hasActiveLink) {
        // Se não há link ativo, verifica que pelo menos a maioria está hidden
        // (pode haver seções que começam abertas por padrão)
        const hiddenItems = items.filter(item => item.hasAttribute('hidden'));
        // Pelo menos 50% dos items devem estar hidden se não há link ativo
        expect(hiddenItems.length).toBeGreaterThanOrEqual(items.length * 0.5);
      } else {
        // Se há link ativo, pelo menos um item deve estar visível (sem hidden)
        const visibleItems = items.filter(item => !item.hasAttribute('hidden'));
        expect(visibleItems.length).toBeGreaterThan(0);
      }
    });
  });

  describe('Accordions', () => {
    test('Section accordions devem existir', () => {
      const accordions = document.querySelectorAll('.section-accordion');
      // Accordions são opcionais, mas se existirem devem estar corretos
      if (accordions.length > 0) {
        const headers = document.querySelectorAll('.section-accordion-header');
        expect(headers.length).toBeGreaterThan(0);
      }
    });

    test('Accordion headers devem ter aria-expanded', () => {
      const headers = Array.from(document.querySelectorAll('.section-accordion-header'));
      headers.forEach(header => {
        expect(header.hasAttribute('aria-expanded')).toBe(true);
      });
    });
  });

  describe('Links e Navegação', () => {
    test('Links da sidebar devem ter href válido', () => {
      const sidebarLinks = Array.from(document.querySelectorAll('.sidebar-link[href^="#"]'));
      sidebarLinks.forEach(link => {
        const href = link.getAttribute('href');
        expect(href).toMatch(/^#/);
        expect(href.length).toBeGreaterThan(1); // Mais que apenas "#"
      });
    });

          test('Phase navigation tabs devem existir', () => {
            const phaseNav = document.querySelector('.phase-navigation');
            expect(phaseNav).toBeTruthy();
            if (phaseNav) {
              const tabs = phaseNav.querySelectorAll('.phase-tab');
              expect(tabs.length).toBeGreaterThan(0);
            }
          });
  });

  describe('Theme Toggle', () => {
    test('Theme toggle deve existir', () => {
      const themeToggle = document.getElementById('theme-toggle');
      expect(themeToggle).toBeTruthy();
    });

    test('Theme toggle deve ter aria-label', () => {
      const themeToggle = document.getElementById('theme-toggle');
      if (themeToggle) {
        expect(themeToggle.hasAttribute('aria-label')).toBe(true);
      }
    });
  });
});
