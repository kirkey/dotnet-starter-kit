// Rate Schedule Exceptions

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when a rate schedule is not found by ID.
/// </summary>
public sealed class RateScheduleByIdNotFoundException(DefaultIdType id) : NotFoundException($"rate schedule with id {id} not found");

/// <summary>
/// Exception thrown when a rate schedule is not found by schedule name.
/// </summary>
public sealed class RateScheduleByNameNotFoundException(string scheduleName) : NotFoundException($"rate schedule with name '{scheduleName}' not found");

/// <summary>
/// Exception thrown when trying to create a rate schedule with a duplicate name.
/// </summary>
public sealed class DuplicateRateScheduleNameException(string scheduleName) : ConflictException($"rate schedule with name '{scheduleName}' already exists");

/// <summary>
/// Exception thrown when a rate amount is invalid (negative).
/// </summary>
public sealed class InvalidRateAmountException() : ForbiddenException("rate amount cannot be negative");

/// <summary>
/// Exception thrown when the effective date is invalid.
/// </summary>
public sealed class InvalidEffectiveDateException() : ForbiddenException("effective date cannot be in the past for new rate schedules");

/// <summary>
/// Exception thrown when trying to modify an active rate schedule.
/// </summary>
public sealed class CannotModifyActiveRateScheduleException(DefaultIdType id) : ForbiddenException($"cannot modify active rate schedule with id {id}");

/// <summary>
/// Exception thrown when rate tiers are overlapping or invalid.
/// </summary>
public sealed class InvalidRateTiersException() : ForbiddenException("rate tiers must be sequential and non-overlapping");

/// <summary>
/// Exception thrown when trying to delete a rate schedule that is in use.
/// </summary>
public sealed class CannotDeleteActiveRateScheduleException(DefaultIdType id) : ForbiddenException($"cannot delete rate schedule with id {id} that is currently in use");
