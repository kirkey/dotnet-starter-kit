namespace Store.Domain.Exceptions.PosSale;

public sealed class PosSaleNotFoundException(DefaultIdType id)
    : NotFoundException($"POS Sale with ID '{id}' was not found.") {}

