# Imagens do Wiki Arah

## Diagrama do Domínio API

A imagem do diagrama isométrico do Domínio API deve ser colocada em:

```
frontend/wiki/public/Arah-api-domain-diagram.png
```

### Especificações

- **Nome do arquivo**: `Arah-api-domain-diagram.png`
- **Formato**: PNG (ou SVG)
- **Tamanho recomendado**: 1200x800px (ou maior para melhor qualidade)
- **Descrição**: Diagrama isométrico mostrando o Arah API DOMAIN com o TERRITÓRIO no centro e conexões para FEED, MAP, HEALTH, FEATURES, MEMBERSHIP & GOVERNANCE, e AUTENTICAÇÃO

### Componente que usa esta imagem

- `frontend/wiki/components/content/ApiDomainDiagram.tsx`
- Exibido na página inicial (`/`) após o conteúdo de `ONBOARDING_PUBLICO.md`

### Fallback

Se a imagem não estiver disponível, o componente exibirá uma mensagem informativa indicando onde a imagem deve ser colocada.
