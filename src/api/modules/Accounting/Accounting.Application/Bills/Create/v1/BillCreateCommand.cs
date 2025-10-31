namespace Accounting.Application.Bills.Create.v1;

/// <summary>
/// Command to create a new bill (accounts payable invoice).
/// </summary>
public record BillCreateCommand(
    string BillNumber,
    DefaultIdType VendorId,
    string VendorInvoiceNumber,
    DateTime BillDate,
    DateTime DueDate,
    decimal SubtotalAmount,
    decimal TaxAmount,
    decimal ShippingAmount,
    string? PaymentTerms = null,
    decimal DiscountAmount = 0,
    DateTime? DiscountDate = null,
    DefaultIdType? PurchaseOrderId = null,
    DefaultIdType? ExpenseAccountId = null,
    DefaultIdType? PeriodId = null,
    string? Description = null,
    string? Notes = null
) : IRequest<BillCreateResponse>;

