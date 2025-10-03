namespace Store.Domain.Exceptions.PickList;

/// <summary>
/// Exception thrown when a pick list is not found.
/// </summary>
public class PickListNotFoundException : NotFoundException
{
    public PickListNotFoundException(DefaultIdType pickListId)
        : base($"Pick list with ID '{pickListId}' was not found.")
    {
    }

    public PickListNotFoundException(string pickListNumber)
        : base($"Pick list '{pickListNumber}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when a pick list cannot be modified.
/// </summary>
public class PickListCannotBeModifiedException(DefaultIdType pickListId, string status)
    : BadRequestException($"Pick list '{pickListId}' in status '{status}' cannot be modified.");

/// <summary>
/// Exception thrown when pick list status transition is invalid.
/// </summary>
public class InvalidPickListStatusException(string currentStatus, string attemptedStatus)
    : BadRequestException($"Cannot transition pick list from '{currentStatus}' to '{attemptedStatus}'.");

/// <summary>
/// Exception thrown when a pick list with the same number already exists.
/// </summary>
public class PickListAlreadyExistsException(string pickListNumber)
    : ConflictException($"Pick list with number '{pickListNumber}' already exists.");
