namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Get.v1;

public sealed record EmployeeDocumentResponse
{
    public DefaultIdType Id { get; init; }
    public DefaultIdType EmployeeId { get; init; }
    public string DocumentType { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string? FileName { get; init; }
    public string? FilePath { get; init; }
    public long? FileSize { get; init; }
    public DateTime? ExpiryDate { get; init; }
    public bool IsExpired { get; init; }
    public int? DaysUntilExpiry { get; init; }
    public string? IssueNumber { get; init; }
    public DateTime? IssueDate { get; init; }
    public DateTime UploadedDate { get; init; }
    public int Version { get; init; }
    public string? Notes { get; init; }
    public bool IsActive { get; init; }
}

