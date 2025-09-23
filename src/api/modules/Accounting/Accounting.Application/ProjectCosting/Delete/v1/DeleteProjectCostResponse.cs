namespace Accounting.Application.ProjectCosting.Delete.v1;

/// <summary>
/// Response for the delete project cost command.
/// </summary>
/// <param name="ProjectCostId">The unique identifier of the deleted project cost entry</param>
/// <param name="IsDeleted">Indicates whether the deletion was successful</param>
/// <param name="Message">Additional information about the deletion</param>
public sealed record DeleteProjectCostResponse(
    DefaultIdType ProjectCostId,
    bool IsDeleted,
    string Message);
