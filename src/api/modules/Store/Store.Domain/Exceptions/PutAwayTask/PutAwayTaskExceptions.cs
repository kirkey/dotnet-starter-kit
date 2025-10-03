namespace Store.Domain.Exceptions.PutAwayTask;

/// <summary>
/// Exception thrown when a put-away task is not found.
/// </summary>
public class PutAwayTaskNotFoundException : NotFoundException
{
    public PutAwayTaskNotFoundException(DefaultIdType putAwayTaskId)
        : base($"Put-away task with ID '{putAwayTaskId}' was not found.")
    {
    }

    public PutAwayTaskNotFoundException(string taskNumber)
        : base($"Put-away task '{taskNumber}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when a put-away task cannot be modified.
/// </summary>
public class PutAwayTaskCannotBeModifiedException(DefaultIdType putAwayTaskId, string status)
    : BadRequestException($"Put-away task '{putAwayTaskId}' in status '{status}' cannot be modified.");

/// <summary>
/// Exception thrown when put-away task status transition is invalid.
/// </summary>
public class InvalidPutAwayTaskStatusException(string currentStatus, string attemptedStatus)
    : BadRequestException($"Cannot transition put-away task from '{currentStatus}' to '{attemptedStatus}'.");

/// <summary>
/// Exception thrown when a put-away task with the same number already exists.
/// </summary>
public class PutAwayTaskAlreadyExistsException(string taskNumber)
    : ConflictException($"Put-away task with number '{taskNumber}' already exists.");
