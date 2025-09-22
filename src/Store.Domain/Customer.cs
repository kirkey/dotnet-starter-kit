namespace Store.Domain;

/// <summary>
/// Represents a business or retail customer with comprehensive account management, credit control, and relationship tracking.
/// </summary>
/// <remarks>
/// Use cases:
/// - Manage customer accounts for both retail and wholesale business segments.
/// - Track customer balances, credit limits, and payment history for credit management.
/// - Support customer segmentation with type-based pricing and discount strategies.
/// - Maintain accurate contact information for delivery, invoicing, and communication.
/// - Calculate customer lifetime value and purchase patterns for marketing insights.
/// - Enable customer-specific pricing agreements and volume discounts.
/// - Support multi-channel customer engagement (in-store, online, wholesale).
/// - Generate customer reports for sales analysis and business development.
/// 
/// Default values:
/// - Code: required unique identifier, max 50 characters (example: "CUST-001", "RETAIL-123")
/// - CustomerType: required classification, max 50 characters (example: "Retail", "Wholesale", "Corporate")
/// - ContactPerson: required primary contact, max 100 characters (example: "John Smith")
/// - Email: required contact email, max 255 characters (example: "john@example.com")
/// - Phone: required contact phone, max 50 characters (example: "+1-555-0123")
/// - Address: required delivery address, max 500 characters
/// - City: required city name, max 100 characters (example: "Seattle")
/// - State: optional state/region, max 100 characters (example: "WA")
/// - Country: required country code, max 100 characters (example: "US")
/// - PostalCode: optional postal code, max 20 characters (example: "98101")
/// - CreditLimit: 0.00 (no credit limit unless specified)
/// - CurrentBalance: 0.00 (outstanding balance starts at zero)
/// - LifetimeValue: 0.00 (calculated from purchase history)
/// - IsActive: true (customers are active by default)
/// 
/// Business rules:
/// - Code must be unique within the system
/// - Email must be valid format and unique
/// - Phone number should be valid format
/// - CreditLimit must be non-negative
/// - CurrentBalance represents outstanding amount owed
/// - Cannot delete customers with transaction history
/// - Wholesale customers typically have higher credit limits
/// - Customer type determines pricing and payment terms
/// - Address must be complete for delivery services
/// - Inactive customers cannot place new orders
/// </remarks>
/// <seealso cref="Store.Domain.Events.CustomerCreated"/>
/// <seealso cref="Store.Domain.Events.CustomerUpdated"/>
/// <seealso cref="Store.Domain.Events.CustomerBalanceUpdated"/>
/// <seealso cref="Store.Domain.Events.CustomerLifetimeValueUpdated"/>
/// <seealso cref="Store.Domain.Events.CustomerActivated"/>
/// <seealso cref="Store.Domain.Events.CustomerDeactivated"/>
/// <seealso cref="Store.Domain.Events.CustomerCreditLimitChanged"/>
/// <seealso cref="Store.Domain.Exceptions.Customer.CustomerNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.Customer.DuplicateCustomerCodeException"/>
/// <seealso cref="Store.Domain.Exceptions.Customer.InvalidCustomerEmailException"/>
/// <seealso cref="Store.Domain.Exceptions.Customer.CustomerCreditLimitExceededException"/>
public sealed class Customer : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Short unique code for the customer. Example: "CUST-1001".
    /// Max length: 50.
    /// </summary>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Type of customer: e.g. "Retail", "Wholesale", "Corporate".
    /// Use to apply pricing or payment rules. Max length: 50.
    /// </summary>
    public string CustomerType { get; private set; } = default!; // Retail, Wholesale, Corporate

    /// <summary>
    /// Primary contact person for this customer. Example: "Jane Doe".
    /// Max length: 100.
    /// </summary>
    public string ContactPerson { get; private set; } = default!;

    /// <summary>
    /// Contact email used for notifications and invoices. Example: "contact@example.com".
    /// Max length: 255.
    /// </summary>
    public string Email { get; private set; } = default!;

    /// <summary>
    /// Contact phone number. Example: "+1-555-0100".
    /// Max length: 50.
    /// </summary>
    public string Phone { get; private set; } = default!;

    /// <summary>
    /// Primary postal address for deliveries.
    /// Max length: 500.
    /// </summary>
    public string Address { get; private set; } = default!;

    /// <summary>
    /// City of the customer's address. Example: "Seattle".
    /// Max length: 100.
    /// </summary>
    public string City { get; private set; } = default!;

    /// <summary>
    /// State or region (optional). Example: "WA".
    /// Max length: 100.
    /// </summary>
    public string? State { get; private set; }

    /// <summary>
    /// Country of the customer. Example: "US".
    /// Max length: 100.
    /// </summary>
    public string Country { get; private set; } = default!;

    /// <summary>
    /// Postal/ZIP code (optional). Example: "98101".
    /// Max length: 20.
    /// </summary>
    public string? PostalCode { get; private set; }

    /// <summary>
    /// Allowed credit limit for the customer. Default: 0.
    /// Use to check if customer can place orders on credit.
    /// </summary>
    public decimal CreditLimit { get; private set; }

    /// <summary>
    /// Current outstanding balance. Default: 0.
    /// Updated when invoices are created or payments received.
    /// </summary>
    public decimal CurrentBalance { get; private set; }

    /// <summary>
    /// Number of days before payment is due. Default commonly 30.
    /// Example: 30 for net-30 terms, 0 for immediate payment.
    /// </summary>
    public int PaymentTermsDays { get; private set; }

    /// <summary>
    /// Standard discount percentage to apply for this customer (0-100). Default: 0.
    /// Example: 5.0 for 5% discount, 10.0 for 10% discount.
    /// </summary>
    public decimal DiscountPercentage { get; private set; }

    /// <summary>
    /// Whether the customer account is active. Default: true.
    /// Used to disable customers without deleting records.
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
    /// Updated when sales orders are placed.
    /// </summary>
    public DateTime? LastOrderDate { get; private set; }

    /// <summary>
    /// Cumulative value of all orders placed by this customer. Default: 0.
    /// Updated when orders are completed or invoiced.
    /// </summary>
    public decimal LifetimeValue { get; private set; }
    
    /// <summary>
    /// Navigation property to sales orders placed by this customer.
    /// </summary>
    public ICollection<SalesOrder> SalesOrders { get; private set; } = new List<SalesOrder>();

    /// <summary>
    /// Navigation property to wholesale contracts for this customer.
    /// </summary>
    public ICollection<WholesaleContract> WholesaleContracts { get; private set; } = new List<WholesaleContract>();

    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex PhoneRegex = new(@"^[0-9+()\-\s]{5,50}$", RegexOptions.Compiled);

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
        if (!PhoneRegex.IsMatch(phone)) throw new ArgumentException("Phone format is invalid", nameof(phone));

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
        if (discountPercentage is < 0m or > 100m) throw new ArgumentException("DiscountPercentage must be between 0 and 100", nameof(discountPercentage));

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

    /// <summary>
    /// Factory method to create a new Customer.
    /// </summary>
    /// <param name="name">The name of the customer. Required.</param>
    /// <param name="description">Optional description for the customer.</param>
    /// <param name="code">Unique code for the customer. Required.</param>
    /// <param name="customerType">Type of customer (e.g. Retail, Wholesale, Corporate). Required.</param>
    /// <param name="contactPerson">Primary contact person for the customer. Required.</param>
    /// <param name="email">Contact email for the customer. Required.</param>
    /// <param name="phone">Contact phone number for the customer. Required.</param>
    /// <param name="address">Primary address for the customer. Required.</param>
    /// <param name="city">City of the customer's address. Required.</param>
    /// <param name="state">State or region of the customer's address. Optional.</param>
    /// <param name="country">Country of the customer. Required.</param>
    /// <param name="postalCode">Postal/ZIP code of the customer. Optional.</param>
    /// <param name="creditLimit">Allowed credit limit for the customer. Default: 0.</param>
    /// <param name="paymentTermsDays">Number of days before payment is due. Default: 30.</param>
    /// <param name="discountPercentage">Standard discount percentage for the customer. Default: 0.</param>
    /// <param name="taxNumber">Tax identifier (e.g. VAT or GST number). Optional.</param>
    /// <param name="businessLicense">Business license number for corporate customers. Optional.</param>
    /// <param name="notes">Additional notes for the customer. Optional.</param>
    /// <returns>A new instance of the <see cref="Customer"/> class.</returns>
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

    /// <summary>
    /// Updates the current balance of the customer.
    /// </summary>
    /// <param name="amount">The amount to update the balance by. Can be positive or negative.</param>
    /// <param name="operation">The operation to perform: ADD, SUBTRACT, or SET.</param>
    /// <returns>The updated <see cref="Customer"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the operation is invalid.</exception>
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

    /// <summary>
    /// Updates the lifetime value of the customer.
    /// </summary>
    /// <param name="additionalValue">The additional value to add to the lifetime value.</param>
    /// <returns>The updated <see cref="Customer"/> instance.</returns>
    public Customer UpdateLifetimeValue(decimal additionalValue)
    {
        LifetimeValue += additionalValue;
        QueueDomainEvent(new CustomerLifetimeValueUpdated { Customer = this, AdditionalValue = additionalValue });
        return this;
    }

    /// <summary>
    /// Records the date of the customer's last order.
    /// </summary>
    /// <param name="orderDate">The date of the last order.</param>
    /// <returns>The updated <see cref="Customer"/> instance.</returns>
    public Customer RecordLastOrder(DateTime orderDate)
    {
        LastOrderDate = orderDate;
        return this;
    }

    /// <summary>
    /// Checks if the customer is a wholesale customer.
    /// </summary>
    /// <returns>True if the customer is wholesale, false otherwise.</returns>
    public bool IsWholesaleCustomer() => CustomerType.Equals("Wholesale", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Checks if the customer is a retail customer.
    /// </summary>
    /// <returns>True if the customer is retail, false otherwise.</returns>
    public bool IsRetailCustomer() => CustomerType.Equals("Retail", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Checks if the customer is a corporate customer.
    /// </summary>
    /// <returns>True if the customer is corporate, false otherwise.</returns>
    public bool IsCorporateCustomer() => CustomerType.Equals("Corporate", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Checks if the customer is over their credit limit.
    /// </summary>
    /// <returns>True if the customer is over credit limit, false otherwise.</returns>
    public bool IsOverCreditLimit() => CurrentBalance > CreditLimit;

    /// <summary>
    /// Gets the available credit for the customer.
    /// </summary>
    /// <returns>The amount of available credit.</returns>
    public decimal GetAvailableCredit() => Math.Max(0, CreditLimit - CurrentBalance);

    /// <summary>
    /// Updates the customer information.
    /// </summary>
    /// <param name="name">The new name of the customer.</param>
    /// <param name="description">The new description of the customer.</param>
    /// <param name="code">The new unique code for the customer.</param>
    /// <param name="customerType">The new type of customer.</param>
    /// <param name="contactPerson">The new primary contact person for the customer.</param>
    /// <param name="email">The new contact email for the customer.</param>
    /// <param name="phone">The new contact phone number for the customer.</param>
    /// <param name="address">The new primary address for the customer.</param>
    /// <param name="city">The new city of the customer's address.</param>
    /// <param name="state">The new state or region of the customer's address.</param>
    /// <param name="country">The new country of the customer.</param>
    /// <param name="postalCode">The new postal/ZIP code of the customer.</param>
    /// <param name="creditLimit">The new allowed credit limit for the customer.</param>
    /// <param name="paymentTermsDays">The new number of days before payment is due.</param>
    /// <param name="discountPercentage">The new standard discount percentage for the customer.</param>
    /// <param name="taxNumber">The new tax identifier for the customer.</param>
    /// <param name="businessLicense">The new business license number for corporate customers.</param>
    /// <param name="notes">The new additional notes for the customer.</param>
    /// <returns>The updated <see cref="Customer"/> instance.</returns>
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
            if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));
            Name = name;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            if (description is { Length: > 2000 }) throw new ArgumentException("Description must not exceed 2000 characters", nameof(description));
            Description = description;
            isUpdated = true;
        }

        if (!string.Equals(Code, code, StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code is required", nameof(code));
            if (code.Length > 50) throw new ArgumentException("Code must not exceed 50 characters", nameof(code));
            Code = code;
            isUpdated = true;
        }

        if (!string.Equals(CustomerType, customerType, StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(customerType)) throw new ArgumentException("CustomerType is required", nameof(customerType));
            if (customerType.Length > 50) throw new ArgumentException("CustomerType must not exceed 50 characters", nameof(customerType));
            CustomerType = customerType;
            isUpdated = true;
        }

        if (!string.Equals(ContactPerson, contactPerson, StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(contactPerson)) throw new ArgumentException("ContactPerson is required", nameof(contactPerson));
            if (contactPerson.Length > 100) throw new ArgumentException("ContactPerson must not exceed 100 characters", nameof(contactPerson));
            ContactPerson = contactPerson;
            isUpdated = true;
        }

        if (!string.Equals(Email, email, StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required", nameof(email));
            if (email.Length > 255) throw new ArgumentException("Email must not exceed 255 characters", nameof(email));
            if (!EmailRegex.IsMatch(email)) throw new Exceptions.Customer.InvalidCustomerEmailException(email);
            Email = email;
            isUpdated = true;
        }

        if (!string.Equals(Phone, phone, StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("Phone is required", nameof(phone));
            if (phone.Length > 50) throw new ArgumentException("Phone must not exceed 50 characters", nameof(phone));
            if (!PhoneRegex.IsMatch(phone)) throw new ArgumentException("Phone format is invalid", nameof(phone));
            Phone = phone;
            isUpdated = true;
        }

        if (!string.Equals(Address, address, StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Address is required", nameof(address));
            if (address.Length > 500) throw new ArgumentException("Address must not exceed 500 characters", nameof(address));
            Address = address;
            isUpdated = true;
        }

        if (!string.Equals(City, city, StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City is required", nameof(city));
            if (city.Length > 100) throw new ArgumentException("City must not exceed 100 characters", nameof(city));
            City = city;
            isUpdated = true;
        }

        if (!string.Equals(State, state, StringComparison.OrdinalIgnoreCase))
        {
            if (state is { Length: > 100 }) throw new ArgumentException("State must not exceed 100 characters", nameof(state));
            State = state;
            isUpdated = true;
        }

        if (!string.Equals(Country, country, StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(country)) throw new ArgumentException("Country is required", nameof(country));
            if (country.Length > 100) throw new ArgumentException("Country must not exceed 100 characters", nameof(country));
            Country = country;
            isUpdated = true;
        }

        if (!string.Equals(PostalCode, postalCode, StringComparison.OrdinalIgnoreCase))
        {
            if (postalCode is { Length: > 20 }) throw new ArgumentException("Postal code must not exceed 20 characters", nameof(postalCode));
            PostalCode = postalCode;
            isUpdated = true;
        }

        if (CreditLimit != creditLimit)
        {
            if (creditLimit < 0m) throw new ArgumentException("CreditLimit cannot be negative", nameof(creditLimit));
            var previous = CreditLimit;
            CreditLimit = creditLimit;
            QueueDomainEvent(new Events.CustomerCreditLimitChanged { Customer = this, PreviousCreditLimit = previous, NewCreditLimit = CreditLimit });
            isUpdated = true;
        }

        if (PaymentTermsDays != paymentTermsDays)
        {
            if (paymentTermsDays < 0) throw new ArgumentException("PaymentTermsDays must be zero or greater", nameof(paymentTermsDays));
            PaymentTermsDays = paymentTermsDays;
            isUpdated = true;
        }

        if (DiscountPercentage != discountPercentage)
        {
            if (discountPercentage is < 0m or > 100m) throw new ArgumentException("DiscountPercentage must be between 0 and 100", nameof(discountPercentage));
            DiscountPercentage = discountPercentage;
            isUpdated = true;
        }

        if (!string.Equals(TaxNumber, taxNumber, StringComparison.OrdinalIgnoreCase))
        {
            if (taxNumber is { Length: > 50 }) throw new ArgumentException("TaxNumber must not exceed 50 characters", nameof(taxNumber));
            TaxNumber = taxNumber;
            isUpdated = true;
        }

        if (!string.Equals(BusinessLicense, businessLicense, StringComparison.OrdinalIgnoreCase))
        {
            if (businessLicense is { Length: > 100 }) throw new ArgumentException("BusinessLicense must not exceed 100 characters", nameof(businessLicense));
            BusinessLicense = businessLicense;
            isUpdated = true;
        }

        if (!string.Equals(Notes, notes, StringComparison.OrdinalIgnoreCase))
        {
            if (notes is { Length: > 2000 }) throw new ArgumentException("Notes must not exceed 2000 characters", nameof(notes));
            Notes = notes;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new Events.CustomerUpdated { Customer = this });
        }

        return this;
    }

    /// <summary>
    /// Activates the customer account if not already active.
    /// </summary>
    public Customer Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new Events.CustomerActivated { Customer = this });
        }
        return this;
    }

    /// <summary>
    /// Deactivates the customer account if currently active.
    /// </summary>
    public Customer Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new Events.CustomerDeactivated { Customer = this });
        }
        return this;
    }

    /// <summary>
    /// Changes the customer's credit limit with validation and emits a domain event when changed.
    /// </summary>
    public Customer ChangeCreditLimit(decimal newCreditLimit)
    {
        if (newCreditLimit < 0m) throw new ArgumentException("CreditLimit cannot be negative", nameof(newCreditLimit));
        if (newCreditLimit != CreditLimit)
        {
            var prev = CreditLimit;
            CreditLimit = newCreditLimit;
            QueueDomainEvent(new Events.CustomerCreditLimitChanged { Customer = this, PreviousCreditLimit = prev, NewCreditLimit = CreditLimit });
        }
        return this;
    }
}
