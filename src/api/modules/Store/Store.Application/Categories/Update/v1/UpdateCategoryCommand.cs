using FSH.Framework.Core.Storage.File.Features;

namespace FSH.Starter.WebApi.Store.Application.Categories.Update.v1;

/// <summary>
/// Command to update an existing Category.
/// If <see cref="Image"/> is provided, the image will be uploaded and <see cref="ImageUrl"/> will be set to the returned URI.
/// </summary>
public sealed record UpdateCategoryCommand : IRequest<UpdateCategoryResponse>
{
    /// <summary>
    /// The identifier of the category to update. If omitted in the body, it will be taken from the route by the endpoint.
    /// </summary>
    public DefaultIdType Id { get; init; }

    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Code { get; init; }
    public DefaultIdType? ParentCategoryId { get; init; }
    public bool? IsActive { get; init; }
    public int? SortOrder { get; init; }
    public string? ImageUrl { get; init; }

    /// <summary>
    /// Optional image payload uploaded by the client. When provided, the image will be saved to storage and ImageUrl will be set to the saved file URL.
    /// </summary>
    public FileUploadCommand? Image { get; init; }
}
