using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for report definitions.
/// Creates standard report definitions for MFI reporting.
/// </summary>
internal static class ReportDefinitionSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.ReportDefinitions.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var reports = new (string Code, string Name, string Category, string Desc, string Frequency, bool IsRegulatory)[]
        {
            // Regulatory Reports
            ("RPT-BSP-STR", "Suspicious Transaction Report", "Regulatory", "BSP-required STR for AML compliance", "OnDemand", true),
            ("RPT-BSP-CTR", "Covered Transaction Report", "Regulatory", "BSP-required CTR for transactions above threshold", "Weekly", true),
            ("RPT-CIC", "CIC Credit Report Submission", "Regulatory", "Credit Information Corporation data submission", "Monthly", true),
            ("RPT-AMLC", "AMLC Annual Report", "Regulatory", "Anti-Money Laundering Council annual compliance report", "Yearly", true),
            
            // Financial Reports
            ("RPT-BS", "Balance Sheet", "Financial", "Statement of financial position", "Monthly", false),
            ("RPT-IS", "Income Statement", "Financial", "Profit and loss statement", "Monthly", false),
            ("RPT-CF", "Cash Flow Statement", "Financial", "Statement of cash flows", "Monthly", false),
            ("RPT-TB", "Trial Balance", "Financial", "Trial balance of all accounts", "Monthly", false),
            
            // Loan Reports
            ("RPT-LOAN-AGE", "Loan Aging Report", "Portfolio", "Portfolio aging analysis by days past due", "Weekly", false),
            ("RPT-LOAN-PAR", "Portfolio at Risk Report", "Portfolio", "PAR calculation and trend analysis", "Weekly", false),
            ("RPT-LOAN-DISB", "Loan Disbursement Report", "Portfolio", "Summary of loans disbursed", "Daily", false),
            ("RPT-LOAN-COL", "Loan Collection Report", "Portfolio", "Summary of collections received", "Daily", false),
            
            // Savings Reports
            ("RPT-SAV-BAL", "Savings Balance Report", "Deposits", "Summary of savings account balances", "Daily", false),
            ("RPT-SAV-DORM", "Dormant Accounts Report", "Deposits", "List of dormant savings accounts", "Monthly", false),
            
            // Member Reports
            ("RPT-MEM-REG", "Member Registration Report", "Membership", "New member registrations", "Weekly", false),
            ("RPT-MEM-DEMO", "Member Demographics Report", "Membership", "Member demographic analysis", "Monthly", false),
            
            // Staff Reports
            ("RPT-LO-PERF", "Loan Officer Performance", "Staff", "Individual loan officer performance metrics", "Monthly", false),
            ("RPT-BRANCH-PERF", "Branch Performance", "Staff", "Branch-level performance dashboard", "Monthly", false),
        };

        foreach (var rpt in reports)
        {
            var report = ReportDefinition.Create(
                code: rpt.Code,
                name: rpt.Name,
                category: rpt.Category,
                outputFormat: ReportDefinition.FormatPdf,
                description: rpt.Desc);

            if (rpt.Frequency != "OnDemand")
            {
                report.ConfigureSchedule(rpt.Frequency, null, null, null);
            }

            report.Activate();
            await context.ReportDefinitions.AddAsync(report, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} report definitions", tenant, reports.Length);
    }
}
