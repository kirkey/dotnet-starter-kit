using Accounting.Application.GeneralLedger.Specifications;
using Accounting.Application.TrialBalance.Queries.GenerateTrialBalance.v1;
using Accounting.Domain;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.TrialBalance.Queries.GenerateTrialBalance.v1;

public sealed class GenerateTrialBalanceQueryHandler(
    ILogger<GenerateTrialBalanceQueryHandler> logger,
    [FromKeyedServices("accounting:generalledger")] IRepository<Domain.GeneralLedger> ledgerRepository,
    [FromKeyedServices("accounting:accounts")] IRepository<ChartOfAccount> accountRepository)
    : IRequestHandler<GenerateTrialBalanceQuery, TrialBalanceDto>
{
    public async Task<TrialBalanceDto> Handle(GenerateTrialBalanceQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Get all accounts
        var accounts = await accountRepository.ListAsync(cancellationToken);
        
        // Filter by account type if specified
        if (!string.IsNullOrEmpty(request.AccountTypeFilter))
        {
            accounts = accounts.Where(a => a.AccountType == request.AccountTypeFilter).ToList();
        }

        // Get general ledger entries up to the specified date
        var ledgerEntries = await ledgerRepository.ListAsync(
            new GeneralLedgerByDateRangeSpec(null, request.AsOfDate), cancellationToken);

        // Calculate balances for each account
        var accountBalances = accounts.Select(account =>
        {
            var accountEntries = ledgerEntries.Where(le => le.AccountId == account.Id);
            var totalDebits = accountEntries.Sum(le => le.Debit);
            var totalCredits = accountEntries.Sum(le => le.Credit);

            var debitBalance = 0m;
            var creditBalance = 0m;

            // Calculate net balance based on normal balance
            var netBalance = totalDebits - totalCredits;
            if (account.NormalBalance == "Debit")
            {
                if (netBalance >= 0)
                    debitBalance = netBalance;
                else
                    creditBalance = Math.Abs(netBalance);
            }
            else
            {
                if (netBalance <= 0)
                    creditBalance = Math.Abs(netBalance);
                else
                    debitBalance = netBalance;
            }

            return new TrialBalanceLineDto
            {
                AccountId = account.Id,
                AccountCode = account.AccountCode,
                AccountName = account.AccountName,
                AccountType = account.AccountType,
                DebitBalance = debitBalance,
                CreditBalance = creditBalance
            };
        }).ToList();

        // Filter out zero balances if requested
        if (!request.IncludeZeroBalances)
        {
            accountBalances = accountBalances
                .Where(ab => ab.DebitBalance != 0 || ab.CreditBalance != 0)
                .ToList();
        }

        var result = new TrialBalanceDto
        {
            AsOfDate = request.AsOfDate,
            Lines = accountBalances.OrderBy(ab => ab.AccountCode).ToList(),
            TotalDebits = accountBalances.Sum(ab => ab.DebitBalance),
            TotalCredits = accountBalances.Sum(ab => ab.CreditBalance)
        };

        logger.LogInformation("Generated trial balance as of {AsOfDate} with {LineCount} accounts", 
            request.AsOfDate, result.Lines.Count);

        return result;
    }
}
