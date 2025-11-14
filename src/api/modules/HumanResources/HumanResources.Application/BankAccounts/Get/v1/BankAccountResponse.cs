namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Get.v1;

/// <summary>
/// Response object for Bank Account details.
/// </summary>
public sealed record BankAccountResponse
{
    /// <summary>
    /// Gets the unique identifier of the bank account.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Gets the employee identifier.
    /// </summary>
    public DefaultIdType EmployeeId { get; init; }

    /// <summary>
    /// Gets the last 4 digits of account number (for display).
    /// </summary>
    public string? Last4Digits { get; init; }

    /// <summary>
    /// Gets the bank name.
    /// </summary>
    public string BankName { get; init; } = default!;

    /// <summary>
    /// Gets the account type.
    /// </summary>
    public string AccountType { get; init; } = default!;

    /// <summary>
    /// Gets the account holder name.
    /// </summary>
    public string AccountHolderName { get; init; } = default!;

    /// <summary>
    /// Gets a value indicating whether this is the primary account.
    /// </summary>
    public bool IsPrimary { get; init; }

    /// <summary>
    /// Gets a value indicating whether the account is active.
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Gets a value indicating whether the account is verified.
    /// </summary>
    public bool IsVerified { get; init; }

    /// <summary>
    /// Gets the verification date.
    /// </summary>
    public DateTime? VerificationDate { get; init; }

    /// <summary>
    /// Gets the SWIFT code (for international accounts).
    /// </summary>
    public string? SwiftCode { get; init; }

    /// <summary>
    /// Gets the IBAN (for international accounts).
    /// </summary>
    public string? Iban { get; init; }

    /// <summary>
    /// Gets the currency code.
    /// </summary>
    public string? CurrencyCode { get; init; }

    /// <summary>
    /// Gets any notes about the account.
    /// </summary>
    public string? Notes { get; init; }
}

