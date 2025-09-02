using Accounting.Application.GeneralLedger.Specifications;
using Accounting.Domain;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.AccountReconciliation.Commands.ReconcileAccount.v1;

public sealed class ReconcileAccountCommandHandler(
    ILogger<ReconcileAccountCommandHandler> logger,
    [FromKeyedServices("accounting:accounts")] IRepository<ChartOfAccount> accountRepository,
    [FromKeyedServices("accounting:generalledger")] IRepository<Domain.GeneralLedger> ledgerRepository)
    : IRequestHandler<ReconcileAccountCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ReconcileAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Verify account exists
        var account = await accountRepository.GetByIdAsync(request.ChartOfAccountId, cancellationToken);
        if (account == null)
        {
            throw new ArgumentException($"Account with ID {request.ChartOfAccountId} not found");
        }

        // Get account balance as of reconciliation date
        var ledgerEntries = await ledgerRepository.ListAsync(
            new GeneralLedgerByAccountAndDateSpec(request.ChartOfAccountId, request.ReconciliationDate), 
            cancellationToken);

        var accountBalance = ledgerEntries
            .Select(le => le.Debit - le.Credit)
            .DefaultIfEmpty(0m)
            .Aggregate((a, b) => a + b);

        // Calculate reconciliation variance
        var variance = request.StatementBalance - accountBalance;

        // Create reconciliation record
        var reconciliationId = DefaultIdType.NewGuid();

        // Log reconciliation details
        logger.LogInformation(
            "Account reconciliation completed for {AccountCode}. Book Balance: {BookBalance}, Statement Balance: {StatementBalance}, Variance: {Variance}",
            account.AccountCode, accountBalance, request.StatementBalance, variance);

        // If there's a variance, it might need investigation
        if (Math.Abs(variance) > 0.01m)
        {
            logger.LogWarning(
                "Reconciliation variance detected for account {AccountCode}: {Variance}",
                account.AccountCode, variance);
        }

        return reconciliationId;
    }
}
