using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Search.v1;

/// <summary>
/// Command to search report generations.
/// </summary>
public sealed record SearchReportGenerationsCommand : PaginationFilter, IRequest<PagedList<ReportGenerationResponse>>
{
    /// <summary>
    /// Filter by report definition ID.
    /// </summary>
    public DefaultIdType? ReportDefinitionId { get; set; }

    /// <summary>
    /// Filter by status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by trigger type.
    /// </summary>
    public string? Trigger { get; set; }
}

