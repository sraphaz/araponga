import fs from "fs";
import path from "path";

export interface MermaidTheme {
  id: string;
  name: string;
  author?: string;
  version?: string;
  mermaid: {
    theme: string;
    themeVariables: Record<string, string>;
  };
}

let themes: MermaidTheme[] = [];

export const defaultThemeId = "ocean";

export function loadThemes(basePath: string): MermaidTheme[] {
  const themeRoot = path.resolve(basePath);
  if (!fs.existsSync(themeRoot)) {
    themes = [];
    return themes;
  }

  const entries = fs.readdirSync(themeRoot, { withFileTypes: true });
  themes = entries
    .filter((entry) => entry.isDirectory())
    .map((entry) => path.join(themeRoot, entry.name, "theme.json"))
    .filter((themePath) => fs.existsSync(themePath))
    .map((themePath) => {
      const raw = fs.readFileSync(themePath, "utf-8");
      return JSON.parse(raw) as MermaidTheme;
    });

  return themes;
}

export function getThemeById(id: string): MermaidTheme | undefined {
  return themes.find((theme) => theme.id === id);
}

export function getDefaultTheme(): MermaidTheme | undefined {
  return getThemeById(defaultThemeId) ?? themes[0];
}
