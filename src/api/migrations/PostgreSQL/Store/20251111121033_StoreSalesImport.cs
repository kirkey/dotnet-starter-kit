using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Store
{
    /// <inheritdoc />
    public partial class StoreSalesImport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesImports",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ImportNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ImportDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SalesPeriodFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SalesPeriodTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TotalRecords = table.Column<int>(type: "integer", nullable: false),
                    ProcessedRecords = table.Column<int>(type: "integer", nullable: false),
                    ErrorRecords = table.Column<int>(type: "integer", nullable: false),
                    TotalQuantity = table.Column<int>(type: "integer", nullable: false),
                    TotalValue = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsReversed = table.Column<bool>(type: "boolean", nullable: false),
                    ReversedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReversedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ReversalReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ProcessedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ErrorMessage = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
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
                    table.PrimaryKey("PK_SalesImports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesImports_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "store",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalesImportItems",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SalesImportId = table.Column<Guid>(type: "uuid", nullable: false),
                    LineNumber = table.Column<int>(type: "integer", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Barcode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ItemName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    QuantitySold = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    InventoryTransactionId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsProcessed = table.Column<bool>(type: "boolean", nullable: false),
                    HasError = table.Column<bool>(type: "boolean", nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
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
                    table.PrimaryKey("PK_SalesImportItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesImportItems_InventoryTransactions_InventoryTransaction~",
                        column: x => x.InventoryTransactionId,
                        principalSchema: "store",
                        principalTable: "InventoryTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesImportItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "store",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesImportItems_SalesImports_SalesImportId",
                        column: x => x.SalesImportId,
                        principalSchema: "store",
                        principalTable: "SalesImports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesImportItems_Barcode",
                schema: "store",
                table: "SalesImportItems",
                column: "Barcode");

            migrationBuilder.CreateIndex(
                name: "IX_SalesImportItems_InventoryTransactionId",
                schema: "store",
                table: "SalesImportItems",
                column: "InventoryTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesImportItems_ItemId",
                schema: "store",
                table: "SalesImportItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesImportItems_SalesImportId",
                schema: "store",
                table: "SalesImportItems",
                column: "SalesImportId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesImportItems_SalesImportId_LineNumber",
                schema: "store",
                table: "SalesImportItems",
                columns: new[] { "SalesImportId", "LineNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesImports_ImportDate",
                schema: "store",
                table: "SalesImports",
                column: "ImportDate");

            migrationBuilder.CreateIndex(
                name: "IX_SalesImports_ImportNumber",
                schema: "store",
                table: "SalesImports",
                column: "ImportNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesImports_SalesPeriodFrom_SalesPeriodTo",
                schema: "store",
                table: "SalesImports",
                columns: new[] { "SalesPeriodFrom", "SalesPeriodTo" });

            migrationBuilder.CreateIndex(
                name: "IX_SalesImports_Status",
                schema: "store",
                table: "SalesImports",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SalesImports_WarehouseId",
                schema: "store",
                table: "SalesImports",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesImportItems",
                schema: "store");

            migrationBuilder.DropTable(
                name: "SalesImports",
                schema: "store");
        }
    }
}
