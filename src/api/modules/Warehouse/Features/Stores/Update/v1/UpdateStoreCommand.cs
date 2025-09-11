using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Stores.Update.v1;

public sealed record UpdateStoreCommand(
    DefaultIdType Id,
    string? Name,
    string? Code,
    string? Address,
    string? Phone,
    string? Manager,
    bool? IsActive,
    DefaultIdType? CompanyId) : IRequest<UpdateStoreResponse>;

