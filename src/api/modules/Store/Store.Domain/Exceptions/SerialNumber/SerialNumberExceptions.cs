namespace Store.Domain.Exceptions.SerialNumber;

/// <summary>
/// Exception thrown when a serial number is not found.
/// </summary>
public class SerialNumberNotFoundException : NotFoundException
{
    public SerialNumberNotFoundException(DefaultIdType serialNumberId)
        : base($"Serial number with ID '{serialNumberId}' was not found.")
    {
    }

    public SerialNumberNotFoundException(string serialNumberValue)
        : base($"Serial number '{serialNumberValue}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when attempting to create a serial number that already exists.
/// </summary>
public class DuplicateSerialNumberException : ConflictException
{
    public DuplicateSerialNumberException(string serialNumberValue)
        : base($"Serial number '{serialNumberValue}' already exists.")
    {
    }
}

/// <summary>
/// Exception thrown when serial number is in invalid status for operation.
/// </summary>
public class InvalidSerialNumberStatusException(string serialNumberValue, string currentStatus, string requiredStatus)
    : BadRequestException($"Serial number '{serialNumberValue}' is in '{currentStatus}' status but requires '{requiredStatus}' status for this operation.");
