using MediatR;

namespace FSH.Starter.WebApi.Catalog.Application.Brands.Update.v1;
public sealed record UpdateBrandCommand(
    DefaultIdType Id,
    string? Name,
    string? Description = null,
    string? Notes = null) : IRequest<UpdateBrandResponse>;
