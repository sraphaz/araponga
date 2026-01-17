"use client";

import { ReactNode } from "react";
import { Accordion, AccordionItem, AccordionTrigger, AccordionContent } from "../ui/Accordion";

interface DocumentSectionProps {
  title: string;
  id?: string;
  defaultOpen?: boolean;
  children: ReactNode;
  level?: 2 | 3 | 4;
}

export function DocumentSection({
  title,
  id,
  defaultOpen = false,
  children,
  level = 2,
}: DocumentSectionProps) {
  const HeadingTag = `h${level}` as keyof JSX.IntrinsicElements;

  // Para seções muito curtas (< 500 caracteres), não usar accordion
  const contentLength = typeof children === "string" ? children.length : 1000;
  const shouldCollapse = contentLength > 500;

  if (!shouldCollapse) {
    return (
      <section id={id} className="document-section mb-8">
        <HeadingTag className="document-section-heading">{title}</HeadingTag>
        <div className="document-section-content">{children}</div>
      </section>
    );
  }

  return (
    <section id={id} className="document-section mb-6">
      <Accordion type="single" collapsible defaultValue={defaultOpen ? id : undefined}>
        <AccordionItem value={id || title} className="border-none">
          <AccordionTrigger className="document-section-trigger">
            <HeadingTag className="document-section-heading mb-0">{title}</HeadingTag>
          </AccordionTrigger>
          <AccordionContent className="document-section-content pt-4">
            {children}
          </AccordionContent>
        </AccordionItem>
      </Accordion>
    </section>
  );
}
