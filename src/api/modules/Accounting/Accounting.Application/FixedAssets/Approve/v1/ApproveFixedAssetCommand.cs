namespace Accounting.Application.FixedAssets.Approve.v1;

/// <summary>
/// Command to approve a fixed asset record.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed record ApproveFixedAssetCommand(
    DefaultIdType FixedAssetId
) : IRequest<DefaultIdType>;

