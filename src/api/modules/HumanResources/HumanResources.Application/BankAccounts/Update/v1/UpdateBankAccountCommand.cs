namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Update.v1;

/// <summary>
/// Command to update a bank account.
/// </summary>
public sealed record UpdateBankAccountCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] string? BankName = null,
    [property: DefaultValue(null)] string? AccountHolderName = null,
    [property: DefaultValue(null)] string? SwiftCode = null,
    [property: DefaultValue(null)] string? Iban = null,
    [property: DefaultValue(null)] string? Notes = null,
    [property: DefaultValue(false)] bool SetAsPrimary = false,
    [property: DefaultValue(false)] bool MarkAsVerified = false) : IRequest<UpdateBankAccountResponse>;

