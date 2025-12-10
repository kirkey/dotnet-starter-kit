using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.MicroFinance
{
    /// <inheritdoc />
    public partial class InitialMicroFinanceDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "microfinance");

            migrationBuilder.CreateTable(
                name: "ApprovalWorkflows",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    EntityType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    MinAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MaxAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    NumberOfLevels = table.Column<int>(type: "integer", nullable: false),
                    IsSequential = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_ApprovalWorkflows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Address = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Phone = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    BranchType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ParentBranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    ManagerName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ManagerPhone = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    ManagerEmail = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    OpeningDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ClosingDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    OperatingHours = table.Column<string>(type: "text", nullable: true),
                    Timezone = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CashHoldingLimit = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Branches_ParentBranchId",
                        column: x => x.ParentBranchId,
                        principalSchema: "microfinance",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CollateralInsurances",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CollateralId = table.Column<Guid>(type: "uuid", nullable: false),
                    PolicyNumber = table.Column<string>(type: "text", nullable: false),
                    InsurerName = table.Column<string>(type: "text", nullable: false),
                    InsuranceType = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CoverageAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PremiumAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Deductible = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    EffectiveDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RenewalDate = table.Column<DateOnly>(type: "date", nullable: true),
                    InsurerContact = table.Column<string>(type: "text", nullable: true),
                    InsurerPhone = table.Column<string>(type: "text", nullable: true),
                    InsurerEmail = table.Column<string>(type: "text", nullable: true),
                    IsMfiAsBeneficiary = table.Column<bool>(type: "boolean", nullable: false),
                    BeneficiaryName = table.Column<string>(type: "text", nullable: true),
                    PolicyDocumentPath = table.Column<string>(type: "text", nullable: true),
                    LastPremiumPaidDate = table.Column<DateOnly>(type: "date", nullable: true),
                    NextPremiumDueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    RenewalReminderDays = table.Column<int>(type: "integer", nullable: false),
                    AutoRenewal = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
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
                    table.PrimaryKey("PK_CollateralInsurances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CollateralReleases",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CollateralId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReleaseReference = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RequestedById = table.Column<Guid>(type: "uuid", nullable: false),
                    ReleaseMethod = table.Column<string>(type: "text", nullable: true),
                    RecipientName = table.Column<string>(type: "text", nullable: true),
                    RecipientIdNumber = table.Column<string>(type: "text", nullable: true),
                    RecipientContact = table.Column<string>(type: "text", nullable: true),
                    ApprovedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ApprovedById = table.Column<Guid>(type: "uuid", nullable: true),
                    ReleasedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ReleasedById = table.Column<Guid>(type: "uuid", nullable: true),
                    RejectionReason = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    ReleaseDocumentPath = table.Column<string>(type: "text", nullable: true),
                    RecipientSignaturePath = table.Column<string>(type: "text", nullable: true),
                    DocumentsReturned = table.Column<bool>(type: "boolean", nullable: false),
                    RegistrationCleared = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
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
                    table.PrimaryKey("PK_CollateralReleases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CollateralTypes",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    DefaultLtvPercent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MaxLtvPercent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DefaultUsefulLifeYears = table.Column<int>(type: "integer", nullable: false),
                    AnnualDepreciationRate = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    RequiresInsurance = table.Column<bool>(type: "boolean", nullable: false),
                    RequiresAppraisal = table.Column<bool>(type: "boolean", nullable: false),
                    RevaluationFrequencyMonths = table.Column<int>(type: "integer", nullable: false),
                    RequiresRegistration = table.Column<bool>(type: "boolean", nullable: false),
                    RequiredDocuments = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 128, nullable: false),
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
                    table.PrimaryKey("PK_CollateralTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationTemplates",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Channel = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Category = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Subject = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Body = table.Column<string>(type: "character varying(16384)", maxLength: 16384, nullable: false),
                    Placeholders = table.Column<string>(type: "text", nullable: true),
                    Language = table.Column<string>(type: "text", nullable: false),
                    RequiresApproval = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_CommunicationTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerCases",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CaseNumber = table.Column<string>(type: "text", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Priority = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Channel = table.Column<string>(type: "text", nullable: false),
                    AssignedToId = table.Column<Guid>(type: "uuid", nullable: true),
                    RelatedLoanId = table.Column<Guid>(type: "uuid", nullable: true),
                    RelatedSavingsAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    OpenedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FirstResponseAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ResolvedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ClosedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Resolution = table.Column<string>(type: "text", nullable: true),
                    EscalationLevel = table.Column<int>(type: "integer", nullable: false),
                    EscalatedToId = table.Column<Guid>(type: "uuid", nullable: true),
                    EscalatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SlaHours = table.Column<int>(type: "integer", nullable: false),
                    SlaBreached = table.Column<bool>(type: "boolean", nullable: false),
                    CustomerSatisfactionScore = table.Column<int>(type: "integer", nullable: true),
                    CustomerFeedback = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_CustomerCases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSegments",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    SegmentType = table.Column<string>(type: "text", nullable: false),
                    SegmentCriteria = table.Column<string>(type: "text", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    MemberCount = table.Column<int>(type: "integer", nullable: false),
                    MinIncomeLevel = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MaxIncomeLevel = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    RiskLevel = table.Column<string>(type: "text", nullable: true),
                    DefaultInterestModifier = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    DefaultFeeModifier = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    TargetProducts = table.Column<string>(type: "text", nullable: true),
                    MarketingCampaigns = table.Column<string>(type: "text", nullable: true),
                    LastUpdatedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 128, nullable: false),
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
                    table.PrimaryKey("PK_CustomerSegments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSurveys",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SurveyType = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Questions = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TargetSegments = table.Column<string>(type: "text", nullable: true),
                    TotalResponses = table.Column<int>(type: "integer", nullable: false),
                    AverageScore = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    NpsScore = table.Column<int>(type: "integer", nullable: true),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsAnonymous = table.Column<bool>(type: "boolean", nullable: false),
                    SendReminders = table.Column<bool>(type: "boolean", nullable: false),
                    ReminderDays = table.Column<int>(type: "integer", nullable: false),
                    ThankYouMessage = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_CustomerSurveys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeeDefinitions",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    FeeType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CalculationType = table.Column<string>(type: "text", nullable: false),
                    AppliesTo = table.Column<string>(type: "text", nullable: false),
                    ChargeFrequency = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MinAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MaxAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    IsTaxable = table.Column<bool>(type: "boolean", nullable: false),
                    TaxRate = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_FeeDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceProducts",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    InsuranceType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Provider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    MinCoverage = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MaxCoverage = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PremiumCalculation = table.Column<string>(type: "text", nullable: false),
                    PremiumRate = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PremiumRateTable = table.Column<string>(type: "text", nullable: true),
                    MinAge = table.Column<int>(type: "integer", nullable: true),
                    MaxAge = table.Column<int>(type: "integer", nullable: true),
                    WaitingPeriodDays = table.Column<int>(type: "integer", nullable: false),
                    PremiumUpfront = table.Column<bool>(type: "boolean", nullable: false),
                    MandatoryWithLoan = table.Column<bool>(type: "boolean", nullable: false),
                    CoveredEvents = table.Column<string>(type: "text", nullable: true),
                    Exclusions = table.Column<string>(type: "text", nullable: true),
                    TermsConditions = table.Column<string>(type: "character varying(8192)", maxLength: 8192, nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_InsuranceProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentAccounts",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountNumber = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RiskProfile = table.Column<string>(type: "text", nullable: false),
                    TotalInvested = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrentValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalGainLoss = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalGainLossPercent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    RealizedGains = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    UnrealizedGains = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalDividends = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    HoldingsCount = table.Column<int>(type: "integer", nullable: false),
                    FirstInvestmentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LastTransactionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AssignedAdvisorId = table.Column<Guid>(type: "uuid", nullable: true),
                    HasSip = table.Column<bool>(type: "boolean", nullable: false),
                    SipAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    SipFrequency = table.Column<string>(type: "text", nullable: true),
                    NextSipDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LinkedSavingsAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    InvestmentGoal = table.Column<string>(type: "text", nullable: true),
                    TargetDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TargetAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_InvestmentAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentProducts",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProductType = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RiskLevel = table.Column<string>(type: "text", nullable: false),
                    MinimumInvestment = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MaximumInvestment = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ManagementFeePercent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PerformanceFeePercent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    EntryLoadPercent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ExitLoadPercent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ExpectedReturnMin = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ExpectedReturnMax = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    LockInPeriodDays = table.Column<int>(type: "integer", nullable: false),
                    MinimumHoldingDays = table.Column<int>(type: "integer", nullable: true),
                    CurrentNav = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    NavDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TotalAum = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalInvestors = table.Column<int>(type: "integer", nullable: false),
                    FundManager = table.Column<string>(type: "text", nullable: true),
                    Benchmark = table.Column<string>(type: "text", nullable: true),
                    YtdReturn = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    OneYearReturn = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ThreeYearReturn = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    AllowPartialRedemption = table.Column<bool>(type: "boolean", nullable: false),
                    AllowSip = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_InvestmentProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanProducts",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    MinLoanAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MaxLoanAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    InterestRate = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false),
                    InterestMethod = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    MinTermMonths = table.Column<int>(type: "integer", nullable: false),
                    MaxTermMonths = table.Column<int>(type: "integer", nullable: false),
                    RepaymentFrequency = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    GracePeriodDays = table.Column<int>(type: "integer", nullable: false),
                    LatePenaltyRate = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
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
                    table.PrimaryKey("PK_LoanProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketingCampaigns",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CampaignType = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TargetSegments = table.Column<string>(type: "text", nullable: true),
                    TargetProducts = table.Column<string>(type: "text", nullable: true),
                    Channels = table.Column<string>(type: "text", nullable: false),
                    MessageTemplate = table.Column<string>(type: "text", nullable: true),
                    Budget = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SpentAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TargetAudience = table.Column<int>(type: "integer", nullable: false),
                    ReachedCount = table.Column<int>(type: "integer", nullable: false),
                    ResponseCount = table.Column<int>(type: "integer", nullable: false),
                    ConversionCount = table.Column<int>(type: "integer", nullable: false),
                    ResponseRate = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ConversionRate = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Roi = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedById = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 128, nullable: false),
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
                    table.PrimaryKey("PK_MarketingCampaigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Address = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    NationalId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Occupation = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    MonthlyIncome = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    JoinDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
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
                name: "MfiConfigurations",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    DataType = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsEncrypted = table.Column<bool>(type: "boolean", nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false),
                    RequiresRestart = table.Column<bool>(type: "boolean", nullable: false),
                    DefaultValue = table.Column<string>(type: "text", nullable: true),
                    ValidationRules = table.Column<string>(type: "text", nullable: true),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_MfiConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentGateways",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Provider = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    MerchantId = table.Column<string>(type: "text", nullable: true),
                    ApiKeyEncrypted = table.Column<string>(type: "text", nullable: true),
                    SecretKeyEncrypted = table.Column<string>(type: "text", nullable: true),
                    WebhookUrl = table.Column<string>(type: "text", nullable: true),
                    WebhookSecret = table.Column<string>(type: "text", nullable: true),
                    TransactionFeePercent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TransactionFeeFixed = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MinTransactionAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MaxTransactionAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SupportsRefunds = table.Column<bool>(type: "boolean", nullable: false),
                    SupportsRecurring = table.Column<bool>(type: "boolean", nullable: false),
                    SupportsMobileWallet = table.Column<bool>(type: "boolean", nullable: false),
                    SupportsCardPayments = table.Column<bool>(type: "boolean", nullable: false),
                    SupportsBankTransfer = table.Column<bool>(type: "boolean", nullable: false),
                    IsTestMode = table.Column<bool>(type: "boolean", nullable: false),
                    CallbackUrl = table.Column<string>(type: "text", nullable: true),
                    IpWhitelist = table.Column<string>(type: "text", nullable: true),
                    TimeoutSeconds = table.Column<int>(type: "integer", nullable: false),
                    RetryAttempts = table.Column<int>(type: "integer", nullable: false),
                    LastSuccessfulConnection = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_PaymentGateways", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportDefinitions",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Category = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    OutputFormat = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ParametersDefinition = table.Column<string>(type: "text", nullable: true),
                    Query = table.Column<string>(type: "character varying(16384)", maxLength: 16384, nullable: true),
                    LayoutTemplate = table.Column<string>(type: "text", nullable: true),
                    IsScheduled = table.Column<bool>(type: "boolean", nullable: false),
                    ScheduleFrequency = table.Column<string>(type: "text", nullable: true),
                    ScheduleDay = table.Column<int>(type: "integer", nullable: true),
                    ScheduleTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    ScheduleRecipients = table.Column<string>(type: "text", nullable: true),
                    LastGeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_ReportDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskCategories",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    RiskType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ParentCategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultSeverity = table.Column<string>(type: "text", nullable: false),
                    WeightFactor = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AlertThreshold = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    RequiresEscalation = table.Column<bool>(type: "boolean", nullable: false),
                    EscalationHours = table.Column<int>(type: "integer", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Notes = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_RiskCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskCategories_RiskCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalSchema: "microfinance",
                        principalTable: "RiskCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SavingsProducts",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    InterestRate = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false),
                    InterestCalculation = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    InterestPostingFrequency = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    MinOpeningBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MinBalanceForInterest = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MinWithdrawalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MaxWithdrawalPerDay = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    AllowOverdraft = table.Column<bool>(type: "boolean", nullable: false),
                    OverdraftLimit = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
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
                    table.PrimaryKey("PK_SavingsProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShareProducts",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    NominalValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MinSharesForMembership = table.Column<int>(type: "integer", nullable: false),
                    MaxSharesPerMember = table.Column<int>(type: "integer", nullable: true),
                    AllowTransfer = table.Column<bool>(type: "boolean", nullable: false),
                    AllowRedemption = table.Column<bool>(type: "boolean", nullable: false),
                    MinHoldingPeriodMonths = table.Column<int>(type: "integer", nullable: true),
                    PaysDividends = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_ShareProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalLevels",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowId = table.Column<Guid>(type: "uuid", nullable: false),
                    LevelNumber = table.Column<int>(type: "integer", nullable: false),
                    RequiredRole = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    RequiredUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    MaxApprovalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CanBeSkipped = table.Column<bool>(type: "boolean", nullable: false),
                    SlaHours = table.Column<int>(type: "integer", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_ApprovalLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalLevels_ApprovalWorkflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalSchema: "microfinance",
                        principalTable: "ApprovalWorkflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalRequests",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    WorkflowId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CurrentLevel = table.Column<int>(type: "integer", nullable: false),
                    TotalLevels = table.Column<int>(type: "integer", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SubmittedById = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    SlaDueAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Comments = table.Column<string>(type: "text", nullable: true),
                    RejectionReason = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_ApprovalRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalRequests_ApprovalWorkflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalSchema: "microfinance",
                        principalTable: "ApprovalWorkflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BranchTargets",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Period = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    PeriodStart = table.Column<DateOnly>(type: "date", nullable: false),
                    PeriodEnd = table.Column<DateOnly>(type: "date", nullable: false),
                    TargetValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MetricUnit = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    AchievedValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AchievementPercentage = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    MinimumThreshold = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    StretchTarget = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Weight = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_BranchTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchTargets_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "microfinance",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CashVaults",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    VaultType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    OpeningBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MinimumBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MaximumBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Location = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CustodianName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    CustodianUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastReconciliationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastReconciledBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    DenominationBreakdown = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_CashVaults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashVaults_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "microfinance",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    FirstName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    LastName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Phone = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    AlternatePhone = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    NationalId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Address = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    Department = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    JobTitle = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Designation = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Role = table.Column<string>(type: "text", nullable: false),
                    EmploymentType = table.Column<string>(type: "text", nullable: false),
                    JoiningDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ProbationEndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ConfirmationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TerminationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ReportingManagerId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReportingTo = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    BasicSalary = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    BankAccountNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    BankName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    EmergencyContactName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CanApproveLoan = table.Column<bool>(type: "boolean", nullable: false),
                    LoanApprovalLimit = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Notes = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
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
                    table.PrimaryKey("PK_Staff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staff_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "microfinance",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staff_Staff_ReportingManagerId",
                        column: x => x.ReportingManagerId,
                        principalSchema: "microfinance",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentTransactions",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InvestmentAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionReference = table.Column<string>(type: "text", nullable: false),
                    TransactionType = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Units = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    NavAtTransaction = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    EntryLoadAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ExitLoadAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    NetAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    GainLoss = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    RequestedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ProcessedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AllotmentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    SwitchToProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    SourceAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentMode = table.Column<string>(type: "text", nullable: true),
                    PaymentReference = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    FailureReason = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
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
                    table.PrimaryKey("PK_InvestmentTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentTransactions_InvestmentAccounts_InvestmentAccount~",
                        column: x => x.InvestmentAccountId,
                        principalSchema: "microfinance",
                        principalTable: "InvestmentAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionStrategies",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LoanProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    TriggerDaysPastDue = table.Column<int>(type: "integer", nullable: false),
                    MaxDaysPastDue = table.Column<int>(type: "integer", nullable: true),
                    MinOutstandingAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MaxOutstandingAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ActionType = table.Column<string>(type: "text", nullable: false),
                    MessageTemplate = table.Column<string>(type: "text", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    RepeatIntervalDays = table.Column<int>(type: "integer", nullable: true),
                    MaxRepetitions = table.Column<int>(type: "integer", nullable: true),
                    EscalateOnFailure = table.Column<bool>(type: "boolean", nullable: false),
                    RequiresApproval = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    EffectiveFrom = table.Column<DateOnly>(type: "date", nullable: true),
                    EffectiveTo = table.Column<DateOnly>(type: "date", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 128, nullable: false),
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
                    table.PrimaryKey("PK_CollectionStrategies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionStrategies_LoanProducts_LoanProductId",
                        column: x => x.LoanProductId,
                        principalSchema: "microfinance",
                        principalTable: "LoanProducts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CreditBureauReports",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    InquiryId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReportNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    BureauName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ReportDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreditScore = table.Column<int>(type: "integer", nullable: true),
                    ScoreMin = table.Column<int>(type: "integer", nullable: true),
                    ScoreMax = table.Column<int>(type: "integer", nullable: true),
                    ScoreModel = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    RiskGrade = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    ActiveAccounts = table.Column<int>(type: "integer", nullable: true),
                    ClosedAccounts = table.Column<int>(type: "integer", nullable: true),
                    DelinquentAccounts = table.Column<int>(type: "integer", nullable: true),
                    TotalOutstandingBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    TotalCreditLimit = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CreditUtilization = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    RecentInquiries = table.Column<int>(type: "integer", nullable: true),
                    CreditHistoryMonths = table.Column<int>(type: "integer", nullable: true),
                    LatePayments12Months = table.Column<int>(type: "integer", nullable: true),
                    LatePayments24Months = table.Column<int>(type: "integer", nullable: true),
                    Defaults = table.Column<int>(type: "integer", nullable: true),
                    Bankruptcies = table.Column<int>(type: "integer", nullable: true),
                    Collections = table.Column<int>(type: "integer", nullable: true),
                    PublicRecords = table.Column<int>(type: "integer", nullable: true),
                    DebtToIncomeRatio = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    RawReportData = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Notes = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
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
                    table.PrimaryKey("PK_CreditBureauReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditBureauReports_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    PrincipalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    InterestRate = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false),
                    TermMonths = table.Column<int>(type: "integer", nullable: false),
                    RepaymentFrequency = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Purpose = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    ApplicationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ApprovalDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DisbursementDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpectedEndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ActualEndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    OutstandingPrincipal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    OutstandingInterest = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalPaid = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    RejectionReason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
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
                    table.PrimaryKey("PK_Loans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loans_LoanProducts_LoanProductId",
                        column: x => x.LoanProductId,
                        principalSchema: "microfinance",
                        principalTable: "LoanProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loans_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberGroups",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    FormationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LeaderMemberId = table.Column<Guid>(type: "uuid", nullable: true),
                    LoanOfficerId = table.Column<Guid>(type: "uuid", nullable: true),
                    MeetingLocation = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    MeetingFrequency = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    MeetingDay = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    MeetingTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
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
                    table.PrimaryKey("PK_MemberGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberGroups_Members_LeaderMemberId",
                        column: x => x.LeaderMemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ReportGenerations",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Trigger = table.Column<string>(type: "text", nullable: false),
                    Parameters = table.Column<string>(type: "text", nullable: true),
                    ReportStartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ReportEndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    OutputFormat = table.Column<string>(type: "text", nullable: true),
                    OutputFile = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: true),
                    RecordCount = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DurationMs = table.Column<long>(type: "bigint", nullable: true),
                    ErrorMessage = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_ReportGenerations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportGenerations_ReportDefinitions_ReportDefinitionId",
                        column: x => x.ReportDefinitionId,
                        principalSchema: "microfinance",
                        principalTable: "ReportDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RiskIndicators",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RiskCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Formula = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Unit = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Direction = table.Column<string>(type: "text", nullable: false),
                    Frequency = table.Column<string>(type: "text", nullable: false),
                    DataSource = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    TargetValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    GreenThreshold = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    YellowThreshold = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    OrangeThreshold = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    RedThreshold = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CurrentValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    PreviousValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CurrentHealth = table.Column<string>(type: "text", nullable: false),
                    LastMeasuredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WeightFactor = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
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
                    table.PrimaryKey("PK_RiskIndicators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskIndicators_RiskCategories_RiskCategoryId",
                        column: x => x.RiskCategoryId,
                        principalSchema: "microfinance",
                        principalTable: "RiskCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SavingsAccounts",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    SavingsProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalDeposits = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalWithdrawals = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalInterestEarned = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    OpenedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ClosedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LastInterestPostingDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
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
                    table.PrimaryKey("PK_SavingsAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavingsAccounts_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SavingsAccounts_SavingsProducts_SavingsProductId",
                        column: x => x.SavingsProductId,
                        principalSchema: "microfinance",
                        principalTable: "SavingsProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShareAccounts",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShareProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumberOfShares = table.Column<int>(type: "integer", nullable: false),
                    TotalShareValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalPurchases = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalRedemptions = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalDividendsEarned = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalDividendsPaid = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    OpenedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ClosedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_ShareAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShareAccounts_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareAccounts_ShareProducts_ShareProductId",
                        column: x => x.ShareProductId,
                        principalSchema: "microfinance",
                        principalTable: "ShareProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalDecisions",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    ApproverId = table.Column<Guid>(type: "uuid", nullable: false),
                    Decision = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    DecisionAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Comments = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_ApprovalDecisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalDecisions_ApprovalRequests_RequestId",
                        column: x => x.RequestId,
                        principalSchema: "microfinance",
                        principalTable: "ApprovalRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TellerSessions",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    CashVaultId = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    TellerUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TellerName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    SessionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OpeningBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalCashIn = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalCashOut = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ExpectedClosingBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ActualClosingBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Variance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    TransactionCount = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    SupervisorUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    SupervisorName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    SupervisorVerificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClosingDenominations = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_TellerSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TellerSessions_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "microfinance",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TellerSessions_CashVaults_CashVaultId",
                        column: x => x.CashVaultId,
                        principalSchema: "microfinance",
                        principalTable: "CashVaults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AgentBankings",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    BusinessName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ContactName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Address = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    GpsCoordinates = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Tier = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    LinkedStaffId = table.Column<Guid>(type: "uuid", nullable: true),
                    FloatBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MinFloatBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MaxFloatBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CommissionRate = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalCommissionEarned = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DailyTransactionLimit = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyTransactionLimit = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DailyVolumeProcessed = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyVolumeProcessed = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalTransactionsToday = table.Column<int>(type: "integer", nullable: false),
                    TotalTransactionsMonth = table.Column<int>(type: "integer", nullable: false),
                    ContractStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ContractEndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LastTrainingDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LastAuditDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsKycVerified = table.Column<bool>(type: "boolean", nullable: false),
                    DeviceId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    OperatingHours = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_AgentBankings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgentBankings_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "microfinance",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AgentBankings_Staff_LinkedStaffId",
                        column: x => x.LinkedStaffId,
                        principalSchema: "microfinance",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AmlAlerts",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AlertCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: true),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: true),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    AlertType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Severity = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TriggerRule = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    AlertedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InvestigationStartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AssignedToId = table.Column<Guid>(type: "uuid", nullable: true),
                    ResolvedById = table.Column<Guid>(type: "uuid", nullable: true),
                    ResolutionNotes = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    SarReference = table.Column<string>(type: "text", nullable: true),
                    SarFiledDate = table.Column<DateOnly>(type: "date", nullable: true),
                    RequiresReporting = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
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
                    table.PrimaryKey("PK_AmlAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmlAlerts_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AmlAlerts_Staff_AssignedToId",
                        column: x => x.AssignedToId,
                        principalSchema: "microfinance",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AmlAlerts_Staff_ResolvedById",
                        column: x => x.ResolvedById,
                        principalSchema: "microfinance",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Category = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    EntityType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    FilePath = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    MimeType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    OriginalFileName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IssuingAuthority = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DocumentNumber = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    VerifiedById = table.Column<Guid>(type: "uuid", nullable: true),
                    VerifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    Tags = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
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
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Staff_VerifiedById",
                        column: x => x.VerifiedById,
                        principalSchema: "microfinance",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "KycDocuments",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    DocumentNumber = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    FileName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    FilePath = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    MimeType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IssuingAuthority = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VerifiedById = table.Column<Guid>(type: "uuid", nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
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
                    table.PrimaryKey("PK_KycDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KycDocuments_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KycDocuments_Staff_VerifiedById",
                        column: x => x.VerifiedById,
                        principalSchema: "microfinance",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "LoanOfficerTargets",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StaffId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Period = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    PeriodStart = table.Column<DateOnly>(type: "date", nullable: false),
                    PeriodEnd = table.Column<DateOnly>(type: "date", nullable: false),
                    TargetValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MetricUnit = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    AchievedValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AchievementPercentage = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    MinimumThreshold = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    StretchTarget = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Weight = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IncentiveAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    StretchBonus = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 512, nullable: true),
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
                    table.PrimaryKey("PK_LoanOfficerTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanOfficerTargets_Staff_StaffId",
                        column: x => x.StaffId,
                        principalSchema: "microfinance",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffTrainings",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StaffId = table.Column<Guid>(type: "uuid", nullable: false),
                    TrainingCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    TrainingName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    TrainingType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    DeliveryMethod = table.Column<string>(type: "text", nullable: false),
                    Provider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Location = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DurationHours = table.Column<int>(type: "integer", nullable: true),
                    Score = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    PassingScore = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CertificateIssued = table.Column<bool>(type: "boolean", nullable: false),
                    CertificationNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    CertificationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CertificationExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TrainingCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CompletionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_StaffTrainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffTrainings_Staff_StaffId",
                        column: x => x.StaffId,
                        principalSchema: "microfinance",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CollectionCases",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CaseNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedCollectorId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Priority = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Classification = table.Column<string>(type: "text", nullable: false),
                    OpenedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    AssignedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ClosedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DaysPastDueAtOpen = table.Column<int>(type: "integer", nullable: false),
                    CurrentDaysPastDue = table.Column<int>(type: "integer", nullable: false),
                    AmountOverdue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalOutstanding = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AmountRecovered = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    LastContactDate = table.Column<DateOnly>(type: "date", nullable: true),
                    NextFollowUpDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ContactAttempts = table.Column<int>(type: "integer", nullable: false),
                    ClosureReason = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
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
                    table.PrimaryKey("PK_CollectionCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionCases_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CollectionCases_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CollectionCases_Staff_AssignedCollectorId",
                        column: x => x.AssignedCollectorId,
                        principalSchema: "microfinance",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CommunicationLogs",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: true),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: true),
                    Channel = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Recipient = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Subject = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Body = table.Column<string>(type: "character varying(16384)", maxLength: 16384, nullable: false),
                    DeliveryStatus = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OpenedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    ExternalId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    SentByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_CommunicationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunicationLogs_CommunicationTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalSchema: "microfinance",
                        principalTable: "CommunicationTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommunicationLogs_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommunicationLogs_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreditBureauInquiries",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: true),
                    InquiryNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    BureauName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Purpose = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    InquiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequestedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    RequestedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ResponseReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreditScore = table.Column<int>(type: "integer", nullable: true),
                    CreditReportId = table.Column<Guid>(type: "uuid", nullable: true),
                    InquiryCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_CreditBureauInquiries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditBureauInquiries_CreditBureauReports_CreditReportId",
                        column: x => x.CreditReportId,
                        principalSchema: "microfinance",
                        principalTable: "CreditBureauReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditBureauInquiries_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditBureauInquiries_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreditScores",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: true),
                    ScoreType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ScoreModel = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Score = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ScoreMin = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ScoreMax = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ScorePercentile = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Grade = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ProbabilityOfDefault = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    LossGivenDefault = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ExposureAtDefault = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ExpectedLoss = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ScoredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Source = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    CreditBureauReportId = table.Column<Guid>(type: "uuid", nullable: true),
                    ScoreFactors = table.Column<string>(type: "text", nullable: true),
                    ScoreChange = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    PreviousScoreId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
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
                    table.PrimaryKey("PK_CreditScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditScores_CreditBureauReports_CreditBureauReportId",
                        column: x => x.CreditBureauReportId,
                        principalSchema: "microfinance",
                        principalTable: "CreditBureauReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditScores_CreditScores_PreviousScoreId",
                        column: x => x.PreviousScoreId,
                        principalSchema: "microfinance",
                        principalTable: "CreditScores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditScores_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditScores_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InsurancePolicies",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InsuranceProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: true),
                    PolicyNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CoverageAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PremiumAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalPremiumPaid = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    NextPremiumDue = table.Column<DateOnly>(type: "date", nullable: true),
                    BeneficiaryName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    BeneficiaryRelation = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    BeneficiaryContact = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    WaitingPeriodEnd = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CancelledDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CancellationReason = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_InsurancePolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsurancePolicies_InsuranceProducts_InsuranceProductId",
                        column: x => x.InsuranceProductId,
                        principalSchema: "microfinance",
                        principalTable: "InsuranceProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InsurancePolicies_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InsurancePolicies_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InterestRateChanges",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ChangeType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EffectiveDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PreviousRate = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false),
                    NewRate = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false),
                    ChangeReason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ApprovedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ApprovalDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AppliedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
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
                    table.PrimaryKey("PK_InterestRateChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestRateChanges_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoanCollaterals",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    CollateralType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    EstimatedValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ForcedSaleValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ValuationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    DocumentReference = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_LoanCollaterals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanCollaterals_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoanDisbursementTranches",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    TrancheSequence = table.Column<int>(type: "integer", nullable: false),
                    TrancheNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ScheduledDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DisbursedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Deductions = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    NetAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DisbursementMethod = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    BankAccountNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    BankName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ReferenceNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Milestone = table.Column<string>(type: "text", nullable: true),
                    MilestoneVerified = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ApprovedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DisbursedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Notes = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    LoanId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
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
                    table.PrimaryKey("PK_LoanDisbursementTranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanDisbursementTranches_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoanDisbursementTranches_Loans_LoanId1",
                        column: x => x.LoanId1,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LoanGuarantors",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    GuarantorMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    GuaranteedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Relationship = table.Column<string>(type: "text", nullable: true),
                    GuaranteeDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_LoanGuarantors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanGuarantors_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoanGuarantors_Members_GuarantorMemberId",
                        column: x => x.GuarantorMemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoanRepayments",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    RepaymentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PrincipalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    InterestAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PenaltyAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ReversalReason = table.Column<string>(type: "text", nullable: true),
                    ReversedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_LoanRepayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanRepayments_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoanRestructures",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    RestructureNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    RestructureType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Reason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EffectiveDate = table.Column<DateOnly>(type: "date", nullable: true),
                    OriginalPrincipal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    OriginalInterestRate = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    OriginalRemainingTerm = table.Column<int>(type: "integer", nullable: false),
                    OriginalInstallmentAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    NewPrincipal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    NewInterestRate = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    NewTerm = table.Column<int>(type: "integer", nullable: false),
                    NewInstallmentAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    GracePeriodMonths = table.Column<int>(type: "integer", nullable: false),
                    WaivedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    RestructureFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ApprovedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
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
                    table.PrimaryKey("PK_LoanRestructures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanRestructures_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoanSchedules",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstallmentNumber = table.Column<int>(type: "integer", nullable: false),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PrincipalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    InterestAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PaidAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IsPaid = table.Column<bool>(type: "boolean", nullable: false),
                    PaidDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_LoanSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanSchedules_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoanWriteOffs",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    WriteOffNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    WriteOffType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Reason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    WriteOffDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PrincipalWriteOff = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    InterestWriteOff = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PenaltiesWriteOff = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    FeesWriteOff = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalWriteOff = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    RecoveredAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DaysPastDue = table.Column<int>(type: "integer", nullable: false),
                    CollectionAttempts = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ApprovedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
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
                    table.PrimaryKey("PK_LoanWriteOffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanWriteOffs_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupMemberships",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    JoinDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LeaveDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Role = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_GroupMemberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMemberships_MemberGroups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "microfinance",
                        principalTable: "MemberGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupMemberships_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoanApplications",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    ApplicationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RequestedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ApprovedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    RequestedTermMonths = table.Column<int>(type: "integer", nullable: false),
                    ApprovedTermMonths = table.Column<int>(type: "integer", nullable: true),
                    Purpose = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    BusinessType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    MonthlyIncome = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MonthlyExpenses = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ExistingDebt = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    DebtToIncomeRatio = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CreditScore = table.Column<int>(type: "integer", nullable: true),
                    RiskGrade = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    AssignedOfficerId = table.Column<Guid>(type: "uuid", nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DecisionByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DecisionAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    ApprovalConditions = table.Column<string>(type: "text", nullable: true),
                    ApprovalExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
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
                    table.PrimaryKey("PK_LoanApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanApplications_LoanProducts_LoanProductId",
                        column: x => x.LoanProductId,
                        principalSchema: "microfinance",
                        principalTable: "LoanProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoanApplications_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoanApplications_MemberGroups_MemberGroupId",
                        column: x => x.MemberGroupId,
                        principalSchema: "microfinance",
                        principalTable: "MemberGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoanApplications_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoanOfficerAssignments",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StaffId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignmentType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: true),
                    MemberGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: true),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    AssignmentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PreviousStaffId = table.Column<Guid>(type: "uuid", nullable: true),
                    Reason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
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
                    table.PrimaryKey("PK_LoanOfficerAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanOfficerAssignments_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "microfinance",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoanOfficerAssignments_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoanOfficerAssignments_MemberGroups_MemberGroupId",
                        column: x => x.MemberGroupId,
                        principalSchema: "microfinance",
                        principalTable: "MemberGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoanOfficerAssignments_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoanOfficerAssignments_Staff_PreviousStaffId",
                        column: x => x.PreviousStaffId,
                        principalSchema: "microfinance",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoanOfficerAssignments_Staff_StaffId",
                        column: x => x.StaffId,
                        principalSchema: "microfinance",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RiskAlerts",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RiskCategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    RiskIndicatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    AlertNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    Severity = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ThresholdValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ActualValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Variance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    AlertedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: true),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: true),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    AcknowledgedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    AcknowledgedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AssignedToUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Resolution = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    ResolvedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsEscalated = table.Column<bool>(type: "boolean", nullable: false),
                    EscalationLevel = table.Column<int>(type: "integer", nullable: false),
                    EscalatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_RiskAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskAlerts_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "microfinance",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RiskAlerts_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RiskAlerts_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RiskAlerts_RiskCategories_RiskCategoryId",
                        column: x => x.RiskCategoryId,
                        principalSchema: "microfinance",
                        principalTable: "RiskCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RiskAlerts_RiskIndicators_RiskIndicatorId",
                        column: x => x.RiskIndicatorId,
                        principalSchema: "microfinance",
                        principalTable: "RiskIndicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FixedDeposits",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CertificateNumber = table.Column<string>(type: "text", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    SavingsProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    LinkedSavingsAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    PrincipalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    InterestRate = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false),
                    TermMonths = table.Column<int>(type: "integer", nullable: false),
                    DepositDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MaturityDate = table.Column<DateOnly>(type: "date", nullable: false),
                    InterestEarned = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    InterestPaid = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MaturityInstruction = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ClosedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_FixedDeposits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedDeposits_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixedDeposits_SavingsAccounts_LinkedSavingsAccountId",
                        column: x => x.LinkedSavingsAccountId,
                        principalSchema: "microfinance",
                        principalTable: "SavingsAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FixedDeposits_SavingsProducts_SavingsProductId",
                        column: x => x.SavingsProductId,
                        principalSchema: "microfinance",
                        principalTable: "SavingsProducts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MobileWallets",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Provider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ExternalWalletId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Tier = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DailyLimit = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyLimit = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DailyUsed = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyUsed = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    KycVerifiedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LastActivityDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsLinkedToBankAccount = table.Column<bool>(type: "boolean", nullable: false),
                    LinkedSavingsAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsPinSet = table.Column<bool>(type: "boolean", nullable: false),
                    PinHash = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    FailedPinAttempts = table.Column<int>(type: "integer", nullable: false),
                    PinBlockedUntil = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_MobileWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MobileWallets_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MobileWallets_SavingsAccounts_LinkedSavingsAccountId",
                        column: x => x.LinkedSavingsAccountId,
                        principalSchema: "microfinance",
                        principalTable: "SavingsAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SavingsTransactions",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SavingsAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    TransactionType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    BalanceAfter = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TransactionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 256, nullable: true),
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
                    table.PrimaryKey("PK_SavingsTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavingsTransactions_SavingsAccounts_SavingsAccountId",
                        column: x => x.SavingsAccountId,
                        principalSchema: "microfinance",
                        principalTable: "SavingsAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeeCharges",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FeeDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: true),
                    SavingsAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    ShareAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ChargeDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AmountPaid = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    PaidDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
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
                    table.PrimaryKey("PK_FeeCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeCharges_FeeDefinitions_FeeDefinitionId",
                        column: x => x.FeeDefinitionId,
                        principalSchema: "microfinance",
                        principalTable: "FeeDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeCharges_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeCharges_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeCharges_SavingsAccounts_SavingsAccountId",
                        column: x => x.SavingsAccountId,
                        principalSchema: "microfinance",
                        principalTable: "SavingsAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeCharges_ShareAccounts_ShareAccountId",
                        column: x => x.ShareAccountId,
                        principalSchema: "microfinance",
                        principalTable: "ShareAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShareTransactions",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ShareAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "text", nullable: false),
                    TransactionType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    NumberOfShares = table.Column<int>(type: "integer", nullable: false),
                    PricePerShare = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SharesBalanceAfter = table.Column<int>(type: "integer", nullable: false),
                    TransactionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PaymentMethod = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_ShareTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShareTransactions_ShareAccounts_ShareAccountId",
                        column: x => x.ShareAccountId,
                        principalSchema: "microfinance",
                        principalTable: "ShareAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CollectionActions",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CollectionCaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ActionDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PerformedById = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactMethod = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberCalled = table.Column<string>(type: "text", nullable: true),
                    ContactPerson = table.Column<string>(type: "text", nullable: true),
                    Outcome = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    PromisedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    PromisedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    NextFollowUpDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
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
                    table.PrimaryKey("PK_CollectionActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionActions_CollectionCases_CollectionCaseId",
                        column: x => x.CollectionCaseId,
                        principalSchema: "microfinance",
                        principalTable: "CollectionCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CollectionActions_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CollectionActions_Staff_PerformedById",
                        column: x => x.PerformedById,
                        principalSchema: "microfinance",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DebtSettlements",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CollectionCaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    SettlementType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    OriginalOutstanding = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SettlementAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AmountPaid = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    RemainingBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    NumberOfInstallments = table.Column<int>(type: "integer", nullable: true),
                    InstallmentAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ProposedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ApprovedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CompletedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Terms = table.Column<string>(type: "text", nullable: false),
                    Justification = table.Column<string>(type: "text", nullable: true),
                    ProposedById = table.Column<Guid>(type: "uuid", nullable: false),
                    ApprovedById = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
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
                    table.PrimaryKey("PK_DebtSettlements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DebtSettlements_CollectionCases_CollectionCaseId",
                        column: x => x.CollectionCaseId,
                        principalSchema: "microfinance",
                        principalTable: "CollectionCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DebtSettlements_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DebtSettlements_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DebtSettlements_Staff_ApprovedById",
                        column: x => x.ApprovedById,
                        principalSchema: "microfinance",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DebtSettlements_Staff_ProposedById",
                        column: x => x.ProposedById,
                        principalSchema: "microfinance",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LegalActions",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CollectionCaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    CaseReference = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ActionType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    InitiatedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    FiledDate = table.Column<DateOnly>(type: "date", nullable: true),
                    NextHearingDate = table.Column<DateOnly>(type: "date", nullable: true),
                    JudgmentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ClosedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CourtName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    LawyerName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ClaimAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    JudgmentAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    AmountRecovered = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    LegalCosts = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CourtFees = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    JudgmentSummary = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
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
                    table.PrimaryKey("PK_LegalActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalActions_CollectionCases_CollectionCaseId",
                        column: x => x.CollectionCaseId,
                        principalSchema: "microfinance",
                        principalTable: "CollectionCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LegalActions_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LegalActions_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceClaims",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InsurancePolicyId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ClaimType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    IncidentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    FiledDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ClaimAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ApprovedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    PaidAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ReviewedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApprovedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DecisionAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    PaymentReference = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    PaymentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Documents = table.Column<string>(type: "text", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
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
                    table.PrimaryKey("PK_InsuranceClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsuranceClaims_InsurancePolicies_InsurancePolicyId",
                        column: x => x.InsurancePolicyId,
                        principalSchema: "microfinance",
                        principalTable: "InsurancePolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CollateralValuations",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CollateralId = table.Column<Guid>(type: "uuid", nullable: false),
                    ValuationReference = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ValuationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ValuationMethod = table.Column<string>(type: "text", nullable: false),
                    AppraiserName = table.Column<string>(type: "text", nullable: true),
                    AppraiserCompany = table.Column<string>(type: "text", nullable: true),
                    AppraiserLicense = table.Column<string>(type: "text", nullable: true),
                    MarketValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ForcedSaleValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    InsurableValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PreviousValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ValueChange = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ValueChangePercent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Condition = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    DocumentPath = table.Column<string>(type: "text", nullable: true),
                    ApprovedById = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    RejectionReason = table.Column<string>(type: "text", nullable: true),
                    LoanCollateralId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
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
                    table.PrimaryKey("PK_CollateralValuations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollateralValuations_LoanCollaterals_LoanCollateralId",
                        column: x => x.LoanCollateralId,
                        principalSchema: "microfinance",
                        principalTable: "LoanCollaterals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MobileTransactions",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionReference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TransactionType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Fee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    NetAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SourcePhone = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    DestinationPhone = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    RecipientWalletId = table.Column<Guid>(type: "uuid", nullable: true),
                    LinkedLoanId = table.Column<Guid>(type: "uuid", nullable: true),
                    LinkedSavingsAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProviderReference = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ProviderResponse = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    InitiatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FailureReason = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    ReversalOfTransactionId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReversedByTransactionId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_MobileTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MobileTransactions_Loans_LinkedLoanId",
                        column: x => x.LinkedLoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MobileTransactions_MobileTransactions_ReversalOfTransaction~",
                        column: x => x.ReversalOfTransactionId,
                        principalSchema: "microfinance",
                        principalTable: "MobileTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MobileTransactions_MobileWallets_RecipientWalletId",
                        column: x => x.RecipientWalletId,
                        principalSchema: "microfinance",
                        principalTable: "MobileWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MobileTransactions_MobileWallets_WalletId",
                        column: x => x.WalletId,
                        principalSchema: "microfinance",
                        principalTable: "MobileWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MobileTransactions_SavingsAccounts_LinkedSavingsAccountId",
                        column: x => x.LinkedSavingsAccountId,
                        principalSchema: "microfinance",
                        principalTable: "SavingsAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UssdSessions",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ServiceCode = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: true),
                    WalletId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CurrentMenu = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Language = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    CurrentOperation = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    SessionData = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    MenuLevel = table.Column<int>(type: "integer", nullable: false),
                    StepCount = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastActivityAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    SessionTimeoutSeconds = table.Column<int>(type: "integer", nullable: false),
                    LastInput = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    LastOutput = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    IsAuthenticated = table.Column<bool>(type: "boolean", nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_UssdSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UssdSessions_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UssdSessions_MobileWallets_WalletId",
                        column: x => x.WalletId,
                        principalSchema: "microfinance",
                        principalTable: "MobileWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "FeePayments",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FeeChargeId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanRepaymentId = table.Column<Guid>(type: "uuid", nullable: true),
                    SavingsTransactionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Reference = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    PaymentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    PaymentSource = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ReversalReason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    ReversedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
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
                    table.PrimaryKey("PK_FeePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeePayments_FeeCharges_FeeChargeId",
                        column: x => x.FeeChargeId,
                        principalSchema: "microfinance",
                        principalTable: "FeeCharges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeeWaivers",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FeeChargeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reference = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    WaiverType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    OriginalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    WaivedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    WaiverReason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ApprovedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedBy = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ApprovalDate = table.Column<DateOnly>(type: "date", nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
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
                    table.PrimaryKey("PK_FeeWaivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeWaivers_FeeCharges_FeeChargeId",
                        column: x => x.FeeChargeId,
                        principalSchema: "microfinance",
                        principalTable: "FeeCharges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PromiseToPays",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CollectionCaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    CollectionActionId = table.Column<Guid>(type: "uuid", nullable: true),
                    PromiseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PromisedPaymentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PromisedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ActualAmountPaid = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ActualPaymentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    PaymentMethod = table.Column<string>(type: "text", nullable: true),
                    BreachReason = table.Column<string>(type: "text", nullable: true),
                    RescheduleCount = table.Column<int>(type: "integer", nullable: false),
                    RecordedById = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 4096, nullable: true),
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
                    table.PrimaryKey("PK_PromiseToPays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromiseToPays_CollectionActions_CollectionActionId",
                        column: x => x.CollectionActionId,
                        principalSchema: "microfinance",
                        principalTable: "CollectionActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PromiseToPays_CollectionCases_CollectionCaseId",
                        column: x => x.CollectionCaseId,
                        principalSchema: "microfinance",
                        principalTable: "CollectionCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PromiseToPays_Loans_LoanId",
                        column: x => x.LoanId,
                        principalSchema: "microfinance",
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PromiseToPays_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PromiseToPays_Staff_RecordedById",
                        column: x => x.RecordedById,
                        principalSchema: "microfinance",
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QrPayments",
                schema: "microfinance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WalletId = table.Column<Guid>(type: "uuid", nullable: true),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: true),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: true),
                    QrCode = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    QrType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Status = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Reference = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    MaxUses = table.Column<int>(type: "integer", nullable: false),
                    CurrentUses = table.Column<int>(type: "integer", nullable: false),
                    GeneratedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastUsedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastTransactionId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_QrPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QrPayments_AgentBankings_AgentId",
                        column: x => x.AgentId,
                        principalSchema: "microfinance",
                        principalTable: "AgentBankings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_QrPayments_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "microfinance",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_QrPayments_MobileTransactions_LastTransactionId",
                        column: x => x.LastTransactionId,
                        principalSchema: "microfinance",
                        principalTable: "MobileTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_QrPayments_MobileWallets_WalletId",
                        column: x => x.WalletId,
                        principalSchema: "microfinance",
                        principalTable: "MobileWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgentBankings_AgentCode",
                schema: "microfinance",
                table: "AgentBankings",
                column: "AgentCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgentBankings_BranchId",
                schema: "microfinance",
                table: "AgentBankings",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentBankings_LinkedStaffId",
                schema: "microfinance",
                table: "AgentBankings",
                column: "LinkedStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentBankings_Status",
                schema: "microfinance",
                table: "AgentBankings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AmlAlerts_AlertCode",
                schema: "microfinance",
                table: "AmlAlerts",
                column: "AlertCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AmlAlerts_AlertType",
                schema: "microfinance",
                table: "AmlAlerts",
                column: "AlertType");

            migrationBuilder.CreateIndex(
                name: "IX_AmlAlerts_AssignedToId",
                schema: "microfinance",
                table: "AmlAlerts",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_AmlAlerts_MemberId",
                schema: "microfinance",
                table: "AmlAlerts",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_AmlAlerts_ResolvedById",
                schema: "microfinance",
                table: "AmlAlerts",
                column: "ResolvedById");

            migrationBuilder.CreateIndex(
                name: "IX_AmlAlerts_Severity",
                schema: "microfinance",
                table: "AmlAlerts",
                column: "Severity");

            migrationBuilder.CreateIndex(
                name: "IX_AmlAlerts_Status",
                schema: "microfinance",
                table: "AmlAlerts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalDecisions_ApproverId",
                schema: "microfinance",
                table: "ApprovalDecisions",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalDecisions_DecisionAt",
                schema: "microfinance",
                table: "ApprovalDecisions",
                column: "DecisionAt");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalDecisions_Request_Level",
                schema: "microfinance",
                table: "ApprovalDecisions",
                columns: new[] { "RequestId", "Level" });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalDecisions_RequestId",
                schema: "microfinance",
                table: "ApprovalDecisions",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalLevels_Workflow_Level",
                schema: "microfinance",
                table: "ApprovalLevels",
                columns: new[] { "WorkflowId", "LevelNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalLevels_WorkflowId",
                schema: "microfinance",
                table: "ApprovalLevels",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRequests_Entity",
                schema: "microfinance",
                table: "ApprovalRequests",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRequests_RequestNumber",
                schema: "microfinance",
                table: "ApprovalRequests",
                column: "RequestNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRequests_Status",
                schema: "microfinance",
                table: "ApprovalRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRequests_WorkflowId",
                schema: "microfinance",
                table: "ApprovalRequests",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalWorkflows_Code",
                schema: "microfinance",
                table: "ApprovalWorkflows",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalWorkflows_EntityType",
                schema: "microfinance",
                table: "ApprovalWorkflows",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalWorkflows_IsActive",
                schema: "microfinance",
                table: "ApprovalWorkflows",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_ParentBranchId",
                schema: "microfinance",
                table: "Branches",
                column: "ParentBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Status",
                schema: "microfinance",
                table: "Branches",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_BranchTargets_BranchId",
                schema: "microfinance",
                table: "BranchTargets",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchTargets_Status",
                schema: "microfinance",
                table: "BranchTargets",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CashVaults_BranchId",
                schema: "microfinance",
                table: "CashVaults",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CashVaults_CustodianUserId",
                schema: "microfinance",
                table: "CashVaults",
                column: "CustodianUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CashVaults_Status",
                schema: "microfinance",
                table: "CashVaults",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CollateralInsurances_CollateralId",
                schema: "microfinance",
                table: "CollateralInsurances",
                column: "CollateralId");

            migrationBuilder.CreateIndex(
                name: "IX_CollateralInsurances_Status",
                schema: "microfinance",
                table: "CollateralInsurances",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CollateralReleases_ApprovedById",
                schema: "microfinance",
                table: "CollateralReleases",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_CollateralReleases_CollateralId",
                schema: "microfinance",
                table: "CollateralReleases",
                column: "CollateralId");

            migrationBuilder.CreateIndex(
                name: "IX_CollateralReleases_LoanId",
                schema: "microfinance",
                table: "CollateralReleases",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_CollateralReleases_ReleasedById",
                schema: "microfinance",
                table: "CollateralReleases",
                column: "ReleasedById");

            migrationBuilder.CreateIndex(
                name: "IX_CollateralReleases_RequestedById",
                schema: "microfinance",
                table: "CollateralReleases",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_CollateralReleases_Status",
                schema: "microfinance",
                table: "CollateralReleases",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CollateralTypes_Status",
                schema: "microfinance",
                table: "CollateralTypes",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CollateralValuations_ApprovedById",
                schema: "microfinance",
                table: "CollateralValuations",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_CollateralValuations_CollateralId",
                schema: "microfinance",
                table: "CollateralValuations",
                column: "CollateralId");

            migrationBuilder.CreateIndex(
                name: "IX_CollateralValuations_LoanCollateralId",
                schema: "microfinance",
                table: "CollateralValuations",
                column: "LoanCollateralId");

            migrationBuilder.CreateIndex(
                name: "IX_CollateralValuations_Status",
                schema: "microfinance",
                table: "CollateralValuations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionActions_ActionDateTime",
                schema: "microfinance",
                table: "CollectionActions",
                column: "ActionDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionActions_ActionType",
                schema: "microfinance",
                table: "CollectionActions",
                column: "ActionType");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionActions_CollectionCaseId",
                schema: "microfinance",
                table: "CollectionActions",
                column: "CollectionCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionActions_LoanId",
                schema: "microfinance",
                table: "CollectionActions",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionActions_PerformedById",
                schema: "microfinance",
                table: "CollectionActions",
                column: "PerformedById");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionCases_AssignedCollectorId",
                schema: "microfinance",
                table: "CollectionCases",
                column: "AssignedCollectorId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionCases_CaseNumber",
                schema: "microfinance",
                table: "CollectionCases",
                column: "CaseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollectionCases_LoanId",
                schema: "microfinance",
                table: "CollectionCases",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionCases_MemberId",
                schema: "microfinance",
                table: "CollectionCases",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionCases_Status",
                schema: "microfinance",
                table: "CollectionCases",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionStrategies_LoanProductId",
                schema: "microfinance",
                table: "CollectionStrategies",
                column: "LoanProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationLogs_LoanId",
                schema: "microfinance",
                table: "CommunicationLogs",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationLogs_MemberId",
                schema: "microfinance",
                table: "CommunicationLogs",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationLogs_SentByUserId",
                schema: "microfinance",
                table: "CommunicationLogs",
                column: "SentByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationLogs_TemplateId",
                schema: "microfinance",
                table: "CommunicationLogs",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationTemplates_Status",
                schema: "microfinance",
                table: "CommunicationTemplates",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CreditBureauInquiries_CreditReportId",
                schema: "microfinance",
                table: "CreditBureauInquiries",
                column: "CreditReportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreditBureauInquiries_LoanId",
                schema: "microfinance",
                table: "CreditBureauInquiries",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditBureauInquiries_MemberId",
                schema: "microfinance",
                table: "CreditBureauInquiries",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditBureauInquiries_RequestedByUserId",
                schema: "microfinance",
                table: "CreditBureauInquiries",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditBureauInquiries_Status",
                schema: "microfinance",
                table: "CreditBureauInquiries",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CreditBureauReports_MemberId",
                schema: "microfinance",
                table: "CreditBureauReports",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditBureauReports_Status",
                schema: "microfinance",
                table: "CreditBureauReports",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CreditScores_CreditBureauReportId",
                schema: "microfinance",
                table: "CreditScores",
                column: "CreditBureauReportId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditScores_LoanId",
                schema: "microfinance",
                table: "CreditScores",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditScores_MemberId",
                schema: "microfinance",
                table: "CreditScores",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditScores_PreviousScoreId",
                schema: "microfinance",
                table: "CreditScores",
                column: "PreviousScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditScores_Status",
                schema: "microfinance",
                table: "CreditScores",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCases_AssignedToId",
                schema: "microfinance",
                table: "CustomerCases",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCases_EscalatedToId",
                schema: "microfinance",
                table: "CustomerCases",
                column: "EscalatedToId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCases_MemberId",
                schema: "microfinance",
                table: "CustomerCases",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCases_RelatedLoanId",
                schema: "microfinance",
                table: "CustomerCases",
                column: "RelatedLoanId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCases_RelatedSavingsAccountId",
                schema: "microfinance",
                table: "CustomerCases",
                column: "RelatedSavingsAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCases_Status",
                schema: "microfinance",
                table: "CustomerCases",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSegments_Status",
                schema: "microfinance",
                table: "CustomerSegments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSurveys_BranchId",
                schema: "microfinance",
                table: "CustomerSurveys",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSurveys_Status",
                schema: "microfinance",
                table: "CustomerSurveys",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DebtSettlements_ApprovedById",
                schema: "microfinance",
                table: "DebtSettlements",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_DebtSettlements_CollectionCaseId",
                schema: "microfinance",
                table: "DebtSettlements",
                column: "CollectionCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DebtSettlements_LoanId",
                schema: "microfinance",
                table: "DebtSettlements",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_DebtSettlements_MemberId",
                schema: "microfinance",
                table: "DebtSettlements",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_DebtSettlements_ProposedById",
                schema: "microfinance",
                table: "DebtSettlements",
                column: "ProposedById");

            migrationBuilder.CreateIndex(
                name: "IX_DebtSettlements_ReferenceNumber",
                schema: "microfinance",
                table: "DebtSettlements",
                column: "ReferenceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DebtSettlements_Status",
                schema: "microfinance",
                table: "DebtSettlements",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentType",
                schema: "microfinance",
                table: "Documents",
                column: "DocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Entity",
                schema: "microfinance",
                table: "Documents",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_EntityId",
                schema: "microfinance",
                table: "Documents",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Status",
                schema: "microfinance",
                table: "Documents",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_VerifiedById",
                schema: "microfinance",
                table: "Documents",
                column: "VerifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCharges_FeeDefinitionId",
                schema: "microfinance",
                table: "FeeCharges",
                column: "FeeDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCharges_LoanId",
                schema: "microfinance",
                table: "FeeCharges",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCharges_MemberId",
                schema: "microfinance",
                table: "FeeCharges",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCharges_SavingsAccountId",
                schema: "microfinance",
                table: "FeeCharges",
                column: "SavingsAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCharges_ShareAccountId",
                schema: "microfinance",
                table: "FeeCharges",
                column: "ShareAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCharges_Status",
                schema: "microfinance",
                table: "FeeCharges",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FeeDefinitions_Code",
                schema: "microfinance",
                table: "FeeDefinitions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeeDefinitions_FeeType",
                schema: "microfinance",
                table: "FeeDefinitions",
                column: "FeeType");

            migrationBuilder.CreateIndex(
                name: "IX_FeeDefinitions_IsActive",
                schema: "microfinance",
                table: "FeeDefinitions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_FeePayments_FeeChargeId",
                schema: "microfinance",
                table: "FeePayments",
                column: "FeeChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeePayments_PaymentDate",
                schema: "microfinance",
                table: "FeePayments",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_FeePayments_Status",
                schema: "microfinance",
                table: "FeePayments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FeeWaivers_FeeChargeId",
                schema: "microfinance",
                table: "FeeWaivers",
                column: "FeeChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeWaivers_RequestDate",
                schema: "microfinance",
                table: "FeeWaivers",
                column: "RequestDate");

            migrationBuilder.CreateIndex(
                name: "IX_FeeWaivers_Status",
                schema: "microfinance",
                table: "FeeWaivers",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeposits_LinkedSavingsAccountId",
                schema: "microfinance",
                table: "FixedDeposits",
                column: "LinkedSavingsAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeposits_MaturityDate",
                schema: "microfinance",
                table: "FixedDeposits",
                column: "MaturityDate");

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeposits_MemberId",
                schema: "microfinance",
                table: "FixedDeposits",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeposits_SavingsProductId",
                schema: "microfinance",
                table: "FixedDeposits",
                column: "SavingsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeposits_Status",
                schema: "microfinance",
                table: "FixedDeposits",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberships_GroupId",
                schema: "microfinance",
                table: "GroupMemberships",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberships_MemberId",
                schema: "microfinance",
                table: "GroupMemberships",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberships_Status",
                schema: "microfinance",
                table: "GroupMemberships",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceClaims_ApprovedByUserId",
                schema: "microfinance",
                table: "InsuranceClaims",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceClaims_InsurancePolicyId",
                schema: "microfinance",
                table: "InsuranceClaims",
                column: "InsurancePolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceClaims_ReviewedByUserId",
                schema: "microfinance",
                table: "InsuranceClaims",
                column: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceClaims_Status",
                schema: "microfinance",
                table: "InsuranceClaims",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_InsuranceProductId",
                schema: "microfinance",
                table: "InsurancePolicies",
                column: "InsuranceProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_LoanId",
                schema: "microfinance",
                table: "InsurancePolicies",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_MemberId",
                schema: "microfinance",
                table: "InsurancePolicies",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_Status",
                schema: "microfinance",
                table: "InsurancePolicies",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceProducts_Status",
                schema: "microfinance",
                table: "InsuranceProducts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_InterestRateChanges_EffectiveDate",
                schema: "microfinance",
                table: "InterestRateChanges",
                column: "EffectiveDate");

            migrationBuilder.CreateIndex(
                name: "IX_InterestRateChanges_LoanId",
                schema: "microfinance",
                table: "InterestRateChanges",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_InterestRateChanges_Status",
                schema: "microfinance",
                table: "InterestRateChanges",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentAccounts_AssignedAdvisorId",
                schema: "microfinance",
                table: "InvestmentAccounts",
                column: "AssignedAdvisorId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentAccounts_LinkedSavingsAccountId",
                schema: "microfinance",
                table: "InvestmentAccounts",
                column: "LinkedSavingsAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentAccounts_MemberId",
                schema: "microfinance",
                table: "InvestmentAccounts",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentAccounts_Status",
                schema: "microfinance",
                table: "InvestmentAccounts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentProducts_Status",
                schema: "microfinance",
                table: "InvestmentProducts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTransactions_InvestmentAccountId",
                schema: "microfinance",
                table: "InvestmentTransactions",
                column: "InvestmentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTransactions_ProductId",
                schema: "microfinance",
                table: "InvestmentTransactions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTransactions_SourceAccountId",
                schema: "microfinance",
                table: "InvestmentTransactions",
                column: "SourceAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTransactions_Status",
                schema: "microfinance",
                table: "InvestmentTransactions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTransactions_SwitchToProductId",
                schema: "microfinance",
                table: "InvestmentTransactions",
                column: "SwitchToProductId");

            migrationBuilder.CreateIndex(
                name: "IX_KycDocuments_DocumentType",
                schema: "microfinance",
                table: "KycDocuments",
                column: "DocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_KycDocuments_MemberDocument",
                schema: "microfinance",
                table: "KycDocuments",
                columns: new[] { "MemberId", "DocumentType" });

            migrationBuilder.CreateIndex(
                name: "IX_KycDocuments_MemberId",
                schema: "microfinance",
                table: "KycDocuments",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_KycDocuments_Status",
                schema: "microfinance",
                table: "KycDocuments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_KycDocuments_VerifiedById",
                schema: "microfinance",
                table: "KycDocuments",
                column: "VerifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_LegalActions_CaseReference",
                schema: "microfinance",
                table: "LegalActions",
                column: "CaseReference");

            migrationBuilder.CreateIndex(
                name: "IX_LegalActions_CollectionCaseId",
                schema: "microfinance",
                table: "LegalActions",
                column: "CollectionCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalActions_LoanId",
                schema: "microfinance",
                table: "LegalActions",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalActions_MemberId",
                schema: "microfinance",
                table: "LegalActions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalActions_Status",
                schema: "microfinance",
                table: "LegalActions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_AssignedOfficerId",
                schema: "microfinance",
                table: "LoanApplications",
                column: "AssignedOfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_DecisionByUserId",
                schema: "microfinance",
                table: "LoanApplications",
                column: "DecisionByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_LoanId",
                schema: "microfinance",
                table: "LoanApplications",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_LoanProductId",
                schema: "microfinance",
                table: "LoanApplications",
                column: "LoanProductId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_MemberGroupId",
                schema: "microfinance",
                table: "LoanApplications",
                column: "MemberGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_MemberId",
                schema: "microfinance",
                table: "LoanApplications",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_Status",
                schema: "microfinance",
                table: "LoanApplications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LoanCollaterals_CollateralType",
                schema: "microfinance",
                table: "LoanCollaterals",
                column: "CollateralType");

            migrationBuilder.CreateIndex(
                name: "IX_LoanCollaterals_LoanId",
                schema: "microfinance",
                table: "LoanCollaterals",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanCollaterals_Status",
                schema: "microfinance",
                table: "LoanCollaterals",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LoanDisbursementTranches_ApprovedByUserId",
                schema: "microfinance",
                table: "LoanDisbursementTranches",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanDisbursementTranches_DisbursedByUserId",
                schema: "microfinance",
                table: "LoanDisbursementTranches",
                column: "DisbursedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanDisbursementTranches_LoanId",
                schema: "microfinance",
                table: "LoanDisbursementTranches",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanDisbursementTranches_LoanId1",
                schema: "microfinance",
                table: "LoanDisbursementTranches",
                column: "LoanId1");

            migrationBuilder.CreateIndex(
                name: "IX_LoanDisbursementTranches_Status",
                schema: "microfinance",
                table: "LoanDisbursementTranches",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LoanGuarantors_GuarantorMemberId",
                schema: "microfinance",
                table: "LoanGuarantors",
                column: "GuarantorMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanGuarantors_LoanId",
                schema: "microfinance",
                table: "LoanGuarantors",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanGuarantors_Status",
                schema: "microfinance",
                table: "LoanGuarantors",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LoanOfficerAssignments_BranchId",
                schema: "microfinance",
                table: "LoanOfficerAssignments",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanOfficerAssignments_LoanId",
                schema: "microfinance",
                table: "LoanOfficerAssignments",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanOfficerAssignments_MemberGroupId",
                schema: "microfinance",
                table: "LoanOfficerAssignments",
                column: "MemberGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanOfficerAssignments_MemberId",
                schema: "microfinance",
                table: "LoanOfficerAssignments",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanOfficerAssignments_PreviousStaffId",
                schema: "microfinance",
                table: "LoanOfficerAssignments",
                column: "PreviousStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanOfficerAssignments_StaffId",
                schema: "microfinance",
                table: "LoanOfficerAssignments",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanOfficerAssignments_Status",
                schema: "microfinance",
                table: "LoanOfficerAssignments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LoanOfficerTargets_StaffId",
                schema: "microfinance",
                table: "LoanOfficerTargets",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanOfficerTargets_Status",
                schema: "microfinance",
                table: "LoanOfficerTargets",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LoanProducts_Code",
                schema: "microfinance",
                table: "LoanProducts",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanProducts_IsActive",
                schema: "microfinance",
                table: "LoanProducts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_LoanRepayments_LoanId",
                schema: "microfinance",
                table: "LoanRepayments",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanRepayments_ReceiptNumber",
                schema: "microfinance",
                table: "LoanRepayments",
                column: "ReceiptNumber");

            migrationBuilder.CreateIndex(
                name: "IX_LoanRestructures_ApprovedByUserId",
                schema: "microfinance",
                table: "LoanRestructures",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanRestructures_LoanId",
                schema: "microfinance",
                table: "LoanRestructures",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanRestructures_Status",
                schema: "microfinance",
                table: "LoanRestructures",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LoanNumber",
                schema: "microfinance",
                table: "Loans",
                column: "LoanNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LoanProductId",
                schema: "microfinance",
                table: "Loans",
                column: "LoanProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_MemberId",
                schema: "microfinance",
                table: "Loans",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_Status",
                schema: "microfinance",
                table: "Loans",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LoanSchedules_LoanId",
                schema: "microfinance",
                table: "LoanSchedules",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanSchedules_Unique",
                schema: "microfinance",
                table: "LoanSchedules",
                columns: new[] { "LoanId", "InstallmentNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanWriteOffs_ApprovedByUserId",
                schema: "microfinance",
                table: "LoanWriteOffs",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanWriteOffs_LoanId",
                schema: "microfinance",
                table: "LoanWriteOffs",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanWriteOffs_Status",
                schema: "microfinance",
                table: "LoanWriteOffs",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MarketingCampaigns_ApprovedById",
                schema: "microfinance",
                table: "MarketingCampaigns",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_MarketingCampaigns_CreatedById",
                schema: "microfinance",
                table: "MarketingCampaigns",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MarketingCampaigns_Status",
                schema: "microfinance",
                table: "MarketingCampaigns",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MemberGroups_Code",
                schema: "microfinance",
                table: "MemberGroups",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberGroups_LeaderMemberId",
                schema: "microfinance",
                table: "MemberGroups",
                column: "LeaderMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberGroups_LoanOfficerId",
                schema: "microfinance",
                table: "MemberGroups",
                column: "LoanOfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberGroups_Status",
                schema: "microfinance",
                table: "MemberGroups",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Members_Email",
                schema: "microfinance",
                table: "Members",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Members_IsActive",
                schema: "microfinance",
                table: "Members",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Members_MemberNumber",
                schema: "microfinance",
                table: "Members",
                column: "MemberNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_Name",
                schema: "microfinance",
                table: "Members",
                columns: new[] { "LastName", "FirstName" });

            migrationBuilder.CreateIndex(
                name: "IX_Members_NationalId",
                schema: "microfinance",
                table: "Members",
                column: "NationalId");

            migrationBuilder.CreateIndex(
                name: "IX_MfiConfigurations_BranchId",
                schema: "microfinance",
                table: "MfiConfigurations",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileTransactions_InitiatedAt",
                schema: "microfinance",
                table: "MobileTransactions",
                column: "InitiatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MobileTransactions_LinkedLoanId",
                schema: "microfinance",
                table: "MobileTransactions",
                column: "LinkedLoanId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileTransactions_LinkedSavingsAccountId",
                schema: "microfinance",
                table: "MobileTransactions",
                column: "LinkedSavingsAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileTransactions_RecipientWalletId",
                schema: "microfinance",
                table: "MobileTransactions",
                column: "RecipientWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileTransactions_ReversalOfTransactionId",
                schema: "microfinance",
                table: "MobileTransactions",
                column: "ReversalOfTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileTransactions_Status",
                schema: "microfinance",
                table: "MobileTransactions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MobileTransactions_TransactionReference",
                schema: "microfinance",
                table: "MobileTransactions",
                column: "TransactionReference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MobileTransactions_WalletId",
                schema: "microfinance",
                table: "MobileTransactions",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileWallets_LinkedSavingsAccountId",
                schema: "microfinance",
                table: "MobileWallets",
                column: "LinkedSavingsAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileWallets_MemberId",
                schema: "microfinance",
                table: "MobileWallets",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileWallets_PhoneNumber",
                schema: "microfinance",
                table: "MobileWallets",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MobileWallets_Status",
                schema: "microfinance",
                table: "MobileWallets",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentGateways_Status",
                schema: "microfinance",
                table: "PaymentGateways",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PromiseToPays_CollectionActionId",
                schema: "microfinance",
                table: "PromiseToPays",
                column: "CollectionActionId");

            migrationBuilder.CreateIndex(
                name: "IX_PromiseToPays_CollectionCaseId",
                schema: "microfinance",
                table: "PromiseToPays",
                column: "CollectionCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PromiseToPays_LoanId",
                schema: "microfinance",
                table: "PromiseToPays",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_PromiseToPays_MemberId",
                schema: "microfinance",
                table: "PromiseToPays",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_PromiseToPays_PromisedPaymentDate",
                schema: "microfinance",
                table: "PromiseToPays",
                column: "PromisedPaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_PromiseToPays_RecordedById",
                schema: "microfinance",
                table: "PromiseToPays",
                column: "RecordedById");

            migrationBuilder.CreateIndex(
                name: "IX_PromiseToPays_Status",
                schema: "microfinance",
                table: "PromiseToPays",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_QrPayments_AgentId",
                schema: "microfinance",
                table: "QrPayments",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_QrPayments_LastTransactionId",
                schema: "microfinance",
                table: "QrPayments",
                column: "LastTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_QrPayments_MemberId",
                schema: "microfinance",
                table: "QrPayments",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_QrPayments_QrCode",
                schema: "microfinance",
                table: "QrPayments",
                column: "QrCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QrPayments_Status",
                schema: "microfinance",
                table: "QrPayments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_QrPayments_WalletId",
                schema: "microfinance",
                table: "QrPayments",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportDefinitions_Status",
                schema: "microfinance",
                table: "ReportDefinitions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ReportGenerations_BranchId",
                schema: "microfinance",
                table: "ReportGenerations",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportGenerations_ReportDefinitionId",
                schema: "microfinance",
                table: "ReportGenerations",
                column: "ReportDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportGenerations_RequestedByUserId",
                schema: "microfinance",
                table: "ReportGenerations",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportGenerations_Status",
                schema: "microfinance",
                table: "ReportGenerations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAlerts_BranchId",
                schema: "microfinance",
                table: "RiskAlerts",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAlerts_LoanId",
                schema: "microfinance",
                table: "RiskAlerts",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAlerts_MemberId",
                schema: "microfinance",
                table: "RiskAlerts",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAlerts_RiskCategoryId",
                schema: "microfinance",
                table: "RiskAlerts",
                column: "RiskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAlerts_RiskIndicatorId",
                schema: "microfinance",
                table: "RiskAlerts",
                column: "RiskIndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskAlerts_Status",
                schema: "microfinance",
                table: "RiskAlerts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_RiskCategories_ParentCategoryId",
                schema: "microfinance",
                table: "RiskCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskCategories_Status",
                schema: "microfinance",
                table: "RiskCategories",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_RiskIndicators_RiskCategoryId",
                schema: "microfinance",
                table: "RiskIndicators",
                column: "RiskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskIndicators_Status",
                schema: "microfinance",
                table: "RiskIndicators",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SavingsAccounts_AccountNumber",
                schema: "microfinance",
                table: "SavingsAccounts",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavingsAccounts_MemberId",
                schema: "microfinance",
                table: "SavingsAccounts",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_SavingsAccounts_OpenedDate",
                schema: "microfinance",
                table: "SavingsAccounts",
                column: "OpenedDate");

            migrationBuilder.CreateIndex(
                name: "IX_SavingsAccounts_SavingsProductId",
                schema: "microfinance",
                table: "SavingsAccounts",
                column: "SavingsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SavingsAccounts_Status",
                schema: "microfinance",
                table: "SavingsAccounts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SavingsProducts_Code",
                schema: "microfinance",
                table: "SavingsProducts",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavingsProducts_IsActive",
                schema: "microfinance",
                table: "SavingsProducts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_SavingsTransactions_Reference",
                schema: "microfinance",
                table: "SavingsTransactions",
                column: "Reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavingsTransactions_SavingsAccountId",
                schema: "microfinance",
                table: "SavingsTransactions",
                column: "SavingsAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SavingsTransactions_TransactionDate",
                schema: "microfinance",
                table: "SavingsTransactions",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_SavingsTransactions_TransactionType",
                schema: "microfinance",
                table: "SavingsTransactions",
                column: "TransactionType");

            migrationBuilder.CreateIndex(
                name: "IX_ShareAccounts_AccountNumber",
                schema: "microfinance",
                table: "ShareAccounts",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShareAccounts_MemberId",
                schema: "microfinance",
                table: "ShareAccounts",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareAccounts_ShareProductId",
                schema: "microfinance",
                table: "ShareAccounts",
                column: "ShareProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareAccounts_Status",
                schema: "microfinance",
                table: "ShareAccounts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ShareProducts_Code",
                schema: "microfinance",
                table: "ShareProducts",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShareProducts_IsActive",
                schema: "microfinance",
                table: "ShareProducts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ShareTransactions_ShareAccountId",
                schema: "microfinance",
                table: "ShareTransactions",
                column: "ShareAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareTransactions_TransactionType",
                schema: "microfinance",
                table: "ShareTransactions",
                column: "TransactionType");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_BranchId",
                schema: "microfinance",
                table: "Staff",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_ReportingManagerId",
                schema: "microfinance",
                table: "Staff",
                column: "ReportingManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_Status",
                schema: "microfinance",
                table: "Staff",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_UserId",
                schema: "microfinance",
                table: "Staff",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffTrainings_StaffId",
                schema: "microfinance",
                table: "StaffTrainings",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffTrainings_Status",
                schema: "microfinance",
                table: "StaffTrainings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TellerSessions_BranchId",
                schema: "microfinance",
                table: "TellerSessions",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TellerSessions_CashVaultId",
                schema: "microfinance",
                table: "TellerSessions",
                column: "CashVaultId");

            migrationBuilder.CreateIndex(
                name: "IX_TellerSessions_Status",
                schema: "microfinance",
                table: "TellerSessions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TellerSessions_SupervisorUserId",
                schema: "microfinance",
                table: "TellerSessions",
                column: "SupervisorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TellerSessions_TellerUserId",
                schema: "microfinance",
                table: "TellerSessions",
                column: "TellerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UssdSessions_MemberId",
                schema: "microfinance",
                table: "UssdSessions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_UssdSessions_PhoneNumber",
                schema: "microfinance",
                table: "UssdSessions",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_UssdSessions_SessionId",
                schema: "microfinance",
                table: "UssdSessions",
                column: "SessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UssdSessions_Status",
                schema: "microfinance",
                table: "UssdSessions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_UssdSessions_WalletId",
                schema: "microfinance",
                table: "UssdSessions",
                column: "WalletId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmlAlerts",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "ApprovalDecisions",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "ApprovalLevels",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "BranchTargets",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "CollateralInsurances",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "CollateralReleases",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "CollateralTypes",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "CollateralValuations",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "CollectionStrategies",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "CommunicationLogs",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "CreditBureauInquiries",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "CreditScores",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "CustomerCases",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "CustomerSegments",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "CustomerSurveys",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "DebtSettlements",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "Documents",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "FeePayments",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "FeeWaivers",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "FixedDeposits",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "GroupMemberships",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "InsuranceClaims",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "InterestRateChanges",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "InvestmentProducts",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "InvestmentTransactions",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "KycDocuments",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "LegalActions",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "LoanApplications",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "LoanDisbursementTranches",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "LoanGuarantors",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "LoanOfficerAssignments",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "LoanOfficerTargets",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "LoanRepayments",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "LoanRestructures",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "LoanSchedules",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "LoanWriteOffs",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "MarketingCampaigns",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "MfiConfigurations",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "PaymentGateways",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "PromiseToPays",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "QrPayments",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "ReportGenerations",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "RiskAlerts",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "SavingsTransactions",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "ShareTransactions",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "StaffTrainings",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "TellerSessions",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "UssdSessions",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "ApprovalRequests",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "LoanCollaterals",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "CommunicationTemplates",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "CreditBureauReports",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "FeeCharges",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "InsurancePolicies",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "InvestmentAccounts",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "MemberGroups",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "CollectionActions",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "AgentBankings",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "MobileTransactions",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "ReportDefinitions",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "RiskIndicators",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "CashVaults",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "ApprovalWorkflows",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "FeeDefinitions",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "ShareAccounts",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "InsuranceProducts",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "CollectionCases",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "MobileWallets",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "RiskCategories",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "ShareProducts",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "Loans",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "Staff",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "SavingsAccounts",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "LoanProducts",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "Branches",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "Members",
                schema: "microfinance");

            migrationBuilder.DropTable(
                name: "SavingsProducts",
                schema: "microfinance");
        }
    }
}
