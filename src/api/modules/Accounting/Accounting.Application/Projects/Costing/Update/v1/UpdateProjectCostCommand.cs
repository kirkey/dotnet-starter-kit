using System.ComponentModel;

namespace Accounting.Application.Projects.Costing.Update.v1;

/// <summary>
/// Command to update an existing project cost entry with validation.
/// </summary>
/// <param name="Id">The unique identifier of the project cost entry to update</param>
/// <param name="EntryDate">Updated date when the cost was incurred</param>
/// <param name="Amount">Updated cost amount</param>
/// <param name="Description">Updated description of the cost entry</param>
/// <param name="Category">Updated cost category</param>
/// <param name="CostCenter">Updated cost center</param>
/// <param name="WorkOrderNumber">Updated work order number</param>
/// <param name="IsBillable">Updated billable status</param>
/// <param name="Vendor">Updated vendor reference</param>
/// <param name="InvoiceNumber">Updated invoice number</param>
public sealed record UpdateProjectCostCommand(
    DefaultIdType Id,
    [property: DefaultValue("2025-09-23")] DateTime? EntryDate = null,
    [property: DefaultValue(1500.00)] decimal? Amount = null,
    [property: DefaultValue("Updated materials cost")] string? Description = null,
    [property: DefaultValue("Equipment")] string? Category = null,
    [property: DefaultValue("CC002")] string? CostCenter = null,
    [property: DefaultValue("WO-2025-002")] string? WorkOrderNumber = null,
    [property: DefaultValue(true)] bool? IsBillable = null,
    [property: DefaultValue("XYZ Vendor")] string? Vendor = null,
    [property: DefaultValue("INV-67890")] string? InvoiceNumber = null) : IRequest<UpdateProjectCostResponse>;
