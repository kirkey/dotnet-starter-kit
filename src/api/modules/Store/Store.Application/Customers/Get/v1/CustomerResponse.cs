namespace FSH.Starter.WebApi.Store.Application.Customers.Get.v1;

public sealed record CustomerResponse(
    DefaultIdType? Id, 
    string Name, 
    string? Description,
    string Code,
    string CustomerType,
    string ContactPerson,
    string Email,
    string Phone,
    string Address,
    string City,
    string? State,
    string Country,
    string? PostalCode,
    decimal CreditLimit,
    int PaymentTermsDays,
    decimal DiscountPercentage,
    string? TaxNumber,
    string? BusinessLicense,
    string? Notes);
