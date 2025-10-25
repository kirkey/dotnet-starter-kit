﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Store
{
    /// <inheritdoc />
    public partial class GoodsRecceipt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CycleCountItems_CycleCountId",
                schema: "store",
                table: "CycleCountItems");

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                schema: "store",
                table: "GoodsReceipts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseLocationId",
                schema: "store",
                table: "GoodsReceipts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "store",
                table: "GoodsReceiptItems",
                type: "VARCHAR(1024)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseOrderItemId",
                schema: "store",
                table: "GoodsReceiptItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitCost",
                schema: "store",
                table: "GoodsReceiptItems",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipts_PurchaseOrderId",
                schema: "store",
                table: "GoodsReceipts",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipts_WarehouseId",
                schema: "store",
                table: "GoodsReceipts",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceiptItems_PurchaseOrderItemId",
                schema: "store",
                table: "GoodsReceiptItems",
                column: "PurchaseOrderItemId");

            // Remove duplicate CycleCountItems before creating unique index
            migrationBuilder.Sql(@"
                DELETE FROM store.""CycleCountItems"" a
                USING store.""CycleCountItems"" b
                WHERE a.""Id"" > b.""Id""
                  AND a.""CycleCountId"" = b.""CycleCountId""
                  AND a.""ItemId"" = b.""ItemId"";
            ");

            migrationBuilder.CreateIndex(
                name: "IX_CycleCountItems_CycleCountId_ItemId",
                schema: "store",
                table: "CycleCountItems",
                columns: new[] { "CycleCountId", "ItemId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GoodsReceipts_PurchaseOrderId",
                schema: "store",
                table: "GoodsReceipts");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceipts_WarehouseId",
                schema: "store",
                table: "GoodsReceipts");

            migrationBuilder.DropIndex(
                name: "IX_GoodsReceiptItems_PurchaseOrderItemId",
                schema: "store",
                table: "GoodsReceiptItems");

            migrationBuilder.DropIndex(
                name: "IX_CycleCountItems_CycleCountId_ItemId",
                schema: "store",
                table: "CycleCountItems");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                schema: "store",
                table: "GoodsReceipts");

            migrationBuilder.DropColumn(
                name: "WarehouseLocationId",
                schema: "store",
                table: "GoodsReceipts");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderItemId",
                schema: "store",
                table: "GoodsReceiptItems");

            migrationBuilder.DropColumn(
                name: "UnitCost",
                schema: "store",
                table: "GoodsReceiptItems");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "store",
                table: "GoodsReceiptItems",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(1024)",
                oldMaxLength: 200);

            migrationBuilder.CreateIndex(
                name: "IX_CycleCountItems_CycleCountId",
                schema: "store",
                table: "CycleCountItems",
                column: "CycleCountId");
        }
    }
}
