namespace Accounting.Application.ProjectCosting.Create.v1;

/// <summary>
/// Response after creating a project costing entry.
/// </summary>
/// <param name="Id">The ID of the created project costing entry.</param>
public sealed record CreateProjectCostingResponse(DefaultIdType Id);
