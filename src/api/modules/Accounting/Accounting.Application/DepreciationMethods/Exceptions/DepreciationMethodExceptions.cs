namespace Accounting.Application.DepreciationMethods.Exceptions;

public class DepreciationMethodNotFoundException(DefaultIdType id)
    : NotFoundException($"Depreciation Method with ID {id} was not found.");

public class DepreciationMethodCodeAlreadyExistsException(string code)
    : ConflictException($"Depreciation Method with code '{code}' already exists.");
