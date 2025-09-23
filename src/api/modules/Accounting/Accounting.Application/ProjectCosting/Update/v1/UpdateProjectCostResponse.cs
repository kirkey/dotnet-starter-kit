namespace Accounting.Application.ProjectCosting.Update.v1;

/// <summary>
/// Response for the update project cost command containing the updated cost entry ID.
/// </summary>
/// <param name="ProjectCostId">The unique identifier of the updated project cost entry</param>
public sealed record UpdateProjectCostResponse(DefaultIdType ProjectCostId);
