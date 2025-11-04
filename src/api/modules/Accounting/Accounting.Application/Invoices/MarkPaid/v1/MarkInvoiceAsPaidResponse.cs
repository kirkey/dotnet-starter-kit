namespace Accounting.Application.Invoices.MarkPaid.v1;

/// <summary>
/// Response after marking an invoice as paid.
/// </summary>
/// <param name="InvoiceId">Invoice identifier.</param>
/// <param name="Status">Updated invoice status.</param>
/// <param name="PaidAmount">Total amount paid.</param>
/// <param name="PaidDate">Date when payment was received.</param>
public sealed record MarkInvoiceAsPaidResponse(
    DefaultIdType InvoiceId,
    string Status,
    decimal PaidAmount,
    DateTime? PaidDate
);

