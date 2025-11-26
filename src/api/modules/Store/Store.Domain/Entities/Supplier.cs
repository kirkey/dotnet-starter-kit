namespace Store.Domain.Entities;

/// <summary>
/// Supplier that provides goods to the store. Stores contact, payment and rating information.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track supplier contact details for orders and deliveries.
/// - Apply supplier credit limits and payment terms.
/// - Maintain supplier performance ratings and evaluations.
/// - Store website and business information for procurement decisions.
/// - Monitor supplier activity and relationship status.
/// </remarks>
/// <seealso cref="Store.Domain.Events.SupplierCreated"/>
/// <seealso cref="Store.Domain.Events.SupplierUpdated"/>
/// <seealso cref="Store.Domain.Events.SupplierActivated"/>
/// <seealso cref="Store.Domain.Events.SupplierDeactivated"/>
/// <seealso cref="Store.Domain.Exceptions.Supplier.SupplierNotFoundException"/>
public sealed class Supplier : AuditableEntity, IAggregateRoot
{
    // Domain Constants - Binary Limits (Powers of 2)
    /// <summary>
    /// Maximum length for the supplier name field. (2^8 = 256)
    /// </summary>
    public const int NameMaxLength = 256;

    /// <summary>
    /// Minimum length for the supplier name field.
    /// </summary>
    public const int NameMinLength = 2;

    /// <summary>
    /// Maximum length for the supplier description field. (2^11 = 2048)
    /// </summary>
    public const int DescriptionMaxLength = 2048;

    /// <summary>
    /// Maximum length for the supplier notes field. (2^12 = 4096)
    /// </summary>
    public const int NotesMaxLength = 4096;

    /// <summary>
    /// Maximum length for the supplier code field. (50)
    /// </summary>
    public const int CodeMaxLength = 50;

    /// <summary>
    /// Maximum length for the contact person field. (100)
    /// </summary>
    public const int ContactPersonMaxLength = 100;

    /// <summary>
    /// Maximum length for the email field. (2^8 = 256)
    /// </summary>
    public const int EmailMaxLength = 256;

    /// <summary>
    /// Maximum length for the phone field. (50)
    /// </summary>
    public const int PhoneMaxLength = 50;

    /// <summary>
    /// Maximum length for the address field. (500)
    /// </summary>
    public const int AddressMaxLength = 500;

    /// <summary>
    /// Maximum length for the postal code field. (20)
    /// </summary>
    public const int PostalCodeMaxLength = 20;

    /// <summary>
    /// Maximum length for the website field. (2^8 = 256)
    /// </summary>
    public const int WebsiteMaxLength = 256;

    /// <summary>
    /// Maximum length for the image URL field. (500)
    /// </summary>
    public const int ImageUrlMaxLength = 500;

    /// <summary>
    /// Short supplier code. Example: "SUP-001".
    /// Max length: 50.
    /// </summary>
    public string Code { get; private set; } = null!;

    /// <summary>
    /// Main contact person at the supplier.
    /// Example: "John Smith". Max length: 100.
    /// </summary>
    public string ContactPerson { get; private set; } = null!;

    /// <summary>
    /// Contact email for supplier communications.
    /// Example: "orders@supplier.com". Max length: 255.
    /// </summary>
    public string Email { get; private set; } = null!;

    /// <summary>
    /// Contact phone number for the supplier.
    /// Example: "+1-555-0200". Max length: 50.
    /// </summary>
    public string Phone { get; private set; } = null!;

    /// <summary>
    /// Supplier address used for deliveries and billing.
    /// Max length: 500.
    /// </summary>
    public string Address { get; private set; } = null!;


    /// <summary>
    /// Postal code (optional).
    /// Example: "97201". Max length: 20.
    /// </summary>
    public string? PostalCode { get; private set; }

    /// <summary>
    /// Supplier website (optional).
    /// Example: "https://www.supplier.com". Max length: 255.
    /// </summary>
    public string? Website { get; private set; }

    /// <summary>
    /// Optional credit limit for supplier purchases.
    /// Example: 50000.00. Must be &gt;= 0 if specified.
    /// </summary>
    public decimal? CreditLimit { get; private set; }

    /// <summary>
    /// Days allowed for payment. Default: 30.
    /// Example: 30 for net-30 terms, 15 for net-15.
    /// </summary>
    public int PaymentTermsDays { get; private set; }

    /// <summary>
    /// Indicates if the supplier is active.
    /// Default: true. Used to disable suppliers without deleting records.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Supplier rating between 0 and 5.
    /// Example: 4.5 for excellent supplier, 2.0 for poor performance.
    /// Default: 0.
    /// </summary>
    public decimal Rating { get; private set; }
    
    /// <summary>
    /// Navigation property to items supplied by this supplier.
    /// </summary>
    public ICollection<Item> Items { get; private set; } = new List<Item>();

    /// <summary>
    /// Navigation property to purchase orders placed with this supplier.
    /// </summary>
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
        string? postalCode,
        string? website,
        decimal? creditLimit,
        int paymentTermsDays,
        bool isActive,
        decimal rating,
        string? notes,
        string? imageUrl = null)
    {
        // Validate required fields and lengths
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));
        if (name.Length < NameMinLength)
            throw new ArgumentException($"Name must be at least {NameMinLength} characters", nameof(name));
        if (name.Length > NameMaxLength)
            throw new ArgumentException($"Name must not exceed {NameMaxLength} characters", nameof(name));

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code is required", nameof(code));
        if (code.Length > CodeMaxLength)
            throw new ArgumentException($"Code must not exceed {CodeMaxLength} characters", nameof(code));

        if (string.IsNullOrWhiteSpace(contactPerson))
            throw new ArgumentException("Contact person is required", nameof(contactPerson));
        if (contactPerson.Length > ContactPersonMaxLength)
            throw new ArgumentException($"Contact person must not exceed {ContactPersonMaxLength} characters", nameof(contactPerson));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));
        if (email.Length > EmailMaxLength)
            throw new ArgumentException($"Email must not exceed {EmailMaxLength} characters", nameof(email));
        if (!EmailRegex.IsMatch(email))
            throw new ArgumentException("Email format is invalid", nameof(email));

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone is required", nameof(phone));
        if (phone.Length > PhoneMaxLength)
            throw new ArgumentException($"Phone must not exceed {PhoneMaxLength} characters", nameof(phone));

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address is required", nameof(address));
        if (address.Length > AddressMaxLength)
            throw new ArgumentException($"Address must not exceed {AddressMaxLength} characters", nameof(address));


        if (postalCode is { Length: > PostalCodeMaxLength })
            throw new ArgumentException($"Postal code must not exceed {PostalCodeMaxLength} characters", nameof(postalCode));

        if (website is { Length: > WebsiteMaxLength })
            throw new ArgumentException($"Website must not exceed {WebsiteMaxLength} characters", nameof(website));

        if (creditLimit is < 0m)
            throw new ArgumentException("Credit limit cannot be negative", nameof(creditLimit));

        if (paymentTermsDays < 0)
            throw new ArgumentException("Payment terms days must be zero or greater", nameof(paymentTermsDays));

        if (rating is < 0m or > 5m)
            throw new ArgumentException("Rating must be between 0 and 5", nameof(rating));

        if (imageUrl is { Length: > ImageUrlMaxLength })
            throw new ArgumentException($"ImageUrl must not exceed {ImageUrlMaxLength} characters", nameof(imageUrl));

        Id = id;
        Name = name;
        Description = description;
        Code = code;
        ContactPerson = contactPerson;
        Email = email;
        Phone = phone;
        Address = address;
        PostalCode = postalCode;
        Website = website;
        CreditLimit = creditLimit;
        PaymentTermsDays = paymentTermsDays;
        IsActive = isActive;
        Rating = rating;
        Notes = notes;
        ImageUrl = imageUrl;

        QueueDomainEvent(new SupplierCreated { Supplier = this });
    }

    /// <summary>
    /// Creates a new supplier with the specified details.
    /// </summary>
    /// <param name="name">The name of the supplier. Max length: 200.</param>
    /// <param name="description">Optional description of the supplier. Max length: 2000.</param>
    /// <param name="code">The supplier code. Max length: 50.</param>
    /// <param name="contactPerson">The main contact person at the supplier. Max length: 100.</param>
    /// <param name="email">The contact email for the supplier. Max length: 255.</param>
    /// <param name="phone">The contact phone number for the supplier. Max length: 50.</param>
    /// <param name="address">The address of the supplier. Max length: 500.</param>
    /// <param name="postalCode">Optional postal code for the supplier. Max length: 20.</param>
    /// <param name="website">Optional website URL for the supplier. Max length: 255.</param>
    /// <param name="creditLimit">Optional credit limit for the supplier. Must be &gt;= 0 if specified.</param>
    /// <param name="paymentTermsDays">Optional payment terms in days. Default is 30.</param>
    /// <param name="isActive">Optional flag indicating if the supplier is active. Default is true.</param>
    /// <param name="rating">Optional initial rating for the supplier. Must be between 0 and 5.</param>
    /// <param name="notes">Optional notes or comments about the supplier. Max length: 2000.</param>
    /// <returns>A new <see cref="Supplier"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when any of the required fields are invalid.</exception>
    public static Supplier Create(
        string name,
        string? description,
        string code,
        string contactPerson,
        string email,
        string phone,
        string address,
        string? postalCode = null,
        string? website = null,
        decimal? creditLimit = null,
        int paymentTermsDays = 30,
        bool isActive = true,
        decimal rating = 0,
        string? notes = null,
        string? imageUrl = null)
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
            postalCode,
            website,
            creditLimit,
            paymentTermsDays,
            isActive,
            rating,
            notes,
            imageUrl);
    }

    /// <summary>
    /// Updates the details of an existing supplier.
    /// </summary>
    /// <param name="name">New name for the supplier. Max length: 200.</param>
    /// <param name="description">New description for the supplier. Max length: 2000.</param>
    /// <param name="contactPerson">New contact person for the supplier. Max length: 100.</param>
    /// <param name="email">New email for the supplier. Max length: 255.</param>
    /// <param name="phone">New phone number for the supplier. Max length: 50.</param>
    /// <param name="address">New address for the supplier. Max length: 500.</param>
    /// <param name="postalCode">New postal code for the supplier. Max length: 20.</param>
    /// <param name="website">New website URL for the supplier. Max length: 255.</param>
    /// <param name="creditLimit">New credit limit for the supplier. Must be &gt;= 0 if specified.</param>
    /// <param name="paymentTermsDays">New payment terms in days for the supplier. Must be zero or greater.</param>
    /// <param name="rating">New rating for the supplier. Must be between 0 and 5.</param>
    /// <param name="notes">New notes or comments about the supplier. Max length: 2000.</param>
    /// <returns>The updated <see cref="Supplier"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when any of the updated fields are invalid.</exception>
    public Supplier Update(
        string? name,
        string? description,
        string? contactPerson,
        string? email,
        string? phone,
        string? address,
        string? postalCode,
        string? website,
        decimal? creditLimit,
        int? paymentTermsDays,
        decimal? rating,
        string? notes,
        string? imageUrl = null)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            if (name.Length < NameMinLength) throw new ArgumentException($"Name must be at least {NameMinLength} characters", nameof(name));
            if (name.Length > NameMaxLength) throw new ArgumentException($"Name must not exceed {NameMaxLength} characters", nameof(name));
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
            if (contactPerson.Length > ContactPersonMaxLength) throw new ArgumentException($"Contact person must not exceed {ContactPersonMaxLength} characters", nameof(contactPerson));
            ContactPerson = contactPerson;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(email) && !string.Equals(Email, email, StringComparison.OrdinalIgnoreCase))
        {
            if (email.Length > EmailMaxLength) throw new ArgumentException($"Email must not exceed {EmailMaxLength} characters", nameof(email));
            if (!EmailRegex.IsMatch(email)) throw new ArgumentException("Email format is invalid", nameof(email));
            Email = email;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(phone) && !string.Equals(Phone, phone, StringComparison.OrdinalIgnoreCase))
        {
            if (phone.Length > PhoneMaxLength) throw new ArgumentException($"Phone must not exceed {PhoneMaxLength} characters", nameof(phone));
            Phone = phone;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(address) && !string.Equals(Address, address, StringComparison.OrdinalIgnoreCase))
        {
            if (address.Length > AddressMaxLength) throw new ArgumentException($"Address must not exceed {AddressMaxLength} characters", nameof(address));
            Address = address;
            isUpdated = true;
        }

        if (!string.Equals(PostalCode, postalCode, StringComparison.OrdinalIgnoreCase))
        {
            if (postalCode is { Length: > PostalCodeMaxLength }) throw new ArgumentException($"Postal code must not exceed {PostalCodeMaxLength} characters", nameof(postalCode));
            PostalCode = postalCode;
            isUpdated = true;
        }

        if (!string.Equals(Website, website, StringComparison.OrdinalIgnoreCase))
        {
            if (website is { Length: > WebsiteMaxLength }) throw new ArgumentException($"Website must not exceed {WebsiteMaxLength} characters", nameof(website));
            Website = website;
            isUpdated = true;
        }

        if (creditLimit != CreditLimit)
        {
            if (creditLimit is < 0m) throw new ArgumentException("Credit limit cannot be negative", nameof(creditLimit));
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
            if (rating.Value is < 0m or > 5m) throw new ArgumentException("Rating must be between 0 and 5", nameof(rating));
            Rating = rating.Value;
            isUpdated = true;
        }

        if (!string.Equals(Notes, notes, StringComparison.OrdinalIgnoreCase))
        {
            if (notes is { Length: > NotesMaxLength }) throw new ArgumentException($"Notes must not exceed {NotesMaxLength} characters", nameof(notes));
            Notes = notes;
            isUpdated = true;
        }

        if (!string.Equals(ImageUrl, imageUrl, StringComparison.OrdinalIgnoreCase))
        {
            if (imageUrl is { Length: > ImageUrlMaxLength }) throw new ArgumentException($"ImageUrl must not exceed {ImageUrlMaxLength} characters", nameof(imageUrl));
            ImageUrl = imageUrl;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new SupplierUpdated { Supplier = this });
        }

        return this;
    }

    /// <summary>
    /// Activates the supplier, allowing orders and transactions.
    /// </summary>
    /// <returns>The updated <see cref="Supplier"/> instance.</returns>
    public Supplier Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new SupplierActivated { Supplier = this });
        }
        return this;
    }

    /// <summary>
    /// Deactivates the supplier, preventing further orders and transactions.
    /// </summary>
    /// <returns>The updated <see cref="Supplier"/> instance.</returns>
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
