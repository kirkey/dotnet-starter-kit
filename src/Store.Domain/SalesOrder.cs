using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Store.Domain.Events;

namespace Store.Domain;

public class SalesOrder : AuditableEntity, IAggregateRoot
{
    public string OrderNumber { get; private set; } = default!;
    public DefaultIdType CustomerId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public DateTime? DeliveryDate { get; private set; }
    public string Status { get; private set; } = default!; // Draft, Confirmed, Processing, Shipped, Delivered, Cancelled
    public string OrderType { get; private set; } = default!; // Retail, Wholesale, Online, InStore
    public decimal SubTotal { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal ShippingAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string PaymentStatus { get; private set; } = default!; // Pending, Paid, Partial, Overdue
    public string PaymentMethod { get; private set; } = default!; // Cash, Card, Transfer, Credit
    public string? DeliveryAddress { get; private set; }
    public string? Notes { get; private set; }
    public bool IsUrgent { get; private set; }
    public string? SalesPersonId { get; private set; }
    public DefaultIdType? WarehouseId { get; private set; }
    
    public virtual Customer Customer { get; private set; } = default!;
    public virtual Warehouse? Warehouse { get; private set; }
    public virtual ICollection<SalesOrderItem> Items { get; private set; } = new List<SalesOrderItem>();

    private SalesOrder() { }

    private SalesOrder(
        DefaultIdType id,
        string orderNumber,
        DefaultIdType customerId,
        DateTime orderDate,
        string status,
        string orderType,
        string paymentStatus,
        string paymentMethod,
        string? deliveryAddress,
        string? notes,
        bool isUrgent,
        string? salesPersonId,
        DefaultIdType? warehouseId)
    {
        Id = id;
        OrderNumber = orderNumber;
        CustomerId = customerId;
        OrderDate = orderDate;
        Status = status;
        OrderType = orderType;
        PaymentStatus = paymentStatus;
        PaymentMethod = paymentMethod;
        DeliveryAddress = deliveryAddress;
        Notes = notes;
        IsUrgent = isUrgent;
        SalesPersonId = salesPersonId;
        WarehouseId = warehouseId;
        SubTotal = 0;
        TaxAmount = 0;
        DiscountAmount = 0;
        ShippingAmount = 0;
        TotalAmount = 0;

        QueueDomainEvent(new SalesOrderCreated { SalesOrder = this });
    }

    public static SalesOrder Create(
        string orderNumber,
        DefaultIdType customerId,
        DateTime orderDate,
        string status = "Draft",
        string orderType = "Retail",
        string paymentStatus = "Pending",
        string paymentMethod = "Cash",
        string? deliveryAddress = null,
        string? notes = null,
        bool isUrgent = false,
        string? salesPersonId = null,
        DefaultIdType? warehouseId = null)
    {
        return new SalesOrder(
            DefaultIdType.NewGuid(),
            orderNumber,
            customerId,
            orderDate,
            status,
            orderType,
            paymentStatus,
            paymentMethod,
            deliveryAddress,
            notes,
            isUrgent,
            salesPersonId,
            warehouseId);
    }

    public SalesOrder AddItem(DefaultIdType groceryItemId, int quantity, decimal unitPrice, decimal? discount = null)
    {
        var existingItem = Items.FirstOrDefault(i => i.GroceryItemId == groceryItemId);
        
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var newItem = SalesOrderItem.Create(Id, groceryItemId, quantity, unitPrice, discount);
            Items.Add(newItem);
        }

        RecalculateTotals();
        QueueDomainEvent(new SalesOrderItemAdded { SalesOrder = this, GroceryItemId = groceryItemId });
        
        return this;
    }

    public SalesOrder UpdateStatus(string status)
    {
        if (!string.Equals(Status, status, StringComparison.OrdinalIgnoreCase))
        {
            var previousStatus = Status;
            Status = status;
            
            QueueDomainEvent(new SalesOrderStatusChanged 
            { 
                SalesOrder = this, 
                PreviousStatus = previousStatus, 
                NewStatus = status 
            });
        }

        return this;
    }

    public SalesOrder UpdatePaymentStatus(string paymentStatus)
    {
        if (!string.Equals(PaymentStatus, paymentStatus, StringComparison.OrdinalIgnoreCase))
        {
            var previousStatus = PaymentStatus;
            PaymentStatus = paymentStatus;
            
            QueueDomainEvent(new SalesOrderPaymentStatusChanged 
            { 
                SalesOrder = this, 
                PreviousStatus = previousStatus, 
                NewStatus = paymentStatus 
            });
        }

        return this;
    }

    public SalesOrder ApplyDiscount(decimal discountAmount)
    {
        DiscountAmount = discountAmount;
        RecalculateTotals();
        QueueDomainEvent(new SalesOrderDiscountApplied { SalesOrder = this, DiscountAmount = discountAmount });
        return this;
    }

    public SalesOrder SetDeliveryDate(DateTime deliveryDate)
    {
        DeliveryDate = deliveryDate;
        QueueDomainEvent(new SalesOrderDeliveryScheduled { SalesOrder = this });
        return this;
    }

    private void RecalculateTotals()
    {
        SubTotal = Items.Sum(i => i.TotalPrice);
        TotalAmount = SubTotal + TaxAmount + ShippingAmount - DiscountAmount;
    }

    public bool IsWholesaleOrder() => OrderType.Equals("Wholesale", StringComparison.OrdinalIgnoreCase);
    public bool IsRetailOrder() => OrderType.Equals("Retail", StringComparison.OrdinalIgnoreCase);
    public bool IsOverdue() => PaymentStatus == "Overdue";
    public bool IsPaid() => PaymentStatus == "Paid";
    public bool IsDelivered() => Status == "Delivered";
}
