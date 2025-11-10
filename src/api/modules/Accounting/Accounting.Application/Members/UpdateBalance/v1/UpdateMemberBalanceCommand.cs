namespace Accounting.Application.Members.UpdateBalance.v1;

/// <summary>
/// Command to update a member's balance.
/// </summary>
public sealed record UpdateMemberBalanceCommand(
    DefaultIdType Id,
    decimal NewBalance
) : IRequest<DefaultIdType>;

