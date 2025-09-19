namespace Accounting.Application.Consumptions.Responses;

/// <summary>
/// Response model representing a consumption reading entity.
/// Contains meter reading information including usage calculations and billing period details.
/// </summary>
public class ConsumptionResponse : BaseDto
{
    /// <summary>
    /// Unique identifier for the consumption record.
    /// </summary>
    public new DefaultIdType Id { get; set; }

    /// <summary>
    /// Identifier for the meter associated with this consumption reading.
    /// </summary>
    public DefaultIdType MeterId { get; set; }
    
    /// <summary>
    /// Date when the meter reading was taken.
    /// </summary>
    public DateTime ReadingDate { get; set; }
    
    /// <summary>
    /// Current meter reading value.
    /// </summary>
    public decimal CurrentReading { get; set; }
    
    /// <summary>
    /// Previous meter reading value for comparison.
    /// </summary>
    public decimal PreviousReading { get; set; }
    
    /// <summary>
    /// Calculated kilowatt hours used during the billing period.
    /// </summary>
    public decimal KWhUsed { get; set; }
    
    /// <summary>
    /// Billing period identifier for this consumption record.
    /// </summary>
    public string BillingPeriod { get; set; } = string.Empty;
    
    /// <summary>
    /// Type of reading (e.g., "Manual", "Automatic", "Estimated").
    /// </summary>
    public string ReadingType { get; set; } = string.Empty;
    
    /// <summary>
    /// Multiplier applied to calculate actual consumption from meter reading.
    /// </summary>
    public decimal? Multiplier { get; set; }
    
    /// <summary>
    /// Indicates whether the reading is valid and accurate.
    /// </summary>
    public bool IsValidReading { get; set; }
    
    /// <summary>
    /// Source of the reading (e.g., "Field Reading", "Remote System", "Estimated").
    /// </summary>
    public string? ReadingSource { get; set; }
    
    /// <summary>
    /// Description or additional details about the consumption reading.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Additional notes or comments about the consumption reading.
    /// </summary>
    public string? Notes { get; set; }
}
