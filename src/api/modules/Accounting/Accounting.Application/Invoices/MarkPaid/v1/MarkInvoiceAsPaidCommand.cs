namespace Accounting.Application.Invoices.MarkPaid.v1;

/// <summary>
/// Command to mark an invoice as fully paid.
/// </summary>
/// <param name="InvoiceId">Invoice identifier.</param>
/// <param name="PaidDate">Date when payment was received.</param>
/// <param name="PaymentMethod">Method used for payment (e.g., Cash, Check, Credit Card).</param>
public sealed record MarkInvoiceAsPaidCommand(
    DefaultIdType InvoiceId,
    DateTime PaidDate,
    string? PaymentMethod = null
) : IRequest<MarkInvoiceAsPaidResponse>;

