using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Search.v1;

public class SearchLoanCollateralsSpecs : EntitiesByPaginationFilterSpec<LoanCollateral, LoanCollateralResponse>
{
    public SearchLoanCollateralsSpecs(SearchLoanCollateralsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(lc => lc.CreatedOn, !command.HasOrderBy())
            .Where(lc => lc.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(lc => lc.CollateralType == command.CollateralType, !string.IsNullOrWhiteSpace(command.CollateralType))
            .Where(lc => lc.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status));
}
