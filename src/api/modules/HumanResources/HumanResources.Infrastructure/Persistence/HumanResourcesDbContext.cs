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
    public DbSet<EmployeeDocument> EmployeeDocuments { get; set; } = null!;
    public DbSet<Attendance> Attendance { get; set; } = null!;
    public DbSet<Timesheet> Timesheets { get; set; } = null!;
    public DbSet<TimesheetLine> TimesheetLines { get; set; } = null!;
    public DbSet<Shift> Shifts { get; set; } = null!;
    public DbSet<ShiftBreak> ShiftBreaks { get; set; } = null!;
    public DbSet<ShiftAssignment> ShiftAssignments { get; set; } = null!;

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

