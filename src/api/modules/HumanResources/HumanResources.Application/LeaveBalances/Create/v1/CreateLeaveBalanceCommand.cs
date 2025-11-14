namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Create.v1;

public sealed record CreateLeaveBalanceCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType LeaveTypeId,
    [property: DefaultValue(2025)] int Year,
    [property: DefaultValue(0)] decimal OpeningBalance = 0) : IRequest<CreateLeaveBalanceResponse>;

