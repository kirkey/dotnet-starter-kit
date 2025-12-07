using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a document stored for a member, loan, or other entity.
/// </summary>
public sealed class Document : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int NameMaxLength = 256;
    public const int TypeMaxLength = 64;
    public const int CategoryMaxLength = 64;
    public const int StatusMaxLength = 32;
    public const int FilePathMaxLength = 1024;
    public const int MimeTypeMaxLength = 128;
    public const int DescriptionMaxLength = 512;
    
    // Document Status
    public const string StatusActive = "Active";
    public const string StatusArchived = "Archived";
    public const string StatusDeleted = "Deleted";
    public const string StatusExpired = "Expired";
    
    // Document Types
    public const string TypeIdentification = "Identification";
    public const string TypeProofOfAddress = "ProofOfAddress";
    public const string TypeIncomeProof = "IncomeProof";
    public const string TypeCollateral = "Collateral";
    public const string TypeContract = "Contract";
    public const string TypePhoto = "Photo";
    public const string TypeSignature = "Signature";
    public const string TypeStatement = "Statement";
    public const string TypeOther = "Other";
    
    // Entity Types
    public const string EntityMember = "Member";
    public const string EntityLoan = "Loan";
    public const string EntityCollateral = "Collateral";
    public const string EntityGuarantor = "Guarantor";
    public const string EntityGroup = "Group";

    public string DocumentType { get; private set; } = default!;
    public string? Category { get; private set; }
    public string Status { get; private set; } = StatusActive;
    public string EntityType { get; private set; } = default!;
    public Guid EntityId { get; private set; }
    public string FilePath { get; private set; } = default!;
    public string? MimeType { get; private set; }
    public long FileSizeBytes { get; private set; }
    public string? OriginalFileName { get; private set; }
    public DateOnly? IssueDate { get; private set; }
    public DateOnly? ExpiryDate { get; private set; }
    public string? IssuingAuthority { get; private set; }
    public string? DocumentNumber { get; private set; }
    public bool IsVerified { get; private set; }
    public Guid? VerifiedById { get; private set; }
    public DateTimeOffset? VerifiedAt { get; private set; }
    public bool IsRequired { get; private set; }
    public int DisplayOrder { get; private set; }
    public string? Tags { get; private set; }

    private Document() { }

    public static Document Create(
        string name,
        string documentType,
        string entityType,
        Guid entityId,
        string filePath,
        long fileSizeBytes,
        string? mimeType = null,
        string? category = null,
        string? description = null,
        string? originalFileName = null)
    {
        var document = new Document
        {
            Name = name,
            DocumentType = documentType,
            EntityType = entityType,
            EntityId = entityId,
            FilePath = filePath,
            FileSizeBytes = fileSizeBytes,
            MimeType = mimeType,
            Category = category,
            Description = description,
            OriginalFileName = originalFileName,
            Status = StatusActive
        };

        document.QueueDomainEvent(new DocumentCreated(document));
        return document;
    }

    public Document SetDocumentDetails(
        string? documentNumber = null,
        DateOnly? issueDate = null,
        DateOnly? expiryDate = null,
        string? issuingAuthority = null)
    {
        DocumentNumber = documentNumber;
        IssueDate = issueDate;
        ExpiryDate = expiryDate;
        IssuingAuthority = issuingAuthority;
        return this;
    }

    public Document Verify(Guid verifiedById)
    {
        IsVerified = true;
        VerifiedById = verifiedById;
        VerifiedAt = DateTimeOffset.UtcNow;
        QueueDomainEvent(new DocumentVerified(Id, EntityType, EntityId));
        return this;
    }

    public Document Archive()
    {
        Status = StatusArchived;
        return this;
    }

    public Document MarkExpired()
    {
        Status = StatusExpired;
        QueueDomainEvent(new DocumentExpired(Id, EntityType, EntityId));
        return this;
    }

    public Document Delete()
    {
        Status = StatusDeleted;
        return this;
    }

    public Document Update(
        string? name = null,
        string? description = null,
        string? tags = null,
        int? displayOrder = null)
    {
        if (name is not null) Name = name;
        if (description is not null) Description = description;
        if (tags is not null) Tags = tags;
        if (displayOrder.HasValue) DisplayOrder = displayOrder.Value;

        QueueDomainEvent(new DocumentUpdated(this));
        return this;
    }
}
