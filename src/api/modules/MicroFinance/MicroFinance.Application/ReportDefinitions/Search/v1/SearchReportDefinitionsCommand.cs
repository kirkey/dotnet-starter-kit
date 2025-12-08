// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/ReportDefinitions/Search/v1/SearchReportDefinitionsCommand.cs
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Search.v1;

/// <summary>
/// Command for searching report definitions with pagination and filters.
/// </summary>
public class SearchReportDefinitionsCommand : PaginationFilter, IRequest<PagedList<ReportDefinitionResponse>>
{
    /// <summary>
    /// Filter by report name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Filter by code.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Filter by category.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Filter by status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by output format.
    /// </summary>
    public string? OutputFormat { get; set; }

    /// <summary>
    /// Filter by whether scheduled.
    /// </summary>
    public bool? IsScheduled { get; set; }
}

