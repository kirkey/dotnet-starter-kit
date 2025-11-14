namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Delete.v1;

/// <summary>
/// Handler for deleting a bank account.
/// </summary>
public sealed class DeleteBankAccountHandler(
    ILogger<DeleteBankAccountHandler> logger,
    [FromKeyedServices("hr:bankaccounts")] IRepository<BankAccount> repository)
    : IRequestHandler<DeleteBankAccountCommand, DeleteBankAccountResponse>
{
    public async Task<DeleteBankAccountResponse> Handle(
        DeleteBankAccountCommand request,
        CancellationToken cancellationToken)
    {
        var bankAccount = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (bankAccount is null)
            throw new Exception($"Bank account not found: {request.Id}");

        await repository.DeleteAsync(bankAccount, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Bank account {BankAccountId} deleted successfully", bankAccount.Id);

        return new DeleteBankAccountResponse(bankAccount.Id);
    }
}
