using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.Member;

public record MemberCreated(DefaultIdType MemberId, string MemberNumber, string MemberName, string ServiceAddress, DateTime MembershipDate, string? Description, string? Notes) : DomainEvent;

public record MemberUpdated(DefaultIdType Id, string MemberNumber, string MemberName, string ServiceAddress, string? Description, string? Notes) : DomainEvent;

public record MemberBalanceUpdated(DefaultIdType Id, string MemberNumber, decimal CurrentBalance) : DomainEvent;

public record MemberStatusChanged(DefaultIdType Id, string MemberNumber, bool IsActive, string AccountStatus) : DomainEvent;

public record MemberSuspended(DefaultIdType MemberId, string MemberNumber, string MemberName) : DomainEvent;

public record MemberReactivated(DefaultIdType MemberId, string MemberNumber, string MemberName) : DomainEvent;
