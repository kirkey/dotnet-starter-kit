using Accounting.Application.GeneralLedgers.Specifications;

namespace Accounting.Application.FinancialStatements.Queries.GenerateCashFlowStatement.v1;

public sealed class GenerateCashFlowStatementQueryHandler(
    ILogger<GenerateCashFlowStatementQueryHandler> logger,
    [FromKeyedServices("accounting:general-ledger")] IRepository<GeneralLedger> ledgerRepository,
    [FromKeyedServices("accounting:accounts")] IRepository<ChartOfAccount> accountRepository)
    : IRequestHandler<GenerateCashFlowStatementQuery, CashFlowStatementDto>
{
    public async Task<CashFlowStatementDto> Handle(GenerateCashFlowStatementQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Get all accounts
        var accounts = await accountRepository.ListAsync(cancellationToken);
        
        // Get general ledger entries for the period
        var ledgerEntries = await ledgerRepository.ListAsync(
            new GeneralLedgerByDateRangeSpec(request.StartDate, request.EndDate), cancellationToken);

        // Calculate cash flow activities
        var operatingActivities = new CashFlowSectionDto
        {
            SectionName = "Operating Activities",
            Lines = new List<CashFlowLineDto>
            {
                new() { Description = "Net Income", Amount = CalculateNetIncome(accounts, ledgerEntries) },
                new() { Description = "Depreciation and Amortization", Amount = CalculateDepreciation(accounts, ledgerEntries) },
                new() { Description = "Changes in Working Capital", Amount = CalculateWorkingCapitalChanges(accounts, ledgerEntries) }
            }
        };

        var investingActivities = new CashFlowSectionDto
        {
            SectionName = "Investing Activities",
            Lines = new List<CashFlowLineDto>
            {
                new() { Description = "Purchase of Fixed Assets", Amount = CalculateFixedAssetPurchases(accounts, ledgerEntries) },
                new() { Description = "Sale of Fixed Assets", Amount = CalculateFixedAssetSales(accounts, ledgerEntries) }
            }
        };

        var financingActivities = new CashFlowSectionDto
        {
            SectionName = "Financing Activities",
            Lines = new List<CashFlowLineDto>
            {
                new() { Description = "Loan Proceeds", Amount = CalculateLoanProceeds(accounts, ledgerEntries) },
                new() { Description = "Loan Payments", Amount = CalculateLoanPayments(accounts, ledgerEntries) },
                new() { Description = "Owner Contributions", Amount = CalculateOwnerContributions(accounts, ledgerEntries) }
            }
        };

        var result = new CashFlowStatementDto
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Method = request.Method,
            OperatingActivities = operatingActivities,
            InvestingActivities = investingActivities,
            FinancingActivities = financingActivities,
            BeginningCashBalance = CalculateBeginningCashBalance(accounts, ledgerEntries, request.StartDate)
        };

        logger.LogInformation("Generated cash flow statement for period {StartDate} to {EndDate}. Net Cash Flow: {NetCashFlow}", 
            request.StartDate, request.EndDate, result.NetCashFlow);

        return result;
    }

    private decimal CalculateNetIncome(IEnumerable<ChartOfAccount> accounts, IEnumerable<GeneralLedger> ledgerEntries)
    {
        var revenueAccounts = accounts.Where(a => a.AccountType == "Revenue");
        var expenseAccounts = accounts.Where(a => a.AccountType.Contains("Expense"));
        
        var revenue = revenueAccounts.Sum(acc => 
            ledgerEntries.Where(le => le.AccountId == acc.Id).Sum(le => le.Credit - le.Debit));
        var expenses = expenseAccounts.Sum(acc => 
            ledgerEntries.Where(le => le.AccountId == acc.Id).Sum(le => le.Debit - le.Credit));
        
        return revenue - expenses;
    }

    private decimal CalculateDepreciation(IEnumerable<ChartOfAccount> accounts, IEnumerable<GeneralLedger> ledgerEntries)
    {
        var depreciationAccounts = accounts.Where(a => a.AccountName.Contains("Depreciation"));
        return depreciationAccounts.Sum(acc => 
            ledgerEntries.Where(le => le.AccountId == acc.Id).Sum(le => le.Debit));
    }

    private decimal CalculateWorkingCapitalChanges(IEnumerable<ChartOfAccount> accounts, IEnumerable<GeneralLedger> ledgerEntries)
    {
        // Simplified calculation - would need more sophisticated logic in practice
        var currentAssets = accounts.Where(a => a.AccountType == "Current Asset" && !a.AccountName.Contains("Cash"));
        var currentLiabilities = accounts.Where(a => a.AccountType == "Current Liability");
        
        var assetChanges = currentAssets.Sum(acc => 
            ledgerEntries.Where(le => le.AccountId == acc.Id).Sum(le => le.Debit - le.Credit));
        var liabilityChanges = currentLiabilities.Sum(acc => 
            ledgerEntries.Where(le => le.AccountId == acc.Id).Sum(le => le.Credit - le.Debit));
        
        return liabilityChanges - assetChanges;
    }

    private decimal CalculateFixedAssetPurchases(IEnumerable<ChartOfAccount> accounts, IEnumerable<GeneralLedger> ledgerEntries)
    {
        var fixedAssetAccounts = accounts.Where(a => a.AccountType == "Fixed Asset");
        return -fixedAssetAccounts.Sum(acc => 
            ledgerEntries.Where(le => le.AccountId == acc.Id).Sum(le => le.Debit));
    }

    private decimal CalculateFixedAssetSales(IEnumerable<ChartOfAccount> accounts, IEnumerable<GeneralLedger> ledgerEntries)
    {
        // This would need more sophisticated logic to track asset disposals
        return 0;
    }

    private decimal CalculateLoanProceeds(IEnumerable<ChartOfAccount> accounts, IEnumerable<GeneralLedger> ledgerEntries)
    {
        var loanAccounts = accounts.Where(a => a.AccountName.Contains("Loan") && a.AccountType.Contains("Liability"));
        return loanAccounts.Sum(acc => 
            ledgerEntries.Where(le => le.AccountId == acc.Id).Sum(le => le.Credit));
    }

    private decimal CalculateLoanPayments(IEnumerable<ChartOfAccount> accounts, IEnumerable<GeneralLedger> ledgerEntries)
    {
        var loanAccounts = accounts.Where(a => a.AccountName.Contains("Loan") && a.AccountType.Contains("Liability"));
        return -loanAccounts.Sum(acc => 
            ledgerEntries.Where(le => le.AccountId == acc.Id).Sum(le => le.Debit));
    }

    private decimal CalculateOwnerContributions(IEnumerable<ChartOfAccount> accounts, IEnumerable<GeneralLedger> ledgerEntries)
    {
        var equityAccounts = accounts.Where(a => a.AccountType == "Equity" && a.AccountName.Contains("Capital"));
        return equityAccounts.Sum(acc => 
            ledgerEntries.Where(le => le.AccountId == acc.Id).Sum(le => le.Credit));
    }

    private decimal CalculateBeginningCashBalance(IEnumerable<ChartOfAccount> accounts, IEnumerable<GeneralLedger> ledgerEntries, DateTime startDate)
    {
        var cashAccounts = accounts.Where(a => a.AccountName.Contains("Cash") || a.AccountName.Contains("Bank"));
        return cashAccounts.Sum(acc => 
            ledgerEntries.Where(le => le.AccountId == acc.Id && le.TransactionDate < startDate)
                .Sum(le => le.Debit - le.Credit));
    }
}
