namespace Accounting.Application.ProjectCosting.Create.v1;

/// <summary>
/// Response for the create project cost command containing the newly created cost entry ID.
/// </summary>
/// <param name="ProjectCostId">The unique identifier of the created project cost entry</param>
public sealed record CreateProjectCostResponse(DefaultIdType ProjectCostId);
