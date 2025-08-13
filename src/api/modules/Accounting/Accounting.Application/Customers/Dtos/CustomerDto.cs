namespace Accounting.Application.Customers.Dtos;

public class CustomerDto
{
    public DefaultIdType Id { get; set; }
    public string CustomerCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? BillingAddress { get; set; }
    public string? ContactPerson { get; set; }
    public string? Email { get; set; }
    public string? Terms { get; set; }
    public string? RevenueAccountCode { get; set; }
    public string? RevenueAccountName { get; set; }
    public string? Tin { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public decimal CreditLimit { get; set; }
    public decimal CurrentBalance { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    public CustomerDto(
        DefaultIdType id,
        string customerCode,
        string name,
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
        string? description,
        string? notes)
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
        IsActive = isActive;
        CreditLimit = creditLimit;
        CurrentBalance = currentBalance;
        Description = description;
        Notes = notes;
    }
}
