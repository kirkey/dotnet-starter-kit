using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Get.v1;

public sealed class GetTimesheetHandler(
    [FromKeyedServices("hr:timesheets")] IReadRepository<Domain.Entities.Timesheet> repository)
    : IRequestHandler<GetTimesheetRequest, TimesheetResponse>
{
    public async Task<TimesheetResponse> Handle(
        GetTimesheetRequest request,
        CancellationToken cancellationToken)
    {
        var timesheet = await repository
            .FirstOrDefaultAsync(new TimesheetByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (timesheet is null)
            throw new TimesheetNotFoundException(request.Id);

        return new TimesheetResponse
        {
            Id = timesheet.Id,
            EmployeeId = timesheet.EmployeeId,
            StartDate = timesheet.StartDate,
            EndDate = timesheet.EndDate,
            PeriodType = timesheet.PeriodType,
            RegularHours = timesheet.RegularHours,
            OvertimeHours = timesheet.OvertimeHours,
            TotalHours = timesheet.TotalHours,
            Status = timesheet.Status,
            ApproverManagerId = timesheet.ApproverManagerId,
            SubmittedDate = timesheet.SubmittedDate,
            ApprovedDate = timesheet.ApprovedDate,
            ManagerComment = timesheet.ManagerComment,
            RejectionReason = timesheet.RejectionReason,
            IsLocked = timesheet.IsLocked,
            IsApproved = timesheet.IsApproved
        };
    }
}

