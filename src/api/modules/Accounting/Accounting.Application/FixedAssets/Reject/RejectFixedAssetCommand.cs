namespace Accounting.Application.FixedAssets.Reject;

/// <summary>
/// Command to reject a fixed asset acquisition.
/// </summary>
public sealed record RejectFixedAssetCommand(
    DefaultIdType FixedAssetId,
    string RejectedBy,
    string? Reason
) : IRequest<DefaultIdType>;
