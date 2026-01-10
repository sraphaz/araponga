import localFont from "next/font/local";

export const geist = localFont({
  src: "./fonts/Geist-VariableFont_wght.ttf",
  variable: "--font-sans",
  display: "swap",
  weight: "100 900",
});

export const sora = localFont({
  src: "./fonts/Sora-VariableFont_wght.ttf",
  variable: "--font-display",
  display: "swap",
  weight: "100 800",
});

export const notoSansTC = localFont({
  src: "./fonts/NotoSansTC-VariableFont_wght.ttf",
  variable: "--font-cjk-sans",
  display: "swap",
  weight: "100 900",
});

export const notoSerifSC = localFont({
  src: "./fonts/NotoSerifSC-VariableFont_wght.ttf",
  variable: "--font-serif",
  display: "swap",
  weight: "200 900",
});
