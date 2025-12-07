using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for loan officer assignments.
/// Assigns loan officers to members and groups.
/// </summary>
internal static class LoanOfficerAssignmentSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.LoanOfficerAssignments.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        // Get loan officers
        var loanOfficers = await context.Staff
            .Where(s => s.Role == Staff.RoleLoanOfficer && s.Status == Staff.StatusActive)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!loanOfficers.Any()) return;

        var members = await context.Members.Where(m => m.IsActive).ToListAsync(cancellationToken).ConfigureAwait(false);
        var groups = await context.MemberGroups.Where(g => g.Status == MemberGroup.StatusActive).ToListAsync(cancellationToken).ConfigureAwait(false);

        var random = new Random(42);
        int assignmentCount = 0;

        // Assign members to loan officers (distribute evenly)
        for (int i = 0; i < members.Count; i++)
        {
            var member = members[i];
            var officer = loanOfficers[i % loanOfficers.Count];

            var assignment = LoanOfficerAssignment.AssignToMember(
                staffId: officer.Id,
                memberId: member.Id,
                assignmentDate: DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-random.Next(1, 12))),
                reason: "Initial assignment during onboarding");

            await context.LoanOfficerAssignments.AddAsync(assignment, cancellationToken).ConfigureAwait(false);
            assignmentCount++;
        }

        // Assign groups to loan officers
        for (int i = 0; i < groups.Count; i++)
        {
            var group = groups[i];
            var officer = loanOfficers[i % loanOfficers.Count];

            var assignment = LoanOfficerAssignment.AssignToGroup(
                staffId: officer.Id,
                memberGroupId: group.Id,
                assignmentDate: DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-random.Next(1, 6))),
                reason: "Group formation assignment");

            await context.LoanOfficerAssignments.AddAsync(assignment, cancellationToken).ConfigureAwait(false);
            assignmentCount++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} loan officer assignments (members and groups)", tenant, assignmentCount);
    }
}
