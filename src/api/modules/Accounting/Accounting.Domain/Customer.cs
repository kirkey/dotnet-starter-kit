using Accounting.Domain.Events.Customer;

namespace Accounting.Domain;

/// <summary>
/// Represents a customer account for billing/accounts receivable, including contact details, terms, and credit control.
/// </summary>
/// <remarks>
/// Tracks balance and credit limit for credit checks and supports activation/deactivation lifecycle.
/// Defaults: <see cref="IsActive"/> true on creation via factory, <see cref="CurrentBalance"/> 0, <see cref="CreditLimit"/> as provided.
/// </remarks>
public class Customer : AuditableEntity, IAggregateRoot
{
    private const int MaxCustomerCodeLength = 16;
    private const int MaxNameLength = 256;
    private const int MaxAddressLength = 500;
    private const int MaxContactPersonLength = 256;
    private const int MaxEmailLength = 256;
    private const int MaxTermsLength = 100;
    private const int MaxRevenueAccountCodeLength = 16;
    private const int MaxRevenueAccountNameLength = 256;
    private const int MaxTinLength = 50;
    private const int MaxPhoneNumberLength = 50;
    private const int MaxDescriptionLength = 1000;
    private const int MaxNotesLength = 1000;

    /// <summary>
    /// Unique external code for the customer, trimmed and length-limited.
    /// </summary>
    public string CustomerCode { get; private set; } = string.Empty;

    /// <summary>
    /// Primary physical or service address.
    /// </summary>
    public string? Address { get; private set; }

    /// <summary>
    /// Billing address if different from the service address.
    /// </summary>
    public string? BillingAddress { get; private set; }

    /// <summary>
    /// Customer contact person name.
    /// </summary>
    public string? ContactPerson { get; private set; }

    /// <summary>
    /// Customer email address.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Payment terms (e.g., Net 30).
    /// </summary>
    public string? Terms { get; private set; }

    /// <summary>
    /// Default revenue account code for invoicing.
    /// </summary>
    public string? RevenueAccountCode { get; private set; }

    /// <summary>
    /// Default revenue account name for invoicing.
    /// </summary>
    public string? RevenueAccountName { get; private set; }

    /// <summary>
    /// Tax identification number (TIN/VAT) for regulatory purposes.
    /// </summary>
    public string? Tin { get; private set; }

    /// <summary>
    /// Primary phone number.
    /// </summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// Whether the customer account is active. Defaults to true at creation.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Maximum allowed outstanding balance before new credit is restricted.
    /// </summary>
    public decimal CreditLimit { get; private set; }

    /// <summary>
    /// Current outstanding balance.
    /// </summary>
    public decimal CurrentBalance { get; private set; }

    private Customer()
    {
        // EF Core requires a parameterless constructor for entity instantiation
        CustomerCode = string.Empty;
        Name = string.Empty;
        CreditLimit = 0;
        CurrentBalance = 0;
        IsActive = false;
    }

    private Customer(string customerCode, string name, string? address, string? billingAddress,
        string? contactPerson, string? email, string? terms, string? revenueAccountCode, string? revenueAccountName,
        string? tin, string? phoneNumber, decimal creditLimit, string? description, string? notes)
    {
        var cc = customerCode.Trim();
        if (string.IsNullOrWhiteSpace(cc))
            throw new ArgumentException("Customer code is required.");
        if (cc.Length > MaxCustomerCodeLength)
            throw new ArgumentException($"Customer code cannot exceed {MaxCustomerCodeLength} characters.");

        var nm = name.Trim();
        if (string.IsNullOrWhiteSpace(nm))
            throw new ArgumentException("Customer name is required.");
        if (nm.Length > MaxNameLength)
            throw new ArgumentException($"Customer name cannot exceed {MaxNameLength} characters.");

        if (creditLimit < 0)
            throw new InvalidCustomerCreditLimitException();

        CustomerCode = cc;
        Name = nm;
        Address = address?.Trim();
        if (Address?.Length > MaxAddressLength) Address = Address.Substring(0, MaxAddressLength);
        BillingAddress = billingAddress?.Trim();
        if (BillingAddress?.Length > MaxAddressLength) BillingAddress = BillingAddress.Substring(0, MaxAddressLength);
        ContactPerson = contactPerson?.Trim();
        if (ContactPerson?.Length > MaxContactPersonLength) ContactPerson = ContactPerson.Substring(0, MaxContactPersonLength);
        Email = email?.Trim(); if (Email?.Length > MaxEmailLength) Email = Email.Substring(0, MaxEmailLength);
        Terms = terms?.Trim(); if (Terms?.Length > MaxTermsLength) Terms = Terms.Substring(0, MaxTermsLength);
        RevenueAccountCode = revenueAccountCode?.Trim(); if (RevenueAccountCode?.Length > MaxRevenueAccountCodeLength) RevenueAccountCode = RevenueAccountCode.Substring(0, MaxRevenueAccountCodeLength);
        RevenueAccountName = revenueAccountName?.Trim(); if (RevenueAccountName?.Length > MaxRevenueAccountNameLength) RevenueAccountName = RevenueAccountName.Substring(0, MaxRevenueAccountNameLength);
        Tin = tin?.Trim(); if (Tin?.Length > MaxTinLength) Tin = Tin.Substring(0, MaxTinLength);
        PhoneNumber = phoneNumber?.Trim(); if (PhoneNumber?.Length > MaxPhoneNumberLength) PhoneNumber = PhoneNumber.Substring(0, MaxPhoneNumberLength);
        CreditLimit = creditLimit;
        CurrentBalance = 0;

        // Use base class Description/Notes (if applicable)
        var desc = description?.Trim(); if (desc?.Length > MaxDescriptionLength) desc = desc.Substring(0, MaxDescriptionLength);
        var nts = notes?.Trim(); if (nts?.Length > MaxNotesLength) nts = nts.Substring(0, MaxNotesLength);
        Description = desc;
        Notes = nts;
        IsActive = true;

        QueueDomainEvent(new CustomerCreated(Id, CustomerCode, Name, Email, Terms, CreditLimit, Description, Notes));
    }

    /// <summary>
    /// Factory to create a new customer with validation and sensible defaults.
    /// </summary>
    public static Customer Create(string customerCode, string name, string? address = null, string? billingAddress = null,
        string? contactPerson = null, string? email = null, string? terms = null, string? revenueAccountCode = null,
        string? revenueAccountName = null, string? tin = null, string? phoneNumber = null, decimal creditLimit = 0,
        string? description = null, string? notes = null)
    {
        return new Customer(customerCode, name, address, billingAddress, contactPerson,
            email, terms, revenueAccountCode, revenueAccountName, tin, phoneNumber, creditLimit, description, notes);
    }

    /// <summary>
    /// Update customer metadata; trims and enforces length constraints where applicable.
    /// </summary>
    public Customer Update(string? customerCode, string? name, string? address, string? billingAddress,
        string? contactPerson, string? email, string? terms, string? revenueAccountCode, string? revenueAccountName,
        string? tin, string? phoneNumber, decimal? creditLimit, string? description, string? notes)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(customerCode) && !string.Equals(CustomerCode, customerCode, StringComparison.OrdinalIgnoreCase))
        {
            var cc = customerCode.Trim(); if (cc.Length > MaxCustomerCodeLength) cc = cc.Substring(0, MaxCustomerCodeLength);
            CustomerCode = cc; isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            var nm = name.Trim(); if (nm.Length > MaxNameLength) nm = nm.Substring(0, MaxNameLength);
            Name = nm; isUpdated = true;
        }

        if (address != Address)
        {
            Address = address?.Trim();
            if (Address?.Length > MaxAddressLength)
            {
                Address = Address.Substring(0, MaxAddressLength);
            }
            isUpdated = true;
        }

        if (billingAddress != BillingAddress)
        {
            BillingAddress = billingAddress?.Trim();
            if (BillingAddress?.Length > MaxAddressLength)
            {
                BillingAddress = BillingAddress.Substring(0, MaxAddressLength);
            }
            isUpdated = true;
        }

        if (contactPerson != ContactPerson)
        {
            ContactPerson = contactPerson?.Trim();
            if (ContactPerson?.Length > MaxContactPersonLength)
            {
                ContactPerson = ContactPerson.Substring(0, MaxContactPersonLength);
            }
            isUpdated = true;
        }

        if (email != Email)
        {
            Email = email?.Trim();
            if (Email?.Length > MaxEmailLength)
            {
                Email = Email.Substring(0, MaxEmailLength);
            }
            isUpdated = true;
        }

        if (terms != Terms)
        {
            Terms = terms?.Trim();
            if (Terms?.Length > MaxTermsLength)
            {
                Terms = Terms.Substring(0, MaxTermsLength);
            }
            isUpdated = true;
        }

        if (revenueAccountCode != RevenueAccountCode)
        {
            RevenueAccountCode = revenueAccountCode?.Trim();
            if (RevenueAccountCode?.Length > MaxRevenueAccountCodeLength)
            {
                RevenueAccountCode = RevenueAccountCode.Substring(0, MaxRevenueAccountCodeLength);
            }
            isUpdated = true;
        }

        if (revenueAccountName != RevenueAccountName)
        {
            RevenueAccountName = revenueAccountName?.Trim();
            if (RevenueAccountName?.Length > MaxRevenueAccountNameLength)
            {
                RevenueAccountName = RevenueAccountName.Substring(0, MaxRevenueAccountNameLength);
            }
            isUpdated = true;
        }

        if (tin != Tin)
        {
            Tin = tin?.Trim();
            if (Tin?.Length > MaxTinLength)
            {
                Tin = Tin.Substring(0, MaxTinLength);
            }
            isUpdated = true;
        }

        if (phoneNumber != PhoneNumber)
        {
            PhoneNumber = phoneNumber?.Trim();
            if (PhoneNumber?.Length > MaxPhoneNumberLength)
            {
                PhoneNumber = PhoneNumber.Substring(0, MaxPhoneNumberLength);
            }
            isUpdated = true;
        }

        if (creditLimit.HasValue && CreditLimit != creditLimit.Value)
        {
            if (creditLimit.Value < 0)
                throw new InvalidCustomerCreditLimitException();
            CreditLimit = creditLimit.Value;
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            if (Description?.Length > MaxDescriptionLength)
            {
                Description = Description.Substring(0, MaxDescriptionLength);
            }
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            if (Notes?.Length > MaxNotesLength)
            {
                Notes = Notes.Substring(0, MaxNotesLength);
            }
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new CustomerUpdated(this));
        }

        return this;
    }

    /// <summary>
    /// Increase balance due to e.g., invoice posting; emits a balance-changed event.
    /// </summary>
    public Customer AddToBalance(decimal amount, string transactionType, string? reference = null)
    {
        if (amount <= 0)
            throw new InvalidCustomerBalanceTransactionException();

        CurrentBalance += amount;
        QueueDomainEvent(new CustomerBalanceChanged(Id, CurrentBalance, amount, transactionType, reference));
        return this;
    }

    /// <summary>
    /// Reduce balance due to payment/credit memo; emits a balance-changed event.
    /// </summary>
    public Customer ReduceBalance(decimal amount, string transactionType, string? reference = null)
    {
        if (amount <= 0)
            throw new InvalidCustomerBalanceTransactionException();

        CurrentBalance -= amount;
        QueueDomainEvent(new CustomerBalanceChanged(Id, CurrentBalance, -amount, transactionType, reference));
        return this;
    }

    /// <summary>
    /// Set a new credit limit; emits a credit-limit-changed event.
    /// </summary>
    public Customer SetCreditLimit(decimal newCreditLimit)
    {
        if (newCreditLimit < 0)
            throw new InvalidCustomerCreditLimitException();

        var oldLimit = CreditLimit;
        CreditLimit = newCreditLimit;
        QueueDomainEvent(new CustomerCreditLimitChanged(Id, oldLimit, newCreditLimit));
        return this;
    }

    public Customer Activate()
    {
        if (IsActive)
            throw new CustomerAlreadyActiveException(Id);

        IsActive = true;
        QueueDomainEvent(new CustomerActivated(Id, CustomerCode, Name));
        return this;
    }

    public Customer Deactivate()
    {
        if (!IsActive)
            throw new CustomerAlreadyInactiveException(Id);

        IsActive = false;
        QueueDomainEvent(new CustomerDeactivated(Id, CustomerCode, Name));
        return this;
    }

    /// <summary>
    /// Whether the current balance exceeds the allowed credit limit.
    /// </summary>
    public bool IsCreditLimitExceeded()
    {
        return CurrentBalance > CreditLimit;
    }

    /// <summary>
    /// Available credit left before hitting the credit limit (not below zero).
    /// </summary>
    public decimal GetAvailableCredit()
    {
        return Math.Max(0, CreditLimit - CurrentBalance);
    }

    /// <summary>
    /// Perform a credit check for a proposed order amount.
    /// Throws when it would exceed the credit limit.
    /// </summary>
    public bool CanProcessOrder(decimal orderAmount)
    {
        if ((CurrentBalance + orderAmount) > CreditLimit)
            throw new CustomerCreditLimitExceededException(Id, CurrentBalance, CreditLimit);
            
        return (CurrentBalance + orderAmount) <= CreditLimit;
    }
}
