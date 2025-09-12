namespace FSH.Starter.WebApi.Store.Application.Customers.Update.v1;

public sealed record UpdateCustomerCommand(
    DefaultIdType Id,
    string? Name,
    string? Description = null,
    string? Code = null,
    string? CustomerType = null,
    string? ContactPerson = null,
    string? Email = null,
    string? Phone = null,
    string? Address = null,
    string? City = null,
    string? State = null,
    string? Country = null,
    string? PostalCode = null,
    decimal CreditLimit = 0,
    int PaymentTermsDays = 0,
    decimal DiscountPercentage = 0,
    string? TaxNumber = null,
    string? BusinessLicense = null,
    string? Notes = null) : IRequest<UpdateCustomerResponse>;
