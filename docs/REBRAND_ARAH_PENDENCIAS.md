# Rebrand Araponga → Arah – pendências manuais

O rebrand foi aplicado na branch `chore/rebrand-araponga-to-arah`. A solução é `Arah.sln`, os projetos são `Arah.*`, imagens Docker `arah-api`/`arah-bff`, app Flutter `arah_app`, etc.

**Referências de caminho e texto já foram atualizadas** (Arah.sln, workflows, scripts, launch.json, Dockerfile, devportal, README, etc.) para `frontend/arah.app` e `backend/Arah.Api.Bff`.

## 1. Renomear as pastas (último passo manual)

No Windows, feche a IDE/processos que usem essas pastas e, na raiz do repo, execute:

```powershell
git mv frontend/araponga.app frontend/arah.app
git mv backend/Araponga.Api.Bff backend/Arah.Api.Bff
```

Se der *Permission denied*, feche o Cursor/VS Code, qualquer terminal com `cd` nessas pastas, e tente de novo em um PowerShell novo.

## Banco de dados

- Em desenvolvimento, o Postgres usa por padrão usuário/senha/DB **arah** (não mais `araponga`).
- Se você tinha dados no volume antigo, pode precisar recriar o banco ou ajustar variáveis (`POSTGRES_USER`, `POSTGRES_DB`, `POSTGRES_PASSWORD`) para o que estiver usando.

## Build e testes

- Backend: `dotnet build Arah.sln` e `dotnet test` a partir da raiz.
- Flutter: após renomear, `cd frontend/arah.app` e `flutter pub get` / `flutter run`.
