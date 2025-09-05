using System.Diagnostics.Metrics;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public static class WarehouseMetrics
{
    private static readonly Meter _meter = new("FSH.Warehouse", "1.0.0");

    // Warehouse Metrics
    public static readonly Counter<long> Created = _meter.CreateCounter<long>(
        "warehouse.created",
        "warehouses",
        "Number of warehouses created");

    public static readonly Counter<long> Updated = _meter.CreateCounter<long>(
        "warehouse.updated",
        "warehouses",
        "Number of warehouses updated");

    public static readonly Counter<long> Activated = _meter.CreateCounter<long>(
        "warehouse.activated",
        "warehouses",
        "Number of warehouses activated");

    public static readonly Counter<long> Deactivated = _meter.CreateCounter<long>(
        "warehouse.deactivated",
        "warehouses",
        "Number of warehouses deactivated");

    // Stock Movement Metrics
    public static readonly Counter<long> StockMovementsCreated = _meter.CreateCounter<long>(
        "stock_movements.created",
        "movements",
        "Number of stock movements created");

    public static readonly Counter<long> StockMovementsConfirmed = _meter.CreateCounter<long>(
        "stock_movements.confirmed",
        "movements",
        "Number of stock movements confirmed");

    public static readonly Counter<long> StockMovementsCancelled = _meter.CreateCounter<long>(
        "stock_movements.cancelled",
        "movements",
        "Number of stock movements cancelled");

    // Inventory Metrics
    public static readonly Counter<long> InventoryItemsCreated = _meter.CreateCounter<long>(
        "inventory_items.created",
        "items",
        "Number of inventory items created");

    public static readonly Counter<long> StockAdjustments = _meter.CreateCounter<long>(
        "stock.adjustments",
        "adjustments",
        "Number of stock adjustments made");

    public static readonly Counter<long> LowStockAlerts = _meter.CreateCounter<long>(
        "alerts.low_stock",
        "alerts",
        "Number of low stock alerts generated");

    public static readonly Counter<long> OverstockAlerts = _meter.CreateCounter<long>(
        "alerts.overstock",
        "alerts",
        "Number of overstock alerts generated");

    public static readonly Histogram<double> StockMovementQuantity = _meter.CreateHistogram<double>(
        "stock_movement.quantity",
        "units",
        "Distribution of stock movement quantities");

    public static readonly Gauge<long> TotalWarehouses = _meter.CreateGauge<long>(
        "warehouses.total",
        "warehouses",
        "Total number of active warehouses");

    public static readonly Gauge<long> TotalInventoryItems = _meter.CreateGauge<long>(
        "inventory_items.total",
        "items",
        "Total number of inventory items across all warehouses");
}
