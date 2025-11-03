// Bill Exceptions

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when a bill is not found by ID.
/// </summary>
public sealed class BillByIdNotFoundException(DefaultIdType id) : NotFoundException($"bill with id {id} not found");

/// <summary>
/// Exception thrown when a bill is not found by bill number.
/// </summary>
public sealed class BillByNumberNotFoundException(string billNumber) : NotFoundException($"bill with number {billNumber} not found");

/// <summary>
/// Exception thrown when trying to create a bill with a duplicate bill number.
/// </summary>
public sealed class DuplicateBillNumberException(string billNumber) : ConflictException($"bill with number {billNumber} already exists");

/// <summary>
/// Exception thrown when trying to modify an approved bill.
/// </summary>
public sealed class CannotModifyApprovedBillException(DefaultIdType id) : ForbiddenException($"cannot modify approved bill with id {id}");

/// <summary>
/// Exception thrown when trying to modify a paid bill.
/// </summary>
public sealed class CannotModifyPaidBillException(DefaultIdType id) : ForbiddenException($"cannot modify paid bill with id {id}");

/// <summary>
/// Exception thrown when trying to void a bill with payments applied.
/// </summary>
public sealed class CannotVoidBillWithPaymentsException(DefaultIdType id) : ForbiddenException($"cannot void bill with id {id} that has payments applied");

/// <summary>
/// Exception thrown when payment amount exceeds outstanding balance.
/// </summary>
public sealed class PaymentExceedsOutstandingBalanceException(decimal paymentAmount, decimal outstandingAmount) : ForbiddenException($"payment amount {paymentAmount:N2} exceeds outstanding balance {outstandingAmount:N2}");

/// <summary>
/// Exception thrown when trying to submit bill without line items.
/// </summary>
public sealed class CannotSubmitBillWithoutLineItemsException() : ForbiddenException("cannot submit bill with no line items");

/// <summary>
/// Exception thrown when trying to approve bill that is not pending approval.
/// </summary>
public sealed class CannotApproveBillNotPendingApprovalException(DefaultIdType id) : ForbiddenException($"cannot approve bill with id {id} that is not pending approval");

/// <summary>
/// Exception thrown when trying to reject bill that is not pending approval.
/// </summary>
public sealed class CannotRejectBillNotPendingApprovalException(DefaultIdType id) : ForbiddenException($"cannot reject bill with id {id} that is not pending approval");

/// <summary>
/// Exception thrown when trying to apply payment to unapproved bill.
/// </summary>
public sealed class CannotApplyPaymentToUnapprovedBillException(DefaultIdType id) : ForbiddenException($"cannot apply payment to unapproved bill with id {id}");

/// <summary>
/// Exception thrown when trying to revert approval on bill with payments.
/// </summary>
public sealed class CannotRevertApprovalWithPaymentsException(DefaultIdType id) : ForbiddenException($"cannot revert approval on bill with id {id} that has payments applied");

/// <summary>
/// Exception thrown when bill is not approved.
/// </summary>
public sealed class BillNotApprovedException(DefaultIdType id) : ForbiddenException($"bill with id {id} is not approved");

/// <summary>
/// Exception thrown when due date is before bill date.
/// </summary>
public sealed class DueDateBeforeBillDateException() : ForbiddenException("due date must be on or after bill date");

/// <summary>
/// Exception thrown when bill amounts are negative.
/// </summary>
public sealed class BillAmountCannotBeNegativeException(string fieldName) : ForbiddenException($"{fieldName} cannot be negative");

/// <summary>
/// Exception thrown when line item quantity or price is invalid.
/// </summary>
public sealed class InvalidBillLineItemException(string message) : ForbiddenException(message);

