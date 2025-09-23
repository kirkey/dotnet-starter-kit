namespace Accounting.Application.Projects.Delete.v1;

/// <summary>
/// Response for the delete project command.
/// </summary>
/// <param name="ProjectId">The unique identifier of the deleted project</param>
/// <param name="IsDeleted">Indicates whether the deletion was successful</param>
/// <param name="Message">Additional information about the deletion</param>
public sealed record DeleteProjectResponse(
    DefaultIdType ProjectId,
    bool IsDeleted,
    string Message);
