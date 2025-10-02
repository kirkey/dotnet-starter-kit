namespace Accounting.Application.ProjectCosting.Update.v1;

/// <summary>
/// Response after updating a project costing entry.
/// </summary>
/// <param name="Id">The ID of the updated project costing entry.</param>
public sealed record UpdateProjectCostingResponse(DefaultIdType Id);
