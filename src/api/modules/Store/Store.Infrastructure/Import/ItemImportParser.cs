using FSH.Starter.WebApi.Store.Application.Items.Import;

namespace Store.Infrastructure.Import;

using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using FSH.Framework.Core.Storage.File.Features;
using Microsoft.Extensions.Logging;

/// <summary>
/// Implementation of IItemImportParser that uses the framework's DataImport service directly.
/// Parses Excel files (.xlsx) into ItemImportRow objects using the standard data import pipeline.
/// </summary>
public sealed class ItemImportParser(
    ILogger<ItemImportParser> logger,
    IDataImport dataImport) : IItemImportParser
{
    public async Task<IReadOnlyList<ItemImportRow>> ParseAsync(FileUploadCommand file, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(file);
        
        logger.LogInformation("Starting to parse items import file: {FileName}", file.Name);

        try
        {
            // Use the framework's DataImport service directly to parse Excel to strongly-typed objects
            var rows = await dataImport.ToListAsync<ItemImportRow>(
                file, 
                FileType.Document).ConfigureAwait(false);

            logger.LogInformation("Successfully parsed {Count} rows from items import file", rows.Count);
            
            return rows.AsReadOnly();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to parse items import file: {FileName}", file.Name);
            throw new InvalidOperationException($"Failed to parse Excel file '{file.Name}': {ex.Message}", ex);
        }
    }
}
