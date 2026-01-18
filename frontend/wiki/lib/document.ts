/**
 * Utilitários para processamento de documentos Markdown
 * Centraliza lógica de leitura e processamento de documentos
 */

import { readFile } from "fs/promises";
import { join } from "path";
import matter from "gray-matter";
import { remark } from "remark";
import remarkHtml from "remark-html";
import remarkGfm from "remark-gfm";
import {
  getTextContent,
  processMarkdownLinks,
  extractFirstH1,
  addHeadingIds,
} from "./markdown";
import { getTitleFromFileName } from "./file-utils";

export interface DocumentContent {
  content: string;
  frontMatter: Record<string, unknown>;
  title: string;
}

/**
 * Processa conteúdo Markdown e retorna HTML processado
 */
export async function processMarkdownContent(
  filePath: string,
  basePath: string = "/wiki"
): Promise<DocumentContent | null> {
  try {
    const docsPath = join(process.cwd(), "..", "..", "docs", filePath);
    const fileContents = await readFile(docsPath, "utf8");
    const { content, data } = matter(fileContents);

    // Processa markdown para HTML
    const processedContent = await remark()
      .use(remarkHtml)
      .use(remarkGfm)
      .process(content);

    let htmlContent = processedContent.toString();

    // Extrai primeiro H1 para usar como título se não houver frontmatter
    const { title: firstH1Title, content: contentWithoutH1 } = extractFirstH1(htmlContent);
    htmlContent = contentWithoutH1;

    // Adiciona IDs únicos aos headings (h2-h4)
    htmlContent = addHeadingIds(htmlContent);

    // Processa links no HTML renderizado para incluir basePath
    htmlContent = processMarkdownLinks(htmlContent, basePath);

    // Gera título: frontmatter > primeiro H1 > nome do arquivo
    const fileName = filePath.split("/").pop() || "";
    const fallbackTitle = getTitleFromFileName(fileName);
    const title = (data.title as string) || firstH1Title || fallbackTitle;

    return {
      content: htmlContent,
      frontMatter: data,
      title,
    };
  } catch (error) {
    console.error(`Error reading ${filePath}:`, error);
    return null;
  }
}
