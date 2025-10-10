namespace FSH.Framework.Core.Storage;

/// <summary>
/// Generic service for exporting data collections to Excel files.
/// Provides flexible export with custom column mapping and formatting support.
/// </summary>
public interface IDataExportService
{
    /// <summary>
    /// Exports a list of objects to Excel format as a byte array.
    /// </summary>
    /// <typeparam name="T">The type of objects to export.</typeparam>
    /// <param name="data">The collection of objects to export.</param>
    /// <param name="sheetName">The name of the worksheet. Defaults to "Sheet1".</param>
    /// <returns>Byte array containing the Excel file.</returns>
    byte[] ExportToBytes<T>(IEnumerable<T> data, string sheetName = "Sheet1") where T : class;

    /// <summary>
    /// Exports a list of objects to Excel format as a stream.
    /// </summary>
    /// <typeparam name="T">The type of objects to export.</typeparam>
    /// <param name="data">The collection of objects to export.</param>
    /// <param name="sheetName">The name of the worksheet. Defaults to "Sheet1".</param>
    /// <returns>Stream containing the Excel file.</returns>
    Stream ExportToStream<T>(IEnumerable<T> data, string sheetName = "Sheet1") where T : class;

    /// <summary>
    /// Exports data with custom column configuration.
    /// </summary>
    /// <typeparam name="T">The type of objects to export.</typeparam>
    /// <param name="data">The collection of objects to export.</param>
    /// <param name="columnConfigurations">Custom column configurations for export.</param>
    /// <param name="sheetName">The name of the worksheet. Defaults to "Sheet1".</param>
    /// <returns>Stream containing the Excel file.</returns>
    Stream ExportWithConfigurationToStream<T>(
        IEnumerable<T> data,
        IEnumerable<ExportColumnConfiguration<T>> columnConfigurations,
        string sheetName = "Sheet1") where T : class;

    /// <summary>
    /// Exports data using a template file.
    /// </summary>
    /// <typeparam name="T">The type of data model to bind to the template.</typeparam>
    /// <param name="data">The data object to bind to the template.</param>
    /// <param name="templateFilePath">Full path to the template Excel file.</param>
    /// <param name="outputFolder">Directory where the processed file will be saved.</param>
    /// <returns>Stream containing the processed Excel file.</returns>
    Stream ExportUsingTemplate<T>(T data, string templateFilePath, string outputFolder) where T : class;

    /// <summary>
    /// Exports multiple sheets in a single workbook.
    /// </summary>
    /// <param name="sheets">Dictionary of sheet names and their data.</param>
    /// <returns>Stream containing the Excel file with multiple sheets.</returns>
    Stream ExportMultipleSheetsToStream(IDictionary<string, IEnumerable<object>> sheets);
}

/// <summary>
/// Configuration for export column customization.
/// </summary>
/// <typeparam name="T">The type of object being exported.</typeparam>
public sealed class ExportColumnConfiguration<T> where T : class
{
    /// <summary>
    /// The header name for the column in Excel.
    /// </summary>
    public required string HeaderName { get; init; }

    /// <summary>
    /// Function to extract the value from the object.
    /// </summary>
    public required Func<T, object?> ValueSelector { get; init; }

    /// <summary>
    /// Optional format string for the value.
    /// </summary>
    public string? Format { get; init; }

    /// <summary>
    /// Optional column width in Excel units.
    /// </summary>
    public double? ColumnWidth { get; init; }

    /// <summary>
    /// Order of the column (lower numbers appear first).
    /// </summary>
    public int Order { get; init; } = 0;
}

