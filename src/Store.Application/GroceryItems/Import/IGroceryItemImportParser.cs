namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Import;

using FSH.Framework.Core.Storage.File.Features;

/// <summary>
/// Parses a Grocery Items Excel file into structured rows for import.
/// Implementations belong in Infrastructure; this interface lives in Application to keep CQRS clean.
/// </summary>
public interface IGroceryItemImportParser
{
    /// <summary>
    /// Parse the provided Excel file (base64) and return the rows.
    /// </summary>
    /// <param name="file">The uploaded file payload; must be an .xlsx workbook.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Sequence of parsed rows.</returns>
    Task<IReadOnlyList<GroceryItemImportRow>> ParseAsync(FileUploadCommand file, CancellationToken cancellationToken);
}

