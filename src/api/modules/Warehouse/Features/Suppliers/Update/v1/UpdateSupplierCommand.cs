using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Suppliers.Update.v1;

public sealed record UpdateSupplierCommand(
    DefaultIdType Id,
    string? Name,
    string? Code,
    string? ContactPerson,
    string? Address,
    string? Phone,
    string? Email,
    string? TaxId,
    int? PaymentTermsDays,
    bool? IsActive) : IRequest<UpdateSupplierResponse>;

