export default function HowItWorks() {
  return (
    <section className="bg-forest-600 text-white">
      <div className="container-max py-14">
        <h2 className="text-2xl font-semibold tracking-tight">Como funciona na prática</h2>
        <div className="mt-8 grid gap-6 md:grid-cols-2">
          {[
            {
              title: "Login e autenticação",
              body:
                "Usuário faz login e cria identidade básica. Ainda não há vínculo com território específico."
            },
            {
              title: "Escolher um território",
              body:
                "Usuário seleciona o território de interesse. Cada território possui regras próprias."
            },
            {
              title: "Visitante ou Residente",
              body:
                "Sistema atribui papel inicial. Residência pode ser solicitada conforme regras locais."
            },
            {
              title: "Feed e mapa filtrados",
              body:
                "Conteúdo exibido respeita o papel do usuário. Tudo aparece em feed e mapa."
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
