using FSH.Framework.Core.Storage.File.Features;

namespace Store.Application.Items.Import;

/// <summary>
/// Interface for parsing Item import files.
/// Implementations can support various file formats (Excel, CSV, etc.).
/// </summary>
public interface IItemImportParser
{
    /// <summary>
    /// Parses the uploaded file and returns a list of ItemImportRow objects.
    /// </summary>
    /// <param name="file">The uploaded file command containing the file data.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>A read-only list of parsed ItemImportRow objects.</returns>
    Task<IReadOnlyList<ItemImportRow>> ParseAsync(FileUploadCommand file, CancellationToken cancellationToken);
}

