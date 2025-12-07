namespace Accounting.Application.Members.Deactivate.v1;

/// <summary>
/// Command to deactivate a utility member account.
/// </summary>
public sealed record DeactivateUtilityMemberCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

