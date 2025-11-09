namespace Accounting.Application.FixedAssets.Reject.v1;

/// <summary>
/// Command to reject a fixed asset record.
/// </summary>
public sealed record RejectFixedAssetCommand(
    DefaultIdType FixedAssetId,
    string RejectedBy,
    string? Reason
) : IRequest<DefaultIdType>;

