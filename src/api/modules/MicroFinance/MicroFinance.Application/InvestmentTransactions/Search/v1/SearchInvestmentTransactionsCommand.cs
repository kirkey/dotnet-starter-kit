using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Search.v1;

/// <summary>
/// Command to search investment transactions.
/// </summary>
public sealed record SearchInvestmentTransactionsCommand : PaginationFilter, IRequest<PagedList<InvestmentTransactionResponse>>
{
    /// <summary>
    /// Filter by investment account ID.
    /// </summary>
    public DefaultIdType? InvestmentAccountId { get; set; }

    /// <summary>
    /// Filter by status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by transaction type.
    /// </summary>
    public string? TransactionType { get; set; }
}

