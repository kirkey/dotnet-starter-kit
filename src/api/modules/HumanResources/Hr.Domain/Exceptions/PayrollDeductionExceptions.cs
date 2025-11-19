namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when a payroll deduction is not found.
/// </summary>
public sealed class PayrollDeductionNotFoundException(DefaultIdType deductionId)
    : NotFoundException($"Payroll deduction with ID '{deductionId}' was not found.")
{
}

