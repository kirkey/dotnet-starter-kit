namespace Accounting.Application.Projects.Get.v1;

/// <summary>
/// Query to get a project by its unique identifier with all related cost entries.
/// </summary>
/// <param name="Id">The unique identifier of the project to retrieve</param>
public sealed record GetProjectQuery(DefaultIdType Id) : IRequest<GetProjectResponse>;
