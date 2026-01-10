export default function Roadmap() {
  return (
    <section id="roadmap" className="bg-forest-800 text-white">
      <div className="container-max py-14">
        <h2 className="text-2xl font-semibold tracking-tight">Roadmap: evolução com três fases</h2>
        <div className="mt-8 grid gap-6 md:grid-cols-3">
          {[
            {
              title: "Fase 1: MVP sólido",
              items: [
                "Cadastro e autenticação",
                "Seleção de território",
                "Feed e mapa base",
                "Papéis visitante/residente"
              ]
            },
            {
              title: "Fase 2: Mídia regenerativa",
              items: [
                "Alertas comunitários",
                "Eventos e mutirões",
                "Ferramentas de moderação",
                "Camadas territoriais"
              ]
            },
            {
              title: "Fase 3: Propriedades avançadas",
              items: [
                "Governança ampliada",
                "Economias locais",
                "Identidade descentralizada",
                "Integrações e parceiros"
              ]
            }
          ].map((col) => (
            <div key={col.title} className="rounded-2xl bg-white/10 p-6 ring-1 ring-white/15">
              <h3 className="text-base font-semibold">{col.title}</h3>
              <ul className="mt-4 list-disc space-y-2 pl-5 text-sm text-white/85">
                {col.items.map((it) => (
                  <li key={it}>{it}</li>
                ))}
              </ul>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}
