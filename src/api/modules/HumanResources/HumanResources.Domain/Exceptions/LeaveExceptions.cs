namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when leave type is not found.
/// </summary>
public class LeaveTypeNotFoundException : NotFoundException
{
    public LeaveTypeNotFoundException(DefaultIdType id)
        : base($"Leave type with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when leave request is not found.
/// </summary>
public class LeaveRequestNotFoundException : NotFoundException
{
    public LeaveRequestNotFoundException(DefaultIdType id)
        : base($"Leave request with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when leave balance is not found.
/// </summary>
public class LeaveBalanceNotFoundException : NotFoundException
{
    public LeaveBalanceNotFoundException(DefaultIdType employeeId, DefaultIdType leaveTypeId, int year)
        : base($"Leave balance not found for employee '{employeeId}', leave type '{leaveTypeId}', year {year}.")
    {
    }
}

/// <summary>
/// Exception thrown when insufficient leave balance.
/// </summary>
public class InsufficientLeaveBalanceException : BadRequestException
{
    public InsufficientLeaveBalanceException(decimal requested, decimal available)
        : base($"Insufficient leave balance. Requested: {requested} days, Available: {available} days.")
    {
    }
}

/// <summary>
/// Exception thrown when leave request violates minimum notice requirement.
/// </summary>
public class MinimumNoticeViolatedException : BadRequestException
{
    public MinimumNoticeViolatedException(int minimumDays, int daysUntilLeave)
        : base($"Minimum notice of {minimumDays} days required. Leave starts in {daysUntilLeave} days.")
    {
    }
}

/// <summary>
/// Exception thrown when bank account is not found.
/// </summary>
public class BankAccountNotFoundException : NotFoundException
{
    public BankAccountNotFoundException(DefaultIdType id)
        : base($"Bank account with ID '{id}' was not found.")
    {
    }
}

