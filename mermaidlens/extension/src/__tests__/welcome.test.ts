import path from "path";
import { beforeEach, describe, expect, it, vi } from "vitest";

const openTextDocument = vi.fn();
const showTextDocument = vi.fn();

vi.mock("vscode", () => ({
  workspace: {
    openTextDocument
  },
  window: {
    showTextDocument
  },
  Uri: {
    file: (value: string) => ({ fsPath: value })
  },
  ViewColumn: {
    One: 1
  }
}));

import { showWelcomePage } from "../features/welcome";

describe("showWelcomePage", () => {
  beforeEach(() => {
    openTextDocument.mockReset();
    showTextDocument.mockReset();
  });

  it("does nothing when welcome was already shown", async () => {
    const context = {
      extensionPath: "/extensions/mermaidlens",
      globalState: {
        get: vi.fn().mockReturnValue(true),
        update: vi.fn()
      }
    } as const;

    await showWelcomePage(context);

    expect(openTextDocument).not.toHaveBeenCalled();
    expect(showTextDocument).not.toHaveBeenCalled();
    expect(context.globalState.update).not.toHaveBeenCalled();
  });

  it("opens welcome document and updates global state", async () => {
    const context = {
      extensionPath: "/extensions/mermaidlens",
      globalState: {
        get: vi.fn().mockReturnValue(false),
        update: vi.fn()
      }
    } as const;

    const expectedPath = path.join(context.extensionPath, "media", "welcome.md");
    openTextDocument.mockResolvedValue({ uri: { fsPath: expectedPath } });

    await showWelcomePage(context);

    expect(openTextDocument).toHaveBeenCalledWith({ fsPath: expectedPath });
    expect(showTextDocument).toHaveBeenCalledWith(
      { uri: { fsPath: expectedPath } },
      expect.objectContaining({ preview: false, viewColumn: 1 })
    );
    expect(context.globalState.update).toHaveBeenCalledWith("mermaidlens.hasShownWelcome", true);
  });
});
