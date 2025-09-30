using Accounting.Application.Payees.Import;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using FSH.Framework.Core.Storage.File.Features;
using Microsoft.Extensions.Logging;

namespace Accounting.Infrastructure.Import;

/// <summary>
/// Implementation of IPayeeImportParser that uses the framework's DataImport service directly.
/// Parses Excel files (.xlsx) into PayeeImportRow objects using the standard data import pipeline.
/// </summary>
public sealed class PayeeImportParser(
    ILogger<PayeeImportParser> logger,
    IDataImport dataImport) : IPayeeImportParser
{
    public async Task<List<PayeeImportRow>> ParseAsync(FileUploadCommand file, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(file);
        
        logger.LogInformation("Starting to parse payees import file: {FileName}", file.Name);

        try
        {
            // Use the framework's DataImport service directly to parse Excel to strongly-typed objects
            var rows = await dataImport.ToListAsync<PayeeImportRow>(
                file, 
                FileType.Document, 
                "Sheet1").ConfigureAwait(false);

            logger.LogInformation("Successfully parsed {Count} rows from payees/import file", rows.Count);
            
            return rows.ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to parse payees import file: {FileName}", file.Name);
            throw new InvalidOperationException($"Failed to parse Excel file '{file.Name}': {ex.Message}", ex);
        }
    }
}
