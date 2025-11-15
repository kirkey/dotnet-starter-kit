namespace FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Search.v1;

using Resp = FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Get.v1.TimesheetLineResponse;

/// <summary>
/// Request to search timesheet lines with pagination and filters.
/// </summary>
public sealed class SearchTimesheetLinesRequest : PaginationFilter, IRequest<PagedList<Resp>>
{
    /// <summary>Filter by timesheet ID.</summary>
    public DefaultIdType? TimesheetId { get; set; }

    /// <summary>Filter by specific work date.</summary>
    public DateTime? WorkDate { get; set; }

    /// <summary>Filter by date range start.</summary>
    public DateTime? FromDate { get; set; }

    /// <summary>Filter by date range end.</summary>
    public DateTime? ToDate { get; set; }

    /// <summary>Filter by project ID.</summary>
    public string? ProjectId { get; set; }

    /// <summary>Filter by billable status.</summary>
    public bool? IsBillable { get; set; }
}

