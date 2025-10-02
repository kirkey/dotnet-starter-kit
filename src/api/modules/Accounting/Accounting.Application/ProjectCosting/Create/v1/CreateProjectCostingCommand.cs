using System.ComponentModel;

namespace Accounting.Application.ProjectCosting.Create.v1;

/// <summary>
/// Command to create a new project costing entry.
/// </summary>
/// <param name="ProjectId">The ID of the project to associate this cost with.</param>
/// <param name="EntryDate">The date when the cost was incurred.</param>
/// <param name="Amount">The cost amount (must be positive).</param>
/// <param name="AccountId">The chart of accounts ID for this cost.</param>
/// <param name="Category">Optional cost category (e.g., Materials, Labor, Equipment).</param>
/// <param name="CostCenter">Optional cost center code.</param>
/// <param name="WorkOrderNumber">Optional work order reference number.</param>
/// <param name="IsBillable">Whether this cost can be billed to the client.</param>
/// <param name="Vendor">Optional vendor or supplier reference.</param>
/// <param name="JournalEntryId">Optional reference to a journal entry.</param>
/// <param name="Description">Description of the cost entry.</param>
/// <param name="Notes">Optional additional notes.</param>
public sealed record CreateProjectCostingCommand(
    DefaultIdType ProjectId,
    [property: DefaultValue("2025-10-01")] DateTime EntryDate,
    [property: DefaultValue(1000.00)] decimal Amount,
    DefaultIdType AccountId,
    [property: DefaultValue("Labor")] string? Category = null,
    [property: DefaultValue("CC001")] string? CostCenter = null,
    [property: DefaultValue("WO-2025-001")] string? WorkOrderNumber = null,
    [property: DefaultValue(false)] bool IsBillable = false,
    [property: DefaultValue("ABC Vendor")] string? Vendor = null,
    [property: DefaultValue(null)] DefaultIdType? JournalEntryId = null,
    [property: DefaultValue("Project cost entry")] string? Description = null,
    [property: DefaultValue(null)] string? Notes = null) : IRequest<CreateProjectCostingResponse>;
