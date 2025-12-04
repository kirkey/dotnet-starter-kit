using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsTransactions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsTransactions.Search.v1;

public class SearchSavingsTransactionsSpecs : EntitiesByPaginationFilterSpec<SavingsTransaction, SavingsTransactionResponse>
{
    public SearchSavingsTransactionsSpecs(SearchSavingsTransactionsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(t => t.TransactionDate, !command.HasOrderBy())
            .Where(t => t.SavingsAccountId == command.SavingsAccountId!.Value, command.SavingsAccountId.HasValue)
            .Where(t => t.Reference.Contains(command.Reference!), !string.IsNullOrWhiteSpace(command.Reference))
            .Where(t => t.TransactionType == command.TransactionType, !string.IsNullOrWhiteSpace(command.TransactionType))
            .Where(t => t.TransactionDate >= command.DateFrom!.Value, command.DateFrom.HasValue)
            .Where(t => t.TransactionDate <= command.DateTo!.Value, command.DateTo.HasValue);
}
