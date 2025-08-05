using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.FixedAsset;

public record FixedAssetCreated(DefaultIdType Id, string AssetName, DateTime PurchaseDate, decimal PurchasePrice, string AssetType, string? Description, string? Notes) : DomainEvent;

public record FixedAssetUpdated(DefaultIdType Id, string AssetName, string AssetType, string? Description, string? Notes) : DomainEvent;

public record FixedAssetMaintenanceUpdated(DefaultIdType Id, string AssetName, DateTime? LastMaintenanceDate, DateTime? NextMaintenanceDate) : DomainEvent;

public record FixedAssetDepreciationAdded(DefaultIdType Id, string AssetName, decimal DepreciationAmount, decimal CurrentBookValue) : DomainEvent;

public record FixedAssetDisposed(DefaultIdType Id, string AssetName, DateTime DisposalDate, decimal? DisposalAmount, string? DisposalReason) : DomainEvent;

public record AssetMaintenanceScheduled(DefaultIdType AssetId, string AssetName, DateTime MaintenanceDate) : DomainEvent;

public record AssetMaintenanceCompleted(DefaultIdType AssetId, string AssetName, DateTime MaintenanceDate) : DomainEvent;
