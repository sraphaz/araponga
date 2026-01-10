export default function Architecture() {
  return (
    <section className="bg-forest-50">
      <div className="container-max py-14">
        <h2 className="text-2xl font-semibold tracking-tight">
          Arquitetura técnica: domínio primeiro, evolução incremental
        </h2>
        <div className="mt-8 grid gap-8 md:grid-cols-2">
          <div className="rounded-2xl bg-white p-6 shadow-sm ring-1 ring-black/5">
            <h3 className="text-base font-semibold">Componentes principais</h3>
            <ul className="mt-4 list-disc space-y-2 pl-5 text-sm text-forest-800">
              <li>API orientada a domínios (Território, Usuários, Feed, Mapa).</li>
              <li>Camada de autenticação e papéis (visitante/residente).</li>
              <li>Persistência por território e trilhas de auditoria.</li>
            </ul>
          </div>
          <div className="rounded-2xl bg-white p-6 shadow-sm ring-1 ring-black/5">
            <h3 className="text-base font-semibold">Princípios de design</h3>
            <ul className="mt-4 list-disc space-y-2 pl-5 text-sm text-forest-800">
              <li>Domínio primeiro: regras explícitas antes de features.</li>
              <li>Evolução incremental: MVP simples, extensões por módulos.</li>
              <li>Transparência: governança e visibilidade claras.</li>
            </ul>
          </div>
        </div>
      </div>
    </section>
  );
}
