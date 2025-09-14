namespace Accounting.Domain.Exceptions;

// Currency Exceptions
public sealed class CurrencyByIdNotFoundException(DefaultIdType id) : NotFoundException($"currency with id {id} not found");
public sealed class CurrencyByCodeNotFoundException(string currencyCode) : NotFoundException($"currency with code {currencyCode} not found");
public sealed class CurrencyAlreadyActiveException(DefaultIdType id) : ForbiddenException($"currency with id {id} is already active");
public sealed class CurrencyAlreadyInactiveException(DefaultIdType id) : ForbiddenException($"currency with id {id} is already inactive");
public sealed class CurrencyAlreadyBaseCurrencyException(DefaultIdType id) : ForbiddenException($"currency with id {id} is already the base currency");
public sealed class CurrencyNotBaseCurrencyException(DefaultIdType id) : ForbiddenException($"currency with id {id} is not the base currency");
public sealed class CannotDeactivateBaseCurrencyException(DefaultIdType id) : ForbiddenException($"cannot deactivate base currency with id {id}");
public sealed class InvalidCurrencyCodeException(string code) : ForbiddenException($"invalid currency code: {code}. Must be a 3-character ISO code");
public sealed class InvalidCurrencyDecimalPlacesException(int places) : ForbiddenException($"invalid decimal places: {places}. Must be between 0 and 4");
