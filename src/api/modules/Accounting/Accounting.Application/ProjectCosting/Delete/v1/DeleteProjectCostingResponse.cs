namespace Accounting.Application.ProjectCosting.Delete.v1;

/// <summary>
/// Response after deleting a project costing entry.
/// </summary>
/// <param name="Id">The ID of the deleted project costing entry.</param>
public sealed record DeleteProjectCostingResponse(DefaultIdType Id);
