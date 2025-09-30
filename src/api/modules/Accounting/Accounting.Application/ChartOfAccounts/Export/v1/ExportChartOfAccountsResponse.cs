namespace Accounting.Application.ChartOfAccounts.Export.v1;

/// <summary>
/// Response object for Chart of Accounts export operations.
/// Contains the Excel file data and export metadata.
/// </summary>
public sealed class ExportChartOfAccountsResponse
{
    /// <summary>
    /// The Excel file data as byte array ready for download.
    /// </summary>
    public byte[] Data { get; init; } = Array.Empty<byte>();

    /// <summary>
    /// The MIME content type for Excel files.
    /// </summary>
    public string ContentType { get; init; } = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    /// <summary>
    /// The suggested filename for the exported Excel file.
    /// </summary>
    public string FileName { get; init; } = $"ChartOfAccounts_Export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";

    /// <summary>
    /// Number of accounts included in the export.
    /// </summary>
    public int RecordCount { get; init; }

    /// <summary>
    /// Timestamp when the export was generated.
    /// </summary>
    public DateTime ExportedAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Creates a successful export response with Excel data.
    /// </summary>
    /// <param name="data">Excel file byte array</param>
    /// <param name="recordCount">Number of records exported</param>
    /// <returns>Export response object ready for download</returns>
    public static ExportChartOfAccountsResponse Create(byte[] data, int recordCount)
    {
        return new ExportChartOfAccountsResponse
        {
            Data = data,
            RecordCount = recordCount
        };
    }
}
