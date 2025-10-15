using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Accounting
{
    /// <inheritdoc />
    public partial class AccountingCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Checks",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CheckNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    BankAccountCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    BankAccountName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    PayeeName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    VendorId = table.Column<Guid>(type: "uuid", nullable: true),
                    PayeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    IssuedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClearedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VoidedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VoidReason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    PaymentId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExpenseId = table.Column<Guid>(type: "uuid", nullable: true),
                    Memo = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    IsPrinted = table.Column<bool>(type: "boolean", nullable: false),
                    PrintedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PrintedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    IsStopPayment = table.Column<bool>(type: "boolean", nullable: false),
                    StopPaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StopPaymentReason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 1024, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 1024, nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserName = table.Column<string>(type: "VARCHAR(64)", nullable: true),
                    LastModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedByUserName = table.Column<string>(type: "VARCHAR(64)", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedByUserName = table.Column<string>(type: "VARCHAR(64)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Checks_BankAccountCode",
                schema: "accounting",
                table: "Checks",
                column: "BankAccountCode");

            migrationBuilder.CreateIndex(
                name: "IX_Checks_CheckNumber_BankAccount",
                schema: "accounting",
                table: "Checks",
                columns: new[] { "CheckNumber", "BankAccountCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Checks_IssuedDate",
                schema: "accounting",
                table: "Checks",
                column: "IssuedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Checks_PayeeId",
                schema: "accounting",
                table: "Checks",
                column: "PayeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Checks_Status",
                schema: "accounting",
                table: "Checks",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Checks_VendorId",
                schema: "accounting",
                table: "Checks",
                column: "VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Checks",
                schema: "accounting");
        }
    }
}
