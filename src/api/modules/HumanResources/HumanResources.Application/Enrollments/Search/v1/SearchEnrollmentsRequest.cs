using FSH.Starter.WebApi.HumanResources.Application.Enrollments.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Search.v1;

/// <summary>
/// Request to search enrollments with filtering and pagination.
/// </summary>
public class SearchEnrollmentsRequest : PaginationFilter, IRequest<PagedList<EnrollmentResponse>>
{
    /// <summary>
    /// Gets or sets the employee ID filter.
    /// </summary>
    public DefaultIdType? EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the benefit ID filter.
    /// </summary>
    public DefaultIdType? BenefitId { get; set; }

    /// <summary>
    /// Gets or sets the coverage level filter.
    /// </summary>
    public string? CoverageLevel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by active enrollments only.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets the enrollment date from (start date).
    /// </summary>
    public DateTime? EnrollmentDateFrom { get; set; }

    /// <summary>
    /// Gets or sets the enrollment date to (end date).
    /// </summary>
    public DateTime? EnrollmentDateTo { get; set; }
}

