namespace Accounting.Application.Bills.Update.v1;

/// <summary>
/// Command to update an existing bill.
/// </summary>
/// <param name="BillId">Bill identifier.</param>
/// <param name="BillNumber">Bill or vendor invoice number.</param>
/// <param name="BillDate">Date bill was issued.</param>
/// <param name="DueDate">Date payment is due.</param>
/// <param name="Description">Description of the bill.</param>
/// <param name="PeriodId">Optional accounting period.</param>
/// <param name="PaymentTerms">Optional payment terms.</param>
/// <param name="PurchaseOrderNumber">Optional PO reference.</param>
/// <param name="Notes">Optional notes.</param>
public sealed record BillUpdateCommand(
    DefaultIdType BillId,
    string? BillNumber = null,
    DateTime? BillDate = null,
    DateTime? DueDate = null,
    string? Description = null,
    DefaultIdType? PeriodId = null,
    string? PaymentTerms = null,
    string? PurchaseOrderNumber = null,
    string? Notes = null
) : IRequest<UpdateBillResponse>;

