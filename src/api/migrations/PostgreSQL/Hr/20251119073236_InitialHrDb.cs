using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.PostgreSQL.Hr
{
    /// <inheritdoc />
    public partial class InitialHrDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "humanresources");

            migrationBuilder.CreateTable(
                name: "AttendanceReport",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FromDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ToDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GeneratedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    TotalWorkingDays = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    TotalEmployees = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    PresentCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    AbsentCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    LateCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    HalfDayCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    OnLeaveCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    AttendancePercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 0m),
                    LatePercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 0m),
                    ReportData = table.Column<string>(type: "jsonb", nullable: true),
                    ExportPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_AttendanceReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Benefits",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BenefitName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BenefitType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EmployeeContribution = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    EmployerContribution = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    EffectiveStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EffectiveEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CoverageType = table.Column<string>(type: "text", nullable: true),
                    ProviderName = table.Column<string>(type: "text", nullable: true),
                    CoverageAmount = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    WaitingPeriodDays = table.Column<int>(type: "integer", nullable: true),
                    AnnualLimit = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_Benefits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deductions",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeductionName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DeductionType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RecoveryMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RecoveryFixedAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    RecoveryPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    InstallmentCount = table.Column<int>(type: "integer", nullable: true),
                    MaxRecoveryPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    RequiresApproval = table.Column<bool>(type: "boolean", nullable: false),
                    IsRecurring = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    GlAccountCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_Deductions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Designations",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Area = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "National"),
                    SalaryGrade = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    MinimumSalary = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    MaximumSalary = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsManagerial = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TenantId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(1024)", nullable: false),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", maxLength: 256, nullable: false),
                    CreatedByUserName = table.Column<string>(type: "VARCHAR(64)", nullable: true),
                    LastModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", maxLength: 256, nullable: true),
                    LastModifiedByUserName = table.Column<string>(type: "VARCHAR(64)", nullable: true),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedByUserName = table.Column<string>(type: "VARCHAR(64)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTemplates",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DocumentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TemplateContent = table.Column<string>(type: "text", nullable: false),
                    TemplateVariables = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_DocumentTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Holidays",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HolidayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HolidayDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsPaid = table.Column<bool>(type: "boolean", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    PayRateMultiplier = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    IsRecurringAnnually = table.Column<bool>(type: "boolean", nullable: false),
                    RecurringMonthDay = table.Column<int>(type: "integer", nullable: true),
                    RecurringMonth = table.Column<int>(type: "integer", nullable: true),
                    IsMoveable = table.Column<bool>(type: "boolean", nullable: false),
                    MoveableRule = table.Column<string>(type: "text", nullable: true),
                    IsNationwide = table.Column<bool>(type: "boolean", nullable: false),
                    ApplicableRegions = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveReport",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FromDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ToDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GeneratedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    TotalEmployees = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    TotalLeaveTypes = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    TotalLeaveRequests = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ApprovedLeaveCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    PendingLeaveCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    RejectedLeaveCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    TotalLeaveConsumed = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false, defaultValue: 0m),
                    AverageLeavePerEmployee = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false, defaultValue: 0m),
                    HighestLeaveType = table.Column<int>(type: "integer", nullable: false),
                    ReportData = table.Column<string>(type: "jsonb", nullable: true),
                    ExportPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_LeaveReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveTypes",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LeaveName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AnnualAllowance = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    AccrualFrequency = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsPaid = table.Column<bool>(type: "boolean", nullable: false),
                    MaxCarryoverDays = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    CarryoverExpiryMonths = table.Column<int>(type: "integer", nullable: true),
                    RequiresApproval = table.Column<bool>(type: "boolean", nullable: false),
                    MinimumNoticeDay = table.Column<int>(type: "integer", nullable: true),
                    LeaveCode = table.Column<string>(type: "text", nullable: true),
                    ApplicableGender = table.Column<string>(type: "text", nullable: false),
                    MinimumServiceDays = table.Column<int>(type: "integer", nullable: false),
                    RequiresMedicalCertification = table.Column<bool>(type: "boolean", nullable: false),
                    MedicalCertificateAfterDays = table.Column<int>(type: "integer", nullable: false),
                    IsConvertibleToCash = table.Column<bool>(type: "boolean", nullable: false),
                    IsCumulative = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_LeaveTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationalUnits",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    HierarchyPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CostCenter = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_OrganizationalUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationalUnits_OrganizationalUnits_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "humanresources",
                        principalTable: "OrganizationalUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayComponents",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ComponentName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ComponentType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CalculationMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CalculationFormula = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Rate = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    FixedAmount = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: true),
                    MinValue = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: true),
                    MaxValue = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: true),
                    GlAccountCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsCalculated = table.Column<bool>(type: "boolean", nullable: false),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    IsSubjectToTax = table.Column<bool>(type: "boolean", nullable: false),
                    IsTaxExempt = table.Column<bool>(type: "boolean", nullable: false),
                    LaborLawReference = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    AffectsGrossPay = table.Column<bool>(type: "boolean", nullable: false),
                    AffectsNetPay = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_PayComponents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayrollReport",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FromDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ToDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GeneratedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    PayrollPeriod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TotalEmployees = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    TotalPayrollRuns = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    TotalGrossPay = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    TotalNetPay = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    TotalDeductions = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    TotalTaxes = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    TotalBenefits = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    AverageGrossPerEmployee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    AverageNetPerEmployee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    ReportData = table.Column<string>(type: "jsonb", nullable: true),
                    ExportPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_PayrollReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payrolls",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PayFrequency = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TotalGrossPay = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalTaxes = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalDeductions = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalNetPay = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    EmployeeCount = table.Column<int>(type: "integer", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PostedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PaidDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    JournalEntryId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsLocked = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_Payrolls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ShiftName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    IsOvernight = table.Column<bool>(type: "boolean", nullable: false),
                    BreakDurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    WorkingHours = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxBrackets",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaxType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    MinIncome = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    MaxIncome = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Rate = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    FilingStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_TaxBrackets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxMaster",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TaxType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Rate = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    IsCompound = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Jurisdiction = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TaxCollectedAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaxPaidAccountId = table.Column<Guid>(type: "uuid", nullable: true),
                    TaxAuthority = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TaxRegistrationNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ReportingCategory = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_TaxMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneratedDocuments",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    GeneratedContent = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    GeneratedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinalizedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SignedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SignedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SignatureMetadata = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FilePath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_GeneratedDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneratedDocuments_DocumentTemplates_DocumentTemplateId",
                        column: x => x.DocumentTemplateId,
                        principalSchema: "humanresources",
                        principalTable: "DocumentTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OrganizationalUnitId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    HireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    CivilStatus = table.Column<string>(type: "text", nullable: true),
                    Tin = table.Column<string>(type: "text", nullable: true),
                    SssNumber = table.Column<string>(type: "text", nullable: true),
                    PhilHealthNumber = table.Column<string>(type: "text", nullable: true),
                    PagIbigNumber = table.Column<string>(type: "text", nullable: true),
                    EmploymentClassification = table.Column<string>(type: "text", nullable: false),
                    RegularizationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BasicMonthlySalary = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    TerminationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TerminationReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TerminationMode = table.Column<string>(type: "text", nullable: true),
                    SeparationPayBasis = table.Column<string>(type: "text", nullable: true),
                    SeparationPayAmount = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    IsPwd = table.Column<bool>(type: "boolean", nullable: false),
                    PwdIdNumber = table.Column<string>(type: "text", nullable: true),
                    IsSoloParent = table.Column<bool>(type: "boolean", nullable: false),
                    SoloParentIdNumber = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_OrganizationalUnits_OrganizationalUnitId",
                        column: x => x.OrganizationalUnitId,
                        principalSchema: "humanresources",
                        principalTable: "OrganizationalUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayComponentRates",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PayComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    MinAmount = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: false),
                    MaxAmount = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: false),
                    EmployeeRate = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    EmployerRate = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    AdditionalEmployerRate = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    EmployeeAmount = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: true),
                    EmployerAmount = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: true),
                    TaxRate = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    BaseAmount = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: true),
                    ExcessRate = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    EffectiveStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EffectiveEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_PayComponentRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayComponentRates_PayComponents_PayComponentId",
                        column: x => x.PayComponentId,
                        principalSchema: "humanresources",
                        principalTable: "PayComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShiftBreaks",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ShiftId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    BreakType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsPaid = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_ShiftBreaks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftBreaks_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalSchema: "humanresources",
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendance",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClockInTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    ClockOutTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    ClockInLocation = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ClockOutLocation = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    HoursWorked = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MinutesLate = table.Column<int>(type: "integer", nullable: true),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    ManagerComment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_Attendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendance_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankAccount",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountNumber = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Last4Digits = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    RoutingNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BankName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AccountType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AccountHolderName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    VerificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SwiftCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Iban = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CurrencyCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_BankAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccount_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BenefitEnrollments",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    BenefitId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CoverageLevel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EmployeeContributionAmount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    EmployerContributionAmount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CoveredDependentIds = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_BenefitEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BenefitEnrollments_Benefits_BenefitId",
                        column: x => x.BenefitId,
                        principalSchema: "humanresources",
                        principalTable: "Benefits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BenefitEnrollments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DesignationAssignments",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    DesignationId = table.Column<Guid>(type: "uuid", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsPlantilla = table.Column<bool>(type: "boolean", nullable: false),
                    IsActingAs = table.Column<bool>(type: "boolean", nullable: false),
                    AdjustedSalary = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_DesignationAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesignationAssignments_Designations_DesignationId",
                        column: x => x.DesignationId,
                        principalSchema: "humanresources",
                        principalTable: "Designations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DesignationAssignments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeContacts",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    LastName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ContactType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Relationship = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
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
                    table.PrimaryKey("PK_EmployeeContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeContacts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDependents",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    LastName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    DependentType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Relationship = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Ssn = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsBeneficiary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsClaimableDependent = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    EligibilityEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_EmployeeDependents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeDependents_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDocument",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FileName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    FilePath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IssueNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UploadedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_EmployeeDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeDocument_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEducations",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    EducationLevel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FieldOfStudy = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Institution = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    GraduationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Degree = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Gpa = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: true),
                    CertificateNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CertificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    VerificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_EmployeeEducations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeEducations_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePayComponents",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    PayComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignmentType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomRate = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    FixedAmount = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: true),
                    CustomFormula = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EffectiveStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EffectiveEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsRecurring = table.Column<bool>(type: "boolean", nullable: false),
                    IsOneTime = table.Column<bool>(type: "boolean", nullable: false),
                    OneTimeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InstallmentCount = table.Column<int>(type: "integer", nullable: true),
                    CurrentInstallment = table.Column<int>(type: "integer", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: true),
                    RemainingBalance = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: true),
                    ReferenceNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_EmployeePayComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeePayComponents_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeePayComponents_PayComponents_PayComponentId",
                        column: x => x.PayComponentId,
                        principalSchema: "humanresources",
                        principalTable: "PayComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveBalances",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    LeaveTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    OpeningBalance = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    AccruedDays = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    CarriedOverDays = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    TakenDays = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    PendingDays = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    CarryoverExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_LeaveBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveBalances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaveBalances_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalSchema: "humanresources",
                        principalTable: "LeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    LeaveTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NumberOfDays = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ApproverManagerId = table.Column<Guid>(type: "uuid", nullable: true),
                    SubmittedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReviewedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApproverComment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    AttachmentPath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_LeaveRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalSchema: "humanresources",
                        principalTable: "LeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollDeduction",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PayComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeductionType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DeductionAmount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    DeductionPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsAuthorized = table.Column<bool>(type: "boolean", nullable: false),
                    IsRecoverable = table.Column<bool>(type: "boolean", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MaxDeductionLimit = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrganizationalUnitId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_PayrollDeduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollDeduction_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollDeduction_OrganizationalUnits_OrganizationalUnitId",
                        column: x => x.OrganizationalUnitId,
                        principalSchema: "humanresources",
                        principalTable: "OrganizationalUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollDeduction_PayComponents_PayComponentId",
                        column: x => x.PayComponentId,
                        principalSchema: "humanresources",
                        principalTable: "PayComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollLines",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PayrollId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    RegularHours = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    OvertimeHours = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    RegularPay = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    OvertimePay = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    BonusPay = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    OtherEarnings = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    GrossPay = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    IncomeTax = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    SocialSecurityTax = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    MedicareTax = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    OtherTaxes = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    TotalTaxes = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    HealthInsurance = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    RetirementContribution = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    OtherDeductions = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    TotalDeductions = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    NetPay = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BankAccountLast4 = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    CheckNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
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
                    table.PrimaryKey("PK_PayrollLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollLines_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollLines_Payrolls_PayrollId",
                        column: x => x.PayrollId,
                        principalSchema: "humanresources",
                        principalTable: "Payrolls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerformanceReview",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewPeriodStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReviewPeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReviewType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OverallRating = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: false),
                    Strengths = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    AreasForImprovement = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Goals = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ReviewerComments = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    EmployeeComments = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    SubmittedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AcknowledgedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_PerformanceReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerformanceReview_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerformanceReview_Employees_ReviewerId",
                        column: x => x.ReviewerId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShiftAssignments",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShiftId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsRecurring = table.Column<bool>(type: "boolean", nullable: false),
                    RecurringDayOfWeek = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_ShiftAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftAssignments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShiftAssignments_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalSchema: "humanresources",
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Timesheets",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RegularHours = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    OvertimeHours = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    TotalHours = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ApproverManagerId = table.Column<Guid>(type: "uuid", nullable: true),
                    SubmittedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ManagerComment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsLocked = table.Column<bool>(type: "boolean", nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_Timesheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timesheets_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "humanresources",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BenefitAllocations",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrollmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AllocationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AllocatedAmount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    AllocationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReferenceNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    BenefitEnrollmentId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_BenefitAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BenefitAllocations_BenefitEnrollments_BenefitEnrollmentId",
                        column: x => x.BenefitEnrollmentId,
                        principalSchema: "humanresources",
                        principalTable: "BenefitEnrollments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BenefitAllocations_BenefitEnrollments_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalSchema: "humanresources",
                        principalTable: "BenefitEnrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimesheetLines",
                schema: "humanresources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TimesheetId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RegularHours = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    OvertimeHours = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    ProjectId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TaskDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsBillable = table.Column<bool>(type: "boolean", nullable: false),
                    BillingRate = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
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
                    table.PrimaryKey("PK_TimesheetLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimesheetLines_Timesheets_TimesheetId",
                        column: x => x.TimesheetId,
                        principalSchema: "humanresources",
                        principalTable: "Timesheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_AttendanceDate",
                schema: "humanresources",
                table: "Attendance",
                column: "AttendanceDate");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_EmployeeId",
                schema: "humanresources",
                table: "Attendance",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_EmployeeId_AttendanceDate",
                schema: "humanresources",
                table: "Attendance",
                columns: new[] { "EmployeeId", "AttendanceDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_IsActive",
                schema: "humanresources",
                table: "Attendance",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_IsApproved",
                schema: "humanresources",
                table: "Attendance",
                column: "IsApproved");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_Status",
                schema: "humanresources",
                table: "Attendance",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "idx_attendance_report_department_id",
                schema: "humanresources",
                table: "AttendanceReport",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "idx_attendance_report_employee_id",
                schema: "humanresources",
                table: "AttendanceReport",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "idx_attendance_report_generated_on",
                schema: "humanresources",
                table: "AttendanceReport",
                column: "GeneratedOn");

            migrationBuilder.CreateIndex(
                name: "idx_attendance_report_is_active",
                schema: "humanresources",
                table: "AttendanceReport",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "idx_attendance_report_period",
                schema: "humanresources",
                table: "AttendanceReport",
                columns: new[] { "FromDate", "ToDate" });

            migrationBuilder.CreateIndex(
                name: "idx_attendance_report_type",
                schema: "humanresources",
                table: "AttendanceReport",
                column: "ReportType");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_AccountType",
                schema: "humanresources",
                table: "BankAccount",
                column: "AccountType");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_Employee_Primary",
                schema: "humanresources",
                table: "BankAccount",
                columns: new[] { "EmployeeId", "IsPrimary" });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_EmployeeId",
                schema: "humanresources",
                table: "BankAccount",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_IsActive",
                schema: "humanresources",
                table: "BankAccount",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_IsVerified",
                schema: "humanresources",
                table: "BankAccount",
                column: "IsVerified");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_Last4Digits",
                schema: "humanresources",
                table: "BankAccount",
                column: "Last4Digits");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitAllocation_AllocationDate",
                schema: "humanresources",
                table: "BenefitAllocations",
                column: "AllocationDate");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitAllocation_Enrollment_Date",
                schema: "humanresources",
                table: "BenefitAllocations",
                columns: new[] { "EnrollmentId", "AllocationDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BenefitAllocation_EnrollmentId",
                schema: "humanresources",
                table: "BenefitAllocations",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitAllocation_ReferenceNumber",
                schema: "humanresources",
                table: "BenefitAllocations",
                column: "ReferenceNumber");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitAllocation_Status",
                schema: "humanresources",
                table: "BenefitAllocations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitAllocations_BenefitEnrollmentId",
                schema: "humanresources",
                table: "BenefitAllocations",
                column: "BenefitEnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitEnrollment_BenefitId",
                schema: "humanresources",
                table: "BenefitEnrollments",
                column: "BenefitId");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitEnrollment_Effective",
                schema: "humanresources",
                table: "BenefitEnrollments",
                columns: new[] { "EmployeeId", "BenefitId", "EffectiveDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BenefitEnrollment_EmployeeId",
                schema: "humanresources",
                table: "BenefitEnrollments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitEnrollment_IsActive",
                schema: "humanresources",
                table: "BenefitEnrollments",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Benefit_BenefitName",
                schema: "humanresources",
                table: "Benefits",
                column: "BenefitName");

            migrationBuilder.CreateIndex(
                name: "IX_Benefit_BenefitType",
                schema: "humanresources",
                table: "Benefits",
                column: "BenefitType");

            migrationBuilder.CreateIndex(
                name: "IX_Benefit_IsActive",
                schema: "humanresources",
                table: "Benefits",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Benefit_Name_Type",
                schema: "humanresources",
                table: "Benefits",
                columns: new[] { "BenefitName", "BenefitType" });

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_DeductionName",
                schema: "humanresources",
                table: "Deductions",
                column: "DeductionName");

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_DeductionType",
                schema: "humanresources",
                table: "Deductions",
                column: "DeductionType");

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_IsActive",
                schema: "humanresources",
                table: "Deductions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_Type_Active",
                schema: "humanresources",
                table: "Deductions",
                columns: new[] { "DeductionType", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_EDA_Designations",
                schema: "humanresources",
                table: "DesignationAssignments",
                columns: new[] { "DesignationId", "EffectiveDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EDA_EmployeeHistory",
                schema: "humanresources",
                table: "DesignationAssignments",
                columns: new[] { "EmployeeId", "EffectiveDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EDA_PayrollPeriod",
                schema: "humanresources",
                table: "DesignationAssignments",
                columns: new[] { "EffectiveDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EDA_PointInTime",
                schema: "humanresources",
                table: "DesignationAssignments",
                columns: new[] { "EmployeeId", "EffectiveDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Designations_Area",
                schema: "humanresources",
                table: "Designations",
                column: "Area");

            migrationBuilder.CreateIndex(
                name: "IX_Designations_Code",
                schema: "humanresources",
                table: "Designations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Designations_IsActive",
                schema: "humanresources",
                table: "Designations",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Designations_IsManagerial",
                schema: "humanresources",
                table: "Designations",
                column: "IsManagerial");

            migrationBuilder.CreateIndex(
                name: "IX_Designations_SalaryGrade",
                schema: "humanresources",
                table: "Designations",
                column: "SalaryGrade");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_DocumentType",
                schema: "humanresources",
                table: "DocumentTemplates",
                column: "DocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_IsActive",
                schema: "humanresources",
                table: "DocumentTemplates",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_TemplateName",
                schema: "humanresources",
                table: "DocumentTemplates",
                column: "TemplateName");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContacts_ContactType",
                schema: "humanresources",
                table: "EmployeeContacts",
                column: "ContactType");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContacts_EmployeeId",
                schema: "humanresources",
                table: "EmployeeContacts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContacts_EmployeeId_ContactType",
                schema: "humanresources",
                table: "EmployeeContacts",
                columns: new[] { "EmployeeId", "ContactType" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContacts_IsActive",
                schema: "humanresources",
                table: "EmployeeContacts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDependents_DependentType",
                schema: "humanresources",
                table: "EmployeeDependents",
                column: "DependentType");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDependents_EmployeeId",
                schema: "humanresources",
                table: "EmployeeDependents",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDependents_EmployeeId_DependentType",
                schema: "humanresources",
                table: "EmployeeDependents",
                columns: new[] { "EmployeeId", "DependentType" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDependents_IsActive",
                schema: "humanresources",
                table: "EmployeeDependents",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDependents_IsBeneficiary",
                schema: "humanresources",
                table: "EmployeeDependents",
                column: "IsBeneficiary");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDependents_IsClaimableDependent",
                schema: "humanresources",
                table: "EmployeeDependents",
                column: "IsClaimableDependent");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_DocumentType",
                schema: "humanresources",
                table: "EmployeeDocument",
                column: "DocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_EmployeeId",
                schema: "humanresources",
                table: "EmployeeDocument",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_EmployeeId_DocumentType",
                schema: "humanresources",
                table: "EmployeeDocument",
                columns: new[] { "EmployeeId", "DocumentType" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_ExpiryDate",
                schema: "humanresources",
                table: "EmployeeDocument",
                column: "ExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_IsActive",
                schema: "humanresources",
                table: "EmployeeDocument",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_EducationLevel",
                schema: "humanresources",
                table: "EmployeeEducations",
                column: "EducationLevel");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_EmployeeId",
                schema: "humanresources",
                table: "EmployeeEducations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_EmployeeId_EducationLevel",
                schema: "humanresources",
                table: "EmployeeEducations",
                columns: new[] { "EmployeeId", "EducationLevel" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_IsActive",
                schema: "humanresources",
                table: "EmployeeEducations",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_IsVerified",
                schema: "humanresources",
                table: "EmployeeEducations",
                column: "IsVerified");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayComponents_DateRange",
                schema: "humanresources",
                table: "EmployeePayComponents",
                columns: new[] { "EffectiveStartDate", "EffectiveEndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayComponents_Employee_Component_Active",
                schema: "humanresources",
                table: "EmployeePayComponents",
                columns: new[] { "EmployeeId", "PayComponentId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayComponents_EmployeeId",
                schema: "humanresources",
                table: "EmployeePayComponents",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayComponents_PayComponentId",
                schema: "humanresources",
                table: "EmployeePayComponents",
                column: "PayComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayComponents_ReferenceNumber",
                schema: "humanresources",
                table: "EmployeePayComponents",
                column: "ReferenceNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Email",
                schema: "humanresources",
                table: "Employees",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_EmployeeNumber",
                schema: "humanresources",
                table: "Employees",
                column: "EmployeeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_FirstName_LastName",
                schema: "humanresources",
                table: "Employees",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_IsActive",
                schema: "humanresources",
                table: "Employees",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_OrganizationalUnitId",
                schema: "humanresources",
                table: "Employees",
                column: "OrganizationalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Status",
                schema: "humanresources",
                table: "Employees",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedDocument_EntityId",
                schema: "humanresources",
                table: "GeneratedDocuments",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedDocument_EntityId_EntityType",
                schema: "humanresources",
                table: "GeneratedDocuments",
                columns: new[] { "EntityId", "EntityType" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedDocument_GeneratedDate",
                schema: "humanresources",
                table: "GeneratedDocuments",
                column: "GeneratedDate");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedDocument_IsActive",
                schema: "humanresources",
                table: "GeneratedDocuments",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedDocument_Status",
                schema: "humanresources",
                table: "GeneratedDocuments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedDocuments_DocumentTemplateId",
                schema: "humanresources",
                table: "GeneratedDocuments",
                column: "DocumentTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_HolidayDate",
                schema: "humanresources",
                table: "Holidays",
                column: "HolidayDate");

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_IsActive",
                schema: "humanresources",
                table: "Holidays",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalance_EmployeeId",
                schema: "humanresources",
                table: "LeaveBalances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalance_EmployeeId_LeaveTypeId_Year",
                schema: "humanresources",
                table: "LeaveBalances",
                columns: new[] { "EmployeeId", "LeaveTypeId", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalance_LeaveTypeId",
                schema: "humanresources",
                table: "LeaveBalances",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "idx_leave_report_department_id",
                schema: "humanresources",
                table: "LeaveReport",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "idx_leave_report_employee_id",
                schema: "humanresources",
                table: "LeaveReport",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "idx_leave_report_generated_on",
                schema: "humanresources",
                table: "LeaveReport",
                column: "GeneratedOn");

            migrationBuilder.CreateIndex(
                name: "idx_leave_report_is_active",
                schema: "humanresources",
                table: "LeaveReport",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "idx_leave_report_period",
                schema: "humanresources",
                table: "LeaveReport",
                columns: new[] { "FromDate", "ToDate" });

            migrationBuilder.CreateIndex(
                name: "idx_leave_report_type",
                schema: "humanresources",
                table: "LeaveReport",
                column: "ReportType");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_EmployeeId",
                schema: "humanresources",
                table: "LeaveRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_EmployeeId_DateRange",
                schema: "humanresources",
                table: "LeaveRequests",
                columns: new[] { "EmployeeId", "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_IsActive",
                schema: "humanresources",
                table: "LeaveRequests",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_LeaveTypeId",
                schema: "humanresources",
                table: "LeaveRequests",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_Status",
                schema: "humanresources",
                table: "LeaveRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveType_IsActive",
                schema: "humanresources",
                table: "LeaveTypes",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveType_LeaveName",
                schema: "humanresources",
                table: "LeaveTypes",
                column: "LeaveName");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_Code",
                schema: "humanresources",
                table: "OrganizationalUnits",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_HierarchyPath",
                schema: "humanresources",
                table: "OrganizationalUnits",
                column: "HierarchyPath");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_IsActive",
                schema: "humanresources",
                table: "OrganizationalUnits",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_ParentId",
                schema: "humanresources",
                table: "OrganizationalUnits",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_Type",
                schema: "humanresources",
                table: "OrganizationalUnits",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_PayComponentRates_Component_Year_Range",
                schema: "humanresources",
                table: "PayComponentRates",
                columns: new[] { "PayComponentId", "Year", "MinAmount", "MaxAmount" });

            migrationBuilder.CreateIndex(
                name: "IX_PayComponentRates_IsActive",
                schema: "humanresources",
                table: "PayComponentRates",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PayComponentRates_Year",
                schema: "humanresources",
                table: "PayComponentRates",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_PayComponents_Code",
                schema: "humanresources",
                table: "PayComponents",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayComponents_ComponentType",
                schema: "humanresources",
                table: "PayComponents",
                column: "ComponentType");

            migrationBuilder.CreateIndex(
                name: "IX_PayComponents_IsActive",
                schema: "humanresources",
                table: "PayComponents",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PayComponents_IsMandatory",
                schema: "humanresources",
                table: "PayComponents",
                column: "IsMandatory");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_DateRange",
                schema: "humanresources",
                table: "PayrollDeduction",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_EmployeeId",
                schema: "humanresources",
                table: "PayrollDeduction",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_IsActive",
                schema: "humanresources",
                table: "PayrollDeduction",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_OrganizationalUnitId",
                schema: "humanresources",
                table: "PayrollDeduction",
                column: "OrganizationalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_PayComponentId",
                schema: "humanresources",
                table: "PayrollDeduction",
                column: "PayComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_ReferenceNumber",
                schema: "humanresources",
                table: "PayrollDeduction",
                column: "ReferenceNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLine_EmployeeId",
                schema: "humanresources",
                table: "PayrollLines",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLine_PayrollId",
                schema: "humanresources",
                table: "PayrollLines",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLine_PayrollId_EmployeeId",
                schema: "humanresources",
                table: "PayrollLines",
                columns: new[] { "PayrollId", "EmployeeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_payroll_report_date_range",
                schema: "humanresources",
                table: "PayrollReport",
                columns: new[] { "FromDate", "ToDate" });

            migrationBuilder.CreateIndex(
                name: "idx_payroll_report_department_id",
                schema: "humanresources",
                table: "PayrollReport",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "idx_payroll_report_employee_id",
                schema: "humanresources",
                table: "PayrollReport",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "idx_payroll_report_generated_on",
                schema: "humanresources",
                table: "PayrollReport",
                column: "GeneratedOn");

            migrationBuilder.CreateIndex(
                name: "idx_payroll_report_is_active",
                schema: "humanresources",
                table: "PayrollReport",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "idx_payroll_report_period",
                schema: "humanresources",
                table: "PayrollReport",
                column: "PayrollPeriod");

            migrationBuilder.CreateIndex(
                name: "idx_payroll_report_type",
                schema: "humanresources",
                table: "PayrollReport",
                column: "ReportType");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_DateRange",
                schema: "humanresources",
                table: "Payrolls",
                columns: new[] { "StartDate", "EndDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_IsLocked",
                schema: "humanresources",
                table: "Payrolls",
                column: "IsLocked");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_StartDate",
                schema: "humanresources",
                table: "Payrolls",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_Status",
                schema: "humanresources",
                table: "Payrolls",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReview_Employee_Period",
                schema: "humanresources",
                table: "PerformanceReview",
                columns: new[] { "EmployeeId", "ReviewPeriodStart", "ReviewPeriodEnd" });

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReview_EmployeeId",
                schema: "humanresources",
                table: "PerformanceReview",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReview_Reviewer_Period",
                schema: "humanresources",
                table: "PerformanceReview",
                columns: new[] { "ReviewerId", "ReviewPeriodStart", "ReviewPeriodEnd" });

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReview_ReviewerId",
                schema: "humanresources",
                table: "PerformanceReview",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReview_Status",
                schema: "humanresources",
                table: "PerformanceReview",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignment_EmployeeId",
                schema: "humanresources",
                table: "ShiftAssignments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignment_EmployeeId_Period",
                schema: "humanresources",
                table: "ShiftAssignments",
                columns: new[] { "EmployeeId", "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignment_IsActive",
                schema: "humanresources",
                table: "ShiftAssignments",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignment_ShiftId",
                schema: "humanresources",
                table: "ShiftAssignments",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignment_StartDate",
                schema: "humanresources",
                table: "ShiftAssignments",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftBreak_ShiftId",
                schema: "humanresources",
                table: "ShiftBreaks",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Shift_IsActive",
                schema: "humanresources",
                table: "Shifts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Shift_ShiftName",
                schema: "humanresources",
                table: "Shifts",
                column: "ShiftName");

            migrationBuilder.CreateIndex(
                name: "IX_TaxBracket_TaxType_Year",
                schema: "humanresources",
                table: "TaxBrackets",
                columns: new[] { "TaxType", "Year" });

            migrationBuilder.CreateIndex(
                name: "IX_TaxBracket_Type_Year_Range",
                schema: "humanresources",
                table: "TaxBrackets",
                columns: new[] { "TaxType", "Year", "MinIncome", "MaxIncome" });

            migrationBuilder.CreateIndex(
                name: "idx_tax_master_code",
                schema: "humanresources",
                table: "TaxMaster",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_tax_master_is_active",
                schema: "humanresources",
                table: "TaxMaster",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "idx_tax_master_jurisdiction",
                schema: "humanresources",
                table: "TaxMaster",
                column: "Jurisdiction");

            migrationBuilder.CreateIndex(
                name: "idx_tax_master_tax_type",
                schema: "humanresources",
                table: "TaxMaster",
                column: "TaxType");

            migrationBuilder.CreateIndex(
                name: "idx_tax_master_type_jurisdiction_date",
                schema: "humanresources",
                table: "TaxMaster",
                columns: new[] { "TaxType", "Jurisdiction", "EffectiveDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetLine_IsBillable",
                schema: "humanresources",
                table: "TimesheetLines",
                column: "IsBillable");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetLine_TimesheetId",
                schema: "humanresources",
                table: "TimesheetLines",
                column: "TimesheetId");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetLine_WorkDate",
                schema: "humanresources",
                table: "TimesheetLines",
                column: "WorkDate");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_EmployeeId",
                schema: "humanresources",
                table: "Timesheets",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_EmployeeId_Period",
                schema: "humanresources",
                table: "Timesheets",
                columns: new[] { "EmployeeId", "StartDate", "EndDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_IsApproved",
                schema: "humanresources",
                table: "Timesheets",
                column: "IsApproved");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_IsLocked",
                schema: "humanresources",
                table: "Timesheets",
                column: "IsLocked");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_StartDate",
                schema: "humanresources",
                table: "Timesheets",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_Status",
                schema: "humanresources",
                table: "Timesheets",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendance",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "AttendanceReport",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "BankAccount",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "BenefitAllocations",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "Deductions",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "DesignationAssignments",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "EmployeeContacts",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "EmployeeDependents",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "EmployeeDocument",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "EmployeeEducations",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "EmployeePayComponents",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "GeneratedDocuments",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "Holidays",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "LeaveBalances",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "LeaveReport",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "LeaveRequests",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "PayComponentRates",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "PayrollDeduction",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "PayrollLines",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "PayrollReport",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "PerformanceReview",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "ShiftAssignments",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "ShiftBreaks",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "TaxBrackets",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "TaxMaster",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "TimesheetLines",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "BenefitEnrollments",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "Designations",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "DocumentTemplates",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "LeaveTypes",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "PayComponents",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "Payrolls",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "Shifts",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "Timesheets",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "Benefits",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "humanresources");

            migrationBuilder.DropTable(
                name: "OrganizationalUnits",
                schema: "humanresources");
        }
    }
}
