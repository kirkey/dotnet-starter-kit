using FSH.Framework.Core.Storage.File.Features;

namespace FSH.Starter.WebApi.Store.Application.Categories.Create.v1;

public sealed record CreateCategoryCommand(
    [property: DefaultValue("Sample Category")] string? Name,
    [property: DefaultValue("Primary category")] string? Description = null,
    [property: DefaultValue(null)] string? Notes = null,
    [property: DefaultValue("CAT001")] string? Code = null,
    DefaultIdType? ParentCategoryId = null,
    [property: DefaultValue(true)] bool IsActive = true,
    [property: DefaultValue(0)] int SortOrder = 0,
    [property: DefaultValue(null)] string? ImageUrl = null
) : IRequest<CreateCategoryResponse>
{
    /// <summary>
    /// Optional image payload uploaded by the client. When provided, the image is uploaded to storage and ImageUrl is set from the saved file name.
    /// </summary>
    public FileUploadCommand? Image { get; init; }
}
