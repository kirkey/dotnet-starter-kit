namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Get.v1;

/// <summary>
/// Request to get organizational unit by ID.
/// </summary>
public sealed record GetOrganizationalUnitRequest(DefaultIdType Id) : IRequest<OrganizationalUnitResponse>;

