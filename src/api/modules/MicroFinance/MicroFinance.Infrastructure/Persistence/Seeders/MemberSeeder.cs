using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for members with comprehensive test data.
/// Creates 50 members with diverse demographics for testing all MicroFinance features.
/// </summary>
internal static class MemberSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 50;
        var existingCount = await context.Members.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = new (string Num, string First, string Last, string? Middle, string Email, string Phone, string Gender, string Occupation, decimal Income, int BirthYear, string City, bool IsActive)[]
        {
            // Active farmers - Agricultural loan candidates
            ("MBR-001", "John", "Doe", "Michael", "john.doe@email.com", "+1234567890", "Male", "Farmer", 500, 1985, "Greenville", true),
            ("MBR-002", "Mary", "Johnson", null, "mary.j@email.com", "+1234567891", "Female", "Farmer", 450, 1980, "Farmington", true),
            ("MBR-003", "Peter", "Okonkwo", "Chidi", "peter.o@email.com", "+1234567892", "Male", "Farmer", 600, 1975, "Croptown", true),
            ("MBR-004", "Grace", "Abubakar", null, "grace.a@email.com", "+1234567893", "Female", "Farmer", 400, 1988, "Harvest City", true),
            ("MBR-005", "Samuel", "Mensah", "Kofi", "samuel.m@email.com", "+1234567894", "Male", "Farmer", 550, 1982, "Greenville", true),
            
            // Teachers & Education sector
            ("MBR-006", "Jane", "Smith", null, "jane.smith@email.com", "+1234567895", "Female", "Teacher", 800, 1990, "Metropolis", true),
            ("MBR-007", "David", "Williams", "James", "david.w@email.com", "+1234567896", "Male", "Teacher", 850, 1987, "Education City", true),
            ("MBR-008", "Sarah", "Brown", null, "sarah.b@email.com", "+1234567897", "Female", "Head Teacher", 1200, 1978, "Schooltown", true),
            ("MBR-009", "Michael", "Davis", "Edward", "michael.d@email.com", "+1234567898", "Male", "Professor", 2000, 1970, "University Heights", true),
            ("MBR-010", "Emily", "Wilson", null, "emily.w@email.com", "+1234567899", "Female", "Teacher", 750, 1992, "Metropolis", true),
            
            // Healthcare workers
            ("MBR-011", "Robert", "Taylor", "Thomas", "robert.t@email.com", "+1234567900", "Male", "Nurse", 950, 1985, "Medical Center", true),
            ("MBR-012", "Patricia", "Anderson", null, "patricia.a@email.com", "+1234567901", "Female", "Doctor", 3500, 1975, "Healthcare City", true),
            ("MBR-013", "James", "Thomas", "Henry", "james.t@email.com", "+1234567902", "Male", "Pharmacist", 1500, 1982, "Wellness Town", true),
            ("MBR-014", "Linda", "Jackson", null, "linda.j@email.com", "+1234567903", "Female", "Nurse", 900, 1988, "Medical Center", true),
            ("MBR-015", "William", "White", "George", "william.w@email.com", "+1234567904", "Male", "Lab Technician", 1100, 1984, "Healthcare City", true),
            
            // Small business owners - Micro business loan candidates
            ("MBR-016", "Elizabeth", "Harris", null, "elizabeth.h@email.com", "+1234567905", "Female", "Shopkeeper", 1200, 1979, "Commerce Town", true),
            ("MBR-017", "Richard", "Martin", "Allen", "richard.m@email.com", "+1234567906", "Male", "Trader", 1500, 1976, "Market Square", true),
            ("MBR-018", "Barbara", "Garcia", null, "barbara.g@email.com", "+1234567907", "Female", "Baker", 800, 1983, "Food District", true),
            ("MBR-019", "Joseph", "Martinez", "Luis", "joseph.m@email.com", "+1234567908", "Male", "Restaurant Owner", 2500, 1972, "Culinary City", true),
            ("MBR-020", "Susan", "Robinson", null, "susan.r@email.com", "+1234567909", "Female", "Tailor", 650, 1986, "Fashion Town", true),
            
            // Skilled workers
            ("MBR-021", "Charles", "Clark", "Raymond", "charles.c@email.com", "+1234567910", "Male", "Carpenter", 700, 1980, "Builder's Row", true),
            ("MBR-022", "Jessica", "Rodriguez", null, "jessica.r@email.com", "+1234567911", "Female", "Electrician", 900, 1985, "Tech City", true),
            ("MBR-023", "Thomas", "Lewis", "William", "thomas.l@email.com", "+1234567912", "Male", "Mechanic", 750, 1978, "Auto Town", true),
            ("MBR-024", "Margaret", "Lee", null, "margaret.l@email.com", "+1234567913", "Female", "Plumber", 800, 1983, "Service City", true),
            ("MBR-025", "Christopher", "Walker", "Paul", "christopher.w@email.com", "+1234567914", "Male", "Welder", 850, 1981, "Industrial Zone", true),
            
            // Transport sector
            ("MBR-026", "Daniel", "Hall", "Joseph", "daniel.h@email.com", "+1234567915", "Male", "Taxi Driver", 600, 1977, "Transit Hub", true),
            ("MBR-027", "Nancy", "Allen", null, "nancy.a@email.com", "+1234567916", "Female", "Bus Driver", 700, 1982, "Transport City", true),
            ("MBR-028", "Matthew", "Young", "Andrew", "matthew.y@email.com", "+1234567917", "Male", "Truck Driver", 900, 1975, "Logistics Park", true),
            ("MBR-029", "Karen", "King", null, "karen.k@email.com", "+1234567918", "Female", "Courier", 500, 1989, "Delivery Town", true),
            ("MBR-030", "Steven", "Wright", "Michael", "steven.w@email.com", "+1234567919", "Male", "Motorcycle Taxi", 400, 1991, "Quick Transport", true),
            
            // Young entrepreneurs
            ("MBR-031", "Betty", "Lopez", null, "betty.l@email.com", "+1234567920", "Female", "Tech Startup", 1800, 1995, "Innovation Hub", true),
            ("MBR-032", "Donald", "Hill", "Frank", "donald.h@email.com", "+1234567921", "Male", "Digital Marketing", 2000, 1993, "Tech Valley", true),
            ("MBR-033", "Dorothy", "Scott", null, "dorothy.s@email.com", "+1234567922", "Female", "E-commerce", 1500, 1994, "Online City", true),
            ("MBR-034", "Paul", "Green", "Victor", "paul.g@email.com", "+1234567923", "Male", "App Developer", 2500, 1992, "Code Town", true),
            ("MBR-035", "Sandra", "Adams", null, "sandra.a@email.com", "+1234567924", "Female", "Social Media Manager", 1200, 1996, "Media City", true),
            
            // Artisans
            ("MBR-036", "Andrew", "Baker", "Robert", "andrew.b@email.com", "+1234567925", "Male", "Potter", 450, 1979, "Craft Village", true),
            ("MBR-037", "Ashley", "Nelson", null, "ashley.n@email.com", "+1234567926", "Female", "Weaver", 400, 1984, "Artisan Town", true),
            ("MBR-038", "Joshua", "Carter", "Daniel", "joshua.c@email.com", "+1234567927", "Male", "Blacksmith", 550, 1976, "Forge Town", true),
            ("MBR-039", "Kimberly", "Mitchell", null, "kimberly.m@email.com", "+1234567928", "Female", "Jeweler", 800, 1983, "Gem City", true),
            ("MBR-040", "Kevin", "Perez", "Anthony", "kevin.p@email.com", "+1234567929", "Male", "Sculptor", 600, 1980, "Art District", true),
            
            // Inactive members for testing status filters
            ("MBR-041", "Mark", "Roberts", "Alan", "mark.r@email.com", "+1234567930", "Male", "Retired", 300, 1955, "Seniors Town", false),
            ("MBR-042", "Sharon", "Turner", null, "sharon.t@email.com", "+1234567931", "Female", "Homemaker", 0, 1965, "Home City", false),
            ("MBR-043", "Brian", "Phillips", "Lee", "brian.p@email.com", "+1234567932", "Male", "Unemployed", 0, 1990, "Job Seeker Town", false),
            ("MBR-044", "Michelle", "Campbell", null, "michelle.c@email.com", "+1234567933", "Female", "Student", 0, 1998, "University Area", false),
            ("MBR-045", "Edward", "Parker", "John", "edward.p@email.com", "+1234567934", "Male", "Migrated", 0, 1985, "Abroad City", false),
            
            // More active diverse members
            ("MBR-046", "Laura", "Evans", null, "laura.e@email.com", "+1234567935", "Female", "Hotel Manager", 1800, 1981, "Tourism City", true),
            ("MBR-047", "Ronald", "Edwards", "Peter", "ronald.e@email.com", "+1234567936", "Male", "Chef", 1200, 1978, "Gourmet Town", true),
            ("MBR-048", "Deborah", "Collins", null, "deborah.c@email.com", "+1234567937", "Female", "Florist", 500, 1986, "Garden District", true),
            ("MBR-049", "Timothy", "Stewart", "James", "timothy.s@email.com", "+1234567938", "Male", "Security Guard", 450, 1977, "Safe City", true),
            ("MBR-050", "Cynthia", "Sanchez", null, "cynthia.s@email.com", "+1234567939", "Female", "Real Estate Agent", 2200, 1980, "Property Town", true),
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
                dateOfBirth: new DateOnly(m.BirthYear, 1, 15 + (i % 15)),
                gender: m.Gender,
                address: $"{100 + i} Main Street",
                city: m.City,
                state: "Central State",
                postalCode: $"{10000 + i}",
                country: "United States",
                nationalId: $"NAT-{m.Num}",
                occupation: m.Occupation,
                monthlyIncome: m.Income,
                joinDate: DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-24 + (i % 24))));

            // Deactivate inactive members
            if (!m.IsActive)
            {
                member.Deactivate();
            }

            await context.Members.AddAsync(member, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} members with diverse demographics", tenant, targetCount);
    }
}
