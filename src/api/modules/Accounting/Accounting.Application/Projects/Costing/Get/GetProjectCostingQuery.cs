using Accounting.Application.Projects.Costing.Responses;

namespace Accounting.Application.Projects.Costing.Get;

/// <summary>
/// Query to get a single project costing entry by ID.
/// </summary>
/// <param name="Id">The unique identifier of the project costing entry.</param>
public sealed record GetProjectCostingQuery(DefaultIdType Id) : IRequest<ProjectCostingResponse?>;
