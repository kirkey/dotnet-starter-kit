namespace Accounting.Application.Projects.Delete.v1;

/// <summary>
/// Command to delete a project with proper validation and cleanup.
/// </summary>
/// <param name="Id">The unique identifier of the project to delete</param>
public sealed record DeleteProjectCommand(DefaultIdType Id) : IRequest<DeleteProjectResponse>;
