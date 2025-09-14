namespace Store.Domain.Exceptions.Customer;

public sealed class CustomerNotFoundException(DefaultIdType id)
    : NotFoundException($"Customer with ID '{id}' was not found.") {}

