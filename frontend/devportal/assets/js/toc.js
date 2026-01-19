/**
 * Table of Contents (TOC) - Similar à Wiki
 * Gera TOC dinamicamente para phase-panels com múltiplos headings
 */

(function initTableOfContents() {
  'use strict';

  // Configuração
  const TOC_MIN_HEADINGS = 2; // Mínimo de headings para mostrar TOC
  const HEADING_LEVELS = [2, 3, 4]; // H2, H3, H4
  const SCROLL_OFFSET = 100; // Offset para compensar sticky header

  /**
   * Extrai headings de um elemento
   */
  function extractHeadings(container) {
    const headings = [];
    const headingElements = container.querySelectorAll('h2[id], h3[id], h4[id]');

    headingElements.forEach(function(heading) {
      const id = heading.id;
      const level = parseInt(heading.tagName.charAt(1));
      const text = heading.textContent.trim();

      if (id && text && HEADING_LEVELS.indexOf(level) !== -1) {
        headings.push({
          id: id,
          text: text,
          level: level
        });
      }
    });

    return headings;
  }

  /**
   * Gera HTML do TOC
   */
  function generateTOCHTML(headings) {
    if (headings.length === 0) {
      return '';
    }

    var html = '<nav class="toc-container" aria-label="Índice do documento">';
    html += '<div class="toc-header">';
    html += '<h3 class="toc-title">Neste Documento</h3>';
    html += '</div>';
    html += '<ul class="toc-list">';

    headings.forEach(function(item, index) {
      var prevItem = index > 0 ? headings[index - 1] : null;
      var isGroupStart = item.level === 2 || (prevItem && prevItem.level === 2 && item.level > 2);

      var className = 'toc-item toc-item-level-' + item.level;
      if (isGroupStart) {
        className += ' toc-group-start';
      }

      html += '<li class="' + className + '">';
      html += '<button class="toc-link" data-toc-target="' + item.id + '">';
      html += escapeHtml(item.text);
      html += '</button>';
      html += '</li>';
    });

    html += '</ul>';
    html += '</nav>';

    return html;
  }

  /**
   * Escapa HTML para segurança
   */
  function escapeHtml(text) {
    var div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
  }

  /**
   * Rola para um heading
   */
  function scrollToHeading(id) {
    var element = document.getElementById(id);
    if (element) {
      var elementPosition = element.getBoundingClientRect().top;
      var offsetPosition = elementPosition + window.pageYOffset - SCROLL_OFFSET;

      window.scrollTo({
        top: offsetPosition,
        behavior: 'smooth'
      });
    }
  }

  /**
   * Atualiza item ativo no TOC usando IntersectionObserver
   */
  function initTOCScrollSync(tocContainer, headings) {
    if (headings.length === 0) return;

    var observerOptions = {
      root: null,
      rootMargin: '-' + SCROLL_OFFSET + 'px 0px -60% 0px',
      threshold: [0, 0.1, 0.25, 0.5, 0.75, 1]
    };

    var observer = new IntersectionObserver(function(entries) {
      var headingsWithPosition = entries
        .map(function(entry) {
          return {
            id: entry.target.id,
            top: entry.boundingClientRect.top,
            isIntersecting: entry.isIntersecting,
            intersectionRatio: entry.intersectionRatio
          };
        })
        .sort(function(a, b) {
          return a.top - b.top;
        });

      var visibleHeading = headingsWithPosition.find(function(h) {
        return h.isIntersecting && h.top >= SCROLL_OFFSET;
      });

      if (visibleHeading) {
        updateActiveTOCItem(tocContainer, visibleHeading.id);
        return;
      }

      var pastTop = headingsWithPosition.filter(function(h) {
        return h.top < SCROLL_OFFSET;
      });

      if (pastTop.length > 0) {
        updateActiveTOCItem(tocContainer, pastTop[pastTop.length - 1].id);
      }
    }, observerOptions);

    headings.forEach(function(item) {
      var element = document.getElementById(item.id);
      if (element) {
        observer.observe(element);
      }
    });

    // Fallback: scroll listener
    var handleScroll = function() {
      var headingElements = headings.map(function(item) {
        return document.getElementById(item.id);
      }).filter(Boolean);

      if (headingElements.length === 0) return;

      var current = '';

      for (var i = headingElements.length - 1; i >= 0; i--) {
        var heading = headingElements[i];
        var rect = heading.getBoundingClientRect();

        if (rect.top <= SCROLL_OFFSET + 50) {
          current = heading.id;
          break;
        }
      }

      if (current) {
        updateActiveTOCItem(tocContainer, current);
      }
    };

    window.addEventListener('scroll', handleScroll, { passive: true });
    handleScroll(); // Initial check
  }

  /**
   * Atualiza item ativo no TOC
   */
  function updateActiveTOCItem(tocContainer, activeId) {
    var links = tocContainer.querySelectorAll('.toc-link');
    links.forEach(function(link) {
      var item = link.parentElement;
      if (link.getAttribute('data-toc-target') === activeId) {
        item.classList.add('toc-item-active');
      } else {
        item.classList.remove('toc-item-active');
      }
    });
  }

  /**
   * Inicializa TOC para um phase-panel
   */
  function initTOCForPanel(panel) {
    // Extrai headings do panel
    var headings = extractHeadings(panel);

    // Se não há headings suficientes, não cria TOC
    if (headings.length < TOC_MIN_HEADINGS) {
      return;
    }

    // Gera HTML do TOC
    var tocHTML = generateTOCHTML(headings);

    if (!tocHTML) {
      return;
    }

    // Cria wrapper para grid com TOC
    var contentWrapper = document.createElement('div');
    contentWrapper.className = 'phase-panel-content';

    // Move todo o conteúdo do panel para o wrapper
    while (panel.firstChild) {
      contentWrapper.appendChild(panel.firstChild);
    }

    // Cria container do TOC
    var tocWrapper = document.createElement('aside');
    tocWrapper.className = 'phase-panel-toc';
    tocWrapper.innerHTML = tocHTML;

    // Cria grid container
    var gridContainer = document.createElement('div');
    gridContainer.className = 'phase-panel-with-toc';
    gridContainer.appendChild(contentWrapper);
    gridContainer.appendChild(tocWrapper);

    // Adiciona grid ao panel
    panel.appendChild(gridContainer);

    // Inicializa scroll sync
    var tocContainer = tocWrapper.querySelector('.toc-container');
    initTOCScrollSync(tocContainer, headings);

    // Adiciona event listeners aos links do TOC
    var tocLinks = tocContainer.querySelectorAll('.toc-link');
    tocLinks.forEach(function(link) {
      link.addEventListener('click', function(e) {
        e.preventDefault();
        var targetId = link.getAttribute('data-toc-target');
        if (targetId) {
          scrollToHeading(targetId);
        }
      });
    });
  }

  /**
   * Inicializa TOC para todos os phase-panels
   */
  function initAllTOCs() {
    var panels = document.querySelectorAll('.phase-panel');
    panels.forEach(function(panel) {
      initTOCForPanel(panel);
    });
  }

  // Inicializa quando DOM estiver pronto
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initAllTOCs);
  } else {
    initAllTOCs();
  }
})();
