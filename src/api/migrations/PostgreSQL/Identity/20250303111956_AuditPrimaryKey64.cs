using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Identity
{
    /// <inheritdoc />
    public partial class AuditPrimaryKey64 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PrimaryKey",
                schema: "identity",
                table: "AuditTrails",
                type: "VARCHAR(64)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PrimaryKey",
                schema: "identity",
                table: "AuditTrails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(64)",
                oldNullable: true);
        }
    }
}
