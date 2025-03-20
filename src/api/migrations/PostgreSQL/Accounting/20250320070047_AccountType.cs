using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Accounting
{
    /// <inheritdoc />
    public partial class AccountType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "accounting",
                table: "accounts");

            migrationBuilder.AddColumn<string>(
                name: "AccountType",
                schema: "accounting",
                table: "accounts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountType",
                schema: "accounting",
                table: "accounts");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "accounting",
                table: "accounts",
                type: "VARCHAR(64)",
                nullable: false,
                defaultValue: "");
        }
    }
}
