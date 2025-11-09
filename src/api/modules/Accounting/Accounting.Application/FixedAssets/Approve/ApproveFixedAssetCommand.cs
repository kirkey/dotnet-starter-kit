namespace Accounting.Application.FixedAssets.Approve;

/// <summary>
/// Command to approve a fixed asset acquisition.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed record ApproveFixedAssetCommand(
    DefaultIdType FixedAssetId
) : IRequest<DefaultIdType>;
