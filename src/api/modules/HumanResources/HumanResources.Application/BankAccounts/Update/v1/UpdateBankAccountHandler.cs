namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Update.v1;

/// <summary>
/// Handler for updating a bank account.
/// </summary>
public sealed class UpdateBankAccountHandler(
    ILogger<UpdateBankAccountHandler> logger,
    [FromKeyedServices("hr:bankaccounts")] IRepository<BankAccount> repository)
    : IRequestHandler<UpdateBankAccountCommand, UpdateBankAccountResponse>
{
    public async Task<UpdateBankAccountResponse> Handle(
        UpdateBankAccountCommand request,
        CancellationToken cancellationToken)
    {
        var bankAccount = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (bankAccount is null)
            throw new Exception($"Bank account not found: {request.Id}");

        bankAccount.Update(
            bankName: request.BankName,
            accountHolderName: request.AccountHolderName,
            swiftCode: request.SwiftCode,
            iban: request.Iban,
            notes: request.Notes);

        if (request.SetAsPrimary)
            bankAccount.SetAsPrimary();

        if (request.MarkAsVerified)
            bankAccount.MarkAsVerified();

        await repository.UpdateAsync(bankAccount, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Bank account {BankAccountId} updated successfully", bankAccount.Id);

        return new UpdateBankAccountResponse(bankAccount.Id);
    }
}

