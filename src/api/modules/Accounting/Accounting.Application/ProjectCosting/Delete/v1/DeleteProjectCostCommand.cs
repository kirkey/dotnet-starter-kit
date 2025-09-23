namespace Accounting.Application.ProjectCosting.Delete.v1;

/// <summary>
/// Command to delete a project cost entry with proper validation.
/// </summary>
/// <param name="Id">The unique identifier of the project cost entry to delete</param>
public sealed record DeleteProjectCostCommand(DefaultIdType Id) : IRequest<DeleteProjectCostResponse>;
