using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for approval workflows.
/// Creates approval workflow definitions.
/// </summary>
internal static class ApprovalWorkflowSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 8;
        var existingCount = await context.ApprovalWorkflows.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var workflows = new (string Code, string Name, string Entity, string Desc, int Steps, decimal? MinAmount, decimal? MaxAmount)[]
        {
            ("WF-LOAN-S", "Small Loan Approval", "Loan", "Approval workflow for loans up to ₱10,000", 1, 0, 10000),
            ("WF-LOAN-M", "Medium Loan Approval", "Loan", "Approval workflow for loans ₱10,001-₱50,000", 2, 10001, 50000),
            ("WF-LOAN-L", "Large Loan Approval", "Loan", "Approval workflow for loans above ₱50,000", 3, 50001, null),
            ("WF-MEMBER", "New Member Approval", "Member", "Approval workflow for new member registration", 1, null, null),
            ("WF-WRITEOFF", "Loan Write-off Approval", "LoanWriteOff", "Approval workflow for loan write-offs", 3, null, null),
            ("WF-TRANSFER", "Large Transfer Approval", "Transfer", "Approval for transfers above ₱100,000", 2, 100001, null),
            ("WF-FD", "Fixed Deposit Withdrawal", "FixedDeposit", "Early withdrawal approval for fixed deposits", 2, null, null),
            ("WF-EXPENSE", "Expense Approval", "Expense", "Approval workflow for operational expenses", 2, 5000, null),
        };

        foreach (var wf in workflows)
        {
            if (await context.ApprovalWorkflows.AnyAsync(w => w.Code == wf.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var workflow = ApprovalWorkflow.Create(
                code: wf.Code,
                name: wf.Name,
                entityType: wf.Entity,
                numberOfLevels: wf.Steps);
            
            workflow.WithAmountThresholds(wf.MinAmount, wf.MaxAmount);

            workflow.Activate();
            await context.ApprovalWorkflows.AddAsync(workflow, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} approval workflows", tenant, targetCount);
    }
}
