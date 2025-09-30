using MediatR;

namespace FSH.Framework.Core.Storage.File.Features;

/// <summary>
/// Command for uploading files with validation and metadata.
/// Implements CQRS pattern by extending IRequest.
/// </summary>
public class FileUploadCommand : IRequest<FileUploadResponse>
{
    /// <summary>
    /// Gets or sets the name of the uploaded file.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the base64-encoded file data.
    /// </summary>
    public string Data { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file extension or MIME type.
    /// </summary>
    public string Extension { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file size in bytes.
    /// </summary>
    public long? Length { get; set; }
    public string? ContentType { get; set; }

    /// <summary>
    /// Gets or sets additional metadata for the file.
    /// </summary>
    public Dictionary<string, string>? Metadata { get; set; }

    /// <summary>
    /// Validates if the file upload command contains valid data.
    /// </summary>
    /// <returns>True if the command is valid; otherwise, false.</returns>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Name) &&
               !string.IsNullOrWhiteSpace(Data) &&
               !string.IsNullOrWhiteSpace(Extension);
    }

    /// <summary>
    /// Gets the estimated file size from the base64 data.
    /// </summary>
    /// <returns>The estimated file size in bytes.</returns>
    public long GetEstimatedSizeFromData()
    {
        if (string.IsNullOrWhiteSpace(Data))
            return 0;

        // Base64 encoding increases size by ~33%, so we calculate the original size
        return (long)(Data.Length * 0.75);
    }
}
