namespace Store.Domain.Exceptions.CycleCount;

/// <summary>
/// Exception thrown when a cycle count operation is attempted with an invalid status.
/// </summary>
public sealed class InvalidCycleCountStatusException : ForbiddenException
{
    public InvalidCycleCountStatusException(string currentStatus, string? message = null)
        : base(message ?? $"Cannot perform this operation on cycle count with status '{currentStatus}'.") { }
}

