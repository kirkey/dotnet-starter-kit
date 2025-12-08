using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for risk indicators.
/// Creates Key Risk Indicators (KRIs) for risk monitoring.
/// </summary>
internal static class RiskIndicatorSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.RiskIndicators.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var riskCategories = await context.RiskCategories.ToListAsync(cancellationToken).ConfigureAwait(false);
        if (!riskCategories.Any()) return;

        var creditCategory = riskCategories.FirstOrDefault(c => c.Name.Contains("Credit"));
        var operationalCategory = riskCategories.FirstOrDefault(c => c.Name.Contains("Operational"));
        var complianceCategory = riskCategories.FirstOrDefault(c => c.Name.Contains("Compliance"));
        var liquidityCategory = riskCategories.FirstOrDefault(c => c.Name.Contains("Liquidity"));

        var indicators = new (string Code, string Name, string Unit, string Frequency, string Direction, decimal Threshold, decimal Target, decimal Current, DefaultIdType? Category)[]
        {
            // Credit Risk Indicators
            ("KRI-PAR30", "Portfolio at Risk >30 Days", "Percentage", RiskIndicator.FrequencyWeekly, RiskIndicator.DirectionLowerIsBetter, 5.0m, 3.0m, 4.2m, creditCategory?.Id),
            ("KRI-PAR90", "Portfolio at Risk >90 Days", "Percentage", RiskIndicator.FrequencyWeekly, RiskIndicator.DirectionLowerIsBetter, 3.0m, 1.5m, 2.1m, creditCategory?.Id),
            ("KRI-WOFF", "Write-off Ratio", "Percentage", RiskIndicator.FrequencyMonthly, RiskIndicator.DirectionLowerIsBetter, 2.0m, 0.5m, 0.8m, creditCategory?.Id),
            ("KRI-CONC", "Loan Concentration (Top 10)", "Percentage", RiskIndicator.FrequencyMonthly, RiskIndicator.DirectionLowerIsBetter, 20.0m, 10.0m, 15.5m, creditCategory?.Id),
            ("KRI-APPR", "Loan Approval Rate", "Percentage", RiskIndicator.FrequencyMonthly, RiskIndicator.DirectionNeutral, 50.0m, 70.0m, 68.0m, creditCategory?.Id),
            
            // Operational Risk Indicators
            ("KRI-SYSU", "System Uptime", "Percentage", RiskIndicator.FrequencyDaily, RiskIndicator.DirectionHigherIsBetter, 99.0m, 99.9m, 99.7m, operationalCategory?.Id),
            ("KRI-TXFL", "Transaction Failure Rate", "Percentage", RiskIndicator.FrequencyDaily, RiskIndicator.DirectionLowerIsBetter, 5.0m, 1.0m, 1.8m, operationalCategory?.Id),
            ("KRI-STFF", "Staff Turnover Rate", "Percentage", RiskIndicator.FrequencyQuarterly, RiskIndicator.DirectionLowerIsBetter, 15.0m, 8.0m, 10.2m, operationalCategory?.Id),
            ("KRI-FRAU", "Fraud Incident Rate", "Count", RiskIndicator.FrequencyMonthly, RiskIndicator.DirectionLowerIsBetter, 5m, 0m, 2m, operationalCategory?.Id),
            
            // Compliance Risk Indicators
            ("KRI-KYCE", "KYC Expiry Rate", "Percentage", RiskIndicator.FrequencyMonthly, RiskIndicator.DirectionLowerIsBetter, 10.0m, 2.0m, 5.5m, complianceCategory?.Id),
            ("KRI-AMLB", "AML Alert Backlog", "Count", RiskIndicator.FrequencyDaily, RiskIndicator.DirectionLowerIsBetter, 50m, 10m, 25m, complianceCategory?.Id),
            ("KRI-CMPL", "Regulatory Findings", "Count", RiskIndicator.FrequencyQuarterly, RiskIndicator.DirectionLowerIsBetter, 5m, 0m, 2m, complianceCategory?.Id),
            
            // Liquidity Risk Indicators
            ("KRI-LCR", "Liquidity Coverage Ratio", "Percentage", RiskIndicator.FrequencyDaily, RiskIndicator.DirectionHigherIsBetter, 100.0m, 120.0m, 115.0m, liquidityCategory?.Id),
            ("KRI-LD", "Loan-to-Deposit Ratio", "Percentage", RiskIndicator.FrequencyWeekly, RiskIndicator.DirectionNeutral, 85.0m, 75.0m, 78.0m, liquidityCategory?.Id),
            ("KRI-CASH", "Cash Reserve Ratio", "Percentage", RiskIndicator.FrequencyDaily, RiskIndicator.DirectionHigherIsBetter, 10.0m, 15.0m, 13.0m, liquidityCategory?.Id),
        };

        foreach (var kri in indicators)
        {
            if (kri.Category == null) continue;

            var indicator = RiskIndicator.Create(
                riskCategoryId: kri.Category.Value,
                code: kri.Code,
                name: kri.Name,
                unit: kri.Unit,
                frequency: kri.Frequency,
                direction: kri.Direction);

            indicator.Update(
                name: null,
                description: null,
                formula: null,
                unit: null,
                dataSource: null,
                targetValue: kri.Target,
                greenThreshold: null,
                yellowThreshold: kri.Threshold * 0.7m,
                orangeThreshold: null,
                redThreshold: kri.Threshold,
                weightFactor: null,
                notes: null);

            indicator.RecordMeasurement(kri.Current);
            indicator.Activate();

            await context.RiskIndicators.AddAsync(indicator, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} risk indicators", tenant, indicators.Length);
    }
}
