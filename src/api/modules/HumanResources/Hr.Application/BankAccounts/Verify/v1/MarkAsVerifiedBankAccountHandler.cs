namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Verify.v1;

/// <summary>
/// Handler for verifying a bank account.
/// </summary>
public sealed class MarkAsVerifiedBankAccountHandler(
    ILogger<MarkAsVerifiedBankAccountHandler> logger,
    [FromKeyedServices("hr:bankaccounts")] IRepository<BankAccount> repository)
    : IRequestHandler<MarkAsVerifiedBankAccountCommand, MarkAsVerifiedBankAccountResponse>
{
    public async Task<MarkAsVerifiedBankAccountResponse> Handle(
        MarkAsVerifiedBankAccountCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository
            .GetByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (account is null)
            throw new BankAccountNotFoundException(request.Id);

        account.MarkAsVerified();

        // Update notes if provided
        if (!string.IsNullOrWhiteSpace(request.Notes))
            account.Update(notes: request.Notes);

        await repository.UpdateAsync(account, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Bank account {Id} marked as verified on {VerificationDate}",
            account.Id,
            account.VerificationDate);

        return new MarkAsVerifiedBankAccountResponse(
            account.Id,
            account.IsVerified,
            account.VerificationDate!.Value);
    }
}

