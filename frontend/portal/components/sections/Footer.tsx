export default function Footer() {
  return (
    <footer id="apoie" className="bg-forest-100">
      <div className="container-max py-14">
        <div className="grid gap-10 md:grid-cols-2 md:items-center">
          <div>
            <h3 className="text-xl font-semibold">Apoie o desenvolvimento</h3>
            <p className="mt-4 text-sm leading-relaxed text-forest-800">
              Se você quer ver o Araponga nascer como infraestrutura territorial, apoie e participe da
              construção.
            </p>
            <div className="mt-6 flex flex-wrap gap-3">
              <a
                href="#"
                className="rounded-lg bg-forest-700 px-5 py-3 text-sm font-medium text-white hover:bg-forest-800"
              >
                Apoiar o Vivo Regenerativa
              </a>
              <a
                href="mailto:contato@araponga.app"
                className="rounded-lg border border-forest-300 px-5 py-3 text-sm font-medium text-forest-800 hover:bg-forest-50"
              >
                contato@araponga.app
              </a>
            </div>
          </div>
          <div className="rounded-2xl bg-forest-800 p-6 text-white shadow-sm">
            <p className="text-sm font-semibold">EM BREVE</p>
            <p className="mt-2 text-sm text-white/85">Disponível para Android e Apple</p>
            <div className="mt-6 flex flex-wrap gap-3">
              <div className="rounded-lg bg-white/10 px-4 py-2 text-xs">Google Play</div>
              <div className="rounded-lg bg-white/10 px-4 py-2 text-xs">App Store</div>
            </div>
          </div>
        </div>
        <div className="mt-12 border-t border-forest-200 pt-6 text-xs text-forest-700">
          © {new Date().getFullYear()} Araponga. Território-Primeiro, Comunidade-Primeiro.
        </div>
      </div>
    </footer>
  );
}
