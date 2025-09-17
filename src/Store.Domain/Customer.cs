namespace Store.Domain;

/// <summary>
/// Represents a business or retail customer who places sales orders.
/// Keep contact data accurate for invoicing and delivery.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track balances and credit limits for invoicing.
/// - Differentiate behavior for retail vs wholesale customers.
/// - Store contact info for delivery and notifications.
/// </remarks>
public sealed class Customer : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Short unique code for the customer. Example: "CUST-1001".
    /// </summary>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Type of customer: e.g. "Retail", "Wholesale", "Corporate".
    /// Use to apply pricing or payment rules. Default value expected from creator.
    /// </summary>
    public string CustomerType { get; private set; } = default!; // Retail, Wholesale, Corporate

    /// <summary>
    /// Primary contact person for this customer. Example: "Jane Doe".
    /// </summary>
    public string ContactPerson { get; private set; } = default!;

    /// <summary>
    /// Contact email used for notifications and invoices. Example: "contact@example.com".
    /// </summary>
    public string Email { get; private set; } = default!;

    /// <summary>
    /// Contact phone number. Example: "+1-555-0100".
    /// </summary>
    public string Phone { get; private set; } = default!;

    /// <summary>
    /// Primary postal address for deliveries.
    /// </summary>
    public string Address { get; private set; } = default!;

    /// <summary>
    /// City of the customer's address. Example: "Seattle".
    /// </summary>
    public string City { get; private set; } = default!;

    /// <summary>
    /// State or region (optional). Example: "WA".
    /// </summary>
    public string? State { get; private set; }

    /// <summary>
    /// Country of the customer. Example: "US".
    /// </summary>
    public string Country { get; private set; } = default!;

    /// <summary>
    /// Postal/ZIP code (optional). Example: "98101".
    /// </summary>
    public string? PostalCode { get; private set; }

    /// <summary>
    /// Allowed credit limit for the customer. Default: 0.
    /// Use to check if customer can place orders on credit.
    /// </summary>
    public decimal CreditLimit { get; private set; }

    /// <summary>
    /// Current outstanding balance. Default: 0.
    /// </summary>
    public decimal CurrentBalance { get; private set; }

    /// <summary>
    /// Number of days before payment is due. Default commonly 30.
    /// </summary>
    public int PaymentTermsDays { get; private set; }

    /// <summary>
    /// Standard discount percentage to apply for this customer (0-100). Default: 0.
    /// </summary>
    public decimal DiscountPercentage { get; private set; }

    /// <summary>
    /// Whether the customer account is active. Default: true.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Tax identifier (optional). Example: VAT or GST number.
    /// </summary>
    public string? TaxNumber { get; private set; }

    /// <summary>
    /// Business license number (optional) for corporate customers.
    /// </summary>
    public string? BusinessLicense { get; private set; }

    /// <summary>
    /// Date of the customer's last order (optional).
    /// </summary>
    public DateTime? LastOrderDate { get; private set; }

    /// <summary>
    /// Cumulative value of all orders placed by this customer. Default: 0.
    /// </summary>
    public decimal LifetimeValue { get; private set; }
    
    
    public ICollection<SalesOrder> SalesOrders { get; private set; } = new List<SalesOrder>();
    public ICollection<WholesaleContract> WholesaleContracts { get; private set; } = new List<WholesaleContract>();

    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private Customer() { }

    private Customer(
        DefaultIdType id,
        string name,
        string? description,
        string code,
        string customerType,
        string contactPerson,
        string email,
        string phone,
        string address,
        string city,
        string? state,
        string country,
        string? postalCode,
        decimal creditLimit,
        int paymentTermsDays,
        decimal discountPercentage,
        string? taxNumber,
        string? businessLicense,
        string? notes)
    {
        // domain validations
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));

        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code is required", nameof(code));
        if (code.Length > 50) throw new ArgumentException("Code must not exceed 50 characters", nameof(code));

        if (string.IsNullOrWhiteSpace(customerType)) throw new ArgumentException("CustomerType is required", nameof(customerType));
        if (customerType.Length > 50) throw new ArgumentException("CustomerType must not exceed 50 characters", nameof(customerType));

        if (string.IsNullOrWhiteSpace(contactPerson)) throw new ArgumentException("ContactPerson is required", nameof(contactPerson));
        if (contactPerson.Length > 100) throw new ArgumentException("ContactPerson must not exceed 100 characters", nameof(contactPerson));

        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required", nameof(email));
        if (email.Length > 255) throw new ArgumentException("Email must not exceed 255 characters", nameof(email));
        if (!EmailRegex.IsMatch(email)) throw new ArgumentException("Email format is invalid", nameof(email));

        if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("Phone is required", nameof(phone));
        if (phone.Length > 50) throw new ArgumentException("Phone must not exceed 50 characters", nameof(phone));

        if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Address is required", nameof(address));
        if (address.Length > 500) throw new ArgumentException("Address must not exceed 500 characters", nameof(address));

        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City is required", nameof(city));
        if (city.Length > 100) throw new ArgumentException("City must not exceed 100 characters", nameof(city));

        if (state is { Length: > 100 }) throw new ArgumentException("State must not exceed 100 characters", nameof(state));

        if (string.IsNullOrWhiteSpace(country)) throw new ArgumentException("Country is required", nameof(country));
        if (country.Length > 100) throw new ArgumentException("Country must not exceed 100 characters", nameof(country));

        if (postalCode is { Length: > 20 }) throw new ArgumentException("Postal code must not exceed 20 characters", nameof(postalCode));

        if (creditLimit < 0m) throw new ArgumentException("CreditLimit cannot be negative", nameof(creditLimit));
        if (paymentTermsDays < 0) throw new ArgumentException("PaymentTermsDays must be zero or greater", nameof(paymentTermsDays));
        if (discountPercentage < 0m || discountPercentage > 100m) throw new ArgumentException("DiscountPercentage must be between 0 and 100", nameof(discountPercentage));

        Id = id;
        Name = name;
        Description = description;
        Code = code;
        CustomerType = customerType;
        ContactPerson = contactPerson;
        Email = email;
        Phone = phone;
        Address = address;
        City = city;
        State = state;
        Country = country;
        PostalCode = postalCode;
        CreditLimit = creditLimit;
        CurrentBalance = 0;
        PaymentTermsDays = paymentTermsDays;
        DiscountPercentage = discountPercentage;
        IsActive = true;
        TaxNumber = taxNumber;
        BusinessLicense = businessLicense;
        LifetimeValue = 0;
        Notes = notes;

        QueueDomainEvent(new CustomerCreated { Customer = this });
    }

    public static Customer Create(
        string name,
        string? description,
        string code,
        string customerType,
        string contactPerson,
        string email,
        string phone,
        string address,
        string city,
        string? state,
        string country,
        string? postalCode = null,
        decimal creditLimit = 0,
        int paymentTermsDays = 30,
        decimal discountPercentage = 0,
        string? taxNumber = null,
        string? businessLicense = null,
        string? notes = null)
    {
        return new Customer(
            DefaultIdType.NewGuid(),
            name,
            description,
            code,
            customerType,
            contactPerson,
            email,
            phone,
            address,
            city,
            state,
            country,
            postalCode,
            creditLimit,
            paymentTermsDays,
            discountPercentage,
            taxNumber,
            businessLicense,
            notes);
    }

    public Customer UpdateBalance(decimal amount, string operation)
    {
        var previousBalance = CurrentBalance;
        
        switch (operation.ToUpper())
        {
            case "ADD":
                CurrentBalance += amount;
                break;
            case "SUBTRACT":
                CurrentBalance -= amount;
                break;
            case "SET":
                CurrentBalance = amount;
                break;
            default:
                throw new ArgumentException("Invalid balance operation. Use ADD, SUBTRACT, or SET.", nameof(operation));
        }

        QueueDomainEvent(new CustomerBalanceUpdated 
        { 
            Customer = this, 
            PreviousBalance = previousBalance, 
            NewBalance = CurrentBalance,
            Operation = operation,
            Amount = amount
        });

        return this;
    }

    public Customer UpdateLifetimeValue(decimal additionalValue)
    {
        LifetimeValue += additionalValue;
        QueueDomainEvent(new CustomerLifetimeValueUpdated { Customer = this, AdditionalValue = additionalValue });
        return this;
    }

    public Customer RecordLastOrder(DateTime orderDate)
    {
        LastOrderDate = orderDate;
        return this;
    }

    public bool IsWholesaleCustomer() => CustomerType.Equals("Wholesale", StringComparison.OrdinalIgnoreCase);
    public bool IsRetailCustomer() => CustomerType.Equals("Retail", StringComparison.OrdinalIgnoreCase);
    public bool IsCorporateCustomer() => CustomerType.Equals("Corporate", StringComparison.OrdinalIgnoreCase);
    public bool IsOverCreditLimit() => CurrentBalance > CreditLimit;
    public decimal GetAvailableCredit() => Math.Max(0, CreditLimit - CurrentBalance);
    public Customer Update(
        string? name,
        string? description,
        string code,
        string customerType,
        string contactPerson,
        string email,
        string phone,
        string address,
        string city,
        string? state,
        string country,
        string? postalCode,
        decimal creditLimit,
        int paymentTermsDays,
        decimal discountPercentage,
        string? taxNumber,
        string? businessLicense,
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

        if (!string.Equals(Code, code, StringComparison.OrdinalIgnoreCase))
        {
            Code = code;
            isUpdated = true;
        }

        if (!string.Equals(CustomerType, customerType, StringComparison.OrdinalIgnoreCase))
        {
            CustomerType = customerType;
            isUpdated = true;
        }

        if (!string.Equals(ContactPerson, contactPerson, StringComparison.OrdinalIgnoreCase))
        {
            ContactPerson = contactPerson;
            isUpdated = true;
        }

        if (!string.Equals(Email, email, StringComparison.OrdinalIgnoreCase))
        {
            Email = email;
            isUpdated = true;
        }

        if (!string.Equals(Phone, phone, StringComparison.OrdinalIgnoreCase))
        {
            Phone = phone;
            isUpdated = true;
        }

        if (!string.Equals(Address, address, StringComparison.OrdinalIgnoreCase))
        {
            Address = address;
            isUpdated = true;
        }

        if (!string.Equals(City, city, StringComparison.OrdinalIgnoreCase))
        {
            City = city;
            isUpdated = true;
        }

        if (!string.Equals(State, state, StringComparison.OrdinalIgnoreCase))
        {
            State = state;
            isUpdated = true;
        }

        if (!string.Equals(Country, country, StringComparison.OrdinalIgnoreCase))
        {
            Country = country;
            isUpdated = true;
        }

        if (!string.Equals(PostalCode, postalCode, StringComparison.OrdinalIgnoreCase))
        {
            PostalCode = postalCode;
            isUpdated = true;
        }

        if (CreditLimit != creditLimit)
        {
            CreditLimit = creditLimit;
            isUpdated = true;
        }

        if (PaymentTermsDays != paymentTermsDays)
        {
            PaymentTermsDays = paymentTermsDays;
            isUpdated = true;
        }

        if (DiscountPercentage != discountPercentage)
        {
            DiscountPercentage = discountPercentage;
            isUpdated = true;
        }

        if (!string.Equals(TaxNumber, taxNumber, StringComparison.OrdinalIgnoreCase))
        {
            TaxNumber = taxNumber;
            isUpdated = true;
        }

        if (!string.Equals(BusinessLicense, businessLicense, StringComparison.OrdinalIgnoreCase))
        {
            BusinessLicense = businessLicense;
            isUpdated = true;
        }

        if (!string.Equals(Notes, notes, StringComparison.OrdinalIgnoreCase))
        {
            Notes = notes;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new CustomerUpdated { Customer = this });
        }

        return this;
    }
}
