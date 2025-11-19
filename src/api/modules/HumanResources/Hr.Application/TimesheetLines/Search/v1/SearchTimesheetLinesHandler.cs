using FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Search.v1;

/// <summary>
/// Handler for searching timesheet lines.
/// </summary>
public sealed class SearchTimesheetLinesHandler(
    ILogger<SearchTimesheetLinesHandler> logger,
    [FromKeyedServices("hr:timesheetlines")] IReadRepository<TimesheetLine> repository)
    : IRequestHandler<SearchTimesheetLinesRequest, PagedList<TimesheetLineResponse>>
{
    /// <summary>
    /// Handles the request to search timesheet lines.
    /// </summary>
    public async Task<PagedList<TimesheetLineResponse>> Handle(
        SearchTimesheetLinesRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new TimesheetLineSearchSpec(request);
        var lines = await repository
            .ListAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        var totalCount = await repository
            .CountAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        logger.LogInformation(
            "Searched timesheet lines with page {PageNumber}, size {PageSize}, found {TotalCount}",
            request.PageNumber,
            request.PageSize,
            totalCount);

        var responses = lines.Select(e => new TimesheetLineResponse
        {
            Id = e.Id,
            TimesheetId = e.TimesheetId,
            WorkDate = e.WorkDate,
            RegularHours = e.RegularHours,
            OvertimeHours = e.OvertimeHours,
            TotalHours = e.TotalHours,
            ProjectId = e.ProjectId,
            TaskDescription = e.TaskDescription,
            IsBillable = e.IsBillable,
            BillingRate = e.BillingRate
        }).ToList();

        return new PagedList<TimesheetLineResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}

