using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Store.Domain;

public sealed class Customer : AuditableEntity, IAggregateRoot
{
    public string Code { get; private set; } = default!;
    public string CustomerType { get; private set; } = default!; // Retail, Wholesale, Corporate
    public string ContactPerson { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string Phone { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public string City { get; private set; } = default!;
    public string? State { get; private set; }
    public string Country { get; private set; } = default!;
    public string? PostalCode { get; private set; }
    public decimal CreditLimit { get; private set; }
    public decimal CurrentBalance { get; private set; }
    public int PaymentTermsDays { get; private set; }
    public decimal DiscountPercentage { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string? TaxNumber { get; private set; }
    public string? BusinessLicense { get; private set; }
    public DateTime? LastOrderDate { get; private set; }
    public decimal LifetimeValue { get; private set; }
    
    
    public ICollection<SalesOrder> SalesOrders { get; private set; } = new List<SalesOrder>();
    public ICollection<WholesaleContract> WholesaleContracts { get; private set; } = new List<WholesaleContract>();

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

        QueueDomainEvent(new Events.CustomerCreated { Customer = this });
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

        QueueDomainEvent(new Events.CustomerBalanceUpdated 
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
        QueueDomainEvent(new Events.CustomerLifetimeValueUpdated { Customer = this, AdditionalValue = additionalValue });
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
            QueueDomainEvent(new Events.CustomerUpdated { Customer = this });
        }

        return this;
    }
}
