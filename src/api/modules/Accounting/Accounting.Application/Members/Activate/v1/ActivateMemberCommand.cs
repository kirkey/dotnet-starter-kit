namespace Accounting.Application.Members.Activate.v1;

/// <summary>
/// Command to activate a member account.
/// </summary>
public sealed record ActivateMemberCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

