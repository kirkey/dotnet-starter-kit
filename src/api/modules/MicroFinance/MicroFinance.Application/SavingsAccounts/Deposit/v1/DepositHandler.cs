using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Deposit.v1;

public sealed class DepositHandler(
    [FromKeyedServices("microfinance:savingsaccounts")] IRepository<SavingsAccount> repository,
    [FromKeyedServices("microfinance:savingstransactions")] IRepository<SavingsTransaction> transactionRepository,
    ILogger<DepositHandler> logger)
    : IRequestHandler<DepositCommand, DepositResponse>
{
    public async Task<DepositResponse> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository.FirstOrDefaultAsync(
            new SavingsAccountByIdSpec(request.AccountId), cancellationToken).ConfigureAwait(false);

        if (account is null)
        {
            throw new NotFoundException($"Savings account with ID {request.AccountId} not found.");
        }

        if (account.Status != SavingsAccount.StatusActive)
        {
            throw new InvalidOperationException($"Cannot deposit to account in {account.Status} status.");
        }

        account.Deposit(request.Amount);

        var transactionReference = request.TransactionReference ?? $"DEP{DateTime.UtcNow:yyyyMMddHHmmss}";
        var transaction = SavingsTransaction.Create(
            account.Id,
            transactionReference,
            SavingsTransaction.TypeDeposit,
            request.Amount,
            account.Balance,
            null,
            request.Notes,
            request.PaymentMethod
        );

        await transactionRepository.AddAsync(transaction, cancellationToken).ConfigureAwait(false);
        await repository.UpdateAsync(account, cancellationToken).ConfigureAwait(false);
        await transactionRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Deposit of {Amount} to account {AccountNumber}. New balance: {Balance}",
            request.Amount,
            account.AccountNumber,
            account.Balance);

        return new DepositResponse(transaction.Id, account.Balance);
    }
}
