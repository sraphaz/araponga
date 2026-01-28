import type { MermaidPreset } from "./presetRegistry";

export function applyPreset(code: string, preset?: MermaidPreset): string {
  if (!preset) {
    return code;
  }

  const initBlock = `%%{init: ${JSON.stringify(preset.directives)}}%%`;
  return `${initBlock}\n${code}`;
}
