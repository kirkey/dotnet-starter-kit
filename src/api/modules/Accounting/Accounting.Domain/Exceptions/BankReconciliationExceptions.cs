using FSH.Framework.Core.Exceptions;

namespace Accounting.Domain.Exceptions;

public sealed class BankReconciliationNotFoundException(DefaultIdType id) 
    : NotFoundException($"Bank reconciliation with id {id} not found");

public sealed class BankReconciliationCannotBeModifiedException(DefaultIdType id) 
    : ForbiddenException($"Bank reconciliation {id} has been reconciled and cannot be modified");

public sealed class BankReconciliationAlreadyReconciledException(DefaultIdType id) 
    : ForbiddenException($"Bank reconciliation {id} has already been reconciled");

public sealed class InvalidReconciliationStatusException(string message) 
    : ForbiddenException(message);

public sealed class ReconciliationBalanceMismatchException(string message) 
    : BadRequestException(message);

public sealed class BankReconciliationNotApprovedException(DefaultIdType id) 
    : ForbiddenException($"Bank reconciliation {id} has not been approved");

public sealed class InvalidReconciliationDateException(string message) 
    : BadRequestException(message);
