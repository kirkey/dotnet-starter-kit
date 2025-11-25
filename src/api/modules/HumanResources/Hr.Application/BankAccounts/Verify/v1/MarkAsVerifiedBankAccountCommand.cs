namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Verify.v1;

/// <summary>
/// Command to verify a bank account after successful validation.
/// Marks the account as verified and records the verification date.
/// </summary>
public sealed record MarkAsVerifiedBankAccountCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? Notes = null
) : IRequest<MarkAsVerifiedBankAccountResponse>;

/// <summary>
/// Response for bank account verification.
/// </summary>
public sealed record MarkAsVerifiedBankAccountResponse(
    DefaultIdType Id,
    bool IsVerified,
    DateTime VerificationDate);

