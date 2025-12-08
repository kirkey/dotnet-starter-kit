using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.ShareTransactions.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareTransactions.Search.v1;

public class SearchShareTransactionsCommand : PaginationFilter, IRequest<PagedList<ShareTransactionResponse>>
{
    public DefaultIdType? ShareAccountId { get; set; }
    public string? Reference { get; set; }
    public string? TransactionType { get; set; }
    public DateOnly? DateFrom { get; set; }
    public DateOnly? DateTo { get; set; }
}
