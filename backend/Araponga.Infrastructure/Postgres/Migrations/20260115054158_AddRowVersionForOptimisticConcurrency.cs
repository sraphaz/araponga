using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Araponga.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersionForOptimisticConcurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "territory_memberships",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "territory_events",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "map_entities",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "community_posts",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "territory_memberships");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "territory_events");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "map_entities");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "community_posts");
        }
    }
}
