using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for fee charges.
/// Creates actual fee charges applied to member accounts.
/// </summary>
internal static class FeeChargeSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.FeeCharges.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var feeDefinitions = await context.FeeDefinitions.ToListAsync(cancellationToken).ConfigureAwait(false);
        if (!feeDefinitions.Any()) return;

        var loans = await context.Loans.Take(30).ToListAsync(cancellationToken).ConfigureAwait(false);
        var savingsAccounts = await context.SavingsAccounts.Take(30).ToListAsync(cancellationToken).ConfigureAwait(false);

        var random = new Random(42);
        int chargeNumber = 17001;
        int chargeCount = 0;

        // Processing fees for loans
        var processingFee = feeDefinitions.FirstOrDefault(f => f.Name.Contains("Processing"));
        if (processingFee != null)
        {
            foreach (var loan in loans.Take(20))
            {
                var charge = FeeCharge.Create(
                    feeDefinitionId: processingFee.Id,
                    loanId: loan.Id,
                    savingsAccountId: null,
                    shareAccountId: null,
                    memberId: loan.MemberId,
                    chargeDate: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(30, 180))),
                    amount: loan.PrincipalAmount * (processingFee.FeeType == "Percentage" ? processingFee.Amount / 100 : 0.01m),
                    reference: $"FEE-{chargeNumber++:D6}");

                // Most processing fees are paid
                if (random.NextDouble() < 0.9)
                {
                    charge.RecordPayment(charge.Amount);
                }

                await context.FeeCharges.AddAsync(charge, cancellationToken).ConfigureAwait(false);
                chargeCount++;
            }
        }

        // Late fees for disbursed loans (simulating overdue scenario)
        var lateFee = feeDefinitions.FirstOrDefault(f => f.Name.Contains("Late") || f.Name.Contains("Penalty"));
        if (lateFee != null)
        {
            var overdueLoans = loans.Where(l => l.Status == Loan.StatusDisbursed).Take(15);
            foreach (var loan in overdueLoans)
            {
                for (int i = 0; i < random.Next(1, 4); i++)
                {
                    var charge = FeeCharge.Create(
                        feeDefinitionId: lateFee.Id,
                        loanId: loan.Id,
                        savingsAccountId: null,
                        shareAccountId: null,
                        memberId: loan.MemberId,
                        chargeDate: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(1, 60))),
                        amount: lateFee.Amount,
                        reference: $"FEE-{chargeNumber++:D6}");

                    // Some late fees are pending
                    if (random.NextDouble() < 0.4)
                    {
                        charge.RecordPayment(charge.Amount);
                    }

                    await context.FeeCharges.AddAsync(charge, cancellationToken).ConfigureAwait(false);
                    chargeCount++;
                }
            }
        }

        // Maintenance fees for savings accounts
        var maintenanceFee = feeDefinitions.FirstOrDefault(f => f.Name.Contains("Maintenance"));
        if (maintenanceFee != null)
        {
            foreach (var account in savingsAccounts.Take(15))
            {
                var charge = FeeCharge.Create(
                    feeDefinitionId: maintenanceFee.Id,
                    loanId: null,
                    savingsAccountId: account.Id,
                    shareAccountId: null,
                    memberId: account.MemberId,
                    chargeDate: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(1, 30))),
                    amount: maintenanceFee.Amount,
                    reference: $"FEE-{chargeNumber++:D6}");

                charge.RecordPayment(charge.Amount);
                await context.FeeCharges.AddAsync(charge, cancellationToken).ConfigureAwait(false);
                chargeCount++;
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} fee charges", tenant, chargeCount);
    }
}
