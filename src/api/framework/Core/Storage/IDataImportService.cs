using FSH.Framework.Core.Storage.File.Features;

namespace FSH.Framework.Core.Storage;

/// <summary>
/// Generic service for importing data from Excel files into strongly-typed objects.
/// Provides flexible parsing with validation support and error handling.
/// </summary>
public interface IDataImportService
{
    /// <summary>
    /// Parses an Excel file into a list of strongly-typed objects.
    /// </summary>
    /// <typeparam name="T">The target type to map Excel rows to.</typeparam>
    /// <param name="file">The uploaded file command containing the Excel data.</param>
    /// <param name="sheetName">The name of the worksheet to parse. Defaults to "Sheet1".</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of parsed objects from the Excel file.</returns>
    Task<IReadOnlyList<T>> ParseAsync<T>(
        FileUploadCommand file, 
        string sheetName = "Sheet1", 
        CancellationToken cancellationToken = default) where T : class, new();

    /// <summary>
    /// Parses an Excel file with custom column mapping.
    /// </summary>
    /// <typeparam name="T">The target type to map Excel rows to.</typeparam>
    /// <param name="file">The uploaded file command containing the Excel data.</param>
    /// <param name="columnMapper">Function to map Excel row cells to object properties.</param>
    /// <param name="sheetName">The name of the worksheet to parse. Defaults to "Sheet1".</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of parsed objects from the Excel file.</returns>
    Task<IReadOnlyList<T>> ParseWithMapperAsync<T>(
        FileUploadCommand file,
        Func<IDictionary<string, object?>, T> columnMapper,
        string sheetName = "Sheet1",
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Validates an Excel file structure against expected columns.
    /// </summary>
    /// <param name="file">The uploaded file command containing the Excel data.</param>
    /// <param name="expectedColumns">List of expected column names.</param>
    /// <param name="sheetName">The name of the worksheet to validate. Defaults to "Sheet1".</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Validation result with success status and error messages if any.</returns>
    Task<ImportValidationResult> ValidateFileStructureAsync(
        FileUploadCommand file,
        IEnumerable<string> expectedColumns,
        string sheetName = "Sheet1",
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of file structure validation.
/// </summary>
public sealed class ImportValidationResult
{
    /// <summary>
    /// Indicates whether the validation was successful.
    /// </summary>
    public bool IsValid { get; init; }

    /// <summary>
    /// List of validation error messages.
    /// </summary>
    public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();

    /// <summary>
    /// List of missing required columns.
    /// </summary>
    public IReadOnlyList<string> MissingColumns { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    public static ImportValidationResult Success() => new() { IsValid = true };

    /// <summary>
    /// Creates a failed validation result with errors.
    /// </summary>
    public static ImportValidationResult Failure(IReadOnlyList<string> errors, IReadOnlyList<string> missingColumns) =>
        new() { IsValid = false, Errors = errors, MissingColumns = missingColumns };
}

