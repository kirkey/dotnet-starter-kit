using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.PurchaseOrder;

public sealed class PurchaseOrderItemNotFoundException(DefaultIdType id)
    : NotFoundException($"Purchase order item with ID '{id}' was not found.") {}

