namespace FSH.Starter.WebApi.Store.Application.Categories.Update.v1;

using FSH.Framework.Core.Storage.File.Features;

public sealed record UpdateCategoryCommand(
    DefaultIdType Id,
    string? Name,
    string? Description,
    string? Code,
    DefaultIdType? ParentCategoryId,
    bool? IsActive,
    int? SortOrder,
    string? ImageUrl) : IRequest<UpdateCategoryResponse>
{
    /// <summary>
    /// Optional image payload uploaded by the client. When provided, the image will be saved to storage and ImageUrl will be set to the saved file URL.
    /// </summary>
    public FileUploadCommand? Image { get; init; }
}
