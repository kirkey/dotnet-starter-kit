using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for group memberships with comprehensive test data.
/// Creates 30 group memberships across various groups for testing group lending and solidarity features.
/// </summary>
internal static class GroupMembershipSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 30;
        var existingCount = await context.GroupMemberships.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Where(m => m.IsActive).Take(30).ToListAsync(cancellationToken).ConfigureAwait(false);
        var groups = await context.MemberGroups.Where(g => g.IsActive).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 10 || groups.Count < 5) return;

        // Group membership assignments: (MemberIndex, GroupIndex, Role)
        var assignments = new (int MemberIdx, int GroupIdx, string Role)[]
        {
            // Sunrise Women Group (GRP-001) - 5 members
            (0, 0, GroupMembership.RoleLeader),
            (1, 0, GroupMembership.RoleTreasurer),
            (2, 0, GroupMembership.RoleSecretary),
            (3, 0, GroupMembership.RoleMember),
            (4, 0, GroupMembership.RoleMember),
            
            // Farmers United (GRP-002) - 6 members
            (5, 1, GroupMembership.RoleLeader),
            (6, 1, GroupMembership.RoleTreasurer),
            (7, 1, GroupMembership.RoleMember),
            (8, 1, GroupMembership.RoleMember),
            (9, 1, GroupMembership.RoleMember),
            (10, 1, GroupMembership.RoleMember),
            
            // Youth Entrepreneurs (GRP-003) - 5 members
            (11, 2, GroupMembership.RoleLeader),
            (12, 2, GroupMembership.RoleSecretary),
            (13, 2, GroupMembership.RoleMember),
            (14, 2, GroupMembership.RoleMember),
            (15, 2, GroupMembership.RoleMember),
            
            // Market Traders Association (GRP-004) - 5 members
            (16, 3, GroupMembership.RoleLeader),
            (17, 3, GroupMembership.RoleTreasurer),
            (18, 3, GroupMembership.RoleMember),
            (19, 3, GroupMembership.RoleMember),
            (20, 3, GroupMembership.RoleMember),
            
            // Teachers Cooperative (GRP-005) - 5 members
            (21, 4, GroupMembership.RoleLeader),
            (22, 4, GroupMembership.RoleSecretary),
            (23, 4, GroupMembership.RoleTreasurer),
            (24, 4, GroupMembership.RoleMember),
            (25, 4, GroupMembership.RoleMember),
            
            // Additional members in multiple groups (cross-membership)
            (26, 0, GroupMembership.RoleMember),
            (27, 1, GroupMembership.RoleMember),
            (28, 2, GroupMembership.RoleMember),
            (29, 3, GroupMembership.RoleMember),
        };

        foreach (var (memberIdx, groupIdx, role) in assignments)
        {
            if (memberIdx >= members.Count || groupIdx >= groups.Count) continue;

            var memberId = members[memberIdx].Id;
            var groupId = groups[groupIdx].Id;

            var exists = await context.GroupMemberships
                .AnyAsync(gm => gm.MemberId == memberId && gm.GroupId == groupId, cancellationToken)
                .ConfigureAwait(false);

            if (!exists)
            {
                var membership = GroupMembership.Create(
                    memberId: memberId,
                    groupId: groupId,
                    role: role);

                await context.GroupMemberships.AddAsync(membership, cancellationToken).ConfigureAwait(false);
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} group memberships across {GroupCount} groups", tenant, targetCount, groups.Count);
    }
}
