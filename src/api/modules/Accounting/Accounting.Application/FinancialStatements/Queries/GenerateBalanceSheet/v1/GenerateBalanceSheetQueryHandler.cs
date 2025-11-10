using Accounting.Application.GeneralLedgers.Specifications;

namespace Accounting.Application.FinancialStatements.Queries.GenerateBalanceSheet.v1;

public sealed class GenerateBalanceSheetQueryHandler(
    ILogger<GenerateBalanceSheetQueryHandler> logger,
    [FromKeyedServices("accounting:general-ledger")] IRepository<GeneralLedger> ledgerRepository,
    [FromKeyedServices("accounting:accounts")] IRepository<ChartOfAccount> accountRepository)
    : IRequestHandler<GenerateBalanceSheetQuery, BalanceSheetDto>
{
    public async Task<BalanceSheetDto> Handle(GenerateBalanceSheetQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Get all accounts
        var accounts = await accountRepository.ListAsync(cancellationToken);
        
        // Get general ledger entries up to the as-of date
        var ledgerEntries = await ledgerRepository.ListAsync(
            new GeneralLedgerByDateRangeSpec(null, request.AsOfDate), cancellationToken);

        // Calculate account balances
        var accountBalances = accounts.Select(account =>
        {
            var accountEntries = ledgerEntries.Where(le => le.AccountId == account.Id);
            var totalDebits = accountEntries.Sum(le => le.Debit);
            var totalCredits = accountEntries.Sum(le => le.Credit);
            
            // Calculate balance based on account type and normal balance
            var balance = account.NormalBalance == "Debit" 
                ? totalDebits - totalCredits 
                : totalCredits - totalDebits;

            return new { Account = account, Balance = balance };
        }).Where(x => Math.Abs(x.Balance) > 0.01m).ToList();

        // Build Assets section
        var currentAssets = new BalanceSheetSubSectionDto
        {
            SubSectionName = "Current Assets",
            Lines = accountBalances
                .Where(x => x.Account.AccountType == "Current Asset")
                .Select(x => new BalanceSheetLineDto
                {
                    AccountId = x.Account.Id,
                    AccountCode = x.Account.AccountCode,
                    AccountName = x.Account.AccountName,
                    Amount = x.Balance
                }).ToList()
        };

        var fixedAssets = new BalanceSheetSubSectionDto
        {
            SubSectionName = "Fixed Assets",
            Lines = accountBalances
                .Where(x => x.Account.AccountType == "Fixed Asset")
                .Select(x => new BalanceSheetLineDto
                {
                    AccountId = x.Account.Id,
                    AccountCode = x.Account.AccountCode,
                    AccountName = x.Account.AccountName,
                    Amount = x.Balance
                }).ToList()
        };

        var assets = new BalanceSheetSectionDto
        {
            SectionName = "Assets",
            SubSections = new List<BalanceSheetSubSectionDto> { currentAssets, fixedAssets }
        };

        // Build Liabilities section
        var currentLiabilities = new BalanceSheetSubSectionDto
        {
            SubSectionName = "Current Liabilities",
            Lines = accountBalances
                .Where(x => x.Account.AccountType == "Current Liability")
                .Select(x => new BalanceSheetLineDto
                {
                    AccountId = x.Account.Id,
                    AccountCode = x.Account.AccountCode,
                    AccountName = x.Account.AccountName,
                    Amount = x.Balance
                }).ToList()
        };

        var longTermLiabilities = new BalanceSheetSubSectionDto
        {
            SubSectionName = "Long-Term Liabilities",
            Lines = accountBalances
                .Where(x => x.Account.AccountType == "Long-Term Liability")
                .Select(x => new BalanceSheetLineDto
                {
                    AccountId = x.Account.Id,
                    AccountCode = x.Account.AccountCode,
                    AccountName = x.Account.AccountName,
                    Amount = x.Balance
                }).ToList()
        };

        var liabilities = new BalanceSheetSectionDto
        {
            SectionName = "Liabilities",
            SubSections = new List<BalanceSheetSubSectionDto> { currentLiabilities, longTermLiabilities }
        };

        // Build Equity section
        var equity = new BalanceSheetSectionDto
        {
            SectionName = "Equity",
            SubSections = new List<BalanceSheetSubSectionDto>
            {
                new()
                {
                    SubSectionName = "Owner's Equity",
                    Lines = accountBalances
                        .Where(x => x.Account.AccountType == "Equity")
                        .Select(x => new BalanceSheetLineDto
                        {
                            AccountId = x.Account.Id,
                            AccountCode = x.Account.AccountCode,
                            AccountName = x.Account.AccountName,
                            Amount = x.Balance
                        }).ToList()
                }
            }
        };

        var result = new BalanceSheetDto
        {
            AsOfDate = request.AsOfDate,
            ReportFormat = request.ReportFormat,
            Assets = assets,
            Liabilities = liabilities,
            Equity = equity
        };

        logger.LogInformation("Generated balance sheet as of {AsOfDate}. Total Assets: {TotalAssets}, Total Liabilities & Equity: {TotalLiabilitiesEquity}, Balanced: {IsBalanced}", 
            request.AsOfDate, result.TotalAssets, result.TotalLiabilitiesAndEquity, result.IsBalanced);

        return result;
    }
}
