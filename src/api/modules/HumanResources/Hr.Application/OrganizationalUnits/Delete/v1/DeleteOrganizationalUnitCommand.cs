namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Delete.v1;

/// <summary>
/// Command to delete organizational unit.
/// </summary>
public sealed record DeleteOrganizationalUnitCommand(DefaultIdType Id) : IRequest<DeleteOrganizationalUnitResponse>;

