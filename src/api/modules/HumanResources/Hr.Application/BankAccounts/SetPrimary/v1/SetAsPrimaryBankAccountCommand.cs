namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.SetPrimary.v1;

/// <summary>
/// Command to set a bank account as primary for payroll direct deposit.
/// Only one account per employee can be primary.
/// </summary>
public sealed record SetAsPrimaryBankAccountCommand(
    DefaultIdType Id
) : IRequest<SetAsPrimaryBankAccountResponse>;

/// <summary>
/// Response for setting primary bank account.
/// </summary>
public sealed record SetAsPrimaryBankAccountResponse(
    DefaultIdType Id,
    bool IsPrimary,
    string BankName,
    string? Last4Digits);

