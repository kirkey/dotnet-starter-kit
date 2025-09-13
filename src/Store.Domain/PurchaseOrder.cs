using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Store.Domain.Events;

namespace Store.Domain;

public sealed class PurchaseOrder : AuditableEntity, IAggregateRoot
{
    public string OrderNumber { get; private set; } = default!;
    public DefaultIdType SupplierId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public DateTime? ExpectedDeliveryDate { get; private set; }
    public DateTime? ActualDeliveryDate { get; private set; }
    public string Status { get; private set; } = default!; // Draft, Sent, Confirmed, Received, Cancelled
    public decimal TotalAmount { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal NetAmount { get; private set; }
    
    public string? DeliveryAddress { get; private set; }
    public string? ContactPerson { get; private set; }
    public string? ContactPhone { get; private set; }
    public bool IsUrgent { get; private set; }
    
    public Supplier Supplier { get; private set; } = default!;
    public ICollection<PurchaseOrderItem> Items { get; private set; } = new List<PurchaseOrderItem>();

    private PurchaseOrder() { }

    private PurchaseOrder(
        DefaultIdType id,
        string orderNumber,
        DefaultIdType supplierId,
        DateTime orderDate,
        DateTime? expectedDeliveryDate,
        string status,
        string? notes,
        string? deliveryAddress,
        string? contactPerson,
        string? contactPhone,
        bool isUrgent)
    {
        Id = id;
        OrderNumber = orderNumber;
        SupplierId = supplierId;
        OrderDate = orderDate;
        ExpectedDeliveryDate = expectedDeliveryDate;
        Status = status;
        Notes = notes;
        DeliveryAddress = deliveryAddress;
        ContactPerson = contactPerson;
        ContactPhone = contactPhone;
        IsUrgent = isUrgent;
        TotalAmount = 0;
        TaxAmount = 0;
        DiscountAmount = 0;
        NetAmount = 0;

        QueueDomainEvent(new PurchaseOrderCreated { PurchaseOrder = this });
    }

    public static PurchaseOrder Create(
        string orderNumber,
        DefaultIdType supplierId,
        DateTime orderDate,
        DateTime? expectedDeliveryDate = null,
        string status = "Draft",
        string? notes = null,
        string? deliveryAddress = null,
        string? contactPerson = null,
        string? contactPhone = null,
        bool isUrgent = false)
    {
        return new PurchaseOrder(
            DefaultIdType.NewGuid(),
            orderNumber,
            supplierId,
            orderDate,
            expectedDeliveryDate,
            status,
            notes,
            deliveryAddress,
            contactPerson,
            contactPhone,
            isUrgent);
    }

    public PurchaseOrder UpdateStatus(string status)
    {
        if (!string.Equals(Status, status, StringComparison.OrdinalIgnoreCase))
        {
            var previousStatus = Status;
            Status = status;
            
            QueueDomainEvent(new PurchaseOrderStatusChanged 
            { 
                PurchaseOrder = this, 
                PreviousStatus = previousStatus, 
                NewStatus = status 
            });
        }

        return this;
    }

    public PurchaseOrder AddItem(DefaultIdType groceryItemId, int quantity, decimal unitPrice, decimal? discount = null)
    {
        var existingItem = Items.FirstOrDefault(i => i.GroceryItemId == groceryItemId);
        
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var newItem = PurchaseOrderItem.Create(Id, groceryItemId, quantity, unitPrice, discount);
            Items.Add(newItem);
        }

        RecalculateTotals();
        QueueDomainEvent(new PurchaseOrderItemAdded { PurchaseOrder = this, GroceryItemId = groceryItemId });
        
        return this;
    }

    public PurchaseOrder RemoveItem(DefaultIdType groceryItemId)
    {
        var item = Items.FirstOrDefault(i => i.GroceryItemId == groceryItemId);
        if (item != null)
        {
            Items.Remove(item);
            RecalculateTotals();
            QueueDomainEvent(new PurchaseOrderItemRemoved { PurchaseOrder = this, GroceryItemId = groceryItemId });
        }

        return this;
    }

    public PurchaseOrder UpdateDeliveryDate(DateTime? actualDeliveryDate)
    {
        if (ActualDeliveryDate != actualDeliveryDate)
        {
            ActualDeliveryDate = actualDeliveryDate;
            
            if (actualDeliveryDate.HasValue)
            {
                QueueDomainEvent(new PurchaseOrderDelivered { PurchaseOrder = this });
            }
        }

        return this;
    }

    public PurchaseOrder ApplyDiscount(decimal discountAmount)
    {
        if (DiscountAmount != discountAmount)
        {
            DiscountAmount = discountAmount;
            RecalculateTotals();
            QueueDomainEvent(new PurchaseOrderDiscountApplied { PurchaseOrder = this, DiscountAmount = discountAmount });
        }

        return this;
    }

    private void RecalculateTotals()
    {
        TotalAmount = Items.Sum(i => i.TotalPrice);
        NetAmount = TotalAmount + TaxAmount - DiscountAmount;
    }

    public bool IsOverdue() => 
        ExpectedDeliveryDate.HasValue && 
        ExpectedDeliveryDate.Value < DateTime.UtcNow && 
        !ActualDeliveryDate.HasValue &&
        Status != "Cancelled";

    public bool IsDelivered() => ActualDeliveryDate.HasValue;

    public int GetDaysUntilExpectedDelivery() =>
        ExpectedDeliveryDate.HasValue 
            ? (ExpectedDeliveryDate.Value - DateTime.UtcNow).Days 
            : 0;
}
