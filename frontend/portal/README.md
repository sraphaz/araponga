# Arah Landing (Next.js)

Este projeto é um *scaffold* para substituir a publicação no Gamma por um front-end próprio, com controle de **SEO**, **Open Graph (WhatsApp/LinkedIn)**, **favicon** e evolução incremental.

## Setup

```bash
npm i
npm run dev
```

## Deploy (Vercel)

1. Suba este repositório no GitHub.
2. Importe na Vercel.
3. Configure o domínio `Arah.app`.

## Importante: cache de preview (WhatsApp/Telegram)

Após o deploy, o WhatsApp pode manter cache do OG antigo por algum tempo. Para validar rapidamente:
- Use o inspetor de compartilhamento do Facebook/Meta (força re-scrape).

## Próximos ajustes
- Substituir textos “aproximados” pelos textos extraídos (veja `araponga_extracted.md`).
- Trazer a ilustração do topo e os fundos/ornamentos (hoje placeholders).
- Ajustar espaçamentos e tipografia para 1:1 com o layout atual.
