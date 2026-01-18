import Image from "next/image";

interface OfficialIconProps {
  src: string;
  alt: string;
  width?: number;
  height?: number;
  className?: string;
}

export function OfficialIcon({ 
  src, 
  alt, 
  width = 24, 
  height = 24, 
  className = "" 
}: OfficialIconProps) {
  // Para SVGs externos ou URLs oficiais, podemos usar img diretamente
  if (src.startsWith("http")) {
    return (
      <img
        src={src}
        alt={alt}
        width={width}
        height={height}
        className={className}
        style={{ width: `${width}px`, height: `${height}px`, objectFit: "contain" }}
      />
    );
  }

  // Para imagens locais, usar Next.js Image
  return (
    <Image
      src={src}
      alt={alt}
      width={width}
      height={height}
      className={className}
      style={{ width: `${width}px`, height: `${height}px`, objectFit: "contain" }}
    />
  );
}
