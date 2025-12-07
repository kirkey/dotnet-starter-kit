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
            ("MBR-001", "Juan", "Dela Cruz", "Miguel", "juan.delacruz@email.com", "+639171234567", "Male", "Farmer", 15000, 1985, "Nueva Ecija", true),
            ("MBR-002", "Maria", "Santos", null, "maria.santos@email.com", "+639172234567", "Female", "Farmer", 12000, 1980, "Isabela", true),
            ("MBR-003", "Pedro", "Reyes", "Jose", "pedro.reyes@email.com", "+639173234567", "Male", "Farmer", 18000, 1975, "Pangasinan", true),
            ("MBR-004", "Rosa", "Garcia", null, "rosa.garcia@email.com", "+639174234567", "Female", "Farmer", 10000, 1988, "Tarlac", true),
            ("MBR-005", "Antonio", "Ramos", "Carlos", "antonio.ramos@email.com", "+639175234567", "Male", "Farmer", 16000, 1982, "Bulacan", true),
            
            // Teachers & Education sector
            ("MBR-006", "Ana", "Mendoza", null, "ana.mendoza@email.com", "+639176234567", "Female", "Teacher", 25000, 1990, "Quezon City", true),
            ("MBR-007", "Jose", "Torres", "Ramon", "jose.torres@email.com", "+639177234567", "Male", "Teacher", 28000, 1987, "Makati", true),
            ("MBR-008", "Lorna", "Aquino", null, "lorna.aquino@email.com", "+639178234567", "Female", "Head Teacher", 45000, 1978, "Pasig", true),
            ("MBR-009", "Ricardo", "Villanueva", "Eduardo", "ricardo.villanueva@email.com", "+639179234567", "Male", "Professor", 65000, 1970, "Manila", true),
            ("MBR-010", "Cristina", "Bautista", null, "cristina.bautista@email.com", "+639180234567", "Female", "Teacher", 22000, 1992, "Caloocan", true),
            
            // Healthcare workers
            ("MBR-011", "Roberto", "Fernandez", "Luis", "roberto.fernandez@email.com", "+639181234567", "Male", "Nurse", 32000, 1985, "Mandaluyong", true),
            ("MBR-012", "Patricia", "Gonzales", null, "patricia.gonzales@email.com", "+639182234567", "Female", "Doctor", 120000, 1975, "Makati", true),
            ("MBR-013", "Manuel", "Soriano", "Felipe", "manuel.soriano@email.com", "+639183234567", "Male", "Pharmacist", 55000, 1982, "Pasay", true),
            ("MBR-014", "Liza", "Dizon", null, "liza.dizon@email.com", "+639184234567", "Female", "Nurse", 30000, 1988, "Taguig", true),
            ("MBR-015", "Danilo", "Mercado", "Pablo", "danilo.mercado@email.com", "+639185234567", "Male", "Lab Technician", 38000, 1984, "Paranaque", true),
            
            // Small business owners - Micro business loan candidates
            ("MBR-016", "Elena", "Castillo", null, "elena.castillo@email.com", "+639186234567", "Female", "Sari-sari Store Owner", 35000, 1979, "Marikina", true),
            ("MBR-017", "Reynaldo", "Navarro", "Andres", "reynaldo.navarro@email.com", "+639187234567", "Male", "Trader", 50000, 1976, "Divisoria", true),
            ("MBR-018", "Beatriz", "Pascual", null, "beatriz.pascual@email.com", "+639188234567", "Female", "Baker", 28000, 1983, "San Juan", true),
            ("MBR-019", "Eduardo", "Salvador", "Mario", "eduardo.salvador@email.com", "+639189234567", "Male", "Carinderia Owner", 75000, 1972, "Quezon City", true),
            ("MBR-020", "Rosario", "Aguilar", null, "rosario.aguilar@email.com", "+639190234567", "Female", "Mananahi", 20000, 1986, "Pateros", true),
            
            // Skilled workers
            ("MBR-021", "Carlos", "Ocampo", "Rafael", "carlos.ocampo@email.com", "+639191234567", "Male", "Karpintero", 22000, 1980, "Las Pinas", true),
            ("MBR-022", "Jocelyn", "Manalo", null, "jocelyn.manalo@email.com", "+639192234567", "Female", "Electrician", 30000, 1985, "Muntinlupa", true),
            ("MBR-023", "Rolando", "Padilla", "Sergio", "rolando.padilla@email.com", "+639193234567", "Male", "Mekaniko", 25000, 1978, "Valenzuela", true),
            ("MBR-024", "Maricel", "Tolentino", null, "maricel.tolentino@email.com", "+639194234567", "Female", "Tubero", 26000, 1983, "Malabon", true),
            ("MBR-025", "Ernesto", "Velasco", "Arturo", "ernesto.velasco@email.com", "+639195234567", "Male", "Welder", 28000, 1981, "Navotas", true),
            
            // Transport sector
            ("MBR-026", "Dante", "Francisco", "Lorenzo", "dante.francisco@email.com", "+639196234567", "Male", "Taxi Driver", 18000, 1977, "Cubao", true),
            ("MBR-027", "Nelia", "Ignacio", null, "nelia.ignacio@email.com", "+639197234567", "Female", "Bus Driver", 22000, 1982, "Pasay", true),
            ("MBR-028", "Marcelo", "Lorenzo", "Benjamin", "marcelo.lorenzo@email.com", "+639198234567", "Male", "Truck Driver", 30000, 1975, "Calamba", true),
            ("MBR-029", "Karen", "Morales", null, "karen.morales@email.com", "+639199234567", "Female", "Courier", 15000, 1989, "Antipolo", true),
            ("MBR-030", "Joel", "Sta. Maria", "Miguel", "joel.stamaria@email.com", "+639200234567", "Male", "Habal-habal Driver", 12000, 1991, "Rizal", true),
            
            // Young entrepreneurs
            ("MBR-031", "Alyssa", "Dimaculangan", null, "alyssa.dimaculangan@email.com", "+639201234567", "Female", "Tech Startup", 60000, 1995, "BGC", true),
            ("MBR-032", "Marco", "Villareal", "Francis", "marco.villareal@email.com", "+639202234567", "Male", "Digital Marketing", 65000, 1993, "Makati", true),
            ("MBR-033", "Denise", "Catalan", null, "denise.catalan@email.com", "+639203234567", "Female", "Online Seller", 50000, 1994, "Pasig", true),
            ("MBR-034", "Paolo", "Evangelista", "Victor", "paolo.evangelista@email.com", "+639204234567", "Male", "App Developer", 85000, 1992, "Ortigas", true),
            ("MBR-035", "Samantha", "Lacson", null, "samantha.lacson@email.com", "+639205234567", "Female", "Social Media Manager", 40000, 1996, "Quezon City", true),
            
            // Artisans
            ("MBR-036", "Andres", "Bonifacio", "Roberto", "andres.bonifacio@email.com", "+639206234567", "Male", "Potter", 14000, 1979, "Vigan", true),
            ("MBR-037", "Angeline", "Magsaysay", null, "angeline.magsaysay@email.com", "+639207234567", "Female", "Weaver", 12000, 1984, "Ilocos Sur", true),
            ("MBR-038", "Jaime", "Laurel", "Daniel", "jaime.laurel@email.com", "+639208234567", "Male", "Panday", 16000, 1976, "Batangas", true),
            ("MBR-039", "Katrina", "Roxas", null, "katrina.roxas@email.com", "+639209234567", "Female", "Jeweler", 28000, 1983, "Meycauayan", true),
            ("MBR-040", "Kevin", "Osmeña", "Antonio", "kevin.osmena@email.com", "+639210234567", "Male", "Sculptor", 20000, 1980, "Paete", true),
            
            // Inactive members for testing status filters
            ("MBR-041", "Mariano", "Quezon", "Almario", "mariano.quezon@email.com", "+639211234567", "Male", "Retired", 8000, 1955, "Baler", false),
            ("MBR-042", "Sharon", "Estrada", null, "sharon.estrada@email.com", "+639212234567", "Female", "Housewife", 0, 1965, "Cavite", false),
            ("MBR-043", "Bryan", "Macapagal", "Leo", "bryan.macapagal@email.com", "+639213234567", "Male", "Unemployed", 0, 1990, "Pampanga", false),
            ("MBR-044", "Michelle", "Arroyo", null, "michelle.arroyo@email.com", "+639214234567", "Female", "Student", 0, 1998, "Diliman", false),
            ("MBR-045", "Edwin", "Marcos", "Juan", "edwin.marcos@email.com", "+639215234567", "Male", "OFW", 0, 1985, "Ilocos Norte", false),
            
            // More active diverse members
            ("MBR-046", "Lourdes", "Enrile", null, "lourdes.enrile@email.com", "+639216234567", "Female", "Hotel Manager", 60000, 1981, "Boracay", true),
            ("MBR-047", "Ronald", "Recto", "Pedro", "ronald.recto@email.com", "+639217234567", "Male", "Chef", 40000, 1978, "Tagaytay", true),
            ("MBR-048", "Dina", "Sotto", null, "dina.sotto@email.com", "+639218234567", "Female", "Florist", 18000, 1986, "Baguio", true),
            ("MBR-049", "Tomas", "Binay", "Jose", "tomas.binay@email.com", "+639219234567", "Male", "Security Guard", 15000, 1977, "Makati", true),
            ("MBR-050", "Cynthia", "Cayetano", null, "cynthia.cayetano@email.com", "+639220234567", "Female", "Real Estate Agent", 75000, 1980, "Taguig", true),
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
                country: "Philippines",
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
