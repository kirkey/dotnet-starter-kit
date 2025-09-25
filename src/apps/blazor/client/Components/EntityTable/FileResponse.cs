using System.IO;

namespace FSH.Starter.Blazor.Client.Components.EntityTable;

/// <summary>
/// Represents a downloadable file payload.
/// </summary>
public sealed class FileResponse
{
    /// <summary>
    /// The file content stream positioned at the beginning.
    /// </summary>
    public Stream Stream { get; }

    /// <summary>
    /// Creates a new file response.
    /// </summary>
    /// <param name="stream">The content stream to download.</param>
    public FileResponse(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        Stream = stream;
    }
}
