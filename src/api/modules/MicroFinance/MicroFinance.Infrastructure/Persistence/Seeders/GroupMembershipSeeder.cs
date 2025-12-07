using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for group memberships with comprehensive test data.
/// Creates 80 group memberships across various groups for testing group lending and solidarity features.
/// </summary>
internal static class GroupMembershipSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 80;
        var existingCount = await context.GroupMemberships.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Where(m => m.IsActive).Take(80).ToListAsync(cancellationToken).ConfigureAwait(false);
        var groups = await context.MemberGroups.Where(g => g.Status == MemberGroup.StatusActive).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 20 || groups.Count < 10) return;

        // Group membership assignments: (MemberIndex, GroupIndex, Role)
        var assignments = new (int MemberIdx, int GroupIdx, string Role)[]
        {
            // Samahan ng Kababaihan (GRP-001) - 8 members
            (0, 0, GroupMembership.RoleLeader),
            (1, 0, GroupMembership.RoleTreasurer),
            (2, 0, GroupMembership.RoleSecretary),
            (3, 0, GroupMembership.RoleMember),
            (4, 0, GroupMembership.RoleMember),
            (50, 0, GroupMembership.RoleMember),
            (51, 0, GroupMembership.RoleMember),
            (52, 0, GroupMembership.RoleMember),
            
            // Magkakapatid na Magsasaka (GRP-002) - 8 members
            (5, 1, GroupMembership.RoleLeader),
            (6, 1, GroupMembership.RoleTreasurer),
            (7, 1, GroupMembership.RoleMember),
            (8, 1, GroupMembership.RoleMember),
            (9, 1, GroupMembership.RoleMember),
            (10, 1, GroupMembership.RoleMember),
            (53, 1, GroupMembership.RoleMember),
            (54, 1, GroupMembership.RoleMember),
            
            // Kabataang Negosyante (GRP-003) - 7 members
            (11, 2, GroupMembership.RoleLeader),
            (12, 2, GroupMembership.RoleSecretary),
            (13, 2, GroupMembership.RoleMember),
            (14, 2, GroupMembership.RoleMember),
            (15, 2, GroupMembership.RoleMember),
            (31, 2, GroupMembership.RoleMember),
            (32, 2, GroupMembership.RoleMember),
            
            // Samahan ng mga Tindera (GRP-004) - 7 members
            (16, 3, GroupMembership.RoleLeader),
            (17, 3, GroupMembership.RoleTreasurer),
            (18, 3, GroupMembership.RoleMember),
            (19, 3, GroupMembership.RoleMember),
            (20, 3, GroupMembership.RoleMember),
            (66, 3, GroupMembership.RoleMember),
            (67, 3, GroupMembership.RoleMember),
            
            // Kooperatiba ng mga Guro (GRP-005) - 7 members
            (21, 4, GroupMembership.RoleLeader),
            (22, 4, GroupMembership.RoleSecretary),
            (23, 4, GroupMembership.RoleTreasurer),
            (24, 4, GroupMembership.RoleMember),
            (25, 4, GroupMembership.RoleMember),
            (56, 4, GroupMembership.RoleMember),
            (57, 4, GroupMembership.RoleMember),
            
            // Samahan ng mga Nars at Doktor (GRP-006) - 6 members
            (26, 5, GroupMembership.RoleLeader),
            (27, 5, GroupMembership.RoleTreasurer),
            (28, 5, GroupMembership.RoleMember),
            (29, 5, GroupMembership.RoleMember),
            (61, 5, GroupMembership.RoleMember),
            (64, 5, GroupMembership.RoleMember),
            
            // Grupo ng mga Artisano (GRP-007) - 6 members
            (36, 6, GroupMembership.RoleLeader),
            (37, 6, GroupMembership.RoleSecretary),
            (38, 6, GroupMembership.RoleMember),
            (39, 6, GroupMembership.RoleMember),
            (40, 6, GroupMembership.RoleMember),
            (86, 6, GroupMembership.RoleMember),
            
            // Samahan ng mga Tsuper (GRP-008) - 6 members
            (26, 7, GroupMembership.RoleLeader),
            (27, 7, GroupMembership.RoleTreasurer),
            (28, 7, GroupMembership.RoleMember),
            (29, 7, GroupMembership.RoleMember),
            (30, 7, GroupMembership.RoleMember),
            (76, 7, GroupMembership.RoleMember),
            
            // Pag-unlad ng Barangay (GRP-009) - 6 members
            (46, 8, GroupMembership.RoleLeader),
            (47, 8, GroupMembership.RoleSecretary),
            (48, 8, GroupMembership.RoleMember),
            (49, 8, GroupMembership.RoleMember),
            (55, 8, GroupMembership.RoleMember),
            (58, 8, GroupMembership.RoleMember),
            
            // Kababaihan sa Negosyo (GRP-010) - 7 members
            (33, 9, GroupMembership.RoleLeader),
            (34, 9, GroupMembership.RoleTreasurer),
            (35, 9, GroupMembership.RoleMember),
            (70, 9, GroupMembership.RoleMember),
            (72, 9, GroupMembership.RoleMember),
            (74, 9, GroupMembership.RoleMember),
            (85, 9, GroupMembership.RoleMember),
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
