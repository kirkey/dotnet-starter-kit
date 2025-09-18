using Accounting.Domain.Events.Payee;

namespace Accounting.Domain;

/// <summary>
/// Represents a payee/vendor-like entity for expense payments, with default expense account mapping.
/// </summary>
/// <remarks>
/// Stores address, expense account code/name, TIN, and descriptive notes. Strings are trimmed; optional fields may be null.
/// </remarks>
public class Payee : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique code identifying the payee.
    /// </summary>
    public string PayeeCode { get; private set; }

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

    private Payee()
    {
        // EF Core requires a parameterless constructor for entity instantiation
    }
    
    private Payee(string payeeCode, string name, string? address, string? expenseAccountCode, string? expenseAccountName, string? tin, string? description, string? notes)
    {
        PayeeCode = payeeCode.Trim();
        Name = name.Trim();
        Address = address?.Trim();
        ExpenseAccountCode = expenseAccountCode?.Trim();
        ExpenseAccountName = expenseAccountName?.Trim();
        Tin = tin?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();
    }

    /// <summary>
    /// Create a payee with default expense account mapping and metadata.
    /// </summary>
    public static Payee Create(string payeeCode, string name, string? address, string? expenseAccountCode, string? expenseAccountName, string? tin, string? description, string? notes)
    {
        return new Payee(payeeCode, name, address, expenseAccountCode, expenseAccountName, tin, description, notes);
    }

    /// <summary>
    /// Update the payee metadata; trims inputs.
    /// </summary>
    public Payee Update(string? payeeCode, string? name, string? address, string? expenseAccountCode, string? expenseAccountName, string? tin, string? description, string? notes)
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

        if (isUpdated)
        {
            // Optionally queue a domain event here, e.g.:
            QueueDomainEvent(new PayeeUpdated(Id, this));
        }

        return this;
    }
}