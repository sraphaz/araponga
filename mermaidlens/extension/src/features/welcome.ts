import * as path from "path";
import * as vscode from "vscode";

export async function showWelcomePage(context: vscode.ExtensionContext): Promise<void> {
  const hasShown = context.globalState.get<boolean>("mermaidlens.hasShownWelcome");
  if (hasShown) {
    return;
  }

  const welcomeUri = vscode.Uri.file(path.join(context.extensionPath, "media", "welcome.md"));
  const doc = await vscode.workspace.openTextDocument(welcomeUri);

  await vscode.window.showTextDocument(doc, {
    preview: false,
    viewColumn: vscode.ViewColumn.One
  });

  await context.globalState.update("mermaidlens.hasShownWelcome", true);
}
