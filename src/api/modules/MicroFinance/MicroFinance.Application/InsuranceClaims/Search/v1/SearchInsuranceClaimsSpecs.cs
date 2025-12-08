using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Search.v1;

public class SearchInsuranceClaimsSpecs : EntitiesByPaginationFilterSpec<InsuranceClaim, InsuranceClaimResponse>
{
    public SearchInsuranceClaimsSpecs(SearchInsuranceClaimsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(c => c.CreatedOn, !command.HasOrderBy())
            .Where(c => c.InsurancePolicyId == command.InsurancePolicyId!.Value, command.InsurancePolicyId.HasValue)
            .Where(c => c.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(c => c.ClaimType == command.ClaimType, !string.IsNullOrWhiteSpace(command.ClaimType));
}
