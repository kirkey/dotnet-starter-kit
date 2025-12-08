using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Search.v1;

public class SearchCollateralInsurancesSpecs : EntitiesByPaginationFilterSpec<CollateralInsurance, CollateralInsuranceSummaryResponse>
{
    public SearchCollateralInsurancesSpecs(SearchCollateralInsurancesCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.CreatedOn, !command.HasOrderBy())
            .Where(x => x.CollateralId == command.CollateralId!.Value, command.CollateralId.HasValue)
            .Where(x => x.PolicyNumber.Contains(command.PolicyNumber!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.PolicyNumber))
            .Where(x => x.InsurerName.Contains(command.InsurerName!, StringComparison.OrdinalIgnoreCase), !string.IsNullOrWhiteSpace(command.InsurerName))
            .Where(x => x.InsuranceType == command.InsuranceType, !string.IsNullOrWhiteSpace(command.InsuranceType))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.EffectiveDate >= command.EffectiveDateFrom!.Value, command.EffectiveDateFrom.HasValue)
            .Where(x => x.EffectiveDate <= command.EffectiveDateTo!.Value, command.EffectiveDateTo.HasValue)
            .Where(x => x.ExpiryDate >= command.ExpiryDateFrom!.Value, command.ExpiryDateFrom.HasValue)
            .Where(x => x.ExpiryDate <= command.ExpiryDateTo!.Value, command.ExpiryDateTo.HasValue);
}
