export default function Value() {
  return (
    <section className="bg-forest-50">
      <div className="container-max py-14">
        <h2 className="text-2xl font-semibold tracking-tight">Valor gerado para comunidades e parceiros</h2>
        <div className="mt-8 grid gap-6 md:grid-cols-4">
          {[
            { title: "Relevância local", body: "Menos ruído, mais utilidade no dia a dia." },
            { title: "Construção de confiança", body: "Papéis e regras explícitos fortalecem a comunidade." },
            { title: "Organização facilitada", body: "Eventos, alertas e decisões num só lugar." },
            { title: "Custo reduzido", body: "Infra e governança pensadas para pequenos territórios." }
          ].map((c) => (
            <div key={c.title} className="rounded-2xl bg-white p-6 shadow-sm ring-1 ring-black/5">
              <h3 className="text-sm font-semibold">{c.title}</h3>
              <p className="mt-3 text-sm leading-relaxed text-forest-800">{c.body}</p>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}
