namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when deduction is not found.
/// </summary>
public class DeductionNotFoundException : NotFoundException
{
    public DeductionNotFoundException(DefaultIdType deductionId)
        : base($"Deduction with ID '{deductionId}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when deduction operation is invalid.
/// </summary>
public class InvalidDeductionOperationException : BadRequestException
{
    public InvalidDeductionOperationException(string message)
        : base(message)
    {
    }
}
