namespace FSH.Starter.WebApi.Store.Application.Categories.Update.v1;

public sealed record UpdateCategoryCommand(
    DefaultIdType Id,
    string? Name,
    string? Description,
    string? Code,
    DefaultIdType? ParentCategoryId,
    bool? IsActive,
    int? SortOrder,
    string? ImageUrl) : IRequest<UpdateCategoryResponse>;

