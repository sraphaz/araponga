using Testcontainers.PostgreSql;
using Xunit;

namespace Araponga.Tests.TestHelpers;

/// <summary>
/// Test Container para PostgreSQL usando Testcontainers.
/// Inicia automaticamente um container PostgreSQL para testes de integração.
/// </summary>
public sealed class PostgresTestContainer : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    public PostgresTestContainer()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithDatabase("araponga_test")
            .WithUsername("araponga")
            .WithPassword("araponga")
            .WithCleanUp(true)
            .Build();
    }

    /// <summary>
    /// Obtém a connection string do container PostgreSQL.
    /// </summary>
    public string ConnectionString => _container.GetConnectionString();

    /// <summary>
    /// Inicializa o container PostgreSQL.
    /// </summary>
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    /// <summary>
    /// Para e remove o container PostgreSQL.
    /// </summary>
    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}
