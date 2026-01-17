export function Footer() {
  return (
    <footer className="border-t border-forest-200/80 bg-white/60 backdrop-blur-xl mt-24">
      <div className="container-max py-12">
        <div className="text-center space-y-4">
          <p className="text-forest-700 text-lg font-medium">
            Wiki Araponga — Documentação completa da plataforma digital comunitária
            orientada ao território
          </p>
          <p className="text-forest-600 text-sm">
            <a
              href="https://github.com/sraphaz/araponga"
              target="_blank"
              rel="noopener noreferrer"
              className="footer-link"
            >
              Contribuir no GitHub
            </a>
            <span className="mx-3">•</span>
            <a
              href="https://discord.gg/auwqN8Yjgw"
              target="_blank"
              rel="noopener noreferrer"
              className="footer-link"
            >
              Discord da Comunidade
            </a>
          </p>
          <p className="text-forest-500 text-xs mt-6">
            Território como referência • Comunidade como prioridade • Tecnologia como ferramenta
          </p>
        </div>
      </div>
    </footer>
  );
}
