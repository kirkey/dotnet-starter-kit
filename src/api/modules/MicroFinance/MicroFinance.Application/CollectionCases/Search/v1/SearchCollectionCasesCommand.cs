using System.ComponentModel;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Search.v1;

/// <summary>
/// Command for searching collection cases with pagination and filtering capabilities.
/// Follows the CQRS pattern for query operations with comprehensive search functionality.
/// </summary>
public class SearchCollectionCasesCommand : PaginationFilter, IRequest<PagedList<CollectionCaseResponse>>
{
    /// <summary>
    /// Filter by case status (OPEN, ASSIGNED, IN_PROGRESS, PROMISE_TO_PAY, LEGAL, RECOVERED, WRITTEN_OFF, SETTLED, CLOSED).
    /// </summary>
    [DefaultValue("")]
    public string? Status { get; set; }

    /// <summary>
    /// Filter by loan ID.
    /// </summary>
    public DefaultIdType? LoanId { get; set; }

    /// <summary>
    /// Filter by member ID (borrower).
    /// </summary>
    public DefaultIdType? MemberId { get; set; }

    /// <summary>
    /// Filter by assigned collector ID.
    /// </summary>
    public DefaultIdType? AssignedToId { get; set; }

    /// <summary>
    /// Filter by priority level (LOW, MEDIUM, HIGH, CRITICAL).
    /// </summary>
    [DefaultValue("")]
    public string? Priority { get; set; }

    /// <summary>
    /// Filter by loan classification (CURRENT, WATCH, SUBSTANDARD, DOUBTFUL, LOSS).
    /// </summary>
    [DefaultValue("")]
    public string? Classification { get; set; }
}
