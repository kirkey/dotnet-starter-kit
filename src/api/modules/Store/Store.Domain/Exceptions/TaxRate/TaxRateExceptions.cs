namespace Store.Domain.Exceptions.TaxRate;

/// <summary>
/// Exception thrown when a tax rate is not found by ID.
/// </summary>
public sealed class TaxRateByIdNotFoundException(DefaultIdType id) : NotFoundException($"tax rate with id {id} not found");

/// <summary>
/// Exception thrown when a tax rate is not found by name.
/// </summary>
public sealed class TaxRateByNameNotFoundException(string name) : NotFoundException($"tax rate with name '{name}' not found");

/// <summary>
/// Exception thrown when trying to create a tax rate with a duplicate name.
/// </summary>
public sealed class DuplicateTaxRateNameException(string name) : ConflictException($"tax rate with name '{name}' already exists");

/// <summary>
/// Exception thrown when tax rate percentage is invalid.
/// </summary>
public sealed class InvalidTaxRatePercentageException() : ForbiddenException("tax rate must be between 0.00 and 1.00 (0% to 100%)");

/// <summary>
/// Exception thrown when tax rate name is invalid.
/// </summary>
public sealed class InvalidTaxRateNameException() : ForbiddenException("tax rate name cannot be empty and must not exceed 100 characters");

/// <summary>
/// Exception thrown when trying to delete a tax rate that is in use.
/// </summary>
public sealed class CannotDeleteActiveTaxRateException(DefaultIdType id) : ForbiddenException($"cannot delete tax rate with id {id} that is currently in use");
