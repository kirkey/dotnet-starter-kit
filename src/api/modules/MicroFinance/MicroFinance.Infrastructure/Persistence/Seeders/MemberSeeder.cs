using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for members.
/// </summary>
internal static class MemberSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 10;
        var existingCount = await context.Members.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = new (string Num, string First, string Last, string? Middle, string Email, string Phone, string Gender, string Occupation, decimal Income, int BirthYear)[]
        {
            ("MBR-001", "John", "Doe", "Michael", "john.doe@email.com", "+1234567890", "Male", "Farmer", 500, 1985),
            ("MBR-002", "Jane", "Smith", null, "jane.smith@email.com", "+1234567891", "Female", "Teacher", 800, 1990),
            ("MBR-003", "Robert", "Johnson", "William", "robert.j@email.com", "+1234567892", "Male", "Trader", 1200, 1982),
            ("MBR-004", "Mary", "Williams", null, "mary.w@email.com", "+1234567893", "Female", "Nurse", 950, 1988),
            ("MBR-005", "James", "Brown", "Edward", "james.b@email.com", "+1234567894", "Male", "Mechanic", 700, 1975),
            ("MBR-006", "Patricia", "Davis", null, "patricia.d@email.com", "+1234567895", "Female", "Shopkeeper", 600, 1992),
            ("MBR-007", "Michael", "Miller", "Joseph", "michael.m@email.com", "+1234567896", "Male", "Driver", 550, 1980),
            ("MBR-008", "Linda", "Wilson", null, "linda.w@email.com", "+1234567897", "Female", "Tailor", 450, 1987),
            ("MBR-009", "David", "Moore", "Thomas", "david.m@email.com", "+1234567898", "Male", "Carpenter", 650, 1978),
            ("MBR-010", "Elizabeth", "Taylor", null, "elizabeth.t@email.com", "+1234567899", "Female", "Baker", 500, 1995),
        };

        for (int i = existingCount; i < members.Length; i++)
        {
            var m = members[i];
            if (await context.Members.AnyAsync(x => x.MemberNumber == m.Num, cancellationToken).ConfigureAwait(false))
                continue;

            var member = Member.Create(
                memberNumber: m.Num,
                firstName: m.First,
                lastName: m.Last,
                middleName: m.Middle,
                email: m.Email,
                phoneNumber: m.Phone,
                dateOfBirth: new DateOnly(m.BirthYear, 1, 15),
                gender: m.Gender,
                address: $"{100 + i} Main Street",
                city: "Metropolis",
                state: "Central State",
                postalCode: $"1000{i}",
                country: "United States",
                nationalId: $"NAT-{m.Num}",
                occupation: m.Occupation,
                monthlyIncome: m.Income,
                joinDate: DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-12 + i)));

            await context.Members.AddAsync(member, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded members", tenant);
    }
}
