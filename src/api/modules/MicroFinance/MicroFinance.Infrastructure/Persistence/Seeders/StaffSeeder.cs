using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for staff members including loan officers, tellers, and managers.
/// Creates diverse staff for testing assignments and workflows.
/// </summary>
internal static class StaffSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 15;
        var existingCount = await context.Staff.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var branches = await context.Branches.ToListAsync(cancellationToken).ConfigureAwait(false);
        var mainBranch = branches.FirstOrDefault();
        
        var staffData = new (string EmpNum, string FirstName, string LastName, string Role, string Type, string JobTitle, string Email, int BranchIdx)[]
        {
            // Management
            ("EMP-001", "Fernando", "Dela Rosa", Staff.RoleExecutive, Staff.TypeFullTime, "Chief Executive Officer", "fernando.delarosa@mfi.org", 0),
            ("EMP-002", "Maricel", "Santiago", Staff.RoleBranchManager, Staff.TypeFullTime, "Branch Manager - Makati", "maricel.santiago@mfi.org", 1),
            ("EMP-003", "Miguel", "Villanueva", Staff.RoleBranchManager, Staff.TypeFullTime, "Branch Manager - Quezon City", "miguel.villanueva@mfi.org", 2),
            
            // Loan Officers
            ("EMP-004", "Esperanza", "Mendoza", Staff.RoleLoanOfficer, Staff.TypeFullTime, "Senior Loan Officer", "esperanza.mendoza@mfi.org", 1),
            ("EMP-005", "Dario", "Aquino", Staff.RoleLoanOfficer, Staff.TypeFullTime, "Loan Officer", "dario.aquino@mfi.org", 1),
            ("EMP-006", "Jennifer", "Bautista", Staff.RoleLoanOfficer, Staff.TypeFullTime, "Loan Officer", "jennifer.bautista@mfi.org", 2),
            ("EMP-007", "Roberto", "Agustin", Staff.RoleLoanOfficer, Staff.TypeFullTime, "Agricultural Loan Specialist", "roberto.agustin@mfi.org", 3),
            ("EMP-008", "Liza", "Reyes", Staff.RoleLoanOfficer, Staff.TypePartTime, "Part-Time Loan Officer", "liza.reyes@mfi.org", 4),
            
            // Tellers
            ("EMP-009", "Daniel", "Pascual", Staff.RoleTeller, Staff.TypeFullTime, "Head Teller", "daniel.pascual@mfi.org", 1),
            ("EMP-010", "Maria Clara", "Santos", Staff.RoleTeller, Staff.TypeFullTime, "Teller", "mariaclara.santos@mfi.org", 1),
            ("EMP-011", "Kevin", "Tan", Staff.RoleTeller, Staff.TypeFullTime, "Teller", "kevin.tan@mfi.org", 2),
            
            // Other Staff
            ("EMP-012", "Patricia", "Cojuangco", Staff.RoleAccountant, Staff.TypeFullTime, "Chief Accountant", "patricia.cojuangco@mfi.org", 0),
            ("EMP-013", "Christopher", "Lim", Staff.RoleCollectionOfficer, Staff.TypeFullTime, "Collection Officer", "christopher.lim@mfi.org", 1),
            ("EMP-014", "Nancy", "Sy", Staff.RoleCustomerService, Staff.TypeFullTime, "Customer Service Rep", "nancy.sy@mfi.org", 1),
            ("EMP-015", "Mark", "Go", Staff.RoleCompliance, Staff.TypeFullTime, "Compliance Officer", "mark.go@mfi.org", 0),
        };

        var hireDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-3));

        foreach (var data in staffData)
        {
            if (await context.Staff.AnyAsync(s => s.EmployeeNumber == data.EmpNum, cancellationToken).ConfigureAwait(false))
                continue;

            var branch = branches.Count > data.BranchIdx ? branches[data.BranchIdx] : mainBranch;

            var staff = Staff.Create(
                employeeNumber: data.EmpNum,
                firstName: data.FirstName,
                lastName: data.LastName,
                email: data.Email,
                jobTitle: data.JobTitle,
                role: data.Role,
                joiningDate: hireDate.AddDays(Random.Shared.Next(0, 365)),
                employmentType: data.Type,
                branchId: branch?.Id);

            await context.Staff.AddAsync(staff, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} staff members (managers, loan officers, tellers, support)", tenant, targetCount);
    }
}
