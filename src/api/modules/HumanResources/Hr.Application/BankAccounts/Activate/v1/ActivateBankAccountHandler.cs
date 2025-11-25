namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Activate.v1;

/// <summary>
/// Handler for activating a bank account.
/// </summary>
public sealed class ActivateBankAccountHandler(
    ILogger<ActivateBankAccountHandler> logger,
    [FromKeyedServices("hr:bankaccounts")] IRepository<BankAccount> repository)
    : IRequestHandler<ActivateBankAccountCommand, ActivateBankAccountResponse>
{
    public async Task<ActivateBankAccountResponse> Handle(
        ActivateBankAccountCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (account is null)
            throw new BankAccountNotFoundException(request.Id);

        account.Activate();

        await repository.UpdateAsync(account, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Bank account {Id} activated for Employee {EmployeeId}",
            account.Id,
            account.EmployeeId);

        return new ActivateBankAccountResponse(
            account.Id,
            account.IsActive,
            account.BankName);
    }
}

