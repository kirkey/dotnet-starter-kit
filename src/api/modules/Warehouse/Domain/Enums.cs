namespace FSH.Starter.WebApi.Warehouse.Domain;

public enum TransactionType
{
    Purchase,
    Sale,
    Transfer,
    Adjustment,
    Expired,
    Damaged,
    Returned
}

public enum PurchaseOrderStatus
{
    Draft,
    Pending,
    Approved,
    Sent,
    PartiallyReceived,
    Received,
    Cancelled
}

public enum TransferStatus
{
    Pending,
    InTransit,
    Received,
    Cancelled
}

public enum PaymentMethod
{
    Cash,
    CreditCard,
    DebitCard,
    MobilePayment,
    GiftCard,
    StoreCredit
}

public enum SaleStatus
{
    InProgress,
    Completed,
    Cancelled,
    Refunded,
    PartiallyRefunded
}

