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
            ("GRP-001", "Samahan ng Kababaihan ng Barangay", "Women empowerment savings group", "Barangay Hall", MemberGroup.FrequencyWeekly, "Monday"),
            ("GRP-002", "Magkakapatid na Magsasaka", "Agricultural cooperative group", "Palengke ng Bayan", MemberGroup.FrequencyBiweekly, "Wednesday"),
            ("GRP-003", "Kabataang Negosyante", "Young business owners group", "SK Hall", MemberGroup.FrequencyWeekly, "Friday"),
            ("GRP-004", "Samahan ng mga Tindera sa Palengke", "Traders savings collective", "Divisoria Market", MemberGroup.FrequencyWeekly, "Saturday"),
            ("GRP-005", "Kooperatiba ng mga Guro", "Education sector savings group", "DepEd District Office", MemberGroup.FrequencyMonthly, "Tuesday"),
            ("GRP-006", "Samahan ng mga Nars at Doktor", "Medical staff savings", "Philippine General Hospital", MemberGroup.FrequencyBiweekly, "Thursday"),
            ("GRP-007", "Grupo ng mga Artisano", "Craftsmen and artisans group", "Paete Carvers Hall", MemberGroup.FrequencyWeekly, "Wednesday"),
            ("GRP-008", "Samahan ng mga Tsuper", "Drivers and transport staff", "LTFRB Terminal", MemberGroup.FrequencyWeekly, "Monday"),
            ("GRP-009", "Pag-unlad ng Barangay", "Community development savings", "Municipal Hall", MemberGroup.FrequencyMonthly, "Saturday"),
            ("GRP-010", "Kababaihan sa Negosyo", "Female entrepreneurs collective", "TESDA Women's Center", MemberGroup.FrequencyWeekly, "Tuesday"),
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
