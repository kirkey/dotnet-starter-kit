using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Infrastructure.Tenant;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using Microsoft.Extensions.Options;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence;

/// <summary>
/// Database context for HumanResources module.
/// </summary>
public sealed class HumanResourcesDbContext(
    IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor,
    DbContextOptions<HumanResourcesDbContext> options,
    IPublisher publisher,
    IOptions<DatabaseOptions> settings) : FshDbContext(multiTenantContextAccessor, options, publisher, settings)
{
    public DbSet<OrganizationalUnit> OrganizationalUnits { get; set; } = null!;
    public DbSet<Designation> Designations { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<DesignationAssignment> DesignationAssignments { get; set; } = null!;
    public DbSet<EmployeeContact> EmployeeContacts { get; set; } = null!;
    public DbSet<EmployeeDependent> EmployeeDependents { get; set; } = null!;
    public DbSet<EmployeeEducation> EmployeeEducations { get; set; } = null!;
    public DbSet<Attendance> Attendance { get; set; } = null!;
    public DbSet<Timesheet> Timesheets { get; set; } = null!;
    public DbSet<TimesheetLine> TimesheetLines { get; set; } = null!;
    public DbSet<Shift> Shifts { get; set; } = null!;
    public DbSet<ShiftBreak> ShiftBreaks { get; set; } = null!;
    public DbSet<ShiftAssignment> ShiftAssignments { get; set; } = null!;
    public DbSet<LeaveType> LeaveTypes { get; set; } = null!;
    public DbSet<LeaveBalance> LeaveBalances { get; set; } = null!;
    public DbSet<LeaveRequest> LeaveRequests { get; set; } = null!;
    public DbSet<Holiday> Holidays { get; set; } = null!;
    public DbSet<Payroll> Payrolls { get; set; } = null!;
    public DbSet<PayrollLine> PayrollLines { get; set; } = null!;
    public DbSet<PayComponent> PayComponents { get; set; } = null!;
    public DbSet<PayComponentRate> PayComponentRates { get; set; } = null!;
    public DbSet<EmployeePayComponent> EmployeePayComponents { get; set; } = null!;
    public DbSet<TaxBracket> TaxBrackets { get; set; } = null!;
    public DbSet<TaxMaster> TaxMasters { get; set; } = null!;
    public DbSet<Benefit> Benefits { get; set; } = null!;
    public DbSet<BenefitAllocation> BenefitAllocations { get; set; } = null!;
    public DbSet<BenefitEnrollment> BenefitEnrollments { get; set; } = null!;
    public DbSet<Deduction> Deductions { get; set; } = null!;
    public DbSet<AttendanceReport> AttendanceReports { get; set; } = null!;
    public DbSet<LeaveReport> LeaveReports { get; set; } = null!;
    public DbSet<PayrollReport> PayrollReports { get; set; } = null!;
    public DbSet<DocumentTemplate> DocumentTemplates { get; set; } = null!;
    public DbSet<GeneratedDocument> GeneratedDocuments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HumanResourcesDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.HumanResources);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<decimal>().HavePrecision(16, 2);
    }
}

