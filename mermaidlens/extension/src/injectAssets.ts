import type { MermaidTheme } from "@mermaidlens/core";

export function injectAssets(theme?: MermaidTheme): string {
  const mermaidConfig = theme?.mermaid ?? {
    theme: "base",
    themeVariables: {}
  };

  const configJson = JSON.stringify({
    startOnLoad: false,
    theme: mermaidConfig.theme,
    themeVariables: mermaidConfig.themeVariables
  });

  return `
<style>
.mermaidlens-diagram {
  background: #001F3F;
  border-radius: 12px;
  padding: 16px;
  margin: 12px 0;
  box-shadow: 0 0 12px rgba(0, 207, 255, 0.25);
}
.mermaidlens-diagram .mermaid {
  color: #E0F7FF;
}
</style>
<script src="https://cdn.jsdelivr.net/npm/mermaid@10/dist/mermaid.min.js"></script>
<script>
(() => {
  const config = ${configJson};
  const initMermaid = () => {
    if (!window.mermaid) {
      return;
    }
    window.mermaid.initialize(config);
    window.mermaid.run({ querySelector: '.mermaidlens-diagram .mermaid' });
  };
  if (document.readyState === 'complete') {
    initMermaid();
  } else {
    window.addEventListener('load', initMermaid);
  }
})();
</script>
`;
}
