using Accounting.Application.GeneralLedgers.Specifications;

namespace Accounting.Application.Budgets.Queries.GenerateBudgetVarianceAnalysis.v1;

public sealed class GenerateBudgetVarianceAnalysisQueryHandler(
    ILogger<GenerateBudgetVarianceAnalysisQueryHandler> logger,
    [FromKeyedServices("accounting:budgets")] IRepository<Budget> budgetRepository,
    [FromKeyedServices("accounting:generalledger")] IRepository<GeneralLedger> ledgerRepository,
    [FromKeyedServices("accounting:accounts")] IRepository<ChartOfAccount> accountRepository)
    : IRequestHandler<GenerateBudgetVarianceAnalysisQuery, BudgetVarianceAnalysisDto>
{
    public async Task<BudgetVarianceAnalysisDto> Handle(GenerateBudgetVarianceAnalysisQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Get the budget
        var budget = await budgetRepository.GetByIdAsync(request.BudgetId, cancellationToken);
        if (budget == null)
        {
            throw new ArgumentException($"Budget with ID {request.BudgetId} not found");
        }

        // Get all accounts
        var accounts = await accountRepository.ListAsync(cancellationToken);
        
        // Get actual amounts from general ledger for the period
        var ledgerEntries = await ledgerRepository.ListAsync(
            new GeneralLedgerByDateRangeSpec(request.StartDate, request.EndDate), cancellationToken);

        // Calculate variance for each budget line
        var varianceLines = new List<BudgetVarianceLineDto>();

        foreach (var budgetLine in budget.BudgetDetails ?? [])
        {
            var account = accounts.FirstOrDefault(a => a.Id == budgetLine.AccountId);
            if (account == null) continue;

            // Calculate actual amount for this account in the period
            var accountEntries = ledgerEntries.Where(le => le.AccountId == account.Id);
            var actualAmount = account.NormalBalance == "Debit" 
                ? accountEntries.Sum(le => le.Debit - le.Credit)
                : accountEntries.Sum(le => le.Credit - le.Debit);

            // Apply variance threshold filter
            var variance = actualAmount - budgetLine.BudgetedAmount;
            if (Math.Abs(variance) >= request.VarianceThreshold)
            {
                varianceLines.Add(new BudgetVarianceLineDto
                {
                    AccountId = account.Id,
                    AccountCode = account.AccountCode,
                    AccountName = account.AccountName,
                    Category = account.AccountType,
                    BudgetedAmount = budgetLine.BudgetedAmount,
                    ActualAmount = actualAmount
                });
            }
        }

        // Calculate summary statistics
        var summary = new BudgetPerformanceSummaryDto
        {
            TotalAccounts = varianceLines.Count,
            AccountsOverBudget = varianceLines.Count(v => v.Variance > 0),
            AccountsUnderBudget = varianceLines.Count(v => v.Variance < 0),
            AccountsOnTarget = varianceLines.Count(v => Math.Abs(v.Variance) < 0.01m),
            LargestVariance = varianceLines.Any() ? varianceLines.Max(v => Math.Abs(v.Variance)) : 0,
            SmallestVariance = varianceLines.Any() ? varianceLines.Min(v => Math.Abs(v.Variance)) : 0,
            AverageVariancePercentage = varianceLines.Any() ? varianceLines.Average(v => Math.Abs(v.VariancePercentage)) : 0
        };

        var result = new BudgetVarianceAnalysisDto
        {
            BudgetId = budget.Id,
            BudgetName = budget.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            AnalysisType = request.AnalysisType,
            VarianceLines = varianceLines.OrderBy(v => v.AccountCode).ToList(),
            TotalBudgetedAmount = varianceLines.Sum(v => v.BudgetedAmount),
            TotalActualAmount = varianceLines.Sum(v => v.ActualAmount),
            Summary = summary
        };

        logger.LogInformation("Generated budget variance analysis for budget {BudgetName}. Total variance: {TotalVariance}", 
            budget.Name, result.TotalVariance);

        return result;
    }
}
