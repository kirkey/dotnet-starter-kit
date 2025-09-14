using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.Supplier;

public sealed class SupplierNotFoundException(DefaultIdType id)
    : NotFoundException($"Supplier with ID '{id}' was not found.") {}

