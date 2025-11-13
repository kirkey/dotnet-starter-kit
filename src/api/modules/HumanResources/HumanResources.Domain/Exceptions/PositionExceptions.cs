namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when a position is not found.
/// </summary>
public class PositionNotFoundException : NotFoundException
{
    public PositionNotFoundException(DefaultIdType id)
        : base($"Position with ID '{id}' was not found.")
    {
    }

    public PositionNotFoundException(string code)
        : base($"Position with code '{code}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when a position code already exists in an organizational unit.
/// </summary>
public class PositionCodeAlreadyExistsException : ConflictException
{
    public PositionCodeAlreadyExistsException(string code)
        : base($"Position with code '{code}' already exists in this organizational unit.")
    {
    }
}

