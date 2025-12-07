namespace Accounting.Application.Members.UpdateBalance.v1;

/// <summary>
/// Command to update a utility member's balance.
/// </summary>
public sealed record UpdateUtilityMemberBalanceCommand(
    DefaultIdType Id,
    decimal NewBalance
) : IRequest<DefaultIdType>;

