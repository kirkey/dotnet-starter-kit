using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Identity
{
    /// <inheritdoc />
    public partial class AddIdentityDeviceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                schema: "identity",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDateTime",
                schema: "identity",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastLoginDeviceType",
                schema: "identity",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastLoginIp",
                schema: "identity",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastLoginLocation",
                schema: "identity",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLoginDateTime",
                schema: "identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLoginDeviceType",
                schema: "identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLoginIp",
                schema: "identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLoginLocation",
                schema: "identity",
                table: "Users");
        }
    }
}
