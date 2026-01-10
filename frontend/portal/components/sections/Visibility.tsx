export default function Visibility() {
  return (
    <section className="bg-forest-50">
      <div className="container-max py-14">
        <h2 className="text-2xl font-semibold tracking-tight">
          Regras de visibilidade: por que isso importa
        </h2>
        <div className="mt-8 grid gap-10 md:grid-cols-2">
          <div className="rounded-2xl bg-white p-6 shadow-sm ring-1 ring-black/5">
            <h3 className="text-base font-semibold">Visitante &amp; Residente</h3>
            <ul className="mt-4 list-disc space-y-2 pl-5 text-sm text-forest-800">
              <li>Visitante vê o que é público no território.</li>
              <li>Residente acessa conteúdos e dinâmicas restritas.</li>
              <li>Regras claras reduzem ruído e aumentam confiança.</li>
            </ul>
          </div>
          <div className="rounded-2xl bg-white p-6 shadow-sm ring-1 ring-black/5">
            <h3 className="text-base font-semibold">Público &amp; Restrito</h3>
            <ul className="mt-4 list-disc space-y-2 pl-5 text-sm text-forest-800">
              <li>Algumas postagens são úteis para visitantes (serviços, eventos abertos).</li>
              <li>Outras exigem pertencimento (alertas locais, decisões comunitárias).</li>
              <li>O modelo prioriza segurança e autonomia territorial.</li>
            </ul>
          </div>
        </div>
      </div>
    </section>
  );
}
