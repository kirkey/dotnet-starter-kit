namespace FSH.Starter.WebApi.Store.Application.Suppliers.Create.v1;

public sealed record CreateSupplierCommand(
    string Name,
    string? Description,
    string Code,
    string ContactPerson,
    string Email,
    string Phone,
    string Address,
    string City,
    string? State,
    string Country,
    string? PostalCode = null,
    string? Website = null,
    decimal? CreditLimit = null,
    int PaymentTermsDays = 30,
    bool IsActive = true,
    decimal Rating = 0,
    string? Notes = null) : IRequest<CreateSupplierResponse>;

