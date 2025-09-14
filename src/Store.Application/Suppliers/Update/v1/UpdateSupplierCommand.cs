namespace FSH.Starter.WebApi.Store.Application.Suppliers.Update.v1;

public sealed record UpdateSupplierCommand(
    DefaultIdType Id,
    string? Name,
    string? Description,
    string? ContactPerson,
    string? Email,
    string? Phone,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    string? Website,
    decimal? CreditLimit,
    int? PaymentTermsDays,
    decimal? Rating,
    string? Notes) : IRequest<UpdateSupplierResponse>;

