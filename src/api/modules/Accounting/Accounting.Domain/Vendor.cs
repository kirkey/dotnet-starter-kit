using Accounting.Domain.Events.Vendor;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;

public class Vendor : AuditableEntity, IAggregateRoot
{
    public string VendorCode { get; private set; }
    public string? Address { get; private set; }
    public string? ExpenseAccountCode { get; private set; }
    public string? ExpenseAccountName { get; private set; }
    public string? Tin { get; private set; }
    public string? Phone { get; private set; }

    private Vendor(DefaultIdType id, string vendorCode, string name, string? address, string? expenseAccountCode, string? expenseAccountName, string? tin, string? phone, string? description, string? notes)
    {
        Id = id;
        VendorCode = vendorCode.Trim();
        Name = name.Trim();
        Address = address?.Trim();
        ExpenseAccountCode = expenseAccountCode?.Trim();
        ExpenseAccountName = expenseAccountName?.Trim();
        Tin = tin?.Trim();
        Phone = phone?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();
    }

    public static Vendor Create(string vendorCode, string name, string? address, string? expenseAccountCode, string? expenseAccountName, string? tin, string? phone, string? description, string? notes)
    {
        return new Vendor(DefaultIdType.NewGuid(), vendorCode, name, address, expenseAccountCode, expenseAccountName, tin, phone, description, notes);
    }

    public Vendor Update(string? vendorCode, string? name, string? address, string? expenseAccountCode, string? expenseAccountName, string? tin, string? phone, string? description, string? notes)
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
        if (!string.IsNullOrWhiteSpace(phone) && !string.Equals(Phone, phone, StringComparison.OrdinalIgnoreCase))
        {
            Phone = phone.Trim();
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
            QueueDomainEvent(new VendorUpdated(this));
        }
        return this;
    }
}
