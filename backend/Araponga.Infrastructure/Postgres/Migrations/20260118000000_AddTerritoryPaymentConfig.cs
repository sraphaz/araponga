using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Araponga.Infrastructure.Postgres.Migrations;

/// <inheritdoc />
public partial class AddTerritoryPaymentConfig : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "territory_payment_configs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                TerritoryId = table.Column<Guid>(type: "uuid", nullable: false),
                GatewayProvider = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                IsActive = table.Column<bool>(type: "boolean", nullable: false),
                Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                MinimumAmount = table.Column<long>(type: "bigint", nullable: false),
                MaximumAmount = table.Column<long>(type: "bigint", nullable: true),
                ShowFeeBreakdown = table.Column<bool>(type: "boolean", nullable: false),
                FeeTransparencyLevel = table.Column<int>(type: "integer", nullable: false),
                CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_territory_payment_configs", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_territory_payment_configs_TerritoryId",
            table: "territory_payment_configs",
            column: "TerritoryId");

        migrationBuilder.CreateIndex(
            name: "IX_territory_payment_configs_TerritoryId_IsActive",
            table: "territory_payment_configs",
            columns: new[] { "TerritoryId", "IsActive" });

        // Adicionar coluna PaymentIntentId à tabela checkouts
        migrationBuilder.AddColumn<string>(
            name: "PaymentIntentId",
            table: "checkouts",
            type: "character varying(200)",
            maxLength: 200,
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_checkouts_PaymentIntentId",
            table: "checkouts",
            column: "PaymentIntentId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_checkouts_PaymentIntentId",
            table: "checkouts");

        migrationBuilder.DropColumn(
            name: "PaymentIntentId",
            table: "checkouts");

        migrationBuilder.DropTable(name: "territory_payment_configs");
    }
}
