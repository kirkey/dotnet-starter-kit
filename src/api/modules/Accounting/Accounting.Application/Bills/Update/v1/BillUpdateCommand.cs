namespace Accounting.Application.Bills.Update.v1;

/// <summary>
/// Command to update an existing bill.
/// </summary>
public record BillUpdateCommand(
    DefaultIdType Id,
    DateTime? DueDate = null,
    decimal? SubtotalAmount = null,
    decimal? TaxAmount = null,
    decimal? ShippingAmount = null,
    string? PaymentTerms = null,
    string? Description = null,
    string? Notes = null
) : IRequest<DefaultIdType>;

