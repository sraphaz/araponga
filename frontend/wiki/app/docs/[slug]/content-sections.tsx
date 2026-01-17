"use client";

import { useMemo } from "react";
import { Accordion } from "../../../components/ui/Accordion";

interface Section {
  title: string;
  content: string;
  isLong: boolean;
  level: number;
  id?: string;
}

function processContentIntoSections(html: string): Section[] {
  const sections: Section[] = [];
  
  // Primeiro, vamos dividir por headings h2, h3, h4
  const parts = html.split(/(<h[2-4][^>]*>.*?<\/h[2-4]>)/gi);
  
  let currentSection: Section | null = null;
  
  for (const part of parts) {
    // Verifica se é um heading
    const headingMatch = part.match(/<h([2-4])([^>]*)id="([^"]*)"[^>]*>(.*?)<\/h\1>/i);
    
    if (headingMatch) {
      // Se havia uma seção anterior, adiciona ela
      if (currentSection) {
        sections.push(currentSection);
      }
      
      const level = parseInt(headingMatch[1]);
      const id = headingMatch[3];
      const title = headingMatch[4].replace(/<[^>]*>/g, '');
      
      currentSection = {
        title,
        content: part, // O heading faz parte do conteúdo
        isLong: false,
        level,
        id,
      };
    } else if (currentSection) {
      // Adiciona conteúdo à seção atual
      currentSection.content += part;
    } else {
      // Conteúdo antes do primeiro heading
      if (part.trim()) {
        const textLength = part.replace(/<[^>]*>/g, '').trim().length;
        sections.push({
          title: '',
          content: part,
          isLong: textLength > 500,
          level: 1,
        });
      }
    }
  }
  
  // Adiciona última seção se houver
  if (currentSection) {
    const textLength = currentSection.content.replace(/<[^>]*>/g, '').trim().length;
    currentSection.isLong = textLength > 500;
    sections.push(currentSection);
  }
  
  return sections;
}

interface ContentSectionsProps {
  htmlContent: string;
}

export function ContentSections({ htmlContent }: ContentSectionsProps) {
  const sections = useMemo(() => processContentIntoSections(htmlContent), [htmlContent]);
  
  return (
    <div className="markdown-content prose-headings:first:mt-0">
      {sections.map((section, index) => {
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
          const HeadingTag = section.title ? (`h${section.level}` as keyof JSX.IntrinsicElements) : 'div';
          return (
            <section key={index} id={section.id} className="mb-8">
              <div dangerouslySetInnerHTML={{ __html: section.content }} />
            </section>
          );
        }
        
        // Seção longa - usa Accordion
        return (
          <section key={index} id={section.id} className="mb-6">
            <Accordion title={section.title || 'Conteúdo'} defaultOpen={index === 0 || index === 1}>
              <div dangerouslySetInnerHTML={{ __html: section.content }} />
            </Accordion>
          </section>
        );
      })}
    </div>
  );
}
