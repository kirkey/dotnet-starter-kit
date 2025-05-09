using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.MySQL.Accounting
{
    /// <inheritdoc />
    public partial class AccountingAccountCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                schema: "accounting",
                table: "ChartOfAccounts",
                newName: "AccountCode");

            migrationBuilder.RenameIndex(
                name: "IX_ChartOfAccounts_Code",
                schema: "accounting",
                table: "ChartOfAccounts",
                newName: "IX_ChartOfAccounts_AccountCode");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "VARCHAR(2048)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedByUserName",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "VARCHAR(64)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "VARCHAR(2048)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DeletedByUserName",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "VARCHAR(64)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserName",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "VARCHAR(64)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountCode",
                schema: "accounting",
                table: "ChartOfAccounts",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_ChartOfAccounts_AccountCode",
                schema: "accounting",
                table: "ChartOfAccounts",
                newName: "IX_ChartOfAccounts_Code");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(2048)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedByUserName",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(64)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(2048)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DeletedByUserName",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(64)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserName",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(64)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
