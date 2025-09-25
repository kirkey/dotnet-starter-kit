namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Import.v1;

using FSH.Framework.Core.Storage.File.Features;

/// <summary>
/// Command to import grocery items from an Excel file (.xlsx). Returns the total imported count.
/// </summary>
public sealed record ImportGroceryItemsCommand(FileUploadCommand File) : IRequest<int>;

