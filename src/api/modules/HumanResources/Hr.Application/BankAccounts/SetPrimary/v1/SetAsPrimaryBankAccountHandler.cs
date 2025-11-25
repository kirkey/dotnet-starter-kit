namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.SetPrimary.v1;

/// <summary>
/// Handler for setting a bank account as primary.
/// Removes primary status from any other account for the same employee.
/// </summary>
public sealed class SetAsPrimaryBankAccountHandler(
    ILogger<SetAsPrimaryBankAccountHandler> logger,
    [FromKeyedServices("hr:bankaccounts")] IRepository<BankAccount> repository)
    : IRequestHandler<SetAsPrimaryBankAccountCommand, SetAsPrimaryBankAccountResponse>
{
    public async Task<SetAsPrimaryBankAccountResponse> Handle(
        SetAsPrimaryBankAccountCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (account is null)
            throw new BankAccountNotFoundException(request.Id);

        // Set as primary
        account.SetAsPrimary();

        await repository.UpdateAsync(account, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Bank account {Id} set as primary for Employee {EmployeeId}, Bank: {BankName}",
            account.Id,
            account.EmployeeId,
            account.BankName);

        return new SetAsPrimaryBankAccountResponse(
            account.Id,
            account.IsPrimary,
            account.BankName,
            account.Last4Digits);
    }
}

