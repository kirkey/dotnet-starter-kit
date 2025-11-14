namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Create.v1;

/// <summary>
/// Command to create a new bank account.
/// </summary>
public sealed record CreateBankAccountCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue("****1234")] string AccountNumber,
    [property: DefaultValue("123456789")] string RoutingNumber,
    [property: DefaultValue("First National Bank")] string BankName,
    [property: DefaultValue("Checking")] string AccountType,
    [property: DefaultValue("John Doe")] string AccountHolderName,
    [property: DefaultValue(null)] string? SwiftCode = null,
    [property: DefaultValue(null)] string? Iban = null,
    [property: DefaultValue(null)] string? CurrencyCode = null) : IRequest<CreateBankAccountResponse>;

