namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Get.v1;

/// <summary>
/// Response with bank account details (masked for security).
/// </summary>
public sealed record BankAccountResponse(
    DefaultIdType Id,
    DefaultIdType EmployeeId,
    string BankName,
    string? Last4Digits,
    string AccountType,
    string AccountHolderName,
    bool IsPrimary,
    bool IsActive,
    bool IsVerified,
    DateTime? VerificationDate,
    string? SwiftCode,
    string? Iban,
    string? CurrencyCode,
    string? Notes);

