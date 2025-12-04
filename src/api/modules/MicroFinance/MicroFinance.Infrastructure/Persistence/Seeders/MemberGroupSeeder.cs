using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for member groups.
/// </summary>
internal static class MemberGroupSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 10;
        var existingCount = await context.MemberGroups.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var groups = new (string Code, string Name, string Desc, string Location, string Freq, string Day)[]
        {
            ("GRP-001", "Sunrise Women Group", "Women empowerment savings group", "Community Center A", MemberGroup.FrequencyWeekly, "Monday"),
            ("GRP-002", "Farmers United", "Agricultural cooperative group", "Village Hall", MemberGroup.FrequencyBiweekly, "Wednesday"),
            ("GRP-003", "Youth Entrepreneurs", "Young business owners group", "Youth Center", MemberGroup.FrequencyWeekly, "Friday"),
            ("GRP-004", "Market Traders Association", "Traders savings collective", "Market Square", MemberGroup.FrequencyWeekly, "Saturday"),
            ("GRP-005", "Teachers Cooperative", "Education sector savings group", "School Hall", MemberGroup.FrequencyMonthly, "Tuesday"),
            ("GRP-006", "Healthcare Workers Group", "Medical staff savings", "Hospital Meeting Room", MemberGroup.FrequencyBiweekly, "Thursday"),
            ("GRP-007", "Artisan Collective", "Craftsmen and artisans group", "Craft Center", MemberGroup.FrequencyWeekly, "Wednesday"),
            ("GRP-008", "Transport Workers Union", "Drivers and transport staff", "Transport Hub", MemberGroup.FrequencyWeekly, "Monday"),
            ("GRP-009", "Village Development Group", "Community development savings", "Village Chief Office", MemberGroup.FrequencyMonthly, "Saturday"),
            ("GRP-010", "Women in Business", "Female entrepreneurs collective", "Business Center", MemberGroup.FrequencyWeekly, "Tuesday"),
        };

        for (int i = existingCount; i < groups.Length; i++)
        {
            var g = groups[i];
            if (await context.MemberGroups.AnyAsync(x => x.Code == g.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var group = MemberGroup.Create(
                code: g.Code,
                name: g.Name,
                description: g.Desc,
                formationDate: DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-6 + i)),
                meetingLocation: g.Location,
                meetingFrequency: g.Freq,
                meetingDay: g.Day,
                meetingTime: new TimeOnly(9, 0));

            group.Activate();
            await context.MemberGroups.AddAsync(group, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded member groups", tenant);
    }
}
