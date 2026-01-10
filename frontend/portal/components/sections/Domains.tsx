export default function Domains() {
  return (
    <section className="bg-forest-800 text-white">
      <div className="container-max py-14">
        <h2 className="text-2xl font-semibold tracking-tight">Domínios principais da plataforma</h2>
        <p className="mt-4 max-w-3xl text-sm leading-relaxed text-white/85">
          O Araponga organiza-se em domínios funcionais integrados, com responsabilidades claras.
        </p>
        <div className="mt-8 grid gap-6 md:grid-cols-2">
          {[
            {
              title: "Território",
              body: "Regras, geofencing, identidade local, pontos de interesse."
            },
            {
              title: "Usuários",
              body: "Perfis, verificação de residência, papéis e permissões."
            },
            {
              title: "Feed",
              body: "Publicações, alertas, eventos e moderação local."
            },
            {
              title: "Mapa",
              body: "Camadas territoriais, locais, eventos e visualização contextual."
            }
          ].map((c) => (
            <div key={c.title} className="rounded-2xl bg-white/10 p-6 ring-1 ring-white/15">
              <h3 className="text-base font-semibold">{c.title}</h3>
              <p className="mt-3 text-sm leading-relaxed text-white/85">{c.body}</p>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}
