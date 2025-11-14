namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Delete.v1;

public sealed class DeleteTimesheetHandler(
    ILogger<DeleteTimesheetHandler> logger,
    [FromKeyedServices("hr:timesheets")] IRepository<Timesheet> repository)
    : IRequestHandler<DeleteTimesheetCommand, DeleteTimesheetResponse>
{
    public async Task<DeleteTimesheetResponse> Handle(
        DeleteTimesheetCommand request,
        CancellationToken cancellationToken)
    {
        var timesheet = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (timesheet is null)
            throw new TimesheetNotFoundException(request.Id);

        await repository.DeleteAsync(timesheet, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Timesheet {TimesheetId} deleted successfully", timesheet.Id);

        return new DeleteTimesheetResponse(timesheet.Id);
    }
}
