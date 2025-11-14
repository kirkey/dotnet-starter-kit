namespace FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Create.v1;

public sealed record CreateLeaveRequestCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType LeaveTypeId,
    [property: DefaultValue("2025-11-15")] DateTime StartDate,
    [property: DefaultValue("2025-11-20")] DateTime EndDate,
    [property: DefaultValue("")] string Reason = "",
    [property: DefaultValue(null)] DefaultIdType? ApproverManagerId = null) : IRequest<CreateLeaveRequestResponse>;

