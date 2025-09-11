using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public class Supplier : AuditableEntity, IAggregateRoot
{
    public string Code { get; private set; } = string.Empty;
    public string ContactPerson { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string TaxId { get; private set; } = string.Empty;
    public int PaymentTermsDays { get; private set; } = 30;
    public bool IsActive { get; private set; } = true;

    public ICollection<PurchaseOrder> PurchaseOrders { get; private set; } = new List<PurchaseOrder>();
    public ICollection<SupplierProduct> SupplierProducts { get; private set; } = new List<SupplierProduct>();

    private Supplier() { }

    private Supplier(string name, string code, string contactPerson, string address, string phone, string email, string taxId, int paymentTermsDays, bool isActive)
    {
        Name = name;
        Code = code;
        ContactPerson = contactPerson;
        Address = address;
        Phone = phone;
        Email = email;
        TaxId = taxId;
        PaymentTermsDays = paymentTermsDays;
        IsActive = isActive;
    }

    public static Supplier Create(string name, string code, string contactPerson, string address, string phone, string email, string taxId, int paymentTermsDays, bool isActive = true)
        => new(name, code, contactPerson, address, phone, email, taxId, paymentTermsDays, isActive);

    public Supplier Update(string? name, string? code, string? contactPerson, string? address, string? phone, string? email, string? taxId, int? paymentTermsDays, bool? isActive)
    {
        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.Ordinal)) Name = name;
        if (!string.IsNullOrWhiteSpace(code) && !string.Equals(Code, code, StringComparison.Ordinal)) Code = code;
        if (contactPerson is not null && !string.Equals(ContactPerson, contactPerson, StringComparison.Ordinal)) ContactPerson = contactPerson;
        if (address is not null && !string.Equals(Address, address, StringComparison.Ordinal)) Address = address;
        if (phone is not null && !string.Equals(Phone, phone, StringComparison.Ordinal)) Phone = phone;
        if (email is not null && !string.Equals(Email, email, StringComparison.OrdinalIgnoreCase)) Email = email;
        if (taxId is not null && !string.Equals(TaxId, taxId, StringComparison.Ordinal)) TaxId = taxId;
        if (paymentTermsDays.HasValue) PaymentTermsDays = paymentTermsDays.Value;
        if (isActive.HasValue) IsActive = isActive.Value;
        return this;
    }
}

