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

    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

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
        // Validate required fields and lengths
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));
        if (name.Length > 200)
            throw new ArgumentException("Name must not exceed 200 characters", nameof(name));

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code is required", nameof(code));
        if (code.Length > 50)
            throw new ArgumentException("Code must not exceed 50 characters", nameof(code));

        if (string.IsNullOrWhiteSpace(contactPerson))
            throw new ArgumentException("Contact person is required", nameof(contactPerson));
        if (contactPerson.Length > 100)
            throw new ArgumentException("Contact person must not exceed 100 characters", nameof(contactPerson));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));
        if (email.Length > 255)
            throw new ArgumentException("Email must not exceed 255 characters", nameof(email));
        if (!EmailRegex.IsMatch(email))
            throw new ArgumentException("Email format is invalid", nameof(email));

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone is required", nameof(phone));
        if (phone.Length > 50)
            throw new ArgumentException("Phone must not exceed 50 characters", nameof(phone));

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address is required", nameof(address));
        if (address.Length > 500)
            throw new ArgumentException("Address must not exceed 500 characters", nameof(address));

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City is required", nameof(city));
        if (city.Length > 100)
            throw new ArgumentException("City must not exceed 100 characters", nameof(city));

        if (state != null && state.Length > 100)
            throw new ArgumentException("State must not exceed 100 characters", nameof(state));

        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country is required", nameof(country));
        if (country.Length > 100)
            throw new ArgumentException("Country must not exceed 100 characters", nameof(country));

        if (postalCode != null && postalCode.Length > 20)
            throw new ArgumentException("Postal code must not exceed 20 characters", nameof(postalCode));

        if (website != null && website.Length > 255)
            throw new ArgumentException("Website must not exceed 255 characters", nameof(website));

        if (creditLimit.HasValue && creditLimit.Value < 0m)
            throw new ArgumentException("Credit limit cannot be negative", nameof(creditLimit));

        if (paymentTermsDays < 0)
            throw new ArgumentException("Payment terms days must be zero or greater", nameof(paymentTermsDays));

        if (rating < 0m || rating > 5m)
            throw new ArgumentException("Rating must be between 0 and 5", nameof(rating));

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
            if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));
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
            if (contactPerson.Length > 100) throw new ArgumentException("Contact person must not exceed 100 characters", nameof(contactPerson));
            ContactPerson = contactPerson;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(email) && !string.Equals(Email, email, StringComparison.OrdinalIgnoreCase))
        {
            if (email.Length > 255) throw new ArgumentException("Email must not exceed 255 characters", nameof(email));
            if (!EmailRegex.IsMatch(email)) throw new ArgumentException("Email format is invalid", nameof(email));
            Email = email;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(phone) && !string.Equals(Phone, phone, StringComparison.OrdinalIgnoreCase))
        {
            if (phone.Length > 50) throw new ArgumentException("Phone must not exceed 50 characters", nameof(phone));
            Phone = phone;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(address) && !string.Equals(Address, address, StringComparison.OrdinalIgnoreCase))
        {
            if (address.Length > 500) throw new ArgumentException("Address must not exceed 500 characters", nameof(address));
            Address = address;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(city) && !string.Equals(City, city, StringComparison.OrdinalIgnoreCase))
        {
            if (city.Length > 100) throw new ArgumentException("City must not exceed 100 characters", nameof(city));
            City = city;
            isUpdated = true;
        }

        if (!string.Equals(State, state, StringComparison.OrdinalIgnoreCase))
        {
            if (state != null && state.Length > 100) throw new ArgumentException("State must not exceed 100 characters", nameof(state));
            State = state;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(country) && !string.Equals(Country, country, StringComparison.OrdinalIgnoreCase))
        {
            if (country.Length > 100) throw new ArgumentException("Country must not exceed 100 characters", nameof(country));
            Country = country;
            isUpdated = true;
        }

        if (!string.Equals(PostalCode, postalCode, StringComparison.OrdinalIgnoreCase))
        {
            if (postalCode != null && postalCode.Length > 20) throw new ArgumentException("Postal code must not exceed 20 characters", nameof(postalCode));
            PostalCode = postalCode;
            isUpdated = true;
        }

        if (!string.Equals(Website, website, StringComparison.OrdinalIgnoreCase))
        {
            if (website != null && website.Length > 255) throw new ArgumentException("Website must not exceed 255 characters", nameof(website));
            Website = website;
            isUpdated = true;
        }

        if (creditLimit != CreditLimit)
        {
            if (creditLimit.HasValue && creditLimit.Value < 0m) throw new ArgumentException("Credit limit cannot be negative", nameof(creditLimit));
            CreditLimit = creditLimit;
            isUpdated = true;
        }

        if (paymentTermsDays.HasValue && PaymentTermsDays != paymentTermsDays.Value)
        {
            if (paymentTermsDays.Value < 0) throw new ArgumentException("Payment terms days must be zero or greater", nameof(paymentTermsDays));
            PaymentTermsDays = paymentTermsDays.Value;
            isUpdated = true;
        }

        if (rating.HasValue && Rating != rating.Value)
        {
            if (rating.Value < 0m || rating.Value > 5m) throw new ArgumentException("Rating must be between 0 and 5", nameof(rating));
            Rating = rating.Value;
            isUpdated = true;
        }

        if (!string.Equals(Notes, notes, StringComparison.OrdinalIgnoreCase))
        {
            if (notes != null && notes.Length > 2000) throw new ArgumentException("Notes must not exceed 2000 characters", nameof(notes));
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
