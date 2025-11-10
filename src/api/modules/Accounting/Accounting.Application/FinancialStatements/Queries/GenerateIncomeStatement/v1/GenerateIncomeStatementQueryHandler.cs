using Accounting.Application.GeneralLedgers.Specifications;

namespace Accounting.Application.FinancialStatements.Queries.GenerateIncomeStatement.v1;

public sealed class GenerateIncomeStatementQueryHandler(
    ILogger<GenerateIncomeStatementQueryHandler> logger,
    [FromKeyedServices("accounting:general-ledger")] IRepository<GeneralLedger> ledgerRepository,
    [FromKeyedServices("accounting:accounts")] IRepository<ChartOfAccount> accountRepository)
    : IRequestHandler<GenerateIncomeStatementQuery, IncomeStatementDto>
{
    public async Task<IncomeStatementDto> Handle(GenerateIncomeStatementQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Get all accounts
        var accounts = await accountRepository.ListAsync(cancellationToken);
        
        // Get general ledger entries for the period
        var ledgerEntries = await ledgerRepository.ListAsync(
            new GeneralLedgerByDateRangeSpec(request.StartDate, request.EndDate), cancellationToken);

        // Calculate account balances for the period
        var accountBalances = accounts.Select(account =>
        {
            var accountEntries = ledgerEntries.Where(le => le.Id == account.Id);
            var periodActivity = accountEntries.Sum(le => le.Credit - le.Debit);
            
            // For income statement accounts, we want the period activity
            var amount = account.AccountType switch
            {
                "Revenue" => periodActivity,
                "Cost of Goods Sold" => Math.Abs(periodActivity),
                "Operating Expense" => Math.Abs(periodActivity),
                "Other Income" => periodActivity,
                "Other Expense" => Math.Abs(periodActivity),
                _ => 0
            };

            return new { Account = account, Amount = amount };
        }).Where(x => x.Amount != 0).ToList();

        // Build sections
        var revenue = new IncomeStatementSectionDto
        {
            SectionName = "Revenue",
            Lines = accountBalances
                .Where(x => x.Account.AccountType == "Revenue")
                .Select(x => new IncomeStatementLineDto
                {
                    AccountId = x.Account.Id,
                    AccountCode = x.Account.AccountCode,
                    AccountName = x.Account.AccountName,
                    Amount = x.Amount
                }).ToList()
        };

        var costOfGoodsSold = new IncomeStatementSectionDto
        {
            SectionName = "Cost of Goods Sold",
            Lines = accountBalances
                .Where(x => x.Account.AccountType == "Cost of Goods Sold")
                .Select(x => new IncomeStatementLineDto
                {
                    AccountId = x.Account.Id,
                    AccountCode = x.Account.AccountCode,
                    AccountName = x.Account.AccountName,
                    Amount = x.Amount
                }).ToList()
        };

        var operatingExpenses = new IncomeStatementSectionDto
        {
            SectionName = "Operating Expenses",
            Lines = accountBalances
                .Where(x => x.Account.AccountType == "Operating Expense")
                .Select(x => new IncomeStatementLineDto
                {
                    AccountId = x.Account.Id,
                    AccountCode = x.Account.AccountCode,
                    AccountName = x.Account.AccountName,
                    Amount = x.Amount
                }).ToList()
        };

        var otherIncome = new IncomeStatementSectionDto
        {
            SectionName = "Other Income",
            Lines = accountBalances
                .Where(x => x.Account.AccountType == "Other Income")
                .Select(x => new IncomeStatementLineDto
                {
                    AccountId = x.Account.Id,
                    AccountCode = x.Account.AccountCode,
                    AccountName = x.Account.AccountName,
                    Amount = x.Amount
                }).ToList()
        };

        var otherExpenses = new IncomeStatementSectionDto
        {
            SectionName = "Other Expenses",
            Lines = accountBalances
                .Where(x => x.Account.AccountType == "Other Expense")
                .Select(x => new IncomeStatementLineDto
                {
                    AccountId = x.Account.Id,
                    AccountCode = x.Account.AccountCode,
                    AccountName = x.Account.AccountName,
                    Amount = x.Amount
                }).ToList()
        };

        var result = new IncomeStatementDto
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            ReportFormat = request.ReportFormat,
            Revenue = revenue,
            CostOfGoodsSold = costOfGoodsSold,
            OperatingExpenses = operatingExpenses,
            OtherIncome = otherIncome,
            OtherExpenses = otherExpenses
        };

        logger.LogInformation("Generated income statement for period {StartDate} to {EndDate}. Net Income: {NetIncome}", 
            request.StartDate, request.EndDate, result.NetIncome);

        return result;
    }
}
