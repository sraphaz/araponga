export default function Hero() {
  return (
    <section className="bg-gradient-to-b from-forest-100 to-forest-50">
      <div className="container-max py-16 md:py-20">
        <div className="flex flex-col gap-10 md:flex-row md:items-center md:justify-between">
          <div className="max-w-xl">
            <div className="flex items-center gap-3">
              <img src="/icon.png" alt="Araponga" className="h-12 w-12" />
              <h1 className="text-3xl font-semibold tracking-tight md:text-4xl">
                ARAPONGA
              </h1>
            </div>
            <p className="mt-4 text-lg leading-relaxed text-forest-800">
              Território-Primeiro &amp; Comunidade-Primeiro — Plataforma orientada ao território para
              organização comunitária local.
            </p>
            <div className="mt-8 flex flex-wrap items-center gap-3">
              <a
                href="#apoie"
                className="rounded-lg bg-forest-700 px-5 py-3 text-sm font-medium text-white hover:bg-forest-800"
              >
                Apoie o Vivo Regenerativa
              </a>
              <a
                href="#roadmap"
                className="rounded-lg border border-forest-300 px-5 py-3 text-sm font-medium text-forest-800 hover:bg-forest-100"
              >
                Conhecer roadmap
              </a>
            </div>
          </div>
          <div className="w-full max-w-md rounded-2xl bg-white/50 p-6 shadow-sm ring-1 ring-black/5">
            <p className="text-sm font-semibold text-forest-700">O Pássaro que nos inspira</p>
            <p className="mt-3 text-sm leading-relaxed text-forest-800">
              O Araponga, também conhecido como “pássaro-ferreiro”, é famoso por seu canto metálico e
              ressonante nas florestas brasileiras. Sua presença vibrante simboliza a força e
              resiliência da natureza.
            </p>
          </div>
        </div>
      </div>
    </section>
  );
}
