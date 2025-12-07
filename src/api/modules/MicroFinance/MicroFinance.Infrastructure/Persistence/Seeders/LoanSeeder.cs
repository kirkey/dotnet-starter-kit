using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for loans with comprehensive test data.
/// Creates 60 loans across all statuses for testing approval, disbursement, and repayment workflows.
/// </summary>
internal static class LoanSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 60;
        var existingCount = await context.Loans.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Where(m => m.IsActive).Take(60).ToListAsync(cancellationToken).ConfigureAwait(false);
        var products = await context.LoanProducts.Where(p => p.IsActive).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 10 || products.Count < 3) return;

        // Get product references
        var personalLoan = products.FirstOrDefault(p => p.Code == "PERSONAL-LOAN") ?? products[0];
        var agriLoan = products.FirstOrDefault(p => p.Code == "AGRI-LOAN") ?? products[0];
        var microBusiness = products.FirstOrDefault(p => p.Code == "MICRO-BUSINESS") ?? products[0];
        var emergencyLoan = products.FirstOrDefault(p => p.Code == "EMERGENCY-LOAN") ?? products[0];
        var eduLoan = products.FirstOrDefault(p => p.Code == "EDU-LOAN") ?? products[0];

        var loanData = new (int MemberIdx, LoanProduct Product, decimal Amount, int Term, string Purpose, string Status)[]
        {
            // Pending loans - waiting for approval
            (0, agriLoan, 5000, 12, "Pagbili ng pataba at binhi para sa pagtatanim ng mais", "Pending"),
            (1, agriLoan, 8000, 12, "Irrigation equipment para sa gulay", "Pending"),
            (2, personalLoan, 2000, 6, "Pag-aayos ng bahay", "Pending"),
            (3, microBusiness, 10000, 24, "Pagpapalawak ng grocery store inventory", "Pending"),
            (4, emergencyLoan, 500, 3, "Medical emergency expenses", "Pending"),
            (5, personalLoan, 3500, 12, "Pagbayad ng tuition ng anak", "Pending"),
            (6, agriLoan, 7000, 12, "Pagbili ng kalabaw para sa araro", "Pending"),
            (7, microBusiness, 15000, 18, "Catering business expansion", "Pending"),
            
            // Approved loans - ready for disbursement
            (8, personalLoan, 3000, 12, "Vehicle repair at maintenance", "Approved"),
            (9, microBusiness, 15000, 36, "Pagbukas ng pangalawang tindahan", "Approved"),
            (10, eduLoan, 5000, 24, "University tuition fees", "Approved"),
            (11, agriLoan, 12000, 12, "Tractor rental at land preparation", "Approved"),
            (12, personalLoan, 1500, 6, "Gastos sa kasal ng anak", "Approved"),
            (13, microBusiness, 8000, 12, "Restaurant supplies", "Approved"),
            (14, eduLoan, 4000, 18, "Technical-Vocational training", "Approved"),
            (15, emergencyLoan, 1000, 3, "Emergency hospitalization", "Approved"),
            
            // Active/Disbursed loans - in repayment
            (16, microBusiness, 8000, 18, "Pagbili ng kagamitan sa panaderia", "Disbursed"),
            (17, agriLoan, 6000, 12, "Poultry farm expansion", "Disbursed"),
            (18, personalLoan, 4000, 12, "Motorcycle purchase para sa negosyo", "Disbursed"),
            (19, eduLoan, 8000, 36, "Professional certification program", "Disbursed"),
            (20, microBusiness, 20000, 24, "Restaurant renovation", "Disbursed"),
            (21, personalLoan, 2500, 12, "Solar panel installation", "Disbursed"),
            (22, agriLoan, 10000, 12, "Dairy cattle purchase", "Disbursed"),
            (23, microBusiness, 5000, 12, "Tailoring shop equipment", "Disbursed"),
            (24, emergencyLoan, 1000, 3, "Roof repair after storm damage", "Disbursed"),
            (25, personalLoan, 3500, 18, "Computer at office equipment", "Disbursed"),
            (26, agriLoan, 9000, 12, "Rice farming inputs", "Disbursed"),
            (27, microBusiness, 12000, 24, "Bakery equipment upgrade", "Disbursed"),
            (28, personalLoan, 2000, 9, "Home appliances purchase", "Disbursed"),
            (29, eduLoan, 6000, 24, "Masters degree tuition", "Disbursed"),
            (30, microBusiness, 18000, 36, "Grocery store franchise", "Disbursed"),
            (31, agriLoan, 7500, 12, "Fishpond inputs at feeds", "Disbursed"),
            (32, personalLoan, 4500, 18, "Wedding expenses", "Disbursed"),
            (33, microBusiness, 25000, 36, "Beauty salon renovation", "Disbursed"),
            (34, emergencyLoan, 1500, 6, "Medical operation expenses", "Disbursed"),
            (35, eduLoan, 10000, 36, "Nursing school complete tuition", "Disbursed"),
            
            // Rejected loans - for testing rejection workflow
            (36, microBusiness, 50000, 48, "Large factory expansion - exceeds limit", "Rejected"),
            (37, personalLoan, 15000, 6, "Insufficient income documentation", "Rejected"),
            (38, agriLoan, 30000, 24, "Land purchase - outside policy scope", "Rejected"),
            (39, eduLoan, 20000, 12, "No valid enrollment documents", "Rejected"),
            (40, microBusiness, 40000, 36, "Business plan not viable", "Rejected"),
            
            // More pending for variety
            (41, eduLoan, 3000, 12, "Technical training course fees", "Pending"),
            (42, microBusiness, 7500, 18, "Food truck business startup", "Pending"),
            (43, personalLoan, 1800, 9, "Furniture purchase for new home", "Pending"),
            (44, agriLoan, 4500, 6, "Seasonal crop inputs - beans", "Pending"),
            (45, emergencyLoan, 800, 2, "Vehicle breakdown repair", "Pending"),
            (46, microBusiness, 12000, 24, "Beauty salon equipment", "Pending"),
            (47, eduLoan, 6000, 36, "Nursing school tuition", "Pending"),
            (48, personalLoan, 2500, 12, "Home renovation - kitchen", "Pending"),
            (49, agriLoan, 5500, 12, "Vegetable greenhouse construction", "Pending"),
            
            // More disbursed for comprehensive testing
            (50, microBusiness, 11000, 18, "Carinderia equipment", "Disbursed"),
            (51, agriLoan, 8500, 12, "Pig farming inputs", "Disbursed"),
            (52, personalLoan, 3000, 12, "Computer for WFH setup", "Disbursed"),
            (53, eduLoan, 5500, 24, "IT certification course", "Disbursed"),
            (54, microBusiness, 16000, 24, "Auto repair shop tools", "Disbursed"),
            (55, emergencyLoan, 2000, 6, "House fire damage repair", "Disbursed"),
            (56, agriLoan, 11000, 12, "Duck farming expansion", "Disbursed"),
            (57, personalLoan, 4000, 18, "Motorcycle sidecar for business", "Disbursed"),
            (58, microBusiness, 22000, 36, "Laundry shop equipment", "Disbursed"),
            (59, eduLoan, 7000, 24, "Culinary arts program", "Disbursed"),
        };

        int loanNumber = 3001;
        for (int i = 0; i < loanData.Length && i + existingCount < targetCount; i++)
        {
            var data = loanData[i];
            var loanNum = $"LN-{loanNumber + i:D6}";
            
            if (await context.Loans.AnyAsync(l => l.LoanNumber == loanNum, cancellationToken).ConfigureAwait(false))
                continue;

            var member = members[data.MemberIdx % members.Count];
            var product = data.Product;

            var loan = Loan.Create(
                memberId: member.Id,
                loanProductId: product.Id,
                loanNumber: loanNum,
                principalAmount: data.Amount,
                interestRate: product.InterestRate,
                termMonths: data.Term,
                repaymentFrequency: product.RepaymentFrequency,
                purpose: data.Purpose);

            // Apply status transitions based on target status
            switch (data.Status)
            {
                case "Approved":
                    loan.Approve(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-Random.Shared.Next(5, 30))));
                    break;
                    
                case "Disbursed":
                    loan.Approve(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-Random.Shared.Next(60, 120))));
                    loan.Disburse(
                        DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-Random.Shared.Next(30, 90))),
                        DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(data.Term)));
                    break;
                    
                case "Rejected":
                    loan.Reject("Does not meet lending criteria - " + data.Purpose.Split('-').Last().Trim());
                    break;
            }

            await context.Loans.AddAsync(loan, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} loans across all statuses (Pending, Approved, Disbursed, Rejected)", tenant, targetCount);
    }
}
