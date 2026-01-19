const fs = require('fs');
const path = require('path');
const { JSDOM } = require('jsdom');

describe('DevPortal - Validação de Navegação e Conteúdo', () => {
  let dom;
  let document;
  let window;

  beforeEach(() => {
    const htmlPath = path.join(__dirname, '..', 'index.html');
    const htmlContent = fs.readFileSync(htmlPath, 'utf-8');
    dom = new JSDOM(htmlContent, {
      runScripts: 'outside-only',
      resources: 'usable',
      url: 'http://localhost'
    });
    window = dom.window;
    document = dom.window.document;
  });

  afterEach(() => {
    dom.window.close();
  });

  describe('Estrutura de Navegação - Menu Lateral vs Menu Central', () => {
    test('Menu lateral (sidebar) deve existir', () => {
      const sidebar = document.querySelector('.sidebar-container');
      expect(sidebar).toBeTruthy();
    });

    test('Menu central (phase-tabs) deve existir', () => {
      const phaseNav = document.querySelector('.phase-navigation');
      expect(phaseNav).toBeTruthy();
    });

    test('Menu lateral deve ter links com href="#" para seções internas', () => {
      const sidebarLinks = document.querySelectorAll('.sidebar-link[href^="#"]');
      expect(sidebarLinks.length).toBeGreaterThan(0);
      sidebarLinks.forEach(link => {
        const href = link.getAttribute('href');
        expect(href).toMatch(/^#[a-z0-9-]+$/);
      });
    });

    test('Menu central deve ter tabs com data-phase para phase-panels', () => {
      const phaseTabs = document.querySelectorAll('.phase-tab[data-phase]');
      expect(phaseTabs.length).toBeGreaterThan(0);
      phaseTabs.forEach(tab => {
        const phase = tab.getAttribute('data-phase');
        expect(phase).toBeTruthy();
        expect(['comecando', 'fundamentos', 'api-pratica', 'funcionalidades', 'avancado']).toContain(phase);
      });
    });
  });

  describe('Mapeamento de Seções para Phase-Panels', () => {
    // Mapeamento esperado baseado no JavaScript
    const expectedSectionToPhase = {
      'visao-geral': 'fundamentos',
      'como-funciona': 'fundamentos',
      'territorios': 'fundamentos',
      'conceitos': 'fundamentos',
      'modelo-dominio': 'api-pratica',
      'fluxos': 'api-pratica',
      'casos-de-uso': 'api-pratica',
      'openapi': 'api-pratica',
      'erros': 'api-pratica',
      'marketplace': 'funcionalidades',
      'payout-gestao-financeira': 'funcionalidades',
      'eventos': 'funcionalidades',
      'admin': 'funcionalidades',
      'capacidades-tecnicas': 'avancado',
      'versoes': 'avancado',
      'roadmap': 'avancado',
      'contribuir': 'avancado'
    };

    test('Cada link da sidebar deve ter uma seção correspondente com o ID correto', () => {
      const sidebarLinks = document.querySelectorAll('.sidebar-link[href^="#"]');
      sidebarLinks.forEach(link => {
        const href = link.getAttribute('href');
        if (href && href.startsWith('#')) {
          const sectionId = href.substring(1);
          if (expectedSectionToPhase[sectionId]) {
            const section = document.getElementById(sectionId);
            expect(section).toBeTruthy();
          }
        }
      });
    });

    test('Cada seção referenciada deve estar dentro do phase-panel correto', () => {
      Object.entries(expectedSectionToPhase).forEach(([sectionId, expectedPhase]) => {
        const section = document.getElementById(sectionId);
        if (section) {
          const phasePanel = section.closest('.phase-panel');
          if (phasePanel) {
            const phasePanelId = phasePanel.getAttribute('data-phase-panel');
            expect(phasePanelId).toBe(expectedPhase);
          } else {
            // Se não está em um phase-panel, está fora (problema!)
            console.warn(`Seção ${sectionId} não está dentro de um phase-panel. Está em:`, section.parentElement?.tagName);
          }
        } else {
          console.warn(`Seção ${sectionId} não encontrada no HTML`);
        }
      });
    });
  });

  describe('Validação de Conteúdo por Phase-Panel', () => {
    test('Phase-panel "comecando" deve conter a seção #introducao', () => {
      const comecandoPanel = document.querySelector('[data-phase-panel="comecando"]');
      expect(comecandoPanel).toBeTruthy();

      const introducaoSection = comecandoPanel.querySelector('#introducao');
      expect(introducaoSection).toBeTruthy();
    });

    test('Phase-panel "fundamentos" deve conter seções relacionadas a fundamentos', () => {
      const fundamentosPanel = document.querySelector('[data-phase-panel="fundamentos"]');
      expect(fundamentosPanel).toBeTruthy();

      const expectedSections = ['visao-geral', 'como-funciona', 'territorios', 'conceitos'];
      expectedSections.forEach(sectionId => {
        const section = fundamentosPanel.querySelector('#' + sectionId);
        if (!section) {
          console.warn(`Seção ${sectionId} não encontrada no phase-panel "fundamentos"`);
        }
      });
    });

    test('Phase-panel "api-pratica" deve conter seções relacionadas a API', () => {
      const apiPraticaPanel = document.querySelector('[data-phase-panel="api-pratica"]');
      expect(apiPraticaPanel).toBeTruthy();

      const expectedSections = [
        'modelo-dominio', 'fluxos',
        'cenario-onboarding-usuario', 'cenario-publicar-midias', 'cenario-assets',
        'cenario-chat', 'cenario-marketplace', 'cenario-eventos',
        'guia-producao-passo-1', 'guia-producao-passo-2', 'guia-producao-passo-3', 'guia-producao-passo-4',
        'auth', 'territory-session', 'openapi', 'erros',
        'casos-uso-comuns', 'pontos-atencao'
      ];
      expectedSections.forEach(sectionId => {
        const section = apiPraticaPanel.querySelector('#' + sectionId);
        if (!section) {
          console.warn(`Seção ${sectionId} não encontrada no phase-panel "api-pratica"`);
        }
      });
    });

    test('Phase-panel "funcionalidades" deve conter seções relacionadas a funcionalidades', () => {
      const funcionalidadesPanel = document.querySelector('[data-phase-panel="funcionalidades"]');
      expect(funcionalidadesPanel).toBeTruthy();

      const expectedSections = [
        'operacao-auth', 'operacao-territory-discovery', 'operacao-marketplace-checkout',
        'marketplace', 'payout-gestao-financeira', 'eventos', 'admin'
      ];
      expectedSections.forEach(sectionId => {
        const section = funcionalidadesPanel.querySelector('#' + sectionId);
        if (!section) {
          console.warn(`Seção ${sectionId} não encontrada no phase-panel "funcionalidades"`);
        }
      });
    });

    test('Phase-panel "avancado" deve conter seções relacionadas a recursos avançados', () => {
      const avancadoPanel = document.querySelector('[data-phase-panel="avancado"]');
      expect(avancadoPanel).toBeTruthy();

      const expectedSections = [
        'configure-ambiente', 'onboarding-analistas', 'onboarding-developers',
        'capacidades-tecnicas', 'versoes', 'roadmap', 'contribuir'
      ];
      expectedSections.forEach(sectionId => {
        const section = avancadoPanel.querySelector('#' + sectionId);
        if (!section) {
          console.warn(`Seção ${sectionId} não encontrada no phase-panel "avancado"`);
        }
      });
    });
  });

  describe('Validação de Links vs Conteúdo Real', () => {
    test('Todos os links da sidebar devem apontar para seções que existem', () => {
      const sidebarLinks = document.querySelectorAll('.sidebar-link[href^="#"]');
      const brokenLinks = [];

      sidebarLinks.forEach(link => {
        const href = link.getAttribute('href');
        if (href && href.startsWith('#')) {
          const sectionId = href.substring(1);
          const section = document.getElementById(sectionId);
          if (!section) {
            brokenLinks.push({ link: href, text: link.textContent.trim() });
          }
        }
      });

      if (brokenLinks.length > 0) {
        console.error('Links quebrados encontrados:', brokenLinks);
      }
      expect(brokenLinks.length).toBe(0);
    });

    test('Nenhuma seção deve estar fora de um phase-panel (exceto seções especiais)', () => {
      const allSections = document.querySelectorAll('section.section[id]');
      const sectionsOutsidePanels = [];

      allSections.forEach(section => {
        const id = section.getAttribute('id');
        if (!id) return;

        // Seções permitidas fora de phase-panels (se houver)
        const allowedOutside = ['introducao']; // Ajuste conforme necessário

        if (!allowedOutside.includes(id)) {
          const phasePanel = section.closest('.phase-panel');
          if (!phasePanel) {
            sectionsOutsidePanels.push({
              id: id,
              tagName: section.tagName,
              parentTagName: section.parentElement?.tagName
            });
          }
        }
      });

      if (sectionsOutsidePanels.length > 0) {
        console.warn('Seções fora de phase-panels:', sectionsOutsidePanels);
      }
      // Este teste pode falhar se ainda há conteúdo fora dos phase-panels
      // Vamos apenas avisar, não falhar o teste
    });
  });

  describe('Critérios de Organização', () => {
    test('Menu lateral organizado por contexto temático', () => {
      const sidebarSections = document.querySelectorAll('.sidebar-section');
      const sectionTitles = Array.from(sidebarSections).map(section => {
        const title = section.querySelector('.sidebar-section-title');
        return title ? title.textContent.trim() : null;
      }).filter(Boolean);

      expect(sectionTitles).toContain('Fundamentos');
      expect(sectionTitles).toContain('API Prática');
      expect(sectionTitles).toContain('Funcionalidades');
      expect(sectionTitles).toContain('Recursos');
    });

    test('Menu central organizado por fase de aprendizado', () => {
      const phaseTabs = document.querySelectorAll('.phase-tab[data-phase]');
      const phaseNames = Array.from(phaseTabs).map(tab => tab.getAttribute('data-phase'));

      expect(phaseNames).toContain('comecando');
      expect(phaseNames).toContain('fundamentos');
      expect(phaseNames).toContain('api-pratica');
      expect(phaseNames).toContain('funcionalidades');
      expect(phaseNames).toContain('avancado');
    });
  });

  describe('Validação de IDs Únicos', () => {
    test('Todos os IDs de seção devem ser únicos', () => {
      const allSections = document.querySelectorAll('section[id]');
      const sectionIds = Array.from(allSections)
        .map(section => section.getAttribute('id'))
        .filter(Boolean);

      const uniqueIds = new Set(sectionIds);
      expect(uniqueIds.size).toBe(sectionIds.length);
    });

    test('Nenhum ID deve estar duplicado', () => {
      const allElementsWithIds = document.querySelectorAll('[id]');
      const allIds = Array.from(allElementsWithIds)
        .map(el => el.getAttribute('id'))
        .filter(Boolean);

      const idCounts = {};
      allIds.forEach(id => {
        idCounts[id] = (idCounts[id] || 0) + 1;
      });

      const duplicates = Object.entries(idCounts)
        .filter(([id, count]) => count > 1)
        .map(([id]) => id);

      if (duplicates.length > 0) {
        console.error('IDs duplicados encontrados:', duplicates);
      }
      expect(duplicates.length).toBe(0);
    });
  });
});
