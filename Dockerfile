# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1) Copiar solution e csproj (para cache)
COPY *.sln ./

COPY backend/Araponga.Api/*.csproj backend/Araponga.Api/
COPY backend/Araponga.Application/*.csproj backend/Araponga.Application/
COPY backend/Araponga.Domain/*.csproj backend/Araponga.Domain/
COPY backend/Araponga.Infrastructure/*.csproj backend/Araponga.Infrastructure/
COPY backend/Araponga.Shared/*.csproj backend/Araponga.Shared/
COPY backend/Araponga.Tests/*.csproj backend/Araponga.Tests/

# 2) Copiar lock files (importante para CI)
COPY backend/Araponga.Api/packages.lock.json backend/Araponga.Api/
COPY backend/Araponga.Application/packages.lock.json backend/Araponga.Application/
COPY backend/Araponga.Domain/packages.lock.json backend/Araponga.Domain/
COPY backend/Araponga.Infrastructure/packages.lock.json backend/Araponga.Infrastructure/
COPY backend/Araponga.Shared/packages.lock.json backend/Araponga.Shared/
COPY backend/Araponga.Tests/packages.lock.json backend/Araponga.Tests/

# 3) Restore (agora os pacotes realmente existem no container)
WORKDIR /src/backend/Araponga.Api
RUN dotnet restore --locked-mode

# 4) Copiar o resto do código
WORKDIR /src
COPY . .

# 5) Publish sem restore (agora funciona)
WORKDIR /src/backend/Araponga.Api
RUN dotnet publish -c Release -o /app/publish --no-restore


# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "Araponga.Api.dll"]
