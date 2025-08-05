using MediatR;

namespace Accounting.Application.Customers.Update;

public record UpdateCustomerRequest(
    DefaultIdType Id,
    string? CustomerCode = null,
    string? Name = null,
    string? Address = null,
    string? BillingAddress = null,
    string? ContactPerson = null,
    string? Email = null,
    string? Terms = null,
    string? RevenueAccountCode = null,
    string? RevenueAccountName = null,
    string? Tin = null,
    string? PhoneNumber = null,
    decimal? CreditLimit = null,
    bool? IsActive = null,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
