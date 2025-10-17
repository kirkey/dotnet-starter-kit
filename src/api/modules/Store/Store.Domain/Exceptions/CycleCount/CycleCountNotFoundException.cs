namespace Store.Domain.Exceptions.CycleCount;

public sealed class CycleCountNotFoundException(DefaultIdType id)
    : NotFoundException($"Cycle Count with ID '{id}' was not found.") {}

public sealed class CycleCountNotFoundByNumberException(string countNumber)
    : NotFoundException($"Cycle Count with Number '{countNumber}' was not found.") {}

public sealed class CycleCountCannotBeModifiedException(DefaultIdType id, string status)
    : ConflictException($"Cycle Count with ID '{id}' cannot be modified because it is in '{status}' status. Only 'Scheduled' cycle counts can be updated.") {}

