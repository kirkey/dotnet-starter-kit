#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Accounting
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "accounting");

            migrationBuilder.CreateTable(
                name: "AccountingPeriods",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsClosed = table.Column<bool>(type: "boolean", nullable: false),
                    IsAdjustmentPeriod = table.Column<bool>(type: "boolean", nullable: false),
                    FiscalYear = table.Column<int>(type: "integer", nullable: false),
                    PeriodType = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 1024, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
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
                    table.PrimaryKey("PK_AccountingPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accruals",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccrualNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AccrualDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 16, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsReversed = table.Column<bool>(type: "boolean", nullable: false),
                    ReversalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
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
                    table.PrimaryKey("PK_Accruals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankReconciliations",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BankAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReconciliationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatementBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    BookBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AdjustedBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    OutstandingChecksTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DepositsInTransitTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    BankErrors = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    BookErrors = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsReconciled = table.Column<bool>(type: "boolean", nullable: false),
                    ReconciledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReconciledBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ApprovedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StatementNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
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
                    table.PrimaryKey("PK_BankReconciliations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PeriodId = table.Column<Guid>(type: "uuid", nullable: false),
                    PeriodName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    FiscalYear = table.Column<int>(type: "integer", nullable: false),
                    BudgetType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Status = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    TotalBudgetedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalActualAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApprovedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChartOfAccounts",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountCode = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    AccountName = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    AccountType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ParentAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    UsoaCategory = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ParentCode = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IsControlAccount = table.Column<bool>(type: "boolean", nullable: false),
                    NormalBalance = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    AccountLevel = table.Column<int>(type: "integer", nullable: false),
                    AllowDirectPosting = table.Column<bool>(type: "boolean", nullable: false),
                    IsUsoaCompliant = table.Column<bool>(type: "boolean", nullable: false),
                    RegulatoryClassification = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
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
                    table.PrimaryKey("PK_ChartOfAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Consumption",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MeterId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReadingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CurrentReading = table.Column<decimal>(type: "numeric(18,3)", precision: 16, scale: 2, nullable: false),
                    PreviousReading = table.Column<decimal>(type: "numeric(18,3)", precision: 16, scale: 2, nullable: false),
                    KWhUsed = table.Column<decimal>(type: "numeric(18,3)", precision: 16, scale: 2, nullable: false),
                    BillingPeriod = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ReadingType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Multiplier = table.Column<decimal>(type: "numeric(10,4)", precision: 16, scale: 2, nullable: true),
                    IsValidReading = table.Column<bool>(type: "boolean", nullable: false),
                    ReadingSource = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
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
                    table.PrimaryKey("PK_Consumption", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CostCenters",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CostCenterType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ParentCostCenterId = table.Column<Guid>(type: "uuid", nullable: true),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: true),
                    ManagerName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    BudgetAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ActualAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Location = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Notes = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
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
                    table.PrimaryKey("PK_CostCenters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreditMemos",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemoNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MemoDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AppliedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    RefundedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ReferenceType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalDocumentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Reason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsApplied = table.Column<bool>(type: "boolean", nullable: false),
                    AppliedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ApprovedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
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
                    table.PrimaryKey("PK_CreditMemos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DebitMemos",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemoNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MemoDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AppliedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ReferenceType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalDocumentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Reason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsApplied = table.Column<bool>(type: "boolean", nullable: false),
                    AppliedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ApprovedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
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
                    table.PrimaryKey("PK_DebitMemos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeferredRevenues",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeferredRevenueNumber = table.Column<string>(type: "text", nullable: false),
                    RecognitionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsRecognized = table.Column<bool>(type: "boolean", nullable: false),
                    RecognizedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
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
                    table.PrimaryKey("PK_DeferredRevenues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepreciationMethods",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MethodCode = table.Column<string>(type: "text", nullable: false),
                    CalculationFormula = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_DepreciationMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FixedAssets",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetName = table.Column<string>(type: "text", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ServiceLife = table.Column<int>(type: "integer", nullable: false),
                    DepreciationMethodId = table.Column<Guid>(type: "uuid", nullable: false),
                    SalvageValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrentBookValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AccumulatedDepreciationAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepreciationExpenseAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    SerialNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Department = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsDisposed = table.Column<bool>(type: "boolean", nullable: false),
                    DisposalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DisposalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    AssetType = table.Column<string>(type: "text", nullable: false),
                    GpsCoordinates = table.Column<string>(type: "text", nullable: true),
                    SubstationName = table.Column<string>(type: "text", nullable: true),
                    AssetUsoaId = table.Column<Guid>(type: "uuid", nullable: true),
                    RegulatoryClassification = table.Column<string>(type: "text", nullable: true),
                    VoltageRating = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    Capacity = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    Manufacturer = table.Column<string>(type: "text", nullable: true),
                    ModelNumber = table.Column<string>(type: "text", nullable: true),
                    LastMaintenanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NextMaintenanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RequiresUsoaReporting = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_FixedAssets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralLedgers",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntryId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Debit = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    Credit = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    Memo = table.Column<string>(type: "text", nullable: true),
                    UsoaClass = table.Column<string>(type: "text", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "text", nullable: true),
                    PeriodId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_GeneralLedgers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Sku = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "text", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    PaidAmount = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ConsumptionId = table.Column<Guid>(type: "uuid", nullable: true),
                    UsageCharge = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    BasicServiceCharge = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    OtherCharges = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    KWhUsed = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    BillingPeriod = table.Column<string>(type: "text", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PaymentMethod = table.Column<string>(type: "text", nullable: true),
                    LateFee = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    ReconnectionFee = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    DepositAmount = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    RateSchedule = table.Column<string>(type: "text", nullable: true),
                    DemandCharge = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
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
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberNumber = table.Column<string>(type: "text", nullable: false),
                    MemberName = table.Column<string>(type: "text", nullable: false),
                    ServiceAddress = table.Column<string>(type: "text", nullable: false),
                    MailingAddress = table.Column<string>(type: "text", nullable: true),
                    ContactInfo = table.Column<string>(type: "text", nullable: true),
                    AccountStatus = table.Column<string>(type: "text", nullable: false),
                    MeterId = table.Column<Guid>(type: "uuid", nullable: true),
                    RateScheduleId = table.Column<Guid>(type: "uuid", nullable: true),
                    MembershipDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    EmergencyContact = table.Column<string>(type: "text", nullable: true),
                    ServiceClass = table.Column<string>(type: "text", nullable: true),
                    RateSchedule = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meters",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MeterNumber = table.Column<string>(type: "text", nullable: false),
                    MeterType = table.Column<string>(type: "text", nullable: false),
                    Manufacturer = table.Column<string>(type: "text", nullable: false),
                    ModelNumber = table.Column<string>(type: "text", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: true),
                    InstallationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastReadingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastReading = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    Multiplier = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    GpsCoordinates = table.Column<string>(type: "text", nullable: true),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsSmartMeter = table.Column<bool>(type: "boolean", nullable: false),
                    CommunicationProtocol = table.Column<string>(type: "text", nullable: true),
                    LastMaintenanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NextCalibrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AccuracyClass = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    MeterConfiguration = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Meters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatronageCapitals",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    FiscalYear = table.Column<int>(type: "integer", nullable: false),
                    AmountAllocated = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    AmountRetired = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_PatronageCapitals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payees",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PayeeCode = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    ExpenseAccountCode = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    ExpenseAccountName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Tin = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 1024, nullable: false),
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
                    table.PrimaryKey("PK_Payees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentNumber = table.Column<string>(type: "text", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    UnappliedAmount = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    PaymentMethod = table.Column<string>(type: "text", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "text", nullable: true),
                    DepositToAccountCode = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostingBatches",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchNumber = table.Column<string>(type: "text", nullable: false),
                    BatchDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PeriodId = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "text", nullable: false),
                    ApprovedBy = table.Column<string>(type: "text", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
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
                    table.PrimaryKey("PK_PostingBatches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BudgetedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    ClientName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ProjectManager = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Department = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ActualCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ActualRevenue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
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
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpectedDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VendorId = table.Column<Guid>(type: "uuid", nullable: false),
                    VendorName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ReceivedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    BilledAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ApprovalStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsFullyReceived = table.Column<bool>(type: "boolean", nullable: false),
                    IsFullyBilled = table.Column<bool>(type: "boolean", nullable: false),
                    ApprovedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RequesterId = table.Column<Guid>(type: "uuid", nullable: true),
                    RequesterName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CostCenterId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: true),
                    ShipToAddress = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    PaymentTerms = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ReferenceNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Notes = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
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
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RateSchedules",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RateCode = table.Column<string>(type: "text", nullable: false),
                    RateName = table.Column<string>(type: "text", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EnergyRatePerKwh = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    DemandRatePerKw = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    FixedMonthlyCharge = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    IsTimeOfUse = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_RateSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecurringJournalEntries",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Frequency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CustomIntervalDays = table.Column<int>(type: "integer", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DebitAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreditAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NextRunDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastGeneratedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GeneratedCount = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ApprovedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PostingBatchId = table.Column<Guid>(type: "uuid", nullable: true),
                    Memo = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Notes = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
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
                    table.PrimaryKey("PK_RecurringJournalEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegulatoryReports",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ReportType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReportingPeriod = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PeriodStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Draft"),
                    RegulatoryBody = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FilingNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TotalAssets = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    TotalLiabilities = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    TotalEquity = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    TotalRevenue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    TotalExpenses = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    NetIncome = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    RateBase = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    AllowedReturn = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    FilePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PreparedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ReviewedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ApprovedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RequiresAudit = table.Column<bool>(type: "boolean", nullable: false),
                    AuditFirm = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AuditDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_RegulatoryReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SecurityDeposits",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    DepositDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRefunded = table.Column<bool>(type: "boolean", nullable: false),
                    RefundedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RefundReference = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_SecurityDeposits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxCodes",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    TaxType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Rate = table.Column<decimal>(type: "numeric(10,6)", precision: 10, scale: 6, nullable: false),
                    IsCompound = table.Column<bool>(type: "boolean", nullable: false),
                    Jurisdiction = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    TaxCollectedAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaxPaidAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    TaxAuthority = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    TaxRegistrationNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ReportingCategory = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
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
                    table.PrimaryKey("PK_TaxCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VendorCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Address = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    BillingAddress = table.Column<string>(type: "text", nullable: true),
                    ContactPerson = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Terms = table.Column<string>(type: "text", nullable: true),
                    ExpenseAccountCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ExpenseAccountName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Tin = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 256, nullable: false),
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
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WriteOffs",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    WriteOffDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WriteOffType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    RecoveredAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IsRecovered = table.Column<bool>(type: "boolean", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ReceivableAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpenseAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    JournalEntryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ApprovalStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ApprovedBy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Reason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Notes = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
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
                    table.PrimaryKey("PK_WriteOffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetDetails",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BudgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    BudgetedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ActualAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_BudgetDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetDetails_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalSchema: "accounting",
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepreciationEntry",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FixedAssetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Method = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepreciationEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepreciationEntry_FixedAssets_FixedAssetId",
                        column: x => x.FixedAssetId,
                        principalSchema: "accounting",
                        principalTable: "FixedAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceLineItem",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    AccountCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceLineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceLineItem_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "accounting",
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeterReading",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MeterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reading = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    ReadingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReadingType = table.Column<string>(type: "text", nullable: false),
                    ReadBy = table.Column<string>(type: "text", nullable: true),
                    IsValidated = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterReading", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterReading_Meters_MeterId",
                        column: x => x.MeterId,
                        principalSchema: "accounting",
                        principalTable: "Meters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentAllocations",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
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
                    table.PrimaryKey("PK_PaymentAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentAllocations_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalSchema: "accounting",
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntries",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Source = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    IsPosted = table.Column<bool>(type: "boolean", nullable: false),
                    PeriodId = table.Column<Guid>(type: "uuid", nullable: true),
                    OriginalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ApprovalStatus = table.Column<string>(type: "text", nullable: false),
                    ApprovedBy = table.Column<string>(type: "text", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PostingBatchId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_JournalEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntries_PostingBatches_PostingBatchId",
                        column: x => x.PostingBatchId,
                        principalSchema: "accounting",
                        principalTable: "PostingBatches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectCostEntries",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    JournalEntryId = table.Column<Guid>(type: "uuid", nullable: true),
                    CostCenter = table.Column<string>(type: "text", nullable: true),
                    WorkOrderNumber = table.Column<string>(type: "text", nullable: true),
                    IsBillable = table.Column<bool>(type: "boolean", nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    Vendor = table.Column<string>(type: "text", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 512, nullable: false),
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
                    table.PrimaryKey("PK_ProjectCostEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectCostEntries_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "accounting",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RateTier",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RateScheduleId = table.Column<Guid>(type: "uuid", nullable: false),
                    TierOrder = table.Column<int>(type: "integer", nullable: false),
                    UpToKwh = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    RatePerKwh = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateTier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RateTier_RateSchedules_RateScheduleId",
                        column: x => x.RateScheduleId,
                        principalSchema: "accounting",
                        principalTable: "RateSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntryLines",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JournalEntryId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    DebitAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreditAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntryLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntryLines_JournalEntries_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "accounting",
                        principalTable: "JournalEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountingPeriods_FiscalYear_PeriodType",
                schema: "accounting",
                table: "AccountingPeriods",
                columns: new[] { "FiscalYear", "PeriodType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accruals_AccrualNumber",
                schema: "accounting",
                table: "Accruals",
                column: "AccrualNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_BudgetId",
                schema: "accounting",
                table: "BudgetDetails",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_BudgetId_AccountId",
                schema: "accounting",
                table: "BudgetDetails",
                columns: new[] { "BudgetId", "AccountId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_Name_PeriodId",
                schema: "accounting",
                table: "Budgets",
                columns: new[] { "Name", "PeriodId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChartOfAccounts_AccountCode",
                schema: "accounting",
                table: "ChartOfAccounts",
                column: "AccountCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Consumption_MeterId_ReadingDate",
                schema: "accounting",
                table: "Consumption",
                columns: new[] { "MeterId", "ReadingDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_Code",
                schema: "accounting",
                table: "CostCenters",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreditMemos_MemoNumber",
                schema: "accounting",
                table: "CreditMemos",
                column: "MemoNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DebitMemos_MemoNumber",
                schema: "accounting",
                table: "DebitMemos",
                column: "MemoNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepreciationEntry_FixedAssetId",
                schema: "accounting",
                table: "DepreciationEntry",
                column: "FixedAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_Sku",
                schema: "accounting",
                table: "InventoryItems",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLineItem_InvoiceId",
                schema: "accounting",
                table: "InvoiceLineItem",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_PostingBatchId",
                schema: "accounting",
                table: "JournalEntries",
                column: "PostingBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_ReferenceNumber",
                schema: "accounting",
                table: "JournalEntries",
                column: "ReferenceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLines_JournalEntryId",
                schema: "accounting",
                table: "JournalEntryLines",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterReading_MeterId",
                schema: "accounting",
                table: "MeterReading",
                column: "MeterId");

            migrationBuilder.CreateIndex(
                name: "IX_Payees_PayeeCode",
                schema: "accounting",
                table: "Payees",
                column: "PayeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAllocations_PaymentId",
                schema: "accounting",
                table: "PaymentAllocations",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCostEntries_ProjectId",
                schema: "accounting",
                table: "ProjectCostEntries",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_OrderNumber",
                schema: "accounting",
                table: "PurchaseOrders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RateTier_RateScheduleId",
                schema: "accounting",
                table: "RateTier",
                column: "RateScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringJournalEntries_TemplateCode",
                schema: "accounting",
                table: "RecurringJournalEntries",
                column: "TemplateCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegulatoryReports_DueDate",
                schema: "accounting",
                table: "RegulatoryReports",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_RegulatoryReports_PeriodStartDate_PeriodEndDate",
                schema: "accounting",
                table: "RegulatoryReports",
                columns: new[] { "PeriodStartDate", "PeriodEndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_RegulatoryReports_RegulatoryBody",
                schema: "accounting",
                table: "RegulatoryReports",
                column: "RegulatoryBody");

            migrationBuilder.CreateIndex(
                name: "IX_RegulatoryReports_ReportName",
                schema: "accounting",
                table: "RegulatoryReports",
                column: "ReportName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegulatoryReports_ReportType",
                schema: "accounting",
                table: "RegulatoryReports",
                column: "ReportType");

            migrationBuilder.CreateIndex(
                name: "IX_RegulatoryReports_Status",
                schema: "accounting",
                table: "RegulatoryReports",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TaxCodes_Code",
                schema: "accounting",
                table: "TaxCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WriteOffs_ReferenceNumber",
                schema: "accounting",
                table: "WriteOffs",
                column: "ReferenceNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingPeriods",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "Accruals",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "BankReconciliations",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "BudgetDetails",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "ChartOfAccounts",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "Consumption",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "CostCenters",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "CreditMemos",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "DebitMemos",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "DeferredRevenues",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "DepreciationEntry",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "DepreciationMethods",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "GeneralLedgers",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "InventoryItems",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "InvoiceLineItem",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "JournalEntryLines",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "Members",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "MeterReading",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "PatronageCapitals",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "Payees",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "PaymentAllocations",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "ProjectCostEntries",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "PurchaseOrders",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "RateTier",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "RecurringJournalEntries",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "RegulatoryReports",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "SecurityDeposits",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "TaxCodes",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "Vendors",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "WriteOffs",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "Budgets",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "FixedAssets",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "Invoices",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "JournalEntries",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "Meters",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "Payments",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "Projects",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "RateSchedules",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "PostingBatches",
                schema: "accounting");
        }
    }
}
