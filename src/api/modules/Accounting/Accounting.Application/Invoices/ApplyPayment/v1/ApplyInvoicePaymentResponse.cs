namespace Accounting.Application.Invoices.ApplyPayment.v1;

/// <summary>
/// Response after applying a payment to an invoice.
/// </summary>
/// <param name="InvoiceId">Invoice identifier.</param>
/// <param name="Status">Updated invoice status.</param>
/// <param name="TotalAmount">Total invoice amount.</param>
/// <param name="PaidAmount">Cumulative amount paid.</param>
/// <param name="OutstandingAmount">Remaining balance due.</param>
public sealed record ApplyInvoicePaymentResponse(
    DefaultIdType InvoiceId,
    string Status,
    decimal TotalAmount,
    decimal PaidAmount,
    decimal OutstandingAmount
);

