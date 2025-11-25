namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Deactivate.v1;

/// <summary>
/// Handler for deactivating a bank account.
/// Removes primary status if this account was set as primary.
/// </summary>
public sealed class DeactivateBankAccountHandler(
    ILogger<DeactivateBankAccountHandler> logger,
    [FromKeyedServices("hr:bankaccounts")] IRepository<BankAccount> repository)
    : IRequestHandler<DeactivateBankAccountCommand, DeactivateBankAccountResponse>
{
    public async Task<DeactivateBankAccountResponse> Handle(
        DeactivateBankAccountCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (account is null)
            throw new BankAccountNotFoundException(request.Id);

        account.Deactivate();

        await repository.UpdateAsync(account, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Bank account {Id} deactivated for Employee {EmployeeId}. Reason: {Reason}",
            account.Id,
            account.EmployeeId,
            request.Reason ?? "Not specified");

        return new DeactivateBankAccountResponse(
            account.Id,
            account.IsActive,
            account.BankName);
    }
}

