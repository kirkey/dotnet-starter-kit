// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/InsurancePolicies/Search/v1/SearchInsurancePoliciesSpecs.cs
using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Search.v1;

public class SearchInsurancePoliciesSpecs : EntitiesByPaginationFilterSpec<InsurancePolicy, InsurancePolicyResponse>
{
    public SearchInsurancePoliciesSpecs(SearchInsurancePoliciesCommand command)
        : base(command) =>
        Query
            .OrderByDescending(p => p.CreatedOn, !command.HasOrderBy())
            .Where(p => p.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(p => p.InsuranceProductId == command.InsuranceProductId!.Value, command.InsuranceProductId.HasValue)
            .Where(p => p.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status));
}
