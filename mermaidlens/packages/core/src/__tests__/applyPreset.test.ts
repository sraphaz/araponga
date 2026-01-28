import { describe, expect, it } from "vitest";
import { applyPreset } from "../applyPreset";
import type { MermaidPreset } from "../presetRegistry";

describe("applyPreset", () => {
  it("returns original code when preset is undefined", () => {
    const code = "graph TD\nA-->B";
    expect(applyPreset(code)).toBe(code);
  });

  it("injects init block when preset exists", () => {
    const code = "sequenceDiagram\nA->>B: Hi";
    const preset: MermaidPreset = {
      id: "sequence-clean",
      label: "Sequence Clean",
      directives: {
        sequence: {
          mirrorActors: false
        }
      }
    };

    const result = applyPreset(code, preset);
    expect(result).toBe(`%%{init: ${JSON.stringify(preset.directives)}}%%\n${code}`);
  });
});
