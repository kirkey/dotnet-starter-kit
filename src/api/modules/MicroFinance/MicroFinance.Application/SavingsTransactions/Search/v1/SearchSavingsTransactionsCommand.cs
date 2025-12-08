using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsTransactions.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsTransactions.Search.v1;

public class SearchSavingsTransactionsCommand : PaginationFilter, IRequest<PagedList<SavingsTransactionResponse>>
{
    public DefaultIdType? SavingsAccountId { get; set; }
    public string? Reference { get; set; }
    public string? TransactionType { get; set; }
    public DateOnly? DateFrom { get; set; }
    public DateOnly? DateTo { get; set; }
}
