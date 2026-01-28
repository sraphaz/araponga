import { describe, expect, it } from "vitest";
import { scanMermaid } from "../scanMermaid";

describe("scanMermaid", () => {
  it("finds mermaid code fences", () => {
    const markdown = "# Doc\n```mermaid\ngraph TD\nA-->B\n```\nText\n```mermaid\nsequenceDiagram\nA->>B: Hi\n```";
    const blocks = scanMermaid(markdown);

    expect(blocks).toHaveLength(2);
    expect(blocks[0]?.code).toBe("graph TD\nA-->B");
    expect(blocks[0]?.info).toBe("mermaid");
    expect(blocks[1]?.code).toBe("sequenceDiagram\nA->>B: Hi");
  });

  it("returns empty array when no mermaid fences", () => {
    expect(scanMermaid("# No diagrams")).toEqual([]);
  });
});
