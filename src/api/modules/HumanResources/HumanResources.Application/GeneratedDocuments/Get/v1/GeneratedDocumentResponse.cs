namespace FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Get.v1;

public sealed record GeneratedDocumentResponse
{
    public DefaultIdType Id { get; init; }
    public DefaultIdType DocumentTemplateId { get; init; }
    public DefaultIdType EntityId { get; init; }
    public string EntityType { get; init; } = default!;
    public string GeneratedContent { get; init; } = default!;
    public string Status { get; init; } = default!;
    public DateTime GeneratedDate { get; init; }
    public DateTime? FinalizedDate { get; init; }
    public DateTime? SignedDate { get; init; }
    public string? SignedBy { get; init; }
    public string? FilePath { get; init; }
    public int Version { get; init; }
    public bool IsActive { get; init; }
    public string? Notes { get; init; }
}

