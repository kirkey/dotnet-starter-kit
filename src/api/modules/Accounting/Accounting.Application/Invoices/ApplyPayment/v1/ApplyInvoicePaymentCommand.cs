namespace Accounting.Application.Invoices.ApplyPayment.v1;

/// <summary>
/// Command to apply a partial payment to an invoice.
/// </summary>
/// <param name="InvoiceId">Invoice identifier.</param>
/// <param name="Amount">Payment amount to apply.</param>
/// <param name="PaymentDate">Date when payment was received.</param>
/// <param name="PaymentMethod">Method used for payment (e.g., Cash, Check, Credit Card).</param>
public sealed record ApplyInvoicePaymentCommand(
    DefaultIdType InvoiceId,
    decimal Amount,
    DateTime PaymentDate,
    string? PaymentMethod = null
) : IRequest<ApplyInvoicePaymentResponse>;

