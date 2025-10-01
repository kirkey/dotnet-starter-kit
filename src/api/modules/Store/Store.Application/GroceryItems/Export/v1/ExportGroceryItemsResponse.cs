namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Export.v1;

/// <summary>
/// Response containing the exported grocery items data as an Excel file.
/// </summary>
/// <param name="Data">The Excel file data as byte array</param>
/// <param name="ContentType">MIME type for Excel files</param>
/// <param name="FileName">Generated filename with timestamp</param>
/// <param name="Count">Total number of items exported</param>
public sealed record ExportGroceryItemsResponse(
    byte[] Data,
    string ContentType,
    string FileName,
    int Count)
{
    /// <summary>
    /// Creates a response with Excel content type and timestamped filename.
    /// </summary>
    /// <param name="data">Excel file byte data</param>
    /// <param name="count">Number of exported items</param>
    /// <returns>Export response with proper Excel metadata</returns>
    public static ExportGroceryItemsResponse Create(byte[] data, int count)
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        var fileName = $"GroceryItems_Export_{timestamp}.xlsx";
        const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        
        return new ExportGroceryItemsResponse(data, contentType, fileName, count);
    }
}
