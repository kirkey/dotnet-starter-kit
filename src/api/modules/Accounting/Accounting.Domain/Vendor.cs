using Accounting.Domain.Events.Vendor;

namespace Accounting.Domain;

/// <summary>
/// Represents a vendor/supplier with billing and contact details, default expense account mapping, and activation state.
/// </summary>
/// <remarks>
/// Strings are trimmed; <see cref="IsActive"/> defaults to true on creation. Supports activate/deactivate lifecycle.
/// </remarks>
public class Vendor : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique vendor code.
    /// </summary>
    public string VendorCode { get; private set; } = null!;

    /// <summary>
    /// Mailing or physical address.
    /// </summary>
    public string? Address { get; private set; }

    /// <summary>
    /// Billing address used for invoices.
    /// </summary>
    public string? BillingAddress { get; private set; }

    /// <summary>
    /// Primary contact person.
    /// </summary>
    public string? ContactPerson { get; private set; }

    /// <summary>
    /// Vendor email address.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Payment terms (e.g., Net 30).
    /// </summary>
    public string? Terms { get; private set; }

    /// <summary>
    /// Default expense account code for purchases from this vendor.
    /// </summary>
    public string? ExpenseAccountCode { get; private set; }

    /// <summary>
    /// Default expense account name (display) for purchases.
    /// </summary>
    public string? ExpenseAccountName { get; private set; }

    /// <summary>
    /// Tax identification number.
    /// </summary>
    public string? Tin { get; private set; }

    /// <summary>
    /// Primary phone number.
    /// </summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// Whether the vendor is active.
    /// </summary>
    public bool IsActive { get; private set; }

    // Parameterless constructor for EF Core
    private Vendor()
    {
        VendorCode = string.Empty;
    }

    private Vendor(string vendorCode, string name, string? address, string? billingAddress, 
        string? contactPerson, string? email, string? terms, string? expenseAccountCode, string? expenseAccountName, 
        string? tin, string? phoneNumber, string? description, string? notes)
    {
        VendorCode = vendorCode.Trim();
        Name = name.Trim();
        Address = address?.Trim();
        BillingAddress = billingAddress?.Trim();
        ContactPerson = contactPerson?.Trim();
        Email = email?.Trim();
        Terms = terms?.Trim();
        ExpenseAccountCode = expenseAccountCode?.Trim();
        ExpenseAccountName = expenseAccountName?.Trim();
        Tin = tin?.Trim();
        PhoneNumber = phoneNumber?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();
        IsActive = true;

        QueueDomainEvent(new VendorCreated(Id, VendorCode, Name, Email, Terms, Description, Notes));
    }

    /// <summary>
    /// Create a new vendor with default active status and optional metadata.
    /// </summary>
    public static Vendor Create(string vendorCode, string name, string? address = null, string? billingAddress = null,
        string? contactPerson = null, string? email = null, string? terms = null, string? expenseAccountCode = null, 
        string? expenseAccountName = null, string? tin = null, string? phoneNumber = null, string? description = null, string? notes = null)
    {
        return new Vendor(vendorCode, name, address, billingAddress, contactPerson, 
            email, terms, expenseAccountCode, expenseAccountName, tin, phoneNumber, description, notes);
    }

    /// <summary>
    /// Update vendor metadata; trims inputs and emits an update event if any changes occur.
    /// </summary>
    public Vendor Update(string? vendorCode, string? name, string? address, string? billingAddress, 
        string? contactPerson, string? email, string? terms, string? expenseAccountCode, string? expenseAccountName, 
        string? tin, string? phoneNumber, string? description, string? notes)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(vendorCode) && !string.Equals(VendorCode, vendorCode, StringComparison.OrdinalIgnoreCase))
        {
            VendorCode = vendorCode.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name.Trim();
            isUpdated = true;
        }

        if (address != Address)
        {
            Address = address?.Trim();
            isUpdated = true;
        }

        if (billingAddress != BillingAddress)
        {
            BillingAddress = billingAddress?.Trim();
            isUpdated = true;
        }

        if (contactPerson != ContactPerson)
        {
            ContactPerson = contactPerson?.Trim();
            isUpdated = true;
        }

        if (email != Email)
        {
            Email = email?.Trim();
            isUpdated = true;
        }

        if (terms != Terms)
        {
            Terms = terms?.Trim();
            isUpdated = true;
        }

        if (expenseAccountCode != ExpenseAccountCode)
        {
            ExpenseAccountCode = expenseAccountCode?.Trim();
            isUpdated = true;
        }

        if (expenseAccountName != ExpenseAccountName)
        {
            ExpenseAccountName = expenseAccountName?.Trim();
            isUpdated = true;
        }

        if (tin != Tin)
        {
            Tin = tin?.Trim();
            isUpdated = true;
        }

        if (phoneNumber != PhoneNumber)
        {
            PhoneNumber = phoneNumber?.Trim();
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }
        
        if (isUpdated)
        {
            QueueDomainEvent(new VendorUpdated(Id, this));
        }

        return this;
    }

    /// <summary>
    /// Activate the vendor; throws if already active.
    /// </summary>
    public Vendor Activate()
    {
        if (IsActive)
            throw new VendorAlreadyActiveException(Id);

        IsActive = true;
        QueueDomainEvent(new VendorActivated(Id, VendorCode, Name));
        return this;
    }

    /// <summary>
    /// Deactivate the vendor; throws if already inactive.
    /// </summary>
    public Vendor Deactivate()
    {
        if (!IsActive)
            throw new VendorAlreadyInactiveException(Id);

        IsActive = false;
        QueueDomainEvent(new VendorDeactivated(Id, VendorCode, Name));
        return this;
    }
}
