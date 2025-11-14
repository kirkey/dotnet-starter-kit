using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents a document generated from a template for a specific entity.
/// Tracks generated documents, signatures, and versions.
/// </summary>
public class GeneratedDocument : AuditableEntity, IAggregateRoot
{
    private GeneratedDocument() { }

    private GeneratedDocument(
        DefaultIdType id,
        DefaultIdType documentTemplateId,
        DefaultIdType entityId,
        string entityType,
        string generatedContent)
    {
        Id = id;
        DocumentTemplateId = documentTemplateId;
        EntityId = entityId;
        EntityType = entityType;
        GeneratedContent = generatedContent;
        Status = "Draft";
        IsActive = true;
        GeneratedDate = DateTime.UtcNow;
        Version = 1;

        QueueDomainEvent(new DocumentGenerated { Document = this });
    }

    /// <summary>
    /// The template used to generate this document.
    /// </summary>
    public DefaultIdType DocumentTemplateId { get; private set; }
    public DocumentTemplate DocumentTemplate { get; private set; } = default!;

    /// <summary>
    /// ID of the entity this document is associated with (Employee, Payroll, etc).
    /// </summary>
    public DefaultIdType EntityId { get; private set; }

    /// <summary>
    /// Type of entity (Employee, Payroll, LeaveRequest, etc).
    /// </summary>
    public string EntityType { get; private set; } = default!;

    /// <summary>
    /// The generated document content.
    /// </summary>
    public string GeneratedContent { get; private set; } = default!;

    /// <summary>
    /// Document status (Draft, Finalized, Signed, Archived).
    /// </summary>
    public string Status { get; private set; } = default!;

    /// <summary>
    /// Date the document was generated.
    /// </summary>
    public DateTime GeneratedDate { get; private set; }

    /// <summary>
    /// Date the document was finalized.
    /// </summary>
    public DateTime? FinalizedDate { get; private set; }

    /// <summary>
    /// Date the document was signed.
    /// </summary>
    public DateTime? SignedDate { get; private set; }

    /// <summary>
    /// Who signed the document (name/ID).
    /// </summary>
    public string? SignedBy { get; private set; }

    /// <summary>
    /// Signature metadata (timestamp, location, device info).
    /// </summary>
    public string? SignatureMetadata { get; private set; }

    /// <summary>
    /// File path where document is stored.
    /// </summary>
    public string? FilePath { get; private set; }

    /// <summary>
    /// Document version for change tracking.
    /// </summary>
    public int Version { get; private set; }

    /// <summary>
    /// Whether this document is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Comments or notes about the document.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Creates a new generated document.
    /// </summary>
    public static GeneratedDocument Create(
        DefaultIdType documentTemplateId,
        DefaultIdType entityId,
        string entityType,
        string generatedContent)
    {
        if (string.IsNullOrWhiteSpace(generatedContent))
            throw new ArgumentException("Generated content is required.", nameof(generatedContent));

        if (string.IsNullOrWhiteSpace(entityType))
            throw new ArgumentException("Entity type is required.", nameof(entityType));

        var document = new GeneratedDocument(
            DefaultIdType.NewGuid(),
            documentTemplateId,
            entityId,
            entityType,
            generatedContent);

        return document;
    }

    /// <summary>
    /// Updates the document content.
    /// </summary>
    public GeneratedDocument UpdateContent(string generatedContent)
    {
        if (Status != "Draft")
            throw new InvalidOperationException("Can only update draft documents.");

        if (string.IsNullOrWhiteSpace(generatedContent))
            throw new ArgumentException("Generated content is required.", nameof(generatedContent));

        GeneratedContent = generatedContent;
        Version++;

        return this;
    }

    /// <summary>
    /// Finalizes the document (locks for changes).
    /// </summary>
    public GeneratedDocument Finalize()
    {
        if (Status != "Draft")
            throw new InvalidOperationException("Only draft documents can be finalized.");

        Status = "Finalized";
        FinalizedDate = DateTime.UtcNow;

        QueueDomainEvent(new DocumentFinalized { Document = this });
        return this;
    }

    /// <summary>
    /// Records document signature.
    /// </summary>
    public GeneratedDocument RecordSignature(string signedBy, string? signatureMetadata = null)
    {
        if (Status != "Finalized")
            throw new InvalidOperationException("Document must be finalized before signing.");

        if (string.IsNullOrWhiteSpace(signedBy))
            throw new ArgumentException("Signed by is required.", nameof(signedBy));

        Status = "Signed";
        SignedDate = DateTime.UtcNow;
        SignedBy = signedBy;
        SignatureMetadata = signatureMetadata;

        QueueDomainEvent(new DocumentSigned { Document = this });
        return this;
    }

    /// <summary>
    /// Archives the document.
    /// </summary>
    public GeneratedDocument Archive()
    {
        Status = "Archived";
        QueueDomainEvent(new DocumentArchived { Document = this });
        return this;
    }

    /// <summary>
    /// Sets the file storage path.
    /// </summary>
    public GeneratedDocument SetFilePath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path is required.", nameof(filePath));

        FilePath = filePath;
        return this;
    }

    /// <summary>
    /// Adds notes to the document.
    /// </summary>
    public GeneratedDocument AddNotes(string notes)
    {
        if (!string.IsNullOrWhiteSpace(notes))
            Notes = notes;

        return this;
    }

    /// <summary>
    /// Deactivates the document.
    /// </summary>
    public GeneratedDocument Deactivate()
    {
        IsActive = false;
        return this;
    }

    /// <summary>
    /// Activates the document.
    /// </summary>
    public GeneratedDocument Activate()
    {
        IsActive = true;
        return this;
    }
}

/// <summary>
/// Document status constants.
/// </summary>
public static class GeneratedDocumentStatus
{
    public const string Draft = "Draft";
    public const string Finalized = "Finalized";
    public const string Signed = "Signed";
    public const string Archived = "Archived";
}

/// <summary>
/// Entity type constants for generated documents.
/// </summary>
public static class DocumentEntityType
{
    public const string Employee = "Employee";
    public const string Payroll = "Payroll";
    public const string LeaveRequest = "LeaveRequest";
    public const string BenefitEnrollment = "BenefitEnrollment";
    public const string EmployeeDocument = "EmployeeDocument";
    public const string Timesheet = "Timesheet";
}

