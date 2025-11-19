namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Delete.v1;

public sealed class DeleteLeaveRequestHandler(
    ILogger<DeleteLeaveRequestHandler> logger,
    [FromKeyedServices("hr:leaverequests")] IRepository<LeaveRequest> repository)
    : IRequestHandler<DeleteLeaveRequestCommand, DeleteLeaveRequestResponse>
{
    public async Task<DeleteLeaveRequestResponse> Handle(
        DeleteLeaveRequestCommand request,
        CancellationToken cancellationToken)
    {
        var leaveRequest = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (leaveRequest is null)
            throw new LeaveRequestNotFoundException(request.Id);

        await repository.DeleteAsync(leaveRequest, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Leave request {LeaveRequestId} deleted successfully", leaveRequest.Id);

        return new DeleteLeaveRequestResponse(leaveRequest.Id);
    }
}
