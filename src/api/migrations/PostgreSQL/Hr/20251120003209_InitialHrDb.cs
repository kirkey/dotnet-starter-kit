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
                name: "hr");

            migrationBuilder.CreateTable(
                name: "AttendanceReport",
                schema: "hr",
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
                schema: "hr",
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
                schema: "hr",
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
                schema: "hr",
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
                schema: "hr",
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
                schema: "hr",
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
                schema: "hr",
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
                schema: "hr",
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
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "OrganizationalUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayComponents",
                schema: "hr",
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
                schema: "hr",
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
                schema: "hr",
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
                schema: "hr",
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
                schema: "hr",
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
                schema: "hr",
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
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "DocumentTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "OrganizationalUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayComponentRates",
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "PayComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShiftBreaks",
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                schema: "hr",
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
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                schema: "hr",
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
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BenefitEnrollments",
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "Benefits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BenefitEnrollments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DesignationAssignments",
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "Designations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DesignationAssignments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeContacts",
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDependents",
                schema: "hr",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    LastName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    DependentType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDocuments",
                schema: "hr",
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
                    table.PrimaryKey("PK_EmployeeDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeDocuments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEducations",
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePayComponents",
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeePayComponents_PayComponents_PayComponentId",
                        column: x => x.PayComponentId,
                        principalSchema: "hr",
                        principalTable: "PayComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveBalances",
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaveBalances_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalSchema: "hr",
                        principalTable: "LeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalSchema: "hr",
                        principalTable: "LeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollDeductions",
                schema: "hr",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PayComponentId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_PayrollDeductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollDeductions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollDeductions_OrganizationalUnits_OrganizationalUnitId",
                        column: x => x.OrganizationalUnitId,
                        principalSchema: "hr",
                        principalTable: "OrganizationalUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollDeductions_PayComponents_PayComponentId",
                        column: x => x.PayComponentId,
                        principalSchema: "hr",
                        principalTable: "PayComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollLines",
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollLines_Payrolls_PayrollId",
                        column: x => x.PayrollId,
                        principalSchema: "hr",
                        principalTable: "Payrolls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerformanceReviews",
                schema: "hr",
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
                    table.PrimaryKey("PK_PerformanceReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerformanceReviews_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerformanceReviews_Employees_ReviewerId",
                        column: x => x.ReviewerId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShiftAssignments",
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShiftAssignments_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalSchema: "hr",
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Timesheets",
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BenefitAllocations",
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "BenefitEnrollments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BenefitAllocations_BenefitEnrollments_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalSchema: "hr",
                        principalTable: "BenefitEnrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimesheetLines",
                schema: "hr",
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
                        principalSchema: "hr",
                        principalTable: "Timesheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_attendance_report_department_id",
                schema: "hr",
                table: "AttendanceReport",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "idx_attendance_report_employee_id",
                schema: "hr",
                table: "AttendanceReport",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "idx_attendance_report_generated_on",
                schema: "hr",
                table: "AttendanceReport",
                column: "GeneratedOn");

            migrationBuilder.CreateIndex(
                name: "idx_attendance_report_is_active",
                schema: "hr",
                table: "AttendanceReport",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "idx_attendance_report_period",
                schema: "hr",
                table: "AttendanceReport",
                columns: new[] { "FromDate", "ToDate" });

            migrationBuilder.CreateIndex(
                name: "idx_attendance_report_type",
                schema: "hr",
                table: "AttendanceReport",
                column: "ReportType");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_Approval_Workflow",
                schema: "hr",
                table: "Attendances",
                columns: new[] { "IsApproved", "AttendanceDate", "EmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_AttendanceDate",
                schema: "hr",
                table: "Attendances",
                column: "AttendanceDate");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_Date_Active",
                schema: "hr",
                table: "Attendances",
                columns: new[] { "AttendanceDate", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_EmployeeId",
                schema: "hr",
                table: "Attendances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_EmployeeId_AttendanceDate",
                schema: "hr",
                table: "Attendances",
                columns: new[] { "EmployeeId", "AttendanceDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_EmployeeId_Date_Active",
                schema: "hr",
                table: "Attendances",
                columns: new[] { "EmployeeId", "AttendanceDate", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_IsActive",
                schema: "hr",
                table: "Attendances",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_IsApproved",
                schema: "hr",
                table: "Attendances",
                column: "IsApproved");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_Status",
                schema: "hr",
                table: "Attendances",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_Status_Date_Active",
                schema: "hr",
                table: "Attendances",
                columns: new[] { "Status", "AttendanceDate", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_AccountType",
                schema: "hr",
                table: "BankAccounts",
                column: "AccountType");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_Employee_Primary",
                schema: "hr",
                table: "BankAccounts",
                columns: new[] { "EmployeeId", "IsPrimary" });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_EmployeeId",
                schema: "hr",
                table: "BankAccounts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_IsActive",
                schema: "hr",
                table: "BankAccounts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_IsVerified",
                schema: "hr",
                table: "BankAccounts",
                column: "IsVerified");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_Last4Digits",
                schema: "hr",
                table: "BankAccounts",
                column: "Last4Digits");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitAllocation_AllocationDate",
                schema: "hr",
                table: "BenefitAllocations",
                column: "AllocationDate");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitAllocation_DateRange_Status",
                schema: "hr",
                table: "BenefitAllocations",
                columns: new[] { "AllocationDate", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_BenefitAllocation_Enrollment_Date",
                schema: "hr",
                table: "BenefitAllocations",
                columns: new[] { "EnrollmentId", "AllocationDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BenefitAllocation_EnrollmentId",
                schema: "hr",
                table: "BenefitAllocations",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitAllocation_Reference_Status",
                schema: "hr",
                table: "BenefitAllocations",
                columns: new[] { "ReferenceNumber", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_BenefitAllocation_ReferenceNumber",
                schema: "hr",
                table: "BenefitAllocations",
                column: "ReferenceNumber");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitAllocation_Status",
                schema: "hr",
                table: "BenefitAllocations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitAllocation_Status_Type",
                schema: "hr",
                table: "BenefitAllocations",
                columns: new[] { "Status", "AllocationType" });

            migrationBuilder.CreateIndex(
                name: "IX_BenefitAllocations_BenefitEnrollmentId",
                schema: "hr",
                table: "BenefitAllocations",
                column: "BenefitEnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitEnrollment_Active_Effective",
                schema: "hr",
                table: "BenefitEnrollments",
                columns: new[] { "IsActive", "EffectiveDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BenefitEnrollment_BenefitActive",
                schema: "hr",
                table: "BenefitEnrollments",
                columns: new[] { "BenefitId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_BenefitEnrollment_BenefitId",
                schema: "hr",
                table: "BenefitEnrollments",
                column: "BenefitId");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitEnrollment_Effective",
                schema: "hr",
                table: "BenefitEnrollments",
                columns: new[] { "EmployeeId", "BenefitId", "EffectiveDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BenefitEnrollment_EmployeeActive",
                schema: "hr",
                table: "BenefitEnrollments",
                columns: new[] { "EmployeeId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_BenefitEnrollment_EmployeeId",
                schema: "hr",
                table: "BenefitEnrollments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitEnrollment_IsActive",
                schema: "hr",
                table: "BenefitEnrollments",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitEnrollment_Period_Active",
                schema: "hr",
                table: "BenefitEnrollments",
                columns: new[] { "EmployeeId", "EffectiveDate", "EndDate", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Benefit_BenefitName",
                schema: "hr",
                table: "Benefits",
                column: "BenefitName");

            migrationBuilder.CreateIndex(
                name: "IX_Benefit_BenefitType",
                schema: "hr",
                table: "Benefits",
                column: "BenefitType");

            migrationBuilder.CreateIndex(
                name: "IX_Benefit_IsActive",
                schema: "hr",
                table: "Benefits",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Benefit_Name_Type",
                schema: "hr",
                table: "Benefits",
                columns: new[] { "BenefitName", "BenefitType" });

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_Active_Type",
                schema: "hr",
                table: "Deductions",
                columns: new[] { "IsActive", "DeductionType" });

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_DeductionName",
                schema: "hr",
                table: "Deductions",
                column: "DeductionName");

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_DeductionType",
                schema: "hr",
                table: "Deductions",
                column: "DeductionType");

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_GlAccount_Type_Active",
                schema: "hr",
                table: "Deductions",
                columns: new[] { "GlAccountCode", "DeductionType", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_IsActive",
                schema: "hr",
                table: "Deductions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_RecoveryMethod_Active",
                schema: "hr",
                table: "Deductions",
                columns: new[] { "RecoveryMethod", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_Type_Active",
                schema: "hr",
                table: "Deductions",
                columns: new[] { "DeductionType", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_EDA_Designations",
                schema: "hr",
                table: "DesignationAssignments",
                columns: new[] { "DesignationId", "EffectiveDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EDA_EmployeeActive",
                schema: "hr",
                table: "DesignationAssignments",
                columns: new[] { "EmployeeId", "EffectiveDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EDA_PayrollLookup",
                schema: "hr",
                table: "DesignationAssignments",
                columns: new[] { "EffectiveDate", "EndDate", "EmployeeId", "DesignationId" });

            migrationBuilder.CreateIndex(
                name: "IX_EDA_PayrollPeriod",
                schema: "hr",
                table: "DesignationAssignments",
                columns: new[] { "EffectiveDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EDA_PointInTime",
                schema: "hr",
                table: "DesignationAssignments",
                columns: new[] { "EmployeeId", "EffectiveDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Designations_Area",
                schema: "hr",
                table: "Designations",
                column: "Area");

            migrationBuilder.CreateIndex(
                name: "IX_Designations_Area_Active",
                schema: "hr",
                table: "Designations",
                columns: new[] { "Area", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Designations_Code",
                schema: "hr",
                table: "Designations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Designations_Grade_Area_Active",
                schema: "hr",
                table: "Designations",
                columns: new[] { "SalaryGrade", "Area", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Designations_IsActive",
                schema: "hr",
                table: "Designations",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Designations_IsManagerial",
                schema: "hr",
                table: "Designations",
                column: "IsManagerial");

            migrationBuilder.CreateIndex(
                name: "IX_Designations_Manager_Active",
                schema: "hr",
                table: "Designations",
                columns: new[] { "IsManagerial", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Designations_SalaryGrade",
                schema: "hr",
                table: "Designations",
                column: "SalaryGrade");

            migrationBuilder.CreateIndex(
                name: "IX_Designations_Title_Active",
                schema: "hr",
                table: "Designations",
                columns: new[] { "Title", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_Active_Name",
                schema: "hr",
                table: "DocumentTemplates",
                columns: new[] { "IsActive", "TemplateName" });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_DocumentType",
                schema: "hr",
                table: "DocumentTemplates",
                column: "DocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_IsActive",
                schema: "hr",
                table: "DocumentTemplates",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_TemplateName",
                schema: "hr",
                table: "DocumentTemplates",
                column: "TemplateName");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplate_Type_Active",
                schema: "hr",
                table: "DocumentTemplates",
                columns: new[] { "DocumentType", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContacts_ContactType",
                schema: "hr",
                table: "EmployeeContacts",
                column: "ContactType");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContacts_Email_Active",
                schema: "hr",
                table: "EmployeeContacts",
                columns: new[] { "Email", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContacts_EmployeeId",
                schema: "hr",
                table: "EmployeeContacts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContacts_EmployeeId_ContactType",
                schema: "hr",
                table: "EmployeeContacts",
                columns: new[] { "EmployeeId", "ContactType" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContacts_IsActive",
                schema: "hr",
                table: "EmployeeContacts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContacts_Phone_Active",
                schema: "hr",
                table: "EmployeeContacts",
                columns: new[] { "PhoneNumber", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContacts_Priority_Active",
                schema: "hr",
                table: "EmployeeContacts",
                columns: new[] { "Priority", "EmployeeId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeContacts_Type_Active",
                schema: "hr",
                table: "EmployeeContacts",
                columns: new[] { "ContactType", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDependents_Beneficiary_Active",
                schema: "hr",
                table: "EmployeeDependents",
                columns: new[] { "IsBeneficiary", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDependents_Claimable_Active",
                schema: "hr",
                table: "EmployeeDependents",
                columns: new[] { "IsClaimableDependent", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDependents_DependentType",
                schema: "hr",
                table: "EmployeeDependents",
                column: "DependentType");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDependents_Employee_Type_Active",
                schema: "hr",
                table: "EmployeeDependents",
                columns: new[] { "EmployeeId", "DependentType", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDependents_EmployeeId",
                schema: "hr",
                table: "EmployeeDependents",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDependents_EmployeeId_DependentType",
                schema: "hr",
                table: "EmployeeDependents",
                columns: new[] { "EmployeeId", "DependentType" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDependents_IsActive",
                schema: "hr",
                table: "EmployeeDependents",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDependents_IsBeneficiary",
                schema: "hr",
                table: "EmployeeDependents",
                column: "IsBeneficiary");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDependents_IsClaimableDependent",
                schema: "hr",
                table: "EmployeeDependents",
                column: "IsClaimableDependent");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_DocumentType",
                schema: "hr",
                table: "EmployeeDocuments",
                column: "DocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_EmployeeId",
                schema: "hr",
                table: "EmployeeDocuments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_EmployeeId_DocumentType",
                schema: "hr",
                table: "EmployeeDocuments",
                columns: new[] { "EmployeeId", "DocumentType" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_ExpiryDate",
                schema: "hr",
                table: "EmployeeDocuments",
                column: "ExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_IsActive",
                schema: "hr",
                table: "EmployeeDocuments",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_Certificate_Active",
                schema: "hr",
                table: "EmployeeEducations",
                columns: new[] { "CertificateNumber", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_EducationLevel",
                schema: "hr",
                table: "EmployeeEducations",
                column: "EducationLevel");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_EmployeeId",
                schema: "hr",
                table: "EmployeeEducations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_EmployeeId_EducationLevel",
                schema: "hr",
                table: "EmployeeEducations",
                columns: new[] { "EmployeeId", "EducationLevel" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_IsActive",
                schema: "hr",
                table: "EmployeeEducations",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_IsVerified",
                schema: "hr",
                table: "EmployeeEducations",
                column: "IsVerified");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_Level_Active",
                schema: "hr",
                table: "EmployeeEducations",
                columns: new[] { "EducationLevel", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_Summary",
                schema: "hr",
                table: "EmployeeEducations",
                columns: new[] { "EmployeeId", "EducationLevel", "IsVerified" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_Verified_Active",
                schema: "hr",
                table: "EmployeeEducations",
                columns: new[] { "IsVerified", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayComponents_DateRange",
                schema: "hr",
                table: "EmployeePayComponents",
                columns: new[] { "EffectiveStartDate", "EffectiveEndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayComponents_Employee_Component_Active",
                schema: "hr",
                table: "EmployeePayComponents",
                columns: new[] { "EmployeeId", "PayComponentId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayComponents_EmployeeId",
                schema: "hr",
                table: "EmployeePayComponents",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayComponents_PayComponentId",
                schema: "hr",
                table: "EmployeePayComponents",
                column: "PayComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayComponents_ReferenceNumber",
                schema: "hr",
                table: "EmployeePayComponents",
                column: "ReferenceNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Email",
                schema: "hr",
                table: "Employees",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Email_Active",
                schema: "hr",
                table: "Employees",
                columns: new[] { "Email", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_EmployeeNumber",
                schema: "hr",
                table: "Employees",
                column: "EmployeeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_FirstName_LastName",
                schema: "hr",
                table: "Employees",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_IsActive",
                schema: "hr",
                table: "Employees",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_LastName_FirstName_Active",
                schema: "hr",
                table: "Employees",
                columns: new[] { "LastName", "FirstName", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_OrganizationalUnitId",
                schema: "hr",
                table: "Employees",
                column: "OrganizationalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_OrgUnit_Status_Active",
                schema: "hr",
                table: "Employees",
                columns: new[] { "OrganizationalUnitId", "Status", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Status",
                schema: "hr",
                table: "Employees",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Status_Active",
                schema: "hr",
                table: "Employees",
                columns: new[] { "Status", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedDocument_Entity_Date",
                schema: "hr",
                table: "GeneratedDocuments",
                columns: new[] { "EntityId", "EntityType", "GeneratedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedDocument_EntityId",
                schema: "hr",
                table: "GeneratedDocuments",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedDocument_EntityId_EntityType",
                schema: "hr",
                table: "GeneratedDocuments",
                columns: new[] { "EntityId", "EntityType" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedDocument_GeneratedDate",
                schema: "hr",
                table: "GeneratedDocuments",
                column: "GeneratedDate");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedDocument_IsActive",
                schema: "hr",
                table: "GeneratedDocuments",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedDocument_SignedBy_Date",
                schema: "hr",
                table: "GeneratedDocuments",
                columns: new[] { "SignedBy", "GeneratedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedDocument_Status",
                schema: "hr",
                table: "GeneratedDocuments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedDocument_Status_Active",
                schema: "hr",
                table: "GeneratedDocuments",
                columns: new[] { "Status", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedDocument_Status_Date",
                schema: "hr",
                table: "GeneratedDocuments",
                columns: new[] { "Status", "GeneratedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedDocuments_DocumentTemplateId",
                schema: "hr",
                table: "GeneratedDocuments",
                column: "DocumentTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_Date_Active",
                schema: "hr",
                table: "Holidays",
                columns: new[] { "HolidayDate", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_HolidayDate",
                schema: "hr",
                table: "Holidays",
                column: "HolidayDate");

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_IsActive",
                schema: "hr",
                table: "Holidays",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalance_EmployeeId",
                schema: "hr",
                table: "LeaveBalances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalance_EmployeeId_LeaveTypeId_Year",
                schema: "hr",
                table: "LeaveBalances",
                columns: new[] { "EmployeeId", "LeaveTypeId", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalance_EmployeeYear",
                schema: "hr",
                table: "LeaveBalances",
                columns: new[] { "EmployeeId", "Year" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalance_LeaveType_Year",
                schema: "hr",
                table: "LeaveBalances",
                columns: new[] { "LeaveTypeId", "Year" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalance_LeaveTypeId",
                schema: "hr",
                table: "LeaveBalances",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalance_Period_Query",
                schema: "hr",
                table: "LeaveBalances",
                columns: new[] { "EmployeeId", "Year", "LeaveTypeId" });

            migrationBuilder.CreateIndex(
                name: "idx_leave_report_department_id",
                schema: "hr",
                table: "LeaveReport",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "idx_leave_report_employee_id",
                schema: "hr",
                table: "LeaveReport",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "idx_leave_report_generated_on",
                schema: "hr",
                table: "LeaveReport",
                column: "GeneratedOn");

            migrationBuilder.CreateIndex(
                name: "idx_leave_report_is_active",
                schema: "hr",
                table: "LeaveReport",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "idx_leave_report_period",
                schema: "hr",
                table: "LeaveReport",
                columns: new[] { "FromDate", "ToDate" });

            migrationBuilder.CreateIndex(
                name: "idx_leave_report_type",
                schema: "hr",
                table: "LeaveReport",
                column: "ReportType");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_EmployeeHistory",
                schema: "hr",
                table: "LeaveRequests",
                columns: new[] { "EmployeeId", "StartDate", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_EmployeeId",
                schema: "hr",
                table: "LeaveRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_EmployeeId_DateRange",
                schema: "hr",
                table: "LeaveRequests",
                columns: new[] { "EmployeeId", "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_IsActive",
                schema: "hr",
                table: "LeaveRequests",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_LeaveTypeId",
                schema: "hr",
                table: "LeaveRequests",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_Overlap_Detection",
                schema: "hr",
                table: "LeaveRequests",
                columns: new[] { "EmployeeId", "StartDate", "EndDate", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_Pending_Queue",
                schema: "hr",
                table: "LeaveRequests",
                columns: new[] { "Status", "StartDate", "EmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_Status",
                schema: "hr",
                table: "LeaveRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_Workflow",
                schema: "hr",
                table: "LeaveRequests",
                columns: new[] { "Status", "EmployeeId", "StartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveType_IsActive",
                schema: "hr",
                table: "LeaveTypes",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveType_LeaveName",
                schema: "hr",
                table: "LeaveTypes",
                column: "LeaveName");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_Code",
                schema: "hr",
                table: "OrganizationalUnits",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_CostCenter_Active",
                schema: "hr",
                table: "OrganizationalUnits",
                columns: new[] { "CostCenter", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_HierarchyPath",
                schema: "hr",
                table: "OrganizationalUnits",
                column: "HierarchyPath");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_IsActive",
                schema: "hr",
                table: "OrganizationalUnits",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_Location_Type_Active",
                schema: "hr",
                table: "OrganizationalUnits",
                columns: new[] { "Location", "Type", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_Parent_Active",
                schema: "hr",
                table: "OrganizationalUnits",
                columns: new[] { "ParentId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_ParentId",
                schema: "hr",
                table: "OrganizationalUnits",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_Path_Active",
                schema: "hr",
                table: "OrganizationalUnits",
                columns: new[] { "HierarchyPath", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_Type",
                schema: "hr",
                table: "OrganizationalUnits",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_Type_Active",
                schema: "hr",
                table: "OrganizationalUnits",
                columns: new[] { "Type", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_PayComponentRates_Component_Year_Range",
                schema: "hr",
                table: "PayComponentRates",
                columns: new[] { "PayComponentId", "Year", "MinAmount", "MaxAmount" });

            migrationBuilder.CreateIndex(
                name: "IX_PayComponentRates_IsActive",
                schema: "hr",
                table: "PayComponentRates",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PayComponentRates_Year",
                schema: "hr",
                table: "PayComponentRates",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_PayComponent_Calculation_Active",
                schema: "hr",
                table: "PayComponents",
                columns: new[] { "CalculationMethod", "ComponentType", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_PayComponent_GlAccount_Active",
                schema: "hr",
                table: "PayComponents",
                columns: new[] { "GlAccountCode", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_PayComponent_Mandatory_Active",
                schema: "hr",
                table: "PayComponents",
                columns: new[] { "IsMandatory", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_PayComponent_Type_Active",
                schema: "hr",
                table: "PayComponents",
                columns: new[] { "ComponentType", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_PayComponents_Code",
                schema: "hr",
                table: "PayComponents",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayComponents_ComponentType",
                schema: "hr",
                table: "PayComponents",
                column: "ComponentType");

            migrationBuilder.CreateIndex(
                name: "IX_PayComponents_IsActive",
                schema: "hr",
                table: "PayComponents",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PayComponents_IsMandatory",
                schema: "hr",
                table: "PayComponents",
                column: "IsMandatory");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_Active_Employee",
                schema: "hr",
                table: "PayrollDeductions",
                columns: new[] { "IsActive", "EmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_Component_Active",
                schema: "hr",
                table: "PayrollDeductions",
                columns: new[] { "PayComponentId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_DateRange",
                schema: "hr",
                table: "PayrollDeductions",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_EmployeeId",
                schema: "hr",
                table: "PayrollDeductions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_IsActive",
                schema: "hr",
                table: "PayrollDeductions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_OrganizationalUnitId",
                schema: "hr",
                table: "PayrollDeductions",
                column: "OrganizationalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_OrgUnit_Active",
                schema: "hr",
                table: "PayrollDeductions",
                columns: new[] { "OrganizationalUnitId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_PayComponentId",
                schema: "hr",
                table: "PayrollDeductions",
                column: "PayComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_Period_Active",
                schema: "hr",
                table: "PayrollDeductions",
                columns: new[] { "EmployeeId", "StartDate", "EndDate", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_Reference_Active",
                schema: "hr",
                table: "PayrollDeductions",
                columns: new[] { "ReferenceNumber", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollDeduction_ReferenceNumber",
                schema: "hr",
                table: "PayrollDeductions",
                column: "ReferenceNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLine_Completion",
                schema: "hr",
                table: "PayrollLines",
                columns: new[] { "PayrollId", "EmployeeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLine_EmployeeId",
                schema: "hr",
                table: "PayrollLines",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLine_PaymentMethod",
                schema: "hr",
                table: "PayrollLines",
                column: "PaymentMethod");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollLine_PayrollId",
                schema: "hr",
                table: "PayrollLines",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "idx_payroll_report_date_range",
                schema: "hr",
                table: "PayrollReport",
                columns: new[] { "FromDate", "ToDate" });

            migrationBuilder.CreateIndex(
                name: "idx_payroll_report_department_id",
                schema: "hr",
                table: "PayrollReport",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "idx_payroll_report_employee_id",
                schema: "hr",
                table: "PayrollReport",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "idx_payroll_report_generated_on",
                schema: "hr",
                table: "PayrollReport",
                column: "GeneratedOn");

            migrationBuilder.CreateIndex(
                name: "idx_payroll_report_is_active",
                schema: "hr",
                table: "PayrollReport",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "idx_payroll_report_period",
                schema: "hr",
                table: "PayrollReport",
                column: "PayrollPeriod");

            migrationBuilder.CreateIndex(
                name: "idx_payroll_report_type",
                schema: "hr",
                table: "PayrollReport",
                column: "ReportType");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_Active_Period",
                schema: "hr",
                table: "Payrolls",
                columns: new[] { "IsLocked", "StartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_DateRange",
                schema: "hr",
                table: "Payrolls",
                columns: new[] { "StartDate", "EndDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_IsLocked",
                schema: "hr",
                table: "Payrolls",
                column: "IsLocked");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_Locked_Status",
                schema: "hr",
                table: "Payrolls",
                columns: new[] { "IsLocked", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_Period_Status",
                schema: "hr",
                table: "Payrolls",
                columns: new[] { "StartDate", "EndDate", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_StartDate",
                schema: "hr",
                table: "Payrolls",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_Status",
                schema: "hr",
                table: "Payrolls",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReview_Employee_Period",
                schema: "hr",
                table: "PerformanceReviews",
                columns: new[] { "EmployeeId", "ReviewPeriodStart", "ReviewPeriodEnd" });

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReview_EmployeeId",
                schema: "hr",
                table: "PerformanceReviews",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReview_Reviewer_Period",
                schema: "hr",
                table: "PerformanceReviews",
                columns: new[] { "ReviewerId", "ReviewPeriodStart", "ReviewPeriodEnd" });

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReview_ReviewerId",
                schema: "hr",
                table: "PerformanceReviews",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReview_Status",
                schema: "hr",
                table: "PerformanceReviews",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignment_DateRange_Active",
                schema: "hr",
                table: "ShiftAssignments",
                columns: new[] { "StartDate", "EndDate", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignment_EmployeeId",
                schema: "hr",
                table: "ShiftAssignments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignment_EmployeeId_Active",
                schema: "hr",
                table: "ShiftAssignments",
                columns: new[] { "EmployeeId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignment_EmployeeId_Period",
                schema: "hr",
                table: "ShiftAssignments",
                columns: new[] { "EmployeeId", "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignment_IsActive",
                schema: "hr",
                table: "ShiftAssignments",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignment_Period_Active",
                schema: "hr",
                table: "ShiftAssignments",
                columns: new[] { "EmployeeId", "StartDate", "EndDate", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignment_ShiftId",
                schema: "hr",
                table: "ShiftAssignments",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignment_StartDate",
                schema: "hr",
                table: "ShiftAssignments",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAssignment_Utilization",
                schema: "hr",
                table: "ShiftAssignments",
                columns: new[] { "ShiftId", "StartDate", "EndDate", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftBreak_ShiftId",
                schema: "hr",
                table: "ShiftBreaks",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Shift_Active_Name",
                schema: "hr",
                table: "Shifts",
                columns: new[] { "IsActive", "ShiftName" });

            migrationBuilder.CreateIndex(
                name: "IX_Shift_IsActive",
                schema: "hr",
                table: "Shifts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Shift_ShiftName",
                schema: "hr",
                table: "Shifts",
                column: "ShiftName");

            migrationBuilder.CreateIndex(
                name: "IX_Shift_TimeSlot_Active",
                schema: "hr",
                table: "Shifts",
                columns: new[] { "StartTime", "EndTime", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_TaxBracket_TaxType_Year",
                schema: "hr",
                table: "TaxBrackets",
                columns: new[] { "TaxType", "Year" });

            migrationBuilder.CreateIndex(
                name: "IX_TaxBracket_Type_Year_Range",
                schema: "hr",
                table: "TaxBrackets",
                columns: new[] { "TaxType", "Year", "MinIncome", "MaxIncome" });

            migrationBuilder.CreateIndex(
                name: "idx_tax_master_code",
                schema: "hr",
                table: "TaxMaster",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_tax_master_is_active",
                schema: "hr",
                table: "TaxMaster",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "idx_tax_master_jurisdiction",
                schema: "hr",
                table: "TaxMaster",
                column: "Jurisdiction");

            migrationBuilder.CreateIndex(
                name: "idx_tax_master_tax_type",
                schema: "hr",
                table: "TaxMaster",
                column: "TaxType");

            migrationBuilder.CreateIndex(
                name: "idx_tax_master_type_jurisdiction_date",
                schema: "hr",
                table: "TaxMaster",
                columns: new[] { "TaxType", "Jurisdiction", "EffectiveDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetLine_Billable_Date",
                schema: "hr",
                table: "TimesheetLines",
                columns: new[] { "IsBillable", "WorkDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetLine_Completion",
                schema: "hr",
                table: "TimesheetLines",
                columns: new[] { "TimesheetId", "WorkDate", "IsBillable" });

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetLine_IsBillable",
                schema: "hr",
                table: "TimesheetLines",
                column: "IsBillable");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetLine_Project_Tracking",
                schema: "hr",
                table: "TimesheetLines",
                columns: new[] { "ProjectId", "WorkDate", "IsBillable" });

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetLine_TimesheetId",
                schema: "hr",
                table: "TimesheetLines",
                column: "TimesheetId");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetLine_WorkDate",
                schema: "hr",
                table: "TimesheetLines",
                column: "WorkDate");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_EmployeeId",
                schema: "hr",
                table: "Timesheets",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_EmployeeId_Period",
                schema: "hr",
                table: "Timesheets",
                columns: new[] { "EmployeeId", "StartDate", "EndDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_IsApproved",
                schema: "hr",
                table: "Timesheets",
                column: "IsApproved");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_IsLocked",
                schema: "hr",
                table: "Timesheets",
                column: "IsLocked");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_Pending_Approvals",
                schema: "hr",
                table: "Timesheets",
                columns: new[] { "IsApproved", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_Period_Status",
                schema: "hr",
                table: "Timesheets",
                columns: new[] { "StartDate", "EndDate", "EmployeeId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_StartDate",
                schema: "hr",
                table: "Timesheets",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_Status",
                schema: "hr",
                table: "Timesheets",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheet_Status_Approval_Lock",
                schema: "hr",
                table: "Timesheets",
                columns: new[] { "Status", "IsApproved", "IsLocked", "EmployeeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceReport",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "Attendances",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "BankAccounts",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "BenefitAllocations",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "Deductions",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "DesignationAssignments",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "EmployeeContacts",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "EmployeeDependents",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "EmployeeDocuments",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "EmployeeEducations",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "EmployeePayComponents",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "GeneratedDocuments",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "Holidays",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "LeaveBalances",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "LeaveReport",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "LeaveRequests",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "PayComponentRates",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "PayrollDeductions",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "PayrollLines",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "PayrollReport",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "PerformanceReviews",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "ShiftAssignments",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "ShiftBreaks",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "TaxBrackets",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "TaxMaster",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "TimesheetLines",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "BenefitEnrollments",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "Designations",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "DocumentTemplates",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "LeaveTypes",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "PayComponents",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "Payrolls",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "Shifts",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "Timesheets",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "Benefits",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "OrganizationalUnits",
                schema: "hr");
        }
    }
}
