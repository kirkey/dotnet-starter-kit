// MemberExceptions.cs
// Per-domain exceptions for Member aggregate

namespace Accounting.Domain.Exceptions;

public sealed class MemberNotFoundException(DefaultIdType id) : NotFoundException($"member with id {id} not found");
public sealed class MemberNumberNotFoundException(string memberNumber) : NotFoundException($"member with number {memberNumber} not found");
public sealed class MemberAlreadyActiveException(DefaultIdType id) : ForbiddenException($"member with id {id} is already active");
public sealed class MemberAlreadyInactiveException(DefaultIdType id) : ForbiddenException($"member with id {id} is already inactive");
public sealed class InvalidMemberNumberException(string message) : ForbiddenException(message);
public sealed class InvalidMemberContactException(string message) : ForbiddenException(message);
public sealed class InvalidMemberAddressException(string message) : ForbiddenException(message);
public sealed class NegativeMemberBalanceException(DefaultIdType id, decimal balance) : ForbiddenException($"member {id} has invalid negative balance {balance}");

