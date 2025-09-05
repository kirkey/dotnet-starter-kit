namespace FSH.Starter.WebApi.Warehouse.Domain.ValueObjects;

public enum StockMovementType
{
    Inbound = 1,
    Outbound = 2,
    Transfer = 3,
    Adjustment = 4,
    Return = 5,
    Damage = 6,
    Loss = 7
}

public enum StockMovementStatus
{
    Pending = 1,
    Confirmed = 2,
    InProgress = 3,
    Completed = 4,
    Cancelled = 5,
    Failed = 6
}
