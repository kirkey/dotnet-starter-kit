// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/MobileTransactions/Search/v1/SearchMobileTransactionsCommand.cs
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Search.v1;

/// <summary>
/// Command for searching mobile transactions with pagination and filters.
/// </summary>
public class SearchMobileTransactionsCommand : PaginationFilter, IRequest<PagedList<MobileTransactionResponse>>
{
    /// <summary>
    /// Filter by wallet ID.
    /// </summary>
    public Guid? WalletId { get; set; }

    /// <summary>
    /// Filter by member ID.
    /// </summary>
    public Guid? MemberId { get; set; }

    /// <summary>
    /// Filter by transaction type.
    /// </summary>
    public string? TransactionType { get; set; }

    /// <summary>
    /// Filter by status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by transaction date from.
    /// </summary>
    public DateOnly? DateFrom { get; set; }

    /// <summary>
    /// Filter by transaction date to.
    /// </summary>
    public DateOnly? DateTo { get; set; }

    /// <summary>
    /// Filter by reference number.
    /// </summary>
    public string? ReferenceNumber { get; set; }
}

