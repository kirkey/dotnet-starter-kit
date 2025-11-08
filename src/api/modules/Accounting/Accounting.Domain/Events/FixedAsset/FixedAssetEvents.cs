namespace Accounting.Domain.Events.FixedAsset;

public record FixedAssetCreated(DefaultIdType Id, string AssetName, DateTime PurchaseDate, decimal PurchasePrice, string AssetType, string? Description, string? Notes) : DomainEvent;

public record FixedAssetUpdated(DefaultIdType Id, string AssetName, string AssetType, string? Description, string? Notes) : DomainEvent;

public record FixedAssetDeleted(DefaultIdType Id) : DomainEvent;

public record FixedAssetMaintenanceUpdated(DefaultIdType Id, string AssetName, DateTime? LastMaintenanceDate, DateTime? NextMaintenanceDate) : DomainEvent;

public record FixedAssetDepreciationAdded(DefaultIdType Id, string AssetName, decimal DepreciationAmount, decimal CurrentBookValue) : DomainEvent;

public record FixedAssetDisposed(DefaultIdType Id, string AssetName, DateTime DisposalDate, decimal? DisposalAmount, string? DisposalReason) : DomainEvent;

public record AssetMaintenanceScheduled(DefaultIdType AssetId, string AssetName, DateTime MaintenanceDate) : DomainEvent;

public record AssetMaintenanceCompleted(DefaultIdType AssetId, string AssetName, DateTime MaintenanceDate) : DomainEvent;

public record FixedAssetApproved(
    DefaultIdType Id,
    string AssetName,
    string ApprovedBy,
    DateTime ApprovedDate) : DomainEvent;

public record FixedAssetRejected(
    DefaultIdType Id,
    string AssetName,
    string RejectedBy,
    DateTime RejectedDate,
    string? Reason) : DomainEvent;

public record FixedAssetTransferred(
    DefaultIdType Id,
    string AssetName,
    string FromLocation,
    string ToLocation,
    DateTime TransferDate) : DomainEvent;

public record FixedAssetRevalued(
    DefaultIdType Id,
    string AssetName,
    decimal OldValue,
    decimal NewValue,
    DateTime RevaluationDate,
    string? Reason) : DomainEvent;

