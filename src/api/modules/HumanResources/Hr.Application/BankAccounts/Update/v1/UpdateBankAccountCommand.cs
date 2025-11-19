namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Update.v1;

/// <summary>
/// Command to update bank account details and manage primary status.
/// </summary>
public sealed record UpdateBankAccountCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? BankName = null,
    [property: DefaultValue(null)] string? AccountHolderName = null,
    [property: DefaultValue(null)] string? SwiftCode = null,
    [property: DefaultValue(null)] string? Iban = null,
    [property: DefaultValue(null)] string? Notes = null,
    [property: DefaultValue(null)] bool? IsPrimary = null,
    [property: DefaultValue(null)] bool? IsActive = null
) : IRequest<UpdateBankAccountResponse>;

/// <summary>
/// Response for bank account update.
/// </summary>
public sealed record UpdateBankAccountResponse(
    DefaultIdType Id,
    string BankName,
    string? Last4Digits,
    bool IsPrimary,
    bool IsActive);

