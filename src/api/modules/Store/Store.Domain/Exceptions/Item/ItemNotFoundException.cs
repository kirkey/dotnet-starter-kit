namespace Store.Domain.Exceptions.Item;

public sealed class ItemNotFoundException(DefaultIdType id)
    : NotFoundException($"Item with ID '{id}' was not found.") {}

