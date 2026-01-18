import Link from "next/link";

interface Doc {
  name: string;
  path: string;
}

interface CategoryCardProps {
  category: string;
  docs: Doc[];
}

export function CategoryCard({ category, docs }: CategoryCardProps) {
  // Remove emojis do título - design limpo sem excessos
  const titleWithoutEmoji = category.replace(/^[\S\s]*?\s/, "").trim();

  return (
    <div className="category-card">
      <div className="category-card__content">
        <h2 className="text-lg font-semibold text-forest-900 dark:text-forest-50 mb-4">
          {titleWithoutEmoji}
        </h2>
        <ul className="space-y-2 flex-1">
          {docs.map((doc) => {
            const docSlug = doc.path.replace(".md", "");
            return (
              <li key={doc.path}>
                <Link
                  href={`/docs/${docSlug}`}
                  className="doc-link"
                >
                  <span className="doc-link-arrow">→</span>
                  <span className="flex-1">{doc.name}</span>
                </Link>
              </li>
            );
          })}
        </ul>
      </div>
    </div>
  );
}
