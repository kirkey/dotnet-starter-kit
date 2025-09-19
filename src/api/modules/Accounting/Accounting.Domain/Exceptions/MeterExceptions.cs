// Meter Exceptions

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when a meter is not found by ID.
/// </summary>
public sealed class MeterByIdNotFoundException(DefaultIdType id) : NotFoundException($"meter with id {id} not found");

/// <summary>
/// Exception thrown when a meter is not found by meter number.
/// </summary>
public sealed class MeterByNumberNotFoundException(string meterNumber) : NotFoundException($"meter with number {meterNumber} not found");

/// <summary>
/// Exception thrown when trying to create a meter with a duplicate number.
/// </summary>
public sealed class DuplicateMeterNumberException(string meterNumber) : ConflictException($"meter with number {meterNumber} already exists");

/// <summary>
/// Exception thrown when a meter reading is invalid.
/// </summary>
public sealed class InvalidMeterReadingException() : ForbiddenException("meter reading cannot be negative");

/// <summary>
/// Exception thrown when trying to record a reading that is less than the previous reading.
/// </summary>
public sealed class InvalidMeterReadingSequenceException(decimal previousReading, decimal currentReading) 
    : ForbiddenException($"current reading {currentReading} cannot be less than previous reading {previousReading}");

/// <summary>
/// Exception thrown when trying to modify a meter that is in use.
/// </summary>
public sealed class CannotModifyActiveMetedException(DefaultIdType id) : ForbiddenException($"cannot modify active meter with id {id}");

/// <summary>
/// Exception thrown when the meter installation date is invalid.
/// </summary>
public sealed class InvalidMeterInstallationDateException() : ForbiddenException("meter installation date cannot be in the future");

/// <summary>
/// Exception thrown when trying to remove a meter that has recent readings.
/// </summary>
public sealed class CannotRemoveMeterWithReadingsException(DefaultIdType id) : ForbiddenException($"cannot remove meter with id {id} that has recent readings");
