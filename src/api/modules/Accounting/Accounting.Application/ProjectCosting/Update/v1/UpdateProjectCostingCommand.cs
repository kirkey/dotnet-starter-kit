using System.ComponentModel;

namespace Accounting.Application.ProjectCosting.Update.v1;

/// <summary>
/// Command to update an existing project costing entry.
/// </summary>
public sealed record UpdateProjectCostingCommand(
    DefaultIdType Id,
    [property: DefaultValue("2025-10-01")] DateTime? EntryDate = null,
    [property: DefaultValue(1500.00)] decimal? Amount = null,
    [property: DefaultValue("Updated project cost entry")] string? Description = null,
    [property: DefaultValue("Materials")] string? Category = null,
    [property: DefaultValue("CC002")] string? CostCenter = null,
    [property: DefaultValue("WO-2025-002")] string? WorkOrderNumber = null,
    [property: DefaultValue(true)] bool? IsBillable = null,
    [property: DefaultValue("XYZ Vendor")] string? Vendor = null,
    [property: DefaultValue(null)] string? InvoiceNumber = null) : IRequest<UpdateProjectCostingResponse>;
