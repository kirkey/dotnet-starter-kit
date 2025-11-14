using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Search.v1;

/// <summary>
/// Request to search employee dependents with filtering and pagination.
/// </summary>
public class SearchEmployeeDependentsRequest : PaginationFilter, IRequest<PagedList<EmployeeDependentResponse>>
{
    /// <summary>
    /// Gets or sets the search string to filter dependents by name.
    /// </summary>
    public string? SearchString { get; set; }

    /// <summary>
    /// Gets or sets the employee ID to filter dependents for a specific employee.
    /// </summary>
    public DefaultIdType? EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the dependent type filter.
    /// </summary>
    public string? DependentType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by beneficiary status.
    /// </summary>
    public bool? IsBeneficiary { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by claimable status.
    /// </summary>
    public bool? IsClaimableDependent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by active status.
    /// </summary>
    public bool? IsActive { get; set; }
}

