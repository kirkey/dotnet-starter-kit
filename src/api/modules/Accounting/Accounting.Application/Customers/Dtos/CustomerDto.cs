using FSH.Framework.Core.Extensions.Dto;

namespace Accounting.Application.Customers.Dtos;

public class CustomerDto(
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
    string? notes) : BaseDto
{
    public string CustomerCode { get; set; } = customerCode;
    public string? Address { get; set; } = address;
    public string? BillingAddress { get; set; } = billingAddress;
    public string? ContactPerson { get; set; } = contactPerson;
    public string? Email { get; set; } = email;
    public string? Terms { get; set; } = terms;
    public string? RevenueAccountCode { get; set; } = revenueAccountCode;
    public string? RevenueAccountName { get; set; } = revenueAccountName;
    public string? Tin { get; set; } = tin;
    public string? PhoneNumber { get; set; } = phoneNumber;
    public bool IsActive { get; set; } = isActive;
    public decimal CreditLimit { get; set; } = creditLimit;
    public decimal CurrentBalance { get; set; } = currentBalance;
    public string? Notes { get; set; } = notes;
}
