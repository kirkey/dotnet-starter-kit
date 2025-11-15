using FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Search.v1;

using Resp = FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Get.v1.TimesheetLineResponse;

/// <summary>
/// Handler for searching timesheet lines.
/// </summary>
public sealed class SearchTimesheetLinesHandler(
    ILogger<SearchTimesheetLinesHandler> logger,
    [FromKeyedServices("hr:timesheetlines")] IReadRepository<TimesheetLine> repository)
    : IRequestHandler<SearchTimesheetLinesRequest, PagedList<Resp>>
{
    /// <summary>
    /// Handles the request to search timesheet lines.
    /// </summary>
    public async Task<PagedList<Resp>> Handle(
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

        var responses = lines.Select(e => new Resp
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

        return new PagedList<Resp>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}

