using System.ComponentModel;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Search.v1;

/// <summary>
/// Command for searching AML alerts with pagination and filtering capabilities.
/// Follows the CQRS pattern for query operations with comprehensive search functionality.
/// </summary>
public class SearchAmlAlertsCommand : PaginationFilter, IRequest<PagedList<AmlAlertResponse>>
{
    /// <summary>
    /// Filter by alert status (NEW, INVESTIGATING, ESCALATED, CLEARED, CONFIRMED, SAR_FILED, CLOSED).
    /// </summary>
    [DefaultValue("")]
    public string? Status { get; set; }

    /// <summary>
    /// Filter by severity level (LOW, MEDIUM, HIGH, CRITICAL).
    /// </summary>
    [DefaultValue("")]
    public string? Severity { get; set; }

    /// <summary>
    /// Filter by alert type (LARGE_CASH, STRUCTURING, UNUSUAL_PATTERN, etc.).
    /// </summary>
    [DefaultValue("")]
    public string? AlertType { get; set; }

    /// <summary>
    /// Filter by member ID.
    /// </summary>
    public Guid? MemberId { get; set; }
}
