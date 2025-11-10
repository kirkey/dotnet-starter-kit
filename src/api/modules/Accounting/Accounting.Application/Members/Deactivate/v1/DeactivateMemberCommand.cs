namespace Accounting.Application.Members.Deactivate.v1;

/// <summary>
/// Command to deactivate a member account.
/// </summary>
public sealed record DeactivateMemberCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

