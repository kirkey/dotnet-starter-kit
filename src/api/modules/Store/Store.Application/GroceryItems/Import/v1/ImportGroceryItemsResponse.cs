namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Import.v1;

/// <summary>
/// Response for grocery items import operation containing detailed results.
/// </summary>
/// <param name="TotalProcessed">Total number of rows processed from the file</param>
/// <param name="SuccessfulImports">Number of items successfully imported</param>
/// <param name="FailedImports">Number of items that failed to import</param>
/// <param name="SkippedItems">Number of items skipped due to validation or duplicates</param>
/// <param name="Errors">List of error messages for failed imports</param>
public sealed record ImportGroceryItemsResponse(
    int TotalProcessed,
    int SuccessfulImports,
    int FailedImports,
    int SkippedItems,
    IReadOnlyList<string> Errors)
{
    /// <summary>
    /// Creates a successful import response with counts and optional errors.
    /// </summary>
    /// <param name="totalProcessed">Total rows processed</param>
    /// <param name="successfulImports">Successfully imported count</param>
    /// <param name="errors">Optional list of error messages</param>
    /// <returns>Import response with calculated values</returns>
    public static ImportGroceryItemsResponse Create(int totalProcessed, int successfulImports, IReadOnlyList<string>? errors = null)
    {
        var errorList = errors ?? Array.Empty<string>();
        var failedImports = errorList.Count;
        var skippedItems = totalProcessed - successfulImports - failedImports;
        
        return new ImportGroceryItemsResponse(
            totalProcessed,
            successfulImports,
            failedImports,
            Math.Max(0, skippedItems), // Ensure non-negative
            errorList);
    }

    /// <summary>
    /// Gets the success rate as a percentage (0-100).
    /// </summary>
    public decimal SuccessRate => TotalProcessed > 0 
        ? Math.Round((decimal)SuccessfulImports / TotalProcessed * 100, 2)
        : 0;

    /// <summary>
    /// Indicates if the import was completely successful (no errors or skipped items).
    /// </summary>
    public bool IsFullySuccessful => SuccessfulImports == TotalProcessed && FailedImports == 0;

    /// <summary>
    /// Indicates if the import had partial success (some items imported successfully).
    /// </summary>
    public bool HasPartialSuccess => SuccessfulImports > 0 && SuccessfulImports < TotalProcessed;
}
