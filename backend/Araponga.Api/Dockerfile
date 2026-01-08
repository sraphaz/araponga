FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Araponga.sln ./
COPY backend/Araponga.Api/Araponga.Api.csproj backend/Araponga.Api/
COPY backend/Araponga.Application/Araponga.Application.csproj backend/Araponga.Application/
COPY backend/Araponga.Domain/Araponga.Domain.csproj backend/Araponga.Domain/
COPY backend/Araponga.Infrastructure/Araponga.Infrastructure.csproj backend/Araponga.Infrastructure/
COPY backend/Araponga.Shared/Araponga.Shared.csproj backend/Araponga.Shared/
COPY backend/Araponga.Tests/Araponga.Tests.csproj backend/Araponga.Tests/

RUN dotnet restore Araponga.sln

COPY . .
WORKDIR /src/backend/Araponga.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Araponga.Api.dll"]
