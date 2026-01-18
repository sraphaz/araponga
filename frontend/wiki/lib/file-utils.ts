/**
 * Utilitários para manipulação de arquivos e caminhos
 */

import { readdir } from "fs/promises";
import { join } from "path";

/**
 * Remove prefixos numéricos de nomes de arquivo (00_, 01_, etc.)
 */
export function removeNumericPrefix(text: string): string {
  return text.replace(/^\d+_/, "");
}

/**
 * Gera título a partir do nome do arquivo
 * Remove extensão, prefixos numéricos e substitui _ por espaços
 */
export function getTitleFromFileName(fileName: string): string {
  const fileNameWithoutExt = fileName.replace(".md", "");
  const titleWithoutPrefix = removeNumericPrefix(fileNameWithoutExt);
  return titleWithoutPrefix.replace(/_/g, " ");
}

/**
 * Busca recursivamente todos os arquivos .md em um diretório
 */
export async function getAllMarkdownFiles(
  dir: string,
  basePath: string = ""
): Promise<string[]> {
  try {
    const files: string[] = [];
    const entries = await readdir(dir, { withFileTypes: true });

    for (const entry of entries) {
      const fullPath = basePath
        ? `${basePath}/${entry.name}`.replace(/\\/g, "/")
        : entry.name;

      if (entry.isDirectory()) {
        const subDir = join(dir, entry.name);
        const subFiles = await getAllMarkdownFiles(subDir, fullPath);
        files.push(...subFiles);
      } else if (entry.isFile() && entry.name.endsWith(".md")) {
        const relativePath = fullPath.replace(/\.md$/, "");
        files.push(relativePath);
      }
    }

    return files;
  } catch (error) {
    console.error(`Error reading directory ${dir}:`, error);
    return [];
  }
}
