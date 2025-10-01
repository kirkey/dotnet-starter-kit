namespace Store.Domain.Exceptions.CustomerReturn;

public sealed class CustomerReturnNotFoundException(DefaultIdType id)
    : NotFoundException($"Customer Return with ID '{id}' was not found.") {}

