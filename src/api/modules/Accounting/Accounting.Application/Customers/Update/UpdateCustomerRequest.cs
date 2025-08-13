using MediatR;

namespace Accounting.Application.Customers.Update;

public class UpdateCustomerRequest : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public string? CustomerCode { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? BillingAddress { get; set; }
    public string? ContactPerson { get; set; }
    public string? Email { get; set; }
    public string? Terms { get; set; }
    public string? RevenueAccountCode { get; set; }
    public string? RevenueAccountName { get; set; }
    public string? Tin { get; set; }
    public string? PhoneNumber { get; set; }
    public decimal? CreditLimit { get; set; }
    public bool? IsActive { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    public UpdateCustomerRequest(
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
    {
        Id = id;
        CustomerCode = customerCode;
        Name = name;
        Address = address;
        BillingAddress = billingAddress;
        ContactPerson = contactPerson;
        Email = email;
        Terms = terms;
        RevenueAccountCode = revenueAccountCode;
        RevenueAccountName = revenueAccountName;
        Tin = tin;
        PhoneNumber = phoneNumber;
        CreditLimit = creditLimit;
        IsActive = isActive;
        Description = description;
        Notes = notes;
    }
}
