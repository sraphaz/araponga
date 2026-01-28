import fs from "fs";
import os from "os";
import path from "path";
import { describe, expect, it } from "vitest";
import { getDefaultTheme, getThemeById, loadThemes } from "../themeRegistry";
import { getPresetById, loadPresets } from "../presetRegistry";

describe("themeRegistry", () => {
  it("returns empty themes when directory missing", () => {
    const missing = path.join(os.tmpdir(), `mermaidlens-missing-${Date.now()}`);
    expect(loadThemes(missing)).toEqual([]);
    expect(getThemeById("ocean")).toBeUndefined();
    expect(getDefaultTheme()).toBeUndefined();
  });

  it("loads themes and resolves default", () => {
    const dir = fs.mkdtempSync(path.join(os.tmpdir(), "mermaidlens-themes-"));
    const oceanDir = path.join(dir, "ocean");
    const coralDir = path.join(dir, "coralreef");
    fs.mkdirSync(oceanDir);
    fs.mkdirSync(coralDir);

    fs.writeFileSync(
      path.join(oceanDir, "theme.json"),
      JSON.stringify({
        id: "ocean",
        name: "Ocean",
        mermaid: { theme: "base", themeVariables: { primaryColor: "#00CFFF" } }
      })
    );
    fs.writeFileSync(
      path.join(coralDir, "theme.json"),
      JSON.stringify({
        id: "coralreef",
        name: "Coral",
        mermaid: { theme: "base", themeVariables: { primaryColor: "#FF7A59" } }
      })
    );

    const themes = loadThemes(dir);
    expect(themes).toHaveLength(2);
    expect(getThemeById("coralreef")?.name).toBe("Coral");
    expect(getDefaultTheme()?.id).toBe("ocean");
  });
});

describe("presetRegistry", () => {
  it("returns empty presets when directory missing", () => {
    const missing = path.join(os.tmpdir(), `mermaidlens-presets-${Date.now()}`);
    expect(loadPresets(missing)).toEqual([]);
    expect(getPresetById("architecture")).toBeUndefined();
  });

  it("loads presets", () => {
    const dir = fs.mkdtempSync(path.join(os.tmpdir(), "mermaidlens-presets-"));
    const presetDir = path.join(dir, "architecture");
    fs.mkdirSync(presetDir);
    fs.writeFileSync(
      path.join(presetDir, "preset.json"),
      JSON.stringify({
        id: "architecture",
        label: "Architecture Blueprint",
        directives: { flowchart: { curve: "basis" } }
      })
    );

    const presets = loadPresets(dir);
    expect(presets).toHaveLength(1);
    expect(getPresetById("architecture")?.label).toBe("Architecture Blueprint");
  });
});
