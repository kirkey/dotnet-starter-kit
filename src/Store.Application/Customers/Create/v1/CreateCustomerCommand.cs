namespace FSH.Starter.WebApi.Store.Application.Customers.Create.v1;

public sealed record CreateCustomerCommand(
    [property: DefaultValue("Sample Customer")] string? Name,
    [property: DefaultValue("Primary customer")] string? Description = null,
    [property: DefaultValue("CUST001")] string? Code = null,
    [property: DefaultValue("Retail")] string? CustomerType = null,
    [property: DefaultValue("John Doe")] string? ContactPerson = null,
    [property: DefaultValue("john.doe@example.com")] string? Email = null,
    [property: DefaultValue("+1-555-123-4567")] string? Phone = null,
    [property: DefaultValue("123 Main Street")] string? Address = null,
    [property: DefaultValue("New York")] string? City = null,
    [property: DefaultValue("NY")] string? State = null,
    [property: DefaultValue("USA")] string? Country = null,
    [property: DefaultValue("10001")] string? PostalCode = null,
    [property: DefaultValue(10000)] decimal CreditLimit = 10000,
    [property: DefaultValue(30)] int PaymentTermsDays = 30,
    [property: DefaultValue(5.0)] decimal DiscountPercentage = 5.0m,
    [property: DefaultValue(null)] string? TaxNumber = null,
    [property: DefaultValue(null)] string? BusinessLicense = null,
    [property: DefaultValue(null)] string? Notes = null
) : IRequest<CreateCustomerResponse>;
