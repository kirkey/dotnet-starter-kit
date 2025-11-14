namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Update.v1;

/// <summary>
/// Handler for updating bank account.
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
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (account is null)
            throw new BankAccountNotFoundException(request.Id);

        // Update account details if provided
        if (!string.IsNullOrWhiteSpace(request.BankName) || 
            !string.IsNullOrWhiteSpace(request.AccountHolderName) || 
            !string.IsNullOrWhiteSpace(request.SwiftCode) || 
            !string.IsNullOrWhiteSpace(request.Iban) || 
            request.Notes != null)
        {
            account.Update(
                request.BankName,
                request.AccountHolderName,
                request.SwiftCode,
                request.Iban,
                request.Notes);
        }

        // Update primary status if provided
        if (request.IsPrimary.HasValue)
        {
            if (request.IsPrimary.Value)
                account.SetAsPrimary();
            else
                account.RemovePrimaryStatus();
        }

        // Update active status if provided
        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                account.Activate();
            else
                account.Deactivate();
        }

        await repository.UpdateAsync(account, cancellationToken);

        logger.LogInformation(
            "Bank account {Id} updated: Primary {Primary}, Active {Active}",
            account.Id,
            account.IsPrimary,
            account.IsActive);

        return new UpdateBankAccountResponse(
            account.Id,
            account.BankName,
            account.Last4Digits,
            account.IsPrimary,
            account.IsActive);
    }
}

