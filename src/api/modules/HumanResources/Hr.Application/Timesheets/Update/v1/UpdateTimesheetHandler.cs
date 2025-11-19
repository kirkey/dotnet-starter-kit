namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Update.v1;

public sealed class UpdateTimesheetHandler(
    ILogger<UpdateTimesheetHandler> logger,
    [FromKeyedServices("hr:timesheets")] IRepository<Timesheet> repository)
    : IRequestHandler<UpdateTimesheetCommand, UpdateTimesheetResponse>
{
    public async Task<UpdateTimesheetResponse> Handle(
        UpdateTimesheetCommand request,
        CancellationToken cancellationToken)
    {
        var timesheet = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (timesheet is null)
            throw new TimesheetNotFoundException(request.Id);

        // Update status through appropriate workflow methods
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            switch (request.Status.ToLower())
            {
                case "submitted":
                    if (timesheet.Status == "Draft")
                        timesheet.Submit();
                    break;
                case "approved":
                    if (timesheet.Status == "Submitted")
                        timesheet.Approve();
                    break;
                case "rejected":
                    if (timesheet.Status == "Submitted")
                        timesheet.Reject("Rejected by manager");
                    break;
                case "locked":
                    timesheet.Lock();
                    break;
                case "draft":
                    if (timesheet.Status == "Rejected")
                        timesheet.ResetToDraft();
                    break;
            }
        }

        await repository.UpdateAsync(timesheet, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Timesheet {TimesheetId} updated successfully", timesheet.Id);

        return new UpdateTimesheetResponse(timesheet.Id);
    }
}

