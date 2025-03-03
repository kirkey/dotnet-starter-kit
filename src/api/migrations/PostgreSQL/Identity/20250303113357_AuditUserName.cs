using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Identity
{
    /// <inheritdoc />
    public partial class AuditUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PrimaryKey",
                schema: "identity",
                table: "AuditTrails",
                type: "VARCHAR(64)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(64)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                schema: "identity",
                table: "AuditTrails",
                type: "VARCHAR(1024)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                schema: "identity",
                table: "AuditTrails");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryKey",
                schema: "identity",
                table: "AuditTrails",
                type: "VARCHAR(64)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(64)");
        }
    }
}
