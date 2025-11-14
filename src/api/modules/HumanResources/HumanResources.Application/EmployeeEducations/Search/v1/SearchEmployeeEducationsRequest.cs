using FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Search.v1;

/// <summary>
/// Request to search employee education records with filtering and pagination.
/// </summary>
public class SearchEmployeeEducationsRequest : PaginationFilter, IRequest<PagedList<EmployeeEducationResponse>>
{
    /// <summary>
    /// Gets or sets the employee ID filter.
    /// </summary>
    public DefaultIdType? EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the education level filter.
    /// </summary>
    public string? EducationLevel { get; set; }

    /// <summary>
    /// Gets or sets the field of study search string.
    /// </summary>
    public string? FieldOfStudy { get; set; }

    /// <summary>
    /// Gets or sets the institution search string.
    /// </summary>
    public string? Institution { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by active education only.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by verified education only.
    /// </summary>
    public bool? IsVerified { get; set; }

    /// <summary>
    /// Gets or sets the graduation date from (start date).
    /// </summary>
    public DateTime? GraduationDateFrom { get; set; }

    /// <summary>
    /// Gets or sets the graduation date to (end date).
    /// </summary>
    public DateTime? GraduationDateTo { get; set; }
}

