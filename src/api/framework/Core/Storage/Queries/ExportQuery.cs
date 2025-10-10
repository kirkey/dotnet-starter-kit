using MediatR;

namespace FSH.Framework.Core.Storage.Queries;

/// <summary>
/// Generic query for exporting data to Excel files.
/// </summary>
/// <typeparam name="TFilter">The type of filter to apply when fetching data.</typeparam>
/// <typeparam name="TResponse">The type of response returned after export.</typeparam>
public sealed record ExportQuery<TFilter, TResponse> : IRequest<TResponse>
    where TFilter : class
{
    /// <summary>
    /// Filter criteria for the data to export.
    /// </summary>
    public TFilter? Filter { get; init; }

    /// <summary>
    /// Optional worksheet name. Defaults to "Sheet1".
    /// </summary>
    public string SheetName { get; init; } = "Sheet1";
}

/// <summary>
/// Standard response for export operations.
/// </summary>
public sealed record ExportResponse
{
    /// <summary>
    /// The exported file as a byte array.
    /// </summary>
    public required byte[] Data { get; init; }

    /// <summary>
    /// The exported file as a stream.
    /// </summary>
    public Stream? Stream { get; init; }

    /// <summary>
    /// Number of records exported.
    /// </summary>
    public int RecordCount { get; init; }

    /// <summary>
    /// File name for the exported file.
    /// </summary>
    public string FileName { get; init; } = "export.xlsx";

    /// <summary>
    /// Content type for the exported file.
    /// </summary>
    public string ContentType { get; init; } = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    /// <summary>
    /// Creates an export response from byte array.
    /// </summary>
    public static ExportResponse Create(byte[] data, int recordCount, string fileName = "export.xlsx") =>
        new()
        {
            Data = data,
            RecordCount = recordCount,
            FileName = fileName
        };

    /// <summary>
    /// Creates an export response from stream.
    /// </summary>
    public static ExportResponse CreateFromStream(Stream stream, int recordCount, string fileName = "export.xlsx") =>
        new()
        {
            Data = Array.Empty<byte>(),
            Stream = stream,
            RecordCount = recordCount,
            FileName = fileName
        };
}

