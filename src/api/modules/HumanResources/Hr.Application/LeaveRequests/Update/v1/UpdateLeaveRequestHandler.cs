namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Update.v1;

public sealed class UpdateLeaveRequestHandler(
    ILogger<UpdateLeaveRequestHandler> logger,
    [FromKeyedServices("hr:leaverequests")] IRepository<LeaveRequest> repository)
    : IRequestHandler<UpdateLeaveRequestCommand, UpdateLeaveRequestResponse>
{
    public async Task<UpdateLeaveRequestResponse> Handle(
        UpdateLeaveRequestCommand request,
        CancellationToken cancellationToken)
    {
        var leaveRequest = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (leaveRequest is null)
            throw new LeaveRequestNotFoundException(request.Id);

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            switch (request.Status.ToLower())
            {
                case "approved":
                    leaveRequest.Approve(request.ApproverComment);
                    break;
                case "rejected":
                    leaveRequest.Reject(request.ApproverComment ?? "Rejected by manager");
                    break;
                case "cancelled":
                    leaveRequest.Cancel();
                    break;
            }
        }

        await repository.UpdateAsync(leaveRequest, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Leave request {LeaveRequestId} updated successfully with status {Status}", leaveRequest.Id, request.Status);

        return new UpdateLeaveRequestResponse(leaveRequest.Id);
    }
}

