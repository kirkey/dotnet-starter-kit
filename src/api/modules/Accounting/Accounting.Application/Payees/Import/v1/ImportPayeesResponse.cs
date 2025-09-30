namespace Accounting.Application.Payees.Import.v1;

/// <summary>
/// Response object for Payees import operations.
/// Contains the count of successfully imported payees and operation metadata.
/// </summary>
public sealed class ImportPayeesResponse
{
    /// <summary>
    /// Number of payees successfully imported from the Excel file.
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
    /// <param name="importedCount">Number of successfully imported payees</param>
    /// <param name="totalRows">Total number of rows processed</param>
    /// <param name="errors">List of errors encountered</param>
    /// <returns>Import response object</returns>
    public static ImportPayeesResponse Create(int importedCount, int totalRows, List<string>? errors = null)
    {
        return new ImportPayeesResponse
        {
            ImportedCount = importedCount,
            TotalRows = totalRows,
            Errors = errors ?? new List<string>()
        };
    }
}
