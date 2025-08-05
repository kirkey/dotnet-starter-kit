using Accounting.Domain.Events.Vendor;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;

public class Vendor : AuditableEntity, IAggregateRoot
{
    public string VendorCode { get; private set; }
    public string? Address { get; private set; }
    public string? BillingAddress { get; private set; }
    public string? ContactPerson { get; private set; }
    public string? Email { get; private set; }
    public string? Terms { get; private set; }
    public string? ExpenseAccountCode { get; private set; }
    public string? ExpenseAccountName { get; private set; }
    public string? Tin { get; private set; }
    public string? PhoneNumber { get; private set; }
    public bool IsActive { get; private set; }

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

    public static Vendor Create(string vendorCode, string name, string? address = null, string? billingAddress = null,
        string? contactPerson = null, string? email = null, string? terms = null, string? expenseAccountCode = null, 
        string? expenseAccountName = null, string? tin = null, string? phoneNumber = null, string? description = null, string? notes = null)
    {
        return new Vendor(vendorCode, name, address, billingAddress, contactPerson, 
            email, terms, expenseAccountCode, expenseAccountName, tin, phoneNumber, description, notes);
    }

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
            QueueDomainEvent(new VendorUpdated(this));
        }

        return this;
    }

    public Vendor Activate()
    {
        if (IsActive)
            throw new VendorAlreadyActiveException(Id);

        IsActive = true;
        QueueDomainEvent(new VendorActivated(Id, VendorCode, Name));
        return this;
    }

    public Vendor Deactivate()
    {
        if (!IsActive)
            throw new VendorAlreadyInactiveException(Id);

        IsActive = false;
        QueueDomainEvent(new VendorDeactivated(Id, VendorCode, Name));
        return this;
    }
}
