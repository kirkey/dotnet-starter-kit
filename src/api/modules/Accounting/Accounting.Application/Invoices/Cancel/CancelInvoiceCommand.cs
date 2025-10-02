namespace Accounting.Application.Invoices.Cancel;

/// <summary>
/// Command to cancel an unpaid invoice.
/// </summary>
public sealed record CancelInvoiceCommand(
    DefaultIdType InvoiceId,
    string? CancellationReason = null
) : IRequest<DefaultIdType>;
