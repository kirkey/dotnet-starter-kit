using MediatR;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Update.v1;
public sealed record UpdateProductCommand(
    DefaultIdType Id,
    string? Name,
    decimal Price,
    string? Description = null,
    DefaultIdType? BrandId = null) : IRequest<UpdateProductResponse>;
