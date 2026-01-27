/** @type {import('next').NextConfig} */
const nextConfig = {
  images: {
    remotePatterns: [
      {
        protocol: 'https',
        hostname: 'assets-global.website-files.com',
        pathname: '/**',
      },
      {
        protocol: 'https',
        hostname: 'github.githubassets.com',
        pathname: '/**',
      },
    ],
    unoptimized: true // Necessário para static export - desabilita otimização que requer servidor
  },
  // Enable reading markdown files from docs directory
  pageExtensions: ['ts', 'tsx', 'js', 'jsx', 'md', 'mdx'],
  // Para GitHub Pages: export estático
  output: process.env.NEXT_EXPORT === 'true' ? 'export' : undefined,
  // Base path para servir em /wiki dentro do devportal.araponga.app
  basePath: '/wiki',
  trailingSlash: true,
  // Asset prefix também precisa do basePath
  assetPrefix: '/wiki',
  // Headers para CSP - permite scripts inline necessários para theme init
  // Nota: Headers não funcionam com static export, mas podem ser configurados no servidor web
  // Para GitHub Pages, o CSP pode ser configurado via meta tag ou servidor web
  async headers() {
    // Headers não são aplicados em static export, mas mantemos para referência
    // O CSP será aplicado via meta tag no layout.tsx ou pelo servidor web
    return [];
  },
};

export default nextConfig;
