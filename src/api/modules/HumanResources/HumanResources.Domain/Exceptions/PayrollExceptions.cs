namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when payroll is not found.
/// </summary>
public class PayrollNotFoundException : NotFoundException
{
    public PayrollNotFoundException(DefaultIdType id)
        : base($"Payroll with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when payroll line is not found.
/// </summary>
public class PayrollLineNotFoundException : NotFoundException
{
    public PayrollLineNotFoundException(DefaultIdType id)
        : base($"Payroll line with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when benefit is not found.
/// </summary>
public class BenefitNotFoundException : NotFoundException
{
    public BenefitNotFoundException(DefaultIdType id)
        : base($"Benefit with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when benefit enrollment is not found.
/// </summary>
public class BenefitEnrollmentNotFoundException : NotFoundException
{
    public BenefitEnrollmentNotFoundException(DefaultIdType id)
        : base($"Benefit enrollment with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when benefit allocation is not found.
/// </summary>
public class BenefitAllocationNotFoundException : NotFoundException
{
    public BenefitAllocationNotFoundException(DefaultIdType id)
        : base($"Benefit allocation with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when performance review is not found.
/// </summary>
public class PerformanceReviewNotFoundException : NotFoundException
{
    public PerformanceReviewNotFoundException(DefaultIdType id)
        : base($"Performance review with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when holiday is not found.
/// </summary>
public class HolidayNotFoundException : NotFoundException
{
    public HolidayNotFoundException(DefaultIdType id)
        : base($"Holiday with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when pay component is not found.
/// </summary>
public class PayComponentNotFoundException : NotFoundException
{
    public PayComponentNotFoundException(DefaultIdType id)
        : base($"Pay component with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when tax bracket is not found.
/// </summary>
public class TaxBracketNotFoundException : NotFoundException
{
    public TaxBracketNotFoundException(DefaultIdType id)
        : base($"Tax bracket with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when pay component rate is not found.
/// </summary>
public class PayComponentRateNotFoundException : NotFoundException
{
    public PayComponentRateNotFoundException(DefaultIdType id)
        : base($"Pay component rate with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when employee pay component is not found.
/// </summary>
public class EmployeePayComponentNotFoundException : NotFoundException
{
    public EmployeePayComponentNotFoundException(DefaultIdType id)
        : base($"Employee pay component with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when employee has duplicate payroll line.
/// </summary>
public class DuplicatePayrollLineException : BadRequestException
{
    public DuplicatePayrollLineException(DefaultIdType employeeId)
        : base($"Employee '{employeeId}' already has a payroll line in this period.")
    {
    }
}
