using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for member groups.
/// Creates 35 solidarity groups for realistic demo database.
/// </summary>
internal static class MemberGroupSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 35;
        var existingCount = await context.MemberGroups.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var groups = new (string Code, string Name, string Desc, string Location, string Freq, string Day)[]
        {
            ("GRP-001", "Samahan ng Kababaihan ng Barangay", "Women empowerment savings group", "Barangay Hall, Makati", MemberGroup.FrequencyWeekly, "Monday"),
            ("GRP-002", "Magkakapatid na Magsasaka", "Agricultural cooperative group", "Palengke ng Bayan, Nueva Ecija", MemberGroup.FrequencyBiweekly, "Wednesday"),
            ("GRP-003", "Kabataang Negosyante", "Young business owners group", "SK Hall, Quezon City", MemberGroup.FrequencyWeekly, "Friday"),
            ("GRP-004", "Samahan ng mga Tindera sa Palengke", "Traders savings collective", "Divisoria Market, Manila", MemberGroup.FrequencyWeekly, "Saturday"),
            ("GRP-005", "Kooperatiba ng mga Guro", "Education sector savings group", "DepEd District Office, Pasig", MemberGroup.FrequencyMonthly, "Tuesday"),
            ("GRP-006", "Samahan ng mga Nars at Doktor", "Medical staff savings", "Philippine General Hospital, Manila", MemberGroup.FrequencyBiweekly, "Thursday"),
            ("GRP-007", "Grupo ng mga Artisano", "Craftsmen and artisans group", "Paete Carvers Hall, Laguna", MemberGroup.FrequencyWeekly, "Wednesday"),
            ("GRP-008", "Samahan ng mga Tsuper", "Drivers and transport staff", "LTFRB Terminal, Cubao", MemberGroup.FrequencyWeekly, "Monday"),
            ("GRP-009", "Pag-unlad ng Barangay Uno", "Community development savings", "Municipal Hall, Pampanga", MemberGroup.FrequencyMonthly, "Saturday"),
            ("GRP-010", "Kababaihan sa Negosyo", "Female entrepreneurs collective", "TESDA Women's Center, Taguig", MemberGroup.FrequencyWeekly, "Tuesday"),
            
            // Additional groups
            ("GRP-011", "Mangingisda ng Bataan", "Fishermen's savings group", "Fisherman's Wharf, Bataan", MemberGroup.FrequencyWeekly, "Sunday"),
            ("GRP-012", "Kooperatiba ng mga Magsasaka ng Isabela", "Rice farmers cooperative", "Municipal Gym, Isabela", MemberGroup.FrequencyBiweekly, "Friday"),
            ("GRP-013", "Samahan ng mga Barbero at Beautician", "Salon workers savings", "Town Plaza, Marikina", MemberGroup.FrequencyWeekly, "Monday"),
            ("GRP-014", "Grupo ng mga Konstruksyon", "Construction workers group", "Workers Hall, Las Pinas", MemberGroup.FrequencyBiweekly, "Saturday"),
            ("GRP-015", "Kababaihan ng Cebu", "Cebu women's savings group", "Barangay Center, Cebu City", MemberGroup.FrequencyWeekly, "Thursday"),
            ("GRP-016", "Samahan ng mga Vendor sa Davao", "Street vendors collective", "Public Market, Davao City", MemberGroup.FrequencyWeekly, "Wednesday"),
            ("GRP-017", "Grupo Bayanihan ng Baguio", "Community mutual aid", "Session Road Hall, Baguio", MemberGroup.FrequencyMonthly, "Friday"),
            ("GRP-018", "Kooperatiba ng mga Mangga Growers", "Mango farmers group", "Agricultural Center, Guimaras", MemberGroup.FrequencyBiweekly, "Tuesday"),
            ("GRP-019", "Samahan ng mga Overseas Workers Family", "OFW families support group", "OWWA Office, Pasay", MemberGroup.FrequencyMonthly, "Sunday"),
            ("GRP-020", "Grupo ng mga Digital Freelancers", "Online workers savings", "Co-working Space, BGC", MemberGroup.FrequencyWeekly, "Saturday"),
            
            // Additional groups (21-35)
            ("GRP-021", "Samahan ng mga Tricycle Drivers", "Tricycle operators group", "Terminal, Rizal", MemberGroup.FrequencyWeekly, "Tuesday"),
            ("GRP-022", "Kababaihan sa Palawan", "Palawan women's savings", "Town Hall, Puerto Princesa", MemberGroup.FrequencyBiweekly, "Wednesday"),
            ("GRP-023", "Grupo ng mga Factory Workers", "Factory employees savings", "Industrial Zone, Cavite", MemberGroup.FrequencyBiweekly, "Thursday"),
            ("GRP-024", "Samahan ng mga Sari-Sari Store Owners", "Convenience store operators", "Community Center, Caloocan", MemberGroup.FrequencyWeekly, "Monday"),
            ("GRP-025", "Kooperatiba ng mga Coconut Farmers", "Coconut farmers group", "Coco Hall, Quezon Province", MemberGroup.FrequencyMonthly, "Saturday"),
            ("GRP-026", "Grupo ng mga Call Center Agents", "BPO workers savings", "Eastwood Hall, Quezon City", MemberGroup.FrequencyBiweekly, "Friday"),
            ("GRP-027", "Samahan ng mga Nanay sa Malabon", "Mothers savings group", "Barangay Hall, Malabon", MemberGroup.FrequencyWeekly, "Wednesday"),
            ("GRP-028", "Kooperatiba ng mga Hog Raisers", "Pig farmers cooperative", "Livestock Center, Bulacan", MemberGroup.FrequencyBiweekly, "Tuesday"),
            ("GRP-029", "Grupo ng mga Senior Citizens", "Elderly savings group", "Senior Center, Manila", MemberGroup.FrequencyMonthly, "Thursday"),
            ("GRP-030", "Samahan ng mga Labandera", "Laundry workers group", "Community Center, Mandaluyong", MemberGroup.FrequencyWeekly, "Saturday"),
            ("GRP-031", "Kooperatiba ng mga Panaderos", "Bakers cooperative", "Bakery Association, Pampanga", MemberGroup.FrequencyBiweekly, "Monday"),
            ("GRP-032", "Grupo ng mga Online Sellers", "E-commerce sellers group", "Digital Hub, Pasig", MemberGroup.FrequencyWeekly, "Sunday"),
            ("GRP-033", "Samahan ng mga Vegetable Growers", "Vegetable farmers group", "Agri Center, Benguet", MemberGroup.FrequencyBiweekly, "Wednesday"),
            ("GRP-034", "Grupo ng mga Security Guards", "Security personnel savings", "Training Center, Makati", MemberGroup.FrequencyBiweekly, "Friday"),
            ("GRP-035", "Samahan ng mga Ambulant Vendors", "Street vendors collective", "Public Plaza, Manila", MemberGroup.FrequencyWeekly, "Saturday"),
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
