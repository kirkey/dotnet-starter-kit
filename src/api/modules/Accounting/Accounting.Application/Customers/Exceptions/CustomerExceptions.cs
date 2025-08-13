using FSH.Framework.Core.Exceptions;

namespace Accounting.Application.Customers.Exceptions;

public class CustomerNotFoundException(DefaultIdType id) : NotFoundException($"Customer with ID {id} was not found.");

public class CustomerCodeAlreadyExistsException(string code)
    : ForbiddenException($"Customer with code '{code}' already exists.");

public class CustomerNameAlreadyExistsException(string name)
    : ForbiddenException($"Customer with name '{name}' already exists.");

public class InvalidCreditLimitException() : ForbiddenException("Credit limit must be greater than or equal to zero.");
