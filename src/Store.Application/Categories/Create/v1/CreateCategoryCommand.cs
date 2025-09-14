namespace FSH.Starter.WebApi.Store.Application.Categories.Create.v1;

public sealed record CreateCategoryCommand(
    [property: DefaultValue("Sample Category")] string? Name,
    [property: DefaultValue("Primary category")] string? Description = null,
    [property: DefaultValue("CAT001")] string? Code = null,
    DefaultIdType? ParentCategoryId = null,
    [property: DefaultValue(true)] bool IsActive = true,
    [property: DefaultValue(0)] int SortOrder = 0,
    [property: DefaultValue(null)] string? ImageUrl = null
) : IRequest<CreateCategoryResponse>;

