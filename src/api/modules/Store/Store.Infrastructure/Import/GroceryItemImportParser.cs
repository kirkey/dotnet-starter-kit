namespace Store.Infrastructure.Import;

using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using FSH.Framework.Core.Storage.File.Features;
using Microsoft.Extensions.Logging;

/// <summary>
/// Implementation of IGroceryItemImportParser that uses the framework's DataImport service directly.
/// Parses Excel files (.xlsx) into GroceryItemImportRow objects using the standard data import pipeline.
/// </summary>
public sealed class GroceryItemImportParser(
    ILogger<GroceryItemImportParser> logger,
    IDataImport dataImport) : IGroceryItemImportParser
{
    public async Task<IReadOnlyList<GroceryItemImportRow>> ParseAsync(FileUploadCommand file, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(file);
        
        logger.LogInformation("Starting to parse grocery items import file: {FileName}", file.Name);

        try
        {
            // Use the framework's DataImport service directly to parse Excel to strongly-typed objects
            var rows = await dataImport.ToListAsync<GroceryItemImportRow>(
                file, 
                FileType.Document, 
                "Sheet1").ConfigureAwait(false);

            logger.LogInformation("Successfully parsed {Count} rows from grocery items import file", rows.Count);
            
            return rows.AsReadOnly();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to parse grocery items import file: {FileName}", file.Name);
            throw new InvalidOperationException($"Failed to parse Excel file '{file.Name}': {ex.Message}", ex);
        }
    }
}
