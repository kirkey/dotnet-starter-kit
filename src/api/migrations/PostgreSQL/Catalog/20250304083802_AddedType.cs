using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class AddedType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "catalog",
                table: "Products",
                type: "VARCHAR(32)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(16)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                schema: "catalog",
                table: "Products",
                type: "VARCHAR(32)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(16)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "catalog",
                table: "Products",
                type: "VARCHAR(1024)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "catalog",
                table: "Products",
                type: "VARCHAR(64)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "catalog",
                table: "Brands",
                type: "VARCHAR(32)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(16)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                schema: "catalog",
                table: "Brands",
                type: "VARCHAR(32)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(16)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "catalog",
                table: "Brands",
                type: "VARCHAR(64)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "catalog",
                table: "Brands");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "catalog",
                table: "Products",
                type: "VARCHAR(16)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(32)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                schema: "catalog",
                table: "Products",
                type: "VARCHAR(16)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(32)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "catalog",
                table: "Products",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(1024)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "catalog",
                table: "Brands",
                type: "VARCHAR(16)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(32)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                schema: "catalog",
                table: "Brands",
                type: "VARCHAR(16)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(32)",
                oldNullable: true);
        }
    }
}
