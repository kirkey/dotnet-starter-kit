namespace Accounting.Application.SecurityDeposits.Commands;

/// <summary>
/// Response from creating a security deposit.
/// </summary>
/// <param name="Id">The newly created security deposit identifier.</param>
public sealed record CreateSecurityDepositResponse(DefaultIdType Id);

