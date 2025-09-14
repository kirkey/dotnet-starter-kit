using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.PurchaseOrder;

public sealed class PurchaseOrderNotFoundException(DefaultIdType id)
    : NotFoundException($"Purchase Order with ID '{id}' was not found.") {}

