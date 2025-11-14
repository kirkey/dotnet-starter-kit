using FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Search.v1;

/// <summary>
/// Handler for searching bank accounts.
/// </summary>
public sealed class SearchBankAccountsHandler(
    [FromKeyedServices("hr:bankaccounts")] IReadRepository<BankAccount> repository)
    : IRequestHandler<SearchBankAccountsRequest, PagedList<BankAccountResponse>>
{
    public async Task<PagedList<BankAccountResponse>> Handle(
        SearchBankAccountsRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchBankAccountsSpec(request);
        var bankAccounts = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = bankAccounts.Select(MapToResponse).ToList();

        return new PagedList<BankAccountResponse>(responses, request.PageNumber, request.PageSize, totalCount);
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

