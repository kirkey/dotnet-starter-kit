using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.Currencies.Exceptions;

public class CurrencyNotFoundException : NotFoundException
{
    public CurrencyNotFoundException(DefaultIdType id) : base($"Currency with ID {id} was not found.")
    {
    }
}

public class CurrencyCodeAlreadyExistsException : ConflictException
{
    public CurrencyCodeAlreadyExistsException(string code) : base($"Currency with code '{code}' already exists.")
    {
    }
}

public class CurrencyNameAlreadyExistsException : ConflictException
{
    public CurrencyNameAlreadyExistsException(string name) : base($"Currency with name '{name}' already exists.")
    {
    }
}

public class BaseCurrencyChangeNotAllowedException : BadRequestException
{
    public BaseCurrencyChangeNotAllowedException() : base("Cannot change the base currency designation when it's in use.")
    {
    }
}
