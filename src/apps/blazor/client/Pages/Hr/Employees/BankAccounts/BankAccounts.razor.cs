namespace FSH.Starter.Blazor.Client.Pages.Hr.Employees.BankAccounts;

public partial class BankAccounts
{
    // Services are injected in BankAccounts.razor partial file
}

/// <summary>
/// View model for Bank Account form operations.
/// Combines Create and Update command properties with Response properties for UI binding.
/// </summary>
public class BankAccountViewModel
{
    /// <summary>
    /// Bank account ID (from response).
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Selected employee from autocomplete (not persisted directly, used for form binding).
    /// </summary>
    public EmployeeResponse? SelectedEmployee { get; set; }

    /// <summary>
    /// Employee ID who owns this bank account.
    /// </summary>
    public DefaultIdType EmployeeId { get; set; }

    /// <summary>
    /// Account number (masked in display).
    /// </summary>
    public string? AccountNumber { get; set; }

    /// <summary>
    /// Routing number for domestic (ACH) transfers.
    /// </summary>
    public string? RoutingNumber { get; set; }

    /// <summary>
    /// Bank name.
    /// </summary>
    public string? BankName { get; set; }

    /// <summary>
    /// Account type (Checking, Savings, etc.).
    /// </summary>
    public string? AccountType { get; set; }

    /// <summary>
    /// Name on the bank account.
    /// </summary>
    public string? AccountHolderName { get; set; }

    /// <summary>
    /// SWIFT code for international transfers.
    /// </summary>
    public string? SwiftCode { get; set; }

    /// <summary>
    /// IBAN for international transfers.
    /// </summary>
    public string? Iban { get; set; }

    /// <summary>
    /// Currency code (default: USD).
    /// </summary>
    public string? CurrencyCode { get; set; }

    /// <summary>
    /// Additional notes or special instructions.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Whether this is the primary account for payroll deposits.
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Whether this account is active for use.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Whether this account has been verified.
    /// </summary>
    public bool IsVerified { get; set; }

    /// <summary>
    /// Date when account was verified.
    /// </summary>
    public DateTime? VerificationDate { get; set; }

    /// <summary>
    /// Last 4 digits of account number (for display only, never editable).
    /// </summary>
    public string? Last4Digits { get; set; }

    /// <summary>
    /// Converts to CreateBankAccountCommand for API creation.
    /// </summary>
    public CreateBankAccountCommand ToCreateCommand() => new()
    {
        EmployeeId = EmployeeId,
        AccountNumber = AccountNumber ?? string.Empty,
        RoutingNumber = RoutingNumber ?? string.Empty,
        BankName = BankName ?? string.Empty,
        AccountType = AccountType ?? string.Empty,
        AccountHolderName = AccountHolderName ?? string.Empty,
        SwiftCode = SwiftCode,
        Iban = Iban,
        Notes = Notes
    };

    /// <summary>
    /// Converts to UpdateBankAccountCommand for API updates.
    /// </summary>
    public UpdateBankAccountCommand ToUpdateCommand() => new()
    {
        Id = Id,
        BankName = BankName,
        AccountHolderName = AccountHolderName,
        SwiftCode = SwiftCode,
        Iban = Iban,
        Notes = Notes,
        IsPrimary = IsPrimary,
        IsActive = IsActive
    };
}
