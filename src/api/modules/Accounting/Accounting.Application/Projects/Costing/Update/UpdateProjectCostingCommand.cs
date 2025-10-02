namespace Accounting.Application.Projects.Costing.Update;

/// <summary>
/// Command to update an existing project costing entry.
/// </summary>
/// <param name="Id">The unique identifier of the project costing entry to update.</param>
/// <param name="EntryDate">Optional new entry date.</param>
/// <param name="Amount">Optional new cost amount.</param>
/// <param name="Description">Optional new description.</param>
/// <param name="Category">Optional new cost category.</param>
/// <param name="CostCenter">Optional new cost center.</param>
/// <param name="WorkOrderNumber">Optional new work order number.</param>
/// <param name="IsBillable">Optional new billable status.</param>
/// <param name="Vendor">Optional new vendor.</param>
/// <param name="InvoiceNumber">Optional new invoice number.</param>
public sealed record UpdateProjectCostingCommand(
    DefaultIdType Id,
    DateTime? EntryDate = null,
    decimal? Amount = null,
    string? Description = null,
    string? Category = null,
    string? CostCenter = null,
    string? WorkOrderNumber = null,
    bool? IsBillable = null,
    string? Vendor = null,
    string? InvoiceNumber = null
) : IRequest;
