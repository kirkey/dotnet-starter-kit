namespace Accounting.Application.Invoices.LineItems.Add.v1;

/// <summary>
/// Command to add a line item to an invoice.
/// </summary>
/// <param name="InvoiceId">Parent invoice identifier.</param>
/// <param name="Description">Line item description.</param>
/// <param name="Quantity">Quantity of items.</param>
/// <param name="UnitPrice">Price per unit.</param>
/// <param name="AccountId">Optional GL account identifier.</param>
public sealed record AddInvoiceLineItemCommand(
    DefaultIdType InvoiceId,
    string Description,
    decimal Quantity,
    decimal UnitPrice,
    DefaultIdType? AccountId = null
) : IRequest<AddInvoiceLineItemResponse>;

