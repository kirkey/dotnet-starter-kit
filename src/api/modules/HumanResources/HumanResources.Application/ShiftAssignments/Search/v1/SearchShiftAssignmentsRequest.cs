using FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Search.v1;

/// <summary>
/// Request to search shift assignments with pagination and filters.
/// </summary>
public sealed class SearchShiftAssignmentsRequest : PaginationFilter, IRequest<PagedList<ShiftAssignmentResponse>>
{
    /// <summary>Filter by employee ID.</summary>
    public DefaultIdType? EmployeeId { get; set; }

    /// <summary>Filter by shift ID.</summary>
    public DefaultIdType? ShiftId { get; set; }

    /// <summary>Filter by active status.</summary>
    public bool? IsActive { get; set; }

    /// <summary>Filter by recurring status.</summary>
    public bool? IsRecurring { get; set; }
}

