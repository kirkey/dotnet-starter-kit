namespace Accounting.Domain.Exceptions;

// ExchangeRate Exceptions
public sealed class ExchangeRateNotFoundException(DefaultIdType id) : NotFoundException($"exchange rate with id {id} not found");
public sealed class ExchangeRateAlreadyActiveException(DefaultIdType id) : ForbiddenException($"exchange rate with id {id} is already active");
public sealed class ExchangeRateAlreadyInactiveException(DefaultIdType id) : ForbiddenException($"exchange rate with id {id} is already inactive");
public sealed class InvalidExchangeRateException(decimal rate) : ForbiddenException($"exchange rate must be positive, got: {rate}");
public sealed class SameCurrencyExchangeRateException() : ForbiddenException("from and to currencies cannot be the same");
