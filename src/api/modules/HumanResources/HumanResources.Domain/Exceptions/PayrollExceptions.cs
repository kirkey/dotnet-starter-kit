namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when payroll is not found.
/// </summary>
public class PayrollNotFoundException(DefaultIdType id) : NotFoundException($"Payroll with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when payroll line is not found.
/// </summary>
public class PayrollLineNotFoundException(DefaultIdType id)
    : NotFoundException($"Payroll line with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when benefit is not found.
/// </summary>
public class BenefitNotFoundException(DefaultIdType id) : NotFoundException($"Benefit with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when benefit enrollment is not found.
/// </summary>
public class BenefitEnrollmentNotFoundException(DefaultIdType id)
    : NotFoundException($"Benefit enrollment with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when benefit allocation is not found.
/// </summary>
public class BenefitAllocationNotFoundException(DefaultIdType id)
    : NotFoundException($"Benefit allocation with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when performance review is not found.
/// </summary>
public class PerformanceReviewNotFoundException(DefaultIdType id)
    : NotFoundException($"Performance review with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when holiday is not found.
/// </summary>
public class HolidayNotFoundException(DefaultIdType id) : NotFoundException($"Holiday with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when pay component is not found.
/// </summary>
public class PayComponentNotFoundException(DefaultIdType id)
    : NotFoundException($"Pay component with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when tax bracket is not found.
/// </summary>
public class TaxBracketNotFoundException(DefaultIdType id)
    : NotFoundException($"Tax bracket with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when pay component rate is not found.
/// </summary>
public class PayComponentRateNotFoundException(DefaultIdType id)
    : NotFoundException($"Pay component rate with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when employee pay component is not found.
/// </summary>
public class EmployeePayComponentNotFoundException(DefaultIdType id)
    : NotFoundException($"Employee pay component with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when employee has duplicate payroll line.
/// </summary>
public class DuplicatePayrollLineException(DefaultIdType employeeId)
    : BadRequestException($"Employee '{employeeId}' already has a payroll line in this period.");
