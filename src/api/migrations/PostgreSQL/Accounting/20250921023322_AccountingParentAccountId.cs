using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Accounting
{
    /// <inheritdoc />
    public partial class AccountingParentAccountId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubAccountOf",
                schema: "accounting",
                table: "ChartOfAccounts",
                newName: "ParentAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentAccountId",
                schema: "accounting",
                table: "ChartOfAccounts",
                newName: "SubAccountOf");
        }
    }
}
