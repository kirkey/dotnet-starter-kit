namespace Accounting.Application.Projects.Update.v1;

/// <summary>
/// Response for the update project command containing the updated project ID.
/// </summary>
/// <param name="ProjectId">The unique identifier of the updated project</param>
public sealed record UpdateProjectResponse(DefaultIdType ProjectId);
