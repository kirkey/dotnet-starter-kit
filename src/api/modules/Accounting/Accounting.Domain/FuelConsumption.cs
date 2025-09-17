using Accounting.Domain.Events.FuelConsumption;

namespace Accounting.Domain;

/// <summary>
/// Records fuel usage for power generation, including quantity, unit cost, supplier, and environmental metadata.
/// </summary>
/// <remarks>
/// Computes <see cref="TotalCost"/> as <c>Quantity * UnitCost</c> and supports updates with validation.
/// Defaults: optional strings are trimmed; boolean <see cref="IsEmergencyFuel"/> defaults to false.
/// </remarks>
public class FuelConsumption : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Date when the fuel was consumed.
    /// </summary>
    public DateTime ConsumptionDate { get; private set; }

    /// <summary>
    /// Identifier of the power plant or generating unit.
    /// </summary>
    public DefaultIdType PowerPlantId { get; private set; }

    /// <summary>
    /// Type of fuel used (e.g., Coal, Natural Gas, Diesel, Nuclear, Biomass).
    /// </summary>
    public string FuelType { get; private set; } // "Coal", "Natural Gas", "Diesel", "Nuclear", "Biomass"

    /// <summary>
    /// Quantity of fuel consumed during the period.
    /// </summary>
    public decimal Quantity { get; private set; }

    /// <summary>
    /// Unit of measure for <see cref="Quantity"/> (e.g., Tons, MCF, Gallons, BTU).
    /// </summary>
    public string QuantityUnit { get; private set; } // "Tons", "MCF", "Gallons", "BTU"

    /// <summary>
    /// Cost per unit of fuel.
    /// </summary>
    public decimal UnitCost { get; private set; }

    /// <summary>
    /// Total cost derived from <see cref="Quantity"/> times <see cref="UnitCost"/>.
    /// </summary>
    public decimal TotalCost { get; private set; }

    /// <summary>
    /// Optional supplier identifier or code.
    /// </summary>
    public string? SupplierId { get; private set; }

    /// <summary>
    /// Heat content (BTU per unit), for efficiency calculations.
    /// </summary>
    public decimal? BtuContent { get; private set; } // British Thermal Units per unit

    /// <summary>
    /// Sulfur content percentage for environmental reporting.
    /// </summary>
    public decimal? SulfurContent { get; private set; } // For environmental reporting

    /// <summary>
    /// Delivery method (e.g., Pipeline, Truck, Rail, Barge).
    /// </summary>
    public string? DeliveryMethod { get; private set; } // "Pipeline", "Truck", "Rail", "Barge"

    /// <summary>
    /// Whether the fuel was used for emergency generation.
    /// </summary>
    public bool IsEmergencyFuel { get; private set; }
    
    private FuelConsumption()
    {
        // EF Core requires a parameterless constructor for entity instantiation
    }

    private FuelConsumption(DateTime consumptionDate, DefaultIdType powerPlantId,
        string fuelType, decimal quantity, string quantityUnit, decimal unitCost,
        string? supplierId = null, decimal? btuContent = null, decimal? sulfurContent = null,
        string? deliveryMethod = null, bool isEmergencyFuel = false,
        string? description = null, string? notes = null)
    {
        ConsumptionDate = consumptionDate;
        PowerPlantId = powerPlantId;
        FuelType = fuelType.Trim();
        Quantity = quantity;
        QuantityUnit = quantityUnit.Trim();
        UnitCost = unitCost;
        TotalCost = quantity * unitCost;
        SupplierId = supplierId?.Trim();
        BtuContent = btuContent;
        SulfurContent = sulfurContent;
        DeliveryMethod = deliveryMethod?.Trim();
        IsEmergencyFuel = isEmergencyFuel;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new FuelConsumptionRecorded(Id, PowerPlantId, FuelType, Quantity, TotalCost, ConsumptionDate, Description, Notes));
    }

    /// <summary>
    /// Create a new fuel consumption record with validation of quantity and unit cost.
    /// </summary>
    public static FuelConsumption Create(DateTime consumptionDate, DefaultIdType powerPlantId,
        string fuelType, decimal quantity, string quantityUnit, decimal unitCost,
        string? supplierId = null, decimal? btuContent = null, decimal? sulfurContent = null,
        string? deliveryMethod = null, bool isEmergencyFuel = false,
        string? description = null, string? notes = null)
    {
        if (quantity <= 0)
            throw new InvalidFuelQuantityException("Quantity must be greater than zero.");

        if (unitCost < 0)
            throw new InvalidFuelCostException("Unit cost cannot be negative.");

        return new FuelConsumption(consumptionDate, powerPlantId, fuelType,
            quantity, quantityUnit, unitCost, supplierId, btuContent, sulfurContent,
            deliveryMethod, isEmergencyFuel, description, notes);
    }

    /// <summary>
    /// Update fields and recompute <see cref="TotalCost"/> as necessary.
    /// </summary>
    public FuelConsumption Update(decimal? quantity, decimal? unitCost, string? supplierId,
        decimal? btuContent, decimal? sulfurContent, string? deliveryMethod, bool isEmergencyFuel = false,
        string? description = null, string? notes = null)
    {
        bool isUpdated = false;

        if (quantity.HasValue && Quantity != quantity.Value)
        {
            if (quantity.Value <= 0)
                throw new InvalidFuelQuantityException("Quantity must be greater than zero.");
            Quantity = quantity.Value;
            TotalCost = Quantity * UnitCost;
            isUpdated = true;
        }

        if (unitCost.HasValue && UnitCost != unitCost.Value)
        {
            if (unitCost.Value < 0)
                throw new InvalidFuelCostException("Unit cost cannot be negative.");
            UnitCost = unitCost.Value;
            TotalCost = Quantity * UnitCost;
            isUpdated = true;
        }

        if (supplierId != SupplierId)
        {
            SupplierId = supplierId?.Trim();
            isUpdated = true;
        }

        if (btuContent != BtuContent)
        {
            BtuContent = btuContent;
            isUpdated = true;
        }

        if (sulfurContent != SulfurContent)
        {
            SulfurContent = sulfurContent;
            isUpdated = true;
        }

        if (deliveryMethod != DeliveryMethod)
        {
            DeliveryMethod = deliveryMethod?.Trim();
            isUpdated = true;
        }

        if (IsEmergencyFuel != isEmergencyFuel)
        {
            IsEmergencyFuel = isEmergencyFuel;
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new FuelConsumptionUpdated(this));
        }

        return this;
    }

    /// <summary>
    /// Calculate the thermal efficiency (BTU per kWh) given power generated.
    /// </summary>
    public decimal CalculateEfficiency(decimal powerGenerated)
    {
        // Calculate heat rate (BTU per kWh) - lower is better
        if (powerGenerated <= 0 || !BtuContent.HasValue)
            return 0;

        var totalBtu = Quantity * BtuContent.Value;
        return totalBtu / powerGenerated;
    }
}
