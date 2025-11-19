using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence;

/// <summary>
/// Database initializer for HumanResources module.
/// </summary>
internal sealed class HrDbInitializer(
    ILogger<HrDbInitializer> logger,
    HrDbContext context,
    ILogger<PhilippinePayrollSeeder> payrollLogger,
    ILogger<HrDemoDataSeeder> demoLogger) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for humanresources module", context.TenantInfo!.Identifier);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        // Seed default organizational units if none exist
        if (!await context.OrganizationalUnits.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            // Create default department
            var hrDepartment = OrganizationalUnit.Create(
                "HR-001",
                "Human Resources Department",
                OrganizationalUnitType.Department);
            hrDepartment.UpdateHierarchyPath("/HR-001/");

            // Create default division under department
            var recruitmentDivision = OrganizationalUnit.Create(
                "HR-REC-001",
                "Recruitment Division",
                OrganizationalUnitType.Division,
                hrDepartment.Id);
            recruitmentDivision.UpdateHierarchyPath("/HR-001/HR-REC-001/");

            // Create default section under division
            var sourcingSection = OrganizationalUnit.Create(
                "HR-REC-SRC-001",
                "Sourcing Section",
                OrganizationalUnitType.Section,
                recruitmentDivision.Id);
            sourcingSection.UpdateHierarchyPath("/HR-001/HR-REC-001/HR-REC-SRC-001/");

            await context.OrganizationalUnits.AddRangeAsync(
                new[] { hrDepartment, recruitmentDivision, sourcingSection }, 
                cancellationToken).ConfigureAwait(false);
            
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeding default organizational units", context.TenantInfo!.Identifier);

            // Seed default designations for different areas
            var supervisorDesignation = Designation.Create(
                "SUP-001",
                "Supervisor",
                "Metro Manila",
                "Supervises field operations",
                "Grade 4",
                40000,
                55000,
                true);

            var technicianDesignation = Designation.Create(
                "TECH-001",
                "Technician",
                "Metro Manila",
                "Installs and maintains equipment",
                "Grade 2",
                30000,
                42000,
                false);

            var helperDesignation = Designation.Create(
                "HELP-001",
                "Helper",
                "Metro Manila",
                "Assists technicians",
                "Grade 1",
                20000,
                28000,
                false);

            await context.Designations.AddRangeAsync(
                new[] { supervisorDesignation, technicianDesignation, helperDesignation },
                cancellationToken).ConfigureAwait(false);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeding default designations", context.TenantInfo!.Identifier);
        }

        // Seed Philippine payroll components and rates
        var payrollSeeder = new PhilippinePayrollSeeder(payrollLogger, context);
        await payrollSeeder.SeedAsync(cancellationToken).ConfigureAwait(false);

        // Seed demo/sample data for all HR entities
        var demoSeeder = new HrDemoDataSeeder(demoLogger, context);
        await demoSeeder.SeedAsync(cancellationToken).ConfigureAwait(false);
    }
}
