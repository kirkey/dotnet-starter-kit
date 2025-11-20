using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents an employee document (contract, certification, ID, etc.).
/// Tracks document metadata, expiry dates, and storage information.
/// </summary>
/// <remarks>
/// Key Design Points:
/// - Multiple documents per employee
/// - Document type: Contract, Certification, License, Identity, Medical, Other
/// - Expiry date tracking for renewals
/// - Document storage/file management
/// - Version control for updated documents
/// 
/// Example:
/// - Employee John Doe has:
///   - Employment Contract (signed, never expires)
///   - Drivers License (copy, expires 2027)
///   - Certifications (multiple, expires at different dates)
///   - Medical Certificate (annual, expires 2026)
/// </remarks>
public class EmployeeDocument : AuditableEntity, IAggregateRoot
{
    private EmployeeDocument() { }

    private EmployeeDocument(
        DefaultIdType id,
        DefaultIdType employeeId,
        string documentType,
        string title,
        string? fileName = null,
        string? filePath = null,
        long? fileSize = null,
        DateTime? expiryDate = null,
        string? issueNumber = null,
        DateTime? issueDate = null)
    {
        Id = id;
        EmployeeId = employeeId;
        DocumentType = documentType;
        Title = title;
        FileName = fileName;
        FilePath = filePath;
        FileSize = fileSize;
        ExpiryDate = expiryDate;
        IssueNumber = issueNumber;
        IssueDate = issueDate;
        IsActive = true;
        UploadedDate = DateTime.UtcNow;
        Version = 1;

        QueueDomainEvent(new EmployeeDocumentCreated { Document = this });
    }

    /// <summary>
    /// The employee this document is associated with.
    /// </summary>
    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = default!;

    /// <summary>
    /// Type of document (Contract, Certification, License, Identity, Medical, Other).
    /// </summary>
    public string DocumentType { get; private set; } = default!;

    /// <summary>
    /// Title/name of the document.
    /// </summary>
    public string Title { get; private set; } = default!;

    /// <summary>
    /// Original file name.
    /// </summary>
    public string? FileName { get; private set; }

    /// <summary>
    /// File storage path or reference.
    /// </summary>
    public string? FilePath { get; private set; }

    /// <summary>
    /// File size in bytes.
    /// </summary>
    public long? FileSize { get; private set; }

    /// <summary>
    /// Date the document expires (if applicable).
    /// </summary>
    public DateTime? ExpiryDate { get; private set; }

    /// <summary>
    /// Whether this document has expired.
    /// </summary>
    public bool IsExpired => ExpiryDate.HasValue && ExpiryDate < DateTime.Today;

    /// <summary>
    /// Days until expiry (negative if expired).
    /// </summary>
    public int? DaysUntilExpiry => ExpiryDate.HasValue ? (int)(ExpiryDate.Value.Date - DateTime.Today).TotalDays : null;

    /// <summary>
    /// Issue number (license number, certificate number, etc.).
    /// </summary>
    public string? IssueNumber { get; private set; }

    /// <summary>
    /// Date the document was issued.
    /// </summary>
    public DateTime? IssueDate { get; private set; }

    /// <summary>
    /// Date the document was uploaded to system.
    /// </summary>
    public DateTime UploadedDate { get; private set; }

    /// <summary>
    /// Version number for document tracking.
    /// </summary>
    public int Version { get; private set; }

    /// <summary>
    /// Comments or notes about the document.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Whether this document is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creates a new employee document.
    /// </summary>
    public static EmployeeDocument Create(
        DefaultIdType employeeId,
        string documentType,
        string title,
        string? fileName = null,
        string? filePath = null,
        long? fileSize = null,
        DateTime? expiryDate = null,
        string? issueNumber = null,
        DateTime? issueDate = null)
    {
        var document = new EmployeeDocument(
            DefaultIdType.NewGuid(),
            employeeId,
            documentType,
            title,
            fileName,
            filePath,
            fileSize,
            expiryDate,
            issueNumber,
            issueDate);

        return document;
    }

    /// <summary>
    /// Updates document metadata.
    /// </summary>
    public EmployeeDocument Update(
        string? documentType = null,
        string? title = null,
        DateTime? expiryDate = null,
        string? issueNumber = null,
        DateTime? issueDate = null,
        string? notes = null)
    {
        if (!string.IsNullOrWhiteSpace(documentType))
            DocumentType = documentType;

        if (!string.IsNullOrWhiteSpace(title))
            Title = title;

        if (expiryDate.HasValue)
            ExpiryDate = expiryDate;

        if (!string.IsNullOrWhiteSpace(issueNumber))
            IssueNumber = issueNumber;

        if (issueDate.HasValue)
            IssueDate = issueDate;

        if (notes != null)
            Notes = notes;

        QueueDomainEvent(new EmployeeDocumentUpdated { Document = this });
        return this;
    }

    /// <summary>
    /// Updates the file information when document is replaced.
    /// </summary>
    public EmployeeDocument ReplaceFile(
        string fileName,
        string filePath,
        long fileSize)
    {
        FileName = fileName;
        FilePath = filePath;
        FileSize = fileSize;
        Version++;
        UploadedDate = DateTime.UtcNow;

        QueueDomainEvent(new EmployeeDocumentUpdated { Document = this });
        return this;
    }

    /// <summary>
    /// Adds notes to the document.
    /// </summary>
    public EmployeeDocument AddNotes(string notes)
    {
        Notes = notes;
        QueueDomainEvent(new EmployeeDocumentUpdated { Document = this });
        return this;
    }

    /// <summary>
    /// Deactivates this document.
    /// </summary>
    public EmployeeDocument Deactivate()
    {
        IsActive = false;
        QueueDomainEvent(new EmployeeDocumentDeactivated { DocumentId = Id });
        return this;
    }

    /// <summary>
    /// Activates this document.
    /// </summary>
    public EmployeeDocument Activate()
    {
        IsActive = true;
        QueueDomainEvent(new EmployeeDocumentActivated { DocumentId = Id });
        return this;
    }
}

/// <summary>
/// Document type constants.
/// </summary>
public static class DocumentType
{
    public const string Contract = "Contract";
    public const string Certification = "Certification";
    public const string License = "License";
    public const string Identity = "Identity";
    public const string Medical = "Medical";
    public const string Other = "Other";
}

