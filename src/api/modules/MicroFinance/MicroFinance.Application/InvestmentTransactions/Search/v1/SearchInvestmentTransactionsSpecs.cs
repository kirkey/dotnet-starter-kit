using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Search.v1;

public class SearchInvestmentTransactionsSpecs : EntitiesByPaginationFilterSpec<InvestmentTransaction, InvestmentTransactionResponse>
{
    public SearchInvestmentTransactionsSpecs(SearchInvestmentTransactionsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(t => t.CreatedOn, !command.HasOrderBy())
            .Where(t => t.InvestmentAccountId == command.InvestmentAccountId!.Value, command.InvestmentAccountId.HasValue)
            .Where(t => t.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(t => t.TransactionType == command.TransactionType, !string.IsNullOrWhiteSpace(command.TransactionType));
}
