using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Todo
{
    /// <inheritdoc />
    public partial class TodoTitleToName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "todo",
                table: "Todos");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "todo",
                table: "Todos",
                type: "VARCHAR(32)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(16)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                schema: "todo",
                table: "Todos",
                type: "VARCHAR(32)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(16)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                schema: "todo",
                table: "Todos",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "todo",
                table: "Todos",
                type: "VARCHAR(64)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "todo",
                table: "Todos");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "todo",
                table: "Todos",
                type: "VARCHAR(16)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(32)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                schema: "todo",
                table: "Todos",
                type: "VARCHAR(16)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(32)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                schema: "todo",
                table: "Todos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                schema: "todo",
                table: "Todos",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "todo",
                table: "Todos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
