namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Delete.v1;

public sealed record DeleteLeaveRequestCommand(DefaultIdType Id) : IRequest<DeleteLeaveRequestResponse>;

