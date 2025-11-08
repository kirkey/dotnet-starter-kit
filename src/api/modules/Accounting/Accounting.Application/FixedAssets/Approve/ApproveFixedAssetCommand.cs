namespace Accounting.Application.FixedAssets.Approve;

/// <summary>
/// Command to approve a fixed asset acquisition.
/// </summary>
public sealed record ApproveFixedAssetCommand(
    DefaultIdType FixedAssetId,
    string ApprovedBy
) : IRequest<DefaultIdType>;
