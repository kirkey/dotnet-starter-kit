using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.Customer;

public sealed class DuplicateCustomerCodeException(string code)
    : ConflictException($"Customer with Code '{code}' already exists.") {}
