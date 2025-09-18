// DepreciationMethodExceptions.cs
// Per-domain exceptions for Depreciation Methods

namespace Accounting.Domain.Exceptions;

public sealed class DepreciationMethodNotFoundException(DefaultIdType id) : NotFoundException($"depreciation method with id {id} not found");
public sealed class DepreciationMethodAlreadyActiveException(DefaultIdType id) : ForbiddenException($"depreciation method with id {id} is already active");
public sealed class DepreciationMethodAlreadyInactiveException(DefaultIdType id) : ForbiddenException($"depreciation method with id {id} is already inactive");