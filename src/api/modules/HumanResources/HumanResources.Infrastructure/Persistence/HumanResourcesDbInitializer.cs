using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence;

/// <summary>
/// Database initializer for HumanResources module.
/// </summary>
internal sealed class HumanResourcesDbInitializer(
    ILogger<HumanResourcesDbInitializer> logger,
    HumanResourcesDbContext context) : IDbInitializer
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
        // Seed default company if none exists
        const string CompanyCode = "DEFAULT";
        const string CompanyName = "Default Company";
        
        var company = await context.Companies
            .FirstOrDefaultAsync(c => c.CompanyCode == CompanyCode, cancellationToken)
            .ConfigureAwait(false);

        if (company is null)
        {
            company = Company.Create(CompanyCode, CompanyName);
            await context.Companies.AddAsync(company, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeding default company", context.TenantInfo!.Identifier);
        }

        // Seed default organizational units if none exist
        if (!await context.OrganizationalUnits.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            // Create default department
            var hrDepartment = OrganizationalUnit.Create(
                company.Id,
                "HR-001",
                "Human Resources Department",
                OrganizationalUnitType.Department);
            hrDepartment.UpdateHierarchyPath("/HR-001/");

            // Create default division under department
            var recruitmentDivision = OrganizationalUnit.Create(
                company.Id,
                "HR-REC-001",
                "Recruitment Division",
                OrganizationalUnitType.Division,
                hrDepartment.Id);
            recruitmentDivision.UpdateHierarchyPath("/HR-001/HR-REC-001/");

            // Create default section under division
            var sourcingSection = OrganizationalUnit.Create(
                company.Id,
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
        }
    }
}
