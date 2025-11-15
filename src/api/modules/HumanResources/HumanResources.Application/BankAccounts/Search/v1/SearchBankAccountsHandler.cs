namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Search.v1;

using Framework.Core.Paging;
using Framework.Core.Persistence;
using Specifications;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Handler for searching bank accounts.
/// </summary>
public sealed class SearchBankAccountsHandler(
    [FromKeyedServices("hr:bankaccounts")] IReadRepository<BankAccount> repository)
    : IRequestHandler<SearchBankAccountsRequest, PagedList<BankAccountDto>>
{
    public async Task<PagedList<BankAccountDto>> Handle(
        SearchBankAccountsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchBankAccountsSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        var dtos = items.Select(a => new BankAccountDto(
            a.Id,
            a.EmployeeId,
            a.BankName,
            a.Last4Digits,
            a.AccountType,
            a.IsPrimary,
            a.IsActive,
            a.IsVerified)).ToList();

        return new PagedList<BankAccountDto>(
            dtos,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}

