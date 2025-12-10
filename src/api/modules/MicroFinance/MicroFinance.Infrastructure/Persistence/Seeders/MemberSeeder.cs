using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for members with comprehensive test data.
/// Creates 250 members with diverse demographics for realistic demo database.
/// </summary>
internal static class MemberSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 250;
        var existingCount = await context.Members.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = new (string Num, string First, string Last, string? Middle, string Email, string Phone, string Gender, string Occupation, decimal Income, int BirthYear, bool IsActive)[]
        {
            // Active farmers - Agricultural loan candidates
            ("MBR-001", "Juan", "Dela Cruz", "Miguel", "juan.delacruz@email.com", "+639171234567", "Male", "Magsasaka", 15000, 1985, true),
            ("MBR-002", "Maria", "Santos", null, "maria.santos@email.com", "+639172234567", "Female", "Magsasaka", 12000, 1980, true),
            ("MBR-003", "Pedro", "Reyes", "Jose", "pedro.reyes@email.com", "+639173234567", "Male", "Magsasaka", 18000, 1975, true),
            ("MBR-004", "Rosa", "Garcia", null, "rosa.garcia@email.com", "+639174234567", "Female", "Magsasaka", 10000, 1988, true),
            ("MBR-005", "Antonio", "Ramos", "Carlos", "antonio.ramos@email.com", "+639175234567", "Male", "Magsasaka", 16000, 1982, true),
            
            // Teachers & Education sector
            ("MBR-006", "Ana", "Mendoza", null, "ana.mendoza@email.com", "+639176234567", "Female", "Guro", 25000, 1990, true),
            ("MBR-007", "Jose", "Torres", "Ramon", "jose.torres@email.com", "+639177234567", "Male", "Guro", 28000, 1987, true),
            ("MBR-008", "Lorna", "Aquino", null, "lorna.aquino@email.com", "+639178234567", "Female", "Head Teacher", 45000, 1978, true),
            ("MBR-009", "Ricardo", "Villanueva", "Eduardo", "ricardo.villanueva@email.com", "+639179234567", "Male", "Propesor", 65000, 1970, true),
            ("MBR-010", "Cristina", "Bautista", null, "cristina.bautista@email.com", "+639180234567", "Female", "Guro", 22000, 1992, true),
            
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
            ("MBR-036", "Andres", "Bonifacio", "Roberto", "andres.bonifacio@email.com", "+639206234567", "Male", "Magpapalayok", 14000, 1979, true),
            ("MBR-037", "Angeline", "Magsaysay", null, "angeline.magsaysay@email.com", "+639207234567", "Female", "Manghahabi", 12000, 1984, true),
            ("MBR-038", "Jaime", "Laurel", "Daniel", "jaime.laurel@email.com", "+639208234567", "Male", "Panday", 16000, 1976, true),
            ("MBR-039", "Katrina", "Roxas", null, "katrina.roxas@email.com", "+639209234567", "Female", "Alaheriya", 28000, 1983, true),
            ("MBR-040", "Kevin", "Osmeña", "Antonio", "kevin.osmena@email.com", "+639210234567", "Male", "Sculptor", 20000, 1980, true),
            
            // Inactive members for testing status filters
            ("MBR-041", "Mariano", "Quezon", "Almario", "mariano.quezon@email.com", "+639211234567", "Male", "Retirado", 8000, 1955, false),
            ("MBR-042", "Sharon", "Estrada", null, "sharon.estrada@email.com", "+639212234567", "Female", "Maybahay", 0, 1965, false),
            ("MBR-043", "Bryan", "Macapagal", "Leo", "bryan.macapagal@email.com", "+639213234567", "Male", "Walang Trabaho", 0, 1990, false),
            ("MBR-044", "Michelle", "Arroyo", null, "michelle.arroyo@email.com", "+639214234567", "Female", "Estudyante", 0, 1998, false),
            ("MBR-045", "Edwin", "Marcos", "Juan", "edwin.marcos@email.com", "+639215234567", "Male", "OFW", 0, 1985, false),
            
            // More active diverse members
            ("MBR-046", "Lourdes", "Enrile", null, "lourdes.enrile@email.com", "+639216234567", "Female", "Hotel Manager", 60000, 1981, true),
            ("MBR-047", "Ronald", "Recto", "Pedro", "ronald.recto@email.com", "+639217234567", "Male", "Chef", 40000, 1978, true),
            ("MBR-048", "Dina", "Sotto", null, "dina.sotto@email.com", "+639218234567", "Female", "Florist", 18000, 1986, true),
            ("MBR-049", "Tomas", "Binay", "Jose", "tomas.binay@email.com", "+639219234567", "Male", "Guwardiya", 15000, 1977, true),
            ("MBR-050", "Cynthia", "Cayetano", null, "cynthia.cayetano@email.com", "+639220234567", "Female", "Real Estate Agent", 75000, 1980, true),
            
            // Additional 50 members for comprehensive testing
            ("MBR-051", "Benjamin", "Legaspi", "Ramon", "benjamin.legaspi@email.com", "+639221234567", "Male", "Magsasaka", 14000, 1976, true),
            ("MBR-052", "Teresita", "Maceda", null, "teresita.maceda@email.com", "+639222234567", "Female", "Magsasaka", 11000, 1982, true),
            ("MBR-053", "Artemio", "Ventura", "Carlos", "artemio.ventura@email.com", "+639223234567", "Male", "Mangingisda", 13000, 1979, true),
            ("MBR-054", "Florinda", "Pascua", null, "florinda.pascua@email.com", "+639224234567", "Female", "Mangingisda", 12000, 1985, true),
            ("MBR-055", "Gregorio", "Abad", "Manuel", "gregorio.abad@email.com", "+639225234567", "Male", "Magsasaka", 15000, 1973, true),
            
            ("MBR-056", "Corazon", "Diokno", null, "corazon.diokno@email.com", "+639226234567", "Female", "Guro", 26000, 1988, true),
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
            
            // Additional 150 members for comprehensive demo (MBR-101 to MBR-250)
            // Farmers and Fishermen
            ("MBR-101", "Arcadio", "Abalos", "Jose", "arcadio.abalos@email.com", "+639271234567", "Male", "Rice Farmer", 14000, 1974, "Nueva Ecija", true),
            ("MBR-102", "Benedicta", "Bagtas", null, "benedicta.bagtas@email.com", "+639272234567", "Female", "Vegetable Farmer", 12000, 1979, "Benguet", true),
            ("MBR-103", "Cornelio", "Cabrera", "Manuel", "cornelio.cabrera@email.com", "+639273234567", "Male", "Coconut Farmer", 13000, 1976, "Quezon", true),
            ("MBR-104", "Dionisia", "Delos Reyes", null, "dionisia.delosreyes@email.com", "+639274234567", "Female", "Banana Farmer", 15000, 1982, "Davao", true),
            ("MBR-105", "Evaristo", "Enriquez", "Pedro", "evaristo.enriquez@email.com", "+639275234567", "Male", "Fisherman", 11000, 1978, "Iloilo", true),
            ("MBR-106", "Filomena", "Fajardo", null, "filomena.fajardo@email.com", "+639276234567", "Female", "Seaweed Farmer", 10000, 1985, "Zamboanga", true),
            ("MBR-107", "Gerardo", "Galvez", "Antonio", "gerardo.galvez@email.com", "+639277234567", "Male", "Sugar Cane Farmer", 16000, 1972, "Negros Occidental", true),
            ("MBR-108", "Herminia", "Hidalgo", null, "herminia.hidalgo@email.com", "+639278234567", "Female", "Poultry Farmer", 18000, 1980, "Bulacan", true),
            ("MBR-109", "Igmidio", "Ilagan", "Francisco", "igmidio.ilagan@email.com", "+639279234567", "Male", "Hog Farmer", 20000, 1977, "Batangas", true),
            ("MBR-110", "Josefina", "Jimenez", null, "josefina.jimenez@email.com", "+639280234567", "Female", "Duck Farmer", 14000, 1983, "Pampanga", true),
            
            // Factory and Manufacturing Workers
            ("MBR-111", "Kosme", "Kasim", "Andres", "kosme.kasim@email.com", "+639281234567", "Male", "Factory Worker", 16000, 1986, "Cavite", true),
            ("MBR-112", "Leoncia", "Lazaro", null, "leoncia.lazaro@email.com", "+639282234567", "Female", "Garment Worker", 14000, 1988, "Laguna", true),
            ("MBR-113", "Maximo", "Manansala", "Roberto", "maximo.manansala@email.com", "+639283234567", "Male", "Electronics Assembler", 18000, 1984, "Calabarzon", true),
            ("MBR-114", "Nimfa", "Natividad", null, "nimfa.natividad@email.com", "+639284234567", "Female", "Quality Control", 20000, 1982, "Bataan", true),
            ("MBR-115", "Octavio", "Ocampo", "Luis", "octavio.ocampo@email.com", "+639285234567", "Male", "Machine Operator", 17000, 1979, "Bulacan", true),
            ("MBR-116", "Prudencia", "Platon", null, "prudencia.platon@email.com", "+639286234567", "Female", "Packaging Worker", 15000, 1987, "Cavite", true),
            ("MBR-117", "Quirico", "Quintos", "Eduardo", "quirico.quintos@email.com", "+639287234567", "Male", "Forklift Operator", 19000, 1981, "Laguna", true),
            ("MBR-118", "Rufina", "Ramos", null, "rufina.ramos@email.com", "+639288234567", "Female", "Food Processing", 16000, 1985, "Pampanga", true),
            ("MBR-119", "Silvino", "Salazar", "Mariano", "silvino.salazar@email.com", "+639289234567", "Male", "Welder", 22000, 1978, "Batangas", true),
            ("MBR-120", "Trinidad", "Tan", null, "trinidad.tan@email.com", "+639290234567", "Female", "Assembly Line Lead", 25000, 1976, "Rizal", true),
            
            // Service Industry Workers
            ("MBR-121", "Ulpiano", "Umali", "Jose", "ulpiano.umali@email.com", "+639291234567", "Male", "Restaurant Server", 15000, 1990, "Manila", true),
            ("MBR-122", "Violeta", "Valdez", null, "violeta.valdez@email.com", "+639292234567", "Female", "Hotel Housekeeper", 14000, 1988, "Makati", true),
            ("MBR-123", "Wilfredo", "Wenceslao", "Carlos", "wilfredo.wenceslao@email.com", "+639293234567", "Male", "Security Guard", 16000, 1980, "Quezon City", true),
            ("MBR-124", "Xenia", "Xyza", null, "xenia.xyza@email.com", "+639294234567", "Female", "Mall Saleslady", 15000, 1992, "Pasig", true),
            ("MBR-125", "Ysmael", "Yabut", "Fernando", "ysmael.yabut@email.com", "+639295234567", "Male", "Janitor", 13000, 1975, "Mandaluyong", true),
            ("MBR-126", "Zenaida", "Zuniga", null, "zenaida.zuniga@email.com", "+639296234567", "Female", "Call Center Agent", 25000, 1989, "BGC", true),
            ("MBR-127", "Alfonso", "Aldaba", "Miguel", "alfonso.aldaba@email.com", "+639297234567", "Male", "BPO Team Lead", 40000, 1985, "Makati", true),
            ("MBR-128", "Brigida", "Borja", null, "brigida.borja@email.com", "+639298234567", "Female", "Fast Food Crew", 12000, 1995, "Quezon City", true),
            ("MBR-129", "Candido", "Concepcion", "Ramon", "candido.concepcion@email.com", "+639299234567", "Male", "Casino Dealer", 22000, 1987, "Paranaque", true),
            ("MBR-130", "Damiana", "Dimayuga", null, "damiana.dimayuga@email.com", "+639300234567", "Female", "Spa Therapist", 18000, 1986, "Tagaytay", true),
            
            // Healthcare and Caregivers
            ("MBR-131", "Eleuterio", "Estacio", "Juan", "eleuterio.estacio@email.com", "+639301234567", "Male", "Caregiver", 20000, 1982, "Manila", true),
            ("MBR-132", "Felicidad", "Florendo", null, "felicidad.florendo@email.com", "+639302234567", "Female", "Midwife", 25000, 1978, "Laguna", true),
            ("MBR-133", "Gaudencio", "Gutierrez", "Pedro", "gaudencio.gutierrez@email.com", "+639303234567", "Male", "Medical Aide", 18000, 1984, "Batangas", true),
            ("MBR-134", "Horacia", "Hernandez", null, "horacia.hernandez@email.com", "+639304234567", "Female", "Nursing Aide", 17000, 1986, "Cavite", true),
            ("MBR-135", "Ireneo", "Ignacio", "Antonio", "ireneo.ignacio@email.com", "+639305234567", "Male", "Physical Therapy Aide", 19000, 1981, "Quezon City", true),
            ("MBR-136", "Juliana", "Javier", null, "juliana.javier@email.com", "+639306234567", "Female", "Pharmacy Aide", 16000, 1988, "Makati", true),
            ("MBR-137", "Karlo", "Kapunan", "Luis", "karlo.kapunan@email.com", "+639307234567", "Male", "Ambulance Driver", 18000, 1979, "Pasig", true),
            ("MBR-138", "Leonora", "Lacson", null, "leonora.lacson@email.com", "+639308234567", "Female", "Dental Assistant", 17000, 1985, "Mandaluyong", true),
            ("MBR-139", "Melchor", "Macapagal", "Jose", "melchor.macapagal@email.com", "+639309234567", "Male", "Lab Aide", 16000, 1983, "Taguig", true),
            ("MBR-140", "Natividad", "Navarro", null, "natividad.navarro@email.com", "+639310234567", "Female", "Hospital Clerk", 18000, 1980, "Paranaque", true),
            
            // Construction and Trade Workers
            ("MBR-141", "Osmundo", "Orense", "Francisco", "osmundo.orense@email.com", "+639311234567", "Male", "Carpenter", 20000, 1977, "Bulacan", true),
            ("MBR-142", "Pascuala", "Pineda", null, "pascuala.pineda@email.com", "+639312234567", "Female", "Tile Setter", 18000, 1982, "Rizal", true),
            ("MBR-143", "Raymundo", "Rosal", "Miguel", "raymundo.rosal@email.com", "+639313234567", "Male", "Plumber", 22000, 1979, "Laguna", true),
            ("MBR-144", "Salome", "Sarmiento", null, "salome.sarmiento@email.com", "+639314234567", "Female", "Interior Painter", 17000, 1984, "Cavite", true),
            ("MBR-145", "Teodoro", "Tanedo", "Andres", "teodoro.tanedo@email.com", "+639315234567", "Male", "Electrician", 25000, 1976, "Batangas", true),
            ("MBR-146", "Urania", "Ubaldo", null, "urania.ubaldo@email.com", "+639316234567", "Female", "Steel Worker", 20000, 1981, "Pampanga", true),
            ("MBR-147", "Venancio", "Viray", "Roberto", "venancio.viray@email.com", "+639317234567", "Male", "Heavy Equipment Operator", 30000, 1974, "Zambales", true),
            ("MBR-148", "Winifreda", "Wong", null, "winifreda.wong@email.com", "+639318234567", "Female", "Construction Clerk", 18000, 1987, "Tarlac", true),
            ("MBR-149", "Xerxes", "Ybarra", "Carlos", "xerxes.ybarra@email.com", "+639319234567", "Male", "Scaffolder", 19000, 1980, "Nueva Ecija", true),
            ("MBR-150", "Yolly", "Yumang", null, "yolly.yumang@email.com", "+639320234567", "Female", "Safety Officer", 28000, 1978, "Bulacan", true),
            
            // Small Business Owners - Sari-Sari Store
            ("MBR-151", "Zacarias", "Zamora", "Pedro", "zacarias.zamora@email.com", "+639321234567", "Male", "Sari-Sari Store Owner", 25000, 1975, "Quezon City", true),
            ("MBR-152", "Adelaida", "Agbayani", null, "adelaida.agbayani@email.com", "+639322234567", "Female", "Sari-Sari Store Owner", 22000, 1980, "Caloocan", true),
            ("MBR-153", "Bernabe", "Bello", "Juan", "bernabe.bello@email.com", "+639323234567", "Male", "Sari-Sari Store Owner", 20000, 1983, "Valenzuela", true),
            ("MBR-154", "Clarita", "Castaneda", null, "clarita.castaneda@email.com", "+639324234567", "Female", "Sari-Sari Store Owner", 24000, 1978, "Malabon", true),
            ("MBR-155", "Demetrio", "Delos Santos", "Antonio", "demetrio.delossantos@email.com", "+639325234567", "Male", "Sari-Sari Store Owner", 26000, 1976, "Navotas", true),
            
            // Small Business Owners - Food
            ("MBR-156", "Eugenia", "Escobar", null, "eugenia.escobar@email.com", "+639326234567", "Female", "Carinderia Owner", 35000, 1974, "Manila", true),
            ("MBR-157", "Florante", "Felipe", "Luis", "florante.felipe@email.com", "+639327234567", "Male", "Food Cart Vendor", 28000, 1981, "Makati", true),
            ("MBR-158", "Glenda", "Galang", null, "glenda.galang@email.com", "+639328234567", "Female", "Bakery Owner", 40000, 1977, "Quezon City", true),
            ("MBR-159", "Hermogenes", "Hilario", "Miguel", "hermogenes.hilario@email.com", "+639329234567", "Male", "Street Food Vendor", 20000, 1985, "Pasig", true),
            ("MBR-160", "Inocencia", "Imperial", null, "inocencia.imperial@email.com", "+639330234567", "Female", "Kakanin Seller", 18000, 1982, "Marikina", true),
            
            // Transport Operators
            ("MBR-161", "Juanito", "Jimenez", "Pedro", "juanito.jimenez@email.com", "+639331234567", "Male", "Tricycle Operator", 15000, 1979, "Rizal", true),
            ("MBR-162", "Katrina", "Kalaw", null, "katrina.kalaw@email.com", "+639332234567", "Female", "Jeepney Owner", 40000, 1973, "Laguna", true),
            ("MBR-163", "Lamberto", "Lacsamana", "Jose", "lamberto.lacsamana@email.com", "+639333234567", "Male", "Multicab Operator", 30000, 1976, "Cavite", true),
            ("MBR-164", "Marilou", "Magat", null, "marilou.magat@email.com", "+639334234567", "Female", "UV Express Operator", 45000, 1978, "Batangas", true),
            ("MBR-165", "Nicanor", "Nabua", "Antonio", "nicanor.nabua@email.com", "+639335234567", "Male", "Taxi Operator", 50000, 1974, "Manila", true),
            
            // Professionals - Mid Level
            ("MBR-166", "Olivia", "Ordinario", null, "olivia.ordinario@email.com", "+639336234567", "Female", "HR Assistant", 28000, 1989, "Makati", true),
            ("MBR-167", "Policarpio", "Pascual", "Ramon", "policarpio.pascual@email.com", "+639337234567", "Male", "IT Support", 32000, 1986, "BGC", true),
            ("MBR-168", "Rosenda", "Roque", null, "rosenda.roque@email.com", "+639338234567", "Female", "Marketing Assistant", 30000, 1988, "Ortigas", true),
            ("MBR-169", "Silverio", "Soriano", "Carlos", "silverio.soriano@email.com", "+639339234567", "Male", "Sales Representative", 35000, 1984, "Makati", true),
            ("MBR-170", "Teofila", "Tagalog", null, "teofila.tagalog@email.com", "+639340234567", "Female", "Executive Secretary", 38000, 1981, "Pasig", true),
            
            // Young Professionals - Entry Level
            ("MBR-171", "Urbano", "Umipig", "Fernando", "urbano.umipig@email.com", "+639341234567", "Male", "Junior Developer", 30000, 1995, "Quezon City", true),
            ("MBR-172", "Veronica", "Villafuerte", null, "veronica.villafuerte@email.com", "+639342234567", "Female", "Junior Accountant", 25000, 1996, "Mandaluyong", true),
            ("MBR-173", "Waldo", "Wagan", "Luis", "waldo.wagan@email.com", "+639343234567", "Male", "Junior Designer", 28000, 1994, "Taguig", true),
            ("MBR-174", "Xena", "Ximenes", null, "xena.ximenes@email.com", "+639344234567", "Female", "Junior Analyst", 27000, 1997, "Makati", true),
            ("MBR-175", "Yuri", "Yalung", "Pedro", "yuri.yalung@email.com", "+639345234567", "Male", "Fresh Graduate", 22000, 1999, "Manila", true),
            
            // Online Sellers and Digital Economy
            ("MBR-176", "Zosima", "Zabala", null, "zosima.zabala@email.com", "+639346234567", "Female", "Online Seller", 35000, 1991, "Cavite", true),
            ("MBR-177", "Arturo", "Andrada", "Miguel", "arturo.andrada@email.com", "+639347234567", "Male", "Online Seller", 40000, 1989, "Laguna", true),
            ("MBR-178", "Bienvenida", "Bulaon", null, "bienvenida.bulaon@email.com", "+639348234567", "Female", "Reseller", 28000, 1993, "Batangas", true),
            ("MBR-179", "Crisanto", "Cabangon", "Jose", "crisanto.cabangon@email.com", "+639349234567", "Male", "Dropshipper", 25000, 1994, "Rizal", true),
            ("MBR-180", "Dalisay", "Dagohoy", null, "dalisay.dagohoy@email.com", "+639350234567", "Female", "Food Delivery Partner", 20000, 1992, "Quezon City", true),
            
            // Artisans and Craftspeople
            ("MBR-181", "Efren", "Escondo", "Antonio", "efren.escondo@email.com", "+639351234567", "Male", "Woodcraft Maker", 18000, 1980, "Pampanga", true),
            ("MBR-182", "Felicisima", "Fernandez", null, "felicisima.fernandez@email.com", "+639352234567", "Female", "Native Bag Maker", 15000, 1983, "Ilocos Sur", true),
            ("MBR-183", "Gumersindo", "Gabino", "Luis", "gumersindo.gabino@email.com", "+639353234567", "Male", "Bamboo Craft Maker", 14000, 1978, "Bohol", true),
            ("MBR-184", "Hilaria", "Hontiveros", null, "hilaria.hontiveros@email.com", "+639354234567", "Female", "Handicraft Seller", 16000, 1981, "Cebu", true),
            ("MBR-185", "Isagani", "Ilustre", "Pedro", "isagani.ilustre@email.com", "+639355234567", "Male", "Shell Craft Artisan", 17000, 1979, "Palawan", true),
            
            // More Inactive Members for Testing
            ("MBR-186", "Jocosa", "Joson", null, "jocosa.joson@email.com", "+639356234567", "Female", "Retired Nurse", 12000, 1960, "Manila", false),
            ("MBR-187", "Kabigting", "Katipunan", "Jose", "kabigting.katipunan@email.com", "+639357234567", "Male", "Retired Military", 15000, 1958, "Quezon City", false),
            ("MBR-188", "Ludivina", "Labayen", null, "ludivina.labayen@email.com", "+639358234567", "Female", "Deceased", 0, 1950, "Laguna", false),
            ("MBR-189", "Macario", "Mactal", "Antonio", "macario.mactal@email.com", "+639359234567", "Male", "Migrated to Canada", 0, 1982, "Pampanga", false),
            ("MBR-190", "Nora", "Narciso", null, "nora.narciso@email.com", "+639360234567", "Female", "Account Closed", 0, 1985, "Bulacan", false),
            
            // Vendors and Market Sellers
            ("MBR-191", "Olegario", "Oliveros", "Miguel", "olegario.oliveros@email.com", "+639361234567", "Male", "Market Vegetable Vendor", 22000, 1976, "Divisoria", true),
            ("MBR-192", "Perfecta", "Pangilinan", null, "perfecta.pangilinan@email.com", "+639362234567", "Female", "Fish Vendor", 20000, 1979, "Navotas", true),
            ("MBR-193", "Regino", "Reyes", "Fernando", "regino.reyes@email.com", "+639363234567", "Male", "Meat Vendor", 28000, 1974, "Malabon", true),
            ("MBR-194", "Segundina", "Santos", null, "segundina.santos@email.com", "+639364234567", "Female", "Fruit Vendor", 18000, 1982, "Quezon City", true),
            ("MBR-195", "Temistocles", "Toledo", "Luis", "temistocles.toledo@email.com", "+639365234567", "Male", "Dry Goods Vendor", 25000, 1977, "Manila", true),
            
            // More Farmers - Rice and Corn
            ("MBR-196", "Ursino", "Uson", "Pedro", "ursino.uson@email.com", "+639366234567", "Male", "Rice Farmer", 16000, 1973, "Isabela", true),
            ("MBR-197", "Victoria", "Villegas", null, "victoria.villegas@email.com", "+639367234567", "Female", "Corn Farmer", 14000, 1980, "Pangasinan", true),
            ("MBR-198", "Wenceslao", "Wanget", "Jose", "wenceslao.wanget@email.com", "+639368234567", "Male", "Rice Farmer", 15000, 1976, "Nueva Ecija", true),
            ("MBR-199", "Xandra", "Ybanez", null, "xandra.ybanez@email.com", "+639369234567", "Female", "Rice Farmer", 13000, 1984, "Cagayan", true),
            ("MBR-200", "Yoyong", "Yatco", "Antonio", "yoyong.yatco@email.com", "+639370234567", "Male", "Corn Farmer", 12000, 1979, "Tarlac", true),
            
            // Service Workers Continued
            ("MBR-201", "Zoilo", "Zapanta", "Miguel", "zoilo.zapanta@email.com", "+639371234567", "Male", "Barber", 18000, 1981, "Quezon City", true),
            ("MBR-202", "Anicia", "Almeda", null, "anicia.almeda@email.com", "+639372234567", "Female", "Laundry Service Owner", 25000, 1978, "Mandaluyong", true),
            ("MBR-203", "Buenaventura", "Basa", "Luis", "buenaventura.basa@email.com", "+639373234567", "Male", "Car Wash Owner", 30000, 1975, "Pasig", true),
            ("MBR-204", "Carlota", "Crisostomo", null, "carlota.crisostomo@email.com", "+639374234567", "Female", "Massage Therapist", 20000, 1983, "Makati", true),
            ("MBR-205", "Deogracias", "Dayrit", "Pedro", "deogracias.dayrit@email.com", "+639375234567", "Male", "Shoe Repair", 12000, 1970, "Manila", true),
            
            // More Professionals
            ("MBR-206", "Emiliana", "Encarnacion", null, "emiliana.encarnacion@email.com", "+639376234567", "Female", "Architect", 65000, 1982, "Makati", true),
            ("MBR-207", "Fulgencio", "Fabian", "Jose", "fulgencio.fabian@email.com", "+639377234567", "Male", "Civil Engineer", 55000, 1979, "BGC", true),
            ("MBR-208", "Generosa", "Gaerlan", null, "generosa.gaerlan@email.com", "+639378234567", "Female", "Lawyer", 80000, 1976, "Ortigas", true),
            ("MBR-209", "Honesto", "Habana", "Antonio", "honesto.habana@email.com", "+639379234567", "Male", "CPA", 60000, 1980, "Makati", true),
            ("MBR-210", "Isabelita", "Inigo", null, "isabelita.inigo@email.com", "+639380234567", "Female", "Dentist", 70000, 1978, "Quezon City", true),
            
            // Transportation Workers
            ("MBR-211", "Jovencio", "Japitana", "Luis", "jovencio.japitana@email.com", "+639381234567", "Male", "Truck Driver", 22000, 1977, "Valenzuela", true),
            ("MBR-212", "Katalina", "Katigbak", null, "katalina.katigbak@email.com", "+639382234567", "Female", "Dispatcher", 18000, 1985, "Caloocan", true),
            ("MBR-213", "Laurentino", "Lacuesta", "Pedro", "laurentino.lacuesta@email.com", "+639383234567", "Male", "Delivery Man", 16000, 1988, "Quezon City", true),
            ("MBR-214", "Marciana", "Macasaet", null, "marciana.macasaet@email.com", "+639384234567", "Female", "Uber Driver", 25000, 1984, "Makati", true),
            ("MBR-215", "Nemesio", "Nuela", "Jose", "nemesio.nuela@email.com", "+639385234567", "Male", "Bus Conductor", 14000, 1981, "Pasay", true),
            
            // Home-based Workers
            ("MBR-216", "Onofre", "Orbos", "Antonio", "onofre.orbos@email.com", "+639386234567", "Male", "Freelance Programmer", 50000, 1987, "Quezon City", true),
            ("MBR-217", "Patria", "Palawan", null, "patria.palawan@email.com", "+639387234567", "Female", "Virtual Assistant", 35000, 1990, "Cavite", true),
            ("MBR-218", "Ranulfo", "Recometa", "Miguel", "ranulfo.recometa@email.com", "+639388234567", "Male", "Online Tutor", 30000, 1989, "Laguna", true),
            ("MBR-219", "Susana", "Salvacion", null, "susana.salvacion@email.com", "+639389234567", "Female", "Content Writer", 28000, 1992, "Batangas", true),
            ("MBR-220", "Timoteo", "Tiongson", "Luis", "timoteo.tiongson@email.com", "+639390234567", "Male", "Video Editor", 32000, 1988, "Rizal", true),
            
            // Students/Part-timers
            ("MBR-221", "Umbelina", "Umipig", null, "umbelina.umipig@email.com", "+639391234567", "Female", "Working Student", 8000, 1999, "Manila", true),
            ("MBR-222", "Virgilio", "Velazco", "Pedro", "virgilio.velazco@email.com", "+639392234567", "Male", "Part-time Tutor", 10000, 1998, "Quezon City", true),
            ("MBR-223", "Wilfreda", "Wagas", null, "wilfreda.wagas@email.com", "+639393234567", "Female", "Part-time Cashier", 9000, 2000, "Makati", true),
            ("MBR-224", "Xylo", "Yabut", "Jose", "xylo.yabut@email.com", "+639394234567", "Male", "Intern", 5000, 2001, "Pasig", true),
            ("MBR-225", "Yolanda", "Yuson", null, "yolanda.yuson@email.com", "+639395234567", "Female", "Working Student", 8500, 2000, "Mandaluyong", true),
            
            // Cooperative Members
            ("MBR-226", "Zosimo", "Zafra", "Antonio", "zosimo.zafra@email.com", "+639396234567", "Male", "Coop Board Member", 45000, 1970, "Quezon City", true),
            ("MBR-227", "Amalia", "Arnaldo", null, "amalia.arnaldo@email.com", "+639397234567", "Female", "Coop Member", 30000, 1975, "Manila", true),
            ("MBR-228", "Basilisa", "Bustamante", "Maria", "basilisa.bustamante@email.com", "+639398234567", "Female", "Coop Treasurer", 35000, 1972, "Makati", true),
            ("MBR-229", "Celedonio", "Cuenca", "Luis", "celedonio.cuenca@email.com", "+639399234567", "Male", "Coop Secretary", 32000, 1974, "Pasig", true),
            ("MBR-230", "Diosdada", "Diaz", null, "diosdada.diaz@email.com", "+639400234567", "Female", "Coop Member", 28000, 1978, "Ortigas", true),
            
            // Senior Citizens (Pensioners)
            ("MBR-231", "Eusebio", "Estoque", "Pedro", "eusebio.estoque@email.com", "+639401234567", "Male", "Retired Government", 18000, 1955, "Manila", true),
            ("MBR-232", "Fortunata", "Flores", null, "fortunata.flores@email.com", "+639402234567", "Female", "SSS Pensioner", 10000, 1958, "Quezon City", true),
            ("MBR-233", "Godofreda", "Gaces", "Carmen", "godofreda.gaces@email.com", "+639403234567", "Female", "GSIS Pensioner", 15000, 1953, "Makati", true),
            ("MBR-234", "Hermenegildo", "Hizon", "Jose", "hermenegildo.hizon@email.com", "+639404234567", "Male", "Retired Teacher", 12000, 1956, "Laguna", true),
            ("MBR-235", "Ildefonso", "Ibanez", "Antonio", "ildefonso.ibanez@email.com", "+639405234567", "Male", "Retired Military", 20000, 1952, "Batangas", true),
            
            // More Inactive for Balance
            ("MBR-236", "Jovita", "Jaramillo", null, "jovita.jaramillo@email.com", "+639406234567", "Female", "Dormant Account", 0, 1980, "Cavite", false),
            ("MBR-237", "Konrado", "Kapatiran", "Miguel", "konrado.kapatiran@email.com", "+639407234567", "Male", "Left Country", 0, 1978, "Pampanga", false),
            ("MBR-238", "Lolita", "Lumaban", null, "lolita.lumaban@email.com", "+639408234567", "Female", "Transferred to Other MFI", 0, 1983, "Bulacan", false),
            ("MBR-239", "Melanio", "Mabini", "Luis", "melanio.mabini@email.com", "+639409234567", "Male", "Blacklisted", 0, 1975, "Manila", false),
            ("MBR-240", "Nieves", "Nunez", null, "nieves.nunez@email.com", "+639410234567", "Female", "Voluntary Exit", 0, 1988, "Rizal", false),
            
            // Final batch of active members
            ("MBR-241", "Orlando", "Obordo", "Pedro", "orlando.obordo@email.com", "+639411234567", "Male", "Construction Worker", 18000, 1981, "Quezon City", true),
            ("MBR-242", "Purificacion", "Panganiban", null, "purificacion.panganiban@email.com", "+639412234567", "Female", "Factory Supervisor", 30000, 1976, "Cavite", true),
            ("MBR-243", "Rodrigo", "Recto", "Jose", "rodrigo.recto@email.com", "+639413234567", "Male", "Taxi Driver", 20000, 1979, "Manila", true),
            ("MBR-244", "Soledad", "Sulit", null, "soledad.sulit@email.com", "+639414234567", "Female", "Beautician", 22000, 1984, "Makati", true),
            ("MBR-245", "Tomas", "Tanada", "Antonio", "tomas.tanada@email.com", "+639415234567", "Male", "Mechanic", 25000, 1977, "Pasig", true),
            ("MBR-246", "Urduja", "Ungson", null, "urduja.ungson@email.com", "+639416234567", "Female", "Dressmaker", 18000, 1982, "Marikina", true),
            ("MBR-247", "Vivencio", "Vizconde", "Luis", "vivencio.vizconde@email.com", "+639417234567", "Male", "Electrician", 24000, 1978, "Mandaluyong", true),
            ("MBR-248", "Wilhelmina", "Wadoc", null, "wilhelmina.wadoc@email.com", "+639418234567", "Female", "Store Clerk", 15000, 1986, "Taguig", true),
            ("MBR-249", "Xander", "Yanga", "Miguel", "xander.yanga@email.com", "+639419234567", "Male", "Security Guard", 16000, 1980, "Paranaque", true),
            ("MBR-250", "Yessica", "Zapanta", null, "yessica.zapanta@email.com", "+639420234567", "Female", "Office Clerk", 18000, 1988, "Las Pinas", true),
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
                address: $"{100 + i} Main Street, Philippines",
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
        logger.LogInformation("[{Tenant}] seeded {Count} members with diverse demographics for demo database", tenant, targetCount);
    }
}
