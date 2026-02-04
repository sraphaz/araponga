# Plano de implementação – Todas as jornadas do BFF

Este documento descreve o plano para expor, via BFF, todas as jornadas (fluxos de usuário) que hoje existem na API principal, mantendo o BFF desacoplado (proxy por path), com cache quando fizer sentido e documentação alinhada ao `BffJourneyRegistry`.

---

## 1. Estado atual

### 1.1 Jornadas já no BFF (`/api/v2/journeys/`)

| Jornada      | Endpoints | Cache GET | Status   |
|-------------|-----------|-----------|----------|
| **onboarding** | suggested-territories, complete | 300s | ✅ Implementado |
| **feed**       | territory-feed, create-post, interact | 30s | ✅ Implementado |
| **events**     | territory-events, create-event, participate | 45s | ✅ Implementado |
| **marketplace**| search, add-to-cart, checkout | 60s | ✅ Implementado |

O middleware `JourneyProxyMiddleware` encaminha qualquer request em `/api/v2/journeys/*` para a API; o registro e o cache já cobrem essas quatro jornadas.

### 1.2 O que não está no BFF

Toda a superfície **`/api/v1/*`** (e demais rotas) não passa pelo BFF. O front que consome só o BFF hoje não tem acesso a: auth, connections, subscriptions, platform (territories, membership, devices, etc.), users (perfil, preferências), moderation, map, assets, notifications, media, chat, alerts.

---

## 2. Objetivo e critérios

- **Objetivo:** Ter no BFF uma visão por “jornadas” dos fluxos principais do produto, com proxy uniforme, cache onde for seguro e documentação única em `BffJourneyRegistry` + GET `/bff/journeys`.
- **Critérios por jornada:**
  - Escopo claro de endpoints (path + métodos).
  - Decisão de cache: quais GETs cachear e TTL (por path/prefixo).
  - Dependências (ex.: auth obrigatória, headers especiais).
  - Sem lógica de negócio no BFF: apenas proxy (e eventual agregação futura documentada).

---

## 3. Jornadas a implementar (além das atuais)

As jornadas abaixo são agrupadas por **fase** de implementação. Cada uma pode ser exposta no BFF como **prefixo sob `/api/v2/journeys/<nome>`**, reutilizando o mesmo `JourneyProxyMiddleware` e apenas registrando path + cache no BFF.

### Fase 1 – Fundação (auth + usuário)

Indispensáveis para o restante: login e contexto do usuário.

| Jornada   | Descrição | Endpoints API origem | Cache GET | Observações |
|-----------|-----------|----------------------|-----------|-------------|
| **auth**  | Login, refresh, 2FA, logout | `api/v1/auth/*` | Não | Sensível; sem cache. Pode ser proxy transparente ou BFF expor só `/api/v2/journeys/auth/login`, `refresh`, etc. |
| **me**    | Perfil, preferências, interesses, atividade, devices | `api/v1/users/me/*`, `api/v1/users/me/profile`, `preferences`, `interests`, `devices` | 60–120s para GET de perfil/preferências | Agrupa “tudo sobre o usuário logado”. |

**Decisão de implementação:**  
- **Opção A:** Manter auth e “me” na API e o front chamar a API diretamente para esses paths (BFF só para jornadas de produto).  
- **Opção B:** Incluir no BFF como jornadas `auth` e `me`, proxy para `api/v1/auth` e `api/v1/users/me` (path rewrite no middleware ou segundo proxy por prefixo).  

Recomendação: **Fase 1 documentar** as duas jornadas no plano e no `BffJourneyRegistry`; implementar proxy (Opção B) só se o front for consumir 100% via BFF.

---

### Fase 2 – Conexões e plataforma básica

| Jornada         | Descrição | Endpoints API origem | Cache GET | Observações |
|-----------------|-----------|----------------------|-----------|-------------|
| **connections** | Círculos/amigos: listar, solicitar, aceitar, remover | `api/v1/connections/*` | Listagens GET: 30–60s | Jornada de “rede social”. |
| **territories** | Listar territórios, detalhe, entrar (enter), features | `api/v1/territories/*`, `api/v1/territories/{id}/features` | Listagem/detalhe: 60–120s | Base para outras jornadas (feed, events, etc.). |
| **membership**  | Membership no território: become-resident, verify-residency, me, transfer | `api/v1/territories/{id}/membership`, `enter`, `memberships/*` | GET me/membership: 60s | Depende de auth. |

---

### Fase 3 – Conteúdo e descoberta

| Jornada        | Descrição | Endpoints API origem | Cache GET | Observações |
|----------------|-----------|----------------------|-----------|-------------|
| **feed-v1**    | Endpoints adicionais de feed (se houver) além da jornada feed v2 | `api/v1/feed/*` | Conforme endpoint | Só criar se existirem GETs em v1 não cobertos por territory-feed. |
| **map**        | Mapas, camadas, entidades no mapa | `api/v1/map/*` | 60–120s | Cache por viewport/zoom se a API suportar query params estáveis. |
| **assets**     | Listagem/busca de assets (mídia, anexos) | `api/v1/assets/*` | 60s para listagens | Evitar cache para upload/delete. |
| **media**      | Upload, download, config de mídia por território | `api/v1/media/*`, `api/v1/territories/{id}/media-config` | Config: 300s; listagens: 60s | Upload não cachear. |

---

### Fase 4 – Compras, assinaturas e notificações

| Jornada           | Descrição | Endpoints API origem | Cache GET | Observações |
|-------------------|-----------|----------------------|-----------|-------------|
| **subscriptions** | Planos, minha assinatura, capacidades | `api/v1/subscription-plans`, `api/v1/subscriptions`, `api/v1/subscriptions/me`, territórios `subscription-plans` | Planos e “me”: 120s | Webhooks (Stripe/MP) não passam pelo BFF (server-to-server). |
| **notifications** | Listar, marcar lida, config | `api/v1/notifications/*`, `api/v1/...` (notification config) | Listagem: 30s | Invalidação ou TTL curto. |
| **marketplace-v1**| Carrinho (detalhe), lojas, itens, avaliações (complementar à jornada marketplace) | `api/v1/cart`, `api/v1/stores`, `api/v1/items`, rating, etc. | Listagens: 60s | Jornada marketplace atual já cobre search/add-to-cart/checkout; esta fase cobre demais GETs de marketplace. |

---

### Fase 5 – Moderação, chat, alertas e admin

| Jornada       | Descrição | Endpoints API origem | Cache GET | Observações |
|---------------|-----------|----------------------|-----------|-------------|
| **moderation**| Work items, casos, evidências (território e admin) | `api/v1/territories/{id}/work-items`, `moderation/cases`, `evidences`, admin | Listagens: 30–60s | Depende de perfil (moderador/admin). |
| **chat**      | Canais, mensagens, territory chat | `api/v1/chat`, `api/v1/territories/{id}/chat` | Evitar cache em listagens recentes ou TTL muito curto (15–30s) | Pode exigir SSE/WebSocket no futuro; por ora só HTTP. |
| **alerts**    | Alertas do território/usuário | `api/v1/alerts/*` | 30–60s | |
| **admin**     | Configurações de sistema, seed, cache-metrics, verifications (admin) | `api/v1/admin/*` | Conforme endpoint; muitos sem cache | Apenas para clientes admin; pode ser a última fase. |

---

## 4. Abordagem técnica no BFF

### 4.1 Padrão atual (manter)

- Um único **JourneyProxyMiddleware** que:
  - Verifica se o path começa com `/api/v2/journeys/`.
  - Extrai `pathAndQuery` e encaminha para a API (`ApiBaseUrl` + path).
  - Para GET, opcionalmente usa cache (chave: path + query + auth) e TTL por prefixo em `BffOptions.CacheTtlByPath`.

### 4.2 Incluir novas jornadas

- **Opção 1 – Expandir prefixo único (recomendado):**  
  Continuar com um único prefixo `/api/v2/journeys/` e novos “nomes” de jornada:  
  `onboarding`, `feed`, `events`, `marketplace`, `auth`, `me`, `connections`, `territories`, `membership`, `map`, `assets`, `media`, `subscriptions`, `notifications`, `marketplace-v1`, `moderation`, `chat`, `alerts`, `admin`.

  - Na API, ou se mantém tudo em `api/v1/*` e o BFF faz **rewrite** de `/api/v2/journeys/<jornada>/...` → `api/v1/<equivalente>`, ou se criam controllers “journey” em v2 que delegam para os serviços atuais (como já é para onboarding, feed, events, marketplace).

- **Opção 2 – Dois prefixos no BFF:**  
  - `/api/v2/journeys/*` – jornadas já modeladas em v2 (onboarding, feed, events, marketplace).  
  - `/api/v2/proxy/v1/*` – proxy transparente para `api/v1/*`, com regras de cache por path (ex.: `v1/connections`, `v1/territories`).  

O plano recomenda **Opção 1** com registro explícito por jornada no `BffJourneyRegistry` e, na API, ou controllers journey adicionais em v2 que chamem os mesmos serviços, ou um único “proxy controller” em v2 que remapeie para v1 (evitando duplicar lógica).

### 4.3 Cache

- **Regra:** Só GET com resposta 2xx; chave inclui path + query + header de autorização.
- **TTL:** Por prefixo de path em `appsettings` (`CacheTtlByPath`); valores sugeridos por jornada estão nas tabelas acima.
- **Invalidação:** Por enquanto apenas TTL; não há invalidação por evento (pode ser evolução futura).

### 4.4 Autenticação

- O BFF repassa headers (incl. `Authorization`) para a API. A API continua responsável por validar token e retornar 401.
- Nenhuma mudança obrigatória na autenticação para o plano; quando houver “auth journey” no BFF, manter tokens e refresh apenas repassados à API.

---

## 5. Registro e documentação no BFF

- **BffJourneyRegistry:** Para cada nova jornada, adicionar constante (ex.: `Connections`, `Territories`), entrada em `AllPathPrefixes`, lista em `AllEndpoints` (path, method, description) e, quando houver GET cacheável, em `CacheableGetEndpoints` com TTL sugerido.
- **GET `/bff/journeys`:** Já retorna as jornadas a partir do registro; ao adicionar jornadas no registro, a documentação exposta atualiza automaticamente.
- **README ou doc no BFF:** Referenciar este plano e o `BACKEND_LAYERS_AND_NAMING.md` para contexto de “o que é jornada” e onde fica a API.

---

## 6. Cronograma sugerido (ordem de implementação)

| Ordem | Fase   | Jornadas | Entregável |
|-------|--------|----------|------------|
| 1     | –      | (já feito) | onboarding, feed, events, marketplace estáveis e documentados |
| 2     | 1      | auth, me | Decisão Opção A vs B; se B: proxy auth + me e registro no BFF |
| 3     | 2      | connections, territories, membership | Proxy + registro + cache GET; testes de integração BFF |
| 4     | 3      | map, assets, media (e feed-v1 se necessário) | Idem |
| 5     | 4      | subscriptions, notifications, marketplace-v1 | Idem; webhooks fora do BFF |
| 6     | 5      | moderation, chat, alerts, admin | Idem; admin por último |

Cada entrega pode ser uma branch + PR, com testes de proxy e, se aplicável, cobertura de cache para os GETs da jornada.

---

## 7. Checklist por jornada (ao implementar)

- [ ] Endpoints mapeados (path e método) na API atual.
- [ ] Entrada no `BffJourneyRegistry`: nome, `AllEndpoints`, e, se GET cacheável, `CacheableGetEndpoints` e TTL.
- [ ] `CacheTtlByPath` em `appsettings` (e Development) quando houver cache.
- [ ] Testes no BFF: proxy retorna mesmo status/body que a API (com mock ou API real em teste); cache hit/miss quando aplicável.
- [ ] Atualizar `Araponga.Api.Bff.http` com exemplos da nova jornada.
- [ ] Documentar no CHANGELOG ou no PR a nova jornada e qualquer decisão (ex.: “auth só documentada, proxy fica para fase 2”).

---

## 8. Resumo

- **Jornadas já no BFF:** onboarding, feed, events, marketplace (4).
- **Novas jornadas planejadas:** auth, me, connections, territories, membership, map, assets, media, subscriptions, notifications, marketplace-v1, moderation, chat, alerts, admin (15), em 5 fases.
- **Implementação:** manter um único middleware de proxy sob `/api/v2/journeys/`, registrar cada jornada no `BffJourneyRegistry`, configurar cache por path e documentar via GET `/bff/journeys`. Opção de proxy transparente para v1 (path rewrite) ou controllers journey v2 na API que delegam aos serviços existentes.

Este plano pode ser revisado conforme prioridade de produto (ex.: connections e subscriptions antes de admin).
