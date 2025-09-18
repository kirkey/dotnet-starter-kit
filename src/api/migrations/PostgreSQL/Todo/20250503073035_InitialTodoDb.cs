#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Todo;

/// <inheritdoc />
public partial class InitialTodoDb : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "todo");

        migrationBuilder.CreateTable(
            name: "Todos",
            schema: "todo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 100, nullable: false),
                Remarks = table.Column<string>(type: "VARCHAR(32)", nullable: true),
                Status = table.Column<string>(type: "VARCHAR(32)", nullable: true),
                Description = table.Column<string>(type: "text", nullable: true),
                Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                table.PrimaryKey("PK_Todos", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Todos",
            schema: "todo");
    }
}