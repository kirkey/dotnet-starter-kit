using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Specifications;

/// <summary>
/// Specification to search employee dependents with filters.
/// </summary>
public class SearchEmployeeDependentsSpec : EntitiesByPaginationFilterSpec<EmployeeDependent, EmployeeDependentResponse>
{
    public SearchEmployeeDependentsSpec(SearchEmployeeDependentsRequest request)
        : base(request) =>
        Query
            .Where(d => d.EmployeeId == request.EmployeeId, request.EmployeeId.HasValue)
            .Where(d => d.DependentType == request.DependentType, !string.IsNullOrWhiteSpace(request.DependentType))
            .Where(d => d.IsBeneficiary == request.IsBeneficiary, request.IsBeneficiary.HasValue)
            .Where(d => d.IsClaimableDependent == request.IsClaimableDependent, request.IsClaimableDependent.HasValue)
            .Where(d => d.IsActive == request.IsActive, request.IsActive.HasValue)
            .OrderBy(d => d.FirstName, !request.HasOrderBy());
}

