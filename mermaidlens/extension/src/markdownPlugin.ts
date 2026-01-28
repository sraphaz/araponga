import type MarkdownIt from "markdown-it";
import { applyPreset, getPresetById, getThemeById, getDefaultTheme } from "@mermaidlens/core";
import { injectAssets } from "./injectAssets";

interface MermaidLensPluginOptions {
  getThemeId: () => string;
  getPresetId: () => string;
}

export function markdownPlugin(md: MarkdownIt, options: MermaidLensPluginOptions): void {
  const defaultFence = md.renderer.rules.fence ?? ((tokens, idx, opts, env, self) => self.renderToken(tokens, idx, opts));

  md.renderer.rules.fence = (tokens, idx, opts, env, self) => {
    const token = tokens[idx];
    const info = token.info.trim();

    if (info !== "mermaid") {
      return defaultFence(tokens, idx, opts, env, self);
    }

    const preset = getPresetById(options.getPresetId());
    const theme = getThemeById(options.getThemeId()) ?? getDefaultTheme();
    const mermaidCode = applyPreset(token.content.trim(), preset);
    const escapedCode = md.utils.escapeHtml(mermaidCode);

    const assets = env.mermaidlensInjected ? "" : injectAssets(theme);
    env.mermaidlensInjected = true;

    return `${assets}<div class="mermaidlens-diagram"><div class="mermaid">${escapedCode}</div></div>`;
  };
}
