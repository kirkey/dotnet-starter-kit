namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.UpdateRole.v1;

/// <summary>
/// Response after updating membership role.
/// </summary>
public sealed record UpdateMembershipRoleResponse(DefaultIdType Id, string Role, string Message);
