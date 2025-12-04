using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Get.v1;

/// <summary>
/// Handler for getting a savings account by ID.
/// </summary>
public sealed class GetSavingsAccountHandler(
    [FromKeyedServices("microfinance:savingsaccounts")] IReadRepository<SavingsAccount> repository)
    : IRequestHandler<GetSavingsAccountRequest, SavingsAccountResponse>
{
    public async Task<SavingsAccountResponse> Handle(GetSavingsAccountRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository.FirstOrDefaultAsync(
            new SavingsAccountByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (account is null)
        {
            throw new NotFoundException($"Savings account with ID {request.Id} not found.");
        }

        return new SavingsAccountResponse(
            account.Id,
            account.AccountNumber,
            account.MemberId,
            account.Member?.FullName,
            account.SavingsProductId,
            account.SavingsProduct?.Name,
            account.Balance,
            account.TotalDeposits,
            account.TotalWithdrawals,
            account.TotalInterestEarned,
            account.OpenedDate,
            account.ClosedDate,
            account.LastInterestPostingDate,
            account.Status,
            account.Notes);
    }
}
