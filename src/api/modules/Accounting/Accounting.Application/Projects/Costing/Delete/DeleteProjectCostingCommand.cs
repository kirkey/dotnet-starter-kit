namespace Accounting.Application.Projects.Costing.Delete;

/// <summary>
/// Command to delete a project costing entry.
/// </summary>
/// <param name="Id">The unique identifier of the project costing entry to delete.</param>
public sealed record DeleteProjectCostingCommand(DefaultIdType Id) : IRequest;
