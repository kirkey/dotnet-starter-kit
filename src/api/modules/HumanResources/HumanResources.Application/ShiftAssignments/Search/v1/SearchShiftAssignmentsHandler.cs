using FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Search.v1;

/// <summary>
/// Handler for searching shift assignments.
/// </summary>
public sealed class SearchShiftAssignmentsHandler(
    ILogger<SearchShiftAssignmentsHandler> logger,
    [FromKeyedServices("hr:shiftassignments")] IReadRepository<ShiftAssignment> repository)
    : IRequestHandler<SearchShiftAssignmentsRequest, PagedList<ShiftAssignmentResponse>>
{
    public async Task<PagedList<ShiftAssignmentResponse>> Handle(
        SearchShiftAssignmentsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new ShiftAssignmentSearchSpec(request);

        var assignments = await repository
            .ListAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        var totalCount = await repository
            .CountAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        logger.LogInformation(
            "Searched shift assignments with page {PageNumber}, size {PageSize}",
            request.PageNumber,
            request.PageSize);

        var responses = assignments.Select(a => new ShiftAssignmentResponse
        {
            Id = a.Id,
            EmployeeId = a.EmployeeId,
            EmployeeName = a.Employee?.FullName,
            ShiftId = a.ShiftId,
            ShiftName = a.Shift?.ShiftName,
            ShiftStartTime = a.Shift?.StartTime,
            ShiftEndTime = a.Shift?.EndTime,
            StartDate = a.StartDate,
            EndDate = a.EndDate,
            IsRecurring = a.IsRecurring,
            RecurringDayOfWeek = a.RecurringDayOfWeek,
            Notes = a.Notes,
            IsActive = a.IsActive
        }).ToList();

        return new PagedList<ShiftAssignmentResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}

