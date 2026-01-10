import "./globals.css";
import type { Metadata } from "next";

const siteUrl = "https://araponga.app";

export const metadata: Metadata = {
  metadataBase: new URL(siteUrl),
  title: "ARAPONGA — Território-Primeiro & Comunidade-Primeiro",
  description:
    "Plataforma orientada ao território para organização comunitária local. Território primeiro, comunidade primeiro.",
  openGraph: {
    type: "website",
    url: siteUrl,
    title: "ARAPONGA — Território-Primeiro & Comunidade-Primeiro",
    description:
      "Plataforma orientada ao território para organização comunitária local. Território primeiro, comunidade primeiro.",
    images: [
      {
        url: "/og.png",
        width: 1200,
        height: 630,
        alt: "ARAPONGA"
      }
    ]
  },
  twitter: {
    card: "summary_large_image",
    title: "ARAPONGA — Território-Primeiro & Comunidade-Primeiro",
    description:
      "Plataforma orientada ao território para organização comunitária local.",
    images: ["/og.png"]
  },
  icons: {
    icon: "/favicon.png",
    apple: "/icon.png"
  }
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="pt-BR">
      <body>{children}</body>
    </html>
  );
}
