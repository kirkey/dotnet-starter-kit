namespace Store.Domain.Exceptions.CashDrawer;

public sealed class CashDrawerSessionNotFoundException(DefaultIdType id)
    : NotFoundException($"Cash drawer session with ID '{id}' was not found.") {}

