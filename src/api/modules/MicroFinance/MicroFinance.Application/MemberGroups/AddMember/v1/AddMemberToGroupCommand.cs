using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.AddMember.v1;

/// <summary>
/// Command to add a member to a group (creates a GroupMembership).
/// </summary>
public sealed record AddMemberToGroupCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType GroupId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType MemberId,
    [property: DefaultValue("2025-01-15")] DateOnly? JoinDate,
    [property: DefaultValue("Member")] string? Role,
    [property: DefaultValue("Added via group management")] string? Notes) : IRequest<AddMemberToGroupResponse>;
