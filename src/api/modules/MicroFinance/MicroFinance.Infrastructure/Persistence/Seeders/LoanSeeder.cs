using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for loans with comprehensive test data.
/// Creates 30 loans across all statuses for testing approval, disbursement, and repayment workflows.
/// </summary>
internal static class LoanSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 30;
        var existingCount = await context.Loans.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Where(m => m.IsActive).Take(30).ToListAsync(cancellationToken).ConfigureAwait(false);
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
            (0, agriLoan, 5000, 12, "Purchase of fertilizers and seeds for maize farming", "Pending"),
            (1, agriLoan, 8000, 12, "Irrigation equipment for vegetable farming", "Pending"),
            (2, personalLoan, 2000, 6, "Home renovation and repairs", "Pending"),
            (3, microBusiness, 10000, 24, "Expansion of grocery store inventory", "Pending"),
            (4, emergencyLoan, 500, 3, "Medical emergency expenses", "Pending"),
            
            // Approved loans - ready for disbursement
            (5, personalLoan, 3000, 12, "Vehicle repair and maintenance", "Approved"),
            (6, microBusiness, 15000, 36, "Opening second shop location", "Approved"),
            (7, eduLoan, 5000, 24, "University tuition fees", "Approved"),
            (8, agriLoan, 12000, 12, "Tractor rental and land preparation", "Approved"),
            (9, personalLoan, 1500, 6, "Family wedding expenses", "Approved"),
            
            // Active/Disbursed loans - in repayment
            (10, microBusiness, 8000, 18, "Bakery equipment purchase", "Disbursed"),
            (11, agriLoan, 6000, 12, "Poultry farm expansion", "Disbursed"),
            (12, personalLoan, 4000, 12, "Motorcycle purchase for transport business", "Disbursed"),
            (13, eduLoan, 8000, 36, "Professional certification program", "Disbursed"),
            (14, microBusiness, 20000, 24, "Restaurant renovation", "Disbursed"),
            (15, personalLoan, 2500, 12, "Solar panel installation", "Disbursed"),
            (16, agriLoan, 10000, 12, "Dairy cattle purchase", "Disbursed"),
            (17, microBusiness, 5000, 12, "Tailoring shop equipment", "Disbursed"),
            (18, emergencyLoan, 1000, 3, "Roof repair after storm damage", "Disbursed"),
            (19, personalLoan, 3500, 18, "Computer and office equipment", "Disbursed"),
            
            // Rejected loans - for testing rejection workflow
            (20, microBusiness, 50000, 48, "Large factory expansion - exceeds limit", "Rejected"),
            (21, personalLoan, 15000, 6, "Insufficient income documentation", "Rejected"),
            (22, agriLoan, 30000, 24, "Land purchase - outside policy scope", "Rejected"),
            
            // More pending for variety
            (23, eduLoan, 3000, 12, "Technical training course fees", "Pending"),
            (24, microBusiness, 7500, 18, "Food truck business startup", "Pending"),
            (25, personalLoan, 1800, 9, "Furniture purchase for new home", "Pending"),
            (26, agriLoan, 4500, 6, "Seasonal crop inputs - beans", "Pending"),
            (27, emergencyLoan, 800, 2, "Vehicle breakdown repair", "Pending"),
            (28, microBusiness, 12000, 24, "Beauty salon equipment", "Pending"),
            (29, eduLoan, 6000, 36, "Nursing school tuition", "Pending"),
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
