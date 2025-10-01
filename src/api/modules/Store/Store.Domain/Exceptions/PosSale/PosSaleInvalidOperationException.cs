namespace Store.Domain.Exceptions.PosSale;

public sealed class PosSaleInvalidOperationException(string message)
    : BadRequestException(message) {}

