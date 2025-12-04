using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Create.v1;

public sealed class CreateSavingsAccountHandler(
    [FromKeyedServices("microfinance:savingsaccounts")] IRepository<SavingsAccount> repository,
    [FromKeyedServices("microfinance:members")] IRepository<Member> memberRepository,
    [FromKeyedServices("microfinance:savingsproducts")] IRepository<SavingsProduct> savingsProductRepository,
    [FromKeyedServices("microfinance:savingstransactions")] IRepository<SavingsTransaction> transactionRepository,
    ILogger<CreateSavingsAccountHandler> logger)
    : IRequestHandler<CreateSavingsAccountCommand, CreateSavingsAccountResponse>
{
    public async Task<CreateSavingsAccountResponse> Handle(CreateSavingsAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Validate member exists and is active
        var member = await memberRepository.FirstOrDefaultAsync(
            new MemberByIdSpec(request.MemberId), cancellationToken).ConfigureAwait(false);

        if (member is null)
        {
            throw new NotFoundException($"Member with ID {request.MemberId} not found.");
        }

        if (!member.IsActive)
        {
            throw new InvalidOperationException("Cannot create savings account for inactive member.");
        }

        // Validate savings product exists and is active
        var savingsProduct = await savingsProductRepository.FirstOrDefaultAsync(
            new SavingsProductByIdSpec(request.SavingsProductId), cancellationToken).ConfigureAwait(false);

        if (savingsProduct is null)
        {
            throw new NotFoundException($"Savings product with ID {request.SavingsProductId} not found.");
        }

        if (!savingsProduct.IsActive)
        {
            throw new InvalidOperationException("Cannot create account with inactive savings product.");
        }

        // Validate initial deposit against minimum opening balance
        if (request.InitialDeposit < savingsProduct.MinOpeningBalance)
        {
            throw new InvalidOperationException(
                $"Initial deposit must be at least {savingsProduct.MinOpeningBalance}.");
        }

        // Generate unique account number
        var accountNumber = await GenerateUniqueAccountNumber(cancellationToken).ConfigureAwait(false);

        var savingsAccount = SavingsAccount.Create(
            accountNumber,
            member.Id,
            savingsProduct.Id,
            request.InitialDeposit
        );

        await repository.AddAsync(savingsAccount, cancellationToken).ConfigureAwait(false);

        // If there's an initial deposit, create a transaction
        if (request.InitialDeposit > 0)
        {
            var transactionReference = $"DEP{DateTime.UtcNow:yyyyMMddHHmmss}";
            var transaction = SavingsTransaction.Create(
                savingsAccount.Id,
                transactionReference,
                SavingsTransaction.TypeDeposit,
                request.InitialDeposit,
                savingsAccount.Balance,
                null,
                "Initial deposit",
                "CASH"
            );
            await transactionRepository.AddAsync(transaction, cancellationToken).ConfigureAwait(false);
        }

        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await transactionRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Savings account {AccountNumber} created for member {MemberId}", savingsAccount.AccountNumber, savingsAccount.MemberId);

        return new CreateSavingsAccountResponse(savingsAccount.Id, savingsAccount.AccountNumber, savingsAccount.Balance);
    }

    private async Task<string> GenerateUniqueAccountNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"SA{date:yyyyMMdd}";
        var sequence = 1;

        while (true)
        {
            var accountNumber = $"{prefix}{sequence:D4}";
            var exists = await repository.AnyAsync(
                new SavingsAccountByAccountNumberSpec(accountNumber), cancellationToken).ConfigureAwait(false);

            if (!exists)
            {
                return accountNumber;
            }

            sequence++;
        }
    }
}
