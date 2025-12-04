using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Search.v1;

public class SearchLoanProductsSpecs : EntitiesByPaginationFilterSpec<LoanProduct, LoanProductResponse>
{
    public SearchLoanProductsSpecs(SearchLoanProductsCommand command)
        : base(command) =>
        Query
            .OrderBy(lp => lp.Name, !command.HasOrderBy())
            .Where(lp => lp.Code == command.Code, !string.IsNullOrWhiteSpace(command.Code))
            .Where(lp => lp.Name.Contains(command.Name!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.Name))
            .Where(lp => lp.InterestMethod == command.InterestMethod, !string.IsNullOrWhiteSpace(command.InterestMethod))
            .Where(lp => lp.RepaymentFrequency == command.RepaymentFrequency, !string.IsNullOrWhiteSpace(command.RepaymentFrequency))
            .Where(lp => lp.InterestRate >= command.MinInterestRate!.Value, command.MinInterestRate.HasValue)
            .Where(lp => lp.InterestRate <= command.MaxInterestRate!.Value, command.MaxInterestRate.HasValue)
            .Where(lp => lp.IsActive == command.IsActive!.Value, command.IsActive.HasValue);
}
