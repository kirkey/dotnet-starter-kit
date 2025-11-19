namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Delete.v1;

public sealed class DeleteLeaveTypeHandler(
    ILogger<DeleteLeaveTypeHandler> logger,
    [FromKeyedServices("hr:leavetypes")] IRepository<LeaveType> repository)
    : IRequestHandler<DeleteLeaveTypeCommand, DeleteLeaveTypeResponse>
{
    public async Task<DeleteLeaveTypeResponse> Handle(
        DeleteLeaveTypeCommand request,
        CancellationToken cancellationToken)
    {
        var leaveType = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (leaveType is null)
            throw new LeaveTypeNotFoundException(request.Id);

        await repository.DeleteAsync(leaveType, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Leave type {LeaveTypeId} deleted successfully", leaveType.Id);

        return new DeleteLeaveTypeResponse(leaveType.Id);
    }
}
