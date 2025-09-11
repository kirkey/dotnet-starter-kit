using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Companies.Update.v1;

public sealed record UpdateCompanyCommand(
    DefaultIdType Id,
    string? Name,
    string? Address,
    string? Phone,
    string? Email,
    string? TaxId) : IRequest<UpdateCompanyResponse>;

