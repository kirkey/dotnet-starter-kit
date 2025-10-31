using Accounting.Domain.Events.Customer;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a general customer account for non-utility billing, including credit management, payment terms, and account status tracking.
/// </summary>
/// <remarks>
/// Use cases:
/// - Manage customers for general accounts receivable beyond utility members.
/// - Track customer credit limits and available credit for credit management.
/// - Support various customer types (individual, business, government, non-profit).
/// - Enable customer-specific payment terms and discount arrangements.
/// - Support tax exemption tracking for qualifying customers.
/// - Manage customer account status and collections workflow.
/// - Track customer aging and payment history for credit decisions.
/// - Enable customer segmentation and targeted communications.
/// 
/// Default values:
/// - CustomerNumber: required unique identifier (example: "CUST-2025-001234")
/// - CustomerName: required customer name (example: "ABC Corporation")
/// - CustomerType: "Business" (most common for general customers)
/// - BillingAddress: required for invoicing
/// - ShippingAddress: optional if different from billing
/// - CreditLimit: 0.00 (must be explicitly set for credit customers)
/// - CurrentBalance: 0.00 (no initial balance)
/// - Status: "Active" (new customers start as active)
/// - PaymentTerms: "Net 30" (standard default terms)
/// - TaxExempt: false (most customers pay tax)
/// - IsActive: true (customers are active by default)
/// - DiscountPercentage: 0.00 (no discount unless specified)
/// 
/// Business rules:
/// - CustomerNumber must be unique within the system
/// - BillingAddress is required for invoicing
/// - CreditLimit must be non-negative
/// - CurrentBalance cannot exceed CreditLimit for credit customers
/// - Cannot delete customers with transaction history
/// - Status changes require proper authorization
/// - Tax exempt status requires documentation
/// - Payment terms must be valid standard terms
/// - Credit hold prevents new orders/invoices
/// - Inactive customers cannot receive new transactions
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.Customer.CustomerCreated"/>
/// <seealso cref="Accounting.Domain.Events.Customer.CustomerUpdated"/>
/// <seealso cref="Accounting.Domain.Events.Customer.CustomerActivated"/>
/// <seealso cref="Accounting.Domain.Events.Customer.CustomerDeactivated"/>
/// <seealso cref="Accounting.Domain.Events.Customer.CustomerCreditLimitChanged"/>
/// <seealso cref="Accounting.Domain.Events.Customer.CustomerPlacedOnCreditHold"/>
public class Customer : AuditableEntity, IAggregateRoot
{
    private const int MaxCustomerNumberLength = 50;
    private const int MaxCustomerNameLength = 256;
    private const int MaxCustomerTypeLength = 32;
    private const int MaxAddressLength = 500;
    private const int MaxEmailLength = 256;
    private const int MaxPhoneLength = 50;
    private const int MaxContactNameLength = 256;
    private const int MaxPaymentTermsLength = 100;
    private const int MaxStatusLength = 32;
    private const int MaxTaxIdLength = 50;
    private const int MaxDescriptionLength = 2048;
    private const int MaxNotesLength = 2048;

    /// <summary>
    /// Unique customer number assigned by the system.
    /// Example: "CUST-2025-001234", "C-10001". Max length: 50.
    /// </summary>
    public string CustomerNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Customer name (business name or individual name).
    /// Example: "ABC Corporation", "John Smith". Max length: 256.
    /// </summary>
    public string CustomerName { get; private set; } = string.Empty;

    /// <summary>
    /// Type of customer for segmentation and reporting.
    /// Values: "Individual", "Business", "Government", "NonProfit", "Wholesale", "Retail".
    /// Default: "Business". Max length: 32.
    /// </summary>
    public string CustomerType { get; private set; } = string.Empty;

    /// <summary>
    /// Primary billing address for invoices.
    /// Example: "123 Main St, Suite 100, Anytown, ST 12345". Max length: 500.
    /// Required for billing operations.
    /// </summary>
    public string BillingAddress { get; private set; } = string.Empty;

    /// <summary>
    /// Shipping address if different from billing address.
    /// Example: "456 Warehouse Rd, Industrial Park, Anytown, ST 12346". Max length: 500.
    /// </summary>
    public string? ShippingAddress { get; private set; }

    /// <summary>
    /// Primary email address for communications and invoicing.
    /// Example: "billing@abccorp.com". Max length: 256.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Primary phone number.
    /// Example: "(555) 123-4567", "+1-555-123-4567". Max length: 50.
    /// </summary>
    public string? Phone { get; private set; }

    /// <summary>
    /// Optional fax number.
    /// Example: "(555) 123-4568". Max length: 50.
    /// </summary>
    public string? Fax { get; private set; }

    /// <summary>
    /// Primary contact person name.
    /// Example: "Jane Doe", "Accounts Payable Manager". Max length: 256.
    /// </summary>
    public string? ContactName { get; private set; }

    /// <summary>
    /// Contact person's email address.
    /// Example: "jane.doe@abccorp.com". Max length: 256.
    /// </summary>
    public string? ContactEmail { get; private set; }

    /// <summary>
    /// Contact person's phone number.
    /// Example: "(555) 123-4569". Max length: 50.
    /// </summary>
    public string? ContactPhone { get; private set; }

    /// <summary>
    /// Credit limit authorized for this customer.
    /// Example: 50000.00 for $50,000 credit line. Default: 0.00 (cash only).
    /// Must be non-negative.
    /// </summary>
    public decimal CreditLimit { get; private set; }

    /// <summary>
    /// Current account balance owed by customer.
    /// Example: 12500.50 for outstanding invoices. Default: 0.00.
    /// Updated with invoices and payments.
    /// </summary>
    public decimal CurrentBalance { get; private set; }

    /// <summary>
    /// Payment terms for this customer.
    /// Example: "Net 30", "Net 15", "2/10 Net 30", "Due on Receipt", "COD".
    /// Default: "Net 30". Max length: 100.
    /// </summary>
    public string PaymentTerms { get; private set; } = string.Empty;

    /// <summary>
    /// Account status for credit and collections management.
    /// Values: "Active", "Inactive", "CreditHold", "PastDue", "Collections", "Closed".
    /// Default: "Active". Max length: 32.
    /// </summary>
    public string Status { get; private set; } = string.Empty;

    /// <summary>
    /// Whether the customer is tax exempt.
    /// Default: false. True requires tax exemption certificate on file.
    /// </summary>
    public bool TaxExempt { get; private set; }

    /// <summary>
    /// Tax identification number (EIN, SSN, VAT number).
    /// Example: "12-3456789" for EIN. Max length: 50.
    /// Required for tax exempt customers and 1099 reporting.
    /// </summary>
    public string? TaxId { get; private set; }

    /// <summary>
    /// Discount percentage applied to invoices.
    /// Example: 0.05 for 5% discount. Default: 0.00.
    /// Must be between 0 and 1 (0% to 100%).
    /// </summary>
    public decimal DiscountPercentage { get; private set; }

    /// <summary>
    /// Whether the customer account is active.
    /// Default: true. Inactive customers cannot receive new transactions.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Whether the customer is on credit hold.
    /// Default: false. Credit hold prevents new orders/invoices.
    /// </summary>
    public bool IsOnCreditHold { get; private set; }

    /// <summary>
    /// Date the customer account was opened.
    /// Example: 2025-03-15. Set on creation.
    /// </summary>
    public DateTime AccountOpenDate { get; private set; }

    /// <summary>
    /// Date of the last transaction (invoice or payment).
    /// Example: 2025-10-25. Updated automatically with transactions.
    /// </summary>
    public DateTime? LastTransactionDate { get; private set; }

    /// <summary>
    /// Date of the last payment received.
    /// Example: 2025-10-20. Updated when payments are applied.
    /// </summary>
    public DateTime? LastPaymentDate { get; private set; }

    /// <summary>
    /// Amount of the last payment received.
    /// Example: 5000.00. Updated when payments are applied.
    /// </summary>
    public decimal? LastPaymentAmount { get; private set; }

    /// <summary>
    /// Optional default rate schedule for service-based billing.
    /// Links to RateSchedule entity if applicable.
    /// </summary>
    public DefaultIdType? DefaultRateScheduleId { get; private set; }

    /// <summary>
    /// Optional default accounts receivable account override.
    /// Links to ChartOfAccount entity. Uses system default if null.
    /// </summary>
    public DefaultIdType? ReceivableAccountId { get; private set; }

    /// <summary>
    /// Optional sales representative or account manager.
    /// Example: "John Sales", "Sales Team A". Max length: 256.
    /// </summary>
    public string? SalesRepresentative { get; private set; }

    // Parameterless constructor for EF Core
    private Customer()
    {
        CustomerNumber = string.Empty;
        CustomerName = string.Empty;
        CustomerType = "Business";
        BillingAddress = string.Empty;
        PaymentTerms = "Net 30";
        Status = "Active";
    }

    private Customer(string customerNumber, string customerName, string customerType,
        string billingAddress, string? shippingAddress = null, string? email = null,
        string? phone = null, string? contactName = null, decimal creditLimit = 0,
        string paymentTerms = "Net 30", bool taxExempt = false, string? taxId = null,
        decimal discountPercentage = 0, DefaultIdType? defaultRateScheduleId = null,
        DefaultIdType? receivableAccountId = null, string? salesRepresentative = null,
        string? description = null, string? notes = null)
    {
        // Validations
        if (string.IsNullOrWhiteSpace(customerNumber))
            throw new ArgumentException("Customer number is required", nameof(customerNumber));

        if (customerNumber.Length > MaxCustomerNumberLength)
            throw new ArgumentException($"Customer number cannot exceed {MaxCustomerNumberLength} characters", nameof(customerNumber));

        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("Customer name is required", nameof(customerName));

        if (customerName.Length > MaxCustomerNameLength)
            throw new ArgumentException($"Customer name cannot exceed {MaxCustomerNameLength} characters", nameof(customerName));

        if (string.IsNullOrWhiteSpace(customerType))
            throw new ArgumentException("Customer type is required", nameof(customerType));

        if (string.IsNullOrWhiteSpace(billingAddress))
            throw new ArgumentException("Billing address is required", nameof(billingAddress));

        if (creditLimit < 0)
            throw new ArgumentException("Credit limit cannot be negative", nameof(creditLimit));

        if (discountPercentage < 0 || discountPercentage > 1)
            throw new ArgumentException("Discount percentage must be between 0 and 1", nameof(discountPercentage));

        CustomerNumber = customerNumber.Trim();
        Name = customerName.Trim(); // For AuditableEntity compatibility
        CustomerName = customerName.Trim();
        CustomerType = customerType.Trim();
        BillingAddress = billingAddress.Trim();
        ShippingAddress = shippingAddress?.Trim();
        Email = email?.Trim();
        Phone = phone?.Trim();
        ContactName = contactName?.Trim();
        CreditLimit = creditLimit;
        CurrentBalance = 0m;
        PaymentTerms = paymentTerms.Trim();
        Status = "Active";
        TaxExempt = taxExempt;
        TaxId = taxId?.Trim();
        DiscountPercentage = discountPercentage;
        IsActive = true;
        IsOnCreditHold = false;
        AccountOpenDate = DateTime.UtcNow;
        DefaultRateScheduleId = defaultRateScheduleId;
        ReceivableAccountId = receivableAccountId;
        SalesRepresentative = salesRepresentative?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new CustomerCreated(Id, CustomerNumber, CustomerName, CustomerType, CreditLimit, Description, Notes));
    }

    /// <summary>
    /// Factory method to create a new customer with validation.
    /// </summary>
    public static Customer Create(string customerNumber, string customerName, string customerType,
        string billingAddress, string? shippingAddress = null, string? email = null,
        string? phone = null, string? contactName = null, decimal creditLimit = 0,
        string paymentTerms = "Net 30", bool taxExempt = false, string? taxId = null,
        decimal discountPercentage = 0, DefaultIdType? defaultRateScheduleId = null,
        DefaultIdType? receivableAccountId = null, string? salesRepresentative = null,
        string? description = null, string? notes = null)
    {
        return new Customer(customerNumber, customerName, customerType, billingAddress,
            shippingAddress, email, phone, contactName, creditLimit, paymentTerms,
            taxExempt, taxId, discountPercentage, defaultRateScheduleId, receivableAccountId,
            salesRepresentative, description, notes);
    }

    /// <summary>
    /// Update customer details.
    /// </summary>
    public Customer Update(string? customerName = null, string? billingAddress = null,
        string? shippingAddress = null, string? email = null, string? phone = null,
        string? contactName = null, string? contactEmail = null, string? contactPhone = null,
        string? paymentTerms = null, bool? taxExempt = null, string? taxId = null,
        decimal? discountPercentage = null, string? salesRepresentative = null,
        string? description = null, string? notes = null)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(customerName) && CustomerName != customerName.Trim())
        {
            CustomerName = customerName.Trim();
            Name = customerName.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(billingAddress) && BillingAddress != billingAddress.Trim())
        {
            BillingAddress = billingAddress.Trim();
            isUpdated = true;
        }

        if (shippingAddress != ShippingAddress)
        {
            ShippingAddress = shippingAddress?.Trim();
            isUpdated = true;
        }

        if (email != Email)
        {
            Email = email?.Trim();
            isUpdated = true;
        }

        if (phone != Phone)
        {
            Phone = phone?.Trim();
            isUpdated = true;
        }

        if (contactName != ContactName)
        {
            ContactName = contactName?.Trim();
            isUpdated = true;
        }

        if (contactEmail != ContactEmail)
        {
            ContactEmail = contactEmail?.Trim();
            isUpdated = true;
        }

        if (contactPhone != ContactPhone)
        {
            ContactPhone = contactPhone?.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(paymentTerms) && PaymentTerms != paymentTerms.Trim())
        {
            PaymentTerms = paymentTerms.Trim();
            isUpdated = true;
        }

        if (taxExempt.HasValue && TaxExempt != taxExempt.Value)
        {
            TaxExempt = taxExempt.Value;
            isUpdated = true;
        }

        if (taxId != TaxId)
        {
            TaxId = taxId?.Trim();
            isUpdated = true;
        }

        if (discountPercentage.HasValue && DiscountPercentage != discountPercentage.Value)
        {
            if (discountPercentage.Value < 0 || discountPercentage.Value > 1)
                throw new ArgumentException("Discount percentage must be between 0 and 1");
            DiscountPercentage = discountPercentage.Value;
            isUpdated = true;
        }

        if (salesRepresentative != SalesRepresentative)
        {
            SalesRepresentative = salesRepresentative?.Trim();
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new CustomerUpdated(Id, CustomerNumber, CustomerName, Description, Notes));
        }

        return this;
    }

    /// <summary>
    /// Update credit limit with authorization.
    /// </summary>
    public Customer UpdateCreditLimit(decimal newCreditLimit, string authorizedBy)
    {
        if (string.IsNullOrWhiteSpace(authorizedBy))
            throw new ArgumentException("Authorization is required to change credit limit", nameof(authorizedBy));

        if (newCreditLimit < 0)
            throw new ArgumentException("Credit limit cannot be negative", nameof(newCreditLimit));

        decimal oldLimit = CreditLimit;
        CreditLimit = newCreditLimit;

        QueueDomainEvent(new CustomerCreditLimitChanged(Id, CustomerNumber, CustomerName, oldLimit, newCreditLimit, authorizedBy));
        return this;
    }

    /// <summary>
    /// Place customer on credit hold.
    /// </summary>
    public Customer PlaceOnCreditHold(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason is required for credit hold", nameof(reason));

        if (IsOnCreditHold)
            throw new InvalidOperationException("Customer is already on credit hold");

        IsOnCreditHold = true;
        Status = "CreditHold";
        Notes = $"{Notes}\n\nPlaced on credit hold: {reason.Trim()}".Trim();

        QueueDomainEvent(new CustomerPlacedOnCreditHold(Id, CustomerNumber, CustomerName, reason));
        return this;
    }

    /// <summary>
    /// Remove customer from credit hold.
    /// </summary>
    public Customer RemoveFromCreditHold()
    {
        if (!IsOnCreditHold)
            throw new InvalidOperationException("Customer is not on credit hold");

        IsOnCreditHold = false;
        Status = "Active";

        QueueDomainEvent(new CustomerRemovedFromCreditHold(Id, CustomerNumber, CustomerName));
        return this;
    }

    /// <summary>
    /// Deactivate customer account.
    /// </summary>
    public Customer Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("Customer is already inactive");

        IsActive = false;
        Status = "Inactive";

        QueueDomainEvent(new CustomerDeactivated(Id, CustomerNumber, CustomerName));
        return this;
    }

    /// <summary>
    /// Reactivate customer account.
    /// </summary>
    public Customer Activate()
    {
        if (IsActive)
            throw new InvalidOperationException("Customer is already active");

        IsActive = true;
        Status = "Active";

        QueueDomainEvent(new CustomerActivated(Id, CustomerNumber, CustomerName));
        return this;
    }

    /// <summary>
    /// Update current balance from invoice or payment.
    /// </summary>
    public Customer UpdateBalance(decimal amount, DateTime transactionDate)
    {
        CurrentBalance += amount;
        LastTransactionDate = transactionDate;

        if (amount < 0) // Payment
        {
            LastPaymentDate = transactionDate;
            LastPaymentAmount = Math.Abs(amount);
        }

        return this;
    }

    /// <summary>
    /// Available credit remaining.
    /// </summary>
    public decimal AvailableCredit => Math.Max(0, CreditLimit - CurrentBalance);

    /// <summary>
    /// Whether customer has exceeded credit limit.
    /// </summary>
    public bool IsOverCreditLimit => CurrentBalance > CreditLimit && CreditLimit > 0;

    /// <summary>
    /// Percentage of credit limit used.
    /// </summary>
    public decimal CreditUtilizationPercentage => CreditLimit > 0 ? (CurrentBalance / CreditLimit) * 100 : 0;
}

