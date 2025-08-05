namespace FSH.Framework.Core.Extensions.Dto;

public class BaseRequest : BaseRequest<DefaultIdType>;

public abstract class BaseRequest<TId>
{
    public TId? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Remarks { get; set; }
    public string? Status { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    // public bool DeleteCurrentFile { get; set; }
    // public FileUploadRequest? File { get; set; }
}

public abstract class BaseRequestWithApproval : BaseRequestWithApproval<DefaultIdType>;

public abstract class BaseRequestWithApproval<TId> : BaseRequest<TId>
{
    public string? Request { get; set; }
    public string? Feedback { get; set; }

    public DefaultIdType PreparedBy { get; set; } = DefaultIdType.Empty;
    public string? PreparerName { get; set; }
    public DateTime? PreparedOn { get; set; }

    public DefaultIdType ReviewedBy { get; set; } = DefaultIdType.Empty;
    public string? ReviewerName { get; set; }
    public DateTime? ReviewedOn { get; set; }

    public DefaultIdType RecommendedBy { get; set; } = DefaultIdType.Empty;
    public string? RecommenderName { get; set; }
    public DateTime? RecommendedOn { get; set; }

    public DefaultIdType ApprovedBy { get; set; } = DefaultIdType.Empty;
    public string? ApproverName { get; set; }
    public DateTime? ApprovedOn { get; set; }
}
