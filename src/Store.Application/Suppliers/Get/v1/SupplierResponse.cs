namespace FSH.Starter.WebApi.Store.Application.Suppliers.Get.v1;

public sealed record SupplierResponse(
    DefaultIdType? Id,
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
    string? PostalCode,
    string? Website,
    decimal? CreditLimit,
    int PaymentTermsDays,
    bool IsActive,
    decimal Rating,
    string? Notes);

