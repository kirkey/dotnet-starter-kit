using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.CycleCount;

public sealed class CycleCountNotFoundException(DefaultIdType id)
    : NotFoundException($"Cycle Count with ID '{id}' was not found.") {}

public sealed class CycleCountNotFoundByNumberException(string countNumber)
    : NotFoundException($"Cycle Count with Number '{countNumber}' was not found.") {}

