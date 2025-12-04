using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for fee definitions.
/// </summary>
internal static class FeeDefinitionSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 10;
        var existingCount = await context.FeeDefinitions.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var fees = new (string Code, string Name, string Type, string Calc, string Applies, string Freq, decimal Amt, string Desc)[]
        {
            ("LOAN-PROC-FEE", "Loan Processing Fee", FeeDefinition.TypeProcessing, FeeDefinition.CalculationPercentage, FeeDefinition.AppliesLoan, FeeDefinition.FrequencyOneTime, 2.0m, "One-time fee for processing loan applications"),
            ("LOAN-DISB-FEE", "Loan Disbursement Fee", FeeDefinition.TypeDisbursement, FeeDefinition.CalculationFlat, FeeDefinition.AppliesLoan, FeeDefinition.FrequencyOneTime, 25, "Fixed fee charged at loan disbursement"),
            ("LATE-PAY-FEE", "Late Payment Penalty", FeeDefinition.TypeLateFee, FeeDefinition.CalculationPercentage, FeeDefinition.AppliesLoan, FeeDefinition.FrequencyPerEvent, 5.0m, "Penalty for late loan repayments"),
            ("ACC-MAINT-FEE", "Account Maintenance Fee", FeeDefinition.TypeAccountMaintenance, FeeDefinition.CalculationFlat, FeeDefinition.AppliesSavings, FeeDefinition.FrequencyMonthly, 2, "Monthly account maintenance fee"),
            ("ATM-WITHD-FEE", "ATM Withdrawal Fee", FeeDefinition.TypeWithdrawal, FeeDefinition.CalculationFlat, FeeDefinition.AppliesSavings, FeeDefinition.FrequencyPerEvent, 1.5m, "Fee for ATM withdrawals"),
            ("TRANSFER-FEE", "Fund Transfer Fee", FeeDefinition.TypeTransfer, FeeDefinition.CalculationFlat, FeeDefinition.AppliesSavings, FeeDefinition.FrequencyPerEvent, 5, "Fee for fund transfers"),
            ("CLOSING-FEE", "Account Closing Fee", FeeDefinition.TypeClosingFee, FeeDefinition.CalculationFlat, FeeDefinition.AppliesSavings, FeeDefinition.FrequencyOneTime, 10, "Fee for closing account prematurely"),
            ("INSURANCE-FEE", "Loan Insurance Premium", FeeDefinition.TypeInsurance, FeeDefinition.CalculationPercentage, FeeDefinition.AppliesLoan, FeeDefinition.FrequencyOneTime, 1.5m, "Insurance premium on loan amount"),
            ("FD-EARLY-FEE", "Early FD Withdrawal Penalty", FeeDefinition.TypeEarlyPayment, FeeDefinition.CalculationPercentage, FeeDefinition.AppliesFixedDeposit, FeeDefinition.FrequencyPerEvent, 3.0m, "Penalty for early fixed deposit withdrawal"),
            ("MEMBER-REG-FEE", "Membership Registration Fee", FeeDefinition.TypeProcessing, FeeDefinition.CalculationFlat, FeeDefinition.AppliesMember, FeeDefinition.FrequencyOneTime, 15, "One-time membership registration fee"),
        };

        for (int i = existingCount; i < fees.Length; i++)
        {
            var f = fees[i];
            if (await context.FeeDefinitions.AnyAsync(x => x.Code == f.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var fee = FeeDefinition.Create(
                code: f.Code,
                name: f.Name,
                feeType: f.Type,
                calculationType: f.Calc,
                appliesTo: f.Applies,
                chargeFrequency: f.Freq,
                amount: f.Amt,
                currencyCode: "USD",
                description: f.Desc);

            await context.FeeDefinitions.AddAsync(fee, cancellationToken).ConfigureAwait(false);
        }

        logger.LogInformation("[{Tenant}] seeded fee definitions", tenant);
    }
}
