export interface MermaidBlock {
  code: string;
  info: string;
  start: number;
  end: number;
}

const mermaidFence = /```mermaid\s*([\s\S]*?)```/g;

export function scanMermaid(markdown: string): MermaidBlock[] {
  const blocks: MermaidBlock[] = [];
  let match: RegExpExecArray | null;

  while ((match = mermaidFence.exec(markdown)) !== null) {
    blocks.push({
      code: match[1].trim(),
      info: "mermaid",
      start: match.index,
      end: match.index + match[0].length
    });
  }

  return blocks;
}
