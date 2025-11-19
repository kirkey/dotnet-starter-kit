using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Infrastructure.Tenant;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using Microsoft.Extensions.Options;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence;

/// <summary>
/// Database context for HumanResources module.
/// </summary>
public sealed class HrDbContext(
    IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor,
    DbContextOptions<HrDbContext> options,
    IPublisher publisher,
    IOptions<DatabaseOptions> settings) : FshDbContext(multiTenantContextAccessor, options, publisher, settings)
{
    // ===== Core HR Management =====
    public DbSet<OrganizationalUnit> OrganizationalUnits { get; set; } = null!;
    public DbSet<Designation> Designations { get; set; } = null!;
    public DbSet<DesignationAssignment> DesignationAssignments { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<EmployeeContact> EmployeeContacts { get; set; } = null!;
    public DbSet<EmployeeDependent> EmployeeDependents { get; set; } = null!;
    public DbSet<EmployeeEducation> EmployeeEducations { get; set; } = null!;
    public DbSet<EmployeeDocument> EmployeeDocuments { get; set; } = null!;
    public DbSet<EmployeePayComponent> EmployeePayComponents { get; set; } = null!;

    // ===== Time & Attendance =====
    public DbSet<Attendance> Attendances { get; set; } = null!;
    public DbSet<Shift> Shifts { get; set; } = null!;
    public DbSet<ShiftBreak> ShiftBreaks { get; set; } = null!;
    public DbSet<ShiftAssignment> ShiftAssignments { get; set; } = null!;
    public DbSet<Timesheet> Timesheets { get; set; } = null!;
    public DbSet<TimesheetLine> TimesheetLines { get; set; } = null!;

    // ===== Leave Management =====
    public DbSet<Holiday> Holidays { get; set; } = null!;
    public DbSet<LeaveType> LeaveTypes { get; set; } = null!;
    public DbSet<LeaveBalance> LeaveBalances { get; set; } = null!;
    public DbSet<LeaveRequest> LeaveRequests { get; set; } = null!;

    // ===== Payroll & Compensation =====
    public DbSet<PayComponent> PayComponents { get; set; } = null!;
    public DbSet<PayComponentRate> PayComponentRates { get; set; } = null!;
    public DbSet<Payroll> Payrolls { get; set; } = null!;
    public DbSet<PayrollLine> PayrollLines { get; set; } = null!;
    public DbSet<TaxBracket> TaxBrackets { get; set; } = null!;
    public DbSet<TaxMaster> TaxMasters { get; set; } = null!;
    public DbSet<Deduction> Deductions { get; set; } = null!;
    public DbSet<PayrollDeduction> PayrollDeductions { get; set; } = null!;

    // ===== Benefits Management =====
    public DbSet<Benefit> Benefits { get; set; } = null!;
    public DbSet<BenefitAllocation> BenefitAllocations { get; set; } = null!;
    public DbSet<BenefitEnrollment> BenefitEnrollments { get; set; } = null!;

    // ===== Performance & HR Operations =====
    public DbSet<PerformanceReview> PerformanceReviews { get; set; } = null!;
    public DbSet<BankAccount> BankAccounts { get; set; } = null!;
    public DbSet<DocumentTemplate> DocumentTemplates { get; set; } = null!;

    // ===== Reporting & Generated Data =====
    public DbSet<AttendanceReport> AttendanceReports { get; set; } = null!;
    public DbSet<LeaveReport> LeaveReports { get; set; } = null!;
    public DbSet<PayrollReport> PayrollReports { get; set; } = null!;
    public DbSet<GeneratedDocument> GeneratedDocuments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HrDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.HumanResources);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<decimal>().HavePrecision(16, 2);
    }
}

