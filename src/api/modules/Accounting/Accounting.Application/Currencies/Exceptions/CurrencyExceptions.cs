using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.Currencies.Exceptions;

public class CurrencyNotFoundException(DefaultIdType id) : NotFoundException($"Currency with ID {id} was not found.");

public class CurrencyCodeAlreadyExistsException(string code)
    : ConflictException($"Currency with code '{code}' already exists.");

public class CurrencyNameAlreadyExistsException(string name)
    : ConflictException($"Currency with name '{name}' already exists.");

public class BaseCurrencyChangeNotAllowedException()
    : BadRequestException("Cannot change the base currency designation when it's in use.");
