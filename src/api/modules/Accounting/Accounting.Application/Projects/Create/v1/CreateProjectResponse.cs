namespace Accounting.Application.Projects.Create.v1;

/// <summary>
/// Response for the create project command containing the newly created project ID.
/// </summary>
/// <param name="ProjectId">The unique identifier of the created project</param>
public sealed record CreateProjectResponse(DefaultIdType ProjectId);
