using FSH.Framework.Core.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain.Events;

/// <summary>Domain event raised when a new member is created.</summary>
public sealed record MemberCreated : DomainEvent
{
    public Member? Member { get; init; }
}

/// <summary>Domain event raised when a member is updated.</summary>
public sealed record MemberUpdated : DomainEvent
{
    public Member? Member { get; init; }
}

/// <summary>Domain event raised when a member is activated.</summary>
public sealed record MemberActivated : DomainEvent
{
    public DefaultIdType MemberId { get; init; }
}

/// <summary>Domain event raised when a member is deactivated.</summary>
public sealed record MemberDeactivated : DomainEvent
{
    public DefaultIdType MemberId { get; init; }
}

