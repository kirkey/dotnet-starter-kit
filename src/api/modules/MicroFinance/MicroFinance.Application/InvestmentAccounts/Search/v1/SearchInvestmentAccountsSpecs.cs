using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Search.v1;

public class SearchInvestmentAccountsSpecs : EntitiesByPaginationFilterSpec<InvestmentAccount, InvestmentAccountResponse>
{
    public SearchInvestmentAccountsSpecs(SearchInvestmentAccountsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(a => a.CreatedOn, !command.HasOrderBy())
            .Where(a => a.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(a => a.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(a => a.RiskProfile == command.RiskProfile, !string.IsNullOrWhiteSpace(command.RiskProfile));
}
