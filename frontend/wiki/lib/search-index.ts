/**
 * Índice de busca para Wiki
 * Indexa documentos Markdown da pasta docs/
 */

import { readdir, readFile } from 'fs/promises';
import { join } from 'path';
import matter from 'gray-matter';
import { remark } from 'remark';
import remarkHtml from 'remark-html';
import remarkGfm from 'remark-gfm';
import { getTextContent } from './markdown';
import { getTitleFromFileName } from './file-utils';

export interface WikiSearchItem {
  id: string;
  title: string;
  content: string;
  url: string;
  category: string;
  description?: string;
}

/**
 * Gera índice de busca para todos os documentos da Wiki
 */
export async function generateWikiIndex(): Promise<WikiSearchItem[]> {
  const docsPath = join(process.cwd(), '..', '..', 'docs');
  const index: WikiSearchItem[] = [];

  try {
    const files = await readdir(docsPath);
    const mdFiles = files.filter(file => file.endsWith('.md'));

    for (const file of mdFiles) {
      try {
        const filePath = join(docsPath, file);
        const fileContents = await readFile(filePath, 'utf8');
        const { content, data } = matter(fileContents);

        // Processa markdown para extrair texto
        const processedContent = await remark()
          .use(remarkHtml)
          .use(remarkGfm)
          .process(content);

        const htmlContent = processedContent.toString();
        const textContent = getTextContent(htmlContent);

        // Determina categoria baseado no nome do arquivo
        const category = getCategoryFromFileName(file);
        const slug = file.replace(/\.md$/, '');

        index.push({
          id: slug,
          title: (data.title as string) || getTitleFromFileName(file),
          content: textContent.substring(0, 500), // Primeiros 500 caracteres para busca
          url: `/wiki/docs/${slug}`,
          category,
          description: data.description as string | undefined,
        });
      } catch (error) {
        console.error(`Error indexing ${file}:`, error);
      }
    }
  } catch (error) {
    console.error('Error reading docs directory:', error);
  }

  return index;
}

/**
 * Determina categoria do documento baseado no nome do arquivo
 */
function getCategoryFromFileName(fileName: string): string {
  if (fileName.startsWith('00_')) return 'Referência';
  if (fileName.startsWith('01_') || fileName.startsWith('02_') || fileName.startsWith('03_') || fileName.startsWith('04_') || fileName.startsWith('05_')) {
    return 'Visão e Produto';
  }
  if (fileName.startsWith('10_') || fileName.startsWith('11_') || fileName.startsWith('12_') || fileName.startsWith('13_')) {
    return 'Arquitetura';
  }
  if (fileName.startsWith('20_') || fileName.startsWith('21_') || fileName.startsWith('22_') || fileName.startsWith('23_')) {
    return 'Desenvolvimento';
  }
  if (fileName.startsWith('ONBOARDING_') || fileName === 'MENTORIA.md' || fileName === 'PRIORIZACAO_PROPOSTAS.md') {
    return 'Onboarding';
  }
  if (fileName.startsWith('SECURITY_')) {
    return 'Segurança';
  }
  if (fileName === 'PROJECT_STRUCTURE.md') {
    return 'Desenvolvimento';
  }
  if (fileName === 'CARTILHA_COMPLETA.md') {
    return 'Onboarding';
  }
  return 'Outros';
}
