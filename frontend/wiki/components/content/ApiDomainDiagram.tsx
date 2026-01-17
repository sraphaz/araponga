"use client";

import Image from "next/image";

export function ApiDomainDiagram() {
  return (
    <div className="api-domain-diagram-container my-16">
      <div className="glass-card animation-fade-in">
        <div className="glass-card__content">
          <div className="text-center mb-8">
            <h2 className="text-3xl md:text-4xl font-bold text-forest-900 dark:text-forest-50 mb-4">
              Visão Geral do Sistema Araponga
            </h2>
            <p className="text-lg text-forest-700 dark:text-forest-300 max-w-2xl mx-auto">
              Diagrama isométrico mostrando como os diferentes módulos se conectam ao território como referência central
            </p>
          </div>

          <div className="relative w-full max-w-5xl mx-auto rounded-2xl overflow-hidden shadow-2xl border-2 border-forest-200/80 dark:border-forest-800/80">
            <Image
              src="/araponga-api-domain-diagram.png"
              alt="Diagrama isométrico do Domínio API Araponga - mostrando TERRITÓRIO no centro com conexões para FEED, MAP, HEALTH, FEATURES, MEMBERSHIP & GOVERNANCE, e AUTENTICAÇÃO"
              width={1200}
              height={800}
              className="w-full h-auto object-contain"
              priority
              onError={(e) => {
                // Fallback se imagem não existir
                const target = e.target as HTMLImageElement;
                target.style.display = 'none';
                const fallback = target.nextElementSibling as HTMLElement;
                if (fallback) fallback.style.display = 'block';
              }}
            />
            <div 
              className="hidden p-8 bg-forest-100 dark:bg-forest-900/50 rounded-xl text-center"
              style={{ display: 'none' }}
            >
              <p className="text-forest-600 dark:text-forest-400 mb-4">
                📊 Diagrama do Domínio API Araponga
              </p>
              <p className="text-sm text-forest-500 dark:text-forest-500">
                A imagem será carregada quando disponível em <code>/public/araponga-api-domain-diagram.png</code>
              </p>
            </div>
          </div>

          <div className="mt-8 grid md:grid-cols-3 gap-4 text-sm text-forest-600 dark:text-forest-400">
            <div className="text-center">
              <span className="block text-2xl mb-2">🗺️</span>
              <p className="font-medium">Território Central</p>
              <p className="text-xs mt-1">Referência geográfica para todos os módulos</p>
            </div>
            <div className="text-center">
              <span className="block text-2xl mb-2">🔗</span>
              <p className="font-medium">Conexões Contextuais</p>
              <p className="text-xs mt-1">Cada módulo se conecta ao território ativo</p>
            </div>
            <div className="text-center">
              <span className="block text-2xl mb-2">👥</span>
              <p className="font-medium">Pessoas no Centro</p>
              <p className="text-xs mt-1">Governança e autonomia comunitária</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
