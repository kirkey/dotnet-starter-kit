using System.ComponentModel.DataAnnotations.Schema;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public class StoreTransfer : AuditableEntity, IAggregateRoot
{
    public string TransferNumber { get; private set; } = string.Empty;
    public DateTime TransferDate { get; private set; }
    public DateTime? ReceivedDate { get; private set; }
    public TransferStatus Status { get; private set; } = TransferStatus.Pending;
    public string NotesText { get; private set; } = string.Empty;
    public string CreatedByName { get; private set; } = string.Empty;
    public string? ReceivedBy { get; private set; }

    public DefaultIdType FromWarehouseId { get; private set; }
    public Warehouse FromWarehouse { get; private set; } = default!;
    public DefaultIdType ToStoreId { get; private set; }
    public Store ToStore { get; private set; } = default!;

    public ICollection<StoreTransferDetail> StoreTransferDetails { get; private set; } = new List<StoreTransferDetail>();

    private StoreTransfer() { }

    public static StoreTransfer Create(string transferNumber, DateTime transferDate, string? notes, string createdByName, DefaultIdType fromWarehouseId, DefaultIdType toStoreId)
    {
        return new StoreTransfer
        {
            TransferNumber = transferNumber,
            TransferDate = transferDate,
            NotesText = notes ?? string.Empty,
            CreatedByName = createdByName,
            FromWarehouseId = fromWarehouseId,
            ToStoreId = toStoreId
        };
    }

    public StoreTransfer Update(string? transferNumber, DateTime? transferDate, DateTime? receivedDate, TransferStatus? status, string? notes, string? receivedBy)
    {
        if (!string.IsNullOrWhiteSpace(transferNumber)) TransferNumber = transferNumber;
        if (transferDate.HasValue) TransferDate = transferDate.Value;
        if (receivedDate.HasValue) ReceivedDate = receivedDate.Value;
        if (status.HasValue) Status = status.Value;
        if (notes is not null) NotesText = notes;
        if (receivedBy is not null) ReceivedBy = receivedBy;
        return this;
    }
}

public class StoreTransferDetail : AuditableEntity
{
    public int RequestedQuantity { get; private set; }
    public int TransferredQuantity { get; private set; }
    public int ReceivedQuantity { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitCost { get; private set; }

    public DefaultIdType StoreTransferId { get; private set; }
    public StoreTransfer StoreTransfer { get; private set; } = default!;
    public DefaultIdType ProductId { get; private set; }
    public Product Product { get; private set; } = default!;
    public DefaultIdType? ProductBatchId { get; private set; }
    public ProductBatch? ProductBatch { get; private set; }

    private StoreTransferDetail() { }

    public static StoreTransferDetail Create(DefaultIdType storeTransferId, DefaultIdType productId, int requestedQuantity, decimal unitCost, DefaultIdType? productBatchId)
    {
        return new StoreTransferDetail
        {
            StoreTransferId = storeTransferId,
            ProductId = productId,
            RequestedQuantity = requestedQuantity,
            TransferredQuantity = 0,
            ReceivedQuantity = 0,
            UnitCost = unitCost,
            ProductBatchId = productBatchId
        };
    }

    public StoreTransferDetail Update(int? requestedQuantity, int? transferredQuantity, int? receivedQuantity, decimal? unitCost)
    {
        if (requestedQuantity.HasValue) RequestedQuantity = requestedQuantity.Value;
        if (transferredQuantity.HasValue) TransferredQuantity = transferredQuantity.Value;
        if (receivedQuantity.HasValue) ReceivedQuantity = receivedQuantity.Value;
        if (unitCost.HasValue) UnitCost = unitCost.Value;
        return this;
    }
}

