using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Store
{
    /// <inheritdoc />
    public partial class WarehouseRemovedAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                schema: "store",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Country",
                schema: "store",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                schema: "store",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "store",
                table: "Warehouses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "store",
                table: "Warehouses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                schema: "store",
                table: "Warehouses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                schema: "store",
                table: "Warehouses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                schema: "store",
                table: "Warehouses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
