using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.ShareTransactions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareTransactions.Search.v1;

public class SearchShareTransactionsSpecs : EntitiesByPaginationFilterSpec<ShareTransaction, ShareTransactionResponse>
{
    public SearchShareTransactionsSpecs(SearchShareTransactionsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(t => t.TransactionDate, !command.HasOrderBy())
            .Where(t => t.ShareAccountId == command.ShareAccountId!.Value, command.ShareAccountId.HasValue)
            .Where(t => t.Reference.Contains(command.Reference!), !string.IsNullOrWhiteSpace(command.Reference))
            .Where(t => t.TransactionType == command.TransactionType, !string.IsNullOrWhiteSpace(command.TransactionType))
            .Where(t => t.TransactionDate >= command.DateFrom!.Value, command.DateFrom.HasValue)
            .Where(t => t.TransactionDate <= command.DateTo!.Value, command.DateTo.HasValue);
}
