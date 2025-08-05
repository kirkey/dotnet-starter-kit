namespace Accounting.Application.Customers.Dtos;

public record CustomerDto(
    DefaultIdType Id,
    string CustomerCode,
    string Name,
    string? Address,
    string? BillingAddress,
    string? ContactPerson,
    string? Email,
    string? Terms,
    string? RevenueAccountCode,
    string? RevenueAccountName,
    string? Tin,
    string? PhoneNumber,
    bool IsActive,
    decimal CreditLimit,
    decimal CurrentBalance,
    string? Description,
    string? Notes);
