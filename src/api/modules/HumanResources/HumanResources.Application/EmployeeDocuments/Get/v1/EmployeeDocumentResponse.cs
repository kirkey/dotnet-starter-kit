namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Get.v1;

/// <summary>
/// Response object for EmployeeDocument entity details.
/// </summary>
public sealed record EmployeeDocumentResponse
{
    /// <summary>
    /// Gets the unique identifier of the document.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Gets the employee identifier.
    /// </summary>
    public DefaultIdType EmployeeId { get; init; }

    /// <summary>
    /// Gets the type of document (Contract, Certification, License, Identity, Medical, Other).
    /// </summary>
    public string DocumentType { get; init; } = default!;

    /// <summary>
    /// Gets the title of the document.
    /// </summary>
    public string Title { get; init; } = default!;

    /// <summary>
    /// Gets the file name.
    /// </summary>
    public string? FileName { get; init; }

    /// <summary>
    /// Gets the file path or reference.
    /// </summary>
    public string? FilePath { get; init; }

    /// <summary>
    /// Gets the file size in bytes.
    /// </summary>
    public long? FileSize { get; init; }

    /// <summary>
    /// Gets the expiry date.
    /// </summary>
    public DateTime? ExpiryDate { get; init; }

    /// <summary>
    /// Gets a value indicating whether the document is expired.
    /// </summary>
    public bool IsExpired { get; init; }

    /// <summary>
    /// Gets days until expiry.
    /// </summary>
    public int? DaysUntilExpiry { get; init; }

    /// <summary>
    /// Gets the issue number.
    /// </summary>
    public string? IssueNumber { get; init; }

    /// <summary>
    /// Gets the issue date.
    /// </summary>
    public DateTime? IssueDate { get; init; }

    /// <summary>
    /// Gets the uploaded date.
    /// </summary>
    public DateTime UploadedDate { get; init; }

    /// <summary>
    /// Gets the version number.
    /// </summary>
    public int Version { get; init; }

    /// <summary>
    /// Gets the notes or comments.
    /// </summary>
    public string? Notes { get; init; }

    /// <summary>
    /// Gets a value indicating whether the document is active.
    /// </summary>
    public bool IsActive { get; init; }
}

