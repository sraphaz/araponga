# Ará (App Flutter)

App mobile **território-first** e **comunidade-first**. Nome do app: **Ará**. Interface clean, leve e harmônica, com **fundo escuro** como padrão para conforto visual. Conecta ao **BFF** do repositório.

**Jornada:** Acesso sem login → seleção de território → feed da região; troca de território em Explorar; login opcional (para publicar, perfil e notificações).

## Requisitos

- [Flutter](https://flutter.dev) 3.19+ (stable)
- Dart 3.2+

## Configuração inicial (primeira vez)

Se a pasta do projeto foi criada manualmente (sem `flutter create`), gere os arquivos de plataforma:

```bash
cd frontend/araponga.app
flutter create . --project-name araponga_app --org com.araponga --platforms android,ios,web
```

Depois instale as dependências:

```bash
flutter pub get
```

## BFF

O app consome as jornadas do BFF em `/api/v2/journeys/*`. Configure a URL base:

- **Desenvolvimento:** por padrão usa `http://localhost:5001`. Ajuste no código em `lib/core/config/app_config.dart` ou use:

  ```bash
  flutter run --dart-define=BFF_BASE_URL=http://SEU_IP:PORTA
  ```

- **Android emulador:** use `http://10.0.2.2:5001` para localhost do host.
- **iOS simulador:** `http://localhost:5001` funciona.

Certifique-se de que o BFF está rodando (projeto `Araponga.Api.Bff` no backend).

## Estrutura principal

```
lib/
├── main.dart              # Entry + ProviderScope
├── app.dart               # MaterialApp + tema + go_router
├── app_router.dart        # Rotas (login, home shell)
├── core/
│   ├── config/            # AppConfig, BFF base URL
│   ├── network/           # BffClient (GET/POST jornadas)
│   └── theme/              # AppTheme (light/dark)
└── features/
    ├── auth/              # Login (jornada auth)
    ├── home/               # MainShellScreen (bottom nav)
    ├── feed/               # Feed do território (jornada feed)
    ├── explore/            # Explorar (territories, map, events)
    └── profile/            # Perfil (jornada me)
```

## Jornadas BFF utilizadas

| Jornada   | Uso no app                    |
|-----------|--------------------------------|
| auth      | Login, refresh, logout        |
| me        | Perfil, preferências, devices |
| feed      | Feed do território            |
| territories | Lista, explorar            |
| onboarding | Primeiro acesso (TODO)      |

## Rodar

```bash
cd frontend/araponga.app
flutter pub get
flutter run
```

## Próximos passos

Documentados em **[docs/NEXT_STEPS.md](docs/NEXT_STEPS.md)**. Já implementado: acesso sem login, território primeiro (seleção → feed da região), troca de território em Explorar, perfil com CTA "Entrar" para guest, tema escuro e nome **Ará**.

## Produção

- **BFF_BASE_URL:** Definir em build (e.g. `--dart-define=BFF_BASE_URL=https://bff.seudominio.com`).
- **Auth:** A API expõe login social (`auth/social`). O app usa provider `dev` para desenvolvimento (email + documento placeholder). Em produção, integrar Google/Apple Sign-In e chamar `auth/social` com os dados do provedor.
- **Tokens:** Guardados em `flutter_secure_storage` (KeyStore/Keychain). Logout limpa tokens.
- **401:** O cliente BFF chama logout automático e o router redireciona para `/login`.
- **Território:** ID do território ativo em `SharedPreferences` e enviado como `X-Session-Id` nas requisições.

## Documentação do projeto

- [Plano do frontend Flutter](../../docs/24_FLUTTER_FRONTEND_PLAN.md)
- [Diretrizes de design](../../docs/26_FLUTTER_DESIGN_GUIDELINES.md)
- [Roadmap de implementação](../../docs/25_FLUTTER_IMPLEMENTATION_ROADMAP.md)
- [Alinhamento API/BFF](../../docs/34_FLUTTER_API_STRATEGIC_ALIGNMENT.md)
- [Exemplo BFF Flutter](../../docs/BFF_FLUTTER_EXAMPLE.md)
