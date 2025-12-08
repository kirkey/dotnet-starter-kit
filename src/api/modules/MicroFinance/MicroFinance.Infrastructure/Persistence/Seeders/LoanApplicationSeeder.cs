using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for loan applications.
/// Creates loan applications in various stages for demo database.
/// </summary>
internal static class LoanApplicationSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 100;
        var existingCount = await context.LoanApplications.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Where(m => m.IsActive).Take(100).ToListAsync(cancellationToken).ConfigureAwait(false);
        var products = await context.LoanProducts.Where(p => p.IsActive).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (!members.Any() || !products.Any()) return;

        // Get staff IDs for officer assignments
        var staffIds = await context.Staff.Select(s => s.Id).ToListAsync(cancellationToken).ConfigureAwait(false);

        var random = new Random(42);
        int appNumber = 6001;

        var applications = new (int MemberIdx, int ProductIdx, decimal Amount, int Term, string Purpose, string Status)[]
        {
            // Draft applications
            (0, 0, 5000, 12, "Pangkabuhayan - Sari-sari store expansion", LoanApplication.StatusDraft),
            (1, 1, 8000, 12, "Pagbili ng binhi at pataba", LoanApplication.StatusDraft),
            (2, 0, 3000, 6, "Emergency medical expenses", LoanApplication.StatusDraft),
            
            // Submitted - waiting review
            (3, 2, 15000, 24, "Bakery equipment purchase", LoanApplication.StatusSubmitted),
            (4, 1, 10000, 12, "Irrigation system installation", LoanApplication.StatusSubmitted),
            (5, 0, 4000, 12, "Home improvement - roofing", LoanApplication.StatusSubmitted),
            (6, 3, 5000, 24, "University tuition for anak", LoanApplication.StatusSubmitted),
            
            // Under Review
            (7, 2, 20000, 36, "Food processing equipment", LoanApplication.StatusUnderReview),
            (8, 0, 6000, 12, "Motorcycle for delivery business", LoanApplication.StatusUnderReview),
            (9, 1, 12000, 12, "Livestock purchase - baboy", LoanApplication.StatusUnderReview),
            (10, 2, 8000, 18, "Tailoring shop expansion", LoanApplication.StatusUnderReview),
            
            // Pending Documents
            (11, 0, 7000, 12, "Computer for online business", LoanApplication.StatusPendingDocuments),
            (12, 2, 25000, 36, "Restaurant renovation", LoanApplication.StatusPendingDocuments),
            (13, 1, 9000, 12, "Rice mill maintenance", LoanApplication.StatusPendingDocuments),
            
            // Pending Approval
            (14, 0, 5500, 12, "Vehicle repair", LoanApplication.StatusPendingApproval),
            (15, 2, 18000, 24, "Beauty salon equipment", LoanApplication.StatusPendingApproval),
            (16, 3, 8000, 36, "Nursing school tuition", LoanApplication.StatusPendingApproval),
            (17, 1, 7500, 12, "Poultry house construction", LoanApplication.StatusPendingApproval),
            
            // Approved - ready for disbursement
            (18, 0, 4000, 12, "Furniture for new house", LoanApplication.StatusApproved),
            (19, 2, 12000, 18, "Catering business startup", LoanApplication.StatusApproved),
            (20, 1, 6000, 12, "Vegetable farming inputs", LoanApplication.StatusApproved),
            
            // Conditionally Approved
            (21, 2, 30000, 36, "Trucking business - need additional guarantor", LoanApplication.StatusConditionallyApproved),
            (22, 0, 10000, 18, "Need collateral documentation", LoanApplication.StatusConditionallyApproved),
            
            // Rejected
            (23, 2, 100000, 48, "Exceeds maximum loan limit", LoanApplication.StatusRejected),
            (24, 0, 20000, 6, "Insufficient income documentation", LoanApplication.StatusRejected),
            (25, 1, 50000, 24, "Outside agricultural scope", LoanApplication.StatusRejected),
            
            // Withdrawn
            (26, 0, 3000, 6, "Member decided not to proceed", LoanApplication.StatusWithdrawn),
            (27, 2, 8000, 12, "Found alternative financing", LoanApplication.StatusWithdrawn),
            
            // More applications for variety
            (28, 0, 2500, 6, "School supplies for children", LoanApplication.StatusSubmitted),
            (29, 2, 11000, 18, "Auto repair shop tools", LoanApplication.StatusUnderReview),
            (30, 1, 8500, 12, "Fish cage expansion", LoanApplication.StatusPendingApproval),
            (31, 3, 6500, 24, "IT certification course", LoanApplication.StatusApproved),
            (32, 0, 4500, 12, "Wedding expenses", LoanApplication.StatusSubmitted),
            (33, 2, 16000, 24, "Laundry shop equipment", LoanApplication.StatusPendingDocuments),
            (34, 1, 7000, 12, "Duck farm inputs", LoanApplication.StatusUnderReview),
            (35, 0, 3500, 9, "Medical procedure", LoanApplication.StatusPendingApproval),
            (36, 2, 22000, 36, "Hardware store inventory", LoanApplication.StatusSubmitted),
            (37, 3, 9000, 36, "Culinary arts training", LoanApplication.StatusDraft),
            (38, 1, 5000, 6, "Seasonal planting inputs", LoanApplication.StatusSubmitted),
            (39, 0, 6000, 12, "Tricycle purchase", LoanApplication.StatusUnderReview),
        };

        foreach (var app in applications)
        {
            var appNum = $"APP-{appNumber++:D6}";
            if (await context.LoanApplications.AnyAsync(a => a.ApplicationNumber == appNum, cancellationToken).ConfigureAwait(false))
                continue;

            var member = members[app.MemberIdx % members.Count];
            var product = products[app.ProductIdx % products.Count];

            var application = LoanApplication.Create(
                applicationNumber: appNum,
                memberId: member.Id,
                loanProductId: product.Id,
                requestedAmount: app.Amount,
                requestedTermMonths: app.Term,
                purpose: app.Purpose);

            var userId = staffIds.Any() ? staffIds[random.Next(staffIds.Count)] : DefaultIdType.Empty;

            // Apply status transitions
            switch (app.Status)
            {
                case LoanApplication.StatusSubmitted:
                    application.Submit();
                    break;
                case LoanApplication.StatusUnderReview:
                    application.Submit();
                    application.AssignToOfficer(userId);
                    break;
                case LoanApplication.StatusPendingDocuments:
                    application.Submit();
                    application.AssignToOfficer(userId);
                    application.RequestDocuments("Additional documents required");
                    break;
                case LoanApplication.StatusPendingApproval:
                    application.Submit();
                    application.AssignToOfficer(userId);
                    application.SubmitForApproval();
                    break;
                case LoanApplication.StatusApproved:
                    application.Submit();
                    application.AssignToOfficer(userId);
                    application.SubmitForApproval();
                    application.Approve(userId, app.Amount, app.Term);
                    break;
                case LoanApplication.StatusConditionallyApproved:
                    application.Submit();
                    application.AssignToOfficer(userId);
                    application.SubmitForApproval();
                    application.ConditionallyApprove(userId, app.Amount, app.Term, "Requires additional guarantor or collateral");
                    break;
                case LoanApplication.StatusRejected:
                    application.Submit();
                    application.AssignToOfficer(userId);
                    application.Reject(userId, app.Purpose.Split('-').Last().Trim());
                    break;
                case LoanApplication.StatusWithdrawn:
                    application.Submit();
                    application.Withdraw(app.Purpose);
                    break;
            }

            await context.LoanApplications.AddAsync(application, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} loan applications across all stages", tenant, targetCount);
    }
}
