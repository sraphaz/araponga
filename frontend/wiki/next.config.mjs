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
  // Desabilita prefetch automático para evitar duplicação de basePath
  // Bug conhecido do Next.js: https://github.com/vercel/next.js/issues/73427
  experimental: {
    optimizePackageImports: [],
  },
};

export default nextConfig;
