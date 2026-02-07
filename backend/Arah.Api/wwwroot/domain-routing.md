# Domínios públicos do Arah

## Visão geral

O portal técnico do Arah fica em `https://Arah.eco.br/` e é servido pelo GitHub Pages a partir da pasta `docs/`.
A apresentação pública (landing) usa o domínio `https://Arah.app/`, com a identidade visual e metadados definidos neste repositório.

## Domínios adquiridos

- `Arah.eco.br` → portal técnico (GitHub Pages).
- `Arah.app` → landing pública.
- `Arah.org` → redireciona para `https://Arah.eco.br/`.

## Lógica de publicação e redirect

- O `docs/index.html` mantém o redirect automático para a landing pública.
- Quando o usuário retorna da landing, a experiência fica no portal técnico (`Arah.eco.br`).
- Ajustes de DNS e redirects externos (ex: `Arah.org` → `Arah.eco.br`) são feitos no provedor de domínio.

## Validação rápida

- Abra `https://Arah.eco.br/` e confirme que o redirecionamento leva para a landing pública.
- Verifique se o retorno para o portal técnico funciona com o parâmetro `?fromLanding=1`.
