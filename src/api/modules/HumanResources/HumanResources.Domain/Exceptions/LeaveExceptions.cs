namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when leave type is not found.
/// </summary>
public class LeaveTypeNotFoundException(DefaultIdType id)
    : NotFoundException($"Leave type with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when leave request is not found.
/// </summary>
public class LeaveRequestNotFoundException(DefaultIdType id)
    : NotFoundException($"Leave request with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when leave balance is not found.
/// </summary>
public class LeaveBalanceNotFoundException(DefaultIdType employeeId, DefaultIdType leaveTypeId, int year)
    : NotFoundException(
        $"Leave balance not found for employee '{employeeId}', leave type '{leaveTypeId}', year {year}.");

/// <summary>
/// Exception thrown when insufficient leave balance.
/// </summary>
public class InsufficientLeaveBalanceException(decimal requested, decimal available)
    : BadRequestException($"Insufficient leave balance. Requested: {requested} days, Available: {available} days.");

/// <summary>
/// Exception thrown when leave request violates minimum notice requirement.
/// </summary>
public class MinimumNoticeViolatedException(int minimumDays, int daysUntilLeave)
    : BadRequestException($"Minimum notice of {minimumDays} days required. Leave starts in {daysUntilLeave} days.");

/// <summary>
/// Exception thrown when bank account is not found.
/// </summary>
public class BankAccountNotFoundException(DefaultIdType id)
    : NotFoundException($"Bank account with ID '{id}' was not found.");

