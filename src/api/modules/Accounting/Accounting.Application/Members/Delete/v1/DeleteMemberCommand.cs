namespace Accounting.Application.Members.Delete.v1;

/// <summary>
/// Command to delete a utility member account.
/// Only inactive members with no transaction history can be deleted.
/// </summary>
public sealed record DeleteUtilityMemberCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

