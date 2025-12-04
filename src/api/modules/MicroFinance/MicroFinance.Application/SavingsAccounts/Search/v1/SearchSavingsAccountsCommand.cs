using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Search.v1;

/// <summary>
/// Command to search savings accounts with pagination.
/// </summary>
public sealed class SearchSavingsAccountsCommand : PaginationFilter, IRequest<PagedList<SavingsAccountResponse>>
{
    /// <summary>Filter by member ID.</summary>
    public Guid? MemberId { get; set; }

    /// <summary>Filter by savings product ID.</summary>
    public Guid? SavingsProductId { get; set; }

    /// <summary>Filter by account status.</summary>
    public string? Status { get; set; }
}
