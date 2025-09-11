using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Categories.Update.v1;

public sealed record UpdateCategoryCommand(
    DefaultIdType Id,
    string? Name,
    string? Code,
    string? Description,
    bool? IsActive) : IRequest<UpdateCategoryResponse>;

