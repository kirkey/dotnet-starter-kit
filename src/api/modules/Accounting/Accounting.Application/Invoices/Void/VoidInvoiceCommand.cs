namespace Accounting.Application.Invoices.Void;

/// <summary>
/// Command to void an invoice (can be used for paid or unpaid invoices).
/// </summary>
public sealed record VoidInvoiceCommand(
    DefaultIdType InvoiceId,
    string? VoidReason = null
) : IRequest<DefaultIdType>;
