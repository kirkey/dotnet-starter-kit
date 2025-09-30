using FSH.Framework.Core.Storage.File.Features;

namespace Accounting.Application.ChartOfAccounts.Import;

/// <summary>
/// Interface for parsing Excel files containing Chart of Accounts data.
/// Handles Excel file processing and converts rows to strongly-typed ChartOfAccountImportRow objects.
/// </summary>
public interface IChartOfAccountImportParser
{
    /// <summary>
    /// Parses an Excel file and extracts Chart of Accounts data into strongly-typed objects.
    /// </summary>
    /// <param name="file">The uploaded Excel file containing Chart of Accounts data</param>
    /// <param name="cancellationToken">Cancellation token for async operations</param>
    /// <returns>List of parsed Chart of Account rows ready for validation and processing</returns>
    Task<List<ChartOfAccountImportRow>> ParseAsync(FileUploadCommand file, CancellationToken cancellationToken);
}
