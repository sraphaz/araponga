import fs from "fs";
import path from "path";

export interface MermaidPreset {
  id: string;
  label: string;
  directives: Record<string, unknown>;
}

let presets: MermaidPreset[] = [];

export function loadPresets(basePath: string): MermaidPreset[] {
  const presetRoot = path.resolve(basePath);
  if (!fs.existsSync(presetRoot)) {
    presets = [];
    return presets;
  }

  const entries = fs.readdirSync(presetRoot, { withFileTypes: true });
  presets = entries
    .filter((entry) => entry.isDirectory())
    .map((entry) => path.join(presetRoot, entry.name, "preset.json"))
    .filter((presetPath) => fs.existsSync(presetPath))
    .map((presetPath) => {
      const raw = fs.readFileSync(presetPath, "utf-8");
      return JSON.parse(raw) as MermaidPreset;
    });

  return presets;
}

export function getPresetById(id: string): MermaidPreset | undefined {
  return presets.find((preset) => preset.id === id);
}
