using MediatR;

namespace FSH.Starter.WebApi.Catalog.Application.Brands.Delete.v1;
public sealed record DeleteBrandCommand(
    DefaultIdType Id) : IRequest;
