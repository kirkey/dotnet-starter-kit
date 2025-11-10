namespace Accounting.Application.Members.Delete.v1;

/// <summary>
/// Command to delete a member account.
/// Only inactive members with no transaction history can be deleted.
/// </summary>
public sealed record DeleteMemberCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

