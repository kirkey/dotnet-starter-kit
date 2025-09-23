namespace Accounting.Application.ProjectCosting.Get.v1;

/// <summary>
/// Query to get a project cost entry by its unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the project cost entry to retrieve</param>
public sealed record GetProjectCostQuery(DefaultIdType Id) : IRequest<GetProjectCostResponse>;
