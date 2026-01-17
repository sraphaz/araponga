"use client";

import { Accordion } from "../../../components/ui/Accordion";

interface Section {
  title: string;
  content: string;
  isLong: boolean;
  level: number;
  id?: string;
}

export function processContentIntoSections(html: string): Section[] {
  const sections: Section[] = [];
  
  // Regex para encontrar headings e seu conteúdo
  const headingRegex = /<h([2-4])([^>]*)id="([^"]*)"[^>]*>(.*?)<\/h\1>/gi;
  
  let lastIndex = 0;
  let match;
  
  while ((match = headingRegex.exec(html)) !== null) {
    const level = parseInt(match[1]);
    const id = match[3];
    const title = match[4].replace(/<[^>]*>/g, ''); // Remove HTML tags do título
    
    // Conteúdo até o próximo heading ou fim do documento
    const nextMatch = headingRegex.exec(html);
    const contentEnd = nextMatch ? nextMatch.index : html.length;
    
    // Pega o conteúdo entre este heading e o próximo
    const content = html.substring(match.index + match[0].length, contentEnd);
    
    // Calcula se é longo (mais de 500 caracteres de texto)
    const textLength = content.replace(/<[^>]*>/g, '').trim().length;
    const isLong = textLength > 500;
    
    if (match.index > lastIndex) {
      // Conteúdo antes do primeiro heading
      const beforeContent = html.substring(lastIndex, match.index);
      if (beforeContent.trim()) {
        sections.push({
          title: '',
          content: beforeContent,
          isLong: beforeContent.replace(/<[^>]*>/g, '').trim().length > 500,
          level: 1,
        });
      }
    }
    
    sections.push({
      title,
      content,
      isLong,
      level,
      id,
    });
    
    lastIndex = contentEnd;
  }
  
  // Se sobrar conteúdo após o último heading
  if (lastIndex < html.length) {
    const remainingContent = html.substring(lastIndex);
    if (remainingContent.trim()) {
      sections.push({
        title: '',
        content: remainingContent,
        isLong: remainingContent.replace(/<[^>]*>/g, '').trim().length > 500,
        level: 1,
      });
    }
  }
  
  return sections;
}

export function renderSections(sections: Section[]) {
  return sections.map((section, index) => {
    if (!section.title && !section.isLong) {
      // Conteúdo sem título e curto - renderiza diretamente
      return (
        <div
          key={index}
          className="mb-6"
          dangerouslySetInnerHTML={{ __html: section.content }}
        />
      );
    }
    
    if (!section.isLong) {
      // Seção com título mas curta - renderiza normalmente
      const HeadingTag = section.title ? `h${section.level}` as keyof JSX.IntrinsicElements : 'div';
      return (
        <section key={index} id={section.id} className="mb-8">
          {section.title && <HeadingTag className="document-section-heading">{section.title}</HeadingTag>}
          <div
            className="document-section-content"
            dangerouslySetInnerHTML={{ __html: section.content }}
          />
        </section>
      );
    }
    
    // Seção longa - usa Accordion
    return (
      <section key={index} id={section.id} className="mb-6">
        <Accordion title={section.title || 'Conteúdo'} defaultOpen={index === 0}>
          <div dangerouslySetInnerHTML={{ __html: section.content }} />
        </Accordion>
      </section>
    );
  });
}
