using FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Search.v1;

/// <summary>
/// Request to search payroll lines with filtering and pagination.
/// </summary>
public class SearchPayrollLinesRequest : PaginationFilter, IRequest<PagedList<PayrollLineResponse>>
{
    /// <summary>
    /// Gets or sets the payroll ID filter.
    /// </summary>
    public DefaultIdType? PayrollId { get; set; }

    /// <summary>
    /// Gets or sets the employee ID filter.
    /// </summary>
    public DefaultIdType? EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the minimum net pay filter.
    /// </summary>
    public decimal? MinNetPay { get; set; }

    /// <summary>
    /// Gets or sets the maximum net pay filter.
    /// </summary>
    public decimal? MaxNetPay { get; set; }
}

