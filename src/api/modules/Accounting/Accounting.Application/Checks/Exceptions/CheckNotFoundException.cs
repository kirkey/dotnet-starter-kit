namespace Accounting.Application.Checks.Exceptions;

/// <summary>
/// Exception thrown when a check is not found.
/// </summary>
public class CheckNotFoundException : Exception
{
    public CheckNotFoundException(DefaultIdType checkId)
        : base($"Check with ID '{checkId}' was not found.")
    {
    }

    public CheckNotFoundException(string checkNumber)
        : base($"Check with number '{checkNumber}' was not found.")
    {
    }
}

