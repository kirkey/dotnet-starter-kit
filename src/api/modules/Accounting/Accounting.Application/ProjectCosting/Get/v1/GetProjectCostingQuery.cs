namespace Accounting.Application.ProjectCosting.Get.v1;

/// <summary>
/// Query to get a project costing entry by ID.
/// </summary>
/// <param name="Id">The ID of the project costing entry to retrieve.</param>
public sealed record GetProjectCostingQuery(DefaultIdType Id) : IRequest<ProjectCostingResponse>;
