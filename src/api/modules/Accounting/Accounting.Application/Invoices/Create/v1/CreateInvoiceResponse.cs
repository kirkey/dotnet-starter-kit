namespace Accounting.Application.Invoices.Create.v1;

/// <summary>
/// Response returned after creating an invoice.
/// </summary>
/// <param name="Id">The unique identifier of the created invoice.</param>
public sealed record CreateInvoiceResponse(DefaultIdType Id);

