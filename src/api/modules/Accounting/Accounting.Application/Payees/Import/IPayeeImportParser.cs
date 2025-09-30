using FSH.Framework.Core.Storage.File.Features;

namespace Accounting.Application.Payees.Import;

/// <summary>
/// Interface for parsing Excel files containing Payees data.
/// Handles Excel file processing and converts rows to strongly-typed PayeeImportRow objects.
/// </summary>
public interface IPayeeImportParser
{
    /// <summary>
    /// Parses an Excel file and extracts Payees data into strongly-typed objects.
    /// </summary>
    /// <param name="file">The uploaded Excel file containing Payees data</param>
    /// <param name="cancellationToken">Cancellation token for async operations</param>
    /// <returns>List of parsed Payee rows ready for validation and processing</returns>
    Task<List<PayeeImportRow>> ParseAsync(FileUploadCommand file, CancellationToken cancellationToken);
}
