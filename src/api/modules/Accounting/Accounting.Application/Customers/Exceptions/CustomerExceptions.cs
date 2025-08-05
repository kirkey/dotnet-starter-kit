using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.Customers.Exceptions;

public class CustomerNotFoundException : NotFoundException
{
    public CustomerNotFoundException(DefaultIdType id) : base($"Customer with ID {id} was not found.")
    {
    }
}

public class CustomerCodeAlreadyExistsException : ForbiddenException
{
    public CustomerCodeAlreadyExistsException(string code) : base($"Customer with code '{code}' already exists.")
    {
    }
}

public class InvalidCreditLimitException : ForbiddenException
{
    public InvalidCreditLimitException() : base("Credit limit must be greater than or equal to zero.")
    {
    }
}
