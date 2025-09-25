namespace FSH.Framework.Core.Storage.File.Features;

/// <summary>
/// Response model for file upload operations.
/// Contains the result of a file upload request including the file URL and metadata.
/// </summary>
public class FileUploadResponse
{
    /// <summary>
    /// Gets or sets the URL of the uploaded file.
    /// </summary>
    public string FileUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier for the uploaded file.
    /// </summary>
    public string FileId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the original file name.
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file size in bytes.
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Gets or sets the MIME type of the uploaded file.
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp when the file was uploaded.
    /// </summary>
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets additional metadata associated with the uploaded file.
    /// </summary>
    public Dictionary<string, string>? Metadata { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the upload was successful.
    /// </summary>
    public bool IsSuccess { get; set; } = true;

    /// <summary>
    /// Gets or sets the error message if the upload failed.
    /// </summary>
    public string? ErrorMessage { get; set; }
}
