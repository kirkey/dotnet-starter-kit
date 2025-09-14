using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.SalesOrder;

public sealed class SalesOrderItemNotFoundException(DefaultIdType id)
    : NotFoundException($"Sales order item with ID '{id}' was not found.") {}

