namespace FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Search.v1;

using FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Get.v1;

/// <summary>
/// Request to search employee pay components with filtering and pagination.
/// </summary>
public sealed class SearchEmployeePayComponentsRequest : PaginationFilter, IRequest<PagedList<EmployeePayComponentResponse>>
{
    /// <summary>
    /// Filter by employee ID.
    /// </summary>
    public DefaultIdType? EmployeeId { get; set; }

    /// <summary>
    /// Filter by pay component ID.
    /// </summary>
    public DefaultIdType? PayComponentId { get; set; }

    /// <summary>
    /// Filter by assignment type (Standard, Override, Addition, OneTime).
    /// </summary>
    public string? AssignmentType { get; set; }

    /// <summary>
    /// Filter by active status.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Filter by recurring status.
    /// </summary>
    public bool? IsRecurring { get; set; }
}

