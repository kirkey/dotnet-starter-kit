using FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Get.v1;

/// <summary>
/// Handler for retrieving a bank account by ID.
/// </summary>
public sealed class GetBankAccountHandler(
    [FromKeyedServices("hr:bankaccounts")] IReadRepository<BankAccount> repository)
    : IRequestHandler<GetBankAccountRequest, BankAccountResponse>
{
    public async Task<BankAccountResponse> Handle(
        GetBankAccountRequest request,
        CancellationToken cancellationToken)
    {
        var bankAccount = await repository
            .FirstOrDefaultAsync(new BankAccountByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (bankAccount is null)
            throw new Exception($"Bank account not found: {request.Id}");

        return MapToResponse(bankAccount);
    }

    private static BankAccountResponse MapToResponse(BankAccount bankAccount)
    {
        return new BankAccountResponse
        {
            Id = bankAccount.Id,
            EmployeeId = bankAccount.EmployeeId,
            Last4Digits = bankAccount.Last4Digits,
            BankName = bankAccount.BankName,
            AccountType = bankAccount.AccountType,
            AccountHolderName = bankAccount.AccountHolderName,
            IsPrimary = bankAccount.IsPrimary,
            IsActive = bankAccount.IsActive,
            IsVerified = bankAccount.IsVerified,
            VerificationDate = bankAccount.VerificationDate,
            SwiftCode = bankAccount.SwiftCode,
            Iban = bankAccount.Iban,
            CurrencyCode = bankAccount.CurrencyCode,
            Notes = bankAccount.Notes
        };
    }
}

