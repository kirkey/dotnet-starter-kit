using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public class Company : AuditableEntity, IAggregateRoot
{
    // Address/Contact
    public string Address { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string TaxId { get; private set; } = string.Empty;

    // Navigation
    public ICollection<Store> Stores { get; private set; } = new List<Store>();
    public ICollection<Warehouse> Warehouses { get; private set; } = new List<Warehouse>();

    private Company() { }

    private Company(string name, string address, string phone, string email, string taxId)
    {
        Name = name;
        Address = address;
        Phone = phone;
        Email = email;
        TaxId = taxId;
    }

    public static Company Create(string name, string address, string phone, string email, string taxId)
        => new(name, address, phone, email, taxId);

    public Company Update(string? name, string? address, string? phone, string? email, string? taxId)
    {
        bool updated = false;
        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.Ordinal))
        { Name = name; updated = true; }
        if (address is not null && !string.Equals(Address, address, StringComparison.Ordinal))
        { Address = address; updated = true; }
        if (phone is not null && !string.Equals(Phone, phone, StringComparison.Ordinal))
        { Phone = phone; updated = true; }
        if (email is not null && !string.Equals(Email, email, StringComparison.OrdinalIgnoreCase))
        { Email = email; updated = true; }
        if (taxId is not null && !string.Equals(TaxId, taxId, StringComparison.Ordinal))
        { TaxId = taxId; updated = true; }
        return this;
    }
}

