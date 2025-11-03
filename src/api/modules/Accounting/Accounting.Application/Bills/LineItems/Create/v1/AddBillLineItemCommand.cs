using System.ComponentModel;

namespace Accounting.Application.Bills.LineItems.Create.v1;

/// <summary>
/// Command to add a new line item to a bill.
/// </summary>
/// <param name="BillId">The ID of the bill to add the line item to.</param>
/// <param name="LineNumber">Line number for ordering (must be positive).</param>
/// <param name="Description">Description of goods/services (required, max 500 chars).</param>
/// <param name="Quantity">Quantity of items (must be greater than zero).</param>
/// <param name="UnitPrice">Price per unit (cannot be negative).</param>
/// <param name="Amount">Extended line amount (should equal Quantity × UnitPrice).</param>
/// <param name="ChartOfAccountId">GL account for posting (required).</param>
/// <param name="TaxCodeId">Optional tax code.</param>
/// <param name="TaxAmount">Tax amount (cannot be negative, default 0).</param>
/// <param name="ProjectId">Optional project reference.</param>
/// <param name="CostCenterId">Optional cost center reference.</param>
/// <param name="Notes">Optional notes (max 1000 chars).</param>
public sealed record AddBillLineItemCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType BillId,
    [property: DefaultValue(1)] int LineNumber,
    [property: DefaultValue("Line item description")] string Description,
    [property: DefaultValue(1.0)] decimal Quantity,
    [property: DefaultValue(100.00)] decimal UnitPrice,
    [property: DefaultValue(100.00)] decimal Amount,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType ChartOfAccountId,
    DefaultIdType? TaxCodeId = null,
    [property: DefaultValue(0)] decimal TaxAmount = 0,
    DefaultIdType? ProjectId = null,
    DefaultIdType? CostCenterId = null,
    string? Notes = null
) : IRequest<AddBillLineItemResponse>;
