namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Update.v1;

public sealed class UpdateTimesheetHandler(
    ILogger<UpdateTimesheetHandler> logger,
    [FromKeyedServices("hr:timesheets")] IRepository<Domain.Entities.Timesheet> repository)
    : IRequestHandler<UpdateTimesheetCommand, UpdateTimesheetResponse>
{
    public async Task<UpdateTimesheetResponse> Handle(
        UpdateTimesheetCommand request,
        CancellationToken cancellationToken)
    {
        var timesheet = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (timesheet is null)
            throw new TimesheetNotFoundException(request.Id);

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            switch (request.Status)
            {
                case "Submitted":
                    timesheet.Submit(request.ApproverId);
                    break;

                case "Approved":
                    timesheet.Approve(request.ManagerComment);
                    break;

                case "Rejected":
                    timesheet.Reject(request.ManagerComment ?? "No reason provided");
                    break;

                case "Locked":
                    timesheet.Lock();
                    break;
            }
        }

        await repository.UpdateAsync(timesheet, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Timesheet {TimesheetId} updated successfully, Status: {Status}", timesheet.Id, timesheet.Status);

        return new UpdateTimesheetResponse(timesheet.Id);
    }
}

