// Invoice Exceptions

using FSH.Framework.Core.Exceptions;

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when an invoice is not found by ID.
/// </summary>
public sealed class InvoiceByIdNotFoundException(DefaultIdType id) : NotFoundException($"invoice with id {id} not found");

/// <summary>
/// Exception thrown when an invoice is not found by invoice number.
/// </summary>
public sealed class InvoiceByNumberNotFoundException(string invoiceNumber) : NotFoundException($"invoice with number {invoiceNumber} not found");

/// <summary>
/// Exception thrown when trying to create an invoice with a duplicate number.
/// </summary>
public sealed class DuplicateInvoiceNumberException(string invoiceNumber) : ConflictException($"invoice with number {invoiceNumber} already exists");

/// <summary>
/// Exception thrown when an invoice amount is invalid (negative).
/// </summary>
public sealed class InvalidInvoiceAmountException() : ForbiddenException("invoice amount cannot be negative");

/// <summary>
/// Exception thrown when trying to modify a paid invoice.
/// </summary>
public sealed class CannotModifyPaidInvoiceException(DefaultIdType id) : ForbiddenException($"cannot modify paid invoice with id {id}");

/// <summary>
/// Exception thrown when trying to pay more than the outstanding balance.
/// </summary>
public sealed class PaymentExceedsOutstandingBalanceException(decimal outstandingBalance, decimal paymentAmount) 
    : ForbiddenException($"payment amount {paymentAmount:C} exceeds outstanding balance {outstandingBalance:C}");

/// <summary>
/// Exception thrown when the invoice date is invalid.
/// </summary>
public sealed class InvalidInvoiceDateException() : ForbiddenException("invoice date cannot be in the future");

/// <summary>
/// Exception thrown when trying to void an already paid invoice.
/// </summary>
public sealed class CannotVoidPaidInvoiceException(DefaultIdType id) : ForbiddenException($"cannot void paid invoice with id {id}");

/// <summary>
/// Exception thrown when the due date is before the invoice date.
/// </summary>
public sealed class InvalidDueDateException() : ForbiddenException("due date cannot be before invoice date");
