namespace Accounting.Application.SecurityDeposits.Commands;

/// <summary>
/// Command to create a new security deposit for a member.
/// </summary>
public sealed record CreateSecurityDepositCommand(
    /// <summary>
    /// The member who is paying the deposit.
    /// </summary>
    DefaultIdType MemberId,
    
    /// <summary>
    /// The deposit amount; must be positive.
    /// </summary>
    decimal Amount,
    
    /// <summary>
    /// Date when the deposit was received.
    /// </summary>
    DateTime DepositDate,
    
    /// <summary>
    /// Optional notes about the deposit.
    /// </summary>
    string? Notes = null
) : IRequest<CreateSecurityDepositResponse>;
