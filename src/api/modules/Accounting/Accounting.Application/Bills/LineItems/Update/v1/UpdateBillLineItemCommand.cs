using System.ComponentModel;

namespace Accounting.Application.Bills.LineItems.Update.v1;

/// <summary>
/// Command to update an existing bill line item.
/// </summary>
/// <param name="LineItemId">The ID of the line item to update.</param>
/// <param name="BillId">The ID of the bill (for validation).</param>
/// <param name="LineNumber">Updated line number for ordering.</param>
/// <param name="Description">Updated description of goods/services.</param>
/// <param name="Quantity">Updated quantity of items.</param>
/// <param name="UnitPrice">Updated price per unit.</param>
/// <param name="Amount">Updated extended line amount.</param>
/// <param name="ChartOfAccountId">Updated GL account for posting.</param>
/// <param name="TaxCodeId">Optional updated tax code.</param>
/// <param name="TaxAmount">Updated tax amount.</param>
/// <param name="ProjectId">Optional updated project reference.</param>
/// <param name="CostCenterId">Optional updated cost center reference.</param>
/// <param name="Notes">Optional updated notes.</param>
public sealed record UpdateBillLineItemCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType LineItemId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType BillId,
    [property: DefaultValue(1)] int LineNumber,
    [property: DefaultValue("Updated line item description")] string Description,
    [property: DefaultValue(1.0)] decimal Quantity,
    [property: DefaultValue(100.00)] decimal UnitPrice,
    [property: DefaultValue(100.00)] decimal Amount,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType ChartOfAccountId,
    DefaultIdType? TaxCodeId = null,
    [property: DefaultValue(0)] decimal TaxAmount = 0,
    DefaultIdType? ProjectId = null,
    DefaultIdType? CostCenterId = null,
    string? Notes = null
) : IRequest<UpdateBillLineItemResponse>;

