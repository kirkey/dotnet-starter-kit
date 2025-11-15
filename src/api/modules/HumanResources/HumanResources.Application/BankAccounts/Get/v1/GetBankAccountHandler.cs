namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Get.v1;

using Framework.Core.Persistence;
using Specifications;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Handler for getting bank account details.
/// </summary>
public sealed class GetBankAccountHandler(
    [FromKeyedServices("hr:bankaccounts")] IReadRepository<BankAccount> repository)
    : IRequestHandler<GetBankAccountRequest, BankAccountResponse>
{
    public async Task<BankAccountResponse> Handle(
        GetBankAccountRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new BankAccountByIdSpec(request.Id);
        var account = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (account is null)
            throw new BankAccountNotFoundException(request.Id);

        return new BankAccountResponse(
            account.Id,
            account.EmployeeId,
            account.BankName,
            account.Last4Digits,
            account.AccountType,
            account.AccountHolderName,
            account.IsPrimary,
            account.IsActive,
            account.IsVerified,
            account.VerificationDate,
            account.SwiftCode,
            account.Iban,
            account.CurrencyCode,
            account.Notes);
    }
}

