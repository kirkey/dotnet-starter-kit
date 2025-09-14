using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.SalesOrder;

public sealed class SalesOrderNotFoundException(DefaultIdType id)
    : NotFoundException($"Sales order with ID '{id}' was not found.") {}

