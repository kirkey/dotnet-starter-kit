using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents an employee's bank account for salary direct deposit.
/// Tracks banking information for payroll processing.
/// </summary>
/// <remarks>
/// Key Design Points:
/// - One or multiple bank accounts per employee
/// - Bank account number (encrypted at rest)
/// - Account type: Checking, Savings
/// - Routing number for ACH transactions
/// - Primary account for payroll
/// - International/Domestic support
/// 
/// Example:
/// - Employee John Doe has:
///   - Primary Checking Account: Last 4 digits 1234
///   - Savings Account: Last 4 digits 5678
/// </remarks>
public class BankAccount : AuditableEntity, IAggregateRoot
{
    private BankAccount() { }

    private BankAccount(
        DefaultIdType id,
        DefaultIdType employeeId,
        string accountNumber,
        string routingNumber,
        string bankName,
        string accountType,
        string accountHolderName)
    {
        Id = id;
        EmployeeId = employeeId;
        AccountNumber = accountNumber;
        RoutingNumber = routingNumber;
        BankName = bankName;
        AccountType = accountType;
        AccountHolderName = accountHolderName;
        IsActive = true;
        IsPrimary = false;

        QueueDomainEvent(new BankAccountCreated { BankAccount = this });
    }

    /// <summary>
    /// The employee this bank account belongs to.
    /// </summary>
    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = default!;

    /// <summary>
    /// Bank account number (encrypted at rest).
    /// </summary>
    public string AccountNumber { get; private set; } = default!;

    /// <summary>
    /// Last 4 digits of account number (for display).
    /// </summary>
    public string? Last4Digits { get; private set; }

    /// <summary>
    /// Routing number for ACH transactions (encrypted at rest).
    /// </summary>
    public string RoutingNumber { get; private set; } = default!;

    /// <summary>
    /// Name of the bank.
    /// </summary>
    public string BankName { get; private set; } = default!;

    /// <summary>
    /// Type of account (Checking, Savings).
    /// </summary>
    public string AccountType { get; private set; } = default!;

    /// <summary>
    /// Name of the account holder.
    /// </summary>
    public string AccountHolderName { get; private set; } = default!;

    /// <summary>
    /// Whether this is the primary account for direct deposit.
    /// </summary>
    public bool IsPrimary { get; private set; }

    /// <summary>
    /// Whether the account is active and valid.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Whether the account has been verified.
    /// </summary>
    public bool IsVerified { get; private set; }

    /// <summary>
    /// Date the account was verified.
    /// </summary>
    public DateTime? VerificationDate { get; private set; }

    /// <summary>
    /// SWIFT code for international transfers (if applicable).
    /// </summary>
    public string? SwiftCode { get; private set; }

    /// <summary>
    /// IBAN for international transfers (if applicable).
    /// </summary>
    public string? Iban { get; private set; }

    /// <summary>
    /// Currency code for international accounts (e.g., USD, EUR).
    /// </summary>
    public string? CurrencyCode { get; private set; }

    /// <summary>
    /// Notes or additional information about the account.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Creates a new bank account.
    /// </summary>
    public static BankAccount Create(
        DefaultIdType employeeId,
        string accountNumber,
        string routingNumber,
        string bankName,
        string accountType,
        string accountHolderName,
        string? swiftCode = null,
        string? iban = null,
        string? currencyCode = null)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("Account number is required.", nameof(accountNumber));

        if (string.IsNullOrWhiteSpace(routingNumber))
            throw new ArgumentException("Routing number is required.", nameof(routingNumber));

        var account = new BankAccount(
            DefaultIdType.NewGuid(),
            employeeId,
            accountNumber,
            routingNumber,
            bankName,
            accountType,
            accountHolderName)
        {
            SwiftCode = swiftCode,
            Iban = iban,
            CurrencyCode = currencyCode ?? "USD",
            Last4Digits = accountNumber.Length >= 4 
                ? accountNumber.Substring(accountNumber.Length - 4) 
                : accountNumber
        };

        return account;
    }

    /// <summary>
    /// Sets this account as the primary for direct deposit.
    /// </summary>
    public BankAccount SetAsPrimary()
    {
        IsPrimary = true;
        QueueDomainEvent(new BankAccountUpdated { BankAccount = this });
        return this;
    }

    /// <summary>
    /// Removes primary status from this account.
    /// </summary>
    public BankAccount RemovePrimaryStatus()
    {
        IsPrimary = false;
        QueueDomainEvent(new BankAccountUpdated { BankAccount = this });
        return this;
    }

    /// <summary>
    /// Marks account as verified.
    /// </summary>
    public BankAccount MarkAsVerified()
    {
        IsVerified = true;
        VerificationDate = DateTime.UtcNow;
        QueueDomainEvent(new BankAccountUpdated { BankAccount = this });
        return this;
    }

    /// <summary>
    /// Updates account information.
    /// </summary>
    public BankAccount Update(
        string? bankName = null,
        string? accountHolderName = null,
        string? swiftCode = null,
        string? iban = null,
        string? notes = null)
    {
        if (!string.IsNullOrWhiteSpace(bankName))
            BankName = bankName;

        if (!string.IsNullOrWhiteSpace(accountHolderName))
            AccountHolderName = accountHolderName;

        if (!string.IsNullOrWhiteSpace(swiftCode))
            SwiftCode = swiftCode;

        if (!string.IsNullOrWhiteSpace(iban))
            Iban = iban;

        if (notes != null)
            Notes = notes;

        QueueDomainEvent(new BankAccountUpdated { BankAccount = this });
        return this;
    }

    /// <summary>
    /// Deactivates this bank account.
    /// </summary>
    public BankAccount Deactivate()
    {
        IsActive = false;
        IsPrimary = false;
        QueueDomainEvent(new BankAccountDeactivated { BankAccountId = Id });
        return this;
    }

    /// <summary>
    /// Activates this bank account.
    /// </summary>
    public BankAccount Activate()
    {
        IsActive = true;
        QueueDomainEvent(new BankAccountActivated { BankAccountId = Id });
        return this;
    }
}

/// <summary>
/// Bank account type constants.
/// </summary>
public static class BankAccountType
{
    /// <summary>
    /// Checking account.
    /// </summary>
    public const string Checking = "Checking";

    /// <summary>
    /// Savings account.
    /// </summary>
    public const string Savings = "Savings";

    /// <summary>
    /// Money market account.
    /// </summary>
    public const string MoneyMarket = "MoneyMarket";

    /// <summary>
    /// Other account type.
    /// </summary>
    public const string Other = "Other";
}

