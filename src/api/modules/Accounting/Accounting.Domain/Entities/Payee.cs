using Accounting.Domain.Events.Payee;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a payee/vendor entity for expense payments with default account mapping and tax identification.
/// </summary>
/// <remarks>
/// Use cases:
/// - Manage vendor master data for accounts payable processing and expense tracking.
/// - Map payees to default expense accounts for automated journal entry creation.
/// - Track tax identification numbers (TIN/EIN) for 1099 reporting and tax compliance.
/// - Support vendor lifecycle management with activation/deactivation capabilities.
/// - Enable vendor address management for check printing and correspondence.
/// - Facilitate expense categorization by linking payees to specific chart of accounts.
/// - Support vendor performance tracking and payment history analysis.
/// - Enable duplicate vendor prevention through code and TIN validation.
/// 
/// Default values:
/// - PayeeCode: required unique identifier (example: "VEND001", "UTIL-ELEC")
/// - Address: optional mailing address (example: "123 Vendor St, Business City, ST 12345")
/// - ExpenseAccountCode: optional default account code (example: "5100" for office supplies)
/// - ExpenseAccountName: optional account name (example: "Office Supplies Expense")
/// - TIN: optional tax identification number (example: "12-3456789" for EIN)
/// - IsActive: true (new payees are active by default)
/// - Name: inherited payee name (example: "ABC Office Supply Company")
/// - Description: inherited detailed description (example: "Primary office supply vendor")
/// 
/// Business rules:
/// - PayeeCode must be unique within the system
/// - TIN format should be validated for tax reporting compliance
/// - Cannot deactivate payees with pending payments or recent transaction history
/// - ExpenseAccountCode must exist in chart of accounts if specified
/// - Address format should support check printing requirements
/// - Duplicate TIN validation to prevent vendor duplication
/// - Name changes require careful tracking for audit purposes
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.Payee.PayeeCreated"/>
/// <seealso cref="Accounting.Domain.Events.Payee.PayeeUpdated"/>
/// <seealso cref="Accounting.Domain.Events.Payee.PayeeActivated"/>
/// <seealso cref="Accounting.Domain.Events.Payee.PayeeDeactivated"/>
public class Payee : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique code identifying the payee.
    /// </summary>
    public string PayeeCode { get; private set; } = string.Empty;

    /// <summary>
    /// Mailing or physical address.
    /// </summary>
    public string? Address { get; private set; }

    /// <summary>
    /// Default expense account code to use for payments to this payee.
    /// </summary>
    public string? ExpenseAccountCode { get; private set; }

    /// <summary>
    /// Default expense account name for readability.
    /// </summary>
    public string? ExpenseAccountName { get; private set; }

    /// <summary>
    /// Taxpayer identification number.
    /// </summary>
    public string? Tin { get; private set; }

    /// <summary>
    /// Whether the payee is active and can be used for payments. Default: true.
    /// Used to retire payees without deleting historical data.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    private Payee()
    {
        // EF Core requires a parameterless constructor for entity instantiation
    }
    
    private Payee(string payeeCode, string name, string? address, string? expenseAccountCode, string? expenseAccountName, string? tin, string? description, string? notes, string? imageUrl)
    {
        PayeeCode = payeeCode.Trim();
        Name = name.Trim();
        Address = address?.Trim();
        ExpenseAccountCode = expenseAccountCode?.Trim();
        ExpenseAccountName = expenseAccountName?.Trim();
        Tin = tin?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();
        ImageUrl = imageUrl;
    }

    /// <summary>
    /// Create a payee with default expense account mapping and metadata.
    /// </summary>
    public static Payee Create(string payeeCode, string name, string? address, string? expenseAccountCode, string? expenseAccountName, string? tin, string? description, string? notes, string? imageUrl = null)
    {
        return new Payee(payeeCode, name, address, expenseAccountCode, expenseAccountName, tin, description, notes, imageUrl);
    }

    /// <summary>
    /// Update the payee metadata; trims inputs.
    /// </summary>
    public Payee Update(string? payeeCode, string? name, string? address, string? expenseAccountCode, string? expenseAccountName, string? tin, string? description, string? notes, string? imageUrl)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(payeeCode) && !string.Equals(PayeeCode, payeeCode, StringComparison.OrdinalIgnoreCase))
        {
            PayeeCode = payeeCode.Trim();
            isUpdated = true;
        }
        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name.Trim();
            isUpdated = true;
        }
        if (!string.IsNullOrWhiteSpace(address) && !string.Equals(Address, address, StringComparison.OrdinalIgnoreCase))
        {
            Address = address.Trim();
            isUpdated = true;
        }
        if (!string.IsNullOrWhiteSpace(expenseAccountCode) && !string.Equals(ExpenseAccountCode, expenseAccountCode, StringComparison.OrdinalIgnoreCase))
        {
            ExpenseAccountCode = expenseAccountCode.Trim();
            isUpdated = true;
        }
        if (!string.IsNullOrWhiteSpace(expenseAccountName) && !string.Equals(ExpenseAccountName, expenseAccountName, StringComparison.OrdinalIgnoreCase))
        {
            ExpenseAccountName = expenseAccountName.Trim();
            isUpdated = true;
        }
        if (!string.IsNullOrWhiteSpace(tin) && !string.Equals(Tin, tin, StringComparison.OrdinalIgnoreCase))
        {
            Tin = tin.Trim();
            isUpdated = true;
        }
        if (!string.IsNullOrWhiteSpace(description) && !string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description.Trim();
            isUpdated = true;
        }
        if (!string.IsNullOrWhiteSpace(notes) && !string.Equals(Notes, notes, StringComparison.OrdinalIgnoreCase))
        {
            Notes = notes.Trim();
            isUpdated = true;
        }
        if (!string.Equals(ImageUrl, imageUrl, StringComparison.OrdinalIgnoreCase))
        {
            ImageUrl = imageUrl;
            isUpdated = true;
        }

        if (isUpdated)
        {
            // Optionally queue a domain event here, e.g.:
            QueueDomainEvent(new PayeeUpdated(Id, this));
        }

        return this;
    }

    /// <summary>
    /// Activates the payee, allowing it to be used for payments.
    /// Raises a PayeeActivated domain event if the payee was previously inactive.
    /// </summary>
    /// <returns>The updated payee instance</returns>
    public Payee Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new PayeeActivated(Id, this));
        }
        return this;
    }

    /// <summary>
    /// Deactivates the payee, preventing it from being used for new payments.
    /// Historical data and existing references remain intact.
    /// Raises a PayeeDeactivated domain event if the payee was previously active.
    /// </summary>
    /// <returns>The updated payee instance</returns>
    /// <remarks>
    /// Business rule: Cannot deactivate payees with pending payments or recent transaction history.
    /// This validation should be implemented at the application layer.
    /// </remarks>
    public Payee Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new PayeeDeactivated(Id, this));
        }
        return this;
    }
}
