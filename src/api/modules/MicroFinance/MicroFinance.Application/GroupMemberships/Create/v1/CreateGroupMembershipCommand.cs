using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Create.v1;

public sealed record CreateGroupMembershipCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType MemberId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType GroupId,
    [property: DefaultValue("2025-01-15")] DateOnly? JoinDate,
    [property: DefaultValue("Member")] string? Role,
    [property: DefaultValue("Member added via API")] string? Notes) : IRequest<CreateGroupMembershipResponse>;
