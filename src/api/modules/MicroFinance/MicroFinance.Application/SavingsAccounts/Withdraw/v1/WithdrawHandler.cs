using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Withdraw.v1;

public sealed class WithdrawHandler(
    [FromKeyedServices("microfinance:savingsaccounts")] IRepository<SavingsAccount> repository,
    [FromKeyedServices("microfinance:savingsproducts")] IRepository<SavingsProduct> productRepository,
    [FromKeyedServices("microfinance:savingstransactions")] IRepository<SavingsTransaction> transactionRepository,
    ILogger<WithdrawHandler> logger)
    : IRequestHandler<WithdrawCommand, WithdrawResponse>
{
    public async Task<WithdrawResponse> Handle(WithdrawCommand request, CancellationToken cancellationToken)
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
            throw new InvalidOperationException($"Cannot withdraw from account in {account.Status} status.");
        }

        // Get the savings product for withdrawal rules
        var product = await productRepository.FirstOrDefaultAsync(
            new SavingsProductByIdSpec(account.SavingsProductId), cancellationToken).ConfigureAwait(false);

        if (product is null)
        {
            throw new NotFoundException($"Savings product not found.");
        }

        // Check minimum withdrawal amount
        if (request.Amount < product.MinWithdrawalAmount)
        {
            throw new InvalidOperationException(
                $"Minimum withdrawal amount is {product.MinWithdrawalAmount}.");
        }

        // Check maximum withdrawal per day if set
        if (product.MaxWithdrawalPerDay.HasValue)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var withdrawalsToday = account.Transactions
                .Where(t => t.TransactionType == SavingsTransaction.TypeWithdrawal &&
                           t.TransactionDate == today)
                .Sum(t => t.Amount);

            if (withdrawalsToday + request.Amount > product.MaxWithdrawalPerDay.Value)
            {
                throw new InvalidOperationException(
                    $"Maximum daily withdrawal ({product.MaxWithdrawalPerDay.Value}) would be exceeded.");
            }
        }

        // Check minimum balance requirement (considering overdraft if allowed)
        var balanceAfterWithdrawal = account.Balance - request.Amount;
        var minimumAllowedBalance = product.AllowOverdraft && product.OverdraftLimit.HasValue
            ? -product.OverdraftLimit.Value
            : product.MinBalanceForInterest;

        if (balanceAfterWithdrawal < minimumAllowedBalance)
        {
            var availableForWithdrawal = account.Balance - minimumAllowedBalance;
            throw new InvalidOperationException(
                $"Withdrawal would result in balance below minimum. Available for withdrawal: {Math.Max(0, availableForWithdrawal)}");
        }

        account.Withdraw(request.Amount);

        var transactionReference = $"WDR{DateTime.UtcNow:yyyyMMddHHmmss}";
        var transaction = SavingsTransaction.Create(
            account.Id,
            transactionReference,
            SavingsTransaction.TypeWithdrawal,
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
            "Withdrawal of {Amount} from account {AccountNumber}. New balance: {Balance}",
            request.Amount,
            account.AccountNumber,
            account.Balance);

        return new WithdrawResponse(transaction.Id, account.Balance);
    }
}
