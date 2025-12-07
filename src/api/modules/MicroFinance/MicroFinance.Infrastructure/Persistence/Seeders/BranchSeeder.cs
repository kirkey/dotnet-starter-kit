using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for branches/offices to support multi-branch operations.
/// Creates a head office and regional branches for testing.
/// </summary>
internal static class BranchSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 6;
        var existingCount = await context.Branches.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var branchData = new (string Code, string Name, string Type, string Address, string City, string State, string Country, string Phone, string Email)[]
        {
            // Head Office
            ("HQ-001", "Main Office - Makati", Branch.TypeHeadOffice, "123 Ayala Avenue", "Makati City", "Metro Manila", "Philippines", "+632-8123-4567", "headoffice@mfi.org.ph"),
            
            // Regional Branches
            ("BR-001", "Quezon City Branch", Branch.TypeBranch, "456 EDSA corner Timog", "Quezon City", "Metro Manila", "Philippines", "+632-8234-5678", "quezoncity@mfi.org.ph"),
            ("BR-002", "Cebu Branch", Branch.TypeBranch, "789 Osmena Boulevard", "Cebu City", "Cebu", "Philippines", "+632-8345-6789", "cebu@mfi.org.ph"),
            ("BR-003", "Davao Branch", Branch.TypeBranch, "321 Roxas Avenue", "Davao City", "Davao del Sur", "Philippines", "+632-8456-7890", "davao@mfi.org.ph"),
            ("BR-004", "Pampanga Branch", Branch.TypeBranch, "555 MacArthur Highway", "San Fernando", "Pampanga", "Philippines", "+632-8567-8901", "pampanga@mfi.org.ph"),
            
            // Service Center
            ("SC-001", "Nueva Ecija Service Center", Branch.TypeServiceCenter, "100 Maharlika Highway", "Cabanatuan City", "Nueva Ecija", "Philippines", "+632-8678-9012", "nuevaecija@mfi.org.ph"),
        };

        foreach (var data in branchData)
        {
            if (await context.Branches.AnyAsync(b => b.Code == data.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var branch = Branch.Create(
                code: data.Code,
                name: data.Name,
                branchType: data.Type,
                address: data.Address,
                city: data.City,
                state: data.State,
                country: data.Country,
                phone: data.Phone,
                email: data.Email,
                openingDate: DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-2)));

            await context.Branches.AddAsync(branch, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} branches (1 head office, 4 branches, 1 service center)", tenant, targetCount);
    }
}
