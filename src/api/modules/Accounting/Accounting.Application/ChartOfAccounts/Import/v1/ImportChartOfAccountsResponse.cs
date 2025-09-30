namespace Accounting.Application.ChartOfAccounts.Import.v1;

/// <summary>
/// Response object for Chart of Accounts import operations.
/// Contains the count of successfully imported accounts and operation metadata.
/// </summary>
public sealed class ImportChartOfAccountsResponse
{
    /// <summary>
    /// Number of accounts successfully imported from the Excel file.
    /// </summary>
    public int ImportedCount { get; init; }

    /// <summary>
    /// Total number of rows processed from the Excel file.
    /// </summary>
    public int TotalRows { get; init; }

    /// <summary>
    /// Number of rows that failed validation or import.
    /// </summary>
    public int ErrorCount => TotalRows - ImportedCount;

    /// <summary>
    /// List of validation or processing errors encountered during import.
    /// </summary>
    public List<string> Errors { get; init; } = new();

    /// <summary>
    /// Timestamp when the import operation was completed.
    /// </summary>
    public DateTime CompletedAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Creates a successful import response.
    /// </summary>
    /// <param name="importedCount">Number of successfully imported accounts</param>
    /// <param name="totalRows">Total number of rows processed</param>
    /// <param name="errors">List of errors encountered</param>
    /// <returns>Import response object</returns>
    public static ImportChartOfAccountsResponse Create(int importedCount, int totalRows, List<string>? errors = null)
    {
        return new ImportChartOfAccountsResponse
        {
            ImportedCount = importedCount,
            TotalRows = totalRows,
            Errors = errors ?? new List<string>()
        };
    }
}
