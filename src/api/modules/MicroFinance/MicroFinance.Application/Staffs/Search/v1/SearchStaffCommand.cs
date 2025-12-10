// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/Staff/Search/v1/SearchStaffCommand.cs

using FSH.Starter.WebApi.MicroFinance.Application.Staffs.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Application.Staffs.Search.v1;

/// <summary>
/// Command for searching staff members with pagination and filters.
/// </summary>
public class SearchStaffCommand : PaginationFilter, IRequest<PagedList<StaffResponse>>
{
    /// <summary>
    /// Filter by first name.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Filter by last name.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Filter by employee number.
    /// </summary>
    public string? EmployeeNumber { get; set; }

    /// <summary>
    /// Filter by email.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Filter by branch ID.
    /// </summary>
    public DefaultIdType? BranchId { get; set; }

    /// <summary>
    /// Filter by role.
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// Filter by status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by department.
    /// </summary>
    public string? Department { get; set; }
}

