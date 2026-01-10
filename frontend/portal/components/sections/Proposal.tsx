export default function Proposal() {
  return (
    <section className="bg-forest-100/70">
      <div className="container-max py-14">
        <h2 className="text-2xl font-semibold tracking-tight">
          A proposta: território primeiro, comunidade primeiro
        </h2>
        <div className="mt-8 grid gap-6 md:grid-cols-3">
          {[
            {
              title: "Orientado ao território",
              body:
                "Cada comunidade tem regras, identidade e contexto próprios. O Araponga parte do território como unidade primária."
            },
            {
              title: "Feed local + mapa integrado",
              body:
                "Postagens, lugares, alertas e eventos aparecem em timeline e mapa, sempre filtrados pelo território."
            },
            {
              title: "Governança explícita",
              body:
                "Regras de visibilidade e participação são claras: visitante vs residente, conteúdos públicos vs restritos."
            }
          ].map((c) => (
            <div key={c.title} className="rounded-2xl bg-white/70 p-6 ring-1 ring-black/5">
              <h3 className="text-base font-semibold">{c.title}</h3>
              <p className="mt-3 text-sm leading-relaxed text-forest-800">{c.body}</p>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}
