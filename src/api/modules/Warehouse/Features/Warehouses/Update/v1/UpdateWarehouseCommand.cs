using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Warehouses.Update.v1;

public sealed record UpdateWarehouseCommand(
    DefaultIdType Id,
    string? Name,
    string? Code,
    string? Address,
    string? Phone,
    string? Manager,
    bool? IsActive,
    DefaultIdType? CompanyId) : IRequest<UpdateWarehouseResponse>;

