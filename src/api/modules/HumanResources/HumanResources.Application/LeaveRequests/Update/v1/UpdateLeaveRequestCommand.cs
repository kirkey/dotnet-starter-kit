namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Update.v1;

public sealed record UpdateLeaveRequestCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] string? Status = null,
    [property: DefaultValue(null)] string? ApproverComment = null) : IRequest<UpdateLeaveRequestResponse>;

