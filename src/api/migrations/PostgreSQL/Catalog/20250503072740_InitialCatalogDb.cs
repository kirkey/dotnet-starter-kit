#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Catalog;

/// <inheritdoc />
public partial class InitialCatalogDb : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "catalog");

        migrationBuilder.CreateTable(
            name: "Brands",
            schema: "catalog",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 100, nullable: false),
                Remarks = table.Column<string>(type: "VARCHAR(32)", nullable: true),
                Status = table.Column<string>(type: "VARCHAR(32)", nullable: true),
                Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                Notes = table.Column<string>(type: "text", nullable: true),
                FilePath = table.Column<string>(type: "text", nullable: true),
                CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedByUserName = table.Column<string>(type: "text", nullable: true),
                LastModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                LastModifiedByUserName = table.Column<string>(type: "text", nullable: true),
                DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                DeletedByUserName = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Brands", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Products",
            schema: "catalog",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Price = table.Column<decimal>(type: "numeric", nullable: false),
                BrandId = table.Column<Guid>(type: "uuid", nullable: true),
                TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 100, nullable: false),
                Remarks = table.Column<string>(type: "VARCHAR(32)", nullable: true),
                Status = table.Column<string>(type: "VARCHAR(32)", nullable: true),
                Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                Notes = table.Column<string>(type: "text", nullable: true),
                FilePath = table.Column<string>(type: "text", nullable: true),
                CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedByUserName = table.Column<string>(type: "text", nullable: true),
                LastModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                LastModifiedByUserName = table.Column<string>(type: "text", nullable: true),
                DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                DeletedByUserName = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Products", x => x.Id);
                table.ForeignKey(
                    name: "FK_Products_Brands_BrandId",
                    column: x => x.BrandId,
                    principalSchema: "catalog",
                    principalTable: "Brands",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Products_BrandId",
            schema: "catalog",
            table: "Products",
            column: "BrandId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Products",
            schema: "catalog");

        migrationBuilder.DropTable(
            name: "Brands",
            schema: "catalog");
    }
}