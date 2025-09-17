using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Store
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "store");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ParentCategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalSchema: "store",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ContactPerson = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CreditLimit = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    PaymentTermsDays = table.Column<int>(type: "integer", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 16, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    TaxNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BusinessLicense = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastOrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LifetimeValue = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2000, nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceLists",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PriceListName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PriceListType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    MinimumOrderValue = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: true),
                    CustomerType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2000, nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_PriceLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ContactPerson = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Website = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreditLimit = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: true),
                    PaymentTermsDays = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Rating = table.Column<decimal>(type: "numeric(3,2)", precision: 16, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2000, nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ManagerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ManagerEmail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ManagerPhone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TotalCapacity = table.Column<decimal>(type: "numeric(18,3)", precision: 16, scale: 2, nullable: false),
                    UsedCapacity = table.Column<decimal>(type: "numeric(18,3)", precision: 16, scale: 2, nullable: false, defaultValue: 0m),
                    CapacityUnit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsMainWarehouse = table.Column<bool>(type: "boolean", nullable: false),
                    LastInventoryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WholesaleContracts",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContractNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MinimumOrderValue = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    VolumeDiscountPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 16, scale: 2, nullable: false),
                    PaymentTermsDays = table.Column<int>(type: "integer", nullable: false),
                    CreditLimit = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    DeliveryTerms = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ContractTerms = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    AutoRenewal = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_WholesaleContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WholesaleContracts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "store",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpectedDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    NetAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    DeliveryAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ContactPerson = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ContactPhone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsUrgent = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2000, nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "store",
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrders",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OrderType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SubTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    ShippingAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    PaymentStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DeliveryAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsUrgent = table.Column<bool>(type: "boolean", nullable: false),
                    SalesPersonId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2000, nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_SalesOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "store",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesOrders_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "store",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseLocations",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Aisle = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Section = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Shelf = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Bin = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Capacity = table.Column<decimal>(type: "numeric(18,3)", precision: 16, scale: 2, nullable: false),
                    UsedCapacity = table.Column<decimal>(type: "numeric(18,3)", precision: 16, scale: 2, nullable: false, defaultValue: 0m),
                    CapacityUnit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    RequiresTemperatureControl = table.Column<bool>(type: "boolean", nullable: false),
                    MinTemperature = table.Column<decimal>(type: "numeric(5,2)", precision: 16, scale: 2, nullable: true),
                    MaxTemperature = table.Column<decimal>(type: "numeric(5,2)", precision: 16, scale: 2, nullable: true),
                    TemperatureUnit = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_WarehouseLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseLocations_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "store",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CycleCounts",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CountNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    ScheduledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActualStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CountType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CounterName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SupervisorName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TotalItemsToCount = table.Column<int>(type: "integer", nullable: false),
                    ItemsCountedCorrect = table.Column<int>(type: "integer", nullable: false),
                    ItemsWithDiscrepancies = table.Column<int>(type: "integer", nullable: false),
                    AccuracyPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 16, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2000, nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_CycleCounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CycleCounts_WarehouseLocations_WarehouseLocationId",
                        column: x => x.WarehouseLocationId,
                        principalSchema: "store",
                        principalTable: "WarehouseLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CycleCounts_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "store",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroceryItems",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SKU = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Barcode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    MinimumStock = table.Column<int>(type: "integer", nullable: false),
                    MaximumStock = table.Column<int>(type: "integer", nullable: false),
                    CurrentStock = table.Column<int>(type: "integer", nullable: false),
                    ReorderPoint = table.Column<int>(type: "integer", nullable: false),
                    IsPerishable = table.Column<bool>(type: "boolean", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Manufacturer = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Weight = table.Column<decimal>(type: "numeric(18,3)", precision: 16, scale: 2, nullable: false),
                    WeightUnit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: true),
                    WarehouseLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_GroceryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroceryItems_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "store",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroceryItems_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "store",
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroceryItems_WarehouseLocations_WarehouseLocationId",
                        column: x => x.WarehouseLocationId,
                        principalSchema: "store",
                        principalTable: "WarehouseLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransfers",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    TransferNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FromWarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToWarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    ToLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    TransferDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpectedArrivalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualArrivalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TransferType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Priority = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TotalValue = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    TransportMethod = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TrackingNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RequestedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ApprovedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2000, nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_InventoryTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransfers_WarehouseLocations_FromLocationId",
                        column: x => x.FromLocationId,
                        principalSchema: "store",
                        principalTable: "WarehouseLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_InventoryTransfers_WarehouseLocations_ToLocationId",
                        column: x => x.ToLocationId,
                        principalSchema: "store",
                        principalTable: "WarehouseLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_InventoryTransfers_Warehouses_FromWarehouseId",
                        column: x => x.FromWarehouseId,
                        principalSchema: "store",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransfers_Warehouses_ToWarehouseId",
                        column: x => x.ToWarehouseId,
                        principalSchema: "store",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CycleCountItems",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CycleCountId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroceryItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    SystemQuantity = table.Column<int>(type: "integer", nullable: false),
                    CountedQuantity = table.Column<int>(type: "integer", nullable: true),
                    VarianceQuantity = table.Column<int>(type: "integer", nullable: true),
                    CountDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CountedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RequiresRecount = table.Column<bool>(type: "boolean", nullable: false),
                    RecountReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 1000, nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_CycleCountItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CycleCountItems_CycleCounts_CycleCountId",
                        column: x => x.CycleCountId,
                        principalSchema: "store",
                        principalTable: "CycleCounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CycleCountItems_GroceryItems_GroceryItemId",
                        column: x => x.GroceryItemId,
                        principalSchema: "store",
                        principalTable: "GroceryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransactions",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    GroceryItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: true),
                    WarehouseLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    TransactionType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Reason = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    QuantityBefore = table.Column<int>(type: "integer", nullable: false),
                    QuantityAfter = table.Column<int>(type: "integer", nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    TotalCost = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PerformedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    ApprovedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2000, nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_InventoryTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_GroceryItems_GroceryItemId",
                        column: x => x.GroceryItemId,
                        principalSchema: "store",
                        principalTable: "GroceryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "store",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_WarehouseLocations_WarehouseLocationId",
                        column: x => x.WarehouseLocationId,
                        principalSchema: "store",
                        principalTable: "WarehouseLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "store",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PriceListItems",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PriceListId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroceryItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 16, scale: 2, nullable: true),
                    MinimumQuantity = table.Column<decimal>(type: "numeric(18,3)", precision: 16, scale: 2, nullable: true),
                    MaximumQuantity = table.Column<decimal>(type: "numeric(18,3)", precision: 16, scale: 2, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_PriceListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceListItems_GroceryItems_GroceryItemId",
                        column: x => x.GroceryItemId,
                        principalSchema: "store",
                        principalTable: "GroceryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceListItems_PriceLists_PriceListId",
                        column: x => x.PriceListId,
                        principalSchema: "store",
                        principalTable: "PriceLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderItems",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroceryItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    ReceivedQuantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 1000, nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_PurchaseOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_GroceryItems_GroceryItemId",
                        column: x => x.GroceryItemId,
                        principalSchema: "store",
                        principalTable: "GroceryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "store",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrderItems",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SalesOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroceryItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false, defaultValue: 0m),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    ShippedQuantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsWholesaleItem = table.Column<bool>(type: "boolean", nullable: false),
                    WholesaleTierPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 1000, nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_SalesOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrderItems_GroceryItems_GroceryItemId",
                        column: x => x.GroceryItemId,
                        principalSchema: "store",
                        principalTable: "GroceryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesOrderItems_SalesOrders_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalSchema: "store",
                        principalTable: "SalesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockAdjustments",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AdjustmentNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    GroceryItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    AdjustmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AdjustmentType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Reason = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    QuantityBefore = table.Column<int>(type: "integer", nullable: false),
                    AdjustmentQuantity = table.Column<int>(type: "integer", nullable: false),
                    QuantityAfter = table.Column<int>(type: "integer", nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false, defaultValue: 0m),
                    TotalCostImpact = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false, defaultValue: 0m),
                    Reference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AdjustedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ApprovedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    BatchNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2000, nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_StockAdjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockAdjustments_GroceryItems_GroceryItemId",
                        column: x => x.GroceryItemId,
                        principalSchema: "store",
                        principalTable: "GroceryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockAdjustments_WarehouseLocations_WarehouseLocationId",
                        column: x => x.WarehouseLocationId,
                        principalSchema: "store",
                        principalTable: "WarehouseLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StockAdjustments_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "store",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WholesalePricings",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WholesaleContractId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroceryItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    MinimumQuantity = table.Column<int>(type: "integer", nullable: false),
                    MaximumQuantity = table.Column<int>(type: "integer", nullable: true),
                    TierPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 16, scale: 2, nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_WholesalePricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WholesalePricings_GroceryItems_GroceryItemId",
                        column: x => x.GroceryItemId,
                        principalSchema: "store",
                        principalTable: "GroceryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WholesalePricings_WholesaleContracts_WholesaleContractId",
                        column: x => x.WholesaleContractId,
                        principalSchema: "store",
                        principalTable: "WholesaleContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransferItems",
                schema: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InventoryTransferId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroceryItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    LineTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_InventoryTransferItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransferItems_GroceryItems_GroceryItemId",
                        column: x => x.GroceryItemId,
                        principalSchema: "store",
                        principalTable: "GroceryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransferItems_InventoryTransfers_InventoryTransfer~",
                        column: x => x.InventoryTransferId,
                        principalSchema: "store",
                        principalTable: "InventoryTransfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Code",
                schema: "store",
                table: "Categories",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                schema: "store",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Code",
                schema: "store",
                table: "Customers",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CycleCountItems_CycleCountId",
                schema: "store",
                table: "CycleCountItems",
                column: "CycleCountId");

            migrationBuilder.CreateIndex(
                name: "IX_CycleCountItems_GroceryItemId",
                schema: "store",
                table: "CycleCountItems",
                column: "GroceryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CycleCounts_CountNumber",
                schema: "store",
                table: "CycleCounts",
                column: "CountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CycleCounts_WarehouseId",
                schema: "store",
                table: "CycleCounts",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_CycleCounts_WarehouseLocationId",
                schema: "store",
                table: "CycleCounts",
                column: "WarehouseLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_GroceryItems_Barcode",
                schema: "store",
                table: "GroceryItems",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroceryItems_CategoryId",
                schema: "store",
                table: "GroceryItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GroceryItems_SKU",
                schema: "store",
                table: "GroceryItems",
                column: "SKU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroceryItems_SupplierId",
                schema: "store",
                table: "GroceryItems",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_GroceryItems_WarehouseLocationId",
                schema: "store",
                table: "GroceryItems",
                column: "WarehouseLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_GroceryItemId",
                schema: "store",
                table: "InventoryTransactions",
                column: "GroceryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_PurchaseOrderId",
                schema: "store",
                table: "InventoryTransactions",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_TransactionNumber",
                schema: "store",
                table: "InventoryTransactions",
                column: "TransactionNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_WarehouseId",
                schema: "store",
                table: "InventoryTransactions",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_WarehouseLocationId",
                schema: "store",
                table: "InventoryTransactions",
                column: "WarehouseLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferItems_GroceryItemId",
                schema: "store",
                table: "InventoryTransferItems",
                column: "GroceryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferItems_InventoryTransferId",
                schema: "store",
                table: "InventoryTransferItems",
                column: "InventoryTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfers_FromLocationId",
                schema: "store",
                table: "InventoryTransfers",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfers_FromWarehouseId",
                schema: "store",
                table: "InventoryTransfers",
                column: "FromWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfers_ToLocationId",
                schema: "store",
                table: "InventoryTransfers",
                column: "ToLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfers_ToWarehouseId",
                schema: "store",
                table: "InventoryTransfers",
                column: "ToWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransfers_TransferNumber",
                schema: "store",
                table: "InventoryTransfers",
                column: "TransferNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_GroceryItemId",
                schema: "store",
                table: "PriceListItems",
                column: "GroceryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_PriceListId",
                schema: "store",
                table: "PriceListItems",
                column: "PriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_GroceryItemId",
                schema: "store",
                table: "PurchaseOrderItems",
                column: "GroceryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_PurchaseOrderId",
                schema: "store",
                table: "PurchaseOrderItems",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_OrderNumber",
                schema: "store",
                table: "PurchaseOrders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SupplierId",
                schema: "store",
                table: "PurchaseOrders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderItems_GroceryItemId",
                schema: "store",
                table: "SalesOrderItems",
                column: "GroceryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderItems_SalesOrderId",
                schema: "store",
                table: "SalesOrderItems",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CustomerId",
                schema: "store",
                table: "SalesOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_OrderNumber",
                schema: "store",
                table: "SalesOrders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_WarehouseId",
                schema: "store",
                table: "SalesOrders",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_AdjustmentNumber",
                schema: "store",
                table: "StockAdjustments",
                column: "AdjustmentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_GroceryItemId",
                schema: "store",
                table: "StockAdjustments",
                column: "GroceryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_WarehouseId",
                schema: "store",
                table: "StockAdjustments",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustments_WarehouseLocationId",
                schema: "store",
                table: "StockAdjustments",
                column: "WarehouseLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Code",
                schema: "store",
                table: "Suppliers",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseLocations_Code",
                schema: "store",
                table: "WarehouseLocations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseLocations_WarehouseId",
                schema: "store",
                table: "WarehouseLocations",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_Code",
                schema: "store",
                table: "Warehouses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WholesaleContracts_ContractNumber",
                schema: "store",
                table: "WholesaleContracts",
                column: "ContractNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WholesaleContracts_CustomerId",
                schema: "store",
                table: "WholesaleContracts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_WholesalePricings_GroceryItemId",
                schema: "store",
                table: "WholesalePricings",
                column: "GroceryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WholesalePricings_WholesaleContractId",
                schema: "store",
                table: "WholesalePricings",
                column: "WholesaleContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CycleCountItems",
                schema: "store");

            migrationBuilder.DropTable(
                name: "InventoryTransactions",
                schema: "store");

            migrationBuilder.DropTable(
                name: "InventoryTransferItems",
                schema: "store");

            migrationBuilder.DropTable(
                name: "PriceListItems",
                schema: "store");

            migrationBuilder.DropTable(
                name: "PurchaseOrderItems",
                schema: "store");

            migrationBuilder.DropTable(
                name: "SalesOrderItems",
                schema: "store");

            migrationBuilder.DropTable(
                name: "StockAdjustments",
                schema: "store");

            migrationBuilder.DropTable(
                name: "WholesalePricings",
                schema: "store");

            migrationBuilder.DropTable(
                name: "CycleCounts",
                schema: "store");

            migrationBuilder.DropTable(
                name: "InventoryTransfers",
                schema: "store");

            migrationBuilder.DropTable(
                name: "PriceLists",
                schema: "store");

            migrationBuilder.DropTable(
                name: "PurchaseOrders",
                schema: "store");

            migrationBuilder.DropTable(
                name: "SalesOrders",
                schema: "store");

            migrationBuilder.DropTable(
                name: "GroceryItems",
                schema: "store");

            migrationBuilder.DropTable(
                name: "WholesaleContracts",
                schema: "store");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "store");

            migrationBuilder.DropTable(
                name: "Suppliers",
                schema: "store");

            migrationBuilder.DropTable(
                name: "WarehouseLocations",
                schema: "store");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "store");

            migrationBuilder.DropTable(
                name: "Warehouses",
                schema: "store");
        }
    }
}
