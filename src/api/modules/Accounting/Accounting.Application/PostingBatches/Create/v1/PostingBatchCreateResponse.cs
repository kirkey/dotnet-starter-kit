namespace Accounting.Application.PostingBatches.Create.v1;

/// <summary>
/// Response returned after successfully creating a posting batch.
/// </summary>
public sealed record PostingBatchCreateResponse
{
    public DefaultIdType Id { get; init; }
    public string BatchNumber { get; init; } = string.Empty;
    public DateTime BatchDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string ApprovalStatus { get; init; } = string.Empty;
}

