namespace Accounting.Application.Members.Activate.v1;

/// <summary>
/// Command to activate a utility member account.
/// </summary>
public sealed record ActivateUtilityMemberCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

