namespace Accounting.Application.Customers.Responses;

/// <summary>
/// Response model representing a customer entity.
/// Contains customer information including contact details, billing information, and account status.
/// </summary>
public class CustomerResponse(
    string customerCode,
    string? address,
    string? billingAddress,
    string? contactPerson,
    string? email,
    string? terms,
    string? revenueAccountCode,
    string? revenueAccountName,
    string? tin,
    string? phoneNumber,
    bool isActive,
    decimal creditLimit,
    decimal currentBalance,
    string? notes) : BaseResponse
{
    /// <summary>
    /// Unique customer code identifier.
    /// </summary>
    public string CustomerCode { get; set; } = customerCode;
    
    /// <summary>
    /// Customer's primary address.
    /// </summary>
    public string? Address { get; set; } = address;
    
    /// <summary>
    /// Customer's billing address (if different from primary address).
    /// </summary>
    public string? BillingAddress { get; set; } = billingAddress;
    
    /// <summary>
    /// Primary contact person at the customer organization.
    /// </summary>
    public string? ContactPerson { get; set; } = contactPerson;
    
    /// <summary>
    /// Customer's email address.
    /// </summary>
    public string? Email { get; set; } = email;
    
    /// <summary>
    /// Payment terms for this customer (e.g., "Net 30", "COD").
    /// </summary>
    public string? Terms { get; set; } = terms;
    
    /// <summary>
    /// Revenue account code associated with this customer.
    /// </summary>
    public string? RevenueAccountCode { get; set; } = revenueAccountCode;
    
    /// <summary>
    /// Revenue account name associated with this customer.
    /// </summary>
    public string? RevenueAccountName { get; set; } = revenueAccountName;
    
    /// <summary>
    /// Tax identification number for the customer.
    /// </summary>
    public string? Tin { get; set; } = tin;
    
    /// <summary>
    /// Customer's phone number.
    /// </summary>
    public string? PhoneNumber { get; set; } = phoneNumber;
    
    /// <summary>
    /// Indicates if the customer account is currently active.
    /// </summary>
    public bool IsActive { get; set; } = isActive;
    
    /// <summary>
    /// Maximum credit limit allowed for this customer.
    /// </summary>
    public decimal CreditLimit { get; set; } = creditLimit;
    
    /// <summary>
    /// Current outstanding balance for this customer.
    /// </summary>
    public decimal CurrentBalance { get; set; } = currentBalance;
    
    /// <summary>
    /// Additional notes or comments about the customer.
    /// </summary>
    public string? Notes { get; set; } = notes;
}
