using FSH.Framework.Core.Storage.File.Features;

namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Import.v1;

/// <summary>
/// Command to import grocery items from an Excel file (.xlsx). Returns detailed import results.
/// </summary>
public sealed record ImportGroceryItemsCommand(FileUploadCommand File) : IRequest<ImportGroceryItemsResponse>;
