using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for members with comprehensive test data.
/// Creates 100 members with diverse demographics for testing all MicroFinance features.
/// </summary>
internal static class MemberSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 100;
        var existingCount = await context.Members.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = new (string Num, string First, string Last, string? Middle, string Email, string Phone, string Gender, string Occupation, decimal Income, int BirthYear, string City, bool IsActive)[]
        {
            // Active farmers - Agricultural loan candidates
            ("MBR-001", "Juan", "Dela Cruz", "Miguel", "juan.delacruz@email.com", "+639171234567", "Male", "Magsasaka", 15000, 1985, "Nueva Ecija", true),
            ("MBR-002", "Maria", "Santos", null, "maria.santos@email.com", "+639172234567", "Female", "Magsasaka", 12000, 1980, "Isabela", true),
            ("MBR-003", "Pedro", "Reyes", "Jose", "pedro.reyes@email.com", "+639173234567", "Male", "Magsasaka", 18000, 1975, "Pangasinan", true),
            ("MBR-004", "Rosa", "Garcia", null, "rosa.garcia@email.com", "+639174234567", "Female", "Magsasaka", 10000, 1988, "Tarlac", true),
            ("MBR-005", "Antonio", "Ramos", "Carlos", "antonio.ramos@email.com", "+639175234567", "Male", "Magsasaka", 16000, 1982, "Bulacan", true),
            
            // Teachers & Education sector
            ("MBR-006", "Ana", "Mendoza", null, "ana.mendoza@email.com", "+639176234567", "Female", "Guro", 25000, 1990, "Quezon City", true),
            ("MBR-007", "Jose", "Torres", "Ramon", "jose.torres@email.com", "+639177234567", "Male", "Guro", 28000, 1987, "Makati", true),
            ("MBR-008", "Lorna", "Aquino", null, "lorna.aquino@email.com", "+639178234567", "Female", "Head Teacher", 45000, 1978, "Pasig", true),
            ("MBR-009", "Ricardo", "Villanueva", "Eduardo", "ricardo.villanueva@email.com", "+639179234567", "Male", "Propesor", 65000, 1970, "Manila", true),
            ("MBR-010", "Cristina", "Bautista", null, "cristina.bautista@email.com", "+639180234567", "Female", "Guro", 22000, 1992, "Caloocan", true),
            
            // Healthcare workers
            ("MBR-011", "Roberto", "Fernandez", "Luis", "roberto.fernandez@email.com", "+639181234567", "Male", "Nars", 32000, 1985, "Mandaluyong", true),
            ("MBR-012", "Patricia", "Gonzales", null, "patricia.gonzales@email.com", "+639182234567", "Female", "Doktor", 120000, 1975, "Makati", true),
            ("MBR-013", "Manuel", "Soriano", "Felipe", "manuel.soriano@email.com", "+639183234567", "Male", "Parmasiyutiko", 55000, 1982, "Pasay", true),
            ("MBR-014", "Liza", "Dizon", null, "liza.dizon@email.com", "+639184234567", "Female", "Nars", 30000, 1988, "Taguig", true),
            ("MBR-015", "Danilo", "Mercado", "Pablo", "danilo.mercado@email.com", "+639185234567", "Male", "Lab Technician", 38000, 1984, "Paranaque", true),
            
            // Small business owners - Micro business loan candidates
            ("MBR-016", "Elena", "Castillo", null, "elena.castillo@email.com", "+639186234567", "Female", "May-ari ng Sari-sari Store", 35000, 1979, "Marikina", true),
            ("MBR-017", "Reynaldo", "Navarro", "Andres", "reynaldo.navarro@email.com", "+639187234567", "Male", "Nagbebenta", 50000, 1976, "Divisoria", true),
            ("MBR-018", "Beatriz", "Pascual", null, "beatriz.pascual@email.com", "+639188234567", "Female", "Panadero", 28000, 1983, "San Juan", true),
            ("MBR-019", "Eduardo", "Salvador", "Mario", "eduardo.salvador@email.com", "+639189234567", "Male", "May-ari ng Carinderia", 75000, 1972, "Quezon City", true),
            ("MBR-020", "Rosario", "Aguilar", null, "rosario.aguilar@email.com", "+639190234567", "Female", "Mananahi", 20000, 1986, "Pateros", true),
            
            // Skilled workers
            ("MBR-021", "Carlos", "Ocampo", "Rafael", "carlos.ocampo@email.com", "+639191234567", "Male", "Karpintero", 22000, 1980, "Las Pinas", true),
            ("MBR-022", "Jocelyn", "Manalo", null, "jocelyn.manalo@email.com", "+639192234567", "Female", "Elektrisyan", 30000, 1985, "Muntinlupa", true),
            ("MBR-023", "Rolando", "Padilla", "Sergio", "rolando.padilla@email.com", "+639193234567", "Male", "Mekaniko", 25000, 1978, "Valenzuela", true),
            ("MBR-024", "Maricel", "Tolentino", null, "maricel.tolentino@email.com", "+639194234567", "Female", "Tubero", 26000, 1983, "Malabon", true),
            ("MBR-025", "Ernesto", "Velasco", "Arturo", "ernesto.velasco@email.com", "+639195234567", "Male", "Welder", 28000, 1981, "Navotas", true),
            
            // Transport sector
            ("MBR-026", "Dante", "Francisco", "Lorenzo", "dante.francisco@email.com", "+639196234567", "Male", "Drayber ng Taxi", 18000, 1977, "Cubao", true),
            ("MBR-027", "Nelia", "Ignacio", null, "nelia.ignacio@email.com", "+639197234567", "Female", "Drayber ng Bus", 22000, 1982, "Pasay", true),
            ("MBR-028", "Marcelo", "Lorenzo", "Benjamin", "marcelo.lorenzo@email.com", "+639198234567", "Male", "Drayber ng Truck", 30000, 1975, "Calamba", true),
            ("MBR-029", "Karen", "Morales", null, "karen.morales@email.com", "+639199234567", "Female", "Courier", 15000, 1989, "Antipolo", true),
            ("MBR-030", "Joel", "Sta. Maria", "Miguel", "joel.stamaria@email.com", "+639200234567", "Male", "Habal-habal Driver", 12000, 1991, "Rizal", true),
            
            // Young entrepreneurs
            ("MBR-031", "Alyssa", "Dimaculangan", null, "alyssa.dimaculangan@email.com", "+639201234567", "Female", "Tech Startup Owner", 60000, 1995, "BGC", true),
            ("MBR-032", "Marco", "Villareal", "Francis", "marco.villareal@email.com", "+639202234567", "Male", "Digital Marketing", 65000, 1993, "Makati", true),
            ("MBR-033", "Denise", "Catalan", null, "denise.catalan@email.com", "+639203234567", "Female", "Online Seller", 50000, 1994, "Pasig", true),
            ("MBR-034", "Paolo", "Evangelista", "Victor", "paolo.evangelista@email.com", "+639204234567", "Male", "App Developer", 85000, 1992, "Ortigas", true),
            ("MBR-035", "Samantha", "Lacson", null, "samantha.lacson@email.com", "+639205234567", "Female", "Social Media Manager", 40000, 1996, "Quezon City", true),
            
            // Artisans
            ("MBR-036", "Andres", "Bonifacio", "Roberto", "andres.bonifacio@email.com", "+639206234567", "Male", "Magpapalayok", 14000, 1979, "Vigan", true),
            ("MBR-037", "Angeline", "Magsaysay", null, "angeline.magsaysay@email.com", "+639207234567", "Female", "Manghahabi", 12000, 1984, "Ilocos Sur", true),
            ("MBR-038", "Jaime", "Laurel", "Daniel", "jaime.laurel@email.com", "+639208234567", "Male", "Panday", 16000, 1976, "Batangas", true),
            ("MBR-039", "Katrina", "Roxas", null, "katrina.roxas@email.com", "+639209234567", "Female", "Alaheriya", 28000, 1983, "Meycauayan", true),
            ("MBR-040", "Kevin", "Osmeña", "Antonio", "kevin.osmena@email.com", "+639210234567", "Male", "Sculptor", 20000, 1980, "Paete", true),
            
            // Inactive members for testing status filters
            ("MBR-041", "Mariano", "Quezon", "Almario", "mariano.quezon@email.com", "+639211234567", "Male", "Retirado", 8000, 1955, "Baler", false),
            ("MBR-042", "Sharon", "Estrada", null, "sharon.estrada@email.com", "+639212234567", "Female", "Maybahay", 0, 1965, "Cavite", false),
            ("MBR-043", "Bryan", "Macapagal", "Leo", "bryan.macapagal@email.com", "+639213234567", "Male", "Walang Trabaho", 0, 1990, "Pampanga", false),
            ("MBR-044", "Michelle", "Arroyo", null, "michelle.arroyo@email.com", "+639214234567", "Female", "Estudyante", 0, 1998, "Diliman", false),
            ("MBR-045", "Edwin", "Marcos", "Juan", "edwin.marcos@email.com", "+639215234567", "Male", "OFW", 0, 1985, "Ilocos Norte", false),
            
            // More active diverse members
            ("MBR-046", "Lourdes", "Enrile", null, "lourdes.enrile@email.com", "+639216234567", "Female", "Hotel Manager", 60000, 1981, "Boracay", true),
            ("MBR-047", "Ronald", "Recto", "Pedro", "ronald.recto@email.com", "+639217234567", "Male", "Chef", 40000, 1978, "Tagaytay", true),
            ("MBR-048", "Dina", "Sotto", null, "dina.sotto@email.com", "+639218234567", "Female", "Florist", 18000, 1986, "Baguio", true),
            ("MBR-049", "Tomas", "Binay", "Jose", "tomas.binay@email.com", "+639219234567", "Male", "Guwardiya", 15000, 1977, "Makati", true),
            ("MBR-050", "Cynthia", "Cayetano", null, "cynthia.cayetano@email.com", "+639220234567", "Female", "Real Estate Agent", 75000, 1980, "Taguig", true),
            
            // Additional 50 members for comprehensive testing
            ("MBR-051", "Benjamin", "Legaspi", "Ramon", "benjamin.legaspi@email.com", "+639221234567", "Male", "Magsasaka", 14000, 1976, "Pampanga", true),
            ("MBR-052", "Teresita", "Maceda", null, "teresita.maceda@email.com", "+639222234567", "Female", "Magsasaka", 11000, 1982, "Tarlac", true),
            ("MBR-053", "Artemio", "Ventura", "Carlos", "artemio.ventura@email.com", "+639223234567", "Male", "Mangingisda", 13000, 1979, "Bataan", true),
            ("MBR-054", "Florinda", "Pascua", null, "florinda.pascua@email.com", "+639224234567", "Female", "Mangingisda", 12000, 1985, "Zambales", true),
            ("MBR-055", "Gregorio", "Abad", "Manuel", "gregorio.abad@email.com", "+639225234567", "Male", "Magsasaka", 15000, 1973, "Cagayan", true),
            
            ("MBR-056", "Corazon", "Diokno", null, "corazon.diokno@email.com", "+639226234567", "Female", "Guro", 26000, 1988, "Laguna", true),
            ("MBR-057", "Feliciano", "Sison", "Andres", "feliciano.sison@email.com", "+639227234567", "Male", "Guro", 27000, 1986, "Batangas", true),
            ("MBR-058", "Dolores", "Palma", null, "dolores.palma@email.com", "+639228234567", "Female", "Librarian", 24000, 1981, "Cavite", true),
            ("MBR-059", "Herminio", "Cojuangco", "Luis", "herminio.cojuangco@email.com", "+639229234567", "Male", "School Administrator", 55000, 1974, "Manila", true),
            ("MBR-060", "Imelda", "Tantoco", null, "imelda.tantoco@email.com", "+639230234567", "Female", "Daycare Worker", 18000, 1990, "Pasig", true),
            
            ("MBR-061", "Jaime", "Locsin", "Pedro", "jaime.locsin@email.com", "+639231234567", "Male", "Midwife", 28000, 1983, "Batangas", true),
            ("MBR-062", "Leticia", "Araneta", null, "leticia.araneta@email.com", "+639232234567", "Female", "Dentista", 80000, 1977, "Makati", true),
            ("MBR-063", "Narciso", "Yulo", "Fernando", "narciso.yulo@email.com", "+639233234567", "Male", "Physical Therapist", 45000, 1984, "Quezon City", true),
            ("MBR-064", "Olivia", "Ponce", null, "olivia.ponce@email.com", "+639234234567", "Female", "Caregiver", 22000, 1986, "Paranaque", true),
            ("MBR-065", "Perfecto", "Montinola", "Jose", "perfecto.montinola@email.com", "+639235234567", "Male", "Medical Technologist", 40000, 1980, "Mandaluyong", true),
            
            ("MBR-066", "Quirino", "Cuenco", "Miguel", "quirino.cuenco@email.com", "+639236234567", "Male", "Tindero sa Palengke", 25000, 1978, "Divisoria", true),
            ("MBR-067", "Rosalia", "Lim", null, "rosalia.lim@email.com", "+639237234567", "Female", "May-ari ng Tindahan", 45000, 1975, "Binondo", true),
            ("MBR-068", "Salvador", "Yuchengco", "Antonio", "salvador.yuchengco@email.com", "+639238234567", "Male", "Supplier", 70000, 1972, "Pasay", true),
            ("MBR-069", "Teofilo", "Ang", null, "teofilo.ang@email.com", "+639239234567", "Male", "Wholesaler", 85000, 1970, "Manila", true),
            ("MBR-070", "Ursula", "Zobel", null, "ursula.zobel@email.com", "+639240234567", "Female", "Boutique Owner", 55000, 1982, "BGC", true),
            
            ("MBR-071", "Vicente", "Madrigal", "Ramon", "vicente.madrigal@email.com", "+639241234567", "Male", "Mason", 20000, 1979, "Bulacan", true),
            ("MBR-072", "Wilma", "Concepcion", null, "wilma.concepcion@email.com", "+639242234567", "Female", "Seamstress", 18000, 1984, "Marikina", true),
            ("MBR-073", "Xyrus", "Romualdez", "Jose", "xyrus.romualdez@email.com", "+639243234567", "Male", "Auto Mechanic", 28000, 1981, "Pasig", true),
            ("MBR-074", "Yolanda", "Prieto", null, "yolanda.prieto@email.com", "+639244234567", "Female", "Beautician", 22000, 1987, "Quezon City", true),
            ("MBR-075", "Zosimo", "Elizalde", "Fernando", "zosimo.elizalde@email.com", "+639245234567", "Male", "Painter", 19000, 1980, "Caloocan", true),
            
            ("MBR-076", "Amado", "Guzman", "Luis", "amado.guzman@email.com", "+639246234567", "Male", "Tricycle Driver", 14000, 1976, "Quezon City", true),
            ("MBR-077", "Bella", "Ortigas", null, "bella.ortigas@email.com", "+639247234567", "Female", "Jeepney Operator", 35000, 1973, "Pasig", true),
            ("MBR-078", "Cesar", "Ayala", "Antonio", "cesar.ayala@email.com", "+639248234567", "Male", "Delivery Rider", 20000, 1992, "Makati", true),
            ("MBR-079", "Diana", "Lopez", null, "diana.lopez@email.com", "+639249234567", "Female", "Grab Driver", 25000, 1988, "Taguig", true),
            ("MBR-080", "Emilio", "Tuazon", "Manuel", "emilio.tuazon@email.com", "+639250234567", "Male", "Angkas Rider", 18000, 1993, "Mandaluyong", true),
            
            ("MBR-081", "Felisa", "Sy", null, "felisa.sy@email.com", "+639251234567", "Female", "Freelance Writer", 35000, 1991, "BGC", true),
            ("MBR-082", "Gaspar", "Tan", "Alfredo", "gaspar.tan@email.com", "+639252234567", "Male", "Graphic Designer", 40000, 1990, "Makati", true),
            ("MBR-083", "Helena", "Go", null, "helena.go@email.com", "+639253234567", "Female", "Virtual Assistant", 30000, 1994, "Quezon City", true),
            ("MBR-084", "Isidro", "Ong", "Ricardo", "isidro.ong@email.com", "+639254234567", "Male", "Web Developer", 75000, 1989, "Ortigas", true),
            ("MBR-085", "Josefa", "Chua", null, "josefa.chua@email.com", "+639255234567", "Female", "Content Creator", 55000, 1995, "Pasig", true),
            
            ("MBR-086", "Lorenzo", "Lao", "Benjamin", "lorenzo.lao@email.com", "+639256234567", "Male", "Woodcarver", 16000, 1977, "Paete", true),
            ("MBR-087", "Marina", "Co", null, "marina.co@email.com", "+639257234567", "Female", "Basket Weaver", 11000, 1980, "Bohol", true),
            ("MBR-088", "Nestor", "Wu", "Carlos", "nestor.wu@email.com", "+639258234567", "Male", "Furniture Maker", 25000, 1975, "Cebu", true),
            ("MBR-089", "Ofelia", "Luy", null, "ofelia.luy@email.com", "+639259234567", "Female", "Embroiderer", 13000, 1983, "Pampanga", true),
            ("MBR-090", "Patricio", "Chan", "Eduardo", "patricio.chan@email.com", "+639260234567", "Male", "Pottery Maker", 15000, 1978, "Vigan", true),
            
            // Inactive members for more testing variety
            ("MBR-091", "Remedios", "Aboitiz", null, "remedios.aboitiz@email.com", "+639261234567", "Female", "Retired Teacher", 10000, 1958, "Cebu", false),
            ("MBR-092", "Santiago", "Gokongwei", "Jose", "santiago.gokongwei@email.com", "+639262234567", "Male", "Retired Engineer", 15000, 1955, "Manila", false),
            ("MBR-093", "Teodora", "Razon", null, "teodora.razon@email.com", "+639263234567", "Female", "Pensioner", 8000, 1952, "Laguna", false),
            ("MBR-094", "Ulises", "Pangilinan", "Ramon", "ulises.pangilinan@email.com", "+639264234567", "Male", "Disabled", 5000, 1975, "Pampanga", false),
            ("MBR-095", "Virginia", "Lucio", null, "virginia.lucio@email.com", "+639265234567", "Female", "Migrated Abroad", 0, 1985, "Quezon City", false),
            
            // More active members
            ("MBR-096", "Wenceslao", "Villar", "Pedro", "wenceslao.villar@email.com", "+639266234567", "Male", "Construction Foreman", 45000, 1976, "Las Pinas", true),
            ("MBR-097", "Ximena", "Floirendo", null, "ximena.floirendo@email.com", "+639267234567", "Female", "Travel Agent", 38000, 1984, "Davao", true),
            ("MBR-098", "Ysabel", "Alcantara", "Maria", "ysabel.alcantara@email.com", "+639268234567", "Female", "Insurance Agent", 50000, 1982, "Cebu", true),
            ("MBR-099", "Zenon", "Escudero", "Luis", "zenon.escudero@email.com", "+639269234567", "Male", "Bank Teller", 28000, 1988, "Makati", true),
            ("MBR-100", "Adelina", "Soriano", null, "adelina.soriano@email.com", "+639270234567", "Female", "Accountant", 55000, 1983, "Ortigas", true),
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
