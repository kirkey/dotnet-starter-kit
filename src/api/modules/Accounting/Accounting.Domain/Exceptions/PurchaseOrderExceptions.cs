using FSH.Framework.Core.Exceptions;

namespace Accounting.Domain.Exceptions;

public sealed class PurchaseOrderNotFoundException(DefaultIdType id) 
    : NotFoundException($"Purchase order with id {id} not found");

public sealed class PurchaseOrderCannotBeModifiedException(DefaultIdType id) 
    : ForbiddenException($"Purchase order {id} has been approved and cannot be modified");

public sealed class PurchaseOrderAlreadyApprovedException(DefaultIdType id) 
    : ForbiddenException($"Purchase order {id} has already been approved");

public sealed class PurchaseOrderNotReceivedException(DefaultIdType id) 
    : ForbiddenException($"Purchase order {id} has not been received");

public sealed class PurchaseOrderCannotBeCancelledException(DefaultIdType id, string reason) 
    : ForbiddenException($"Purchase order {id} cannot be cancelled: {reason}");

public sealed class InvalidPurchaseOrderStatusException(string message) 
    : ForbiddenException(message);

public sealed class InvalidPurchaseOrderAmountException(string message) 
    : BadRequestException(message);

public sealed class PurchaseOrderBilledAmountExceedsReceivedException(DefaultIdType id) 
    : BadRequestException($"Billed amount exceeds received amount for purchase order {id}");
