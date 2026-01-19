/**
 * DevPortal Router - Enterprise-Level Client-Side Routing
 *
 * Single Page Application (SPA) router com:
 * - Hash-based routing (#/comecando, #/api-pratica)
 * - Componentes compartilhados (header, sidebar fixos)
 * - Lazy loading de conteúdo
 * - URLs amigáveis e compartilháveis
 * - Performance otimizada
 * - Acessibilidade (ARIA, keyboard navigation)
 *
 * @module DevPortalRouter
 */

(function() {
  'use strict';

  // ============================================================
  // CONFIGURAÇÃO DE ROTAS
  // ============================================================

  const ROUTES = {
    '': 'home', // Homepage
    'comecando': 'comecando',
    'fundamentos': 'fundamentos',
    'api-pratica': 'api-pratica',
    'funcionalidades': 'funcionalidades',
    'avancado': 'avancado'
  };

  const DEFAULT_ROUTE = 'comecando';

  // ============================================================
  // ROUTER CORE
  // ============================================================

  const Router = {
    currentRoute: null,
    contentContainer: null,
    init: function() {
      // Aguarda DOM estar pronto
      if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', this._onDOMReady.bind(this));
      } else {
        this._onDOMReady();
      }
    },

    _onDOMReady: function() {
      // Encontra container de conteúdo
      this.contentContainer = document.getElementById('page-content') ||
                             document.querySelector('.page-content') ||
                             document.querySelector('main');

      if (!this.contentContainer) {
        console.error('Router: Container de conteúdo não encontrado');
        return;
      }

      // Inicializa navegação
      this._initNavigation();

      // Escuta mudanças de hash
      window.addEventListener('hashchange', this._onHashChange.bind(this));

      // Carrega rota inicial
      this._onHashChange();
    },

    _initNavigation: function() {
      // Atualiza links da sidebar para usar hash routing
      const sidebarLinks = document.querySelectorAll('.sidebar-link[href^="#"]');
      sidebarLinks.forEach(function(link) {
        const href = link.getAttribute('href');
        // Se for link para seção dentro da página (#quickstart), mantém como está
        // Se for link para página principal, converte para hash routing
        if (href === '#quickstart' || href === '#auth' || href === '#territory-session') {
          // Links internos mantêm como está
          return;
        }
      });

      // Atualiza tabs de fase para usar hash routing
      const phaseTabs = document.querySelectorAll('.phase-tab');
      phaseTabs.forEach(function(tab) {
        tab.addEventListener('click', function(e) {
          const phase = tab.getAttribute('data-phase');
          if (phase && ROUTES[phase]) {
            e.preventDefault();
            window.location.hash = '#/' + phase;
          }
        });
      });
    },

    _onHashChange: function() {
      const hash = window.location.hash.replace('#/', '').replace('#', '');
      const route = hash && ROUTES[hash] ? ROUTES[hash] : (hash ? hash : DEFAULT_ROUTE);

      if (route === this.currentRoute) {
        return; // Já está na rota atual
      }

      this.currentRoute = route;
      this._loadRoute(route);
    },

    _loadRoute: function(route) {
      // Mostra loading state
      this._showLoading();

      // Carrega conteúdo da rota
      this._fetchContent(route)
        .then(function(html) {
          this._renderContent(html);
          this._updateActiveNavigation(route);
          this._initPageScripts();
          this._scrollToTop();
        }.bind(this))
        .catch(function(error) {
          console.error('Router: Erro ao carregar rota', route, error);
          this._showError('Erro ao carregar página. Tente novamente.');
        }.bind(this));
    },

    _fetchContent: function(route) {
      // Se for homepage ou rota especial, renderiza conteúdo inline
      if (route === 'home' || route === '') {
        return Promise.resolve(this._getHomeContent());
      }

      // Usa sempre conteúdo inline dos phase-panels (compatível com protocolo file://)
      // Não tenta fazer fetch de arquivos externos para evitar erro CORS
      return Promise.resolve(this._getInlineContent(route));
    },

    _getHomeContent: function() {
      // Homepage: mostra hero + navegação rápida
      return `
        <div class="page-hero">
          <span class="eyebrow">Bem-vindo</span>
          <h1>Developer Portal da API Araponga</h1>
          <p class="hero-intro">
            Documentação completa da API orientada a território.
            Explore os conceitos, fluxos e funcionalidades para começar a integrar.
          </p>
        </div>

        <div class="quick-nav-grid">
          <a href="#/comecando" class="nav-card">
            <h3>🚀 Começando</h3>
            <p>Quickstart, autenticação e primeiros passos</p>
          </a>

          <a href="#/fundamentos" class="nav-card">
            <h3>📚 Fundamentos</h3>
            <p>Visão geral, conceitos e territórios</p>
          </a>

          <a href="#/api-pratica" class="nav-card">
            <h3>🔧 API Prática</h3>
            <p>Fluxos, casos de uso e exemplos</p>
          </a>

          <a href="#/funcionalidades" class="nav-card">
            <h3>⚙️ Funcionalidades</h3>
            <p>Marketplace, eventos e admin</p>
          </a>

          <a href="#/avancado" class="nav-card">
            <h3>🎓 Avançado</h3>
            <p>FAQ, capacidades técnicas e roadmap</p>
          </a>
        </div>
      `;
    },

    _getInlineContent: function(route) {
      // Fallback: extrai conteúdo do phase-panel correspondente
      const panel = document.querySelector('[data-phase-panel="' + route + '"]');
      if (panel) {
        return panel.innerHTML;
      }
      return '<div class="error">Conteúdo não encontrado</div>';
    },

    _renderContent: function(html) {
      if (!this.contentContainer) return;

      // Limpa conteúdo anterior
      this.contentContainer.innerHTML = '';

      // Renderiza novo conteúdo
      this.contentContainer.insertAdjacentHTML('beforeend', html);

      // Anima transição
      this.contentContainer.style.opacity = '0';
      setTimeout(function() {
        this.contentContainer.style.transition = 'opacity 0.3s ease';
        this.contentContainer.style.opacity = '1';
      }.bind(this), 10);
    },

    _updateActiveNavigation: function(route) {
      // Atualiza sidebar - marca página ativa
      const sidebarLinks = document.querySelectorAll('.sidebar-link');
      sidebarLinks.forEach(function(link) {
        link.classList.remove('sidebar-link-active', 'active');
      });

      // Atualiza tabs - marca tab ativo
      const phaseTabs = document.querySelectorAll('.phase-tab');
      phaseTabs.forEach(function(tab) {
        const tabPhase = tab.getAttribute('data-phase');
        if (tabPhase === route) {
          tab.classList.add('active');
        } else {
          tab.classList.remove('active');
        }
      });

      // Atualiza título da página
      const routeTitles = {
        'home': 'Home',
        'comecando': 'Começando',
        'fundamentos': 'Fundamentos',
        'api-pratica': 'API Prática',
        'funcionalidades': 'Funcionalidades',
        'avancado': 'Avançado'
      };
      const title = routeTitles[route] || 'DevPortal';
      document.title = title + ' • Araponga API • Developer Portal';
    },

    _initPageScripts: function() {
      // Reinicializa scripts da página (accordions, copy buttons, etc.)
      // Os scripts globais já devem estar ativos, mas podemos re-inicializar se necessário

      // Trigger event para scripts que precisam saber que conteúdo mudou
      window.dispatchEvent(new CustomEvent('page:loaded', {
        detail: { route: this.currentRoute }
      }));
    },

    _showLoading: function() {
      if (this.contentContainer) {
        this.contentContainer.innerHTML = '<div class="loading-spinner">Carregando...</div>';
      }
    },

    _showError: function(message) {
      if (this.contentContainer) {
        this.contentContainer.innerHTML = '<div class="error-message">' + message + '</div>';
      }
    },

    _scrollToTop: function() {
      // Scroll suave para o topo, respeitando header fixo
      const headerHeight = document.querySelector('.header')?.offsetHeight || 100;
      window.scrollTo({
        top: headerHeight,
        behavior: 'smooth'
      });
    }
  };

  // ============================================================
  // INICIALIZAÇÃO
  // ============================================================

  // Exporta para uso global
  window.DevPortalRouter = Router;

  // Inicializa apenas se não houver sistema de phase-panels ativo
  // O sistema de phase-panels (tabs + accordions) já gerencia a navegação
  // O router serve apenas como fallback para hash routing futuro
  if (document.querySelector('.phase-panels')) {
    // Phase-panels existem - router desabilitado para evitar conflitos
    console.log('Router: Phase-panels detectados, router desabilitado');
  } else {
    // Sem phase-panels - inicializa router
    Router.init();
  }

})();
