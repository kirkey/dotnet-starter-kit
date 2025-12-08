using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Search.v1;

/// <summary>
/// Command to search investment accounts.
/// </summary>
public sealed record SearchInvestmentAccountsCommand : PaginationFilter, IRequest<PagedList<InvestmentAccountResponse>>
{
    /// <summary>
    /// Filter by member ID.
    /// </summary>
    public DefaultIdType? MemberId { get; set; }

    /// <summary>
    /// Filter by status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by risk profile.
    /// </summary>
    public string? RiskProfile { get; set; }
}

