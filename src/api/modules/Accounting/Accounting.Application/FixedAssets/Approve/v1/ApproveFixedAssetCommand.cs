namespace Accounting.Application.FixedAssets.Approve.v1;

/// <summary>
/// Command to approve a fixed asset record.
/// </summary>
public sealed record ApproveFixedAssetCommand(
    DefaultIdType FixedAssetId,
    string ApprovedBy
) : IRequest<DefaultIdType>;

