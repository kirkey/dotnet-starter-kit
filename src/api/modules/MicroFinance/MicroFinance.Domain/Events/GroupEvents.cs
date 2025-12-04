using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

// ============================================
// Member Group Events
// ============================================

/// <summary>Domain event raised when a new member group is created.</summary>
public sealed record MemberGroupCreated : DomainEvent
{
    public MemberGroup? MemberGroup { get; init; }
}

/// <summary>Domain event raised when a member group is updated.</summary>
public sealed record MemberGroupUpdated : DomainEvent
{
    public MemberGroup? MemberGroup { get; init; }
}

/// <summary>Domain event raised when a member group is activated.</summary>
public sealed record MemberGroupActivated : DomainEvent
{
    public DefaultIdType GroupId { get; init; }
}

/// <summary>Domain event raised when a member group is deactivated.</summary>
public sealed record MemberGroupDeactivated : DomainEvent
{
    public DefaultIdType GroupId { get; init; }
}

/// <summary>Domain event raised when a member group is dissolved.</summary>
public sealed record MemberGroupDissolved : DomainEvent
{
    public DefaultIdType GroupId { get; init; }
    public string? Reason { get; init; }
}

// ============================================
// Group Membership Events
// ============================================

/// <summary>Domain event raised when a group membership is created.</summary>
public sealed record GroupMembershipCreated : DomainEvent
{
    public GroupMembership? GroupMembership { get; init; }
}

/// <summary>Domain event raised when a membership role is changed.</summary>
public sealed record GroupMembershipRoleChanged : DomainEvent
{
    public DefaultIdType MembershipId { get; init; }
    public string? NewRole { get; init; }
}

/// <summary>Domain event raised when a membership is withdrawn.</summary>
public sealed record GroupMembershipWithdrawn : DomainEvent
{
    public DefaultIdType MembershipId { get; init; }
}
