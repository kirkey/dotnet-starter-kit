using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Domain constants for KycDocument entity.
/// </summary>
public static class KycDocumentConstants
{
    /// <summary>Maximum length for document type. (2^5 = 32)</summary>
    public const int DocumentTypeMaxLength = 32;

    /// <summary>Maximum length for document number. (2^6 = 64)</summary>
    public const int DocumentNumberMaxLength = 64;

    /// <summary>Maximum length for file name. (2^8 = 256)</summary>
    public const int FileNameMaxLength = 256;

    /// <summary>Maximum length for file path. (2^10 = 1024)</summary>
    public const int FilePathMaxLength = 1024;

    /// <summary>Maximum length for mime type. (2^7 = 128)</summary>
    public const int MimeTypeMaxLength = 128;

    /// <summary>Maximum length for status. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for notes. (2^11 = 2048)</summary>
    public const int NotesMaxLength = 2048;
}

/// <summary>
/// Represents a KYC (Know Your Customer) document for a member.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Store member identification documents (ID, passport, etc.)</description></item>
///   <item><description>Track document verification status</description></item>
///   <item><description>Manage document expiry and renewal reminders</description></item>
///   <item><description>Support regulatory compliance for AML/KYC requirements</description></item>
///   <item><description>Maintain document version history</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// KYC documents are required by financial regulations to verify member identity.
/// Each document goes through a verification process before being accepted.
/// Document expiry is monitored to ensure ongoing compliance.
/// </para>
/// </remarks>
public class KycDocument : AuditableEntity, IAggregateRoot
{
    // Document Types
    /// <summary>National ID card.</summary>
    public const string TypeNationalId = "NATIONAL_ID";
    /// <summary>Passport.</summary>
    public const string TypePassport = "PASSPORT";
    /// <summary>Driver's license.</summary>
    public const string TypeDriversLicense = "DRIVERS_LICENSE";
    /// <summary>Voter's ID.</summary>
    public const string TypeVoterId = "VOTER_ID";
    /// <summary>Utility bill (proof of address).</summary>
    public const string TypeUtilityBill = "UTILITY_BILL";
    /// <summary>Bank statement.</summary>
    public const string TypeBankStatement = "BANK_STATEMENT";
    /// <summary>Tax ID/PIN certificate.</summary>
    public const string TypeTaxId = "TAX_ID";
    /// <summary>Business registration.</summary>
    public const string TypeBusinessReg = "BUSINESS_REG";
    /// <summary>Photograph.</summary>
    public const string TypePhoto = "PHOTO";
    /// <summary>Signature specimen.</summary>
    public const string TypeSignature = "SIGNATURE";
    /// <summary>Other document.</summary>
    public const string TypeOther = "OTHER";

    // Verification Statuses
    /// <summary>Document pending verification.</summary>
    public const string StatusPending = "PENDING";
    /// <summary>Document verified and accepted.</summary>
    public const string StatusVerified = "VERIFIED";
    /// <summary>Document rejected.</summary>
    public const string StatusRejected = "REJECTED";
    /// <summary>Document expired.</summary>
    public const string StatusExpired = "EXPIRED";
    /// <summary>Document requires re-verification.</summary>
    public const string StatusReVerification = "RE_VERIFICATION";

    /// <summary>Gets the member ID.</summary>
    public DefaultIdType MemberId { get; private set; }

    /// <summary>Gets the member navigation property.</summary>
    public virtual Member? Member { get; private set; }

    /// <summary>Gets the document type.</summary>
    public string DocumentType { get; private set; } = default!;

    /// <summary>Gets the document number (e.g., ID number).</summary>
    public string? DocumentNumber { get; private set; }

    /// <summary>Gets the original file name.</summary>
    public string FileName { get; private set; } = default!;

    /// <summary>Gets the file path/storage location.</summary>
    public string FilePath { get; private set; } = default!;

    /// <summary>Gets the MIME type.</summary>
    public string MimeType { get; private set; } = default!;

    /// <summary>Gets the file size in bytes.</summary>
    public long FileSize { get; private set; }

    /// <summary>Gets the document issue date.</summary>
    public DateOnly? IssueDate { get; private set; }

    /// <summary>Gets the document expiry date.</summary>
    public DateOnly? ExpiryDate { get; private set; }

    /// <summary>Gets the issuing authority.</summary>
    public string? IssuingAuthority { get; private set; }

    /// <summary>Gets the verification status.</summary>
    public string Status { get; private set; } = default!;

    /// <summary>Gets the date the document was verified.</summary>
    public DateTime? VerifiedAt { get; private set; }

    /// <summary>Gets the user ID who verified the document.</summary>
    public DefaultIdType? VerifiedById { get; private set; }

    /// <summary>Gets the rejection reason if rejected.</summary>
    public string? RejectionReason { get; private set; }

    /// <summary>Gets whether this is the primary document of its type.</summary>
    public bool IsPrimary { get; private set; }

    private KycDocument() { }

    private KycDocument(
        DefaultIdType id,
        DefaultIdType memberId,
        string documentType,
        string fileName,
        string filePath,
        string mimeType,
        long fileSize)
    {
        Id = id;
        MemberId = memberId;
        DocumentType = documentType;
        FileName = fileName.Trim();
        FilePath = filePath.Trim();
        MimeType = mimeType.Trim();
        FileSize = fileSize;
        Status = StatusPending;
        IsPrimary = false;

        QueueDomainEvent(new KycDocumentUploaded { KycDocument = this });
    }

    /// <summary>Creates a new KycDocument.</summary>
    public static KycDocument Create(
        DefaultIdType memberId,
        string documentType,
        string fileName,
        string filePath,
        string mimeType,
        long fileSize)
    {
        return new KycDocument(
            DefaultIdType.NewGuid(),
            memberId,
            documentType,
            fileName,
            filePath,
            mimeType,
            fileSize);
    }

    /// <summary>Sets the document number and details.</summary>
    public KycDocument WithDocumentDetails(
        string? documentNumber,
        DateOnly? issueDate,
        DateOnly? expiryDate,
        string? issuingAuthority)
    {
        DocumentNumber = documentNumber?.Trim();
        IssueDate = issueDate;
        ExpiryDate = expiryDate;
        IssuingAuthority = issuingAuthority?.Trim();
        return this;
    }

    /// <summary>Marks as primary document.</summary>
    public KycDocument SetAsPrimary()
    {
        IsPrimary = true;
        return this;
    }

    /// <summary>Verifies the document.</summary>
    public KycDocument Verify(DefaultIdType verifiedById, string? notes = null)
    {
        if (Status != StatusPending && Status != StatusReVerification)
            throw new InvalidOperationException($"Cannot verify document in {Status} status.");

        Status = StatusVerified;
        VerifiedAt = DateTime.UtcNow;
        VerifiedById = verifiedById;
        if (!string.IsNullOrWhiteSpace(notes))
        {
            Notes = notes.Trim();
        }

        QueueDomainEvent(new KycDocumentVerified { DocumentId = Id, MemberId = MemberId });
        return this;
    }

    /// <summary>Rejects the document.</summary>
    public KycDocument Reject(DefaultIdType rejectedById, string reason)
    {
        if (Status != StatusPending && Status != StatusReVerification)
            throw new InvalidOperationException($"Cannot reject document in {Status} status.");

        Status = StatusRejected;
        VerifiedAt = DateTime.UtcNow;
        VerifiedById = rejectedById;
        RejectionReason = reason?.Trim();

        QueueDomainEvent(new KycDocumentRejected { DocumentId = Id, MemberId = MemberId, Reason = reason });
        return this;
    }

    /// <summary>Marks document as expired.</summary>
    public KycDocument MarkAsExpired()
    {
        Status = StatusExpired;
        QueueDomainEvent(new KycDocumentExpired { DocumentId = Id, MemberId = MemberId });
        return this;
    }

    /// <summary>Requests re-verification.</summary>
    public KycDocument RequestReVerification(string reason)
    {
        Status = StatusReVerification;
        Notes = $"Re-verification requested: {reason}";
        return this;
    }

    /// <summary>Checks if document is expired.</summary>
    public bool IsExpired()
    {
        if (!ExpiryDate.HasValue) return false;
        return ExpiryDate.Value < DateOnly.FromDateTime(DateTime.UtcNow);
    }

    /// <summary>Gets days until expiry.</summary>
    public int? DaysUntilExpiry()
    {
        if (!ExpiryDate.HasValue) return null;
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return ExpiryDate.Value.DayNumber - today.DayNumber;
    }
}
