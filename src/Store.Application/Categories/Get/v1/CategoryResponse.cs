namespace FSH.Starter.WebApi.Store.Application.Categories.Get.v1;

public sealed record CategoryResponse(
    DefaultIdType Id,
    DefaultIdType? ParentCategoryId,
    string Code,
    string Name,
    bool IsActive,
    int SortOrder,
    string? Description,
    string? Notes,
    string? ImageUrl);
