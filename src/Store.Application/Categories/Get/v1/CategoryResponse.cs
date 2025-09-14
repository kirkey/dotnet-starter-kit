namespace FSH.Starter.WebApi.Store.Application.Categories.Get.v1;

public sealed record CategoryResponse(
    DefaultIdType? Id,
    string Name,
    string? Description,
    string Code,
    DefaultIdType? ParentCategoryId,
    bool IsActive,
    int SortOrder,
    string? ImageUrl);

