export default function Problem() {
  return (
    <section className="bg-forest-50">
      <div className="container-max py-14">
        <h2 className="text-2xl font-semibold tracking-tight">
          O ruído da escala global dificulta o contexto local
        </h2>
        <div className="mt-8 grid gap-6 md:grid-cols-3">
          {[
            {
              title: "Relevância diluída",
              body:
                "Plataformas generalistas priorizam alcance global, tornando difícil encontrar conteúdos realmente relevantes para o entorno imediato."
            },
            {
              title: "Falta de contexto territorial",
              body:
                "Ferramentas existentes não reconhecem as especificidades de cada lugar: regras, dinâmicas de confiança e organização comunitária."
            },
            {
              title: "Dependência de gigantes",
              body:
                "Comunidades ficam reféns de algoritmos opacos e políticas que não atendem às necessidades do território."
            }
          ].map((c) => (
            <div
              key={c.title}
              className="rounded-2xl bg-white p-6 shadow-sm ring-1 ring-black/5"
            >
              <h3 className="text-base font-semibold text-forest-900">{c.title}</h3>
              <p className="mt-3 text-sm leading-relaxed text-forest-800">{c.body}</p>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}
