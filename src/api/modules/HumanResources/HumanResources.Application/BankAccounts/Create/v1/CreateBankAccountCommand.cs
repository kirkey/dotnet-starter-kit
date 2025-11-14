namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Create.v1;

/// <summary>
/// Command to create employee bank account for direct deposit.
/// Supports domestic (ACH) and international (SWIFT/IBAN) transfers.
/// </summary>
public sealed record CreateBankAccountCommand(
    DefaultIdType EmployeeId,
    [property: DefaultValue("1234567890")] string AccountNumber,
    [property: DefaultValue("021000021")] string RoutingNumber,
    [property: DefaultValue("Bank of America")] string BankName,
    [property: DefaultValue("Checking")] string AccountType,
    [property: DefaultValue("John Doe")] string AccountHolderName,
    [property: DefaultValue(null)] string? SwiftCode = null,
    [property: DefaultValue(null)] string? Iban = null,
    [property: DefaultValue(null)] string? Notes = null
) : IRequest<CreateBankAccountResponse>;

/// <summary>
/// Response for bank account creation.
/// </summary>
public sealed record CreateBankAccountResponse(
    DefaultIdType Id,
    DefaultIdType EmployeeId,
    string BankName,
    string? Last4Digits,
    string AccountType,
    bool IsPrimary);

