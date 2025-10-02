// Fuel / Fuel Consumption Exceptions

using FSH.Framework.Core.Exceptions;

namespace Accounting.Domain.Exceptions;

public sealed class FuelConsumptionNotFoundException(DefaultIdType id) : NotFoundException($"fuel consumption record with id {id} not found");
public sealed class InvalidFuelTypeException(string message) : ForbiddenException(message);
public sealed class InvalidFuelQuantityException(string message) : ForbiddenException(message);
public sealed class InvalidFuelCostException(string message) : ForbiddenException(message);