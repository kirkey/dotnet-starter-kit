using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Get.v1;

/// <summary>
/// Handler for retrieving a leave request by ID.
/// </summary>
public sealed class GetLeaveRequestHandler(
    [FromKeyedServices("hr:leaverequests")] IReadRepository<LeaveRequest> repository)
    : IRequestHandler<GetLeaveRequestRequest, LeaveRequestResponse>
{
    public async Task<LeaveRequestResponse> Handle(
        GetLeaveRequestRequest request,
        CancellationToken cancellationToken)
    {
        var leaveRequest = await repository
            .FirstOrDefaultAsync(new LeaveRequestByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (leaveRequest is null)
            throw new LeaveRequestNotFoundException(request.Id);

        return new LeaveRequestResponse
        {
            Id = leaveRequest.Id,
            EmployeeId = leaveRequest.EmployeeId,
            LeaveTypeId = leaveRequest.LeaveTypeId,
            StartDate = leaveRequest.StartDate,
            EndDate = leaveRequest.EndDate,
            NumberOfDays = leaveRequest.NumberOfDays,
            Reason = leaveRequest.Reason,
            Status = leaveRequest.Status,
            ApproverManagerId = leaveRequest.ApproverManagerId,
            SubmittedDate = leaveRequest.SubmittedDate,
            ReviewedDate = leaveRequest.ReviewedDate,
            ApproverComment = leaveRequest.ApproverComment,
            IsActive = leaveRequest.IsActive
        };
    }
}

