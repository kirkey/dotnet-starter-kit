namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.KycDocuments;

/// <summary>
/// ViewModel used by the KycDocuments page for add/edit operations.
/// Mirrors the shape of the API's CreateKycDocumentCommand so Mapster/Adapt can map between them.
/// </summary>
public class KycDocumentViewModel
{
    /// <summary>
    /// Primary identifier of the KYC document.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// The member this document belongs to. Required.
    /// </summary>
    public DefaultIdType MemberId { get; set; }

    /// <summary>
    /// Selected member response for autocomplete binding.
    /// </summary>
    public MemberResponse? SelectedMember { get; set; }

    /// <summary>
    /// Type of document. Required.
    /// Values: NationalId, Passport, DriverLicense, ProofOfAddress, Photo
    /// </summary>
    public string DocumentType { get; set; } = "NationalId";

    /// <summary>
    /// Document number (e.g., passport number, ID number). Required.
    /// </summary>
    public string? DocumentNumber { get; set; }

    /// <summary>
    /// Authority that issued the document.
    /// </summary>
    public string? IssuingAuthority { get; set; }

    /// <summary>
    /// Date when the document was issued.
    /// </summary>
    public DateTimeOffset? IssueDate { get; set; }

    /// <summary>
    /// DateTime wrapper for IssueDate to work with MudDatePicker.
    /// </summary>
    public DateTime? IssueDateValue
    {
        get => IssueDate?.LocalDateTime;
        set => IssueDate = value.HasValue ? new DateTimeOffset(value.Value) : null;
    }

    /// <summary>
    /// Date when the document expires.
    /// </summary>
    public DateTimeOffset? ExpiryDate { get; set; }

    /// <summary>
    /// DateTime wrapper for ExpiryDate to work with MudDatePicker.
    /// </summary>
    public DateTime? ExpiryDateValue
    {
        get => ExpiryDate?.LocalDateTime;
        set => ExpiryDate = value.HasValue ? new DateTimeOffset(value.Value) : null;
    }

    /// <summary>
    /// Name of the uploaded file.
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Path or URL to the document file.
    /// </summary>
    public string? FilePath { get; set; }

    /// <summary>
    /// MIME type of the uploaded file (e.g., application/pdf, image/jpeg).
    /// </summary>
    public string? MimeType { get; set; }

    /// <summary>
    /// Size of the uploaded file in bytes.
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Current status of the document.
    /// Values: Pending, Verified, Rejected, Expired
    /// Read-only - set by workflow actions.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Reason for rejection, if rejected.
    /// </summary>
    public string? RejectionReason { get; set; }

    /// <summary>
    /// Sync IDs from selected autocomplete values.
    /// </summary>
    public void SyncIdsFromSelections()
    {
        if (SelectedMember != null)
            MemberId = SelectedMember.Id;
    }
}
