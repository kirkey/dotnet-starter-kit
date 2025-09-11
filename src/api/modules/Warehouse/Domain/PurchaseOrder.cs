using System.ComponentModel.DataAnnotations.Schema;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public class PurchaseOrder : AuditableEntity, IAggregateRoot
{
    public string OrderNumber { get; private set; } = string.Empty;
    public DateTime OrderDate { get; private set; }
    public DateTime ExpectedDeliveryDate { get; private set; }
    public DateTime? ActualDeliveryDate { get; private set; }
    public PurchaseOrderStatus Status { get; private set; } = PurchaseOrderStatus.Draft;
    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; private set; }
    public string NotesText { get; private set; } = string.Empty;
    public string CreatedByName { get; private set; } = string.Empty;

    public DefaultIdType SupplierId { get; private set; }
    public Supplier Supplier { get; private set; } = default!;
    public DefaultIdType WarehouseId { get; private set; }
    public Warehouse Warehouse { get; private set; } = default!;

    public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; private set; } = new List<PurchaseOrderDetail>();

    private PurchaseOrder() { }

    public static PurchaseOrder Create(string orderNumber, DateTime orderDate, DateTime expectedDeliveryDate, decimal subTotal, decimal taxAmount, decimal totalAmount, string? notes, string createdByName, DefaultIdType supplierId, DefaultIdType warehouseId)
    {
        return new PurchaseOrder
        {
            OrderNumber = orderNumber,
            OrderDate = orderDate,
            ExpectedDeliveryDate = expectedDeliveryDate,
            SubTotal = subTotal,
            TaxAmount = taxAmount,
            TotalAmount = totalAmount,
            NotesText = notes ?? string.Empty,
            CreatedByName = createdByName,
            SupplierId = supplierId,
            WarehouseId = warehouseId
        };
    }

    public PurchaseOrder Update(string? orderNumber, DateTime? orderDate, DateTime? expectedDeliveryDate, DateTime? actualDeliveryDate, PurchaseOrderStatus? status, decimal? subTotal, decimal? taxAmount, decimal? totalAmount, string? notes)
    {
        if (!string.IsNullOrWhiteSpace(orderNumber)) OrderNumber = orderNumber;
        if (orderDate.HasValue) OrderDate = orderDate.Value;
        if (expectedDeliveryDate.HasValue) ExpectedDeliveryDate = expectedDeliveryDate.Value;
        if (actualDeliveryDate.HasValue) ActualDeliveryDate = actualDeliveryDate.Value;
        if (status.HasValue) Status = status.Value;
        if (subTotal.HasValue) SubTotal = subTotal.Value;
        if (taxAmount.HasValue) TaxAmount = taxAmount.Value;
        if (totalAmount.HasValue) TotalAmount = totalAmount.Value;
        if (notes is not null) NotesText = notes;
        return this;
    }
}

public class PurchaseOrderDetail : AuditableEntity
{
    public int OrderedQuantity { get; private set; }
    public int ReceivedQuantity { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal LineTotal { get; private set; }
    public DateTime? ExpiryDate { get; private set; }

    public DefaultIdType PurchaseOrderId { get; private set; }
    public PurchaseOrder PurchaseOrder { get; private set; } = default!;
    public DefaultIdType ProductId { get; private set; }
    public Product Product { get; private set; } = default!;

    public ICollection<ProductBatch> ProductBatches { get; private set; } = new List<ProductBatch>();

    private PurchaseOrderDetail() { }

    public static PurchaseOrderDetail Create(DefaultIdType purchaseOrderId, DefaultIdType productId, int orderedQuantity, decimal unitPrice, decimal lineTotal, DateTime? expiryDate)
    {
        return new PurchaseOrderDetail
        {
            PurchaseOrderId = purchaseOrderId,
            ProductId = productId,
            OrderedQuantity = orderedQuantity,
            ReceivedQuantity = 0,
            UnitPrice = unitPrice,
            LineTotal = lineTotal,
            ExpiryDate = expiryDate
        };
    }

    public PurchaseOrderDetail Update(int? orderedQuantity, int? receivedQuantity, decimal? unitPrice, decimal? lineTotal, DateTime? expiryDate)
    {
        if (orderedQuantity.HasValue) OrderedQuantity = orderedQuantity.Value;
        if (receivedQuantity.HasValue) ReceivedQuantity = receivedQuantity.Value;
        if (unitPrice.HasValue) UnitPrice = unitPrice.Value;
        if (lineTotal.HasValue) LineTotal = lineTotal.Value;
        if (expiryDate.HasValue) ExpiryDate = expiryDate.Value;
        return this;
    }
}

