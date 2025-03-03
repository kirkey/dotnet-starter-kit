using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Todo
{
    /// <inheritdoc />
    public partial class UpdateEntityWithUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModified",
                schema: "todo",
                table: "Todos",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "Deleted",
                schema: "todo",
                table: "Todos",
                newName: "DeletedOn");

            migrationBuilder.RenameColumn(
                name: "Created",
                schema: "todo",
                table: "Todos",
                newName: "CreatedOn");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "todo",
                table: "Todos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                schema: "todo",
                table: "Todos",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                schema: "todo",
                table: "Todos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserName",
                schema: "todo",
                table: "Todos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "todo",
                table: "Todos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                schema: "todo",
                table: "Todos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedByUserName",
                schema: "todo",
                table: "Todos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "todo",
                table: "Todos",
                type: "VARCHAR(1024)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "todo",
                table: "Todos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                schema: "todo",
                table: "Todos",
                type: "VARCHAR(16)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "todo",
                table: "Todos",
                type: "VARCHAR(16)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "DeletedByUserName",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "FilePath",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserName",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "Remarks",
                schema: "todo",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "todo",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                schema: "todo",
                table: "Todos",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "DeletedOn",
                schema: "todo",
                table: "Todos",
                newName: "Deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "todo",
                table: "Todos",
                newName: "Created");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "todo",
                table: "Todos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                schema: "todo",
                table: "Todos",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);
        }
    }
}
