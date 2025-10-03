namespace Store.Domain.Exceptions.LotNumber;

/// <summary>
/// Exception thrown when a lot number is not found.
/// </summary>
public class LotNumberNotFoundException : NotFoundException
{
    public LotNumberNotFoundException(DefaultIdType lotNumberId)
        : base($"Lot number with ID '{lotNumberId}' was not found.")
    {
    }

    public LotNumberNotFoundException(string lotCode, DefaultIdType itemId)
        : base($"Lot number '{lotCode}' for item '{itemId}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when attempting to create a lot number with a duplicate code.
/// </summary>
public class DuplicateLotNumberException : ConflictException
{
    public DuplicateLotNumberException(string lotCode, DefaultIdType itemId)
        : base($"Lot number '{lotCode}' already exists for item '{itemId}'.")
    {
    }
}

/// <summary>
/// Exception thrown when trying to use an expired lot.
/// </summary>
public class LotNumberExpiredException(string lotCode, DateTime expirationDate)
    : BadRequestException($"Lot number '{lotCode}' expired on {expirationDate:yyyy-MM-dd}.");

/// <summary>
/// Exception thrown when insufficient quantity in lot.
/// </summary>
public class InsufficientLotQuantityException(string lotCode, int available, int required)
    : BadRequestException($"Lot '{lotCode}' has insufficient quantity. Available: {available}, Required: {required}");
