namespace Accounting.Application.Invoices.Void.v1;

/// <summary>
/// Command to void an invoice.
/// </summary>
/// <param name="InvoiceId">Invoice identifier.</param>
/// <param name="Reason">Reason for voiding.</param>
public sealed record VoidInvoiceCommand(
    DefaultIdType InvoiceId,
    string? Reason = null
) : IRequest<VoidInvoiceResponse>;

