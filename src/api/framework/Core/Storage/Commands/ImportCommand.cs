using FSH.Framework.Core.Storage.File.Features;
using MediatR;

namespace FSH.Framework.Core.Storage.Commands;

/// <summary>
/// Generic command for importing data from Excel files.
/// </summary>
/// <typeparam name="TResponse">The type of response returned after import.</typeparam>
public sealed record ImportCommand<TResponse> : IRequest<TResponse>
{
    /// <summary>
    /// The uploaded Excel file to import.
    /// </summary>
    public required FileUploadCommand File { get; init; }

    /// <summary>
    /// Optional worksheet name to import from. Defaults to "Sheet1".
    /// </summary>
    public string SheetName { get; init; } = "Sheet1";

    /// <summary>
    /// Indicates whether to validate file structure before processing.
    /// </summary>
    public bool ValidateStructure { get; init; } = true;
}

/// <summary>
/// Standard response for import operations.
/// </summary>
public sealed record ImportResponse
{
    /// <summary>
    /// Number of records successfully imported.
    /// </summary>
    public int ImportedCount { get; init; }

    /// <summary>
    /// Number of records that failed to import.
    /// </summary>
    public int FailedCount { get; init; }

    /// <summary>
    /// Total number of records processed.
    /// </summary>
    public int TotalCount => ImportedCount + FailedCount;

    /// <summary>
    /// List of error messages for failed imports.
    /// </summary>
    public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Indicates whether the import was completely successful.
    /// </summary>
    public bool IsSuccess => FailedCount == 0;

    /// <summary>
    /// Creates a successful import response.
    /// </summary>
    public static ImportResponse Success(int importedCount) =>
        new() { ImportedCount = importedCount, FailedCount = 0 };

    /// <summary>
    /// Creates a partial success import response with errors.
    /// </summary>
    public static ImportResponse PartialSuccess(int importedCount, int failedCount, IReadOnlyList<string> errors) =>
        new() { ImportedCount = importedCount, FailedCount = failedCount, Errors = errors };

    /// <summary>
    /// Creates a failed import response.
    /// </summary>
    public static ImportResponse Failure(IReadOnlyList<string> errors) =>
        new() { ImportedCount = 0, FailedCount = 0, Errors = errors };
}

