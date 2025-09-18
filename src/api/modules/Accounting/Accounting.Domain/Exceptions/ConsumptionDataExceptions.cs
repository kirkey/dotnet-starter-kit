// ConsumptionData Exceptions

namespace Accounting.Domain.Exceptions;

public sealed class ConsumptionDataAlreadyExistsException(DefaultIdType meterId, DateTime date) : ForbiddenException($"Consumption data for meter {meterId} on {date:yyyy-MM-dd} already exists.");
public sealed class ConsumptionDataNotFoundException(DefaultIdType id) : NotFoundException($"Consumption data with Id {id} not found.");
public sealed class InvalidMeterReadingException() : ForbiddenException("meter reading is invalid (negative values are not allowed or previous/current inconsistent)."
);