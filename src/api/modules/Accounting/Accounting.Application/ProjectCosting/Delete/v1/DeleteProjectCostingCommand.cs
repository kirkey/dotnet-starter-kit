namespace Accounting.Application.ProjectCosting.Delete.v1;

/// <summary>
/// Command to delete a project costing entry.
/// </summary>
/// <param name="Id">The ID of the project costing entry to delete.</param>
public sealed record DeleteProjectCostingCommand(DefaultIdType Id) : IRequest<DeleteProjectCostingResponse>;
