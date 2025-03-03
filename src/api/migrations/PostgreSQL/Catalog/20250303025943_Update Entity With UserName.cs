using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Catalog
{
    /// <inheritdoc />
    public partial class UpdateEntityWithUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModified",
                schema: "catalog",
                table: "Products",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "Deleted",
                schema: "catalog",
                table: "Products",
                newName: "DeletedOn");

            migrationBuilder.RenameColumn(
                name: "Created",
                schema: "catalog",
                table: "Products",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                schema: "catalog",
                table: "Brands",
                newName: "LastModifiedOn");

            migrationBuilder.RenameColumn(
                name: "Deleted",
                schema: "catalog",
                table: "Brands",
                newName: "DeletedOn");

            migrationBuilder.RenameColumn(
                name: "Created",
                schema: "catalog",
                table: "Brands",
                newName: "CreatedOn");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                schema: "catalog",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserName",
                schema: "catalog",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                schema: "catalog",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedByUserName",
                schema: "catalog",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "catalog",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                schema: "catalog",
                table: "Products",
                type: "VARCHAR(16)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "catalog",
                table: "Products",
                type: "VARCHAR(16)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "catalog",
                table: "Brands",
                type: "VARCHAR(1024)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserName",
                schema: "catalog",
                table: "Brands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserName",
                schema: "catalog",
                table: "Brands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                schema: "catalog",
                table: "Brands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedByUserName",
                schema: "catalog",
                table: "Brands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "catalog",
                table: "Brands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                schema: "catalog",
                table: "Brands",
                type: "VARCHAR(16)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "catalog",
                table: "Brands",
                type: "VARCHAR(16)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedByUserName",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FilePath",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserName",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Remarks",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedByUserName",
                schema: "catalog",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "DeletedByUserName",
                schema: "catalog",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "FilePath",
                schema: "catalog",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserName",
                schema: "catalog",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "catalog",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Remarks",
                schema: "catalog",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "catalog",
                table: "Brands");

            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                schema: "catalog",
                table: "Products",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "DeletedOn",
                schema: "catalog",
                table: "Products",
                newName: "Deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "catalog",
                table: "Products",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "LastModifiedOn",
                schema: "catalog",
                table: "Brands",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "DeletedOn",
                schema: "catalog",
                table: "Brands",
                newName: "Deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "catalog",
                table: "Brands",
                newName: "Created");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "catalog",
                table: "Brands",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(1024)",
                oldMaxLength: 100);
        }
    }
}
