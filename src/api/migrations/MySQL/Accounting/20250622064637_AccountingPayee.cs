using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.MySQL.Accounting
{
    /// <inheritdoc />
    public partial class AccountingPayee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                schema: "accounting",
                table: "ChartOfAccounts");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "accounting",
                table: "ChartOfAccounts");

            migrationBuilder.CreateTable(
                name: "Payees",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PayeeCode = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpenseAccountCode = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpenseAccountName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tin = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 1024, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedByUserName = table.Column<string>(type: "VARCHAR(64)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedOn = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    LastModifiedByUserName = table.Column<string>(type: "VARCHAR(64)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DeletedByUserName = table.Column<string>(type: "VARCHAR(64)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payees", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Payees_PayeeCode",
                schema: "accounting",
                table: "Payees",
                column: "PayeeCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payees",
                schema: "accounting");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "VARCHAR(32)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "VARCHAR(32)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
