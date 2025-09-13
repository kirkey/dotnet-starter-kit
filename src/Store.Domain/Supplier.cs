using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Store.Domain.Events;

namespace Store.Domain;

public sealed class Supplier : AuditableEntity, IAggregateRoot
{
    public string Code { get; private set; } = default!;
    public string ContactPerson { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string Phone { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public string City { get; private set; } = default!;
    public string? State { get; private set; }
    public string Country { get; private set; } = default!;
    public string? PostalCode { get; private set; }
    public string? Website { get; private set; }
    public decimal? CreditLimit { get; private set; }
    public int PaymentTermsDays { get; private set; }
    public bool IsActive { get; private set; } = true;
    public decimal Rating { get; private set; } = 0;
    
    
    public ICollection<GroceryItem> GroceryItems { get; private set; } = new List<GroceryItem>();
    public ICollection<PurchaseOrder> PurchaseOrders { get; private set; } = new List<PurchaseOrder>();

    private Supplier() { }

    private Supplier(
        DefaultIdType id,
        string name,
        string? description,
        string code,
        string contactPerson,
        string email,
        string phone,
        string address,
        string city,
        string? state,
        string country,
        string? postalCode,
        string? website,
        decimal? creditLimit,
        int paymentTermsDays,
        bool isActive,
        decimal rating,
        string? notes)
    {
        Id = id;
        Name = name;
        Description = description;
        Code = code;
        ContactPerson = contactPerson;
        Email = email;
        Phone = phone;
        Address = address;
        City = city;
        State = state;
        Country = country;
        PostalCode = postalCode;
        Website = website;
        CreditLimit = creditLimit;
        PaymentTermsDays = paymentTermsDays;
        IsActive = isActive;
        Rating = rating;
        Notes = notes;

        QueueDomainEvent(new SupplierCreated { Supplier = this });
    }

    public static Supplier Create(
        string name,
        string? description,
        string code,
        string contactPerson,
        string email,
        string phone,
        string address,
        string city,
        string? state,
        string country,
        string? postalCode = null,
        string? website = null,
        decimal? creditLimit = null,
        int paymentTermsDays = 30,
        bool isActive = true,
        decimal rating = 0,
        string? notes = null)
    {
        return new Supplier(
            DefaultIdType.NewGuid(),
            name,
            description,
            code,
            contactPerson,
            email,
            phone,
            address,
            city,
            state,
            country,
            postalCode,
            website,
            creditLimit,
            paymentTermsDays,
            isActive,
            rating,
            notes);
    }

    public Supplier Update(
        string? name,
        string? description,
        string? contactPerson,
        string? email,
        string? phone,
        string? address,
        string? city,
        string? state,
        string? country,
        string? postalCode,
        string? website,
        decimal? creditLimit,
        int? paymentTermsDays,
        decimal? rating,
        string? notes)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(contactPerson) && !string.Equals(ContactPerson, contactPerson, StringComparison.OrdinalIgnoreCase))
        {
            ContactPerson = contactPerson;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(email) && !string.Equals(Email, email, StringComparison.OrdinalIgnoreCase))
        {
            Email = email;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(phone) && !string.Equals(Phone, phone, StringComparison.OrdinalIgnoreCase))
        {
            Phone = phone;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(address) && !string.Equals(Address, address, StringComparison.OrdinalIgnoreCase))
        {
            Address = address;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(city) && !string.Equals(City, city, StringComparison.OrdinalIgnoreCase))
        {
            City = city;
            isUpdated = true;
        }

        if (!string.Equals(State, state, StringComparison.OrdinalIgnoreCase))
        {
            State = state;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(country) && !string.Equals(Country, country, StringComparison.OrdinalIgnoreCase))
        {
            Country = country;
            isUpdated = true;
        }

        if (!string.Equals(PostalCode, postalCode, StringComparison.OrdinalIgnoreCase))
        {
            PostalCode = postalCode;
            isUpdated = true;
        }

        if (!string.Equals(Website, website, StringComparison.OrdinalIgnoreCase))
        {
            Website = website;
            isUpdated = true;
        }

        if (creditLimit != CreditLimit)
        {
            CreditLimit = creditLimit;
            isUpdated = true;
        }

        if (paymentTermsDays.HasValue && PaymentTermsDays != paymentTermsDays.Value)
        {
            PaymentTermsDays = paymentTermsDays.Value;
            isUpdated = true;
        }

        if (rating.HasValue && Rating != rating.Value)
        {
            Rating = rating.Value;
            isUpdated = true;
        }

        if (!string.Equals(Notes, notes, StringComparison.OrdinalIgnoreCase))
        {
            Notes = notes;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new SupplierUpdated { Supplier = this });
        }

        return this;
    }

    public Supplier Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new SupplierActivated { Supplier = this });
        }
        return this;
    }

    public Supplier Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new SupplierDeactivated { Supplier = this });
        }
        return this;
    }
}
