namespace Accounting.Application.Bills.Create.v1;

/// <summary>
/// Command to create a new bill.
/// </summary>
/// <param name="BillNumber">Unique bill or vendor invoice number.</param>
/// <param name="VendorId">Vendor who issued the bill.</param>
/// <param name="BillDate">Date bill was issued.</param>
/// <param name="DueDate">Date payment is due.</param>
/// <param name="Description">Description of the bill.</param>
/// <param name="PeriodId">Optional accounting period.</param>
/// <param name="PaymentTerms">Optional payment terms.</param>
/// <param name="PurchaseOrderNumber">Optional PO reference.</param>
/// <param name="Notes">Optional notes.</param>
/// <param name="LineItems">Line items for the bill.</param>
public sealed record CreateBillCommand(
    string BillNumber,
    DefaultIdType VendorId,
    DateTime BillDate,
    DateTime DueDate,
    string? Description = null,
    DefaultIdType? PeriodId = null,
    string? PaymentTerms = null,
    string? PurchaseOrderNumber = null,
    string? Notes = null,
    List<BillLineItemDto>? LineItems = null
) : IRequest<BillCreateResponse>;

/// <summary>
/// DTO for bill line items in create/update commands.
/// </summary>
/// <param name="LineNumber">Line number for ordering.</param>
/// <param name="Description">Description of goods/services.</param>
/// <param name="Quantity">Quantity of items.</param>
/// <param name="UnitPrice">Price per unit.</param>
/// <param name="Amount">Extended line amount.</param>
/// <param name="ChartOfAccountId">GL account for posting.</param>
/// <param name="TaxCodeId">Optional tax code.</param>
/// <param name="TaxAmount">Optional tax amount.</param>
/// <param name="ProjectId">Optional project reference.</param>
/// <param name="CostCenterId">Optional cost center reference.</param>
/// <param name="Notes">Optional notes.</param>
public sealed record BillLineItemDto(
    int LineNumber,
    string Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal Amount,
    DefaultIdType ChartOfAccountId,
    DefaultIdType? TaxCodeId = null,
    decimal TaxAmount = 0,
    DefaultIdType? ProjectId = null,
    DefaultIdType? CostCenterId = null,
    string? Notes = null
);

