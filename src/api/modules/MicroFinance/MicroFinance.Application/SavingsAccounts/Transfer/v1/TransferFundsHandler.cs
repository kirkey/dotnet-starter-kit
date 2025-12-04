using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Transfer.v1;

public sealed class TransferFundsHandler(
    [FromKeyedServices("microfinance:savingsaccounts")] IRepository<SavingsAccount> accountRepository,
    [FromKeyedServices("microfinance:savingstransactions")] IRepository<SavingsTransaction> transactionRepository,
    ILogger<TransferFundsHandler> logger)
    : IRequestHandler<TransferFundsCommand, TransferFundsResponse>
{
    public async Task<TransferFundsResponse> Handle(TransferFundsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Get source account
        var sourceAccount = await accountRepository.FirstOrDefaultAsync(
            new SavingsAccountByIdSpec(request.FromAccountId), cancellationToken).ConfigureAwait(false);

        if (sourceAccount is null)
        {
            throw new NotFoundException($"Source account with ID {request.FromAccountId} not found.");
        }

        // Get destination account
        var destAccount = await accountRepository.FirstOrDefaultAsync(
            new SavingsAccountByIdSpec(request.ToAccountId), cancellationToken).ConfigureAwait(false);

        if (destAccount is null)
        {
            throw new NotFoundException($"Destination account with ID {request.ToAccountId} not found.");
        }

        // Verify both accounts are active
        if (sourceAccount.Status != SavingsAccount.StatusActive)
        {
            throw new InvalidOperationException($"Source account is not active. Current status: {sourceAccount.Status}");
        }

        if (destAccount.Status != SavingsAccount.StatusActive)
        {
            throw new InvalidOperationException($"Destination account is not active. Current status: {destAccount.Status}");
        }

        // Verify sufficient balance
        if (sourceAccount.Balance < request.Amount)
        {
            throw new InvalidOperationException($"Insufficient balance. Available: {sourceAccount.Balance}, Requested: {request.Amount}");
        }

        var transactionDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var transferReference = $"TRF{DateTime.UtcNow:yyyyMMddHHmmss}";

        // Create withdrawal transaction
        sourceAccount.Withdraw(request.Amount);
        var withdrawalTxn = SavingsTransaction.Create(
            sourceAccount.Id,
            transferReference,
            SavingsTransaction.TypeWithdrawal,
            request.Amount,
            sourceAccount.Balance,
            transactionDate,
            $"Transfer out to {destAccount.AccountNumber}");

        // Create deposit transaction
        destAccount.Deposit(request.Amount);
        var depositTxn = SavingsTransaction.Create(
            destAccount.Id,
            transferReference,
            SavingsTransaction.TypeDeposit,
            request.Amount,
            destAccount.Balance,
            transactionDate,
            $"Transfer in from {sourceAccount.AccountNumber}");

        // Save all changes
        await transactionRepository.AddAsync(withdrawalTxn, cancellationToken).ConfigureAwait(false);
        await transactionRepository.AddAsync(depositTxn, cancellationToken).ConfigureAwait(false);
        await accountRepository.UpdateAsync(sourceAccount, cancellationToken).ConfigureAwait(false);
        await accountRepository.UpdateAsync(destAccount, cancellationToken).ConfigureAwait(false);

        await transactionRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await accountRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Transfer completed: {Amount} from {SourceAccount} to {DestAccount}. Ref: {Reference}",
            request.Amount,
            sourceAccount.AccountNumber,
            destAccount.AccountNumber,
            transferReference);

        return new TransferFundsResponse(
            withdrawalTxn.Id,
            depositTxn.Id,
            sourceAccount.Balance,
            destAccount.Balance);
    }
}
