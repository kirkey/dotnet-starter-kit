// Consumption Exceptions

namespace Accounting.Domain.Exceptions;

public sealed class ConsumptionAlreadyExistsException(DefaultIdType meterId, DateTime date) : ForbiddenException($"Consumption data for meter {meterId} on {date:yyyy-MM-dd} already exists.");
public sealed class ConsumptionNotFoundException(DefaultIdType id) : NotFoundException($"Consumption data with Id {id} not found.");
public sealed class InvalidMeterReadingException() : ForbiddenException("meter reading is invalid (negative values are not allowed or previous/current inconsistent)."
);
