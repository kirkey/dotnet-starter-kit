using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Search.v1;

/// <summary>
/// Request to search employee dependents.
/// </summary>
public class SearchEmployeeDependentsRequest : PaginationFilter, IRequest<PagedList<EmployeeDependentResponse>>
{
    public DefaultIdType? EmployeeId { get; set; }
    public string? DependentType { get; set; }
    public bool? IsBeneficiary { get; set; }
    public bool? IsClaimableDependent { get; set; }
    public bool? IsActive { get; set; }
}

