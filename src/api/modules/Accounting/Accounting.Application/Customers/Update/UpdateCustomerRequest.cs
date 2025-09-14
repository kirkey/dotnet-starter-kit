namespace Accounting.Application.Customers.Update;

public class UpdateCustomerRequest(
    DefaultIdType id,
    string? customerCode = null,
    string? name = null,
    string? address = null,
    string? billingAddress = null,
    string? contactPerson = null,
    string? email = null,
    string? terms = null,
    string? revenueAccountCode = null,
    string? revenueAccountName = null,
    string? tin = null,
    string? phoneNumber = null,
    decimal? creditLimit = null,
    bool? isActive = null,
    string? description = null,
    string? notes = null)
    : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; } = id;
    public string? CustomerCode { get; set; } = customerCode;
    public string? Name { get; set; } = name;
    public string? Address { get; set; } = address;
    public string? BillingAddress { get; set; } = billingAddress;
    public string? ContactPerson { get; set; } = contactPerson;
    public string? Email { get; set; } = email;
    public string? Terms { get; set; } = terms;
    public string? RevenueAccountCode { get; set; } = revenueAccountCode;
    public string? RevenueAccountName { get; set; } = revenueAccountName;
    public string? Tin { get; set; } = tin;
    public string? PhoneNumber { get; set; } = phoneNumber;
    public decimal? CreditLimit { get; set; } = creditLimit;
    public bool? IsActive { get; set; } = isActive;
    public string? Description { get; set; } = description;
    public string? Notes { get; set; } = notes;
}
