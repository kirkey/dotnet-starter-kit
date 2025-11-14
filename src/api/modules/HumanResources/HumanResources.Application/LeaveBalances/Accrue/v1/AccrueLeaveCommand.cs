namespace FSH.Starter.WebApi.HumanResources.Application.LeaveBalances.Accrue.v1;

/// <summary>
/// Command to accrue leave days for an employee's leave balance.
/// Used for monthly/quarterly/annual accrual processing per Philippines Labor Code.
/// </summary>
public sealed record AccrueLeaveCommand(
    DefaultIdType EmployeeId,
    DefaultIdType LeaveTypeId,
    [property: DefaultValue(2025)] int Year,
    [property: DefaultValue(1.0)] decimal DaysToAccrue
) : IRequest<AccrueLeaveResponse>;

/// <summary>
/// Response for leave accrual operation.
/// </summary>
public sealed record AccrueLeaveResponse(
    DefaultIdType Id,
    decimal TotalAccrued,
    decimal RemainingBalance);

