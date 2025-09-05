namespace FSH.Starter.WebApi.Warehouse.Domain.ValueObjects;

public record UnitOfMeasure(string Name, string Symbol, string? Description = null)
{
    public static readonly UnitOfMeasure Piece = new("Piece", "pcs", "Individual units");
    public static readonly UnitOfMeasure Kilogram = new("Kilogram", "kg", "Weight in kilograms");
    public static readonly UnitOfMeasure Liter = new("Liter", "L", "Volume in liters");
    public static readonly UnitOfMeasure Meter = new("Meter", "m", "Length in meters");
    public static readonly UnitOfMeasure Box = new("Box", "box", "Packaged in boxes");
    public static readonly UnitOfMeasure Pallet = new("Pallet", "plt", "Stored on pallets");
}
