using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for risk categories.
/// Creates risk classification categories for risk management.
/// </summary>
internal static class RiskCategorySeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 15;
        var existingCount = await context.RiskCategories.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var categories = new (string Code, string Name, string Type, string Desc, string Severity, decimal Weight)[]
        {
            // Credit Risk Categories
            ("RISK-CR01", "Default Risk", RiskCategory.TypeCredit, "Risk of borrower failing to repay principal or interest", RiskCategory.SeverityHigh, 30),
            ("RISK-CR02", "Concentration Risk", RiskCategory.TypeCredit, "Over-exposure to single borrower or sector", RiskCategory.SeverityMedium, 20),
            ("RISK-CR03", "Collateral Risk", RiskCategory.TypeCredit, "Risk of collateral value depreciation", RiskCategory.SeverityMedium, 15),
            
            // Operational Risk Categories
            ("RISK-OP01", "Fraud Risk", RiskCategory.TypeOperational, "Internal or external fraud incidents", RiskCategory.SeverityCritical, 25),
            ("RISK-OP02", "Technology Risk", RiskCategory.TypeOperational, "System failures or cyber security threats", RiskCategory.SeverityHigh, 20),
            ("RISK-OP03", "Process Risk", RiskCategory.TypeOperational, "Failures in internal processes or controls", RiskCategory.SeverityMedium, 15),
            ("RISK-OP04", "Human Error Risk", RiskCategory.TypeOperational, "Errors by staff in transactions or documentation", RiskCategory.SeverityLow, 10),
            
            // Compliance Risk Categories
            ("RISK-CP01", "AML/CFT Risk", RiskCategory.TypeCompliance, "Anti-money laundering and counter-terrorism financing", RiskCategory.SeverityCritical, 30),
            ("RISK-CP02", "Regulatory Risk", RiskCategory.TypeCompliance, "Non-compliance with BSP and other regulations", RiskCategory.SeverityHigh, 25),
            ("RISK-CP03", "Data Privacy Risk", RiskCategory.TypeCompliance, "Violations of Data Privacy Act", RiskCategory.SeverityMedium, 20),
            
            // Market Risk Categories
            ("RISK-MK01", "Interest Rate Risk", RiskCategory.TypeMarket, "Adverse changes in interest rates", RiskCategory.SeverityMedium, 15),
            ("RISK-MK02", "Liquidity Risk", RiskCategory.TypeLiquidity, "Inability to meet financial obligations", RiskCategory.SeverityHigh, 25),
            
            // Strategic Risk
            ("RISK-ST01", "Competition Risk", RiskCategory.TypeStrategic, "Loss of market share to competitors", RiskCategory.SeverityMedium, 15),
            ("RISK-ST02", "Reputation Risk", RiskCategory.TypeReputational, "Damage to organizational reputation", RiskCategory.SeverityHigh, 20),
            
            // Counterparty Risk
            ("RISK-CP04", "Partner Default Risk", RiskCategory.TypeCounterparty, "Risk of partner institutions failing", RiskCategory.SeverityMedium, 15),
        };

        foreach (var cat in categories)
        {
            if (await context.RiskCategories.AnyAsync(r => r.Code == cat.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var category = RiskCategory.Create(
                code: cat.Code,
                name: cat.Name,
                riskType: cat.Type,
                description: cat.Desc,
                defaultSeverity: cat.Severity,
                riskWeight: cat.Weight);

            await context.RiskCategories.AddAsync(category, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} risk categories", tenant, targetCount);
    }
}
