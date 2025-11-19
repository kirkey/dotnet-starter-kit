using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Search.v1;

/// <summary>
/// Request to search payrolls with filtering and pagination.
/// </summary>
public class SearchPayrollsRequest : PaginationFilter, IRequest<PagedList<PayrollResponse>>
{
    /// <summary>
    /// Gets or sets the start date for filtering.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date for filtering.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or sets the pay frequency filter (Weekly, BiWeekly, Monthly).
    /// </summary>
    public string? PayFrequency { get; set; }

    /// <summary>
    /// Gets or sets the status filter (Draft, Processing, Processed, Posted, Paid).
    /// </summary>
    public string? Status { get; set; }
}

