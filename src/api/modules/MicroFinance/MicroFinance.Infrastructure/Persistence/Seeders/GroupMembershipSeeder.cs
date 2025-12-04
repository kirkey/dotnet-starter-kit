using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for group memberships.
/// </summary>
internal static class GroupMembershipSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 10;
        var existingCount = await context.GroupMemberships.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
        var groups = await context.MemberGroups.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 2 || groups.Count < 1) return;

        // Assign first 2 members to first group
        for (int i = 0; i < Math.Min(2, members.Count) && groups.Count > 0; i++)
        {
            var exists = await context.GroupMemberships
                .AnyAsync(gm => gm.MemberId == members[i].Id && gm.GroupId == groups[0].Id, cancellationToken)
                .ConfigureAwait(false);

            if (!exists)
            {
                var membership = GroupMembership.Create(
                    memberId: members[i].Id,
                    groupId: groups[0].Id,
                    role: i == 0 ? GroupMembership.RoleLeader : GroupMembership.RoleMember);

                await context.GroupMemberships.AddAsync(membership, cancellationToken).ConfigureAwait(false);
            }
        }

        // Assign members 3-4 to second group
        for (int i = 2; i < Math.Min(4, members.Count) && groups.Count > 1; i++)
        {
            var exists = await context.GroupMemberships
                .AnyAsync(gm => gm.MemberId == members[i].Id && gm.GroupId == groups[1].Id, cancellationToken)
                .ConfigureAwait(false);

            if (!exists)
            {
                var membership = GroupMembership.Create(
                    memberId: members[i].Id,
                    groupId: groups[1].Id,
                    role: i == 2 ? GroupMembership.RoleLeader : GroupMembership.RoleMember);

                await context.GroupMemberships.AddAsync(membership, cancellationToken).ConfigureAwait(false);
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded group memberships", tenant);
    }
}
