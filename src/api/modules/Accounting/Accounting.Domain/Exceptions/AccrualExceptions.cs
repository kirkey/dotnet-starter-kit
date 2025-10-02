// Accrual Exceptions

using FSH.Framework.Core.Exceptions;

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when an accrual is not found by ID.
/// </summary>
public sealed class AccrualByIdNotFoundException(DefaultIdType id) : NotFoundException($"accrual with id {id} not found");

/// <summary>
/// Exception thrown when an accrual is not found by accrual number.
/// </summary>
public sealed class AccrualByNumberNotFoundException(string accrualNumber) : NotFoundException($"accrual with number {accrualNumber} not found");

/// <summary>
/// Exception thrown when trying to create an accrual with a duplicate number.
/// </summary>
public sealed class DuplicateAccrualNumberException(string accrualNumber) : ConflictException($"accrual with number {accrualNumber} already exists");

/// <summary>
/// Exception thrown when an accrual amount is invalid (negative or zero).
/// </summary>
public sealed class InvalidAccrualAmountException() : ForbiddenException("accrual amount must be positive");

/// <summary>
/// Exception thrown when trying to reverse an already reversed accrual.
/// </summary>
public sealed class AccrualAlreadyReversedException(DefaultIdType id) : ForbiddenException($"accrual with id {id} is already reversed");

/// <summary>
/// Exception thrown when trying to modify a reversed accrual.
/// </summary>
public sealed class CannotModifyReversedAccrualException(DefaultIdType id) : ForbiddenException($"cannot modify reversed accrual with id {id}");

/// <summary>
/// Exception thrown when the accrual date is invalid.
/// </summary>
public sealed class InvalidAccrualDateException() : ForbiddenException("accrual date cannot be in the future");

/// <summary>
/// Exception thrown when the accrual number format is invalid.
/// </summary>
public sealed class InvalidAccrualNumberFormatException() : ForbiddenException("accrual number cannot be empty or contain only whitespace");
