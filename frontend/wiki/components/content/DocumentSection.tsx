"use client";

import { ReactNode } from "react";
import { Accordion } from "../ui/Accordion";

interface DocumentSectionProps {
  title: string;
  id?: string;
  defaultOpen?: boolean;
  children: ReactNode;
  level?: 2 | 3 | 4;
  isLong?: boolean;
}

export function DocumentSection({
  title,
  id,
  defaultOpen = false,
  children,
  level = 2,
  isLong = false,
}: DocumentSectionProps) {
  const HeadingTag = `h${level}` as keyof JSX.IntrinsicElements;

  // Para seções muito curtas, não usar accordion
  if (!isLong) {
    return (
      <section id={id} className="document-section mb-8">
        <HeadingTag className="document-section-heading">{title}</HeadingTag>
        <div className="document-section-content">{children}</div>
      </section>
    );
  }

  // Para seções longas, usar accordion
  return (
    <section id={id} className="document-section mb-6">
      <Accordion title={title} defaultOpen={defaultOpen}>
        <div dangerouslySetInnerHTML={{ __html: children as string }} />
      </Accordion>
    </section>
  );
}
