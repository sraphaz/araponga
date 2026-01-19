/**
 * Sistema de busca para DevPortal
 * Busca instantânea em seções do DevPortal usando Fuse.js
 */

// Índice de seções do DevPortal
function generateDevPortalIndex() {
  const sections = Array.from(document.querySelectorAll('.section'));
  const index = [];

  sections.forEach((section) => {
    const id = section.id;
    if (!id) return;

    const h2 = section.querySelector('h2');
    const title = h2 ? h2.textContent.trim() : '';
    
    // Extrai texto de toda a seção
    const content = section.textContent || '';
    
    // Determina categoria baseado na estrutura
    const category = getCategoryFromSection(section);

    index.push({
      id,
      title,
      content: content.substring(0, 500), // Primeiros 500 caracteres
      url: `#${id}`,
      category,
    });
  });

  return index;
}

/**
 * Determina categoria da seção baseado na estrutura
 */
function getCategoryFromSection(section) {
  // Verifica se está dentro de uma phase-panel
  const phasePanel = section.closest('.phase-panel');
  if (phasePanel) {
    const phaseTitle = phasePanel.querySelector('.phase-title');
    if (phaseTitle) {
      return phaseTitle.textContent.trim();
    }
  }

  // Verifica seção especial
  if (section.id.startsWith('operacao-')) return 'Operações';
  if (section.id.startsWith('cenario-')) return 'Cenários';
  if (section.id.startsWith('guia-producao-')) return 'Guia de Produção';
  if (section.id === 'modelo-dominio') return 'Fundamentos';
  if (section.id === 'fluxos') return 'Fluxos';
  if (section.id === 'casos-de-uso') return 'Casos de Uso';

  return 'Outros';
}

/**
 * Inicializa busca no DevPortal
 */
function initSearch() {
  // Carrega Fuse.js dinamicamente
  if (typeof Fuse === 'undefined') {
    const script = document.createElement('script');
    script.src = 'https://cdn.jsdelivr.net/npm/fuse.js@7.0.0/dist/fuse.min.js';
    script.onload = () => setupSearch();
    document.head.appendChild(script);
  } else {
    setupSearch();
  }
}

/**
 * Configura busca com Fuse.js
 */
function setupSearch() {
  const index = generateDevPortalIndex();
  let fuse = null;

  // Cria dialog de busca
  const searchDialog = createSearchDialog();

  // Cria trigger de busca
  const searchTrigger = createSearchTrigger();

  // Busca quando query muda
  let searchTimeout;
  const searchInput = searchDialog.querySelector('#search-input');
  
  searchInput.addEventListener('input', (e) => {
    const query = e.target.value.trim();

    clearTimeout(searchTimeout);
    searchTimeout = setTimeout(() => {
      if (!query) {
        showResults([]);
        return;
      }

      if (!fuse) {
        fuse = new Fuse(index, {
          keys: [
            { name: 'title', weight: 0.7 },
            { name: 'content', weight: 0.3 },
          ],
          threshold: 0.4,
          distance: 100,
          minMatchCharLength: 2,
        });
      }

      const results = fuse.search(query).slice(0, 10).map(r => r.item);
      showResults(results);
    }, 150);
  });

  // Atalho Cmd/Ctrl + K
  document.addEventListener('keydown', (e) => {
    if ((e.metaKey || e.ctrlKey) && e.key === 'k') {
      e.preventDefault();
      openSearch();
    }
    if (e.key === 'Escape' && searchDialog.classList.contains('open')) {
      closeSearch();
    }
  });
}

/**
 * Cria dialog de busca
 */
function createSearchDialog() {
  const dialog = document.createElement('div');
  dialog.id = 'search-dialog';
  dialog.className = 'search-dialog';
  dialog.innerHTML = `
    <div class="search-dialog-overlay"></div>
    <div class="search-dialog-content">
      <div class="search-dialog-header">
        <input
          id="search-input"
          type="text"
          placeholder="Buscar no DevPortal... (Cmd/Ctrl + K)"
          autocomplete="off"
        />
        <button class="search-dialog-close" aria-label="Fechar busca">
          <svg width="20" height="20" viewBox="0 0 20 20" fill="none">
            <path d="M15 5L5 15M5 5l10 10" stroke="currentColor" stroke-width="2" stroke-linecap="round"/>
          </svg>
        </button>
      </div>
      <div id="search-results" class="search-dialog-results"></div>
    </div>
  `;

  document.body.appendChild(dialog);

  // Event listeners
  dialog.querySelector('.search-dialog-overlay').addEventListener('click', closeSearch);
  dialog.querySelector('.search-dialog-close').addEventListener('click', closeSearch);

  return dialog;
}

/**
 * Cria trigger de busca
 */
function createSearchTrigger() {
  const header = document.querySelector('header');
  if (!header) return;

  const trigger = document.createElement('button');
  trigger.className = 'search-trigger';
  trigger.innerHTML = `
    <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
      <path d="M7 12A5 5 0 1 0 7 2a5 5 0 0 0 0 10zM13 13l-3-3" stroke="currentColor" stroke-width="2" stroke-linecap="round"/>
    </svg>
    <span>Buscar</span>
    <kbd>⌘K</kbd>
  `;
  trigger.addEventListener('click', openSearch);

  const nav = header.querySelector('nav');
  if (nav) {
    nav.insertBefore(trigger, nav.firstChild);
  }

  return trigger;
}

/**
 * Abre dialog de busca
 */
function openSearch() {
  const dialog = document.getElementById('search-dialog');
  if (dialog) {
    dialog.classList.add('open');
    const input = dialog.querySelector('#search-input');
    if (input) {
      input.focus();
    }
  }
}

/**
 * Fecha dialog de busca
 */
function closeSearch() {
  const dialog = document.getElementById('search-dialog');
  if (dialog) {
    dialog.classList.remove('open');
    const input = dialog.querySelector('#search-input');
    if (input) {
      input.value = '';
      showResults([]);
    }
  }
}

/**
 * Mostra resultados da busca
 */
function showResults(results) {
  const resultsContainer = document.getElementById('search-results');
  if (!resultsContainer) return;

  if (results.length === 0) {
    resultsContainer.innerHTML = '<div class="search-empty">Digite para buscar...</div>';
    return;
  }

  resultsContainer.innerHTML = results
    .map(
      (result) => `
    <a href="${result.url}" class="search-result">
      <div class="search-result-icon">📄</div>
      <div class="search-result-content">
        <h3>${result.title}</h3>
        ${result.category ? `<span class="search-result-category">${result.category}</span>` : ''}
      </div>
      <div class="search-result-arrow">→</div>
    </a>
  `
    )
    .join('');
}

// Inicializa busca quando DOM estiver pronto
if (document.readyState === 'loading') {
  document.addEventListener('DOMContentLoaded', initSearch);
} else {
  initSearch();
}
